//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework.FileSystem;
using GameFramework.ObjectPool;
using System;
using System.Collections.Generic;
using System.IO;

namespace GameFramework.Resource
{
    internal sealed partial class ResourceManager : GameFrameworkModule, IResourceManager
    {
        /// <summary>
        /// 加载资源器。
        /// </summary>
        private sealed partial class ResourceLoader
        {
            private const int CachedHashBytesLength = 4;

            private readonly ResourceManager m_ResourceManager;
            private readonly TaskPool<LoadResourceTaskBase> m_TaskPool;
            private readonly Dictionary<object, int> m_AssetDependencyCount;
            private readonly Dictionary<object, int> m_ResourceDependencyCount;
            private readonly Dictionary<object, object> m_AssetToResourceMap;
            private readonly Dictionary<string, object> m_SceneToAssetMap;
            private readonly LoadBytesCallbacks m_LoadBytesCallbacks;
            private readonly byte[] m_CachedHashBytes;
            private IObjectPool<AssetObject> m_AssetPool;
            private IObjectPool<ResourceObject> m_ResourcePool;

            /// <summary>
            /// 初始化加载资源器的新实例。
            /// </summary>
            /// <param name="resourceManager">资源管理器。</param>
            public ResourceLoader(ResourceManager resourceManager)
            {
                m_ResourceManager = resourceManager;
                m_TaskPool = new TaskPool<LoadResourceTaskBase>();
                m_AssetDependencyCount = new Dictionary<object, int>();
                m_ResourceDependencyCount = new Dictionary<object, int>();
                m_AssetToResourceMap = new Dictionary<object, object>();
                m_SceneToAssetMap = new Dictionary<string, object>(StringComparer.Ordinal);
                m_LoadBytesCallbacks = new LoadBytesCallbacks(OnLoadBinarySuccess, OnLoadBinaryFailure);
                m_CachedHashBytes = new byte[CachedHashBytesLength];
                m_AssetPool = null;
                m_ResourcePool = null;
            }

            /// <summary>
            /// 获取加载资源代理总数量。
            /// </summary>
            public int TotalAgentCount
            {
                get
                {
                    return m_TaskPool.TotalAgentCount;
                }
            }

            /// <summary>
            /// 获取可用加载资源代理数量。
            /// </summary>
            public int FreeAgentCount
            {
                get
                {
                    return m_TaskPool.FreeAgentCount;
                }
            }

            /// <summary>
            /// 获取工作中加载资源代理数量。
            /// </summary>
            public int WorkingAgentCount
            {
                get
                {
                    return m_TaskPool.WorkingAgentCount;
                }
            }

            /// <summary>
            /// 获取等待加载资源任务数量。
            /// </summary>
            public int WaitingTaskCount
            {
                get
                {
                    return m_TaskPool.WaitingTaskCount;
                }
            }

            /// <summary>
            /// 获取或设置资源对象池自动释放可释放对象的间隔秒数。
            /// </summary>
            public float AssetAutoReleaseInterval
            {
                get
                {
                    return m_AssetPool.AutoReleaseInterval;
                }
                set
                {
                    m_AssetPool.AutoReleaseInterval = value;
                }
            }

            /// <summary>
            /// 获取或设置资源对象池的容量。
            /// </summary>
            public int AssetCapacity
            {
                get
                {
                    return m_AssetPool.Capacity;
                }
                set
                {
                    m_AssetPool.Capacity = value;
                }
            }

            /// <summary>
            /// 获取或设置资源对象池对象过期秒数。
            /// </summary>
            public float AssetExpireTime
            {
                get
                {
                    return m_AssetPool.ExpireTime;
                }
                set
                {
                    m_AssetPool.ExpireTime = value;
                }
            }

            /// <summary>
            /// 获取或设置资源对象池的优先级。
            /// </summary>
            public int AssetPriority
            {
                get
                {
                    return m_AssetPool.Priority;
                }
                set
                {
                    m_AssetPool.Priority = value;
                }
            }

            /// <summary>
            /// 获取或设置资源对象池自动释放可释放对象的间隔秒数。
            /// </summary>
            public float ResourceAutoReleaseInterval
            {
                get
                {
                    return m_ResourcePool.AutoReleaseInterval;
                }
                set
                {
                    m_ResourcePool.AutoReleaseInterval = value;
                }
            }

            /// <summary>
            /// 获取或设置资源对象池的容量。
            /// </summary>
            public int ResourceCapacity
            {
                get
                {
                    return m_ResourcePool.Capacity;
                }
                set
                {
                    m_ResourcePool.Capacity = value;
                }
            }

