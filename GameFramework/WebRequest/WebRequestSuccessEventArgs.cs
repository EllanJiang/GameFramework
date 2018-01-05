//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.WebRequest
{
    /// <summary>
    /// Web 请求成功事件。
    /// </summary>
    public sealed class WebRequestSuccessEventArgs : GameFrameworkEventArgs
    {
        private readonly byte[] m_WebResponseBytes;

        /// <summary>
        /// 初始化 Web 请求成功事件的新实例。
        /// </summary>
        /// <param name="serialId">Web 请求任务的序列编号。</param>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="webResponseBytes">Web 响应的数据流。</param>
        /// <param name="userData">用户自定义数据。</param>
        public WebRequestSuccessEventArgs(int serialId, string webRequestUri, byte[] webResponseBytes, object userData)
        {
            SerialId = serialId;
            WebRequestUri = webRequestUri;
            m_WebResponseBytes = webResponseBytes;
            UserData = userData;
        }

        /// <summary>
        /// 获取 Web 请求任务的序列编号。
        /// </summary>
        public int SerialId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取 Web 请求地址。
        /// </summary>
        public string WebRequestUri
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取用户自定义数据。
        /// </summary>
        public object UserData
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取 Web 响应的数据流。
        /// </summary>
        /// <returns>Web 响应的数据流。</returns>
        public byte[] GetWebResponseBytes()
        {
            return m_WebResponseBytes;
        }
    }
}
