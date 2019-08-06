//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Download
{
    /// <summary>
    /// 下载代理辅助器错误事件。
    /// </summary>
    public sealed class DownloadAgentHelperErrorEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化下载代理辅助器错误事件的新实例。
        /// </summary>
        public DownloadAgentHelperErrorEventArgs()
        {
            ErrorMessage = null;
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
        /// 创建下载代理辅助器错误事件。
        /// </summary>
        /// <param name="errorMessage">错误信息。</param>
        /// <returns>创建的下载代理辅助器错误事件。</returns>
        public static DownloadAgentHelperErrorEventArgs Create(string errorMessage)
        {
            DownloadAgentHelperErrorEventArgs downloadAgentHelperErrorEventArgs = ReferencePool.Acquire<DownloadAgentHelperErrorEventArgs>();
            downloadAgentHelperErrorEventArgs.ErrorMessage = errorMessage;
            return downloadAgentHelperErrorEventArgs;
        }

        /// <summary>
        /// 清理下载代理辅助器错误事件。
        /// </summary>
        public override void Clear()
        {
            ErrorMessage = null;
        }
    }
}