            /// <summary>
            /// 获取或设置资源对象池对象过期秒数。
            /// </summary>
            public float ResourceExpireTime
            {
                get
                {
                    return m_ResourcePool.ExpireTime;
                }
                set
                {
                    m_ResourcePool.ExpireTime = value;
                }
            }

            /// <summary>
            /// 获取或设置资源对象池的优先级。
            /// </summary>
            public int ResourcePriority
            {
                get
                {
                    return m_ResourcePool.Priority;
                }
                set
                {
                    m_ResourcePool.Priority = value;
                }
            }

            /// <summary>
            /// 加载资源器轮询。
            /// </summary>
            /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
            /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
            public void Update(float elapseSeconds, float realElapseSeconds)
            {
                m_TaskPool.Update(elapseSeconds, realElapseSeconds);
            }

            /// <summary>
            /// 关闭并清理加载资源器。
            /// </summary>
            public void Shutdown()
            {
                m_TaskPool.Shutdown();
                m_AssetDependencyCount.Clear();
                m_ResourceDependencyCount.Clear();
                m_AssetToResourceMap.Clear();
                m_SceneToAssetMap.Clear();
                LoadResourceAgent.Clear();
            }

            /// <summary>
            /// 设置对象池管理器。
            /// </summary>
            /// <param name="objectPoolManager">对象池管理器。</param>
            public void SetObjectPoolManager(IObjectPoolManager objectPoolManager)
            {
                m_AssetPool = objectPoolManager.CreateMultiSpawnObjectPool<AssetObject>("Asset Pool");
                m_ResourcePool = objectPoolManager.CreateMultiSpawnObjectPool<ResourceObject>("Resource Pool");
            }

            /// <summary>
            /// 增加加载资源代理辅助器。
            /// </summary>
            /// <param name="loadResourceAgentHelper">要增加的加载资源代理辅助器。</param>
            /// <param name="resourceHelper">资源辅助器。</param>
            /// <param name="readOnlyPath">资源只读区路径。</param>
            /// <param name="readWritePath">资源读写区路径。</param>
            /// <param name="decryptResourceCallback">要设置的解密资源回调函数。</param>
            public void AddLoadResourceAgentHelper(ILoadResourceAgentHelper loadResourceAgentHelper, IResourceHelper resourceHelper, string readOnlyPath, string readWritePath, DecryptResourceCallback decryptResourceCallback)
            {
                if (m_AssetPool == null || m_ResourcePool == null)
                {
                    throw new GameFrameworkException("You must set object pool manager first.");
                }

                LoadResourceAgent agent = new LoadResourceAgent(loadResourceAgentHelper, resourceHelper, this, readOnlyPath, readWritePath, decryptResourceCallback ?? DefaultDecryptResourceCallback);
                m_TaskPool.AddAgent(agent);
            }

            /// <summary>
            /// 检查资源是否存在。
            /// </summary>
            /// <param name="assetName">要检查资源的名称。</param>
            /// <returns>检查资源是否存在的结果。</returns>
            public HasAssetResult HasAsset(string assetName)
            {
                ResourceInfo resourceInfo = GetResourceInfo(assetName);
                if (resourceInfo == null)
                {
                    return HasAssetResult.NotExist;
                }

                if (!resourceInfo.Ready && m_ResourceManager.m_ResourceMode != ResourceMode.UpdatableWhilePlaying)
                {
                    return HasAssetResult.NotReady;
                }

                if (resourceInfo.UseFileSystem)
                {
                    return resourceInfo.IsLoadFromBinary ? HasAssetResult.BinaryOnFileSystem : HasAssetResult.AssetOnFileSystem;
                }
                else
                {
                    return resourceInfo.IsLoadFromBinary ? HasAssetResult.BinaryOnDisk : HasAssetResult.AssetOnDisk;
                }
            }

