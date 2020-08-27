//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;

namespace GameFramework.WebRequest
{
    /// <summary>
    /// Web 请求管理器接口。
    /// </summary>
    public interface IWebRequestManager
    {
        /// <summary>
        /// 获取 Web 请求代理总数量。
        /// </summary>
        int TotalAgentCount
        {
            get;
        }

        /// <summary>
        /// 获取可用 Web 请求代理数量。
        /// </summary>
        int FreeAgentCount
        {
            get;
        }

        /// <summary>
        /// 获取工作中 Web 请求代理数量。
        /// </summary>
        int WorkingAgentCount
        {
            get;
        }

        /// <summary>
        /// 获取等待 Web 请求数量。
        /// </summary>
        int WaitingTaskCount
        {
            get;
        }

        /// <summary>
        /// 获取或设置 Web 请求超时时长，以秒为单位。
        /// </summary>
        float Timeout
        {
            get;
            set;
        }

        /// <summary>
        /// Web 请求开始事件。
        /// </summary>
        event EventHandler<WebRequestStartEventArgs> WebRequestStart;

        /// <summary>
        /// Web 请求成功事件。
        /// </summary>
        event EventHandler<WebRequestSuccessEventArgs> WebRequestSuccess;

        /// <summary>
        /// Web 请求失败事件。
        /// </summary>
        event EventHandler<WebRequestFailureEventArgs> WebRequestFailure;

        /// <summary>
        /// 增加 Web 请求代理辅助器。
        /// </summary>
        /// <param name="webRequestAgentHelper">要增加的 Web 请求代理辅助器。</param>
        void AddWebRequestAgentHelper(IWebRequestAgentHelper webRequestAgentHelper);

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        int AddWebRequest(string webRequestUri);

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="postData">要发送的数据流。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        int AddWebRequest(string webRequestUri, byte[] postData);

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="priority">Web 请求任务的优先级。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        int AddWebRequest(string webRequestUri, int priority);

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        int AddWebRequest(string webRequestUri, object userData);

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="postData">要发送的数据流。</param>
        /// <param name="priority">Web 请求任务的优先级。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        int AddWebRequest(string webRequestUri, byte[] postData, int priority);

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="postData">要发送的数据流。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        int AddWebRequest(string webRequestUri, byte[] postData, object userData);

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="priority">Web 请求任务的优先级。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        int AddWebRequest(string webRequestUri, int priority, object userData);

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="postData">要发送的数据流。</param>
        /// <param name="priority">Web 请求任务的优先级。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        int AddWebRequest(string webRequestUri, byte[] postData, int priority, object userData);

        /// <summary>
        /// 移除 Web 请求任务。
        /// </summary>
        /// <param name="serialId">要移除 Web 请求任务的序列编号。</param>
        /// <returns>是否移除 Web 请求任务成功。</returns>
        bool RemoveWebRequest(int serialId);

        /// <summary>
        /// 移除所有 Web 请求任务。
        /// </summary>
        void RemoveAllWebRequests();

        /// <summary>
        /// 获取所有 Web 请求任务的信息。
        /// </summary>
        /// <returns>所有 Web 请求任务的信息。</returns>
        TaskInfo[] GetAllWebRequestInfos();
    }
}
