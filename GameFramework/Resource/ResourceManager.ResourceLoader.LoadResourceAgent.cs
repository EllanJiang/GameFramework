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
        private partial class ResourceLoader
        {
            /// <summary>
            /// 加载资源代理。
            /// </summary>
            private sealed partial class LoadResourceAgent : ITaskAgent<LoadResourceTaskBase>
            {
                private static readonly HashSet<string> s_LoadingAssetNames = new HashSet<string>();
                private static readonly HashSet<string> s_LoadingResourceNames = new HashSet<string>();

                private readonly ILoadResourceAgentHelper m_Helper;
                private readonly IResourceHelper m_ResourceHelper;
                private readonly IObjectPool<AssetObject> m_AssetPool;
                private readonly IObjectPool<ResourceObject> m_ResourcePool;
                private readonly ResourceLoader m_ResourceLoader;
                private readonly string m_ReadOnlyPath;
                private readonly string m_ReadWritePath;
                private readonly DecryptResourceCallback m_DecryptResourceCallback;
                private readonly LinkedList<string> m_LoadingDependencyAssetNames;
                private LoadResourceTaskBase m_Task;
                private WaitingType m_WaitingType;
                private bool m_LoadingAsset;
                private bool m_LoadingResource;

                /// <summary>
                /// 初始化加载资源代理的新实例。
                /// </summary>
                /// <param name="loadResourceAgentHelper">加载资源代理辅助器。</param>
                /// <param name="resourceHelper">资源辅助器。</param>
                /// <param name="assetPool">资源对象池。</param>
                /// <param name="resourcePool">资源对象池。</param>
                /// <param name="resourceLoader">加载资源器。</param>
                /// <param name="readOnlyPath">资源只读区路径。</param>
                /// <param name="readWritePath">资源读写区路径。</param>
                /// <param name="decryptResourceCallback">解密资源回调函数。</param>
                public LoadResourceAgent(ILoadResourceAgentHelper loadResourceAgentHelper, IResourceHelper resourceHelper, IObjectPool<AssetObject> assetPool, IObjectPool<ResourceObject> resourcePool, ResourceLoader resourceLoader, string readOnlyPath, string readWritePath, DecryptResourceCallback decryptResourceCallback)
                {
                    if (loadResourceAgentHelper == null)
                    {
                        throw new GameFrameworkException("Load resource agent helper is invalid.");
                    }

                    if (resourceHelper == null)
                    {
                        throw new GameFrameworkException("Resource helper is invalid.");
                    }

                    if (assetPool == null)
                    {
                        throw new GameFrameworkException("Asset pool is invalid.");
                    }

                    if (resourcePool == null)
                    {
                        throw new GameFrameworkException("Resource pool is invalid.");
                    }

                    if (resourceLoader == null)
                    {
                        throw new GameFrameworkException("Resource loader is invalid.");
                    }

                    if (decryptResourceCallback == null)
                    {
                        throw new GameFrameworkException("Decrypt resource callback is invalid.");
                    }

                    m_Helper = loadResourceAgentHelper;
                    m_ResourceHelper = resourceHelper;
                    m_AssetPool = assetPool;
                    m_ResourcePool = resourcePool;
                    m_ResourceLoader = resourceLoader;
                    m_ReadOnlyPath = readOnlyPath;
                    m_ReadWritePath = readWritePath;
                    m_DecryptResourceCallback = decryptResourceCallback;
                    m_LoadingDependencyAssetNames = new LinkedList<string>();
                    m_Task = null;
                    m_WaitingType = WaitingType.None;
                    m_LoadingAsset = false;
                    m_LoadingResource = false;
                }

                public ILoadResourceAgentHelper Helper
                {
                    get
                    {
                        return m_Helper;
                    }
                }

                /// <summary>
                /// 获取加载资源任务。
                /// </summary>
                public LoadResourceTaskBase Task
                {
                    get
                    {
                        return m_Task;
                    }
                }

                /// <summary>
                /// 初始化加载资源代理。
                /// </summary>
                public void Initialize()
                {
                    m_Helper.LoadResourceAgentHelperUpdate += OnLoadResourceAgentHelperUpdate;
                    m_Helper.LoadResourceAgentHelperReadFileComplete += OnLoadResourceAgentHelperReadFileComplete;
                    m_Helper.LoadResourceAgentHelperReadBytesComplete += OnLoadResourceAgentHelperReadBytesComplete;
                    m_Helper.LoadResourceAgentHelperParseBytesComplete += OnLoadResourceAgentHelperParseBytesComplete;
                    m_Helper.LoadResourceAgentHelperLoadComplete += OnLoadResourceAgentHelperLoadComplete;
                    m_Helper.LoadResourceAgentHelperError += OnLoadResourceAgentHelperError;
                }

                /// <summary>
                /// 加载资源代理轮询。
                /// </summary>
                /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
                /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
                public void Update(float elapseSeconds, float realElapseSeconds)
                {
                    if (m_WaitingType == WaitingType.None)
                    {
                        return;
                    }

                    if (m_WaitingType == WaitingType.WaitForAsset)
                    {
                        if (IsAssetLoading(m_Task.AssetName))
                        {
                            return;
                        }

                        m_WaitingType = WaitingType.None;
                        AssetObject assetObject = m_AssetPool.Spawn(m_Task.AssetName);
                        if (assetObject == null)
                        {
                            TryLoadAsset();
                            return;
                        }

                        OnAssetObjectReady(assetObject);
                        return;
                    }

                    if (m_WaitingType == WaitingType.WaitForDependencyAsset)
                    {
                        LinkedListNode<string> current = m_LoadingDependencyAssetNames.First;
                        while (current != null)
                        {
                            if (!IsAssetLoading(current.Value))
                            {
                                LinkedListNode<string> next = current.Next;
                                if (!m_AssetPool.CanSpawn(current.Value))
                                {
                                    OnError(LoadResourceStatus.DependencyError, string.Format("Can not find dependency asset object named '{0}'.", current.Value));
                                    return;
                                }

                                m_LoadingDependencyAssetNames.Remove(current);
                                current = next;
                                continue;
                            }

                            current = current.Next;
                        }

                        if (m_LoadingDependencyAssetNames.Count > 0)
                        {
                            return;
                        }

                        m_WaitingType = WaitingType.None;
                        OnDependencyAssetReady();
                        return;
                    }

                    if (m_WaitingType == WaitingType.WaitForResource)
                    {
                        if (IsResourceLoading(m_Task.ResourceInfo.ResourceName.Name))
                        {
                            return;
                        }

                        ResourceObject resourceObject = m_ResourcePool.Spawn(m_Task.ResourceInfo.ResourceName.Name);
                        if (resourceObject == null)
                        {
                            OnError(LoadResourceStatus.DependencyError, string.Format("Can not find resource object named '{0}'.", m_Task.ResourceInfo.ResourceName.Name));
                            return;
                        }

                        m_WaitingType = WaitingType.None;
                        OnResourceObjectReady(resourceObject);
                        return;
                    }
                }

                /// <summary>
                /// 关闭并清理加载资源代理。
                /// </summary>
                public void Shutdown()
                {
                    Reset();
                    m_Helper.LoadResourceAgentHelperUpdate -= OnLoadResourceAgentHelperUpdate;
                    m_Helper.LoadResourceAgentHelperReadFileComplete -= OnLoadResourceAgentHelperReadFileComplete;
                    m_Helper.LoadResourceAgentHelperReadBytesComplete -= OnLoadResourceAgentHelperReadBytesComplete;
                    m_Helper.LoadResourceAgentHelperParseBytesComplete -= OnLoadResourceAgentHelperParseBytesComplete;
                    m_Helper.LoadResourceAgentHelperLoadComplete -= OnLoadResourceAgentHelperLoadComplete;
                    m_Helper.LoadResourceAgentHelperError -= OnLoadResourceAgentHelperError;
                }

                /// <summary>
                /// 开始处理加载资源任务。
                /// </summary>
                /// <param name="task">要处理的加载资源任务。</param>
                public void Start(LoadResourceTaskBase task)
                {
                    if (task == null)
                    {
                        throw new GameFrameworkException("Task is invalid.");
                    }

                    m_Task = task;
                    m_Task.StartTime = DateTime.Now;

                    if (IsAssetLoading(m_Task.AssetName))
                    {
                        m_WaitingType = WaitingType.WaitForAsset;
                        return;
                    }

                    TryLoadAsset();
                }

                /// <summary>
                /// 重置加载资源代理。
                /// </summary>
                public void Reset()
                {
                    m_Helper.Reset();
                    m_LoadingDependencyAssetNames.Clear();
                    m_Task = null;
                    m_WaitingType = WaitingType.None;
                    m_LoadingAsset = false;
                    m_LoadingResource = false;
                }

                private static bool IsAssetLoading(string assetName)
                {
                    return s_LoadingAssetNames.Contains(assetName);
                }

                private static bool IsResourceLoading(string resourceName)
                {
                    return s_LoadingResourceNames.Contains(resourceName);
                }

                private void TryLoadAsset()
                {
                    if (!m_Task.IsScene)
                    {
                        AssetObject assetObject = m_AssetPool.Spawn(m_Task.AssetName);
                        if (assetObject != null)
                        {
                            OnAssetObjectReady(assetObject);
                            return;
                        }
                    }

                    m_LoadingAsset = true;
                    s_LoadingAssetNames.Add(m_Task.AssetName);

                    foreach (string dependencyAssetName in m_Task.GetDependencyAssetNames())
                    {
                        if (!m_AssetPool.CanSpawn(dependencyAssetName))
                        {
                            if (!IsAssetLoading(dependencyAssetName))
                            {
                                OnError(LoadResourceStatus.DependencyError, string.Format("Can not find dependency asset object named '{0}'.", dependencyAssetName));
                                return;
                            }

                            m_LoadingDependencyAssetNames.AddLast(dependencyAssetName);
                        }
                    }

                    if (m_LoadingDependencyAssetNames.Count > 0)
                    {
                        m_WaitingType = WaitingType.WaitForDependencyAsset;
                        return;
                    }

                    OnDependencyAssetReady();
                }

                private void OnAssetObjectReady(AssetObject assetObject)
                {
                    m_Helper.Reset();

                    object asset = assetObject.Target;
                    if (m_Task.IsScene)
                    {
                        m_ResourceLoader.m_SceneToAssetMap.Add(m_Task.AssetName, asset);
                    }

                    m_Task.OnLoadAssetSuccess(this, asset, (float)(DateTime.Now - m_Task.StartTime).TotalSeconds);
                    m_Task.Done = true;
                }

                private void OnDependencyAssetReady()
                {
                    if (IsResourceLoading(m_Task.ResourceInfo.ResourceName.Name))
                    {
                        m_WaitingType = WaitingType.WaitForResource;
                        return;
                    }

                    ResourceObject resourceObject = m_ResourcePool.Spawn(m_Task.ResourceInfo.ResourceName.Name);
                    if (resourceObject != null)
                    {
                        OnResourceObjectReady(resourceObject);
                        return;
                    }

                    m_LoadingResource = true;
                    s_LoadingResourceNames.Add(m_Task.ResourceInfo.ResourceName.Name);

                    string fullPath = Utility.Path.GetCombinePath(m_Task.ResourceInfo.StorageInReadOnly ? m_ReadOnlyPath : m_ReadWritePath, Utility.Path.GetResourceNameWithSuffix(m_Task.ResourceInfo.ResourceName.FullName));
                    if (m_Task.ResourceInfo.LoadType == LoadType.LoadFromFile)
                    {
                        m_Helper.ReadFile(fullPath);
                    }
                    else
                    {
                        m_Helper.ReadBytes(fullPath, (int)m_Task.ResourceInfo.LoadType);
                    }
                }

                private void OnResourceObjectReady(ResourceObject resourceObject)
                {
                    m_Task.LoadMain(this, resourceObject.Target);
                }

                private void OnError(LoadResourceStatus status, string errorMessage)
                {
                    m_Helper.Reset();
                    m_Task.OnLoadAssetFailure(this, status, errorMessage);
                    if (m_LoadingAsset)
                    {
                        m_LoadingAsset = false;
                        s_LoadingAssetNames.Remove(m_Task.AssetName);
                    }

                    if (m_LoadingResource)
                    {
                        m_LoadingResource = false;
                        s_LoadingResourceNames.Remove(m_Task.ResourceInfo.ResourceName.Name);
                    }

                    m_Task.Done = true;
                }

                private void OnLoadResourceAgentHelperUpdate(object sender, LoadResourceAgentHelperUpdateEventArgs e)
                {
                    m_Task.OnLoadAssetUpdate(this, e.Type, e.Progress);
                }

                private void OnLoadResourceAgentHelperReadFileComplete(object sender, LoadResourceAgentHelperReadFileCompleteEventArgs e)
                {
                    ResourceObject resourceObject = new ResourceObject(m_Task.ResourceInfo.ResourceName.Name, e.Resource, m_ResourceHelper);
                    m_ResourcePool.Register(resourceObject, true);
                    m_LoadingResource = false;
                    s_LoadingResourceNames.Remove(m_Task.ResourceInfo.ResourceName.Name);
                    OnResourceObjectReady(resourceObject);
                }

                private void OnLoadResourceAgentHelperReadBytesComplete(object sender, LoadResourceAgentHelperReadBytesCompleteEventArgs e)
                {
                    byte[] bytes = e.GetBytes();
                    LoadType loadType = (LoadType)e.LoadType;
                    if (loadType == LoadType.LoadFromMemoryAndQuickDecrypt || loadType == LoadType.LoadFromMemoryAndDecrypt)
                    {
                        bytes = m_DecryptResourceCallback(m_Task.ResourceInfo.ResourceName.Name, m_Task.ResourceInfo.ResourceName.Variant, e.LoadType, m_Task.ResourceInfo.Length, m_Task.ResourceInfo.HashCode, m_Task.ResourceInfo.StorageInReadOnly, bytes);
                    }

                    m_Helper.ParseBytes(bytes);
                }

                private void OnLoadResourceAgentHelperParseBytesComplete(object sender, LoadResourceAgentHelperParseBytesCompleteEventArgs e)
                {
                    ResourceObject resourceObject = new ResourceObject(m_Task.ResourceInfo.ResourceName.Name, e.Resource, m_ResourceHelper);
                    m_ResourcePool.Register(resourceObject, true);
                    m_LoadingResource = false;
                    s_LoadingResourceNames.Remove(m_Task.ResourceInfo.ResourceName.Name);
                    OnResourceObjectReady(resourceObject);
                }

                private void OnLoadResourceAgentHelperLoadComplete(object sender, LoadResourceAgentHelperLoadCompleteEventArgs e)
                {
                    AssetObject assetObject = null;
                    if (m_Task.IsScene)
                    {
                        assetObject = m_AssetPool.Spawn(m_Task.AssetName);
                    }

                    if (assetObject == null)
                    {
                        assetObject = new AssetObject(m_Task.AssetName, e.Asset, m_Task.GetDependencyAssets(), m_Task.Resource, m_AssetPool, m_ResourcePool, m_ResourceHelper, m_ResourceLoader.m_DependencyCount);
                        m_AssetPool.Register(assetObject, true);
                    }

                    m_LoadingAsset = false;
                    s_LoadingAssetNames.Remove(m_Task.AssetName);
                    OnAssetObjectReady(assetObject);
                }

                private void OnLoadResourceAgentHelperError(object sender, LoadResourceAgentHelperErrorEventArgs e)
                {
                    OnError(e.Status, e.ErrorMessage);
                }
            }
        }
    }
}
