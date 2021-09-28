//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework.Download;
using GameFramework.FileSystem;
using System;
using System.Collections.Generic;
using System.IO;

namespace GameFramework.Resource
{
    internal sealed partial class ResourceManager : GameFrameworkModule, IResourceManager
    {
        /// <summary>
        /// 资源更新器。
        /// </summary>
        private sealed partial class ResourceUpdater
        {
            private const int CachedHashBytesLength = 4;
            private const int CachedBytesLength = 0x1000;

            private readonly ResourceManager m_ResourceManager;
            private readonly Queue<ApplyInfo> m_ApplyWaitingInfo;
            private readonly List<UpdateInfo> m_UpdateWaitingInfo;
            private readonly HashSet<UpdateInfo> m_UpdateWaitingInfoWhilePlaying;
            private readonly Dictionary<ResourceName, UpdateInfo> m_UpdateCandidateInfo;
            private readonly SortedDictionary<string, List<int>> m_CachedFileSystemsForGenerateReadWriteVersionList;
            private readonly List<ResourceName> m_CachedResourceNames;
            private readonly byte[] m_CachedHashBytes;
            private readonly byte[] m_CachedBytes;
            private IDownloadManager m_DownloadManager;
            private bool m_CheckResourcesComplete;
            private string m_ApplyingResourcePackPath;
            private FileStream m_ApplyingResourcePackStream;
            private ResourceGroup m_UpdatingResourceGroup;
            private int m_GenerateReadWriteVersionListLength;
            private int m_CurrentGenerateReadWriteVersionListLength;
            private int m_UpdateRetryCount;
            private bool m_FailureFlag;
            private string m_ReadWriteVersionListFileName;
            private string m_ReadWriteVersionListTempFileName;

            public GameFrameworkAction<string, int, long> ResourceApplyStart;
            public GameFrameworkAction<ResourceName, string, string, int, int> ResourceApplySuccess;
            public GameFrameworkAction<ResourceName, string, string> ResourceApplyFailure;
            public GameFrameworkAction<string, bool> ResourceApplyComplete;
            public GameFrameworkAction<ResourceName, string, string, int, int, int> ResourceUpdateStart;
            public GameFrameworkAction<ResourceName, string, string, int, int> ResourceUpdateChanged;
            public GameFrameworkAction<ResourceName, string, string, int, int> ResourceUpdateSuccess;
            public GameFrameworkAction<ResourceName, string, int, int, string> ResourceUpdateFailure;
            public GameFrameworkAction<ResourceGroup, bool> ResourceUpdateComplete;
            public GameFrameworkAction ResourceUpdateAllComplete;

            /// <summary>
            /// 初始化资源更新器的新实例。
            /// </summary>
            /// <param name="resourceManager">资源管理器。</param>
            public ResourceUpdater(ResourceManager resourceManager)
            {
                m_ResourceManager = resourceManager;
                m_ApplyWaitingInfo = new Queue<ApplyInfo>();
                m_UpdateWaitingInfo = new List<UpdateInfo>();
                m_UpdateWaitingInfoWhilePlaying = new HashSet<UpdateInfo>();
                m_UpdateCandidateInfo = new Dictionary<ResourceName, UpdateInfo>();
                m_CachedFileSystemsForGenerateReadWriteVersionList = new SortedDictionary<string, List<int>>(StringComparer.Ordinal);
                m_CachedResourceNames = new List<ResourceName>();
                m_CachedHashBytes = new byte[CachedHashBytesLength];
                m_CachedBytes = new byte[CachedBytesLength];
                m_DownloadManager = null;
                m_CheckResourcesComplete = false;
                m_ApplyingResourcePackPath = null;
                m_ApplyingResourcePackStream = null;
                m_UpdatingResourceGroup = null;
                m_GenerateReadWriteVersionListLength = 0;
                m_CurrentGenerateReadWriteVersionListLength = 0;
                m_UpdateRetryCount = 3;
                m_FailureFlag = false;
                m_ReadWriteVersionListFileName = Utility.Path.GetRegularPath(Path.Combine(m_ResourceManager.m_ReadWritePath, LocalVersionListFileName));
                m_ReadWriteVersionListTempFileName = Utility.Text.Format("{0}.{1}", m_ReadWriteVersionListFileName, TempExtension);

                ResourceApplyStart = null;
                ResourceApplySuccess = null;
                ResourceApplyFailure = null;
                ResourceApplyComplete = null;
                ResourceUpdateStart = null;
                ResourceUpdateChanged = null;
                ResourceUpdateSuccess = null;
                ResourceUpdateFailure = null;
                ResourceUpdateComplete = null;
                ResourceUpdateAllComplete = null;
            }

            /// <summary>
            /// 获取或设置每更新多少字节的资源，重新生成一次版本资源列表。
            /// </summary>
            public int GenerateReadWriteVersionListLength
            {
                get
                {
                    return m_GenerateReadWriteVersionListLength;
                }
                set
                {
                    m_GenerateReadWriteVersionListLength = value;
                }
            }

            /// <summary>
            /// 获取正在应用的资源包路径。
            /// </summary>
            public string ApplyingResourcePackPath
            {
                get
                {
                    return m_ApplyingResourcePackPath;
                }
            }

