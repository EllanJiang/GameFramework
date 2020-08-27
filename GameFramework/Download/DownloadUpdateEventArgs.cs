//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Download
{
    /// <summary>
    /// 下载更新事件。
    /// </summary>
    public sealed class DownloadUpdateEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化下载更新事件的新实例。
        /// </summary>
        public DownloadUpdateEventArgs()
        {
            SerialId = 0;
            DownloadPath = null;
            DownloadUri = null;
            CurrentLength = 0;
            UserData = null;
        }

        /// <summary>
        /// 获取下载任务的序列编号。
        /// </summary>
        public int SerialId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取下载后存放路径。
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
        /// 获取当前大小。
        /// </summary>
        public int CurrentLength
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取用户自定义数据。
        /// </summary>
        public object UserData
        {
            get;
            private set;
        }

        /// <summary>
        /// 创建下载更新事件。
        /// </summary>
        /// <param name="serialId">下载任务的序列编号。</param>
        /// <param name="downloadPath">下载后存放路径。</param>
        /// <param name="downloadUri">下载地址。</param>
        /// <param name="currentLength">当前大小。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>创建的下载更新事件。</returns>
        public static DownloadUpdateEventArgs Create(int serialId, string downloadPath, string downloadUri, int currentLength, object userData)
        {
            DownloadUpdateEventArgs downloadUpdateEventArgs = ReferencePool.Acquire<DownloadUpdateEventArgs>();
            downloadUpdateEventArgs.SerialId = serialId;
            downloadUpdateEventArgs.DownloadPath = downloadPath;
            downloadUpdateEventArgs.DownloadUri = downloadUri;
            downloadUpdateEventArgs.CurrentLength = currentLength;
            downloadUpdateEventArgs.UserData = userData;
            return downloadUpdateEventArgs;
        }

        /// <summary>
        /// 清理下载更新事件。
        /// </summary>
        public override void Clear()
        {
            SerialId = 0;
            DownloadPath = null;
            DownloadUri = null;
            CurrentLength = 0;
            UserData = null;
        }
    }
}