            /// <summary>
            /// 异步加载资源。
            /// </summary>
            /// <param name="assetName">要加载资源的名称。</param>
            /// <param name="assetType">要加载资源的类型。</param>
            /// <param name="priority">加载资源的优先级。</param>
            /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
            /// <param name="userData">用户自定义数据。</param>
            public void LoadAsset(string assetName, Type assetType, int priority, LoadAssetCallbacks loadAssetCallbacks, object userData)
            {
                ResourceInfo resourceInfo = null;
                string[] dependencyAssetNames = null;
                if (!CheckAsset(assetName, out resourceInfo, out dependencyAssetNames))
                {
                    string errorMessage = Utility.Text.Format("Can not load asset '{0}'.", assetName);
                    if (loadAssetCallbacks.LoadAssetFailureCallback != null)
                    {
                        loadAssetCallbacks.LoadAssetFailureCallback(assetName, resourceInfo != null && !resourceInfo.Ready ? LoadResourceStatus.NotReady : LoadResourceStatus.NotExist, errorMessage, userData);
                        return;
                    }

                    throw new GameFrameworkException(errorMessage);
                }

                if (resourceInfo.IsLoadFromBinary)
                {
                    string errorMessage = Utility.Text.Format("Can not load asset '{0}' which is a binary asset.", assetName);
                    if (loadAssetCallbacks.LoadAssetFailureCallback != null)
                    {
                        loadAssetCallbacks.LoadAssetFailureCallback(assetName, LoadResourceStatus.TypeError, errorMessage, userData);
                        return;
                    }

                    throw new GameFrameworkException(errorMessage);
                }

                LoadAssetTask mainTask = LoadAssetTask.Create(assetName, assetType, priority, resourceInfo, dependencyAssetNames, loadAssetCallbacks, userData);
                foreach (string dependencyAssetName in dependencyAssetNames)
                {
                    if (!LoadDependencyAsset(dependencyAssetName, priority, mainTask, userData))
                    {
                        string errorMessage = Utility.Text.Format("Can not load dependency asset '{0}' when load asset '{1}'.", dependencyAssetName, assetName);
                        if (loadAssetCallbacks.LoadAssetFailureCallback != null)
                        {
                            loadAssetCallbacks.LoadAssetFailureCallback(assetName, LoadResourceStatus.DependencyError, errorMessage, userData);
                            return;
                        }

                        throw new GameFrameworkException(errorMessage);
                    }
                }

                m_TaskPool.AddTask(mainTask);
                if (!resourceInfo.Ready)
                {
                    m_ResourceManager.UpdateResource(resourceInfo.ResourceName);
                }
            }

            /// <summary>
            /// 卸载资源。
            /// </summary>
            /// <param name="asset">要卸载的资源。</param>
            public void UnloadAsset(object asset)
            {
                m_AssetPool.Unspawn(asset);
            }

            /// <summary>
            /// 异步加载场景。
            /// </summary>
            /// <param name="sceneAssetName">要加载场景资源的名称。</param>
            /// <param name="priority">加载场景资源的优先级。</param>
            /// <param name="loadSceneCallbacks">加载场景回调函数集。</param>
            /// <param name="userData">用户自定义数据。</param>
            public void LoadScene(string sceneAssetName, int priority, LoadSceneCallbacks loadSceneCallbacks, object userData)
            {
                ResourceInfo resourceInfo = null;
                string[] dependencyAssetNames = null;
                if (!CheckAsset(sceneAssetName, out resourceInfo, out dependencyAssetNames))
                {
                    string errorMessage = Utility.Text.Format("Can not load scene '{0}'.", sceneAssetName);
                    if (loadSceneCallbacks.LoadSceneFailureCallback != null)
                    {
                        loadSceneCallbacks.LoadSceneFailureCallback(sceneAssetName, resourceInfo != null && !resourceInfo.Ready ? LoadResourceStatus.NotReady : LoadResourceStatus.NotExist, errorMessage, userData);
                        return;
                    }

                    throw new GameFrameworkException(errorMessage);
                }

                if (resourceInfo.IsLoadFromBinary)
                {
                    string errorMessage = Utility.Text.Format("Can not load scene asset '{0}' which is a binary asset.", sceneAssetName);
                    if (loadSceneCallbacks.LoadSceneFailureCallback != null)
                    {
                        loadSceneCallbacks.LoadSceneFailureCallback(sceneAssetName, LoadResourceStatus.TypeError, errorMessage, userData);
                        return;
                    }

                    throw new GameFrameworkException(errorMessage);
                }

                LoadSceneTask mainTask = LoadSceneTask.Create(sceneAssetName, priority, resourceInfo, dependencyAssetNames, loadSceneCallbacks, userData);
                foreach (string dependencyAssetName in dependencyAssetNames)
                {
                    if (!LoadDependencyAsset(dependencyAssetName, priority, mainTask, userData))
                    {
                        string errorMessage = Utility.Text.Format("Can not load dependency asset '{0}' when load scene '{1}'.", dependencyAssetName, sceneAssetName);
                        if (loadSceneCallbacks.LoadSceneFailureCallback != null)
                        {
                            loadSceneCallbacks.LoadSceneFailureCallback(sceneAssetName, LoadResourceStatus.DependencyError, errorMessage, userData);
                            return;
                        }

                        throw new GameFrameworkException(errorMessage);
                    }
                }

                m_TaskPool.AddTask(mainTask);
                if (!resourceInfo.Ready)
                {
                    m_ResourceManager.UpdateResource(resourceInfo.ResourceName);
                }
            }

