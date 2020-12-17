//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.WebRequest
{
    /// <summary>
    /// Web 请求开始事件。
    /// </summary>
    public sealed class WebRequestStartEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化 Web 请求开始事件的新实例。
        /// </summary>
        public WebRequestStartEventArgs()
        {
            SerialId = 0;
            WebRequestUri = null;
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
        /// 创建 Web 请求开始事件。
        /// </summary>
        /// <param name="serialId">Web 请求任务的序列编号。</param>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>创建的 Web 请求开始事件。</returns>
        public static WebRequestStartEventArgs Create(int serialId, string webRequestUri, object userData)
        {
            WebRequestStartEventArgs webRequestStartEventArgs = ReferencePool.Acquire<WebRequestStartEventArgs>();
            webRequestStartEventArgs.SerialId = serialId;
            webRequestStartEventArgs.WebRequestUri = webRequestUri;
            webRequestStartEventArgs.UserData = userData;
            return webRequestStartEventArgs;
        }

        /// <summary>
        /// 清理 Web 请求开始事件。
        /// </summary>
        public override void Clear()
        {
            SerialId = 0;
            WebRequestUri = null;
            UserData = null;
        }
    }
}
