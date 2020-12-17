//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.WebRequest
{
    /// <summary>
    /// Web 请求成功事件。
    /// </summary>
    public sealed class WebRequestSuccessEventArgs : GameFrameworkEventArgs
    {
        private byte[] m_WebResponseBytes;

        /// <summary>
        /// 初始化 Web 请求成功事件的新实例。
        /// </summary>
        public WebRequestSuccessEventArgs()
        {
            SerialId = 0;
            WebRequestUri = null;
            m_WebResponseBytes = null;
            UserData = null;
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
        /// 创建 Web 请求成功事件。
        /// </summary>
        /// <param name="serialId">Web 请求任务的序列编号。</param>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="webResponseBytes">Web 响应的数据流。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>创建的 Web 请求成功事件。</returns>
        public static WebRequestSuccessEventArgs Create(int serialId, string webRequestUri, byte[] webResponseBytes, object userData)
        {
            WebRequestSuccessEventArgs webRequestSuccessEventArgs = ReferencePool.Acquire<WebRequestSuccessEventArgs>();
            webRequestSuccessEventArgs.SerialId = serialId;
            webRequestSuccessEventArgs.WebRequestUri = webRequestUri;
            webRequestSuccessEventArgs.m_WebResponseBytes = webResponseBytes;
            webRequestSuccessEventArgs.UserData = userData;
            return webRequestSuccessEventArgs;
        }

        /// <summary>
        /// 清理 Web 请求成功事件。
        /// </summary>
        public override void Clear()
        {
            SerialId = 0;
            WebRequestUri = null;
            m_WebResponseBytes = null;
            UserData = null;
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
