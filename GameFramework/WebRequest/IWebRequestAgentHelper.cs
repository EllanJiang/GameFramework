//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System;

namespace GameFramework.WebRequest
{
    /// <summary>
    /// Web 请求代理辅助器接口。
    /// </summary>
    public interface IWebRequestAgentHelper
    {
        /// <summary>
        /// Web 请求代理辅助器完成事件。
        /// </summary>
        event EventHandler<WebRequestAgentHelperCompleteEventArgs> WebRequestAgentHelperComplete;

        /// <summary>
        /// Web 请求代理辅助器错误事件。
        /// </summary>
        event EventHandler<WebRequestAgentHelperErrorEventArgs> WebRequestAgentHelperError;

        /// <summary>
        /// 通过 Web 请求代理辅助器发送 Web 请求。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="userData">用户自定义数据。</param>
        void Request(string webRequestUri, object userData);

        /// <summary>
        /// 通过 Web 请求代理辅助器发送 Web 请求。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="postData">要发送的数据流。</param>
        /// <param name="userData">用户自定义数据。</param>
        void Request(string webRequestUri, byte[] postData, object userData);

        /// <summary>
        /// 重置 Web 请求代理辅助器。
        /// </summary>
        void Reset();
    }
}
