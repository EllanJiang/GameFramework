//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.WebRequest
{
    /// <summary>
    /// Web 请求失败事件。
    /// </summary>
    public sealed class WebRequestFailureEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化 Web 请求失败事件的新实例。
        /// </summary>
        public WebRequestFailureEventArgs()
        {
            SerialId = 0;
            WebRequestUri = null;
            ErrorMessage = null;
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
        /// 获取错误信息。
        /// </summary>
        public string ErrorMessage
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
        /// 创建 Web 请求失败事件。
        /// </summary>
        /// <param name="serialId">Web 请求任务的序列编号。</param>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="errorMessage">错误信息。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>创建的 Web 请求失败事件。</returns>
        public static WebRequestFailureEventArgs Create(int serialId, string webRequestUri, string errorMessage, object userData)
        {
            WebRequestFailureEventArgs webRequestFailureEventArgs = ReferencePool.Acquire<WebRequestFailureEventArgs>();
            webRequestFailureEventArgs.SerialId = serialId;
            webRequestFailureEventArgs.WebRequestUri = webRequestUri;
            webRequestFailureEventArgs.ErrorMessage = errorMessage;
            webRequestFailureEventArgs.UserData = userData;
            return webRequestFailureEventArgs;
        }

        /// <summary>
        /// 清理 Web 请求失败事件。
        /// </summary>
        public override void Clear()
        {
            SerialId = 0;
            WebRequestUri = null;
            ErrorMessage = null;
            UserData = null;
        }
    }
}
