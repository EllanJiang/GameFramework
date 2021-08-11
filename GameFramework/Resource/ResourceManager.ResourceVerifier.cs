//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework.FileSystem;
using System;
using System.Collections.Generic;
using System.IO;

namespace GameFramework.Resource
{
    internal sealed partial class ResourceManager : GameFrameworkModule, IResourceManager
    {
        /// <summary>
        /// 资源校验器。
        /// </summary>
        private sealed partial class ResourceVerifier
        {
            private const int CachedHashBytesLength = 4;

            private readonly ResourceManager m_ResourceManager;
            private readonly List<VerifyInfo> m_VerifyInfos;
            private readonly byte[] m_CachedHashBytes;
            private bool m_LoadReadWriteVersionListComplete;
            private int m_VerifyResourceLengthPerFrame;
            private int m_VerifyResourceIndex;
            private bool m_FailureFlag;

            public GameFrameworkAction<int, long> ResourceVerifyStart;
            public GameFrameworkAction<ResourceName, int> ResourceVerifySuccess;
            public GameFrameworkAction<ResourceName> ResourceVerifyFailure;
            public GameFrameworkAction<bool> ResourceVerifyComplete;

            /// <summary>
            /// 初始化资源校验器的新实例。
            /// </summary>
            /// <param name="resourceManager">资源管理器。</param>
            public ResourceVerifier(ResourceManager resourceManager)
            {
                m_ResourceManager = resourceManager;
                m_VerifyInfos = new List<VerifyInfo>();
                m_CachedHashBytes = new byte[CachedHashBytesLength];
                m_LoadReadWriteVersionListComplete = false;
                m_VerifyResourceLengthPerFrame = 0;
                m_VerifyResourceIndex = 0;
                m_FailureFlag = false;

                ResourceVerifyStart = null;
                ResourceVerifySuccess = null;
                ResourceVerifyFailure = null;
                ResourceVerifyComplete = null;
            }

            /// <summary>
            /// 资源校验器轮询。
            /// </summary>
            /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
            /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
            public void Update(float elapseSeconds, float realElapseSeconds)
            {
                if (!m_LoadReadWriteVersionListComplete)
                {
                    return;
                }

                int length = 0;
                while (m_VerifyResourceIndex < m_VerifyInfos.Count)
                {
                    VerifyInfo verifyInfo = m_VerifyInfos[m_VerifyResourceIndex];
                    length += verifyInfo.Length;
                    if (VerifyResource(verifyInfo))
                    {
                        m_VerifyResourceIndex++;
                        if (ResourceVerifySuccess != null)
                        {
                            ResourceVerifySuccess(verifyInfo.ResourceName, verifyInfo.Length);
                        }
                    }
                    else
                    {
                        m_FailureFlag = true;
                        m_VerifyInfos.RemoveAt(m_VerifyResourceIndex);
                        if (ResourceVerifyFailure != null)
                        {
                            ResourceVerifyFailure(verifyInfo.ResourceName);
                        }
                    }

                    if (length >= m_VerifyResourceLengthPerFrame)
                    {
                        return;
                    }
                }

                m_LoadReadWriteVersionListComplete = false;
                if (m_FailureFlag)
                {
                    GenerateReadWriteVersionList();
                }

                if (ResourceVerifyComplete != null)
                {
                    ResourceVerifyComplete(!m_FailureFlag);
                }
            }

            /// <summary>
            /// 关闭并清理资源校验器。
            /// </summary>
            public void Shutdown()
            {
                m_VerifyInfos.Clear();
                m_LoadReadWriteVersionListComplete = false;
                m_VerifyResourceLengthPerFrame = 0;
                m_VerifyResourceIndex = 0;
                m_FailureFlag = false;
            }

            /// <summary>
            /// 校验资源。
            /// </summary>
            /// <param name="verifyResourceLengthPerFrame">每帧至少校验资源的大小，以字节为单位。</param>
            public void VerifyResources(int verifyResourceLengthPerFrame)
            {
                if (verifyResourceLengthPerFrame < 0)
                {
                    throw new GameFrameworkException("Verify resource count per frame is invalid.");
                }

                if (m_ResourceManager.m_ResourceHelper == null)
                {
                    throw new GameFrameworkException("Resource helper is invalid.");
                }

                if (string.IsNullOrEmpty(m_ResourceManager.m_ReadWritePath))
                {
                    throw new GameFrameworkException("Read-write path is invalid.");
                }

                m_VerifyResourceLengthPerFrame = verifyResourceLengthPerFrame;
                m_ResourceManager.m_ResourceHelper.LoadBytes(Utility.Path.GetRemotePath(Path.Combine(m_ResourceManager.m_ReadWritePath, LocalVersionListFileName)), new LoadBytesCallbacks(OnLoadReadWriteVersionListSuccess, OnLoadReadWriteVersionListFailure), null);
            }

