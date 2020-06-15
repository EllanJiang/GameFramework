//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework.Download;
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
            private readonly List<ApplyInfo> m_ApplyWaitingInfo;
            private readonly List<UpdateInfo> m_UpdateWaitingInfo;
            private readonly Dictionary<ResourceName, UpdateInfo> m_UpdateCandidateInfo;
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
            private int m_UpdatingCount;
            private bool m_FailureFlag;
            private string m_ReadWriteVersionListFileName;
            private string m_ReadWriteVersionListBackupFileName;

            public GameFrameworkAction<ResourceName, string, string, int, int> ResourceApplySuccess;
            public GameFrameworkAction<ResourceName, string, string> ResourceApplyFailure;
            public GameFrameworkAction<string, bool, bool> ResourceApplyComplete;
            public GameFrameworkAction<ResourceName, string, string, int, int, int> ResourceUpdateStart;
            public GameFrameworkAction<ResourceName, string, string, int, int> ResourceUpdateChanged;
            public GameFrameworkAction<ResourceName, string, string, int, int> ResourceUpdateSuccess;
            public GameFrameworkAction<ResourceName, string, int, int, string> ResourceUpdateFailure;
            public GameFrameworkAction<ResourceGroup, bool, bool> ResourceUpdateComplete;

            /// <summary>
            /// 初始化资源更新器的新实例。
            /// </summary>
            /// <param name="resourceManager">资源管理器。</param>
            public ResourceUpdater(ResourceManager resourceManager)
            {
                m_ResourceManager = resourceManager;
                m_ApplyWaitingInfo = new List<ApplyInfo>();
                m_UpdateWaitingInfo = new List<UpdateInfo>();
                m_UpdateCandidateInfo = new Dictionary<ResourceName, UpdateInfo>();
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
                m_UpdatingCount = 0;
                m_FailureFlag = false;
                m_ReadWriteVersionListFileName = Utility.Path.GetRegularPath(Path.Combine(m_ResourceManager.m_ReadWritePath, LocalVersionListFileName));
                m_ReadWriteVersionListBackupFileName = Utility.Text.Format("{0}.{1}", m_ReadWriteVersionListFileName, BackupExtension);

                ResourceApplySuccess = null;
                ResourceApplyFailure = null;
                ResourceApplyComplete = null;
                ResourceUpdateStart = null;
                ResourceUpdateChanged = null;
                ResourceUpdateSuccess = null;
                ResourceUpdateFailure = null;
                ResourceUpdateComplete = null;
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
            /// 获取正在更新资源数量。
            /// </summary>
            public int UpdatingCount
            {
                get
                {
                    return m_UpdatingCount;
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
                        ApplyInfo applyInfo = m_ApplyWaitingInfo[0];
                        m_ApplyWaitingInfo.RemoveAt(0);
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
                        ResourceApplyComplete(resourcePackPath, !m_FailureFlag, m_UpdateCandidateInfo.Count <= 0);
                    }
                }

                if (m_UpdateWaitingInfo.Count > 0)
                {
                    if (m_DownloadManager.FreeAgentCount > 0)
                    {
                        UpdateInfo updateInfo = m_UpdateWaitingInfo[0];
                        m_UpdateWaitingInfo.RemoveAt(0);
                        string resourceFullNameWithCrc32 = updateInfo.ResourceName.Variant != null ? Utility.Text.Format("{0}.{1}.{2:x8}.{3}", updateInfo.ResourceName.Name, updateInfo.ResourceName.Variant, updateInfo.HashCode, DefaultExtension) : Utility.Text.Format("{0}.{1:x8}.{2}", updateInfo.ResourceName.Name, updateInfo.HashCode, DefaultExtension);
                        m_DownloadManager.AddDownload(updateInfo.ResourcePath, Utility.Path.GetRemotePath(Path.Combine(m_ResourceManager.m_UpdatePrefixUri, resourceFullNameWithCrc32)), updateInfo);
                        m_UpdatingCount++;
                    }

                    return;
                }

                if (m_UpdatingResourceGroup != null && m_UpdatingCount <= 0)
                {
                    ResourceGroup updatingResourceGroup = m_UpdatingResourceGroup;
                    m_UpdatingResourceGroup = null;
                    if (ResourceUpdateComplete != null)
                    {
                        ResourceUpdateComplete(updatingResourceGroup, !m_FailureFlag, m_UpdateCandidateInfo.Count <= 0);
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
            /// <param name="loadType">资源加载方式。</param>
            /// <param name="length">资源大小。</param>
            /// <param name="hashCode">资源哈希值。</param>
            /// <param name="zipLength">压缩后大小。</param>
            /// <param name="zipHashCode">压缩后哈希值。</param>
            /// <param name="resourcePath">资源路径。</param>
            public void AddResourceUpdate(ResourceName resourceName, LoadType loadType, int length, int hashCode, int zipLength, int zipHashCode, string resourcePath)
            {
                m_UpdateCandidateInfo.Add(resourceName, new UpdateInfo(resourceName, loadType, length, hashCode, zipLength, zipHashCode, resourcePath));
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
                            m_ApplyWaitingInfo.Add(new ApplyInfo(resourceName, (LoadType)resource.LoadType, resource.Offset, resource.Length, resource.HashCode, resource.ZipLength, resource.ZipHashCode, updateInfo.ResourcePath));
                        }
                    }
                }
                catch (Exception exception)
                {
                    if (m_ApplyingResourcePackStream != null)
                    {
                        m_ApplyingResourcePackStream.Dispose();
                        m_ApplyingResourcePackStream = null;
                    }

                    throw new GameFrameworkException(Utility.Text.Format("Apply resources '{0}' with exception '{1}'.", resourcePackPath, exception.ToString()), exception);
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

                    m_UpdateCandidateInfo.Clear();
                }
                else
                {
                    ResourceName[] resourceNames = resourceGroup.InternalGetResourceNames();
                    foreach (ResourceName resourceName in resourceNames)
                    {
                        UpdateInfo updateInfo = null;
                        if (!m_UpdateCandidateInfo.TryGetValue(resourceName, out updateInfo))
                        {
                            continue;
                        }

                        m_UpdateWaitingInfo.Add(updateInfo);
                        m_UpdateCandidateInfo.Remove(resourceName);
                    }
                }

                m_UpdatingResourceGroup = resourceGroup;
                m_FailureFlag = false;
            }

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
                if (m_UpdateCandidateInfo.TryGetValue(resourceName, out updateInfo))
                {
                    m_UpdateWaitingInfo.Add(updateInfo);
                    m_UpdateCandidateInfo.Remove(resourceName);
                }
            }

            private bool ApplyResource(ApplyInfo applyInfo)
            {
                long position = m_ApplyingResourcePackStream.Position;
                try
                {
                    bool zip = applyInfo.Length != applyInfo.ZipLength || applyInfo.HashCode != applyInfo.ZipHashCode;

                    int bytesRead = 0;
                    int bytesLeft = applyInfo.ZipLength;
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

                        if (zip)
                        {
                            fileStream.Position = 0L;
                            int hashCode = Utility.Verifier.GetCrc32(fileStream);
                            if (hashCode != applyInfo.ZipHashCode)
                            {
                                if (ResourceApplyFailure != null)
                                {
                                    string errorMessage = Utility.Text.Format("Resource zip hash code error, need '{0}', applied '{1}'.", applyInfo.ZipHashCode.ToString(), hashCode.ToString());
                                    ResourceApplyFailure(applyInfo.ResourceName, m_ApplyingResourcePackPath, errorMessage);
                                }

                                return false;
                            }

                            if (m_ResourceManager.m_DecompressCachedStream == null)
                            {
                                m_ResourceManager.m_DecompressCachedStream = new MemoryStream();
                            }

                            fileStream.Position = 0L;
                            m_ResourceManager.m_DecompressCachedStream.Position = 0L;
                            m_ResourceManager.m_DecompressCachedStream.SetLength(0L);
                            if (!Utility.Zip.Decompress(fileStream, m_ResourceManager.m_DecompressCachedStream))
                            {
                                if (ResourceApplyFailure != null)
                                {
                                    string errorMessage = Utility.Text.Format("Unable to decompress resource '{0}'.", applyInfo.ResourcePath);
                                    ResourceApplyFailure(applyInfo.ResourceName, m_ApplyingResourcePackPath, errorMessage);
                                }

                                return false;
                            }

                            fileStream.Position = 0L;
                            fileStream.SetLength(0L);
                            fileStream.Write(m_ResourceManager.m_DecompressCachedStream.GetBuffer(), 0, (int)m_ResourceManager.m_DecompressCachedStream.Length);
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
                                    string errorMessage = Utility.Text.Format("Resource hash code error, need '{0}', applied '{1}'.", applyInfo.HashCode.ToString(), hashCode.ToString());
                                    ResourceApplyFailure(applyInfo.ResourceName, m_ApplyingResourcePackPath, errorMessage);
                                }

                                return false;
                            }
                        }
                    }

                    m_UpdateCandidateInfo.Remove(applyInfo.ResourceName);
                    m_ResourceManager.m_ResourceInfos[applyInfo.ResourceName].MarkReady();
                    m_ResourceManager.m_ReadWriteResourceInfos.Add(applyInfo.ResourceName, new ReadWriteResourceInfo(applyInfo.LoadType, applyInfo.Length, applyInfo.HashCode));

                    if (ResourceApplySuccess != null)
                    {
                        ResourceApplySuccess(applyInfo.ResourceName, applyInfo.ResourcePath, m_ApplyingResourcePackPath, applyInfo.Length, applyInfo.ZipLength);
                    }

                    string downloadingResource = Utility.Text.Format("{0}.download", applyInfo.ResourcePath);
                    if (File.Exists(downloadingResource))
                    {
                        File.Delete(downloadingResource);
                    }

                    m_CurrentGenerateReadWriteVersionListLength += applyInfo.ZipLength;
                    if (m_ApplyWaitingInfo.Count <= 0 || m_CurrentGenerateReadWriteVersionListLength >= m_GenerateReadWriteVersionListLength)
                    {
                        m_CurrentGenerateReadWriteVersionListLength = 0;
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

                    return false;
                }
                finally
                {
                    m_ApplyingResourcePackStream.Position = position;
                }
            }

            private void GenerateReadWriteVersionList()
            {
                if (File.Exists(m_ReadWriteVersionListFileName))
                {
                    if (File.Exists(m_ReadWriteVersionListBackupFileName))
                    {
                        File.Delete(m_ReadWriteVersionListBackupFileName);
                    }

                    File.Move(m_ReadWriteVersionListFileName, m_ReadWriteVersionListBackupFileName);
                }

                FileStream fileStream = null;
                try
                {
                    fileStream = new FileStream(m_ReadWriteVersionListFileName, FileMode.Create, FileAccess.Write);
                    LocalVersionList.Resource[] resources = m_ResourceManager.m_ReadWriteResourceInfos.Count > 0 ? new LocalVersionList.Resource[m_ResourceManager.m_ReadWriteResourceInfos.Count] : null;
                    if (resources != null)
                    {
                        int index = 0;
                        foreach (KeyValuePair<ResourceName, ReadWriteResourceInfo> i in m_ResourceManager.m_ReadWriteResourceInfos)
                        {
                            resources[index++] = new LocalVersionList.Resource(i.Key.Name, i.Key.Variant, i.Key.Extension, (byte)i.Value.LoadType, i.Value.Length, i.Value.HashCode);
                        }
                    }

                    LocalVersionList versionList = new LocalVersionList(resources);
                    if (!m_ResourceManager.m_ReadWriteVersionListSerializer.Serialize(fileStream, versionList))
                    {
                        throw new GameFrameworkException("Serialize read write version list failure.");
                    }

                    if (fileStream != null)
                    {
                        fileStream.Dispose();
                        fileStream = null;
                    }

                    if (!string.IsNullOrEmpty(m_ReadWriteVersionListBackupFileName))
                    {
                        File.Delete(m_ReadWriteVersionListBackupFileName);
                    }
                }
                catch (Exception exception)
                {
                    if (fileStream != null)
                    {
                        fileStream.Dispose();
                        fileStream = null;
                    }

                    if (File.Exists(m_ReadWriteVersionListFileName))
                    {
                        File.Delete(m_ReadWriteVersionListFileName);
                    }

                    if (!string.IsNullOrEmpty(m_ReadWriteVersionListBackupFileName))
                    {
                        File.Move(m_ReadWriteVersionListBackupFileName, m_ReadWriteVersionListFileName);
                    }

                    throw new GameFrameworkException(Utility.Text.Format("Generate read write version list exception '{0}'.", exception.ToString()), exception);
                }
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

                if (ResourceUpdateStart != null)
                {
                    ResourceUpdateStart(updateInfo.ResourceName, e.DownloadPath, e.DownloadUri, e.CurrentLength, updateInfo.ZipLength, updateInfo.RetryCount);
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

                if (e.CurrentLength > updateInfo.ZipLength)
                {
                    m_DownloadManager.RemoveDownload(e.SerialId);
                    string downloadFile = Utility.Text.Format("{0}.download", e.DownloadPath);
                    if (File.Exists(downloadFile))
                    {
                        File.Delete(downloadFile);
                    }

                    string errorMessage = Utility.Text.Format("When download update, downloaded length is larger than zip length, need '{0}', downloaded '{1}'.", updateInfo.ZipLength.ToString(), e.CurrentLength.ToString());
                    DownloadFailureEventArgs downloadFailureEventArgs = DownloadFailureEventArgs.Create(e.SerialId, e.DownloadPath, e.DownloadUri, errorMessage, e.UserData);
                    OnDownloadFailure(this, downloadFailureEventArgs);
                    ReferencePool.Release(downloadFailureEventArgs);
                    return;
                }

                if (ResourceUpdateChanged != null)
                {
                    ResourceUpdateChanged(updateInfo.ResourceName, e.DownloadPath, e.DownloadUri, e.CurrentLength, updateInfo.ZipLength);
                }
            }

            private void OnDownloadSuccess(object sender, DownloadSuccessEventArgs e)
            {
                UpdateInfo updateInfo = e.UserData as UpdateInfo;
                if (updateInfo == null)
                {
                    return;
                }

                using (FileStream fileStream = new FileStream(e.DownloadPath, FileMode.Open, FileAccess.ReadWrite))
                {
                    bool zip = updateInfo.Length != updateInfo.ZipLength || updateInfo.HashCode != updateInfo.ZipHashCode;

                    int length = (int)fileStream.Length;
                    if (length != updateInfo.ZipLength)
                    {
                        fileStream.Close();
                        string errorMessage = Utility.Text.Format("Resource zip length error, need '{0}', downloaded '{1}'.", updateInfo.ZipLength.ToString(), length.ToString());
                        DownloadFailureEventArgs downloadFailureEventArgs = DownloadFailureEventArgs.Create(e.SerialId, e.DownloadPath, e.DownloadUri, errorMessage, e.UserData);
                        OnDownloadFailure(this, downloadFailureEventArgs);
                        ReferencePool.Release(downloadFailureEventArgs);
                        return;
                    }

                    if (zip)
                    {
                        fileStream.Position = 0L;
                        int hashCode = Utility.Verifier.GetCrc32(fileStream);
                        if (hashCode != updateInfo.ZipHashCode)
                        {
                            fileStream.Close();
                            string errorMessage = Utility.Text.Format("Resource zip hash code error, need '{0}', downloaded '{1}'.", updateInfo.ZipHashCode.ToString(), hashCode.ToString());
                            DownloadFailureEventArgs downloadFailureEventArgs = DownloadFailureEventArgs.Create(e.SerialId, e.DownloadPath, e.DownloadUri, errorMessage, e.UserData);
                            OnDownloadFailure(this, downloadFailureEventArgs);
                            ReferencePool.Release(downloadFailureEventArgs);
                            return;
                        }

                        if (m_ResourceManager.m_DecompressCachedStream == null)
                        {
                            m_ResourceManager.m_DecompressCachedStream = new MemoryStream();
                        }

                        try
                        {
                            fileStream.Position = 0L;
                            m_ResourceManager.m_DecompressCachedStream.Position = 0L;
                            m_ResourceManager.m_DecompressCachedStream.SetLength(0L);
                            if (!Utility.Zip.Decompress(fileStream, m_ResourceManager.m_DecompressCachedStream))
                            {
                                fileStream.Close();
                                string errorMessage = Utility.Text.Format("Unable to decompress resource '{0}'.", e.DownloadPath);
                                DownloadFailureEventArgs downloadFailureEventArgs = DownloadFailureEventArgs.Create(e.SerialId, e.DownloadPath, e.DownloadUri, errorMessage, e.UserData);
                                OnDownloadFailure(this, downloadFailureEventArgs);
                                ReferencePool.Release(downloadFailureEventArgs);
                                return;
                            }

                            if (m_ResourceManager.m_DecompressCachedStream.Length != updateInfo.Length)
                            {
                                fileStream.Close();
                                string errorMessage = Utility.Text.Format("Resource length error, need '{0}', downloaded '{1}'.", updateInfo.Length.ToString(), m_ResourceManager.m_DecompressCachedStream.Length.ToString());
                                DownloadFailureEventArgs downloadFailureEventArgs = DownloadFailureEventArgs.Create(e.SerialId, e.DownloadPath, e.DownloadUri, errorMessage, e.UserData);
                                OnDownloadFailure(this, downloadFailureEventArgs);
                                ReferencePool.Release(downloadFailureEventArgs);
                                return;
                            }

                            fileStream.Position = 0L;
                            fileStream.SetLength(0L);
                            fileStream.Write(m_ResourceManager.m_DecompressCachedStream.GetBuffer(), 0, (int)m_ResourceManager.m_DecompressCachedStream.Length);
                        }
                        catch (Exception exception)
                        {
                            fileStream.Close();
                            string errorMessage = Utility.Text.Format("Unable to decompress resource '{0}' with error message '{1}'.", e.DownloadPath, exception.ToString());
                            DownloadFailureEventArgs downloadFailureEventArgs = DownloadFailureEventArgs.Create(e.SerialId, e.DownloadPath, e.DownloadUri, errorMessage, e.UserData);
                            OnDownloadFailure(this, downloadFailureEventArgs);
                            ReferencePool.Release(downloadFailureEventArgs);
                            return;
                        }
                        finally
                        {
                            m_ResourceManager.m_DecompressCachedStream.Position = 0L;
                            m_ResourceManager.m_DecompressCachedStream.SetLength(0L);
                        }
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
                            string errorMessage = Utility.Text.Format("Resource hash code error, need '{0}', downloaded '{1}'.", updateInfo.HashCode.ToString(), hashCode.ToString());
                            DownloadFailureEventArgs downloadFailureEventArgs = DownloadFailureEventArgs.Create(e.SerialId, e.DownloadPath, e.DownloadUri, errorMessage, e.UserData);
                            OnDownloadFailure(this, downloadFailureEventArgs);
                            ReferencePool.Release(downloadFailureEventArgs);
                            return;
                        }
                    }
                }

                m_UpdatingCount--;
                m_ResourceManager.m_ResourceInfos[updateInfo.ResourceName].MarkReady();
                m_ResourceManager.m_ReadWriteResourceInfos.Add(updateInfo.ResourceName, new ReadWriteResourceInfo(updateInfo.LoadType, updateInfo.Length, updateInfo.HashCode));
                m_CurrentGenerateReadWriteVersionListLength += updateInfo.ZipLength;
                if (m_UpdatingCount <= 0 || m_CurrentGenerateReadWriteVersionListLength >= m_GenerateReadWriteVersionListLength)
                {
                    m_CurrentGenerateReadWriteVersionListLength = 0;
                    GenerateReadWriteVersionList();
                }

                if (ResourceUpdateSuccess != null)
                {
                    ResourceUpdateSuccess(updateInfo.ResourceName, e.DownloadPath, e.DownloadUri, updateInfo.Length, updateInfo.ZipLength);
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

                m_UpdatingCount--;

                if (ResourceUpdateFailure != null)
                {
                    ResourceUpdateFailure(updateInfo.ResourceName, e.DownloadUri, updateInfo.RetryCount, m_UpdateRetryCount, e.ErrorMessage);
                }

                if (updateInfo.RetryCount < m_UpdateRetryCount)
                {
                    updateInfo.RetryCount++;
                    m_UpdateWaitingInfo.Add(updateInfo);
                }
                else
                {
                    m_FailureFlag = true;
                    updateInfo.RetryCount = 0;
                    m_UpdateCandidateInfo.Add(updateInfo.ResourceName, updateInfo);
                }
            }
        }
    }
}