            /// <summary>
            /// 异步卸载场景。
            /// </summary>
            /// <param name="sceneAssetName">要卸载场景资源的名称。</param>
            /// <param name="unloadSceneCallbacks">卸载场景回调函数集。</param>
            /// <param name="userData">用户自定义数据。</param>
            public void UnloadScene(string sceneAssetName, UnloadSceneCallbacks unloadSceneCallbacks, object userData)
            {
                if (m_ResourceManager.m_ResourceHelper == null)
                {
                    throw new GameFrameworkException("You must set resource helper first.");
                }

                object asset = null;
                if (m_SceneToAssetMap.TryGetValue(sceneAssetName, out asset))
                {
                    m_SceneToAssetMap.Remove(sceneAssetName);
                    m_AssetPool.Unspawn(asset);
                    m_AssetPool.ReleaseObject(asset);
                }
                else
                {
                    throw new GameFrameworkException(Utility.Text.Format("Can not find asset of scene '{0}'.", sceneAssetName));
                }

                m_ResourceManager.m_ResourceHelper.UnloadScene(sceneAssetName, unloadSceneCallbacks, userData);
            }

            /// <summary>
            /// 获取二进制资源的实际路径。
            /// </summary>
            /// <param name="binaryAssetName">要获取实际路径的二进制资源的名称。</param>
            /// <returns>二进制资源的实际路径。</returns>
            /// <remarks>此方法仅适用于二进制资源存储在磁盘（而非文件系统）中的情况。若二进制资源存储在文件系统中时，返回值将始终为空。</remarks>
            public string GetBinaryPath(string binaryAssetName)
            {
                ResourceInfo resourceInfo = GetResourceInfo(binaryAssetName);
                if (resourceInfo == null)
                {
                    return null;
                }

                if (!resourceInfo.Ready)
                {
                    return null;
                }

                if (!resourceInfo.IsLoadFromBinary)
                {
                    return null;
                }

                if (resourceInfo.UseFileSystem)
                {
                    return null;
                }

                return Utility.Path.GetRegularPath(Path.Combine(resourceInfo.StorageInReadOnly ? m_ResourceManager.m_ReadOnlyPath : m_ResourceManager.m_ReadWritePath, resourceInfo.ResourceName.FullName));
            }

            /// <summary>
            /// 获取二进制资源的实际路径。
            /// </summary>
            /// <param name="binaryAssetName">要获取实际路径的二进制资源的名称。</param>
            /// <param name="storageInReadOnly">二进制资源是否存储在只读区中。</param>
            /// <param name="storageInFileSystem">二进制资源是否存储在文件系统中。</param>
            /// <param name="relativePath">二进制资源或存储二进制资源的文件系统，相对于只读区或者读写区的相对路径。</param>
            /// <param name="fileName">若二进制资源存储在文件系统中，则指示二进制资源在文件系统中的名称，否则此参数返回空。</param>
            /// <returns>是否获取二进制资源的实际路径成功。</returns>
            public bool GetBinaryPath(string binaryAssetName, out bool storageInReadOnly, out bool storageInFileSystem, out string relativePath, out string fileName)
            {
                storageInReadOnly = false;
                storageInFileSystem = false;
                relativePath = null;
                fileName = null;

                ResourceInfo resourceInfo = GetResourceInfo(binaryAssetName);
                if (resourceInfo == null)
                {
                    return false;
                }

                if (!resourceInfo.Ready)
                {
                    return false;
                }

                if (!resourceInfo.IsLoadFromBinary)
                {
                    return false;
                }

                storageInReadOnly = resourceInfo.StorageInReadOnly;
                if (resourceInfo.UseFileSystem)
                {
                    storageInFileSystem = true;
                    relativePath = Utility.Text.Format("{0}.{1}", resourceInfo.FileSystemName, DefaultExtension);
                    fileName = resourceInfo.ResourceName.FullName;
                }
                else
                {
                    relativePath = resourceInfo.ResourceName.FullName;
                }

                return true;
            }

            /// <summary>
            /// 获取二进制资源的长度。
            /// </summary>
            /// <param name="binaryAssetName">要获取长度的二进制资源的名称。</param>
            /// <returns>二进制资源的长度。</returns>
            public int GetBinaryLength(string binaryAssetName)
            {
                ResourceInfo resourceInfo = GetResourceInfo(binaryAssetName);
                if (resourceInfo == null)
                {
                    return -1;
                }

                if (!resourceInfo.Ready)
                {
                    return -1;
                }

                if (!resourceInfo.IsLoadFromBinary)
                {
                    return -1;
                }

                return resourceInfo.Length;
            }

