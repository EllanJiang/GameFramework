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
        /// 资源检查器。
        /// </summary>
        private sealed partial class ResourceChecker
        {
            private readonly ResourceManager m_ResourceManager;
            private readonly Dictionary<ResourceName, CheckInfo> m_CheckInfos;
            private string m_CurrentVariant;
            private bool m_IgnoreOtherVariant;
            private bool m_UpdatableVersionListReady;
            private bool m_ReadOnlyVersionListReady;
            private bool m_ReadWriteVersionListReady;

            public GameFrameworkAction<ResourceName, string, LoadType, int, int, int, int> ResourceNeedUpdate;
            public GameFrameworkAction<int, int, int, long, long> ResourceCheckComplete;

            /// <summary>
            /// 初始化资源检查器的新实例。
            /// </summary>
            /// <param name="resourceManager">资源管理器。</param>
            public ResourceChecker(ResourceManager resourceManager)
            {
                m_ResourceManager = resourceManager;
                m_CheckInfos = new Dictionary<ResourceName, CheckInfo>();
                m_CurrentVariant = null;
                m_IgnoreOtherVariant = false;
                m_UpdatableVersionListReady = false;
                m_ReadOnlyVersionListReady = false;
                m_ReadWriteVersionListReady = false;

                ResourceNeedUpdate = null;
                ResourceCheckComplete = null;
            }

            /// <summary>
            /// 关闭并清理资源检查器。
            /// </summary>
            public void Shutdown()
            {
                m_CheckInfos.Clear();
            }

            /// <summary>
            /// 检查资源。
            /// </summary>
            /// <param name="currentVariant">当前使用的变体。</param>
            /// <param name="ignoreOtherVariant">是否忽略处理其它变体的资源，若不忽略，将会移除其它变体的资源。</param>
            public void CheckResources(string currentVariant, bool ignoreOtherVariant)
            {
                if (m_ResourceManager.m_ResourceHelper == null)
                {
                    throw new GameFrameworkException("Resource helper is invalid.");
                }

                if (string.IsNullOrEmpty(m_ResourceManager.m_ReadOnlyPath))
                {
                    throw new GameFrameworkException("Read-only path is invalid.");
                }

                if (string.IsNullOrEmpty(m_ResourceManager.m_ReadWritePath))
                {
                    throw new GameFrameworkException("Read-write path is invalid.");
                }

                m_CurrentVariant = currentVariant;
                m_IgnoreOtherVariant = ignoreOtherVariant;
                m_ResourceManager.m_ResourceHelper.LoadBytes(Utility.Path.GetRemotePath(Path.Combine(m_ResourceManager.m_ReadWritePath, RemoteVersionListFileName)), new LoadBytesCallbacks(OnLoadUpdatableVersionListSuccess, OnLoadUpdatableVersionListFailure), null);
                m_ResourceManager.m_ResourceHelper.LoadBytes(Utility.Path.GetRemotePath(Path.Combine(m_ResourceManager.m_ReadOnlyPath, LocalVersionListFileName)), new LoadBytesCallbacks(OnLoadReadOnlyVersionListSuccess, OnLoadReadOnlyVersionListFailure), null);
                m_ResourceManager.m_ResourceHelper.LoadBytes(Utility.Path.GetRemotePath(Path.Combine(m_ResourceManager.m_ReadWritePath, LocalVersionListFileName)), new LoadBytesCallbacks(OnLoadReadWriteVersionListSuccess, OnLoadReadWriteVersionListFailure), null);
            }

            private void SetCachedFileSystemName(ResourceName resourceName, string fileSystemName)
            {
                GetOrAddCheckInfo(resourceName).SetCachedFileSystemName(fileSystemName);
            }

            private void SetVersionInfo(ResourceName resourceName, LoadType loadType, int length, int hashCode, int compressedLength, int compressedHashCode)
            {
                GetOrAddCheckInfo(resourceName).SetVersionInfo(loadType, length, hashCode, compressedLength, compressedHashCode);
            }

            private void SetReadOnlyInfo(ResourceName resourceName, LoadType loadType, int length, int hashCode)
            {
                GetOrAddCheckInfo(resourceName).SetReadOnlyInfo(loadType, length, hashCode);
            }

            private void SetReadWriteInfo(ResourceName resourceName, LoadType loadType, int length, int hashCode)
            {
                GetOrAddCheckInfo(resourceName).SetReadWriteInfo(loadType, length, hashCode);
            }

            private CheckInfo GetOrAddCheckInfo(ResourceName resourceName)
            {
                CheckInfo checkInfo = null;
                if (m_CheckInfos.TryGetValue(resourceName, out checkInfo))
                {
                    return checkInfo;
                }

                checkInfo = new CheckInfo(resourceName);
                m_CheckInfos.Add(checkInfo.ResourceName, checkInfo);

                return checkInfo;
            }

            private void RefreshCheckInfoStatus()
            {
                if (!m_UpdatableVersionListReady || !m_ReadOnlyVersionListReady || !m_ReadWriteVersionListReady)
                {
                    return;
                }

                int movedCount = 0;
                int removedCount = 0;
                int updateCount = 0;
                long updateTotalLength = 0L;
                long updateTotalCompressedLength = 0L;
                foreach (KeyValuePair<ResourceName, CheckInfo> checkInfo in m_CheckInfos)
                {
                    CheckInfo ci = checkInfo.Value;
                    ci.RefreshStatus(m_CurrentVariant, m_IgnoreOtherVariant);
                    if (ci.Status == CheckInfo.CheckStatus.StorageInReadOnly)
                    {
                        m_ResourceManager.m_ResourceInfos.Add(ci.ResourceName, new ResourceInfo(ci.ResourceName, ci.FileSystemName, ci.LoadType, ci.Length, ci.HashCode, ci.CompressedLength, true, true));
                    }
                    else if (ci.Status == CheckInfo.CheckStatus.StorageInReadWrite)
                    {
                        if (ci.NeedMoveToDisk || ci.NeedMoveToFileSystem)
                        {
                            movedCount++;
                            string resourceFullName = ci.ResourceName.FullName;
                            string resourcePath = Utility.Path.GetRegularPath(Path.Combine(m_ResourceManager.m_ReadWritePath, resourceFullName));
                            if (ci.NeedMoveToDisk)
                            {
                                IFileSystem fileSystem = m_ResourceManager.GetFileSystem(ci.ReadWriteFileSystemName, false);
                                if (!fileSystem.SaveAsFile(resourceFullName, resourcePath))
                                {
                                    throw new GameFrameworkException(Utility.Text.Format("Save as file '{0}' to '{1}' from file system '{2}' error.", resourceFullName, resourcePath, fileSystem.FullPath));
                                }

                                fileSystem.DeleteFile(resourceFullName);
                            }

                            if (ci.NeedMoveToFileSystem)
                            {
                                IFileSystem fileSystem = m_ResourceManager.GetFileSystem(ci.FileSystemName, false);
                                if (!fileSystem.WriteFile(resourceFullName, resourcePath))
                                {
                                    throw new GameFrameworkException(Utility.Text.Format("Write resource '{0}' to file system '{1}' error.", resourceFullName, fileSystem.FullPath));
                                }

                                if (File.Exists(resourcePath))
                                {
                                    File.Delete(resourcePath);
                                }
                            }
                        }

                        m_ResourceManager.m_ResourceInfos.Add(ci.ResourceName, new ResourceInfo(ci.ResourceName, ci.FileSystemName, ci.LoadType, ci.Length, ci.HashCode, ci.CompressedLength, false, true));
                        m_ResourceManager.m_ReadWriteResourceInfos.Add(ci.ResourceName, new ReadWriteResourceInfo(ci.FileSystemName, ci.LoadType, ci.Length, ci.HashCode));
                    }
                    else if (ci.Status == CheckInfo.CheckStatus.Update)
                    {
                        m_ResourceManager.m_ResourceInfos.Add(ci.ResourceName, new ResourceInfo(ci.ResourceName, ci.FileSystemName, ci.LoadType, ci.Length, ci.HashCode, ci.CompressedLength, false, false));
                        updateCount++;
                        updateTotalLength += ci.Length;
                        updateTotalCompressedLength += ci.CompressedLength;
                        if (ResourceNeedUpdate != null)
                        {
                            ResourceNeedUpdate(ci.ResourceName, ci.FileSystemName, ci.LoadType, ci.Length, ci.HashCode, ci.CompressedLength, ci.CompressedHashCode);
                        }
                    }
                    else if (ci.Status == CheckInfo.CheckStatus.Unavailable || ci.Status == CheckInfo.CheckStatus.Disuse)
                    {
                        // Do nothing.
                    }
                    else
                    {
                        throw new GameFrameworkException(Utility.Text.Format("Check resources '{0}' error with unknown status.", ci.ResourceName.FullName));
                    }

                    if (ci.NeedRemove)
                    {
                        removedCount++;
                        if (ci.ReadWriteUseFileSystem)
                        {
                            IFileSystem fileSystem = m_ResourceManager.GetFileSystem(ci.ReadWriteFileSystemName, false);
                            fileSystem.DeleteFile(ci.ResourceName.FullName);
                        }
                        else
                        {
                            string resourcePath = Utility.Path.GetRegularPath(Path.Combine(m_ResourceManager.m_ReadWritePath, ci.ResourceName.FullName));
                            if (File.Exists(resourcePath))
                            {
                                File.Delete(resourcePath);
                            }
                        }
                    }
                }

                if (movedCount > 0 || removedCount > 0)
                {
                    RemoveEmptyFileSystems();
                    Utility.Path.RemoveEmptyDirectory(m_ResourceManager.m_ReadWritePath);
                }

                if (ResourceCheckComplete != null)
                {
                    ResourceCheckComplete(movedCount, removedCount, updateCount, updateTotalLength, updateTotalCompressedLength);
                }
            }

            private void RemoveEmptyFileSystems()
            {
                List<string> removedFileSystemNames = null;
                foreach (KeyValuePair<string, IFileSystem> fileSystem in m_ResourceManager.m_ReadWriteFileSystems)
                {
                    if (fileSystem.Value.FileCount <= 0)
                    {
                        if (removedFileSystemNames == null)
                        {
                            removedFileSystemNames = new List<string>();
                        }

                        m_ResourceManager.m_FileSystemManager.DestroyFileSystem(fileSystem.Value, true);
                        removedFileSystemNames.Add(fileSystem.Key);
                    }
                }

                if (removedFileSystemNames != null)
                {
                    foreach (string removedFileSystemName in removedFileSystemNames)
                    {
                        m_ResourceManager.m_ReadWriteFileSystems.Remove(removedFileSystemName);
                    }
                }
            }

            private void OnLoadUpdatableVersionListSuccess(string fileUri, byte[] bytes, float duration, object userData)
            {
                if (m_UpdatableVersionListReady)
                {
                    throw new GameFrameworkException("Updatable version list has been parsed.");
                }

                MemoryStream memoryStream = null;
                try
                {
                    memoryStream = new MemoryStream(bytes, false);
                    UpdatableVersionList versionList = m_ResourceManager.m_UpdatableVersionListSerializer.Deserialize(memoryStream);
                    if (!versionList.IsValid)
                    {
                        throw new GameFrameworkException("Deserialize updatable version list failure.");
                    }

                    UpdatableVersionList.Asset[] assets = versionList.GetAssets();
                    UpdatableVersionList.Resource[] resources = versionList.GetResources();
                    UpdatableVersionList.FileSystem[] fileSystems = versionList.GetFileSystems();
                    UpdatableVersionList.ResourceGroup[] resourceGroups = versionList.GetResourceGroups();
                    m_ResourceManager.m_ApplicableGameVersion = versionList.ApplicableGameVersion;
                    m_ResourceManager.m_InternalResourceVersion = versionList.InternalResourceVersion;
                    m_ResourceManager.m_AssetInfos = new Dictionary<string, AssetInfo>(assets.Length, StringComparer.Ordinal);
                    m_ResourceManager.m_ResourceInfos = new Dictionary<ResourceName, ResourceInfo>(resources.Length, new ResourceNameComparer());
                    m_ResourceManager.m_ReadWriteResourceInfos = new SortedDictionary<ResourceName, ReadWriteResourceInfo>(new ResourceNameComparer());
                    ResourceGroup defaultResourceGroup = m_ResourceManager.GetOrAddResourceGroup(string.Empty);

                    foreach (UpdatableVersionList.FileSystem fileSystem in fileSystems)
                    {
                        int[] resourceIndexes = fileSystem.GetResourceIndexes();
                        foreach (int resourceIndex in resourceIndexes)
                        {
                            UpdatableVersionList.Resource resource = resources[resourceIndex];
                            if (resource.Variant != null && resource.Variant != m_CurrentVariant)
                            {
                                continue;
                            }

                            SetCachedFileSystemName(new ResourceName(resource.Name, resource.Variant, resource.Extension), fileSystem.Name);
                        }
                    }

                    foreach (UpdatableVersionList.Resource resource in resources)
                    {
                        if (resource.Variant != null && resource.Variant != m_CurrentVariant)
                        {
                            continue;
                        }

                        ResourceName resourceName = new ResourceName(resource.Name, resource.Variant, resource.Extension);
                        int[] assetIndexes = resource.GetAssetIndexes();
                        foreach (int assetIndex in assetIndexes)
                        {
                            UpdatableVersionList.Asset asset = assets[assetIndex];
                            int[] dependencyAssetIndexes = asset.GetDependencyAssetIndexes();
                            int index = 0;
                            string[] dependencyAssetNames = new string[dependencyAssetIndexes.Length];
                            foreach (int dependencyAssetIndex in dependencyAssetIndexes)
                            {
                                dependencyAssetNames[index++] = assets[dependencyAssetIndex].Name;
                            }

                            m_ResourceManager.m_AssetInfos.Add(asset.Name, new AssetInfo(asset.Name, resourceName, dependencyAssetNames));
                        }

                        SetVersionInfo(resourceName, (LoadType)resource.LoadType, resource.Length, resource.HashCode, resource.CompressedLength, resource.CompressedHashCode);
                        defaultResourceGroup.AddResource(resourceName, resource.Length, resource.CompressedLength);
                    }

                    foreach (UpdatableVersionList.ResourceGroup resourceGroup in resourceGroups)
                    {
                        ResourceGroup group = m_ResourceManager.GetOrAddResourceGroup(resourceGroup.Name);
                        int[] resourceIndexes = resourceGroup.GetResourceIndexes();
                        foreach (int resourceIndex in resourceIndexes)
                        {
                            UpdatableVersionList.Resource resource = resources[resourceIndex];
                            if (resource.Variant != null && resource.Variant != m_CurrentVariant)
                            {
                                continue;
                            }

                            group.AddResource(new ResourceName(resource.Name, resource.Variant, resource.Extension), resource.Length, resource.CompressedLength);
                        }
                    }

                    m_UpdatableVersionListReady = true;
                    RefreshCheckInfoStatus();
                }
                catch (Exception exception)
                {
                    if (exception is GameFrameworkException)
                    {
                        throw;
                    }

                    throw new GameFrameworkException(Utility.Text.Format("Parse updatable version list exception '{0}'.", exception), exception);
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

            private void OnLoadUpdatableVersionListFailure(string fileUri, string errorMessage, object userData)
            {
                throw new GameFrameworkException(Utility.Text.Format("Updatable version list '{0}' is invalid, error message is '{1}'.", fileUri, string.IsNullOrEmpty(errorMessage) ? "<Empty>" : errorMessage));
            }

            private void OnLoadReadOnlyVersionListSuccess(string fileUri, byte[] bytes, float duration, object userData)
            {
                if (m_ReadOnlyVersionListReady)
                {
                    throw new GameFrameworkException("Read-only version list has been parsed.");
                }

                MemoryStream memoryStream = null;
                try
                {
                    memoryStream = new MemoryStream(bytes, false);
                    LocalVersionList versionList = m_ResourceManager.m_ReadOnlyVersionListSerializer.Deserialize(memoryStream);
                    if (!versionList.IsValid)
                    {
                        throw new GameFrameworkException("Deserialize read-only version list failure.");
                    }

                    LocalVersionList.Resource[] resources = versionList.GetResources();
                    LocalVersionList.FileSystem[] fileSystems = versionList.GetFileSystems();

                    foreach (LocalVersionList.FileSystem fileSystem in fileSystems)
                    {
                        int[] resourceIndexes = fileSystem.GetResourceIndexes();
                        foreach (int resourceIndex in resourceIndexes)
                        {
                            LocalVersionList.Resource resource = resources[resourceIndex];
                            SetCachedFileSystemName(new ResourceName(resource.Name, resource.Variant, resource.Extension), fileSystem.Name);
                        }
                    }

                    foreach (LocalVersionList.Resource resource in resources)
                    {
                        SetReadOnlyInfo(new ResourceName(resource.Name, resource.Variant, resource.Extension), (LoadType)resource.LoadType, resource.Length, resource.HashCode);
                    }

                    m_ReadOnlyVersionListReady = true;
                    RefreshCheckInfoStatus();
                }
                catch (Exception exception)
                {
                    if (exception is GameFrameworkException)
                    {
                        throw;
                    }

                    throw new GameFrameworkException(Utility.Text.Format("Parse read-only version list exception '{0}'.", exception), exception);
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

            private void OnLoadReadOnlyVersionListFailure(string fileUri, string errorMessage, object userData)
            {
                if (m_ReadOnlyVersionListReady)
                {
                    throw new GameFrameworkException("Read-only version list has been parsed.");
                }

                m_ReadOnlyVersionListReady = true;
                RefreshCheckInfoStatus();
            }

            private void OnLoadReadWriteVersionListSuccess(string fileUri, byte[] bytes, float duration, object userData)
            {
                if (m_ReadWriteVersionListReady)
                {
                    throw new GameFrameworkException("Read-write version list has been parsed.");
                }

                MemoryStream memoryStream = null;
                try
                {
                    memoryStream = new MemoryStream(bytes, false);
                    LocalVersionList versionList = m_ResourceManager.m_ReadWriteVersionListSerializer.Deserialize(memoryStream);
                    if (!versionList.IsValid)
                    {
                        throw new GameFrameworkException("Deserialize read-write version list failure.");
                    }

                    LocalVersionList.Resource[] resources = versionList.GetResources();
                    LocalVersionList.FileSystem[] fileSystems = versionList.GetFileSystems();

                    foreach (LocalVersionList.FileSystem fileSystem in fileSystems)
                    {
                        int[] resourceIndexes = fileSystem.GetResourceIndexes();
                        foreach (int resourceIndex in resourceIndexes)
                        {
                            LocalVersionList.Resource resource = resources[resourceIndex];
                            SetCachedFileSystemName(new ResourceName(resource.Name, resource.Variant, resource.Extension), fileSystem.Name);
                        }
                    }

                    foreach (LocalVersionList.Resource resource in resources)
                    {
                        SetReadWriteInfo(new ResourceName(resource.Name, resource.Variant, resource.Extension), (LoadType)resource.LoadType, resource.Length, resource.HashCode);
                    }

                    m_ReadWriteVersionListReady = true;
                    RefreshCheckInfoStatus();
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
                if (m_ReadWriteVersionListReady)
                {
                    throw new GameFrameworkException("Read-write version list has been parsed.");
                }

                m_ReadWriteVersionListReady = true;
                RefreshCheckInfoStatus();
            }
        }
    }
}
