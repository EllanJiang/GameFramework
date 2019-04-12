//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Download
{
    /// <summary>
    ///  下载代理辅助器更新事件。
    /// </summary>
    public sealed class DownloadAgentHelperUpdateEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化下载代理辅助器更新事件的新实例。
        /// </summary>
        /// <param name="deltaLength">下载的数据增量大小。</param>
        public DownloadAgentHelperUpdateEventArgs(int deltaLength)
        {
            DeltaLength = deltaLength;
        }

        /// <summary>
        /// 获取下载的数据增量大小。
        /// </summary>
        public int DeltaLength
        {
            get;
            private set;
        }
    }
}
