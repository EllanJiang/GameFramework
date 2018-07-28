//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.Download;
using GameFramework.ObjectPool;
using System;
using System.Collections.Generic;

namespace GameFramework.Resource
{
    /// <summary>
    /// 资源管理器。
    /// </summary>
    internal sealed partial class ResourceManager : GameFrameworkModule, IResourceManager
    {
        private static readonly char[] PackageListHeader = new char[] { 'E', 'L', 'P' };
        private static readonly char[] VersionListHeader = new char[] { 'E', 'L', 'V' };
        private static readonly char[] ReadOnlyListHeader = new char[] { 'E', 'L', 'R' };
        private static readonly char[] ReadWriteListHeader = new char[] { 'E', 'L', 'W' };
        private const string VersionListFileName = "version";
        private const string ResourceListFileName = "list";
        private const string BackupFileSuffixName = ".bak";
        private const byte ReadWriteListVersionHeader = 0;

        private readonly Dictionary<string, AssetInfo> m_AssetInfos;
        private readonly Dictionary<string, AssetDependencyInfo> m_AssetDependencyInfos;
        private readonly Dictionary<ResourceName, ResourceInfo> m_ResourceInfos;
        private readonly Dictionary<string, ResourceGroup> m_ResourceGroups;
        private readonly SortedDictionary<ResourceName, ReadWriteResourceInfo> m_ReadWriteResourceInfos;
        private ResourceIniter m_ResourceIniter;
        private VersionListProcessor m_VersionListProcessor;
        private ResourceChecker m_ResourceChecker;
        private ResourceUpdater m_ResourceUpdater;
        private ResourceLoader m_ResourceLoader;
        private IResourceHelper m_ResourceHelper;
        private string m_ReadOnlyPath;
        private string m_ReadWritePath;
        private ResourceMode m_ResourceMode;
        private bool m_RefuseSetCurrentVariant;
        private string m_CurrentVariant;
        private string m_UpdatePrefixUri;
        private string m_ApplicableGameVersion;
        private int m_InternalResourceVersion;
        private DecryptResourceCallback m_DecryptResourceCallback;
        private InitResourcesCompleteCallback m_InitResourcesCompleteCallback;
        private UpdateVersionListCallbacks m_UpdateVersionListCallbacks;
        private CheckResourcesCompleteCallback m_CheckResourcesCompleteCallback;
        private UpdateResourcesCompleteCallback m_UpdateResourcesCompleteCallback;
        private EventHandler<ResourceUpdateStartEventArgs> m_ResourceUpdateStartEventHandler;
        private EventHandler<ResourceUpdateChangedEventArgs> m_ResourceUpdateChangedEventHandler;
        private EventHandler<ResourceUpdateSuccessEventArgs> m_ResourceUpdateSuccessEventHandler;
        private EventHandler<ResourceUpdateFailureEventArgs> m_ResourceUpdateFailureEventHandler;

        /// <summary>
        /// 初始化资源管理器的新实例。
        /// </summary>
        public ResourceManager()
        {
            ResourceNameComparer resourceNameComparer = new ResourceNameComparer();
            m_AssetInfos = new Dictionary<string, AssetInfo>();
            m_AssetDependencyInfos = new Dictionary<string, AssetDependencyInfo>();
            m_ResourceInfos = new Dictionary<ResourceName, ResourceInfo>(resourceNameComparer);
            m_ResourceGroups = new Dictionary<string, ResourceGroup>();
            m_ReadWriteResourceInfos = new SortedDictionary<ResourceName, ReadWriteResourceInfo>(resourceNameComparer);

            m_ResourceIniter = null;
            m_VersionListProcessor = null;
            m_ResourceChecker = null;
            m_ResourceUpdater = null;
            m_ResourceLoader = new ResourceLoader(this);

            m_ResourceHelper = null;
            m_ReadOnlyPath = null;
            m_ReadWritePath = null;
            m_ResourceMode = ResourceMode.Unspecified;
            m_RefuseSetCurrentVariant = false;
            m_CurrentVariant = null;
            m_UpdatePrefixUri = null;
            m_ApplicableGameVersion = null;
            m_InternalResourceVersion = 0;
            m_DecryptResourceCallback = null;
            m_InitResourcesCompleteCallback = null;
            m_UpdateVersionListCallbacks = null;
            m_CheckResourcesCompleteCallback = null;
            m_UpdateResourcesCompleteCallback = null;

            m_ResourceUpdateStartEventHandler = null;
            m_ResourceUpdateChangedEventHandler = null;
            m_ResourceUpdateSuccessEventHandler = null;
            m_ResourceUpdateFailureEventHandler = null;
        }

        /// <summary>
        /// 获取游戏框架模块优先级。
        /// </summary>
        /// <remarks>优先级较高的模块会优先轮询，并且关闭操作会后进行。</remarks>
        internal override int Priority
        {
            get
            {
                return 70;
            }
        }

        /// <summary>
        /// 获取资源只读区路径。
        /// </summary>
        public string ReadOnlyPath
        {
            get
            {
                return m_ReadOnlyPath;
            }
        }

        /// <summary>
        /// 获取资源读写区路径。
        /// </summary>
        public string ReadWritePath
        {
            get
            {
                return m_ReadWritePath;
            }
        }

        /// <summary>
        /// 获取资源模式。
        /// </summary>
        public ResourceMode ResourceMode
        {
            get
            {
                return m_ResourceMode;
            }
        }

        /// <summary>
        /// 获取当前变体。
        /// </summary>
        public string CurrentVariant
        {
            get
            {
                return m_CurrentVariant;
            }
        }

        /// <summary>
        /// 获取当前资源适用的游戏版本号。
        /// </summary>
        public string ApplicableGameVersion
        {
            get
            {
                return m_ApplicableGameVersion;
            }
        }

