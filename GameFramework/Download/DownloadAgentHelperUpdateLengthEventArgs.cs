//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Download
{
    /// <summary>
    ///  下载代理辅助器更新数据大小事件。
    /// </summary>
    public sealed class DownloadAgentHelperUpdateLengthEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化下载代理辅助器更新数据大小事件的新实例。
        /// </summary>
        /// <param name="deltaLength">下载的增量数据大小。</param>
        public DownloadAgentHelperUpdateLengthEventArgs(int deltaLength)
        {
            if (deltaLength <= 0)
            {
                throw new GameFrameworkException("Delta length is invalid.");
            }

            DeltaLength = deltaLength;
        }

        /// <summary>
        /// 获取下载的增量数据大小。
        /// </summary>
        public int DeltaLength
        {
            get;
            private set;
        }
    }
}