            /// <summary>
            /// 异步加载二进制资源。
            /// </summary>
            /// <param name="binaryAssetName">要加载二进制资源的名称。</param>
            /// <param name="loadBinaryCallbacks">加载二进制资源回调函数集。</param>
            /// <param name="userData">用户自定义数据。</param>
            public void LoadBinary(string binaryAssetName, LoadBinaryCallbacks loadBinaryCallbacks, object userData)
            {
                ResourceInfo resourceInfo = GetResourceInfo(binaryAssetName);
                if (resourceInfo == null)
                {
                    string errorMessage = Utility.Text.Format("Can not load binary '{0}' which is not exist.", binaryAssetName);
                    if (loadBinaryCallbacks.LoadBinaryFailureCallback != null)
                    {
                        loadBinaryCallbacks.LoadBinaryFailureCallback(binaryAssetName, LoadResourceStatus.NotExist, errorMessage, userData);
                        return;
                    }

                    throw new GameFrameworkException(errorMessage);
                }

                if (!resourceInfo.Ready)
                {
                    string errorMessage = Utility.Text.Format("Can not load binary '{0}' which is not ready.", binaryAssetName);
                    if (loadBinaryCallbacks.LoadBinaryFailureCallback != null)
                    {
                        loadBinaryCallbacks.LoadBinaryFailureCallback(binaryAssetName, LoadResourceStatus.NotReady, errorMessage, userData);
                        return;
                    }

                    throw new GameFrameworkException(errorMessage);
                }

                if (!resourceInfo.IsLoadFromBinary)
                {
                    string errorMessage = Utility.Text.Format("Can not load binary '{0}' which is not a binary asset.", binaryAssetName);
                    if (loadBinaryCallbacks.LoadBinaryFailureCallback != null)
                    {
                        loadBinaryCallbacks.LoadBinaryFailureCallback(binaryAssetName, LoadResourceStatus.TypeError, errorMessage, userData);
                        return;
                    }

                    throw new GameFrameworkException(errorMessage);
                }

                if (resourceInfo.UseFileSystem)
                {
                    loadBinaryCallbacks.LoadBinarySuccessCallback(binaryAssetName, LoadBinaryFromFileSystem(binaryAssetName), 0f, userData);
                }
                else
                {
                    string path = Utility.Path.GetRemotePath(Path.Combine(resourceInfo.StorageInReadOnly ? m_ResourceManager.m_ReadOnlyPath : m_ResourceManager.m_ReadWritePath, resourceInfo.ResourceName.FullName));
                    m_ResourceManager.m_ResourceHelper.LoadBytes(path, m_LoadBytesCallbacks, LoadBinaryInfo.Create(binaryAssetName, resourceInfo, loadBinaryCallbacks, userData));
                }
            }

            /// <summary>
            /// 从文件系统中加载二进制资源。
            /// </summary>
            /// <param name="binaryAssetName">要加载二进制资源的名称。</param>
            /// <returns>存储加载二进制资源的二进制流。</returns>
            public byte[] LoadBinaryFromFileSystem(string binaryAssetName)
            {
                ResourceInfo resourceInfo = GetResourceInfo(binaryAssetName);
                if (resourceInfo == null)
                {
                    throw new GameFrameworkException(Utility.Text.Format("Can not load binary '{0}' from file system which is not exist.", binaryAssetName));
                }

                if (!resourceInfo.Ready)
                {
                    throw new GameFrameworkException(Utility.Text.Format("Can not load binary '{0}' from file system which is not ready.", binaryAssetName));
                }

                if (!resourceInfo.IsLoadFromBinary)
                {
                    throw new GameFrameworkException(Utility.Text.Format("Can not load binary '{0}' from file system which is not a binary asset.", binaryAssetName));
                }

                if (!resourceInfo.UseFileSystem)
                {
                    throw new GameFrameworkException(Utility.Text.Format("Can not load binary '{0}' from file system which is not use file system.", binaryAssetName));
                }

                IFileSystem fileSystem = m_ResourceManager.GetFileSystem(resourceInfo.FileSystemName, resourceInfo.StorageInReadOnly);
                byte[] bytes = fileSystem.ReadFile(resourceInfo.ResourceName.FullName);
                if (bytes == null)
                {
                    return null;
                }

                if (resourceInfo.LoadType == LoadType.LoadFromBinaryAndQuickDecrypt || resourceInfo.LoadType == LoadType.LoadFromBinaryAndDecrypt)
                {
                    DecryptResourceCallback decryptResourceCallback = m_ResourceManager.m_DecryptResourceCallback ?? DefaultDecryptResourceCallback;
                    decryptResourceCallback(bytes, 0, bytes.Length, resourceInfo.ResourceName.Name, resourceInfo.ResourceName.Variant, resourceInfo.ResourceName.Extension, resourceInfo.StorageInReadOnly, resourceInfo.FileSystemName, (byte)resourceInfo.LoadType, resourceInfo.Length, resourceInfo.HashCode);
                }

                return bytes;
            }

