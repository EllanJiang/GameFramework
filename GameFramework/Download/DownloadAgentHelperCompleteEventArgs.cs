//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
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
        private readonly byte[] m_Bytes;

        /// <summary>
        /// 初始化下载代理辅助器完成事件的新实例。
        /// </summary>
        /// <param name="length">下载的数据大小。</param>
        /// <param name="bytes">下载的数据流。</param>
        public DownloadAgentHelperCompleteEventArgs(int length, byte[] bytes)
        {
            Length = length;
            m_Bytes = bytes;
        }

        /// <summary>
        /// 获取下载的数据大小。
        /// </summary>
        public int Length
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取下载的数据流。
        /// </summary>
        public byte[] GetBytes()
        {
            return m_Bytes;
        }
    }
}
