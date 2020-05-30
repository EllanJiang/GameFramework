//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Resource
{
    /// <summary>
    /// 资源更新改变事件。
    /// </summary>
    public sealed class ResourceUpdateChangedEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化资源更新改变事件的新实例。
        /// </summary>
        public ResourceUpdateChangedEventArgs()
        {
            Name = null;
            DownloadPath = null;
            DownloadUri = null;
            CurrentLength = 0;
            ZipLength = 0;
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
        /// 创建资源更新改变事件。
        /// </summary>
        /// <param name="name">资源名称。</param>
        /// <param name="downloadPath">资源下载后存放路径。</param>
        /// <param name="downloadUri">资源下载地址。</param>
        /// <param name="currentLength">当前下载大小。</param>
        /// <param name="zipLength">压缩后大小。</param>
        /// <returns>创建的资源更新改变事件。</returns>
        public static ResourceUpdateChangedEventArgs Create(string name, string downloadPath, string downloadUri, int currentLength, int zipLength)
        {
            ResourceUpdateChangedEventArgs resourceUpdateChangedEventArgs = ReferencePool.Acquire<ResourceUpdateChangedEventArgs>();
            resourceUpdateChangedEventArgs.Name = name;
            resourceUpdateChangedEventArgs.DownloadPath = downloadPath;
            resourceUpdateChangedEventArgs.DownloadUri = downloadUri;
            resourceUpdateChangedEventArgs.CurrentLength = currentLength;
            resourceUpdateChangedEventArgs.ZipLength = zipLength;
            return resourceUpdateChangedEventArgs;
        }

        /// <summary>
        /// 清理资源更新改变事件。
        /// </summary>
        public override void Clear()
        {
            Name = null;
            DownloadPath = null;
            DownloadUri = null;
            CurrentLength = 0;
            ZipLength = 0;
        }
    }
}