            /// <summary>
            /// 获取等待应用资源数量。
            /// </summary>
            public int ApplyWaitingCount
            {
                get
                {
                    return m_ApplyWaitingInfo.Count;
                }
            }

            /// <summary>
            /// 获取或设置资源更新重试次数。
            /// </summary>
            public int UpdateRetryCount
            {
                get
                {
                    return m_UpdateRetryCount;
                }
                set
                {
                    m_UpdateRetryCount = value;
                }
            }

            /// <summary>
            /// 获取正在更新的资源组。
            /// </summary>
            public IResourceGroup UpdatingResourceGroup
            {
                get
                {
                    return m_UpdatingResourceGroup;
                }
            }

            /// <summary>
            /// 获取等待更新资源数量。
            /// </summary>
            public int UpdateWaitingCount
            {
                get
                {
                    return m_UpdateWaitingInfo.Count;
                }
            }

            /// <summary>
            /// 获取使用时下载的等待更新资源数量。
            /// </summary>
            public int UpdateWaitingWhilePlayingCount
            {
                get
                {
                    return m_UpdateWaitingInfoWhilePlaying.Count;
                }
            }

            /// <summary>
            /// 获取候选更新资源数量。
            /// </summary>
            public int UpdateCandidateCount
            {
                get
                {
                    return m_UpdateCandidateInfo.Count;
                }
            }

            /// <summary>
            /// 资源更新器轮询。
            /// </summary>
            /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
            /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
            public void Update(float elapseSeconds, float realElapseSeconds)
            {
                if (m_ApplyingResourcePackStream != null)
                {
                    while (m_ApplyWaitingInfo.Count > 0)
                    {
                        ApplyInfo applyInfo = m_ApplyWaitingInfo.Dequeue();
                        if (ApplyResource(applyInfo))
                        {
                            return;
                        }
                    }

                    Array.Clear(m_CachedBytes, 0, CachedBytesLength);
                    string resourcePackPath = m_ApplyingResourcePackPath;
                    m_ApplyingResourcePackPath = null;
                    m_ApplyingResourcePackStream.Dispose();
                    m_ApplyingResourcePackStream = null;
                    if (ResourceApplyComplete != null)
                    {
                        ResourceApplyComplete(resourcePackPath, !m_FailureFlag);
                    }

                    if (m_UpdateCandidateInfo.Count <= 0 && ResourceUpdateAllComplete != null)
                    {
                        ResourceUpdateAllComplete();
                    }

                    return;
                }

                if (m_UpdateWaitingInfo.Count > 0)
                {
                    int freeCount = m_DownloadManager.FreeAgentCount - m_DownloadManager.WaitingTaskCount;
                    if (freeCount > 0)
                    {
                        for (int i = 0, count = 0; i < m_UpdateWaitingInfo.Count && count < freeCount; i++)
                        {
                            if (DownloadResource(m_UpdateWaitingInfo[i]))
                            {
                                count++;
                            }
                        }
                    }

                    return;
                }
            }

            /// <summary>
            /// 关闭并清理资源更新器。
            /// </summary>
            public void Shutdown()
            {
                if (m_DownloadManager != null)
                {
                    m_DownloadManager.DownloadStart -= OnDownloadStart;
                    m_DownloadManager.DownloadUpdate -= OnDownloadUpdate;
                    m_DownloadManager.DownloadSuccess -= OnDownloadSuccess;
                    m_DownloadManager.DownloadFailure -= OnDownloadFailure;
                }

                m_UpdateWaitingInfo.Clear();
                m_UpdateCandidateInfo.Clear();
                m_CachedFileSystemsForGenerateReadWriteVersionList.Clear();
            }

            /// <summary>
            /// 设置下载管理器。
            /// </summary>
            /// <param name="downloadManager">下载管理器。</param>
            public void SetDownloadManager(IDownloadManager downloadManager)
            {
                if (downloadManager == null)
                {
                    throw new GameFrameworkException("Download manager is invalid.");
                }

                m_DownloadManager = downloadManager;
                m_DownloadManager.DownloadStart += OnDownloadStart;
                m_DownloadManager.DownloadUpdate += OnDownloadUpdate;
                m_DownloadManager.DownloadSuccess += OnDownloadSuccess;
                m_DownloadManager.DownloadFailure += OnDownloadFailure;
            }

            /// <summary>
            /// 增加资源更新。
            /// </summary>
            /// <param name="resourceName">资源名称。</param>
            /// <param name="fileSystemName">资源所在的文件系统名称。</param>
            /// <param name="loadType">资源加载方式。</param>
            /// <param name="length">资源大小。</param>
            /// <param name="hashCode">资源哈希值。</param>
            /// <param name="compressedLength">压缩后大小。</param>
            /// <param name="compressedHashCode">压缩后哈希值。</param>
            /// <param name="resourcePath">资源路径。</param>
            public void AddResourceUpdate(ResourceName resourceName, string fileSystemName, LoadType loadType, int length, int hashCode, int compressedLength, int compressedHashCode, string resourcePath)
            {
                m_UpdateCandidateInfo.Add(resourceName, new UpdateInfo(resourceName, fileSystemName, loadType, length, hashCode, compressedLength, compressedHashCode, resourcePath));
            }

