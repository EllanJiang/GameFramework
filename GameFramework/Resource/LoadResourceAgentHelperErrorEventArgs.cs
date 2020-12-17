//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Resource
{
    /// <summary>
    /// 加载资源代理辅助器错误事件。
    /// </summary>
    public sealed class LoadResourceAgentHelperErrorEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化加载资源代理辅助器错误事件的新实例。
        /// </summary>
        public LoadResourceAgentHelperErrorEventArgs()
        {
            Status = LoadResourceStatus.Success;
            ErrorMessage = null;
        }

        /// <summary>
        /// 获取加载资源状态。
        /// </summary>
        public LoadResourceStatus Status
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取错误信息。
        /// </summary>
        public string ErrorMessage
        {
            get;
            private set;
        }

        /// <summary>
        /// 创建加载资源代理辅助器错误事件。
        /// </summary>
        /// <param name="status">加载资源状态。</param>
        /// <param name="errorMessage">错误信息。</param>
        /// <returns>创建的加载资源代理辅助器错误事件。</returns>
        public static LoadResourceAgentHelperErrorEventArgs Create(LoadResourceStatus status, string errorMessage)
        {
            LoadResourceAgentHelperErrorEventArgs loadResourceAgentHelperErrorEventArgs = ReferencePool.Acquire<LoadResourceAgentHelperErrorEventArgs>();
            loadResourceAgentHelperErrorEventArgs.Status = status;
            loadResourceAgentHelperErrorEventArgs.ErrorMessage = errorMessage;
            return loadResourceAgentHelperErrorEventArgs;
        }

        /// <summary>
        /// 清理加载资源代理辅助器错误事件。
        /// </summary>
        public override void Clear()
        {
            Status = LoadResourceStatus.Success;
            ErrorMessage = null;
        }
    }
}
