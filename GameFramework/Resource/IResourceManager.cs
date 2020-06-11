//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework.Download;
using GameFramework.ObjectPool;
using System;

namespace GameFramework.Resource
{
    /// <summary>
    /// 资源管理器接口。
    /// </summary>
    public interface IResourceManager
    {
        /// <summary>
        /// 获取资源只读区路径。
        /// </summary>
        string ReadOnlyPath
        {
            get;
        }

        /// <summary>
        /// 获取资源读写区路径。
        /// </summary>
        string ReadWritePath
        {
            get;
        }

        /// <summary>
        /// 获取资源模式。
        /// </summary>
        ResourceMode ResourceMode
        {
            get;
        }

        /// <summary>
        /// 获取当前变体。
        /// </summary>
        string CurrentVariant
        {
            get;
        }

        /// <summary>
        /// 获取单机模式版本资源列表序列化器。
        /// </summary>
        PackageVersionListSerializer PackageVersionListSerializer
        {
            get;
        }

        /// <summary>
        /// 获取可更新模式版本资源列表序列化器。
        /// </summary>
        UpdatableVersionListSerializer UpdatableVersionListSerializer
        {
            get;
        }

        /// <summary>
        /// 获取本地只读区版本资源列表序列化器。
        /// </summary>
        ReadOnlyVersionListSerializer ReadOnlyVersionListSerializer
        {
            get;
        }

        /// <summary>
        /// 获取本地读写区版本资源列表序列化器。
        /// </summary>
        ReadWriteVersionListSerializer ReadWriteVersionListSerializer
        {
            get;
        }

        /// <summary>
        /// 获取资源包版本资源列表序列化器。
        /// </summary>
        ResourcePackVersionListSerializer ResourcePackVersionListSerializer
        {
            get;
        }

        /// <summary>
        /// 获取当前资源适用的游戏版本号。
        /// </summary>
        string ApplicableGameVersion
        {
            get;
        }

        /// <summary>
        /// 获取当前内部资源版本号。
        /// </summary>
        int InternalResourceVersion
        {
            get;
        }

        /// <summary>
        /// 获取已准备完毕资源数量。
        /// </summary>
        int AssetCount
        {
            get;
        }

        /// <summary>
        /// 获取已准备完毕资源数量。
        /// </summary>
        int ResourceCount
        {
            get;
        }

        /// <summary>
        /// 获取资源组数量。
        /// </summary>
        int ResourceGroupCount
        {
            get;
        }

        /// <summary>
        /// 获取或设置资源更新下载地址。
        /// </summary>
        string UpdatePrefixUri
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置每更新多少字节的资源，重新生成一次版本资源列表。
        /// </summary>
        int GenerateReadWriteVersionListLength
        {
            get;
            set;
        }

        /// <summary>
        /// 获取正在应用的资源包路径。
        /// </summary>
        string ApplyingResourcePackPath
        {
            get;
        }

        /// <summary>
        /// 获取等待应用资源数量。
        /// </summary>
        int ApplyWaitingCount
        {
            get;
        }

        /// <summary>
        /// 获取或设置资源更新重试次数。
        /// </summary>
        int UpdateRetryCount
        {
            get;
            set;
        }

        /// <summary>
        /// 获取正在更新的资源组。
        /// </summary>
        IResourceGroup UpdatingResourceGroup
        {
            get;
        }

        /// <summary>
        /// 获取等待更新资源数量。
        /// </summary>
        int UpdateWaitingCount
        {
            get;
        }

        /// <summary>
        /// 获取候选更新资源数量。
        /// </summary>
        int UpdateCandidateCount
        {
            get;
        }

        /// <summary>
        /// 获取正在更新资源数量。
        /// </summary>
        int UpdatingCount
        {
            get;
        }

        /// <summary>
        /// 获取加载资源代理总数量。
        /// </summary>
        int LoadTotalAgentCount
        {
            get;
        }

        /// <summary>
        /// 获取可用加载资源代理数量。
        /// </summary>
        int LoadFreeAgentCount
        {
            get;
        }

        /// <summary>
        /// 获取工作中加载资源代理数量。
        /// </summary>
        int LoadWorkingAgentCount
        {
            get;
        }

        /// <summary>
        /// 获取等待加载资源任务数量。
        /// </summary>
        int LoadWaitingTaskCount
        {
            get;
        }