            /// <summary>
            /// 检查资源完成。
            /// </summary>
            /// <param name="needGenerateReadWriteVersionList">是否需要生成读写区版本资源列表。</param>
            public void CheckResourceComplete(bool needGenerateReadWriteVersionList)
            {
                m_CheckResourcesComplete = true;
                if (needGenerateReadWriteVersionList)
                {
                    GenerateReadWriteVersionList();
                }
            }

            /// <summary>
            /// 应用指定资源包的资源。
            /// </summary>
            /// <param name="resourcePackPath">要应用的资源包路径。</param>
            public void ApplyResources(string resourcePackPath)
            {
                if (!m_CheckResourcesComplete)
                {
                    throw new GameFrameworkException("You must check resources complete first.");
                }

                if (m_ApplyingResourcePackStream != null)
                {
                    throw new GameFrameworkException(Utility.Text.Format("There is already a resource pack '{0}' being applied.", m_ApplyingResourcePackPath));
                }

                if (m_UpdatingResourceGroup != null)
                {
                    throw new GameFrameworkException(Utility.Text.Format("There is already a resource group '{0}' being updated.", m_UpdatingResourceGroup.Name));
                }

                if (m_UpdateWaitingInfoWhilePlaying.Count > 0)
                {
                    throw new GameFrameworkException("There are already some resources being updated while playing.");
                }

                try
                {
                    long length = 0L;
                    ResourcePackVersionList versionList = default(ResourcePackVersionList);
                    using (FileStream fileStream = new FileStream(resourcePackPath, FileMode.Open, FileAccess.Read))
                    {
                        length = fileStream.Length;
                        versionList = m_ResourceManager.m_ResourcePackVersionListSerializer.Deserialize(fileStream);
                    }

                    if (!versionList.IsValid)
                    {
                        throw new GameFrameworkException("Deserialize resource pack version list failure.");
                    }

                    if (versionList.Offset + versionList.Length != length)
                    {
                        throw new GameFrameworkException("Resource pack length is invalid.");
                    }

                    m_ApplyingResourcePackPath = resourcePackPath;
                    m_ApplyingResourcePackStream = new FileStream(resourcePackPath, FileMode.Open, FileAccess.Read);
                    m_ApplyingResourcePackStream.Position = versionList.Offset;
                    m_FailureFlag = false;

                    long totalLength = 0L;
                    ResourcePackVersionList.Resource[] resources = versionList.GetResources();
                    foreach (ResourcePackVersionList.Resource resource in resources)
                    {
                        ResourceName resourceName = new ResourceName(resource.Name, resource.Variant, resource.Extension);
                        UpdateInfo updateInfo = null;
                        if (!m_UpdateCandidateInfo.TryGetValue(resourceName, out updateInfo))
                        {
                            continue;
                        }

                        if (updateInfo.LoadType == (LoadType)resource.LoadType && updateInfo.Length == resource.Length && updateInfo.HashCode == resource.HashCode)
                        {
                            totalLength += resource.Length;
                            m_ApplyWaitingInfo.Enqueue(new ApplyInfo(resourceName, updateInfo.FileSystemName, (LoadType)resource.LoadType, resource.Offset, resource.Length, resource.HashCode, resource.CompressedLength, resource.CompressedHashCode, updateInfo.ResourcePath));
                        }
                    }

                    if (ResourceApplyStart != null)
                    {
                        ResourceApplyStart(m_ApplyingResourcePackPath, m_ApplyWaitingInfo.Count, totalLength);
                    }
                }
                catch (Exception exception)
                {
                    if (m_ApplyingResourcePackStream != null)
                    {
                        m_ApplyingResourcePackStream.Dispose();
                        m_ApplyingResourcePackStream = null;
                    }

                    throw new GameFrameworkException(Utility.Text.Format("Apply resources '{0}' with exception '{1}'.", resourcePackPath, exception), exception);
                }
            }

            /// <summary>
            /// 更新指定资源组的资源。
            /// </summary>
            /// <param name="resourceGroup">要更新的资源组。</param>
            public void UpdateResources(ResourceGroup resourceGroup)
            {
                if (m_DownloadManager == null)
                {
                    throw new GameFrameworkException("You must set download manager first.");
                }

                if (!m_CheckResourcesComplete)
                {
                    throw new GameFrameworkException("You must check resources complete first.");
                }

                if (m_ApplyingResourcePackStream != null)
                {
                    throw new GameFrameworkException(Utility.Text.Format("There is already a resource pack '{0}' being applied.", m_ApplyingResourcePackPath));
                }

                if (m_UpdatingResourceGroup != null)
                {
                    throw new GameFrameworkException(Utility.Text.Format("There is already a resource group '{0}' being updated.", m_UpdatingResourceGroup.Name));
                }

                if (string.IsNullOrEmpty(resourceGroup.Name))
                {
                    foreach (KeyValuePair<ResourceName, UpdateInfo> updateInfo in m_UpdateCandidateInfo)
                    {
                        m_UpdateWaitingInfo.Add(updateInfo.Value);
                    }
                }
                else
                {
                    resourceGroup.InternalGetResourceNames(m_CachedResourceNames);
                    foreach (ResourceName resourceName in m_CachedResourceNames)
                    {
                        UpdateInfo updateInfo = null;
                        if (!m_UpdateCandidateInfo.TryGetValue(resourceName, out updateInfo))
                        {
                            continue;
                        }

                        m_UpdateWaitingInfo.Add(updateInfo);
                    }

                    m_CachedResourceNames.Clear();
                }

                m_UpdatingResourceGroup = resourceGroup;
                m_FailureFlag = false;
            }