            private bool VerifyResource(VerifyInfo verifyInfo)
            {
                if (verifyInfo.UseFileSystem)
                {
                    IFileSystem fileSystem = m_ResourceManager.GetFileSystem(verifyInfo.FileSystemName, false);
                    string fileName = verifyInfo.ResourceName.FullName;
                    FileSystem.FileInfo fileInfo = fileSystem.GetFileInfo(fileName);
                    if (!fileInfo.IsValid)
                    {
                        return false;
                    }

                    int length = fileInfo.Length;
                    if (length == verifyInfo.Length)
                    {
                        m_ResourceManager.PrepareCachedStream();
                        fileSystem.ReadFile(fileName, m_ResourceManager.m_CachedStream);
                        m_ResourceManager.m_CachedStream.Position = 0L;
                        int hashCode = 0;
                        if (verifyInfo.LoadType == LoadType.LoadFromMemoryAndQuickDecrypt || verifyInfo.LoadType == LoadType.LoadFromMemoryAndDecrypt
                            || verifyInfo.LoadType == LoadType.LoadFromBinaryAndQuickDecrypt || verifyInfo.LoadType == LoadType.LoadFromBinaryAndDecrypt)
                        {
                            Utility.Converter.GetBytes(verifyInfo.HashCode, m_CachedHashBytes);
                            if (verifyInfo.LoadType == LoadType.LoadFromMemoryAndQuickDecrypt || verifyInfo.LoadType == LoadType.LoadFromBinaryAndQuickDecrypt)
                            {
                                hashCode = Utility.Verifier.GetCrc32(m_ResourceManager.m_CachedStream, m_CachedHashBytes, Utility.Encryption.QuickEncryptLength);
                            }
                            else if (verifyInfo.LoadType == LoadType.LoadFromMemoryAndDecrypt || verifyInfo.LoadType == LoadType.LoadFromBinaryAndDecrypt)
                            {
                                hashCode = Utility.Verifier.GetCrc32(m_ResourceManager.m_CachedStream, m_CachedHashBytes, length);
                            }

                            Array.Clear(m_CachedHashBytes, 0, CachedHashBytesLength);
                        }
                        else
                        {
                            hashCode = Utility.Verifier.GetCrc32(m_ResourceManager.m_CachedStream);
                        }

                        if (hashCode == verifyInfo.HashCode)
                        {
                            return true;
                        }
                    }

                    fileSystem.DeleteFile(fileName);
                    return false;
                }
                else
                {
                    string resourcePath = Utility.Path.GetRegularPath(Path.Combine(m_ResourceManager.ReadWritePath, verifyInfo.ResourceName.FullName));
                    if (!File.Exists(resourcePath))
                    {
                        return false;
                    }

                    using (FileStream fileStream = new FileStream(resourcePath, FileMode.Open, FileAccess.Read))
                    {
                        int length = (int)fileStream.Length;
                        if (length == verifyInfo.Length)
                        {
                            int hashCode = 0;
                            if (verifyInfo.LoadType == LoadType.LoadFromMemoryAndQuickDecrypt || verifyInfo.LoadType == LoadType.LoadFromMemoryAndDecrypt
                                || verifyInfo.LoadType == LoadType.LoadFromBinaryAndQuickDecrypt || verifyInfo.LoadType == LoadType.LoadFromBinaryAndDecrypt)
                            {
                                Utility.Converter.GetBytes(verifyInfo.HashCode, m_CachedHashBytes);
                                if (verifyInfo.LoadType == LoadType.LoadFromMemoryAndQuickDecrypt || verifyInfo.LoadType == LoadType.LoadFromBinaryAndQuickDecrypt)
                                {
                                    hashCode = Utility.Verifier.GetCrc32(fileStream, m_CachedHashBytes, Utility.Encryption.QuickEncryptLength);
                                }
                                else if (verifyInfo.LoadType == LoadType.LoadFromMemoryAndDecrypt || verifyInfo.LoadType == LoadType.LoadFromBinaryAndDecrypt)
                                {
                                    hashCode = Utility.Verifier.GetCrc32(fileStream, m_CachedHashBytes, length);
                                }

                                Array.Clear(m_CachedHashBytes, 0, CachedHashBytesLength);
                            }
                            else
                            {
                                hashCode = Utility.Verifier.GetCrc32(fileStream);
                            }

                            if (hashCode == verifyInfo.HashCode)
                            {
                                return true;
                            }
                        }
                    }

                    File.Delete(resourcePath);
                    return false;
                }
            }