        /// <summary>
        /// 获取或设置资源对象池自动释放可释放对象的间隔秒数。
        /// </summary>
        float AssetAutoReleaseInterval
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置资源对象池的容量。
        /// </summary>
        int AssetCapacity
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置资源对象池对象过期秒数。
        /// </summary>
        float AssetExpireTime
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置资源对象池的优先级。
        /// </summary>
        int AssetPriority
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置资源对象池自动释放可释放对象的间隔秒数。
        /// </summary>
        float ResourceAutoReleaseInterval
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置资源对象池的容量。
        /// </summary>
        int ResourceCapacity
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置资源对象池对象过期秒数。
        /// </summary>
        float ResourceExpireTime
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置资源对象池的优先级。
        /// </summary>
        int ResourcePriority
        {
            get;
            set;
        }

        /// <summary>
        /// 资源应用成功事件。
        /// </summary>
        event EventHandler<ResourceApplySuccessEventArgs> ResourceApplySuccess;

        /// <summary>
        /// 资源应用失败事件。
        /// </summary>
        event EventHandler<ResourceApplyFailureEventArgs> ResourceApplyFailure;

        /// <summary>
        /// 资源更新开始事件。
        /// </summary>
        event EventHandler<ResourceUpdateStartEventArgs> ResourceUpdateStart;

        /// <summary>
        /// 资源更新改变事件。
        /// </summary>
        event EventHandler<ResourceUpdateChangedEventArgs> ResourceUpdateChanged;

        /// <summary>
        /// 资源更新成功事件。
        /// </summary>
        event EventHandler<ResourceUpdateSuccessEventArgs> ResourceUpdateSuccess;

        /// <summary>
        /// 资源更新失败事件。
        /// </summary>
        event EventHandler<ResourceUpdateFailureEventArgs> ResourceUpdateFailure;

        /// <summary>
        /// 设置资源只读区路径。
        /// </summary>
        /// <param name="readOnlyPath">资源只读区路径。</param>
        void SetReadOnlyPath(string readOnlyPath);

        /// <summary>
        /// 设置资源读写区路径。
        /// </summary>
        /// <param name="readWritePath">资源读写区路径。</param>
        void SetReadWritePath(string readWritePath);

        /// <summary>
        /// 设置资源模式。
        /// </summary>
        /// <param name="resourceMode">资源模式。</param>
        void SetResourceMode(ResourceMode resourceMode);

        /// <summary>
        /// 设置当前变体。
        /// </summary>
        /// <param name="currentVariant">当前变体。</param>
        void SetCurrentVariant(string currentVariant);

        /// <summary>
        /// 设置对象池管理器。
        /// </summary>
        /// <param name="objectPoolManager">对象池管理器。</param>
        void SetObjectPoolManager(IObjectPoolManager objectPoolManager);

        /// <summary>
        /// 设置下载管理器。
        /// </summary>
        /// <param name="downloadManager">下载管理器。</param>
        void SetDownloadManager(IDownloadManager downloadManager);

        /// <summary>
        /// 设置解密资源回调函数。
        /// </summary>
        /// <param name="decryptResourceCallback">要设置的解密资源回调函数。</param>
        /// <remarks>如果不设置，将使用默认的解密资源回调函数。</remarks>
        void SetDecryptResourceCallback(DecryptResourceCallback decryptResourceCallback);

        /// <summary>
        /// 设置资源辅助器。
        /// </summary>
        /// <param name="resourceHelper">资源辅助器。</param>
        void SetResourceHelper(IResourceHelper resourceHelper);

        /// <summary>
        /// 增加加载资源代理辅助器。
        /// </summary>
        /// <param name="loadResourceAgentHelper">要增加的加载资源代理辅助器。</param>
        void AddLoadResourceAgentHelper(ILoadResourceAgentHelper loadResourceAgentHelper);

        /// <summary>
        /// 使用单机模式并初始化资源。
        /// </summary>
        /// <param name="initResourcesCompleteCallback">使用单机模式并初始化资源完成时的回调函数。</param>
        void InitResources(InitResourcesCompleteCallback initResourcesCompleteCallback);

        /// <summary>
        /// 使用可更新模式并检查版本资源列表。
        /// </summary>
        /// <param name="latestInternalResourceVersion">最新的内部资源版本号。</param>
        /// <returns>检查版本资源列表结果。</returns>
        CheckVersionListResult CheckVersionList(int latestInternalResourceVersion);