        /// <summary>
        /// 获取当前内部资源版本号。
        /// </summary>
        public int InternalResourceVersion
        {
            get
            {
                return m_InternalResourceVersion;
            }
        }

        /// <summary>
        /// 获取已准备完毕资源数量。
        /// </summary>
        public int AssetCount
        {
            get
            {
                return m_AssetInfos.Count;
            }
        }

        /// <summary>
        /// 获取已准备完毕资源数量。
        /// </summary>
        public int ResourceCount
        {
            get
            {
                return m_ResourceInfos.Count;
            }
        }

        /// <summary>
        /// 获取资源组数量。
        /// </summary>
        public int ResourceGroupCount
        {
            get
            {
                return m_ResourceGroups.Count;
            }
        }

        /// <summary>
        /// 获取或设置资源更新下载地址前缀。
        /// </summary>
        public string UpdatePrefixUri
        {
            get
            {
                return m_UpdatePrefixUri;
            }
            set
            {
                m_UpdatePrefixUri = value;
            }
        }

        /// <summary>
        /// 获取或设置资源更新重试次数。
        /// </summary>
        public int UpdateRetryCount
        {
            get
            {
                return m_ResourceUpdater != null ? m_ResourceUpdater.RetryCount : 0;
            }
            set
            {
                if (m_ResourceUpdater == null)
                {
                    throw new GameFrameworkException("You can not use UpdateRetryCount at this time.");
                }

                m_ResourceUpdater.RetryCount = value;
            }
        }

        /// <summary>
        /// 获取等待更新资源数量。
        /// </summary>
        public int UpdateWaitingCount
        {
            get
            {
                return m_ResourceUpdater != null ? m_ResourceUpdater.UpdateWaitingCount : 0;
            }
        }

        /// <summary>
        /// 获取正在更新资源数量。
        /// </summary>
        public int UpdatingCount
        {
            get
            {
                return m_ResourceUpdater != null ? m_ResourceUpdater.UpdatingCount : 0;
            }
        }

        /// <summary>
        /// 获取加载资源代理总数量。
        /// </summary>
        public int LoadTotalAgentCount
        {
            get
            {
                return m_ResourceLoader.TotalAgentCount;
            }
        }

        /// <summary>
        /// 获取可用加载资源代理数量。
        /// </summary>
        public int LoadFreeAgentCount
        {
            get
            {
                return m_ResourceLoader.FreeAgentCount;
            }
        }

        /// <summary>
        /// 获取工作中加载资源代理数量。
        /// </summary>
        public int LoadWorkingAgentCount
        {
            get
            {
                return m_ResourceLoader.WorkingAgentCount;
            }
        }

        /// <summary>
        /// 获取等待加载资源任务数量。
        /// </summary>
        public int LoadWaitingTaskCount
        {
            get
            {
                return m_ResourceLoader.WaitingTaskCount;
            }
        }

        /// <summary>
        /// 获取或设置资源对象池自动释放可释放对象的间隔秒数。
        /// </summary>
        public float AssetAutoReleaseInterval
        {
            get
            {
                return m_ResourceLoader.AssetAutoReleaseInterval;
            }
            set
            {
                m_ResourceLoader.AssetAutoReleaseInterval = value;
            }
        }

        /// <summary>
        /// 获取或设置资源对象池的容量。
        /// </summary>
        public int AssetCapacity
        {
            get
            {
                return m_ResourceLoader.AssetCapacity;
            }
            set
            {
                m_ResourceLoader.AssetCapacity = value;
            }
        }

        /// <summary>
        /// 获取或设置资源对象池对象过期秒数。
        /// </summary>
        public float AssetExpireTime
        {
            get
            {
                return m_ResourceLoader.AssetExpireTime;
            }
            set
            {
                m_ResourceLoader.AssetExpireTime = value;
            }
        }

        /// <summary>
        /// 获取或设置资源对象池的优先级。
        /// </summary>
        public int AssetPriority
        {
            get
            {
                return m_ResourceLoader.AssetPriority;
            }
            set
            {
                m_ResourceLoader.AssetPriority = value;
            }
        }

        /// <summary>
        /// 获取或设置资源对象池自动释放可释放对象的间隔秒数。
        /// </summary>
        public float ResourceAutoReleaseInterval
        {
            get
            {
                return m_ResourceLoader.ResourceAutoReleaseInterval;
            }
            set
            {
                m_ResourceLoader.ResourceAutoReleaseInterval = value;
            }
        }

        /// <summary>
        /// 获取或设置资源对象池的容量。
        /// </summary>
        public int ResourceCapacity
        {
            get
            {
                return m_ResourceLoader.ResourceCapacity;
            }
            set
            {
                m_ResourceLoader.ResourceCapacity = value;
            }
        }

        /// <summary>
        /// 获取或设置资源对象池对象过期秒数。
        /// </summary>
        public float ResourceExpireTime
        {
            get
            {
                return m_ResourceLoader.ResourceExpireTime;
            }
            set
            {
                m_ResourceLoader.ResourceExpireTime = value;
            }
        }

        /// <summary>
        /// 获取或设置资源对象池的优先级。
        /// </summary>
        public int ResourcePriority
        {
            get
            {
                return m_ResourceLoader.ResourcePriority;
            }
            set
            {
                m_ResourceLoader.ResourcePriority = value;
            }
        }

        /// <summary>
        /// 资源更新开始事件。
        /// </summary>
        public event EventHandler<ResourceUpdateStartEventArgs> ResourceUpdateStart
        {
            add
            {
                m_ResourceUpdateStartEventHandler += value;
            }
            remove
            {
                m_ResourceUpdateStartEventHandler -= value;
            }
        }

