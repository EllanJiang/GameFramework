//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Download
{
    internal sealed partial class DownloadManager : GameFrameworkModule, IDownloadManager
    {
        /// <summary>
        /// 下载任务的状态。
        /// </summary>
        private enum DownloadTaskStatus : byte
        {
            /// <summary>
            /// 准备下载。
            /// </summary>
            Todo = 0,

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