            /// <summary>
            /// 从文件系统中加载二进制资源。
            /// </summary>
            /// <param name="binaryAssetName">要加载二进制资源的名称。</param>
            /// <param name="buffer">存储加载二进制资源的二进制流。</param>
            /// <param name="startIndex">存储加载二进制资源的二进制流的起始位置。</param>
            /// <param name="length">存储加载二进制资源的二进制流的长度。</param>
            /// <returns>实际加载了多少字节。</returns>
            public int LoadBinaryFromFileSystem(string binaryAssetName, byte[] buffer, int startIndex, int length)
            {
                ResourceInfo resourceInfo = GetResourceInfo(binaryAssetName);
                if (resourceInfo == null)
                {
                    throw new GameFrameworkException(Utility.Text.Format("Can not load binary '{0}' from file system which is not exist.", binaryAssetName));
                }

                if (!resourceInfo.Ready)
                {
                    throw new GameFrameworkException(Utility.Text.Format("Can not load binary '{0}' from file system which is not ready.", binaryAssetName));
                }

                if (!resourceInfo.IsLoadFromBinary)
                {
                    throw new GameFrameworkException(Utility.Text.Format("Can not load binary '{0}' from file system which is not a binary asset.", binaryAssetName));
                }

                if (!resourceInfo.UseFileSystem)
                {
                    throw new GameFrameworkException(Utility.Text.Format("Can not load binary '{0}' from file system which is not use file system.", binaryAssetName));
                }

                IFileSystem fileSystem = m_ResourceManager.GetFileSystem(resourceInfo.FileSystemName, resourceInfo.StorageInReadOnly);
                int bytesRead = fileSystem.ReadFile(resourceInfo.ResourceName.FullName, buffer, startIndex, length);
                if (resourceInfo.LoadType == LoadType.LoadFromBinaryAndQuickDecrypt || resourceInfo.LoadType == LoadType.LoadFromBinaryAndDecrypt)
                {
                    DecryptResourceCallback decryptResourceCallback = m_ResourceManager.m_DecryptResourceCallback ?? DefaultDecryptResourceCallback;
                    decryptResourceCallback(buffer, startIndex, bytesRead, resourceInfo.ResourceName.Name, resourceInfo.ResourceName.Variant, resourceInfo.ResourceName.Extension, resourceInfo.StorageInReadOnly, resourceInfo.FileSystemName, (byte)resourceInfo.LoadType, resourceInfo.Length, resourceInfo.HashCode);
                }

                return bytesRead;
            }

            /// <summary>
            /// 从文件系统中加载二进制资源的片段。
            /// </summary>
            /// <param name="binaryAssetName">要加载片段的二进制资源的名称。</param>
            /// <param name="offset">要加载片段的偏移。</param>
            /// <param name="length">要加载片段的长度。</param>
            /// <returns>存储加载二进制资源片段内容的二进制流。</returns>
            public byte[] LoadBinarySegmentFromFileSystem(string binaryAssetName, int offset, int length)
            {
                ResourceInfo resourceInfo = GetResourceInfo(binaryAssetName);
                if (resourceInfo == null)
                {
                    throw new GameFrameworkException(Utility.Text.Format("Can not load binary '{0}' from file system which is not exist.", binaryAssetName));
                }

                if (!resourceInfo.Ready)
                {
                    throw new GameFrameworkException(Utility.Text.Format("Can not load binary '{0}' from file system which is not ready.", binaryAssetName));
                }

                if (!resourceInfo.IsLoadFromBinary)
                {
                    throw new GameFrameworkException(Utility.Text.Format("Can not load binary '{0}' from file system which is not a binary asset.", binaryAssetName));
                }

                if (!resourceInfo.UseFileSystem)
                {
                    throw new GameFrameworkException(Utility.Text.Format("Can not load binary '{0}' from file system which is not use file system.", binaryAssetName));
                }

                IFileSystem fileSystem = m_ResourceManager.GetFileSystem(resourceInfo.FileSystemName, resourceInfo.StorageInReadOnly);
                byte[] bytes = fileSystem.ReadFileSegment(resourceInfo.ResourceName.FullName, offset, length);
                if (bytes == null)
                {
                    return null;
                }

                if (resourceInfo.LoadType == LoadType.LoadFromBinaryAndQuickDecrypt || resourceInfo.LoadType == LoadType.LoadFromBinaryAndDecrypt)
                {
                    DecryptResourceCallback decryptResourceCallback = m_ResourceManager.m_DecryptResourceCallback ?? DefaultDecryptResourceCallback;
                    decryptResourceCallback(bytes, 0, bytes.Length, resourceInfo.ResourceName.Name, resourceInfo.ResourceName.Variant, resourceInfo.ResourceName.Extension, resourceInfo.StorageInReadOnly, resourceInfo.FileSystemName, (byte)resourceInfo.LoadType, resourceInfo.Length, resourceInfo.HashCode);
                }

                return bytes;
            }