        /// <summary>
        /// 资源更新改变事件。
        /// </summary>
        public event EventHandler<ResourceUpdateChangedEventArgs> ResourceUpdateChanged
        {
            add
            {
                m_ResourceUpdateChangedEventHandler += value;
            }
            remove
            {
                m_ResourceUpdateChangedEventHandler -= value;
            }
        }

        /// <summary>
        /// 资源更新成功事件。
        /// </summary>
        public event EventHandler<ResourceUpdateSuccessEventArgs> ResourceUpdateSuccess
        {
            add
            {
                m_ResourceUpdateSuccessEventHandler += value;
            }
            remove
            {
                m_ResourceUpdateSuccessEventHandler -= value;
            }
        }

        /// <summary>
        /// 资源更新失败事件。
        /// </summary>
        public event EventHandler<ResourceUpdateFailureEventArgs> ResourceUpdateFailure
        {
            add
            {
                m_ResourceUpdateFailureEventHandler += value;
            }
            remove
            {
                m_ResourceUpdateFailureEventHandler -= value;
            }
        }

        /// <summary>
        /// 资源管理器轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {
            if (m_ResourceUpdater != null)
            {
                m_ResourceUpdater.Update(elapseSeconds, realElapseSeconds);
            }

            m_ResourceLoader.Update(elapseSeconds, realElapseSeconds);
        }

        /// <summary>
        /// 关闭并清理资源管理器。
        /// </summary>
        internal override void Shutdown()
        {
            if (m_ResourceIniter != null)
            {
                m_ResourceIniter.Shutdown();
                m_ResourceIniter = null;
            }

            if (m_VersionListProcessor != null)
            {
                m_VersionListProcessor.VersionListUpdateSuccess -= OnVersionListProcessorUpdateSuccess;
                m_VersionListProcessor.VersionListUpdateFailure -= OnVersionListProcessorUpdateFailure;
                m_VersionListProcessor.Shutdown();
                m_VersionListProcessor = null;
            }

            if (m_ResourceChecker != null)
            {
                m_ResourceChecker.ResourceNeedUpdate -= OnCheckerResourceNeedUpdate;
                m_ResourceChecker.ResourceCheckComplete -= OnCheckerResourceCheckComplete;
                m_ResourceChecker.Shutdown();
                m_ResourceChecker = null;
            }

            if (m_ResourceUpdater != null)
            {
                m_ResourceUpdater.ResourceUpdateStart -= OnUpdaterResourceUpdateStart;
                m_ResourceUpdater.ResourceUpdateChanged -= OnUpdaterResourceUpdateChanged;
                m_ResourceUpdater.ResourceUpdateSuccess -= OnUpdaterResourceUpdateSuccess;
                m_ResourceUpdater.ResourceUpdateFailure -= OnUpdaterResourceUpdateFailure;
                m_ResourceUpdater.ResourceUpdateAllComplete -= OnUpdaterResourceUpdateAllComplete;
                m_ResourceUpdater.Shutdown();
                m_ResourceUpdater = null;
            }

            if (m_ResourceLoader != null)
            {
                m_ResourceLoader.Shutdown();
                m_ResourceLoader = null;
            }

            m_AssetInfos.Clear();
            m_AssetDependencyInfos.Clear();
            m_ResourceInfos.Clear();
            m_ResourceGroups.Clear();
            m_ReadWriteResourceInfos.Clear();
        }

        /// <summary>
        /// 设置资源只读区路径。
        /// </summary>
        /// <param name="readOnlyPath">资源只读区路径。</param>
        public void SetReadOnlyPath(string readOnlyPath)
        {
            if (string.IsNullOrEmpty(readOnlyPath))
            {
                throw new GameFrameworkException("Readonly path is invalid.");
            }

            if (m_ResourceLoader.TotalAgentCount > 0)
            {
                throw new GameFrameworkException("You must set readonly path before add load resource agent helper.");
            }

            m_ReadOnlyPath = readOnlyPath;
        }

        /// <summary>
        /// 设置资源读写区路径。
        /// </summary>
        /// <param name="readWritePath">资源读写区路径。</param>
        public void SetReadWritePath(string readWritePath)
        {
            if (string.IsNullOrEmpty(readWritePath))
            {
                throw new GameFrameworkException("Read-write path is invalid.");
            }

            if (m_ResourceLoader.TotalAgentCount > 0)
            {
                throw new GameFrameworkException("You must set read-write path before add load resource agent helper.");
            }

            m_ReadWritePath = readWritePath;
        }

        /// <summary>
        /// 设置资源模式。
        /// </summary>
        /// <param name="resourceMode">资源模式。</param>
        public void SetResourceMode(ResourceMode resourceMode)
        {
            if (resourceMode == ResourceMode.Unspecified)
            {
                throw new GameFrameworkException("Resource mode is invalid.");
            }

            if (m_ResourceMode == ResourceMode.Unspecified)
            {
                m_ResourceMode = resourceMode;

                if (m_ResourceMode == ResourceMode.Package)
                {
                    m_ResourceIniter = new ResourceIniter(this);
                    m_ResourceIniter.ResourceInitComplete += OnIniterResourceInitComplete;
                }
                else if (m_ResourceMode == ResourceMode.Updatable)
                {
                    m_VersionListProcessor = new VersionListProcessor(this);
                    m_VersionListProcessor.VersionListUpdateSuccess += OnVersionListProcessorUpdateSuccess;
                    m_VersionListProcessor.VersionListUpdateFailure += OnVersionListProcessorUpdateFailure;

                    m_ResourceChecker = new ResourceChecker(this);
                    m_ResourceChecker.ResourceNeedUpdate += OnCheckerResourceNeedUpdate;
                    m_ResourceChecker.ResourceCheckComplete += OnCheckerResourceCheckComplete;

                    m_ResourceUpdater = new ResourceUpdater(this);
                    m_ResourceUpdater.ResourceUpdateStart += OnUpdaterResourceUpdateStart;
                    m_ResourceUpdater.ResourceUpdateChanged += OnUpdaterResourceUpdateChanged;
                    m_ResourceUpdater.ResourceUpdateSuccess += OnUpdaterResourceUpdateSuccess;
                    m_ResourceUpdater.ResourceUpdateFailure += OnUpdaterResourceUpdateFailure;
                    m_ResourceUpdater.ResourceUpdateAllComplete += OnUpdaterResourceUpdateAllComplete;
                }
            }
            else if (m_ResourceMode != resourceMode)
            {
                throw new GameFrameworkException("You can not change resource mode at this time.");
            }
        }

