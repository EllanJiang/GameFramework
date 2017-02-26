//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 事件类型编号。
    /// </summary>
    public enum EventId
    {
        /// <summary>
        /// 资源初始化完成事件。
        /// </summary>
        ResourceInitComplete,

        /// <summary>
        /// 版本资源列表更新成功事件。
        /// </summary>
        VersionListUpdateSuccess,

        /// <summary>
        /// 版本资源列表更新失败事件。
        /// </summary>
        VersionListUpdateFailure,

        /// <summary>
        /// 资源检查完成事件。
        /// </summary>
        ResourceCheckComplete,

        /// <summary>
        /// 资源更新开始事件。
        /// </summary>
        ResourceUpdateStart,

        /// <summary>
        /// 资源更新改变事件。
        /// </summary>
        ResourceUpdateChanged,

        /// <summary>
        /// 资源更新成功事件。
        /// </summary>
        ResourceUpdateSuccess,

        /// <summary>
        /// 资源更新失败事件。
        /// </summary>
        ResourceUpdateFailure,

        /// <summary>
        /// 资源更新全部完成事件。
        /// </summary>
        ResourceUpdateAllComplete,

        /// <summary>
        /// 下载开始事件。
        /// </summary>
        DownloadStart,

        /// <summary>
        /// 下载更新事件。
        /// </summary>
        DownloadUpdate,

        /// <summary>
        /// 下载成功事件。
        /// </summary>
        DownloadSuccess,

        /// <summary>
        /// 下载失败事件。
        /// </summary>
        DownloadFailure,

        /// <summary>
        /// Web 请求开始事件。
        /// </summary>
        WebRequestStart,

        /// <summary>
        /// Web 请求成功事件。
        /// </summary>
        WebRequestSuccess,

        /// <summary>
        /// Web 请求失败事件。
        /// </summary>
        WebRequestFailure,

        /// <summary>
        /// 网络连接成功事件。
        /// </summary>
        NetworkConnected,

        /// <summary>
        /// 网络连接关闭事件。
        /// </summary>
        NetworkClosed,

        /// <summary>
        /// 发送网络消息包事件。
        /// </summary>
        NetworkSendPacket,

        /// <summary>
        /// 网络心跳包丢失事件。
        /// </summary>
        NetworkMissHeartBeat,

        /// <summary>
        /// 网络错误事件。
        /// </summary>
        NetworkError,

        /// <summary>
        /// 用户自定义网络错误事件。
        /// </summary>
        NetworkCustomError,

        /// <summary>
        /// 加载数据表成功事件。
        /// </summary>
        LoadDataTableSuccess,

        /// <summary>
        /// 加载数据表失败事件。
        /// </summary>
        LoadDataTableFailure,

        /// <summary>
        /// 加载数据表更新事件。
        /// </summary>
        LoadDataTableUpdate,

        /// <summary>
        /// 加载数据表时加载依赖资源事件。
        /// </summary>
        LoadDataTableDependencyAsset,

        /// <summary>
        /// 加载字典成功事件。
        /// </summary>
        LoadDictionarySuccess,

        /// <summary>
        /// 加载字典失败事件。
        /// </summary>
        LoadDictionaryFailure,

        /// <summary>
        /// 加载字典更新事件。
        /// </summary>
        LoadDictionaryUpdate,

        /// <summary>
        /// 加载字典时加载依赖资源事件。
        /// </summary>
        LoadDictionaryDependencyAsset,

        /// <summary>
        /// 加载场景成功事件。
        /// </summary>
        LoadSceneSuccess,

        /// <summary>
        /// 加载场景失败事件。
        /// </summary>
        LoadSceneFailure,

        /// <summary>
        /// 加载场景更新事件。
        /// </summary>
        LoadSceneUpdate,

        /// <summary>
        /// 加载场景时加载依赖资源事件。
        /// </summary>
        LoadSceneDependencyAsset,

        /// <summary>
        /// 卸载场景成功事件。
        /// </summary>
        UnloadSceneSuccess,

        /// <summary>
        /// 卸载场景失败事件。
        /// </summary>
        UnloadSceneFailure,

        /// <summary>
        /// 播放声音成功事件。
        /// </summary>
        PlaySoundSuccess,

        /// <summary>
        /// 播放声音失败事件。
        /// </summary>
        PlaySoundFailure,

        /// <summary>
        /// 播放声音更新事件。
        /// </summary>
        PlaySoundUpdate,

        /// <summary>
        /// 播放声音时加载依赖资源事件。
        /// </summary>
        PlaySoundDependencyAsset,

        /// <summary>
        /// 显示实体成功事件。
        /// </summary>
        ShowEntitySuccess,

        /// <summary>
        /// 显示实体失败事件。
        /// </summary>
        ShowEntityFailure,

        /// <summary>
        /// 显示实体更新事件。
        /// </summary>
        ShowEntityUpdate,

        /// <summary>
        /// 显示实体时加载依赖资源事件。
        /// </summary>
        ShowEntityDependencyAsset,

        /// <summary>
        /// 隐藏实体完成事件。
        /// </summary>
        HideEntityComplete,

        /// <summary>
        /// 打开界面成功事件。
        /// </summary>
        OpenUIFormSuccess,

        /// <summary>
        /// 打开界面失败事件。
        /// </summary>
        OpenUIFormFailure,

        /// <summary>
        /// 打开界面更新事件。
        /// </summary>
        OpenUIFormUpdate,

        /// <summary>
        /// 打开界面时加载依赖资源事件。
        /// </summary>
        OpenUIFormDependencyAsset,

        /// <summary>
        /// 关闭界面完成事件。
        /// </summary>
        CloseUIFormComplete,

        // 注意：在这之前增加新的 Unity Game Framework 事件。

        /// <summary>
        /// 游戏逻辑事件开始标记。
        /// </summary>
        GameEventStart
    }
}
