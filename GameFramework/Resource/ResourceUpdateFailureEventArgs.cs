//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Resource
{
    /// <summary>
    /// 资源更新失败事件。
    /// </summary>
    public sealed class ResourceUpdateFailureEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化资源更新失败事件的新实例。
        /// </summary>
        /// <param name="name">资源名称。</param>
        /// <param name="downloadUri">下载地址。</param>
        /// <param name="retryCount">已重试次数。</param>
        /// <param name="totalRetryCount">设定的重试次数。</param>
        /// <param name="errorMessage">错误信息。</param>
        /// <remarks>当已重试次数达到设定的重试次数时，将不再重试。</remarks>
        public ResourceUpdateFailureEventArgs(string name, string downloadUri, int retryCount, int totalRetryCount, string errorMessage)
        {
            Name = name;
            DownloadUri = downloadUri;
            RetryCount = retryCount;
            TotalRetryCount = totalRetryCount;
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// 获取资源名称。
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取下载地址。
        /// </summary>
        public string DownloadUri
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取已重试次数。
        /// </summary>
        public int RetryCount
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取设定的重试次数。
        /// </summary>
        public int TotalRetryCount
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
    }
}