        /// <summary>
        /// 设置当前变体。
        /// </summary>
        /// <param name="currentVariant">当前变体。</param>
        public void SetCurrentVariant(string currentVariant)
        {
            if (m_RefuseSetCurrentVariant)
            {
                throw new GameFrameworkException("You can net set current variant at this time.");
            }

            m_CurrentVariant = currentVariant;
        }

        /// <summary>
        /// 设置对象池管理器。
        /// </summary>
        /// <param name="objectPoolManager">对象池管理器。</param>
        public void SetObjectPoolManager(IObjectPoolManager objectPoolManager)
        {
            if (objectPoolManager == null)
            {
                throw new GameFrameworkException("Object pool manager is invalid.");
            }

            m_ResourceLoader.SetObjectPoolManager(objectPoolManager);
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

            if (m_VersionListProcessor != null)
            {
                m_VersionListProcessor.SetDownloadManager(downloadManager);
            }

            if (m_ResourceUpdater != null)
            {
                m_ResourceUpdater.SetDownloadManager(downloadManager);
            }
        }

        /// <summary>
        /// 设置解密资源回调函数。
        /// </summary>
        /// <param name="decryptResourceCallback">要设置的解密资源回调函数。</param>
        /// <remarks>如果不设置，将使用默认的解密资源回调函数。</remarks>
        public void SetDecryptResourceCallback(DecryptResourceCallback decryptResourceCallback)
        {
            if (m_ResourceLoader.TotalAgentCount > 0)
            {
                throw new GameFrameworkException("You must set decrypt resource callback before add load resource agent helper.");
            }

            m_DecryptResourceCallback = decryptResourceCallback;
        }

        /// <summary>
        /// 设置资源辅助器。
        /// </summary>
        /// <param name="resourceHelper">资源辅助器。</param>
        public void SetResourceHelper(IResourceHelper resourceHelper)
        {
            if (resourceHelper == null)
            {
                throw new GameFrameworkException("Resource helper is invalid.");
            }

            if (m_ResourceLoader.TotalAgentCount > 0)
            {
                throw new GameFrameworkException("You must set resource helper before add load resource agent helper.");
            }

            m_ResourceHelper = resourceHelper;
        }

        /// <summary>
        /// 增加加载资源代理辅助器。
        /// </summary>
        /// <param name="loadResourceAgentHelper">要增加的加载资源代理辅助器。</param>
        public void AddLoadResourceAgentHelper(ILoadResourceAgentHelper loadResourceAgentHelper)
        {
            m_ResourceLoader.AddLoadResourceAgentHelper(loadResourceAgentHelper, m_ResourceHelper, m_ReadOnlyPath, m_ReadWritePath, m_DecryptResourceCallback);
        }

        /// <summary>
        /// 使用单机模式并初始化资源。
        /// </summary>
        /// <param name="initResourcesCompleteCallback">使用单机模式并初始化资源完成的回调函数。</param>
        public void InitResources(InitResourcesCompleteCallback initResourcesCompleteCallback)
        {
            if (initResourcesCompleteCallback == null)
            {
                throw new GameFrameworkException("Init resources complete callback is invalid.");
            }

            if (m_ResourceMode == ResourceMode.Unspecified)
            {
                throw new GameFrameworkException("You must set resource mode first.");
            }

            if (m_ResourceMode != ResourceMode.Package)
            {
                throw new GameFrameworkException("You can not use InitResources without package resource mode.");
            }

            if (m_ResourceIniter == null)
            {
                throw new GameFrameworkException("You can not use InitResources at this time.");
            }

            m_RefuseSetCurrentVariant = true;
            m_InitResourcesCompleteCallback = initResourcesCompleteCallback;
            m_ResourceIniter.InitResources(m_CurrentVariant);
        }

        /// <summary>
        /// 使用可更新模式并检查版本资源列表。
        /// </summary>
        /// <param name="latestInternalResourceVersion">最新的内部资源版本号。</param>
        /// <returns>检查版本资源列表结果。</returns>
        public CheckVersionListResult CheckVersionList(int latestInternalResourceVersion)
        {
            if (m_ResourceMode == ResourceMode.Unspecified)
            {
                throw new GameFrameworkException("You must set resource mode first.");
            }

            if (m_ResourceMode != ResourceMode.Updatable)
            {
                throw new GameFrameworkException("You can not use InitResources without updatable resource mode.");
            }

            if (m_VersionListProcessor == null)
            {
                throw new GameFrameworkException("You can not use CheckVersionList at this time.");
            }

            return m_VersionListProcessor.CheckVersionList(latestInternalResourceVersion);
        }