            /// <summary>
            /// 停止更新资源。
            /// </summary>
            public void StopUpdateResources()
            {
                if (m_DownloadManager == null)
                {
                    throw new GameFrameworkException("You must set download manager first.");
                }

                if (!m_CheckResourcesComplete)
                {
                    throw new GameFrameworkException("You must check resources complete first.");
                }

                if (m_ApplyingResourcePackStream != null)
                {
                    throw new GameFrameworkException(Utility.Text.Format("There is already a resource pack '{0}' being applied.", m_ApplyingResourcePackPath));
                }

                if (m_UpdatingResourceGroup == null)
                {
                    throw new GameFrameworkException("There is no resource group being updated.");
                }

                m_UpdateWaitingInfo.Clear();
                m_UpdatingResourceGroup = null;
            }

            /// <summary>
            /// 更新指定资源。
            /// </summary>
            /// <param name="resourceName">要更新的资源名称。</param>
            public void UpdateResource(ResourceName resourceName)
            {
                if (m_DownloadManager == null)
                {
                    throw new GameFrameworkException("You must set download manager first.");
                }

                if (!m_CheckResourcesComplete)
                {
                    throw new GameFrameworkException("You must check resources complete first.");
                }

                if (m_ApplyingResourcePackStream != null)
                {
                    throw new GameFrameworkException(Utility.Text.Format("There is already a resource pack '{0}' being applied.", m_ApplyingResourcePackPath));
                }

                UpdateInfo updateInfo = null;
                if (m_UpdateCandidateInfo.TryGetValue(resourceName, out updateInfo) && m_UpdateWaitingInfoWhilePlaying.Add(updateInfo))
                {
                    DownloadResource(updateInfo);
                }
            }

