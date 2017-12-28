//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.WebRequest
{
    /// <summary>
    /// Web 请求代理辅助器错误事件。
    /// </summary>
    public sealed class WebRequestAgentHelperErrorEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化 Web 请求代理辅助器错误事件的新实例。
        /// </summary>
        /// <param name="errorMessage">错误信息。</param>
        public WebRequestAgentHelperErrorEventArgs(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// 获取错误信息。
        /// </summary>
        public string ErrorMessage
        {
            get;
            private set;
        }
    }
}
