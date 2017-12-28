//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
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
        /// <param name="asset">加载的资源。</param>
        public LoadResourceAgentHelperLoadCompleteEventArgs(object asset)
        {
            Asset = asset;
        }

        /// <summary>
        /// 获取加载的资源。
        /// </summary>
        public object Asset
        {
            get;
            private set;
        }
    }
}
