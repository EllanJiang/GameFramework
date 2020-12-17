//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
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
            DeleteDownloading = false;
            ErrorMessage = null;
        }

        /// <summary>
        /// 获取是否需要删除正在下载的文件。
        /// </summary>
        public bool DeleteDownloading
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
        /// 创建下载代理辅助器错误事件。
        /// </summary>
        /// <param name="deleteDownloading">是否需要删除正在下载的文件。</param>
        /// <param name="errorMessage">错误信息。</param>
        /// <returns>创建的下载代理辅助器错误事件。</returns>
        public static DownloadAgentHelperErrorEventArgs Create(bool deleteDownloading, string errorMessage)
        {
            DownloadAgentHelperErrorEventArgs downloadAgentHelperErrorEventArgs = ReferencePool.Acquire<DownloadAgentHelperErrorEventArgs>();
            downloadAgentHelperErrorEventArgs.DeleteDownloading = deleteDownloading;
            downloadAgentHelperErrorEventArgs.ErrorMessage = errorMessage;
            return downloadAgentHelperErrorEventArgs;
        }

        /// <summary>
        /// 清理下载代理辅助器错误事件。
        /// </summary>
        public override void Clear()
        {
            DeleteDownloading = false;
            ErrorMessage = null;
        }
    }
}
