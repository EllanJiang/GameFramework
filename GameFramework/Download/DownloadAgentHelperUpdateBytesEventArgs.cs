//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Download
{
    /// <summary>
    /// 下载代理辅助器更新数据流事件。
    /// </summary>
    public sealed class DownloadAgentHelperUpdateBytesEventArgs : GameFrameworkEventArgs
    {
        private byte[] m_Bytes;

        /// <summary>
        /// 初始化下载代理辅助器更新数据流事件的新实例。
        /// </summary>
        public DownloadAgentHelperUpdateBytesEventArgs()
        {
            m_Bytes = null;
            Offset = 0;
            Length = 0;
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
        /// 创建下载代理辅助器更新数据流事件。
        /// </summary>
        /// <param name="bytes">下载的数据流。</param>
        /// <param name="offset">数据流的偏移。</param>
        /// <param name="length">数据流的长度。</param>
        /// <returns>创建的下载代理辅助器更新数据流事件。</returns>
        public static DownloadAgentHelperUpdateBytesEventArgs Create(byte[] bytes, int offset, int length)
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

            DownloadAgentHelperUpdateBytesEventArgs downloadAgentHelperUpdateBytesEventArgs = ReferencePool.Acquire<DownloadAgentHelperUpdateBytesEventArgs>();
            downloadAgentHelperUpdateBytesEventArgs.m_Bytes = bytes;
            downloadAgentHelperUpdateBytesEventArgs.Offset = offset;
            downloadAgentHelperUpdateBytesEventArgs.Length = length;
            return downloadAgentHelperUpdateBytesEventArgs;
        }

        /// <summary>
        /// 清理下载代理辅助器更新数据流事件。
        /// </summary>
        public override void Clear()
        {
            m_Bytes = null;
            Offset = 0;
            Length = 0;
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
