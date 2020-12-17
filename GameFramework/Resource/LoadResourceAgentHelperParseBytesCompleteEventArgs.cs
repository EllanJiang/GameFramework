//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Resource
{
    /// <summary>
    /// 加载资源代理辅助器异步将资源二进制流转换为加载对象完成事件。
    /// </summary>
    public sealed class LoadResourceAgentHelperParseBytesCompleteEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化加载资源代理辅助器异步将资源二进制流转换为加载对象完成事件的新实例。
        /// </summary>
        public LoadResourceAgentHelperParseBytesCompleteEventArgs()
        {
            Resource = null;
        }

        /// <summary>
        /// 获取加载对象。
        /// </summary>
        public object Resource
        {
            get;
            private set;
        }

        /// <summary>
        /// 创建加载资源代理辅助器异步将资源二进制流转换为加载对象完成事件。
        /// </summary>
        /// <param name="resource">资源对象。</param>
        /// <returns>创建的加载资源代理辅助器异步将资源二进制流转换为加载对象完成事件。</returns>
        public static LoadResourceAgentHelperParseBytesCompleteEventArgs Create(object resource)
        {
            LoadResourceAgentHelperParseBytesCompleteEventArgs loadResourceAgentHelperParseBytesCompleteEventArgs = ReferencePool.Acquire<LoadResourceAgentHelperParseBytesCompleteEventArgs>();
            loadResourceAgentHelperParseBytesCompleteEventArgs.Resource = resource;
            return loadResourceAgentHelperParseBytesCompleteEventArgs;
        }

        /// <summary>
        /// 清理加载资源代理辅助器异步将资源二进制流转换为加载对象完成事件。
        /// </summary>
        public override void Clear()
        {
            Resource = null;
        }
    }
}