        /// <summary>
        /// 使用可更新模式并更新版本资源列表。
        /// </summary>
        /// <param name="versionListLength">版本资源列表大小。</param>
        /// <param name="versionListHashCode">版本资源列表哈希值。</param>
        /// <param name="versionListZipLength">版本资源列表压缩后大小。</param>
        /// <param name="versionListZipHashCode">版本资源列表压缩后哈希值。</param>
        /// <param name="updateVersionListCallbacks">版本资源列表更新回调函数集。</param>
        void UpdateVersionList(int versionListLength, int versionListHashCode, int versionListZipLength, int versionListZipHashCode, UpdateVersionListCallbacks updateVersionListCallbacks);

        /// <summary>
        /// 使用可更新模式并检查资源。
        /// </summary>
        /// <param name="checkResourcesCompleteCallback">使用可更新模式并检查资源完成时的回调函数。</param>
        void CheckResources(CheckResourcesCompleteCallback checkResourcesCompleteCallback);

        /// <summary>
        /// 使用可更新模式并应用资源包资源。
        /// </summary>
        /// <param name="resourcePackPath">要应用的资源包路径。</param>
        /// <param name="applyResourcesCompleteCallback">使用可更新模式并应用资源包资源完成时的回调函数。</param>
        void ApplyResources(string resourcePackPath, ApplyResourcesCompleteCallback applyResourcesCompleteCallback);

        /// <summary>
        /// 使用可更新模式并更新全部资源。
        /// </summary>
        /// <param name="updateResourcesCompleteCallback">使用可更新模式并更新默认资源组完成时的回调函数。</param>
        void UpdateResources(UpdateResourcesCompleteCallback updateResourcesCompleteCallback);

        /// <summary>
        /// 使用可更新模式并更新指定资源组的资源。
        /// </summary>
        /// <param name="resourceGroupName">要更新的资源组名称。</param>
        /// <param name="updateResourcesCompleteCallback">使用可更新模式并更新指定资源组完成时的回调函数。</param>
        void UpdateResources(string resourceGroupName, UpdateResourcesCompleteCallback updateResourcesCompleteCallback);

        /// <summary>
        /// 校验资源包。
        /// </summary>
        /// <param name="resourcePackPath">要校验的资源包路径。</param>
        /// <returns>是否校验资源包成功。</returns>
        bool VerifyResourcePack(string resourcePackPath);

        /// <summary>
        /// 检查资源是否存在。
        /// </summary>
        /// <param name="assetName">要检查资源的名称。</param>
        /// <returns>检查资源是否存在的结果。</returns>
        HasAssetResult HasAsset(string assetName);

        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        void LoadAsset(string assetName, LoadAssetCallbacks loadAssetCallbacks);

        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <param name="assetType">要加载资源的类型。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        void LoadAsset(string assetName, Type assetType, LoadAssetCallbacks loadAssetCallbacks);

        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <param name="priority">加载资源的优先级。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        void LoadAsset(string assetName, int priority, LoadAssetCallbacks loadAssetCallbacks);

        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        void LoadAsset(string assetName, LoadAssetCallbacks loadAssetCallbacks, object userData);

        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <param name="assetType">要加载资源的类型。</param>
        /// <param name="priority">加载资源的优先级。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        void LoadAsset(string assetName, Type assetType, int priority, LoadAssetCallbacks loadAssetCallbacks);

        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <param name="assetType">要加载资源的类型。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        void LoadAsset(string assetName, Type assetType, LoadAssetCallbacks loadAssetCallbacks, object userData);

        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <param name="priority">加载资源的优先级。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        void LoadAsset(string assetName, int priority, LoadAssetCallbacks loadAssetCallbacks, object userData);

        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <param name="assetType">要加载资源的类型。</param>
        /// <param name="priority">加载资源的优先级。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        void LoadAsset(string assetName, Type assetType, int priority, LoadAssetCallbacks loadAssetCallbacks, object userData);

        /// <summary>
        /// 卸载资源。
        /// </summary>
        /// <param name="asset">要卸载的资源。</param>
        void UnloadAsset(object asset);

        /// <summary>
        /// 异步加载场景。
        /// </summary>
        /// <param name="sceneAssetName">要加载场景资源的名称。</param>
        /// <param name="loadSceneCallbacks">加载场景回调函数集。</param>
        void LoadScene(string sceneAssetName, LoadSceneCallbacks loadSceneCallbacks);