            /// <summary>
            /// 从文件系统中加载二进制资源的片段。
            /// </summary>
            /// <param name="binaryAssetName">要加载片段的二进制资源的名称。</param>
            /// <param name="offset">要加载片段的偏移。</param>
            /// <param name="buffer">存储加载二进制资源片段内容的二进制流。</param>
            /// <param name="startIndex">存储加载二进制资源片段内容的二进制流的起始位置。</param>
            /// <param name="length">要加载片段的长度。</param>
            /// <returns>实际加载了多少字节。</returns>
            public int LoadBinarySegmentFromFileSystem(string binaryAssetName, int offset, byte[] buffer, int startIndex, int length)
            {
                ResourceInfo resourceInfo = GetResourceInfo(binaryAssetName);
                if (resourceInfo == null)
                {
                    throw new GameFrameworkException(Utility.Text.Format("Can not load binary '{0}' from file system which is not exist.", binaryAssetName));
                }

                if (!resourceInfo.Ready)
                {
                    throw new GameFrameworkException(Utility.Text.Format("Can not load binary '{0}' from file system which is not ready.", binaryAssetName));
                }

                if (!resourceInfo.IsLoadFromBinary)
                {
                    throw new GameFrameworkException(Utility.Text.Format("Can not load binary '{0}' from file system which is not a binary asset.", binaryAssetName));
                }

                if (!resourceInfo.UseFileSystem)
                {
                    throw new GameFrameworkException(Utility.Text.Format("Can not load binary '{0}' from file system which is not use file system.", binaryAssetName));
                }

                IFileSystem fileSystem = m_ResourceManager.GetFileSystem(resourceInfo.FileSystemName, resourceInfo.StorageInReadOnly);
                int bytesRead = fileSystem.ReadFileSegment(resourceInfo.ResourceName.FullName, offset, buffer, startIndex, length);
                if (resourceInfo.LoadType == LoadType.LoadFromBinaryAndQuickDecrypt || resourceInfo.LoadType == LoadType.LoadFromBinaryAndDecrypt)
                {
                    DecryptResourceCallback decryptResourceCallback = m_ResourceManager.m_DecryptResourceCallback ?? DefaultDecryptResourceCallback;
                    decryptResourceCallback(buffer, startIndex, bytesRead, resourceInfo.ResourceName.Name, resourceInfo.ResourceName.Variant, resourceInfo.ResourceName.Extension, resourceInfo.StorageInReadOnly, resourceInfo.FileSystemName, (byte)resourceInfo.LoadType, resourceInfo.Length, resourceInfo.HashCode);
                }

                return bytesRead;
            }

            /// <summary>
            /// 获取所有加载资源任务的信息。
            /// </summary>
            /// <returns>所有加载资源任务的信息。</returns>
            public TaskInfo[] GetAllLoadAssetInfos()
            {
                return m_TaskPool.GetAllTaskInfos();
            }

            /// <summary>
            /// 获取所有加载资源任务的信息。
            /// </summary>
            /// <param name="results">所有加载资源任务的信息。</param>
            public void GetAllLoadAssetInfos(List<TaskInfo> results)
            {
                m_TaskPool.GetAllTaskInfos(results);
            }

            private bool LoadDependencyAsset(string assetName, int priority, LoadResourceTaskBase mainTask, object userData)
            {
                if (mainTask == null)
                {
                    throw new GameFrameworkException("Main task is invalid.");
                }

                ResourceInfo resourceInfo = null;
                string[] dependencyAssetNames = null;
                if (!CheckAsset(assetName, out resourceInfo, out dependencyAssetNames))
                {
                    return false;
                }

                if (resourceInfo.IsLoadFromBinary)
                {
                    return false;
                }

                LoadDependencyAssetTask dependencyTask = LoadDependencyAssetTask.Create(assetName, priority, resourceInfo, dependencyAssetNames, mainTask, userData);
                foreach (string dependencyAssetName in dependencyAssetNames)
                {
                    if (!LoadDependencyAsset(dependencyAssetName, priority, dependencyTask, userData))
                    {
                        return false;
                    }
                }

                m_TaskPool.AddTask(dependencyTask);
                if (!resourceInfo.Ready)
                {
                    m_ResourceManager.UpdateResource(resourceInfo.ResourceName);
                }

                return true;
            }

