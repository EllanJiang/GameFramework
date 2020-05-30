//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Resource
{
    /// <summary>
    /// 资源更新开始事件。
    /// </summary>
    public sealed class ResourceUpdateStartEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化资源更新开始事件的新实例。
        /// </summary>
        public ResourceUpdateStartEventArgs()
        {
            Name = null;
            DownloadPath = null;
            DownloadUri = null;
            CurrentLength = 0;
            ZipLength = 0;
            RetryCount = 0;
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
        /// 获取资源下载后存放路径。
        /// </summary>
        public string DownloadPath
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
        /// 获取当前下载大小。
        /// </summary>
        public int CurrentLength
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取压缩后大小。
        /// </summary>
        public int ZipLength
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取已重试下载次数。
        /// </summary>
        public int RetryCount
        {
            get;
            private set;
        }

        /// <summary>
        /// 创建资源更新开始事件。
        /// </summary>
        /// <param name="name">资源名称。</param>
        /// <param name="downloadPath">资源下载后存放路径。</param>
        /// <param name="downloadUri">资源下载地址。</param>
        /// <param name="currentLength">当前下载大小。</param>
        /// <param name="zipLength">压缩后大小。</param>
        /// <param name="retryCount">已重试下载次数。</param>
        /// <returns>创建的资源更新开始事件。</returns>
        public static ResourceUpdateStartEventArgs Create(string name, string downloadPath, string downloadUri, int currentLength, int zipLength, int retryCount)
        {
            ResourceUpdateStartEventArgs resourceUpdateStartEventArgs = ReferencePool.Acquire<ResourceUpdateStartEventArgs>();
            resourceUpdateStartEventArgs.Name = name;
            resourceUpdateStartEventArgs.DownloadPath = downloadPath;
            resourceUpdateStartEventArgs.DownloadUri = downloadUri;
            resourceUpdateStartEventArgs.CurrentLength = currentLength;
            resourceUpdateStartEventArgs.ZipLength = zipLength;
            resourceUpdateStartEventArgs.RetryCount = retryCount;
            return resourceUpdateStartEventArgs;
        }

        /// <summary>
        /// 清理资源更新开始事件。
        /// </summary>
        public override void Clear()
        {
            Name = null;
            DownloadPath = null;
            DownloadUri = null;
            CurrentLength = 0;
            ZipLength = 0;
            RetryCount = 0;
        }
    }
}
