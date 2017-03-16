//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.Event;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// Web 请求成功事件。
    /// </summary>
    public sealed class WebRequestSuccessEventArgs : GameEventArgs
    {
        private readonly byte[] m_WebResponseBytes;

        /// <summary>
        /// 初始化 Web 请求成功事件的新实例。
        /// </summary>
        /// <param name="e">内部事件。</param>
        public WebRequestSuccessEventArgs(GameFramework.WebRequest.WebRequestSuccessEventArgs e)
        {
            WWWFormInfo wwwFormInfo = (WWWFormInfo)e.UserData;
            SerialId = e.SerialId;
            WebRequestUri = e.WebRequestUri;
            m_WebResponseBytes = e.GetWebResponseBytes();
            UserData = wwwFormInfo.UserData;
        }

        /// <summary>
        /// 获取 Web 请求成功事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return (int)EventId.WebRequestSuccess;
            }
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