            private bool ApplyResource(ApplyInfo applyInfo)
            {
                long position = m_ApplyingResourcePackStream.Position;
                try
                {
                    bool compressed = applyInfo.Length != applyInfo.CompressedLength || applyInfo.HashCode != applyInfo.CompressedHashCode;

                    int bytesRead = 0;
                    int bytesLeft = applyInfo.CompressedLength;
                    string directory = Path.GetDirectoryName(applyInfo.ResourcePath);
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    m_ApplyingResourcePackStream.Position += applyInfo.Offset;
                    using (FileStream fileStream = new FileStream(applyInfo.ResourcePath, FileMode.Create, FileAccess.ReadWrite))
                    {
                        while ((bytesRead = m_ApplyingResourcePackStream.Read(m_CachedBytes, 0, bytesLeft < CachedBytesLength ? bytesLeft : CachedBytesLength)) > 0)
                        {
                            bytesLeft -= bytesRead;
                            fileStream.Write(m_CachedBytes, 0, bytesRead);
                        }

                        if (compressed)
                        {
                            fileStream.Position = 0L;
                            int hashCode = Utility.Verifier.GetCrc32(fileStream);
                            if (hashCode != applyInfo.CompressedHashCode)
                            {
                                if (ResourceApplyFailure != null)
                                {
                                    string errorMessage = Utility.Text.Format("Resource compressed hash code error, need '{0}', applied '{1}'.", applyInfo.CompressedHashCode, hashCode);
                                    ResourceApplyFailure(applyInfo.ResourceName, m_ApplyingResourcePackPath, errorMessage);
                                }

                                m_FailureFlag = true;
                                return false;
                            }

                            fileStream.Position = 0L;
                            m_ResourceManager.PrepareCachedStream();
                            if (!Utility.Compression.Decompress(fileStream, m_ResourceManager.m_CachedStream))
                            {
                                if (ResourceApplyFailure != null)
                                {
                                    string errorMessage = Utility.Text.Format("Unable to decompress resource '{0}'.", applyInfo.ResourcePath);
                                    ResourceApplyFailure(applyInfo.ResourceName, m_ApplyingResourcePackPath, errorMessage);
                                }

                                m_FailureFlag = true;
                                return false;
                            }

                            fileStream.Position = 0L;
                            fileStream.SetLength(0L);
                            fileStream.Write(m_ResourceManager.m_CachedStream.GetBuffer(), 0, (int)m_ResourceManager.m_CachedStream.Length);
                        }
                        else
                        {
                            int hashCode = 0;
                            fileStream.Position = 0L;
                            if (applyInfo.LoadType == LoadType.LoadFromMemoryAndQuickDecrypt || applyInfo.LoadType == LoadType.LoadFromMemoryAndDecrypt
                                || applyInfo.LoadType == LoadType.LoadFromBinaryAndQuickDecrypt || applyInfo.LoadType == LoadType.LoadFromBinaryAndDecrypt)
                            {
                                Utility.Converter.GetBytes(applyInfo.HashCode, m_CachedHashBytes);
                                if (applyInfo.LoadType == LoadType.LoadFromMemoryAndQuickDecrypt || applyInfo.LoadType == LoadType.LoadFromBinaryAndQuickDecrypt)
                                {
                                    hashCode = Utility.Verifier.GetCrc32(fileStream, m_CachedHashBytes, Utility.Encryption.QuickEncryptLength);
                                }
                                else if (applyInfo.LoadType == LoadType.LoadFromMemoryAndDecrypt || applyInfo.LoadType == LoadType.LoadFromBinaryAndDecrypt)
                                {
                                    hashCode = Utility.Verifier.GetCrc32(fileStream, m_CachedHashBytes, applyInfo.Length);
                                }

                                Array.Clear(m_CachedHashBytes, 0, CachedHashBytesLength);
                            }
                            else
                            {
                                hashCode = Utility.Verifier.GetCrc32(fileStream);
                            }

                            if (hashCode != applyInfo.HashCode)
                            {
                                if (ResourceApplyFailure != null)
                                {
                                    string errorMessage = Utility.Text.Format("Resource hash code error, need '{0}', applied '{1}'.", applyInfo.HashCode, hashCode);
                                    ResourceApplyFailure(applyInfo.ResourceName, m_ApplyingResourcePackPath, errorMessage);
                                }

                                m_FailureFlag = true;
                                return false;
                            }
                        }
                    }

                    if (applyInfo.UseFileSystem)
                    {
                        IFileSystem fileSystem = m_ResourceManager.GetFileSystem(applyInfo.FileSystemName, false);
                        bool retVal = fileSystem.WriteFile(applyInfo.ResourceName.FullName, applyInfo.ResourcePath);
                        if (File.Exists(applyInfo.ResourcePath))
                        {
                            File.Delete(applyInfo.ResourcePath);
                        }

                        if (!retVal)
                        {
                            if (ResourceApplyFailure != null)
                            {
                                string errorMessage = Utility.Text.Format("Unable to write resource '{0}' to file system '{1}'.", applyInfo.ResourcePath, applyInfo.FileSystemName);
                                ResourceApplyFailure(applyInfo.ResourceName, m_ApplyingResourcePackPath, errorMessage);
                            }

                            m_FailureFlag = true;
                            return false;
                        }
                    }

                    string downloadingResource = Utility.Text.Format("{0}.download", applyInfo.ResourcePath);
                    if (File.Exists(downloadingResource))
                    {
                        File.Delete(downloadingResource);
                    }

                    m_UpdateCandidateInfo.Remove(applyInfo.ResourceName);
                    m_ResourceManager.m_ResourceInfos[applyInfo.ResourceName].MarkReady();
                    m_ResourceManager.m_ReadWriteResourceInfos.Add(applyInfo.ResourceName, new ReadWriteResourceInfo(applyInfo.FileSystemName, applyInfo.LoadType, applyInfo.Length, applyInfo.HashCode));
                    if (ResourceApplySuccess != null)
                    {
                        ResourceApplySuccess(applyInfo.ResourceName, applyInfo.ResourcePath, m_ApplyingResourcePackPath, applyInfo.Length, applyInfo.CompressedLength);
                    }

                    m_CurrentGenerateReadWriteVersionListLength += applyInfo.CompressedLength;
                    if (m_ApplyWaitingInfo.Count <= 0 || m_CurrentGenerateReadWriteVersionListLength >= m_GenerateReadWriteVersionListLength)
                    {
                        GenerateReadWriteVersionList();
                        return true;
                    }

                    return false;
                }
                catch (Exception exception)
                {
                    if (ResourceApplyFailure != null)
                    {
                        ResourceApplyFailure(applyInfo.ResourceName, m_ApplyingResourcePackPath, exception.ToString());
                    }

                    m_FailureFlag = true;
                    return false;
                }
                finally
                {
                    m_ApplyingResourcePackStream.Position = position;
                }
            }

            private bool DownloadResource(UpdateInfo updateInfo)
            {
                if (updateInfo.Downloading)
                {
                    return false;
                }

                updateInfo.Downloading = true;
                string resourceFullNameWithCrc32 = updateInfo.ResourceName.Variant != null ? Utility.Text.Format("{0}.{1}.{2:x8}.{3}", updateInfo.ResourceName.Name, updateInfo.ResourceName.Variant, updateInfo.HashCode, DefaultExtension) : Utility.Text.Format("{0}.{1:x8}.{2}", updateInfo.ResourceName.Name, updateInfo.HashCode, DefaultExtension);
                m_DownloadManager.AddDownload(updateInfo.ResourcePath, Utility.Path.GetRemotePath(Path.Combine(m_ResourceManager.m_UpdatePrefixUri, resourceFullNameWithCrc32)), updateInfo);
                return true;
            }