        /// <summary>
        /// 使用可更新模式并更新版本资源列表。
        /// </summary>
        /// <param name="versionListLength">版本资源列表大小。</param>
        /// <param name="versionListHashCode">版本资源列表哈希值。</param>
        /// <param name="versionListZipLength">版本资源列表压缩后大小。</param>
        /// <param name="versionListZipHashCode">版本资源列表压缩后哈希值。</param>
        /// <param name="updateVersionListCallbacks">版本资源列表更新回调函数集。</param>
        public void UpdateVersionList(int versionListLength, int versionListHashCode, int versionListZipLength, int versionListZipHashCode, UpdateVersionListCallbacks updateVersionListCallbacks)
        {
            if (updateVersionListCallbacks == null)
            {
                throw new GameFrameworkException("Update version list callbacks is invalid.");
            }

            if (m_ResourceMode == ResourceMode.Unspecified)
            {
                throw new GameFrameworkException("You must set resource mode first.");
            }

            if (m_ResourceMode != ResourceMode.Updatable)
            {
                throw new GameFrameworkException("You can not use InitResources without updatable resource mode.");
            }

            if (m_VersionListProcessor == null)
            {
                throw new GameFrameworkException("You can not use UpdateVersionList at this time.");
            }

            m_UpdateVersionListCallbacks = updateVersionListCallbacks;
            m_VersionListProcessor.UpdateVersionList(versionListLength, versionListHashCode, versionListZipLength, versionListZipHashCode);
        }

        /// <summary>
        /// 使用可更新模式并检查资源。
        /// </summary>
        /// <param name="checkResourcesCompleteCallback">使用可更新模式并检查资源完成的回调函数。</param>
        public void CheckResources(CheckResourcesCompleteCallback checkResourcesCompleteCallback)
        {
            if (checkResourcesCompleteCallback == null)
            {
                throw new GameFrameworkException("Check resources complete callback is invalid.");
            }

            if (m_ResourceMode == ResourceMode.Unspecified)
            {
                throw new GameFrameworkException("You must set resource mode first.");
            }

            if (m_ResourceMode != ResourceMode.Updatable)
            {
                throw new GameFrameworkException("You can not use InitResources without updatable resource mode.");
            }

            if (m_ResourceChecker == null)
            {
                throw new GameFrameworkException("You can not use CheckResources at this time.");
            }

            m_RefuseSetCurrentVariant = true;
            m_CheckResourcesCompleteCallback = checkResourcesCompleteCallback;
            m_ResourceChecker.CheckResources(m_CurrentVariant);
        }

        /// <summary>
        /// 使用可更新模式并更新资源。
        /// </summary>
        /// <param name="updateResourcesCompleteCallback">使用可更新模式并更新资源全部完成的回调函数。</param>
        public void UpdateResources(UpdateResourcesCompleteCallback updateResourcesCompleteCallback)
        {
            if (updateResourcesCompleteCallback == null)
            {
                throw new GameFrameworkException("Update resources complete callback is invalid.");
            }

            if (m_ResourceMode == ResourceMode.Unspecified)
            {
                throw new GameFrameworkException("You must set resource mode first.");
            }

            if (m_ResourceMode != ResourceMode.Updatable)
            {
                throw new GameFrameworkException("You can not use InitResources without updatable resource mode.");
            }

            if (m_ResourceUpdater == null)
            {
                throw new GameFrameworkException("You can not use UpdateResources at this time.");
            }

            m_UpdateResourcesCompleteCallback = updateResourcesCompleteCallback;
            m_ResourceUpdater.UpdateResources();
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
                throw new GameFrameworkException("Asset name is invalid.");
            }