            private void GenerateReadWriteVersionList()
            {
                string readWriteVersionListFileName = Utility.Path.GetRegularPath(Path.Combine(m_ResourceManager.m_ReadWritePath, LocalVersionListFileName));
                string readWriteVersionListTempFileName = Utility.Text.Format("{0}.{1}", readWriteVersionListFileName, TempExtension);
                SortedDictionary<string, List<int>> cachedFileSystemsForGenerateReadWriteVersionList = new SortedDictionary<string, List<int>>(StringComparer.Ordinal);
                FileStream fileStream = null;
                try
                {
                    fileStream = new FileStream(readWriteVersionListTempFileName, FileMode.Create, FileAccess.Write);
                    LocalVersionList.Resource[] resources = m_VerifyInfos.Count > 0 ? new LocalVersionList.Resource[m_VerifyInfos.Count] : null;
                    if (resources != null)
                    {
                        int index = 0;
                        foreach (VerifyInfo i in m_VerifyInfos)
                        {
                            resources[index] = new LocalVersionList.Resource(i.ResourceName.Name, i.ResourceName.Variant, i.ResourceName.Extension, (byte)i.LoadType, i.Length, i.HashCode);
                            if (i.UseFileSystem)
                            {
                                List<int> resourceIndexes = null;
                                if (!cachedFileSystemsForGenerateReadWriteVersionList.TryGetValue(i.FileSystemName, out resourceIndexes))
                                {
                                    resourceIndexes = new List<int>();
                                    cachedFileSystemsForGenerateReadWriteVersionList.Add(i.FileSystemName, resourceIndexes);
                                }

                                resourceIndexes.Add(index);
                            }

                            index++;
                        }
                    }

                    LocalVersionList.FileSystem[] fileSystems = cachedFileSystemsForGenerateReadWriteVersionList.Count > 0 ? new LocalVersionList.FileSystem[cachedFileSystemsForGenerateReadWriteVersionList.Count] : null;
                    if (fileSystems != null)
                    {
                        int index = 0;
                        foreach (KeyValuePair<string, List<int>> i in cachedFileSystemsForGenerateReadWriteVersionList)
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

                    if (File.Exists(readWriteVersionListTempFileName))
                    {
                        File.Delete(readWriteVersionListTempFileName);
                    }

                    throw new GameFrameworkException(Utility.Text.Format("Generate read-write version list exception '{0}'.", exception), exception);
                }

                if (File.Exists(readWriteVersionListFileName))
                {
                    File.Delete(readWriteVersionListFileName);
                }

                File.Move(readWriteVersionListTempFileName, readWriteVersionListFileName);
            }

            private void OnLoadReadWriteVersionListSuccess(string fileUri, byte[] bytes, float duration, object userData)
            {
                MemoryStream memoryStream = null;
                try
                {
                    memoryStream = new MemoryStream(bytes, false);
                    LocalVersionList versionList = m_ResourceManager.m_ReadWriteVersionListSerializer.Deserialize(memoryStream);
                    if (!versionList.IsValid)
                    {
                        throw new GameFrameworkException("Deserialize read write version list failure.");
                    }

                    LocalVersionList.Resource[] resources = versionList.GetResources();
                    LocalVersionList.FileSystem[] fileSystems = versionList.GetFileSystems();
                    Dictionary<ResourceName, string> resourceInFileSystemNames = new Dictionary<ResourceName, string>();
                    foreach (LocalVersionList.FileSystem fileSystem in fileSystems)
                    {
                        int[] resourceIndexes = fileSystem.GetResourceIndexes();
                        foreach (int resourceIndex in resourceIndexes)
                        {
                            LocalVersionList.Resource resource = resources[resourceIndex];
                            resourceInFileSystemNames.Add(new ResourceName(resource.Name, resource.Variant, resource.Extension), fileSystem.Name);
                        }
                    }

                    long totalLength = 0L;
                    foreach (LocalVersionList.Resource resource in resources)
                    {
                        ResourceName resourceName = new ResourceName(resource.Name, resource.Variant, resource.Extension);
                        string fileSystemName = null;
                        resourceInFileSystemNames.TryGetValue(resourceName, out fileSystemName);
                        totalLength += resource.Length;
                        m_VerifyInfos.Add(new VerifyInfo(resourceName, fileSystemName, (LoadType)resource.LoadType, resource.Length, resource.HashCode));
                    }

                    m_LoadReadWriteVersionListComplete = true;
                    if (ResourceVerifyStart != null)
                    {
                        ResourceVerifyStart(m_VerifyInfos.Count, totalLength);
                    }
                }
                catch (Exception exception)
                {
                    if (exception is GameFrameworkException)
                    {
                        throw;
                    }

                    throw new GameFrameworkException(Utility.Text.Format("Parse read-write version list exception '{0}'.", exception), exception);
                }
                finally
                {
                    if (memoryStream != null)
                    {
                        memoryStream.Dispose();
                        memoryStream = null;
                    }
                }
            }

            private void OnLoadReadWriteVersionListFailure(string fileUri, string errorMessage, object userData)
            {
                if (ResourceVerifyComplete != null)
                {
                    ResourceVerifyComplete(true);
                }
            }
        }
    }
}