            private void GenerateReadWriteVersionList()
            {
                FileStream fileStream = null;
                try
                {
                    fileStream = new FileStream(m_ReadWriteVersionListTempFileName, FileMode.Create, FileAccess.Write);
                    LocalVersionList.Resource[] resources = m_ResourceManager.m_ReadWriteResourceInfos.Count > 0 ? new LocalVersionList.Resource[m_ResourceManager.m_ReadWriteResourceInfos.Count] : null;
                    if (resources != null)
                    {
                        int index = 0;
                        foreach (KeyValuePair<ResourceName, ReadWriteResourceInfo> i in m_ResourceManager.m_ReadWriteResourceInfos)
                        {
                            ResourceName resourceName = i.Key;
                            ReadWriteResourceInfo resourceInfo = i.Value;
                            resources[index] = new LocalVersionList.Resource(resourceName.Name, resourceName.Variant, resourceName.Extension, (byte)resourceInfo.LoadType, resourceInfo.Length, resourceInfo.HashCode);
                            if (resourceInfo.UseFileSystem)
                            {
                                List<int> resourceIndexes = null;
                                if (!m_CachedFileSystemsForGenerateReadWriteVersionList.TryGetValue(resourceInfo.FileSystemName, out resourceIndexes))
                                {
                                    resourceIndexes = new List<int>();
                                    m_CachedFileSystemsForGenerateReadWriteVersionList.Add(resourceInfo.FileSystemName, resourceIndexes);
                                }

                                resourceIndexes.Add(index);
                            }

                            index++;
                        }
                    }

                    LocalVersionList.FileSystem[] fileSystems = m_CachedFileSystemsForGenerateReadWriteVersionList.Count > 0 ? new LocalVersionList.FileSystem[m_CachedFileSystemsForGenerateReadWriteVersionList.Count] : null;
                    if (fileSystems != null)
                    {
                        int index = 0;
                        foreach (KeyValuePair<string, List<int>> i in m_CachedFileSystemsForGenerateReadWriteVersionList)
                        {
                            fileSystems[index++] = new LocalVersionList.FileSystem(i.Key, i.Value.ToArray());
                            i.Value.Clear();
                        }
                    }

                    LocalVersionList versionList = new LocalVersionList(resources, fileSystems);
                    if (!m_ResourceManager.m_ReadWriteVersionListSerializer.Serialize(fileStream, versionList))
                    {
                        throw new GameFrameworkException("Serialize read-write version list failure.");
                    }

                    if (fileStream != null)
                    {
                        fileStream.Dispose();
                        fileStream = null;
                    }
                }
                catch (Exception exception)
                {
                    if (fileStream != null)
                    {
                        fileStream.Dispose();
                        fileStream = null;
                    }

                    if (File.Exists(m_ReadWriteVersionListTempFileName))
                    {
                        File.Delete(m_ReadWriteVersionListTempFileName);
                    }

                    throw new GameFrameworkException(Utility.Text.Format("Generate read-write version list exception '{0}'.", exception), exception);
                }

                if (File.Exists(m_ReadWriteVersionListFileName))
                {
                    File.Delete(m_ReadWriteVersionListFileName);
                }

                File.Move(m_ReadWriteVersionListTempFileName, m_ReadWriteVersionListFileName);
                m_CurrentGenerateReadWriteVersionListLength = 0;
            }

            private void OnDownloadStart(object sender, DownloadStartEventArgs e)
            {
                UpdateInfo updateInfo = e.UserData as UpdateInfo;
                if (updateInfo == null)
                {
                    return;
                }

                if (m_DownloadManager == null)
                {
                    throw new GameFrameworkException("You must set download manager first.");
                }

                if (e.CurrentLength > int.MaxValue)
                {
                    throw new GameFrameworkException(Utility.Text.Format("File '{0}' is too large.", e.DownloadPath));
                }

                if (ResourceUpdateStart != null)
                {
                    ResourceUpdateStart(updateInfo.ResourceName, e.DownloadPath, e.DownloadUri, (int)e.CurrentLength, updateInfo.CompressedLength, updateInfo.RetryCount);
                }
            }

            private void OnDownloadUpdate(object sender, DownloadUpdateEventArgs e)
            {
                UpdateInfo updateInfo = e.UserData as UpdateInfo;
                if (updateInfo == null)
                {
                    return;
                }

                if (m_DownloadManager == null)
                {
                    throw new GameFrameworkException("You must set download manager first.");
                }

                if (e.CurrentLength > updateInfo.CompressedLength)
                {
                    m_DownloadManager.RemoveDownload(e.SerialId);
                    string downloadFile = Utility.Text.Format("{0}.download", e.DownloadPath);
                    if (File.Exists(downloadFile))
                    {
                        File.Delete(downloadFile);
                    }

                    string errorMessage = Utility.Text.Format("When download update, downloaded length is larger than compressed length, need '{0}', downloaded '{1}'.", updateInfo.CompressedLength, e.CurrentLength);
                    DownloadFailureEventArgs downloadFailureEventArgs = DownloadFailureEventArgs.Create(e.SerialId, e.DownloadPath, e.DownloadUri, errorMessage, e.UserData);
                    OnDownloadFailure(this, downloadFailureEventArgs);
                    ReferencePool.Release(downloadFailureEventArgs);
                    return;
                }

                if (ResourceUpdateChanged != null)
                {
                    ResourceUpdateChanged(updateInfo.ResourceName, e.DownloadPath, e.DownloadUri, (int)e.CurrentLength, updateInfo.CompressedLength);
                }
            }

