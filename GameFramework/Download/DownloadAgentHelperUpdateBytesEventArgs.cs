//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Download
{
    /// <summary>
    ///  下载代理辅助器更新数据流事件。
    /// </summary>
    public sealed class DownloadAgentHelperUpdateBytesEventArgs : GameFrameworkEventArgs
    {
        private readonly byte[] m_Bytes;

        /// <summary>
        /// 初始化下载代理辅助器更新数据流事件的新实例。
        /// </summary>
        /// <param name="bytes">下载的数据流。</param>
        /// <param name="offset">数据流的偏移。</param>
        /// <param name="length">数据流的长度。</param>
        public DownloadAgentHelperUpdateBytesEventArgs(byte[] bytes, int offset, int length)
        {
            if (bytes == null)
            {
                throw new GameFrameworkException("Bytes is invalid.");
            }

            if (offset < 0 || offset >= bytes.Length)
            {
                throw new GameFrameworkException("Offset is invalid.");
            }

            if (length <= 0 || offset + length > bytes.Length)
            {
                throw new GameFrameworkException("Length is invalid.");
            }

            m_Bytes = bytes;
            Offset = offset;
            Length = length;
        }

        /// <summary>
        /// 获取数据流的偏移。
        /// </summary>
        public int Offset
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取数据流的长度。
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