            return m_ResourceLoader.HasAsset(assetName);
        }

        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        public void LoadAsset(string assetName, LoadAssetCallbacks loadAssetCallbacks)
        {
            if (string.IsNullOrEmpty(assetName))
            {
                throw new GameFrameworkException("Asset name is invalid.");
            }

            if (loadAssetCallbacks == null)
            {
                throw new GameFrameworkException("Load asset callbacks is invalid.");
            }

            m_ResourceLoader.LoadAsset(assetName, null, Constant.DefaultPriority, loadAssetCallbacks, null);
        }

        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <param name="assetType">要加载资源的类型。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        public void LoadAsset(string assetName, Type assetType, LoadAssetCallbacks loadAssetCallbacks)
        {
            if (string.IsNullOrEmpty(assetName))
            {
                throw new GameFrameworkException("Asset name is invalid.");
            }

            if (loadAssetCallbacks == null)
            {
                throw new GameFrameworkException("Load asset callbacks is invalid.");
            }

            m_ResourceLoader.LoadAsset(assetName, assetType, Constant.DefaultPriority, loadAssetCallbacks, null);
        }

        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <param name="priority">加载资源的优先级。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        public void LoadAsset(string assetName, int priority, LoadAssetCallbacks loadAssetCallbacks)
        {
            if (string.IsNullOrEmpty(assetName))
            {
                throw new GameFrameworkException("Asset name is invalid.");
            }

            if (loadAssetCallbacks == null)
            {
                throw new GameFrameworkException("Load asset callbacks is invalid.");
            }

            m_ResourceLoader.LoadAsset(assetName, null, priority, loadAssetCallbacks, null);
        }

        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void LoadAsset(string assetName, LoadAssetCallbacks loadAssetCallbacks, object userData)
        {
            if (string.IsNullOrEmpty(assetName))
            {
                throw new GameFrameworkException("Asset name is invalid.");
            }

            if (loadAssetCallbacks == null)
            {
                throw new GameFrameworkException("Load asset callbacks is invalid.");
            }

            m_ResourceLoader.LoadAsset(assetName, null, Constant.DefaultPriority, loadAssetCallbacks, userData);
        }

        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <param name="assetType">要加载资源的类型。</param>
        /// <param name="priority">加载资源的优先级。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        public void LoadAsset(string assetName, Type assetType, int priority, LoadAssetCallbacks loadAssetCallbacks)
        {
            if (string.IsNullOrEmpty(assetName))
            {
                throw new GameFrameworkException("Asset name is invalid.");
            }

            if (loadAssetCallbacks == null)
            {
                throw new GameFrameworkException("Load asset callbacks is invalid.");
            }

            m_ResourceLoader.LoadAsset(assetName, assetType, priority, loadAssetCallbacks, null);
        }

        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <param name="assetType">要加载资源的类型。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void LoadAsset(string assetName, Type assetType, LoadAssetCallbacks loadAssetCallbacks, object userData)
        {
            if (string.IsNullOrEmpty(assetName))
            {
                throw new GameFrameworkException("Asset name is invalid.");
            }

            if (loadAssetCallbacks == null)
            {
                throw new GameFrameworkException("Load asset callbacks is invalid.");
            }

            m_ResourceLoader.LoadAsset(assetName, assetType, Constant.DefaultPriority, loadAssetCallbacks, userData);
        }

        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <param name="priority">加载资源的优先级。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void LoadAsset(string assetName, int priority, LoadAssetCallbacks loadAssetCallbacks, object userData)
        {
            if (string.IsNullOrEmpty(assetName))
            {
                throw new GameFrameworkException("Asset name is invalid.");
            }

            if (loadAssetCallbacks == null)
            {
                throw new GameFrameworkException("Load asset callbacks is invalid.");
            }

            m_ResourceLoader.LoadAsset(assetName, null, priority, loadAssetCallbacks, userData);
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
            if (string.IsNullOrEmpty(assetName))
            {
                throw new GameFrameworkException("Asset name is invalid.");
            }

            if (loadAssetCallbacks == null)
            {
                throw new GameFrameworkException("Load asset callbacks is invalid.");
            }

            m_ResourceLoader.LoadAsset(assetName, assetType, priority, loadAssetCallbacks, userData);
        }

        /// <summary>
        /// 卸载资源。
        /// </summary>
        /// <param name="asset">要卸载的资源。</param>
        public void UnloadAsset(object asset)
        {
            if (asset == null)
            {
                throw new GameFrameworkException("Asset is invalid.");
            }

            if (m_ResourceLoader == null)
            {
                return;
            }

            m_ResourceLoader.UnloadAsset(asset);
        }

        /// <summary>
        /// 异步加载场景。
        /// </summary>
        /// <param name="sceneAssetName">要加载场景资源的名称。</param>
        /// <param name="loadSceneCallbacks">加载场景回调函数集。</param>
        public void LoadScene(string sceneAssetName, LoadSceneCallbacks loadSceneCallbacks)
        {
            if (string.IsNullOrEmpty(sceneAssetName))
            {
                throw new GameFrameworkException("Scene asset name is invalid.");
            }

            if (loadSceneCallbacks == null)
            {
                throw new GameFrameworkException("Load scene callbacks is invalid.");
            }

            m_ResourceLoader.LoadScene(sceneAssetName, Constant.DefaultPriority, loadSceneCallbacks, null);
        }

        /// <summary>
        /// 异步加载场景。
        /// </summary>
        /// <param name="sceneAssetName">要加载场景资源的名称。</param>
        /// <param name="priority">加载场景资源的优先级。</param>
        /// <param name="loadSceneCallbacks">加载场景回调函数集。</param>
        public void LoadScene(string sceneAssetName, int priority, LoadSceneCallbacks loadSceneCallbacks)
        {
            if (string.IsNullOrEmpty(sceneAssetName))
            {
                throw new GameFrameworkException("Scene asset name is invalid.");
            }

            if (loadSceneCallbacks == null)
            {
                throw new GameFrameworkException("Load scene callbacks is invalid.");
            }

            m_ResourceLoader.LoadScene(sceneAssetName, priority, loadSceneCallbacks, null);
        }

        /// <summary>
        /// 异步加载场景。
        /// </summary>
        /// <param name="sceneAssetName">要加载场景资源的名称。</param>
        /// <param name="loadSceneCallbacks">加载场景回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void LoadScene(string sceneAssetName, LoadSceneCallbacks loadSceneCallbacks, object userData)
        {
            if (string.IsNullOrEmpty(sceneAssetName))
            {
                throw new GameFrameworkException("Scene asset name is invalid.");
            }

            if (loadSceneCallbacks == null)
            {
                throw new GameFrameworkException("Load scene callbacks is invalid.");
            }

            m_ResourceLoader.LoadScene(sceneAssetName, Constant.DefaultPriority, loadSceneCallbacks, userData);
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
            if (string.IsNullOrEmpty(sceneAssetName))
            {
                throw new GameFrameworkException("Scene asset name is invalid.");
            }

            if (loadSceneCallbacks == null)
            {
                throw new GameFrameworkException("Load scene callbacks is invalid.");
            }

            m_ResourceLoader.LoadScene(sceneAssetName, priority, loadSceneCallbacks, userData);
        }

        /// <summary>
        /// 异步卸载场景。
        /// </summary>
        /// <param name="sceneAssetName">要卸载场景资源的名称。</param>
        /// <param name="unloadSceneCallbacks">卸载场景回调函数集。</param>
        public void UnloadScene(string sceneAssetName, UnloadSceneCallbacks unloadSceneCallbacks)
        {
            if (string.IsNullOrEmpty(sceneAssetName))
            {
                throw new GameFrameworkException("Scene asset name is invalid.");
            }

            if (unloadSceneCallbacks == null)
            {
                throw new GameFrameworkException("Unload scene callbacks is invalid.");
            }

            m_ResourceLoader.UnloadScene(sceneAssetName, unloadSceneCallbacks, null);
        }

        /// <summary>
        /// 异步卸载场景。
        /// </summary>
        /// <param name="sceneAssetName">要卸载场景资源的名称。</param>
        /// <param name="unloadSceneCallbacks">卸载场景回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void UnloadScene(string sceneAssetName, UnloadSceneCallbacks unloadSceneCallbacks, object userData)
        {
            if (string.IsNullOrEmpty(sceneAssetName))
            {
                throw new GameFrameworkException("Scene asset name is invalid.");
            }

            if (unloadSceneCallbacks == null)
            {
                throw new GameFrameworkException("Unload scene callbacks is invalid.");
            }

            m_ResourceLoader.UnloadScene(sceneAssetName, unloadSceneCallbacks, userData);
        }

        /// <summary>
        /// 获取资源组是否准备完毕。
        /// </summary>
        /// <param name="resourceGroupName">要检查的资源组名称。</param>
        public bool GetResourceGroupReady(string resourceGroupName)
        {
            ResourceGroup resourceGroup = FindResourceGroup(resourceGroupName);
            if (resourceGroup == null)
            {
                throw new GameFrameworkException(string.Format("Can not find resource group '{0}'.", resourceGroupName));
            }

            return resourceGroup.Ready;
        }

        /// <summary>
        /// 获取资源组资源数量。
        /// </summary>
        /// <param name="resourceGroupName">要检查的资源组名称。</param>
        public int GetResourceGroupResourceCount(string resourceGroupName)
        {
            ResourceGroup resourceGroup = FindResourceGroup(resourceGroupName);
            if (resourceGroup == null)
            {
                throw new GameFrameworkException(string.Format("Can not find resource group '{0}'.", resourceGroupName));
            }

            return resourceGroup.ResourceCount;
        }

        /// <summary>
        /// 获取资源组已准备完成资源数量。
        /// </summary>
        /// <param name="resourceGroupName">要检查的资源组名称。</param>
        public int GetResourceGroupReadyResourceCount(string resourceGroupName)
        {
            ResourceGroup resourceGroup = FindResourceGroup(resourceGroupName);
            if (resourceGroup == null)
            {
                throw new GameFrameworkException(string.Format("Can not find resource group '{0}'.", resourceGroupName));
            }

            return resourceGroup.ReadyResourceCount;
        }

        /// <summary>
        /// 获取资源组总大小。
        /// </summary>
        /// <param name="resourceGroupName">要检查的资源组名称。</param>
        public int GetResourceGroupTotalLength(string resourceGroupName)
        {
            ResourceGroup resourceGroup = FindResourceGroup(resourceGroupName);
            if (resourceGroup == null)
            {
                throw new GameFrameworkException(string.Format("Can not find resource group '{0}'.", resourceGroupName));
            }

            return resourceGroup.TotalLength;
        }

        /// <summary>
        /// 获取资源组已准备完成总大小。
        /// </summary>
        /// <param name="resourceGroupName">要检查的资源组名称。</param>
        public int GetResourceGroupTotalReadyLength(string resourceGroupName)
        {
            ResourceGroup resourceGroup = FindResourceGroup(resourceGroupName);
            if (resourceGroup == null)
            {
                throw new GameFrameworkException(string.Format("Can not find resource group '{0}'.", resourceGroupName));
            }

            return resourceGroup.TotalReadyLength;
        }

        /// <summary>
        /// 获取资源组准备进度。
        /// </summary>
        /// <param name="resourceGroupName">要检查的资源组名称。</param>
        public float GetResourceGroupProgress(string resourceGroupName)
        {
            ResourceGroup resourceGroup = FindResourceGroup(resourceGroupName);
            if (resourceGroup == null)
            {
                throw new GameFrameworkException(string.Format("Can not find resource group '{0}'.", resourceGroupName));
            }

            return resourceGroup.Progress;
        }

        private AssetInfo? GetAssetInfo(string assetName)
        {
            if (string.IsNullOrEmpty(assetName))
            {
                throw new GameFrameworkException("Asset name is invalid.");
            }

            AssetInfo assetInfo = default(AssetInfo);
            if (m_AssetInfos.TryGetValue(assetName, out assetInfo))
            {
                return assetInfo;
            }

            return null;
        }

        private AssetDependencyInfo? GetAssetDependencyInfo(string assetName)
        {
            if (string.IsNullOrEmpty(assetName))
            {
                throw new GameFrameworkException("Asset name is invalid.");
            }

            AssetDependencyInfo assetDependencyInfo = default(AssetDependencyInfo);
            if (m_AssetDependencyInfos.TryGetValue(assetName, out assetDependencyInfo))
            {
                return assetDependencyInfo;
            }

            return null;
        }

        private ResourceInfo? GetResourceInfo(ResourceName resourceName)
        {
            ResourceInfo resourceInfo = default(ResourceInfo);
            if (m_ResourceInfos.TryGetValue(resourceName, out resourceInfo))
            {
                return resourceInfo;
            }

            return null;
        }

        private ResourceGroup FindResourceGroup(string name)
        {
            ResourceGroup resourceGroup = null;
            if (m_ResourceGroups.TryGetValue(name ?? string.Empty, out resourceGroup))
            {
                return resourceGroup;
            }

            return null;
        }

        private ResourceGroup GetResourceGroup(string name)
        {
            ResourceGroup resourceGroup = FindResourceGroup(name);
            if (resourceGroup != null)
            {
                return resourceGroup;
            }

            resourceGroup = new ResourceGroup(m_ResourceInfos);
            m_ResourceGroups.Add(name ?? string.Empty, resourceGroup);

            return resourceGroup;
        }

        private void OnIniterResourceInitComplete()
        {
            m_ResourceIniter.ResourceInitComplete -= OnIniterResourceInitComplete;
            m_ResourceIniter.Shutdown();
            m_ResourceIniter = null;

            m_InitResourcesCompleteCallback();
            m_InitResourcesCompleteCallback = null;
        }

        private void OnVersionListProcessorUpdateSuccess(string downloadPath, string downloadUri)
        {
            m_UpdateVersionListCallbacks.UpdateVersionListSuccessCallback(downloadPath, downloadUri);
        }

        private void OnVersionListProcessorUpdateFailure(string downloadUri, string errorMessage)
        {
            if (m_UpdateVersionListCallbacks.UpdateVersionListFailureCallback != null)
            {
                m_UpdateVersionListCallbacks.UpdateVersionListFailureCallback(downloadUri, errorMessage);
            }
        }

        private void OnCheckerResourceNeedUpdate(ResourceName resourceName, LoadType loadType, int length, int hashCode, int zipLength, int zipHashCode)
        {
            m_ResourceUpdater.AddResourceUpdate(resourceName, loadType, length, hashCode, zipLength, zipHashCode, Utility.Path.GetCombinePath(m_ReadWritePath, Utility.Path.GetResourceNameWithSuffix(resourceName.FullName)), Utility.Path.GetRemotePath(m_UpdatePrefixUri, Utility.Path.GetResourceNameWithCrc32AndSuffix(resourceName.FullName, hashCode)), 0);
        }

        private void OnCheckerResourceCheckComplete(int removedCount, int updateCount, int updateTotalLength, int updateTotalZipLength)
        {
            m_VersionListProcessor.VersionListUpdateSuccess -= OnVersionListProcessorUpdateSuccess;
            m_VersionListProcessor.VersionListUpdateFailure -= OnVersionListProcessorUpdateFailure;
            m_VersionListProcessor.Shutdown();
            m_VersionListProcessor = null;
            m_UpdateVersionListCallbacks = null;

            m_ResourceChecker.ResourceNeedUpdate -= OnCheckerResourceNeedUpdate;
            m_ResourceChecker.ResourceCheckComplete -= OnCheckerResourceCheckComplete;
            m_ResourceChecker.Shutdown();
            m_ResourceChecker = null;

            m_ResourceUpdater.CheckResourceComplete(removedCount > 0);

            if (updateCount <= 0)
            {
                m_ResourceUpdater.ResourceUpdateStart -= OnUpdaterResourceUpdateStart;
                m_ResourceUpdater.ResourceUpdateChanged -= OnUpdaterResourceUpdateChanged;
                m_ResourceUpdater.ResourceUpdateSuccess -= OnUpdaterResourceUpdateSuccess;
                m_ResourceUpdater.ResourceUpdateFailure -= OnUpdaterResourceUpdateFailure;
                m_ResourceUpdater.ResourceUpdateAllComplete -= OnUpdaterResourceUpdateAllComplete;
                m_ResourceUpdater.Shutdown();
                m_ResourceUpdater = null;
            }

            m_CheckResourcesCompleteCallback(updateCount > 0, removedCount, updateCount, updateTotalLength, updateTotalZipLength);
            m_CheckResourcesCompleteCallback = null;
        }

        private void OnUpdaterResourceUpdateStart(ResourceName resourceName, string downloadPath, string downloadUri, int currentLength, int zipLength, int retryCount)
        {
            if (m_ResourceUpdateStartEventHandler != null)
            {
                m_ResourceUpdateStartEventHandler(this, new ResourceUpdateStartEventArgs(resourceName.FullName, downloadPath, downloadUri, currentLength, zipLength, retryCount));
            }
        }

        private void OnUpdaterResourceUpdateChanged(ResourceName resourceName, string downloadPath, string downloadUri, int currentLength, int zipLength)
        {
            if (m_ResourceUpdateChangedEventHandler != null)
            {
                m_ResourceUpdateChangedEventHandler(this, new ResourceUpdateChangedEventArgs(resourceName.FullName, downloadPath, downloadUri, currentLength, zipLength));
            }
        }

        private void OnUpdaterResourceUpdateSuccess(ResourceName resourceName, string downloadPath, string downloadUri, int length, int zipLength)
        {
            if (m_ResourceUpdateSuccessEventHandler != null)
            {
                m_ResourceUpdateSuccessEventHandler(this, new ResourceUpdateSuccessEventArgs(resourceName.FullName, downloadPath, downloadUri, length, zipLength));
            }
        }

        private void OnUpdaterResourceUpdateFailure(ResourceName resourceName, string downloadUri, int retryCount, int totalRetryCount, string errorMessage)
        {
            if (m_ResourceUpdateFailureEventHandler != null)
            {
                m_ResourceUpdateFailureEventHandler(this, new ResourceUpdateFailureEventArgs(resourceName.FullName, downloadUri, retryCount, totalRetryCount, errorMessage));
            }
        }

        private void OnUpdaterResourceUpdateAllComplete()
        {
            m_ResourceUpdater.ResourceUpdateStart -= OnUpdaterResourceUpdateStart;
            m_ResourceUpdater.ResourceUpdateChanged -= OnUpdaterResourceUpdateChanged;
            m_ResourceUpdater.ResourceUpdateSuccess -= OnUpdaterResourceUpdateSuccess;
            m_ResourceUpdater.ResourceUpdateFailure -= OnUpdaterResourceUpdateFailure;
            m_ResourceUpdater.ResourceUpdateAllComplete -= OnUpdaterResourceUpdateAllComplete;
            m_ResourceUpdater.Shutdown();
            m_ResourceUpdater = null;

            m_UpdateResourcesCompleteCallback();
            m_UpdateResourcesCompleteCallback = null;
        }
    }
}
