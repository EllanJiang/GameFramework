//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Download
{
    /// <summary>
    ///  下载代理辅助器完成事件。
    /// </summary>
    public sealed class DownloadAgentHelperCompleteEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化下载代理辅助器完成事件的新实例。
        /// </summary>
        /// <param name="length">下载的数据大小。</param>
        public DownloadAgentHelperCompleteEventArgs(int length)
        {
            if (length <= 0)
            {
                throw new GameFrameworkException("Length is invalid.");
            }

            Length = length;
        }

        /// <summary>
        /// 获取下载的数据大小。
        /// </summary>
        public int Length
        {
            get;
            private set;
        }
    }
}
