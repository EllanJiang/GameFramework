//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Resource
{
    /// <summary>
    /// 加载资源代理辅助器异步加载资源完成事件。
    /// </summary>
    public sealed class LoadResourceAgentHelperLoadCompleteEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化加载资源代理辅助器异步加载资源完成事件的新实例。
        /// </summary>
        public LoadResourceAgentHelperLoadCompleteEventArgs()
        {
            Asset = null;
        }

        /// <summary>
        /// 获取加载的资源。
        /// </summary>
        public object Asset
        {
            get;
            private set;
        }

        /// <summary>
        /// 创建加载资源代理辅助器异步加载资源完成事件。
        /// </summary>
        /// <param name="asset">加载的资源。</param>
        /// <returns>创建的加载资源代理辅助器异步加载资源完成事件。</returns>
        public static LoadResourceAgentHelperLoadCompleteEventArgs Create(object asset)
        {
            LoadResourceAgentHelperLoadCompleteEventArgs loadResourceAgentHelperLoadCompleteEventArgs = ReferencePool.Acquire<LoadResourceAgentHelperLoadCompleteEventArgs>();
            loadResourceAgentHelperLoadCompleteEventArgs.Asset = asset;
            return loadResourceAgentHelperLoadCompleteEventArgs;
        }

        /// <summary>
        /// 清理加载资源代理辅助器异步加载资源完成事件。
        /// </summary>
        public override void Clear()
        {
            Asset = null;
        }
    }
}
