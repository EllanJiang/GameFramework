//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.ObjectPool;
using System;
using System.Collections.Generic;

namespace GameFramework.Resource
{
    internal partial class ResourceManager
    {
        /// <summary>
        /// 加载资源器。
        /// </summary>
        private sealed partial class ResourceLoader
        {
            private readonly ResourceManager m_ResourceManager;
            private readonly TaskPool<LoadResourceTaskBase> m_TaskPool;
            private readonly Dictionary<object, int> m_DependencyCount;
            private readonly Dictionary<string, object> m_SceneToAssetMap;
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
                m_DependencyCount = new Dictionary<object, int>();
                m_SceneToAssetMap = new Dictionary<string, object>();
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
                m_DependencyCount.Clear();
                m_SceneToAssetMap.Clear();
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

                LoadResourceAgent agent = new LoadResourceAgent(loadResourceAgentHelper, resourceHelper, m_AssetPool, m_ResourcePool, this, readOnlyPath, readWritePath, decryptResourceCallback ?? DefaultDecryptResourceCallback);
                m_TaskPool.AddAgent(agent);
            }

            /// <summary>
            /// 检查资源是否存在。
            /// </summary>
            /// <param name="assetName">要检查资源的名称。</param>
            /// <returns>资源是否存在。</returns>
            public bool HasAsset(string assetName)
            {
                if (string.IsNullOrEmpty(assetName))
                {
                    return false;
                }

                return m_ResourceManager.GetAssetInfo(assetName).HasValue;
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
                string resourceChildName = null;
                string[] dependencyAssetNames = null;
                string[] scatteredDependencyAssetNames = null;

                if (!CheckAsset(assetName, out resourceInfo, out resourceChildName, out dependencyAssetNames, out scatteredDependencyAssetNames))
                {
                    string errorMessage = string.Format("Can not load asset '{0}'.", assetName);
                    if (loadAssetCallbacks.LoadAssetFailureCallback != null)
                    {
                        loadAssetCallbacks.LoadAssetFailureCallback(assetName, LoadResourceStatus.NotReady, errorMessage, userData);
                        return;
                    }

                    throw new GameFrameworkException(errorMessage);
                }

                LoadAssetTask mainTask = new LoadAssetTask(assetName, assetType, priority, resourceInfo.Value, resourceChildName, dependencyAssetNames, scatteredDependencyAssetNames, loadAssetCallbacks, userData);
                foreach (string dependencyAssetName in dependencyAssetNames)
                {
                    if (!LoadDependencyAsset(dependencyAssetName, priority, mainTask, userData))
                    {
                        string errorMessage = string.Format("Can not load dependency asset '{0}' when load asset '{1}'.", dependencyAssetName, assetName);
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
                string resourceChildName = null;
                string[] dependencyAssetNames = null;
                string[] scatteredDependencyAssetNames = null;

                if (!CheckAsset(sceneAssetName, out resourceInfo, out resourceChildName, out dependencyAssetNames, out scatteredDependencyAssetNames))
                {
                    string errorMessage = string.Format("Can not load scene '{0}'.", sceneAssetName);
                    if (loadSceneCallbacks.LoadSceneFailureCallback != null)
                    {
                        loadSceneCallbacks.LoadSceneFailureCallback(sceneAssetName, LoadResourceStatus.NotReady, errorMessage, userData);
                        return;
                    }

                    throw new GameFrameworkException(errorMessage);
                }

                LoadSceneTask mainTask = new LoadSceneTask(sceneAssetName, priority, resourceInfo.Value, resourceChildName, dependencyAssetNames, scatteredDependencyAssetNames, loadSceneCallbacks, userData);
                foreach (string dependencyAssetName in dependencyAssetNames)
                {
                    if (!LoadDependencyAsset(dependencyAssetName, priority, mainTask, userData))
                    {
                        string errorMessage = string.Format("Can not load dependency asset '{0}' when load scene '{1}'.", dependencyAssetName, sceneAssetName);
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
                    throw new GameFrameworkException(string.Format("Can not find asset of scene '{0}'.", sceneAssetName));
                }

                m_ResourceManager.m_ResourceHelper.UnloadScene(sceneAssetName, unloadSceneCallbacks, userData);
            }

            private bool LoadDependencyAsset(string assetName, int priority, LoadResourceTaskBase mainTask, object userData)
            {
                if (mainTask == null)
                {
                    throw new GameFrameworkException("Main task is invalid.");
                }

                ResourceInfo? resourceInfo = null;
                string resourceChildName = null;
                string[] dependencyAssetNames = null;
                string[] scatteredDependencyAssetNames = null;

                if (!CheckAsset(assetName, out resourceInfo, out resourceChildName, out dependencyAssetNames, out scatteredDependencyAssetNames))
                {
                    Log.Debug("Can not load asset '{0}'.", assetName);
                    return false;
                }

                LoadDependencyAssetTask dependencyTask = new LoadDependencyAssetTask(assetName, priority, resourceInfo.Value, resourceChildName, dependencyAssetNames, scatteredDependencyAssetNames, mainTask, userData);
                foreach (string dependencyAssetName in dependencyAssetNames)
                {
                    if (!LoadDependencyAsset(dependencyAssetName, priority, dependencyTask, userData))
                    {
                        Log.Debug("Can not load dependency asset '{0}' when load dependency asset '{1}'.", dependencyAssetName, assetName);
                        return false;
                    }
                }

                m_TaskPool.AddTask(dependencyTask);
                return true;
            }

            private bool CheckAsset(string assetName, out ResourceInfo? resourceInfo, out string resourceChildName, out string[] dependencyAssetNames, out string[] scatteredDependencyAssetNames)
            {
                resourceInfo = null;
                resourceChildName = null;
                dependencyAssetNames = null;
                scatteredDependencyAssetNames = null;

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

                int childNamePosition = assetName.LastIndexOf('/');
                if (childNamePosition + 1 >= assetName.Length)
                {
                    return false;
                }

                resourceChildName = assetName.Substring(childNamePosition + 1);

                AssetDependencyInfo? assetDependencyInfo = m_ResourceManager.GetAssetDependencyInfo(assetName);
                if (assetDependencyInfo.HasValue)
                {
                    dependencyAssetNames = assetDependencyInfo.Value.GetDependencyAssetNames();
                    scatteredDependencyAssetNames = assetDependencyInfo.Value.GetScatteredDependencyAssetNames();
                }

                return true;
            }

            private byte[] DefaultDecryptResourceCallback(string name, string variant, int loadType, int length, int hashCode, bool storageInReadOnly, byte[] bytes)
            {
                switch ((LoadType)loadType)
                {
                    case LoadType.LoadFromMemoryAndQuickDecrypt:
                        return Utility.Encryption.GetQuickXorBytes(bytes, Utility.Converter.GetBytes(hashCode));
                    case LoadType.LoadFromMemoryAndDecrypt:
                        return Utility.Encryption.GetXorBytes(bytes, Utility.Converter.GetBytes(hashCode));
                    default:
                        return bytes;
                }
            }
        }
    }
}