            private void OnDownloadSuccess(object sender, DownloadSuccessEventArgs e)
            {
                UpdateInfo updateInfo = e.UserData as UpdateInfo;
                if (updateInfo == null)
                {
                    return;
                }

                try
                {
                    using (FileStream fileStream = new FileStream(e.DownloadPath, FileMode.Open, FileAccess.ReadWrite))
                    {
                        bool compressed = updateInfo.Length != updateInfo.CompressedLength || updateInfo.HashCode != updateInfo.CompressedHashCode;

                        int length = (int)fileStream.Length;
                        if (length != updateInfo.CompressedLength)
                        {
                            fileStream.Close();
                            string errorMessage = Utility.Text.Format("Resource compressed length error, need '{0}', downloaded '{1}'.", updateInfo.CompressedLength, length);
                            DownloadFailureEventArgs downloadFailureEventArgs = DownloadFailureEventArgs.Create(e.SerialId, e.DownloadPath, e.DownloadUri, errorMessage, e.UserData);
                            OnDownloadFailure(this, downloadFailureEventArgs);
                            ReferencePool.Release(downloadFailureEventArgs);
                            return;
                        }

                        if (compressed)
                        {
                            fileStream.Position = 0L;
                            int hashCode = Utility.Verifier.GetCrc32(fileStream);
                            if (hashCode != updateInfo.CompressedHashCode)
                            {
                                fileStream.Close();
                                string errorMessage = Utility.Text.Format("Resource compressed hash code error, need '{0}', downloaded '{1}'.", updateInfo.CompressedHashCode, hashCode);
                                DownloadFailureEventArgs downloadFailureEventArgs = DownloadFailureEventArgs.Create(e.SerialId, e.DownloadPath, e.DownloadUri, errorMessage, e.UserData);
                                OnDownloadFailure(this, downloadFailureEventArgs);
                                ReferencePool.Release(downloadFailureEventArgs);
                                return;
                            }

                            fileStream.Position = 0L;
                            m_ResourceManager.PrepareCachedStream();
                            if (!Utility.Compression.Decompress(fileStream, m_ResourceManager.m_CachedStream))
                            {
                                fileStream.Close();
                                string errorMessage = Utility.Text.Format("Unable to decompress resource '{0}'.", e.DownloadPath);
                                DownloadFailureEventArgs downloadFailureEventArgs = DownloadFailureEventArgs.Create(e.SerialId, e.DownloadPath, e.DownloadUri, errorMessage, e.UserData);
                                OnDownloadFailure(this, downloadFailureEventArgs);
                                ReferencePool.Release(downloadFailureEventArgs);
                                return;
                            }

                            int uncompressedLength = (int)m_ResourceManager.m_CachedStream.Length;
                            if (uncompressedLength != updateInfo.Length)
                            {
                                fileStream.Close();
                                string errorMessage = Utility.Text.Format("Resource length error, need '{0}', downloaded '{1}'.", updateInfo.Length, uncompressedLength);
                                DownloadFailureEventArgs downloadFailureEventArgs = DownloadFailureEventArgs.Create(e.SerialId, e.DownloadPath, e.DownloadUri, errorMessage, e.UserData);
                                OnDownloadFailure(this, downloadFailureEventArgs);
                                ReferencePool.Release(downloadFailureEventArgs);
                                return;
                            }

                            fileStream.Position = 0L;
                            fileStream.SetLength(0L);
                            fileStream.Write(m_ResourceManager.m_CachedStream.GetBuffer(), 0, uncompressedLength);
                        }
                        else
                        {
                            int hashCode = 0;
                            fileStream.Position = 0L;
                            if (updateInfo.LoadType == LoadType.LoadFromMemoryAndQuickDecrypt || updateInfo.LoadType == LoadType.LoadFromMemoryAndDecrypt
                                || updateInfo.LoadType == LoadType.LoadFromBinaryAndQuickDecrypt || updateInfo.LoadType == LoadType.LoadFromBinaryAndDecrypt)
                            {
                                Utility.Converter.GetBytes(updateInfo.HashCode, m_CachedHashBytes);
                                if (updateInfo.LoadType == LoadType.LoadFromMemoryAndQuickDecrypt || updateInfo.LoadType == LoadType.LoadFromBinaryAndQuickDecrypt)
                                {
                                    hashCode = Utility.Verifier.GetCrc32(fileStream, m_CachedHashBytes, Utility.Encryption.QuickEncryptLength);
                                }
                                else if (updateInfo.LoadType == LoadType.LoadFromMemoryAndDecrypt || updateInfo.LoadType == LoadType.LoadFromBinaryAndDecrypt)
                                {
                                    hashCode = Utility.Verifier.GetCrc32(fileStream, m_CachedHashBytes, length);
                                }

                                Array.Clear(m_CachedHashBytes, 0, CachedHashBytesLength);
                            }
                            else
                            {
                                hashCode = Utility.Verifier.GetCrc32(fileStream);
                            }

                            if (hashCode != updateInfo.HashCode)
                            {
                                fileStream.Close();
                                string errorMessage = Utility.Text.Format("Resource hash code error, need '{0}', downloaded '{1}'.", updateInfo.HashCode, hashCode);
                                DownloadFailureEventArgs downloadFailureEventArgs = DownloadFailureEventArgs.Create(e.SerialId, e.DownloadPath, e.DownloadUri, errorMessage, e.UserData);
                                OnDownloadFailure(this, downloadFailureEventArgs);
                                ReferencePool.Release(downloadFailureEventArgs);
                                return;
                            }
                        }
                    }

                    if (updateInfo.UseFileSystem)
                    {
                        IFileSystem fileSystem = m_ResourceManager.GetFileSystem(updateInfo.FileSystemName, false);
                        bool retVal = fileSystem.WriteFile(updateInfo.ResourceName.FullName, updateInfo.ResourcePath);
                        if (File.Exists(updateInfo.ResourcePath))
                        {
                            File.Delete(updateInfo.ResourcePath);
                        }

                        if (!retVal)
                        {
                            string errorMessage = Utility.Text.Format("Write resource to file system '{0}' error.", fileSystem.FullPath);
                            DownloadFailureEventArgs downloadFailureEventArgs = DownloadFailureEventArgs.Create(e.SerialId, e.DownloadPath, e.DownloadUri, errorMessage, e.UserData);
                            OnDownloadFailure(this, downloadFailureEventArgs);
                            ReferencePool.Release(downloadFailureEventArgs);
                            return;
                        }
                    }

                    m_UpdateCandidateInfo.Remove(updateInfo.ResourceName);
                    m_UpdateWaitingInfo.Remove(updateInfo);
                    m_UpdateWaitingInfoWhilePlaying.Remove(updateInfo);
                    m_ResourceManager.m_ResourceInfos[updateInfo.ResourceName].MarkReady();
                    m_ResourceManager.m_ReadWriteResourceInfos.Add(updateInfo.ResourceName, new ReadWriteResourceInfo(updateInfo.FileSystemName, updateInfo.LoadType, updateInfo.Length, updateInfo.HashCode));
                    if (ResourceUpdateSuccess != null)
                    {
                        ResourceUpdateSuccess(updateInfo.ResourceName, e.DownloadPath, e.DownloadUri, updateInfo.Length, updateInfo.CompressedLength);
                    }

                    m_CurrentGenerateReadWriteVersionListLength += updateInfo.CompressedLength;
                    if (m_UpdateCandidateInfo.Count <= 0 || m_UpdateWaitingInfo.Count + m_UpdateWaitingInfoWhilePlaying.Count <= 0 || m_CurrentGenerateReadWriteVersionListLength >= m_GenerateReadWriteVersionListLength)
                    {
                        GenerateReadWriteVersionList();
                    }

                    if (m_UpdatingResourceGroup != null && m_UpdateWaitingInfo.Count <= 0)
                    {
                        ResourceGroup updatingResourceGroup = m_UpdatingResourceGroup;
                        m_UpdatingResourceGroup = null;
                        if (ResourceUpdateComplete != null)
                        {
                            ResourceUpdateComplete(updatingResourceGroup, !m_FailureFlag);
                        }
                    }

                    if (m_UpdateCandidateInfo.Count <= 0 && ResourceUpdateAllComplete != null)
                    {
                        ResourceUpdateAllComplete();
                    }
                }
                catch (Exception exception)
                {
                    string errorMessage = Utility.Text.Format("Update resource '{0}' with error message '{1}'.", e.DownloadPath, exception);
                    DownloadFailureEventArgs downloadFailureEventArgs = DownloadFailureEventArgs.Create(e.SerialId, e.DownloadPath, e.DownloadUri, errorMessage, e.UserData);
                    OnDownloadFailure(this, downloadFailureEventArgs);
                    ReferencePool.Release(downloadFailureEventArgs);
                }
            }

            private void OnDownloadFailure(object sender, DownloadFailureEventArgs e)
            {
                UpdateInfo updateInfo = e.UserData as UpdateInfo;
                if (updateInfo == null)
                {
                    return;
                }

                if (File.Exists(e.DownloadPath))
                {
                    File.Delete(e.DownloadPath);
                }

                if (ResourceUpdateFailure != null)
                {
                    ResourceUpdateFailure(updateInfo.ResourceName, e.DownloadUri, updateInfo.RetryCount, m_UpdateRetryCount, e.ErrorMessage);
                }

                if (updateInfo.RetryCount < m_UpdateRetryCount)
                {
                    updateInfo.Downloading = false;
                    updateInfo.RetryCount++;
                    if (m_UpdateWaitingInfoWhilePlaying.Contains(updateInfo))
                    {
                        DownloadResource(updateInfo);
                    }
                }
                else
                {
                    m_FailureFlag = true;
                    updateInfo.Downloading = false;
                    updateInfo.RetryCount = 0;
                    m_UpdateWaitingInfo.Remove(updateInfo);
                    m_UpdateWaitingInfoWhilePlaying.Remove(updateInfo);
                }
            }
        }
    }
}