            private ResourceInfo GetResourceInfo(string assetName)
            {
                if (string.IsNullOrEmpty(assetName))
                {
                    return null;
                }

                AssetInfo assetInfo = m_ResourceManager.GetAssetInfo(assetName);
                if (assetInfo == null)
                {
                    return null;
                }

                return m_ResourceManager.GetResourceInfo(assetInfo.ResourceName);
            }

            private bool CheckAsset(string assetName, out ResourceInfo resourceInfo, out string[] dependencyAssetNames)
            {
                resourceInfo = null;
                dependencyAssetNames = null;

                if (string.IsNullOrEmpty(assetName))
                {
                    return false;
                }

                AssetInfo assetInfo = m_ResourceManager.GetAssetInfo(assetName);
                if (assetInfo == null)
                {
                    return false;
                }

                resourceInfo = m_ResourceManager.GetResourceInfo(assetInfo.ResourceName);
                if (resourceInfo == null)
                {
                    return false;
                }

                dependencyAssetNames = assetInfo.GetDependencyAssetNames();
                return m_ResourceManager.m_ResourceMode == ResourceMode.UpdatableWhilePlaying ? true : resourceInfo.Ready;
            }

            private void DefaultDecryptResourceCallback(byte[] bytes, int startIndex, int count, string name, string variant, string extension, bool storageInReadOnly, string fileSystem, byte loadType, int length, int hashCode)
            {
                Utility.Converter.GetBytes(hashCode, m_CachedHashBytes);
                switch ((LoadType)loadType)
                {
                    case LoadType.LoadFromMemoryAndQuickDecrypt:
                    case LoadType.LoadFromBinaryAndQuickDecrypt:
                        Utility.Encryption.GetQuickSelfXorBytes(bytes, m_CachedHashBytes);
                        break;

                    case LoadType.LoadFromMemoryAndDecrypt:
                    case LoadType.LoadFromBinaryAndDecrypt:
                        Utility.Encryption.GetSelfXorBytes(bytes, m_CachedHashBytes);
                        break;

                    default:
                        throw new GameFrameworkException("Not supported load type when decrypt resource.");
                }

                Array.Clear(m_CachedHashBytes, 0, CachedHashBytesLength);
            }

            private void OnLoadBinarySuccess(string fileUri, byte[] bytes, float duration, object userData)
            {
                LoadBinaryInfo loadBinaryInfo = (LoadBinaryInfo)userData;
                if (loadBinaryInfo == null)
                {
                    throw new GameFrameworkException("Load binary info is invalid.");
                }

                ResourceInfo resourceInfo = loadBinaryInfo.ResourceInfo;
                if (resourceInfo.LoadType == LoadType.LoadFromBinaryAndQuickDecrypt || resourceInfo.LoadType == LoadType.LoadFromBinaryAndDecrypt)
                {
                    DecryptResourceCallback decryptResourceCallback = m_ResourceManager.m_DecryptResourceCallback ?? DefaultDecryptResourceCallback;
                    decryptResourceCallback(bytes, 0, bytes.Length, resourceInfo.ResourceName.Name, resourceInfo.ResourceName.Variant, resourceInfo.ResourceName.Extension, resourceInfo.StorageInReadOnly, resourceInfo.FileSystemName, (byte)resourceInfo.LoadType, resourceInfo.Length, resourceInfo.HashCode);
                }

                loadBinaryInfo.LoadBinaryCallbacks.LoadBinarySuccessCallback(loadBinaryInfo.BinaryAssetName, bytes, duration, loadBinaryInfo.UserData);
                ReferencePool.Release(loadBinaryInfo);
            }

            private void OnLoadBinaryFailure(string fileUri, string errorMessage, object userData)
            {
                LoadBinaryInfo loadBinaryInfo = (LoadBinaryInfo)userData;
                if (loadBinaryInfo == null)
                {
                    throw new GameFrameworkException("Load binary info is invalid.");
                }

                if (loadBinaryInfo.LoadBinaryCallbacks.LoadBinaryFailureCallback != null)
                {
                    loadBinaryInfo.LoadBinaryCallbacks.LoadBinaryFailureCallback(loadBinaryInfo.BinaryAssetName, LoadResourceStatus.AssetError, errorMessage, loadBinaryInfo.UserData);
                }

                ReferencePool.Release(loadBinaryInfo);
            }
        }
    }
}
