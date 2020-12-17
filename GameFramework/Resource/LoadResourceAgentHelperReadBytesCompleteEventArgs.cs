//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Resource
{
    /// <summary>
    /// 加载资源代理辅助器异步读取资源二进制流完成事件。
    /// </summary>
    public sealed class LoadResourceAgentHelperReadBytesCompleteEventArgs : GameFrameworkEventArgs
    {
        private byte[] m_Bytes;

        /// <summary>
        /// 初始化加载资源代理辅助器异步读取资源二进制流完成事件的新实例。
        /// </summary>
        public LoadResourceAgentHelperReadBytesCompleteEventArgs()
        {
            m_Bytes = null;
        }

        /// <summary>
        /// 创建加载资源代理辅助器异步读取资源二进制流完成事件。
        /// </summary>
        /// <param name="bytes">资源的二进制流。</param>
        /// <returns>创建的加载资源代理辅助器异步读取资源二进制流完成事件。</returns>
        public static LoadResourceAgentHelperReadBytesCompleteEventArgs Create(byte[] bytes)
        {
            LoadResourceAgentHelperReadBytesCompleteEventArgs loadResourceAgentHelperReadBytesCompleteEventArgs = ReferencePool.Acquire<LoadResourceAgentHelperReadBytesCompleteEventArgs>();
            loadResourceAgentHelperReadBytesCompleteEventArgs.m_Bytes = bytes;
            return loadResourceAgentHelperReadBytesCompleteEventArgs;
        }

        /// <summary>
        /// 清理加载资源代理辅助器异步读取资源二进制流完成事件。
        /// </summary>
        public override void Clear()
        {
            m_Bytes = null;
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
