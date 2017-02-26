//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.Event;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 下载失败事件。
    /// </summary>
    public sealed class DownloadFailureEventArgs : GameEventArgs
    {
        /// <summary>
        /// 初始化下载失败事件的新实例。
        /// </summary>
        /// <param name="e">内部事件。</param>
        public DownloadFailureEventArgs(GameFramework.Download.DownloadFailureEventArgs e)
        {
            SerialId = e.SerialId;
            DownloadPath = e.DownloadPath;
            DownloadUri = e.DownloadUri;
            ErrorMessage = e.ErrorMessage;
            UserData = e.UserData;
        }

        /// <summary>
        /// 获取下载失败事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return (int)EventId.DownloadFailure;
            }
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
        /// 获取错误信息。
        /// </summary>
        public string ErrorMessage
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
    }
}
