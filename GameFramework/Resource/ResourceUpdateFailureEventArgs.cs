//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
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
        public ResourceUpdateFailureEventArgs()
        {
            Name = null;
            DownloadUri = null;
            RetryCount = 0;
            TotalRetryCount = 0;
            ErrorMessage = null;
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

        /// <summary>
        /// 创建资源更新失败事件。
        /// </summary>
        /// <param name="name">资源名称。</param>
        /// <param name="downloadUri">下载地址。</param>
        /// <param name="retryCount">已重试次数。</param>
        /// <param name="totalRetryCount">设定的重试次数。</param>
        /// <param name="errorMessage">错误信息。</param>
        /// <returns>创建的资源更新失败事件。</returns>
        /// <remarks>当已重试次数达到设定的重试次数时，将不再重试。</remarks>
        public static ResourceUpdateFailureEventArgs Create(string name, string downloadUri, int retryCount, int totalRetryCount, string errorMessage)
        {
            ResourceUpdateFailureEventArgs resourceUpdateFailureEventArgs = ReferencePool.Acquire<ResourceUpdateFailureEventArgs>();
            resourceUpdateFailureEventArgs.Name = name;
            resourceUpdateFailureEventArgs.DownloadUri = downloadUri;
            resourceUpdateFailureEventArgs.RetryCount = retryCount;
            resourceUpdateFailureEventArgs.TotalRetryCount = totalRetryCount;
            resourceUpdateFailureEventArgs.ErrorMessage = errorMessage;
            return resourceUpdateFailureEventArgs;
        }

        /// <summary>
        /// 清理资源更新失败事件。
        /// </summary>
        public override void Clear()
        {
            Name = null;
            DownloadUri = null;
            RetryCount = 0;
            TotalRetryCount = 0;
            ErrorMessage = null;
        }
    }
}
