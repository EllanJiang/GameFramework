//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

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
                m_SceneToAssetMap = new Dictionary<string, object>();
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
                ResourceInfo? resourceInfo = null;
                string[] dependencyAssetNames = null;
                if (!CheckAsset(assetName, out resourceInfo, out dependencyAssetNames))
                {
                    return HasAssetResult.NotExist;
                }

                return IsLoadFromBinary(resourceInfo.Value.LoadType) ? HasAssetResult.Binary : HasAssetResult.Asset;
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
                ResourceInfo? resourceInfo = null;
                string[] dependencyAssetNames = null;
                if (!CheckAsset(assetName, out resourceInfo, out dependencyAssetNames))
                {
                    string errorMessage = Utility.Text.Format("Can not load asset '{0}'.", assetName);
                    if (loadAssetCallbacks.LoadAssetFailureCallback != null)
                    {
                        loadAssetCallbacks.LoadAssetFailureCallback(assetName, LoadResourceStatus.NotReady, errorMessage, userData);
                        return;
                    }

                    throw new GameFrameworkException(errorMessage);
                }

                if (IsLoadFromBinary(resourceInfo.Value.LoadType))
                {
                    string errorMessage = Utility.Text.Format("Can not load asset '{0}' which is a binary asset.", assetName);
                    if (loadAssetCallbacks.LoadAssetFailureCallback != null)
                    {
                        loadAssetCallbacks.LoadAssetFailureCallback(assetName, LoadResourceStatus.TypeError, errorMessage, userData);
                        return;
                    }

                    throw new GameFrameworkException(errorMessage);
                }

                LoadAssetTask mainTask = LoadAssetTask.Create(assetName, assetType, priority, resourceInfo.Value, dependencyAssetNames, loadAssetCallbacks, userData);
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
                ResourceInfo? resourceInfo = null;
                string[] dependencyAssetNames = null;
                if (!CheckAsset(sceneAssetName, out resourceInfo, out dependencyAssetNames))
                {
                    string errorMessage = Utility.Text.Format("Can not load scene '{0}'.", sceneAssetName);
                    if (loadSceneCallbacks.LoadSceneFailureCallback != null)
                    {
                        loadSceneCallbacks.LoadSceneFailureCallback(sceneAssetName, LoadResourceStatus.NotReady, errorMessage, userData);
                        return;
                    }

                    throw new GameFrameworkException(errorMessage);
                }

                if (IsLoadFromBinary(resourceInfo.Value.LoadType))
                {
                    string errorMessage = Utility.Text.Format("Can not load scene asset '{0}' which is a binary asset.", sceneAssetName);
                    if (loadSceneCallbacks.LoadSceneFailureCallback != null)
                    {
                        loadSceneCallbacks.LoadSceneFailureCallback(sceneAssetName, LoadResourceStatus.TypeError, errorMessage, userData);
                        return;
                    }

                    throw new GameFrameworkException(errorMessage);
                }

                LoadSceneTask mainTask = LoadSceneTask.Create(sceneAssetName, priority, resourceInfo.Value, dependencyAssetNames, loadSceneCallbacks, userData);
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
            public string GetBinaryPath(string binaryAssetName)
            {
                ResourceInfo? resourceInfo = null;
                string[] dependencyAssetNames = null;
                if (!CheckAsset(binaryAssetName, out resourceInfo, out dependencyAssetNames))
                {
                    return null;
                }

                if (!IsLoadFromBinary(resourceInfo.Value.LoadType))
                {
                    return null;
                }

                return Utility.Path.GetRegularPath(Path.Combine(resourceInfo.Value.StorageInReadOnly ? m_ResourceManager.m_ReadOnlyPath : m_ResourceManager.m_ReadWritePath, resourceInfo.Value.ResourceName.FullName));
            }

            /// <summary>
            /// 获取二进制资源的实际路径。
            /// </summary>
            /// <param name="binaryAssetName">要获取实际路径的二进制资源的名称。</param>
            /// <param name="storageInReadOnly">资源是否在只读区。</param>
            /// <param name="relativePath">二进制资源相对于只读区或者读写区的相对路径。</param>
            /// <returns>获取二进制资源的实际路径是否成功。</returns>
            public bool GetBinaryPath(string binaryAssetName, out bool storageInReadOnly, out string relativePath)
            {
                storageInReadOnly = false;
                relativePath = null;

                ResourceInfo? resourceInfo = null;
                string[] dependencyAssetNames = null;
                if (!CheckAsset(binaryAssetName, out resourceInfo, out dependencyAssetNames))
                {
                    return false;
                }

                if (!IsLoadFromBinary(resourceInfo.Value.LoadType))
                {
                    return false;
                }

                storageInReadOnly = resourceInfo.Value.StorageInReadOnly;
                relativePath = resourceInfo.Value.ResourceName.FullName;
                return true;
            }

            /// <summary>
            /// 异步加载二进制资源。
            /// </summary>
            /// <param name="binaryAssetName">要加载二进制资源的名称。</param>
            /// <param name="loadBinaryCallbacks">加载二进制资源回调函数集。</param>
            /// <param name="userData">用户自定义数据。</param>
            public void LoadBinary(string binaryAssetName, LoadBinaryCallbacks loadBinaryCallbacks, object userData)
            {
                ResourceInfo? resourceInfo = null;
                string[] dependencyAssetNames = null;
                if (!CheckAsset(binaryAssetName, out resourceInfo, out dependencyAssetNames))
                {
                    string errorMessage = Utility.Text.Format("Can not load binary '{0}'.", binaryAssetName);
                    if (loadBinaryCallbacks.LoadBinaryFailureCallback != null)
                    {
                        loadBinaryCallbacks.LoadBinaryFailureCallback(binaryAssetName, LoadResourceStatus.NotReady, errorMessage, userData);
                        return;
                    }

                    throw new GameFrameworkException(errorMessage);
                }

                if (!IsLoadFromBinary(resourceInfo.Value.LoadType))
                {
                    string errorMessage = Utility.Text.Format("Can not load binary asset '{0}' which is not a binary asset.", binaryAssetName);
                    if (loadBinaryCallbacks.LoadBinaryFailureCallback != null)
                    {
                        loadBinaryCallbacks.LoadBinaryFailureCallback(binaryAssetName, LoadResourceStatus.TypeError, errorMessage, userData);
                        return;
                    }

                    throw new GameFrameworkException(errorMessage);
                }

                string path = Utility.Path.GetRemotePath(Path.Combine(resourceInfo.Value.StorageInReadOnly ? m_ResourceManager.m_ReadOnlyPath : m_ResourceManager.m_ReadWritePath, resourceInfo.Value.ResourceName.FullName));
                m_ResourceManager.m_ResourceHelper.LoadBytes(path, m_LoadBytesCallbacks, LoadBinaryInfo.Create(binaryAssetName, resourceInfo.Value, loadBinaryCallbacks, userData));
            }

            /// <summary>
            /// 获取所有加载资源任务的信息。
            /// </summary>
            /// <returns>所有加载资源任务的信息。</returns>
            public TaskInfo[] GetAllLoadAssetInfos()
            {
                return m_TaskPool.GetAllTaskInfos();
            }

            private bool LoadDependencyAsset(string assetName, int priority, LoadResourceTaskBase mainTask, object userData)
            {
                if (mainTask == null)
                {
                    throw new GameFrameworkException("Main task is invalid.");
                }

                ResourceInfo? resourceInfo = null;
                string[] dependencyAssetNames = null;
                if (!CheckAsset(assetName, out resourceInfo, out dependencyAssetNames))
                {
                    return false;
                }

                if (IsLoadFromBinary(resourceInfo.Value.LoadType))
                {
                    return false;
                }

                LoadDependencyAssetTask dependencyTask = LoadDependencyAssetTask.Create(assetName, priority, resourceInfo.Value, dependencyAssetNames, mainTask, userData);
                foreach (string dependencyAssetName in dependencyAssetNames)
                {
                    if (!LoadDependencyAsset(dependencyAssetName, priority, dependencyTask, userData))
                    {
                        return false;
                    }
                }

                m_TaskPool.AddTask(dependencyTask);
                return true;
            }

            private bool IsLoadFromBinary(LoadType loadType)
            {
                return loadType == LoadType.LoadFromBinary || loadType == LoadType.LoadFromBinaryAndQuickDecrypt || loadType == LoadType.LoadFromBinaryAndDecrypt;
            }

            private bool CheckAsset(string assetName, out ResourceInfo? resourceInfo, out string[] dependencyAssetNames)
            {
                resourceInfo = null;
                dependencyAssetNames = null;

                if (string.IsNullOrEmpty(assetName))
                {
                    return false;
                }

                AssetInfo? assetInfo = m_ResourceManager.GetAssetInfo(assetName);
                if (!assetInfo.HasValue)
                {
                    return false;
                }

                resourceInfo = m_ResourceManager.GetResourceInfo(assetInfo.Value.ResourceName);
                if (!resourceInfo.HasValue)
                {
                    return false;
                }

                dependencyAssetNames = assetInfo.Value.GetDependencyAssetNames();
                return true;
            }

            private byte[] DefaultDecryptResourceCallback(string name, string variant, byte loadType, int length, int hashCode, bool storageInReadOnly, byte[] bytes)
            {
                switch ((LoadType)loadType)
                {
                    case LoadType.LoadFromMemoryAndQuickDecrypt:
                    case LoadType.LoadFromBinaryAndQuickDecrypt:
                        Utility.Converter.GetBytes(hashCode, m_CachedHashBytes);
                        Utility.Encryption.GetQuickSelfXorBytes(bytes, m_CachedHashBytes);
                        Array.Clear(m_CachedHashBytes, 0, CachedHashBytesLength);
                        return bytes;

                    case LoadType.LoadFromMemoryAndDecrypt:
                    case LoadType.LoadFromBinaryAndDecrypt:
                        Utility.Converter.GetBytes(hashCode, m_CachedHashBytes);
                        Utility.Encryption.GetSelfXorBytes(bytes, m_CachedHashBytes);
                        Array.Clear(m_CachedHashBytes, 0, CachedHashBytesLength);
                        return bytes;

                    default:
                        return bytes;
                }
            }

            private void OnLoadBinarySuccess(string fileUri, byte[] bytes, float duration, object userData)
            {
                LoadBinaryInfo loadBinaryInfo = (LoadBinaryInfo)userData;
                if (loadBinaryInfo == null)
                {
                    throw new GameFrameworkException("Load binary info is invalid.");
                }

                if (loadBinaryInfo.ResourceInfo.LoadType == LoadType.LoadFromBinaryAndQuickDecrypt || loadBinaryInfo.ResourceInfo.LoadType == LoadType.LoadFromBinaryAndDecrypt)
                {
                    DecryptResourceCallback decryptResourceCallback = m_ResourceManager.m_DecryptResourceCallback ?? DefaultDecryptResourceCallback;
                    bytes = decryptResourceCallback(loadBinaryInfo.ResourceInfo.ResourceName.Name, loadBinaryInfo.ResourceInfo.ResourceName.Variant, (byte)loadBinaryInfo.ResourceInfo.LoadType, loadBinaryInfo.ResourceInfo.Length, loadBinaryInfo.ResourceInfo.HashCode, loadBinaryInfo.ResourceInfo.StorageInReadOnly, bytes);
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
