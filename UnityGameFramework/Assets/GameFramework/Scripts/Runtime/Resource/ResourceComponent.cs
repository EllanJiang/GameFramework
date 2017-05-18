//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.Download;
using GameFramework.ObjectPool;
using GameFramework.Resource;
using System;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 资源组件。
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Game Framework/Resource")]
    public sealed partial class ResourceComponent : GameFrameworkComponent
    {
        private IResourceManager m_ResourceManager = null;
        private EventComponent m_EventComponent = null;
        private bool m_EditorResourceMode = false;
        private bool m_ForceUnloadUnusedAssets = false;
        private bool m_PreorderUnloadUnusedAssets = false;
        private bool m_PerformGCCollect = false;
        private AsyncOperation m_AsyncOperation = null;
        private float m_LastOperationElapse = 0f;
        private ResourceHelperBase m_ResourceHelper = null;

        [SerializeField]
        private ResourceMode m_ResourceMode = ResourceMode.Package;

        [SerializeField]
        private ReadWritePathType m_ReadWritePathType = ReadWritePathType.Unspecified;

        [SerializeField]
        private float m_UnloadUnusedAssetsInterval = 60f;

        [SerializeField]
        private float m_AssetAutoReleaseInterval = 60f;

        [SerializeField]
        private int m_AssetCapacity = 64;

        [SerializeField]
        private float m_AssetExpireTime = 60f;

        [SerializeField]
        private int m_AssetPriority = 0;

        [SerializeField]
        private float m_ResourceAutoReleaseInterval = 60f;

        [SerializeField]
        private int m_ResourceCapacity = 16;

        [SerializeField]
        private float m_ResourceExpireTime = 60f;

        [SerializeField]
        private int m_ResourcePriority = 0;

        [SerializeField]
        private string m_UpdatePrefixUri = null;

        [SerializeField]
        private int m_UpdateRetryCount = 3;

        [SerializeField]
        private Transform m_InstanceRoot = null;

        [SerializeField]
        private string m_ResourceHelperTypeName = "UnityGameFramework.Runtime.DefaultResourceHelper";

        [SerializeField]
        private ResourceHelperBase m_CustomResourceHelper = null;

        [SerializeField]
        private string m_LoadResourceAgentHelperTypeName = "UnityGameFramework.Runtime.DefaultLoadResourceAgentHelper";

        [SerializeField]
        private LoadResourceAgentHelperBase m_CustomLoadResourceAgentHelper = null;

        [SerializeField]
        private int m_LoadResourceAgentHelperCount = 3;

        /// <summary>
        /// 获取资源只读路径。
        /// </summary>
        public string ReadOnlyPath
        {
            get
            {
                return m_ResourceManager.ReadOnlyPath;
            }
        }

        /// <summary>
        /// 获取资源读写路径。
        /// </summary>
        public string ReadWritePath
        {
            get
            {
                return m_ResourceManager.ReadWritePath;
            }
        }

        /// <summary>
        /// 获取资源模式。
        /// </summary>
        public ResourceMode ResourceMode
        {
            get
            {
                return m_ResourceManager.ResourceMode;
            }
        }

        /// <summary>
        /// 获取资源读写路径类型。
        /// </summary>
        public ReadWritePathType ReadWritePathType
        {
            get
            {
                return m_ReadWritePathType;
            }
        }

        /// <summary>
        /// 设置当前变体。
        /// </summary>
        public string CurrentVariant
        {
            get
            {
                return m_ResourceManager.CurrentVariant;
            }
        }

        /// <summary>
        /// 获取或设置无用资源释放间隔时间。
        /// </summary>
        public float UnloadUnusedAssetsInterval
        {
            get
            {
                return m_UnloadUnusedAssetsInterval;
            }
            set
            {
                m_UnloadUnusedAssetsInterval = value;
            }
        }

        /// <summary>
        /// 获取当前资源适用的游戏版本号。
        /// </summary>
        public string ApplicableGameVersion
        {
            get
            {
                return m_ResourceManager.ApplicableGameVersion;
            }
        }

        /// <summary>
        /// 获取当前资源内部版本号。
        /// </summary>
        public int InternalResourceVersion
        {
            get
            {
                return m_ResourceManager.InternalResourceVersion;
            }
        }

        /// <summary>
        /// 获取已准备完毕资源数量。
        /// </summary>
        public int AssetCount
        {
            get
            {
                return m_ResourceManager.AssetCount;
            }
        }

        /// <summary>
        /// 获取已准备完毕资源数量。
        /// </summary>
        public int ResourceCount
        {
            get
            {
                return m_ResourceManager.ResourceCount;
            }
        }

        /// <summary>
        /// 获取资源组数量。
        /// </summary>
        public int ResourceGroupCount
        {
            get
            {
                return m_ResourceManager.ResourceGroupCount;
            }
        }

        /// <summary>
        /// 获取或设置资源更新下载地址。
        /// </summary>
        public string UpdatePrefixUri
        {
            get
            {
                return m_ResourceManager.UpdatePrefixUri;
            }
            set
            {
                m_ResourceManager.UpdatePrefixUri = m_UpdatePrefixUri = value;
            }
        }

        /// <summary>
        /// 获取或设置资源更新重试次数。
        /// </summary>
        public int UpdateRetryCount
        {
            get
            {
                return m_ResourceManager.UpdateRetryCount;
            }
            set
            {
                m_ResourceManager.UpdateRetryCount = m_UpdateRetryCount = value;
            }
        }

        /// <summary>
        /// 获取等待更新资源数量。
        /// </summary>
        public int UpdateWaitingCount
        {
            get
            {
                return m_ResourceManager.UpdateWaitingCount;
            }
        }

        /// <summary>
        /// 获取正在更新资源数量。
        /// </summary>
        public int UpdatingCount
        {
            get
            {
                return m_ResourceManager.UpdatingCount;
            }
        }

        /// <summary>
        /// 获取加载资源代理总数量。
        /// </summary>
        public int LoadTotalAgentCount
        {
            get
            {
                return m_ResourceManager.LoadTotalAgentCount;
            }
        }

        /// <summary>
        /// 获取可用加载资源代理数量。
        /// </summary>
        public int LoadFreeAgentCount
        {
            get
            {
                return m_ResourceManager.LoadFreeAgentCount;
            }
        }

        /// <summary>
        /// 获取工作中加载资源代理数量。
        /// </summary>
        public int LoadWorkingAgentCount
        {
            get
            {
                return m_ResourceManager.LoadWorkingAgentCount;
            }
        }

        /// <summary>
        /// 获取等待加载资源任务数量。
        /// </summary>
        public int LoadWaitingTaskCount
        {
            get
            {
                return m_ResourceManager.LoadWaitingTaskCount;
            }
        }

        /// <summary>
        /// 获取或设置资源对象池自动释放可释放对象的间隔秒数。
        /// </summary>
        public float AssetAutoReleaseInterval
        {
            get
            {
                return m_ResourceManager.AssetAutoReleaseInterval;
            }
            set
            {
                m_ResourceManager.AssetAutoReleaseInterval = m_AssetAutoReleaseInterval = value;
            }
        }

        /// <summary>
        /// 获取或设置资源对象池的容量。
        /// </summary>
        public int AssetCapacity
        {
            get
            {
                return m_ResourceManager.AssetCapacity;
            }
            set
            {
                m_ResourceManager.AssetCapacity = m_AssetCapacity = value;
            }
        }

        /// <summary>
        /// 获取或设置资源对象池对象过期秒数。
        /// </summary>
        public float AssetExpireTime
        {
            get
            {
                return m_ResourceManager.AssetExpireTime;
            }
            set
            {
                m_ResourceManager.AssetExpireTime = m_AssetExpireTime = value;
            }
        }

        /// <summary>
        /// 获取或设置资源对象池的优先级。
        /// </summary>
        public int AssetPriority
        {
            get
            {
                return m_ResourceManager.AssetPriority;
            }
            set
            {
                m_ResourceManager.AssetPriority = m_AssetPriority = value;
            }
        }

        /// <summary>
        /// 获取或设置资源对象池自动释放可释放对象的间隔秒数。
        /// </summary>
        public float ResourceAutoReleaseInterval
        {
            get
            {
                return m_ResourceManager.ResourceAutoReleaseInterval;
            }
            set
            {
                m_ResourceManager.ResourceAutoReleaseInterval = m_ResourceAutoReleaseInterval = value;
            }
        }

        /// <summary>
        /// 获取或设置资源对象池的容量。
        /// </summary>
        public int ResourceCapacity
        {
            get
            {
                return m_ResourceManager.ResourceCapacity;
            }
            set
            {
                m_ResourceManager.ResourceCapacity = m_ResourceCapacity = value;
            }
        }

        /// <summary>
        /// 获取或设置资源对象池对象过期秒数。
        /// </summary>
        public float ResourceExpireTime
        {
            get
            {
                return m_ResourceManager.ResourceExpireTime;
            }
            set
            {
                m_ResourceManager.ResourceExpireTime = m_ResourceExpireTime = value;
            }
        }

        /// <summary>
        /// 获取或设置资源对象池的优先级。
        /// </summary>
        public int ResourcePriority
        {
            get
            {
                return m_ResourceManager.ResourcePriority;
            }
            set
            {
                m_ResourceManager.ResourcePriority = m_ResourcePriority = value;
            }
        }

        /// <summary>
        /// 游戏框架组件初始化。
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            BaseComponent baseComponent = GameEntry.GetComponent<BaseComponent>();
            if (baseComponent == null)
            {
                Log.Fatal("Base component is invalid.");
                return;
            }

            m_EventComponent = GameEntry.GetComponent<EventComponent>();
            if (m_EventComponent == null)
            {
                Log.Fatal("Event component is invalid.");
                return;
            }

            m_EditorResourceMode = baseComponent.EditorResourceMode;
            m_ResourceManager = m_EditorResourceMode ? baseComponent.EditorResourceHelper : GameFrameworkEntry.GetModule<IResourceManager>();
            if (m_ResourceManager == null)
            {
                Log.Fatal("Resource manager is invalid.");
                return;
            }

            m_ResourceManager.ResourceInitComplete += OnResourceInitComplete;
            m_ResourceManager.VersionListUpdateSuccess += OnVersionListUpdateSuccess;
            m_ResourceManager.VersionListUpdateFailure += OnVersionListUpdateFailure;
            m_ResourceManager.ResourceCheckComplete += OnResourceCheckComplete;
            m_ResourceManager.ResourceUpdateStart += OnResourceUpdateStart;
            m_ResourceManager.ResourceUpdateChanged += OnResourceUpdateChanged;
            m_ResourceManager.ResourceUpdateSuccess += OnResourceUpdateSuccess;
            m_ResourceManager.ResourceUpdateFailure += OnResourceUpdateFailure;
            m_ResourceManager.ResourceUpdateAllComplete += OnResourceUpdateAllComplete;

            m_ResourceManager.SetReadOnlyPath(Application.streamingAssetsPath);
            if (m_ReadWritePathType == ReadWritePathType.TemporaryCache)
            {
                m_ResourceManager.SetReadWritePath(Application.temporaryCachePath);
            }
            else
            {
                if (m_ReadWritePathType == ReadWritePathType.Unspecified)
                {
                    m_ReadWritePathType = ReadWritePathType.PersistentData;
                }

                m_ResourceManager.SetReadWritePath(Application.persistentDataPath);
            }

            if (m_EditorResourceMode)
            {
                return;
            }

            SetResourceMode(m_ResourceMode);
            m_ResourceManager.SetDownloadManager(GameFrameworkEntry.GetModule<IDownloadManager>());
            m_ResourceManager.SetObjectPoolManager(GameFrameworkEntry.GetModule<IObjectPoolManager>());
            m_ResourceManager.AssetAutoReleaseInterval = m_AssetAutoReleaseInterval;
            m_ResourceManager.AssetCapacity = m_AssetCapacity;
            m_ResourceManager.AssetExpireTime = m_AssetExpireTime;
            m_ResourceManager.AssetPriority = m_AssetPriority;
            m_ResourceManager.ResourceAutoReleaseInterval = m_ResourceAutoReleaseInterval;
            m_ResourceManager.ResourceCapacity = m_ResourceCapacity;
            m_ResourceManager.ResourceExpireTime = m_ResourceExpireTime;
            m_ResourceManager.ResourcePriority = m_ResourcePriority;
            if (m_ResourceMode == ResourceMode.Updatable)
            {
                m_ResourceManager.UpdatePrefixUri = m_UpdatePrefixUri;
                m_ResourceManager.UpdateRetryCount = m_UpdateRetryCount;
            }

            m_ResourceHelper = Helper.CreateHelper(m_ResourceHelperTypeName, m_CustomResourceHelper);
            if (m_ResourceHelper == null)
            {
                Log.Error("Can not create resource helper.");
                return;
            }

            m_ResourceHelper.name = string.Format("Resource Helper");
            Transform transform = m_ResourceHelper.transform;
            transform.SetParent(this.transform);
            transform.localScale = Vector3.one;

            m_ResourceManager.SetResourceHelper(m_ResourceHelper);

            if (m_InstanceRoot == null)
            {
                m_InstanceRoot = (new GameObject("Load Resource Agent Instances")).transform;
                m_InstanceRoot.SetParent(gameObject.transform);
                m_InstanceRoot.localScale = Vector3.one;
            }

            for (int i = 0; i < m_LoadResourceAgentHelperCount; i++)
            {
                AddLoadResourceAgentHelper(i);
            }
        }

        private void Update()
        {
            m_LastOperationElapse += Time.unscaledDeltaTime;
            if (m_AsyncOperation == null && (m_ForceUnloadUnusedAssets || m_PreorderUnloadUnusedAssets && m_LastOperationElapse >= m_UnloadUnusedAssetsInterval))
            {
                Log.Debug("Unload unused assets...");
                m_ForceUnloadUnusedAssets = false;
                m_PreorderUnloadUnusedAssets = false;
                m_LastOperationElapse = 0f;
                m_AsyncOperation = Resources.UnloadUnusedAssets();
            }

            if (m_AsyncOperation != null && m_AsyncOperation.isDone)
            {
                m_AsyncOperation = null;
                if (m_PerformGCCollect)
                {
                    Log.Debug("GC.Collect...");
                    m_PerformGCCollect = false;
                    GC.Collect();
                }
            }
        }

        /// <summary>
        /// 设置资源模式。
        /// </summary>
        /// <param name="resourceMode">资源模式。</param>
        public void SetResourceMode(ResourceMode resourceMode)
        {
            m_ResourceManager.SetResourceMode(resourceMode);
        }

        /// <summary>
        /// 设置当前变体。
        /// </summary>
        /// <param name="currentVariant">当前变体。</param>
        public void SetCurrentVariant(string currentVariant)
        {
            m_ResourceManager.SetCurrentVariant(!string.IsNullOrEmpty(currentVariant) ? currentVariant : null);
        }

        /// <summary>
        /// 设置解密资源回调函数。
        /// </summary>
        /// <param name="decryptResourceCallback">要设置的解密资源回调函数。</param>
        /// <remarks>如果不设置，将使用默认的解密资源回调函数。</remarks>
        public void SetDecryptResourceCallback(DecryptResourceCallback decryptResourceCallback)
        {
            m_ResourceManager.SetDecryptResourceCallback(decryptResourceCallback);
        }

        /// <summary>
        /// 预订执行释放未被使用的资源。
        /// </summary>
        /// <param name="performGCCollect">是否使用垃圾回收。</param>
        public void UnloadUnusedAssets(bool performGCCollect)
        {
            m_PreorderUnloadUnusedAssets = true;
            if (performGCCollect)
            {
                m_PerformGCCollect = performGCCollect;
            }
        }

        /// <summary>
        /// 强制执行释放未被使用的资源。
        /// </summary>
        /// <param name="performGCCollect">是否使用垃圾回收。</param>
        public void ForceUnloadUnusedAssets(bool performGCCollect)
        {
            m_ForceUnloadUnusedAssets = true;
            if (performGCCollect)
            {
                m_PerformGCCollect = performGCCollect;
            }
        }

        /// <summary>
        /// 使用单机模式并初始化资源。
        /// </summary>
        public void InitResources()
        {
            m_ResourceManager.InitResources();
        }

        /// <summary>
        /// 使用可更新模式并检查版本资源列表。
        /// </summary>
        /// <param name="latestInternalResourceVersion">最新的资源内部版本号。</param>
        /// <returns>检查版本资源列表结果。</returns>
        public CheckVersionListResult CheckVersionList(int latestInternalResourceVersion)
        {
            return m_ResourceManager.CheckVersionList(latestInternalResourceVersion);
        }

        /// <summary>
        /// 使用可更新模式并更新版本资源列表。
        /// </summary>
        /// <param name="versionListLength">版本资源列表大小。</param>
        /// <param name="versionListHashCode">版本资源列表哈希值。</param>
        /// <param name="versionListZipLength">版本资源列表压缩后大小。</param>
        /// <param name="versionListZipHashCode">版本资源列表压缩后哈希值。</param>
        public void UpdateVersionList(int versionListLength, int versionListHashCode, int versionListZipLength, int versionListZipHashCode)
        {
            m_ResourceManager.UpdateVersionList(versionListLength, versionListHashCode, versionListZipLength, versionListZipHashCode);
        }

        /// <summary>
        /// 使用可更新模式并检查资源。
        /// </summary>
        public void CheckResources()
        {
            m_ResourceManager.CheckResources();
        }

        /// <summary>
        /// 使用可更新模式并更新资源。
        /// </summary>
        public void UpdateResources()
        {
            m_ResourceManager.UpdateResources();
        }

        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        public void LoadAsset(string assetName, LoadAssetCallbacks loadAssetCallbacks)
        {
            m_ResourceManager.LoadAsset(assetName, loadAssetCallbacks);
        }

        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void LoadAsset(string assetName, LoadAssetCallbacks loadAssetCallbacks, object userData)
        {
            m_ResourceManager.LoadAsset(assetName, loadAssetCallbacks, userData);
        }

        /// <summary>
        /// 卸载资源。
        /// </summary>
        /// <param name="asset">要卸载的资源。</param>
        public void UnloadAsset(object asset)
        {
            m_ResourceManager.UnloadAsset(asset);
        }

        /// <summary>
        /// 获取资源组是否准备完毕。
        /// </summary>
        /// <param name="resourceGroupName">要检查的资源组名称。</param>
        public bool GetResourceGroupReady(string resourceGroupName)
        {
            return m_ResourceManager.GetResourceGroupReady(resourceGroupName);
        }

        /// <summary>
        /// 获取资源组资源数量。
        /// </summary>
        /// <param name="resourceGroupName">要检查的资源组名称。</param>
        public int GetResourceGroupResourceCount(string resourceGroupName)
        {
            return m_ResourceManager.GetResourceGroupResourceCount(resourceGroupName);
        }

        /// <summary>
        /// 获取资源组已准备完成资源数量。
        /// </summary>
        /// <param name="resourceGroupName">要检查的资源组名称。</param>
        public int GetResourceGroupReadyResourceCount(string resourceGroupName)
        {
            return m_ResourceManager.GetResourceGroupReadyResourceCount(resourceGroupName);
        }

        /// <summary>
        /// 获取资源组总大小。
        /// </summary>
        /// <param name="resourceGroupName">要检查的资源组名称。</param>
        public int GetResourceGroupTotalLength(string resourceGroupName)
        {
            return m_ResourceManager.GetResourceGroupTotalLength(resourceGroupName);
        }

        /// <summary>
        /// 获取资源组已准备完成总大小。
        /// </summary>
        /// <param name="resourceGroupName">要检查的资源组名称。</param>
        public int GetResourceGroupTotalReadyLength(string resourceGroupName)
        {
            return m_ResourceManager.GetResourceGroupTotalReadyLength(resourceGroupName);
        }

        /// <summary>
        /// 获取资源组准备进度。
        /// </summary>
        /// <param name="resourceGroupName">要检查的资源组名称。</param>
        public float GetResourceGroupProgress(string resourceGroupName)
        {
            return m_ResourceManager.GetResourceGroupProgress(resourceGroupName);
        }

        /// <summary>
        /// 增加加载资源代理辅助器。
        /// </summary>
        /// <param name="index">加载资源代理辅助器索引。</param>
        private void AddLoadResourceAgentHelper(int index)
        {
            LoadResourceAgentHelperBase loadResourceAgentHelper = Helper.CreateHelper(m_LoadResourceAgentHelperTypeName, m_CustomLoadResourceAgentHelper, index);
            if (loadResourceAgentHelper == null)
            {
                Log.Error("Can not create load resource agent helper.");
                return;
            }

            loadResourceAgentHelper.name = string.Format("Load Resource Agent Helper - {0}", index.ToString());
            Transform transform = loadResourceAgentHelper.transform;
            transform.SetParent(m_InstanceRoot);
            transform.localScale = Vector3.one;

            m_ResourceManager.AddLoadResourceAgentHelper(loadResourceAgentHelper);
        }

        private void OnResourceInitComplete(object sender, GameFramework.Resource.ResourceInitCompleteEventArgs e)
        {
            m_EventComponent.Fire(this, new ResourceInitCompleteEventArgs(e));
        }

        private void OnVersionListUpdateSuccess(object sender, GameFramework.Resource.VersionListUpdateSuccessEventArgs e)
        {
            m_EventComponent.Fire(this, new VersionListUpdateSuccessEventArgs(e));
        }

        private void OnVersionListUpdateFailure(object sender, GameFramework.Resource.VersionListUpdateFailureEventArgs e)
        {
            m_EventComponent.Fire(this, new VersionListUpdateFailureEventArgs(e));
        }

        private void OnResourceCheckComplete(object sender, GameFramework.Resource.ResourceCheckCompleteEventArgs e)
        {
            m_EventComponent.Fire(this, new ResourceCheckCompleteEventArgs(e));
        }

        private void OnResourceUpdateStart(object sender, GameFramework.Resource.ResourceUpdateStartEventArgs e)
        {
            m_EventComponent.Fire(this, new ResourceUpdateStartEventArgs(e));
        }

        private void OnResourceUpdateChanged(object sender, GameFramework.Resource.ResourceUpdateChangedEventArgs e)
        {
            m_EventComponent.Fire(this, new ResourceUpdateChangedEventArgs(e));
        }

        private void OnResourceUpdateSuccess(object sender, GameFramework.Resource.ResourceUpdateSuccessEventArgs e)
        {
            m_EventComponent.Fire(this, new ResourceUpdateSuccessEventArgs(e));
        }

        private void OnResourceUpdateFailure(object sender, GameFramework.Resource.ResourceUpdateFailureEventArgs e)
        {
            m_EventComponent.Fire(this, new ResourceUpdateFailureEventArgs(e));
        }

        private void OnResourceUpdateAllComplete(object sender, GameFramework.Resource.ResourceUpdateAllCompleteEventArgs e)
        {
            m_EventComponent.Fire(this, new ResourceUpdateAllCompleteEventArgs(e));
        }
    }
}