        /// <summary>
        /// 异步加载场景。
        /// </summary>
        /// <param name="sceneAssetName">要加载场景资源的名称。</param>
        /// <param name="priority">加载场景资源的优先级。</param>
        /// <param name="loadSceneCallbacks">加载场景回调函数集。</param>
        void LoadScene(string sceneAssetName, int priority, LoadSceneCallbacks loadSceneCallbacks);

        /// <summary>
        /// 异步加载场景。
        /// </summary>
        /// <param name="sceneAssetName">要加载场景资源的名称。</param>
        /// <param name="loadSceneCallbacks">加载场景回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        void LoadScene(string sceneAssetName, LoadSceneCallbacks loadSceneCallbacks, object userData);

        /// <summary>
        /// 异步加载场景。
        /// </summary>
        /// <param name="sceneAssetName">要加载场景资源的名称。</param>
        /// <param name="priority">加载场景资源的优先级。</param>
        /// <param name="loadSceneCallbacks">加载场景回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        void LoadScene(string sceneAssetName, int priority, LoadSceneCallbacks loadSceneCallbacks, object userData);

        /// <summary>
        /// 异步卸载场景。
        /// </summary>
        /// <param name="sceneAssetName">要卸载场景资源的名称。</param>
        /// <param name="unloadSceneCallbacks">卸载场景回调函数集。</param>
        void UnloadScene(string sceneAssetName, UnloadSceneCallbacks unloadSceneCallbacks);

        /// <summary>
        /// 异步卸载场景。
        /// </summary>
        /// <param name="sceneAssetName">要卸载场景资源的名称。</param>
        /// <param name="unloadSceneCallbacks">卸载场景回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        void UnloadScene(string sceneAssetName, UnloadSceneCallbacks unloadSceneCallbacks, object userData);

        /// <summary>
        /// 获取二进制资源的实际路径。
        /// </summary>
        /// <param name="binaryAssetName">要获取实际路径的二进制资源的名称。</param>
        /// <returns>二进制资源的实际路径。</returns>
        string GetBinaryPath(string binaryAssetName);

        /// <summary>
        /// 获取二进制资源的实际路径。
        /// </summary>
        /// <param name="binaryAssetName">要获取实际路径的二进制资源的名称。</param>
        /// <param name="storageInReadOnly">资源是否在只读区。</param>
        /// <param name="relativePath">二进制资源相对于只读区或者读写区的相对路径。</param>
        /// <returns>获取二进制资源的实际路径是否成功。</returns>
        bool GetBinaryPath(string binaryAssetName, out bool storageInReadOnly, out string relativePath);

        /// <summary>
        /// 异步加载二进制资源。
        /// </summary>
        /// <param name="binaryAssetName">要加载二进制资源的名称。</param>
        /// <param name="loadBinaryCallbacks">加载二进制资源回调函数集。</param>
        void LoadBinary(string binaryAssetName, LoadBinaryCallbacks loadBinaryCallbacks);

        /// <summary>
        /// 异步加载二进制资源。
        /// </summary>
        /// <param name="binaryAssetName">要加载二进制资源的名称。</param>
        /// <param name="loadBinaryCallbacks">加载二进制资源回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        void LoadBinary(string binaryAssetName, LoadBinaryCallbacks loadBinaryCallbacks, object userData);

        /// <summary>
        /// 检查资源组是否存在。
        /// </summary>
        /// <param name="resourceGroupName">要检查资源组的名称。</param>
        /// <returns>资源组是否存在。</returns>
        bool HasResourceGroup(string resourceGroupName);

        /// <summary>
        /// 获取默认资源组。
        /// </summary>
        /// <returns>默认资源组。</returns>
        IResourceGroup GetResourceGroup();

        /// <summary>
        /// 获取资源组。
        /// </summary>
        /// <param name="resourceGroupName">要获取的资源组名称。</param>
        /// <returns>要获取的资源组。</returns>
        IResourceGroup GetResourceGroup(string resourceGroupName);

        /// <summary>
        /// 获取所有加载资源任务的信息。
        /// </summary>
        /// <returns>所有加载资源任务的信息。</returns>
        TaskInfo[] GetAllLoadAssetInfos();
    }
}
