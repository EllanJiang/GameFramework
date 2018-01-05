//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Download
{
    internal partial class DownloadManager
    {
        /// <summary>
        /// 下载任务的状态。
        /// </summary>
        private enum DownloadTaskStatus
        {
            /// <summary>
            /// 准备下载。
            /// </summary>
            Todo,

            /// <summary>
            /// 下载中。
            /// </summary>
            Doing,

            /// <summary>
            /// 下载完成。
            /// </summary>
            Done,

            /// <summary>
            /// 下载错误。
            /// </summary>
            Error
        }
    }
}
