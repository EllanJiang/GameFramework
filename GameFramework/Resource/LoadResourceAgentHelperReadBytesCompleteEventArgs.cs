//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Resource
{
    /// <summary>
    /// 加载资源代理辅助器异步读取资源二进制流完成事件。
    /// </summary>
    public sealed class LoadResourceAgentHelperReadBytesCompleteEventArgs : GameFrameworkEventArgs
    {
        private readonly byte[] m_Bytes;

        /// <summary>
        /// 初始化加载资源代理辅助器异步读取资源二进制流完成事件的新实例。
        /// </summary>
        /// <param name="bytes">资源的二进制流。</param>
        /// <param name="loadType">资源加载方式。</param>
        public LoadResourceAgentHelperReadBytesCompleteEventArgs(byte[] bytes, int loadType)
        {
            m_Bytes = bytes;
            LoadType = loadType;
        }

        /// <summary>
        /// 获取资源加载方式。
        /// </summary>
        public int LoadType
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取资源的二进制流。
        /// </summary>
        /// <returns>资源的二进制流。</returns>
        public byte[] GetBytes()
        {
            return m_Bytes;
        }
    }
}
