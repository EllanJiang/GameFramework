//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace GameFramework.WebRequest
{
    /// <summary>
    /// Web 请求管理器。
    /// </summary>
    internal sealed partial class WebRequestManager : GameFrameworkModule, IWebRequestManager
    {
        private readonly TaskPool<WebRequestTask> m_TaskPool;
        private float m_Timeout;
        private EventHandler<WebRequestStartEventArgs> m_WebRequestStartEventHandler;
        private EventHandler<WebRequestSuccessEventArgs> m_WebRequestSuccessEventHandler;
        private EventHandler<WebRequestFailureEventArgs> m_WebRequestFailureEventHandler;

        /// <summary>
        /// 初始化 Web 请求管理器的新实例。
        /// </summary>
        public WebRequestManager()
        {
            m_TaskPool = new TaskPool<WebRequestTask>();
            m_Timeout = 30f;
            m_WebRequestStartEventHandler = null;
            m_WebRequestSuccessEventHandler = null;
            m_WebRequestFailureEventHandler = null;
        }

        /// <summary>
        /// 获取 Web 请求代理总数量。
        /// </summary>
        public int TotalAgentCount
        {
            get
            {
                return m_TaskPool.TotalAgentCount;
            }
        }

        /// <summary>
        /// 获取可用 Web 请求代理数量。
        /// </summary>
        public int FreeAgentCount
        {
            get
            {
                return m_TaskPool.FreeAgentCount;
            }
        }

        /// <summary>
        /// 获取工作中 Web 请求代理数量。
        /// </summary>
        public int WorkingAgentCount
        {
            get
            {
                return m_TaskPool.WorkingAgentCount;
            }
        }

        /// <summary>
        /// 获取等待 Web 请求数量。
        /// </summary>
        public int WaitingTaskCount
        {
            get
            {
                return m_TaskPool.WaitingTaskCount;
            }
        }

        /// <summary>
        /// 获取或设置 Web 请求超时时长，以秒为单位。
        /// </summary>
        public float Timeout
        {
            get
            {
                return m_Timeout;
            }
            set
            {
                m_Timeout = value;
            }
        }

        /// <summary>
        /// Web 请求开始事件。
        /// </summary>
        public event EventHandler<WebRequestStartEventArgs> WebRequestStart
        {
            add
            {
                m_WebRequestStartEventHandler += value;
            }
            remove
            {
                m_WebRequestStartEventHandler -= value;
            }
        }

        /// <summary>
        /// Web 请求成功事件。
        /// </summary>
        public event EventHandler<WebRequestSuccessEventArgs> WebRequestSuccess
        {
            add
            {
                m_WebRequestSuccessEventHandler += value;
            }
            remove
            {
                m_WebRequestSuccessEventHandler -= value;
            }
        }

        /// <summary>
        /// Web 请求失败事件。
        /// </summary>
        public event EventHandler<WebRequestFailureEventArgs> WebRequestFailure
        {
            add
            {
                m_WebRequestFailureEventHandler += value;
            }
            remove
            {
                m_WebRequestFailureEventHandler -= value;
            }
        }

        /// <summary>
        /// Web 请求管理器轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {
            m_TaskPool.Update(elapseSeconds, realElapseSeconds);
        }

        /// <summary>
        /// 关闭并清理 Web 请求管理器。
        /// </summary>
        internal override void Shutdown()
        {
            m_TaskPool.Shutdown();
        }

        /// <summary>
        /// 增加 Web 请求代理辅助器。
        /// </summary>
        /// <param name="webRequestAgentHelper">要增加的 Web 请求代理辅助器。</param>
        public void AddWebRequestAgentHelper(IWebRequestAgentHelper webRequestAgentHelper)
        {
            WebRequestAgent agent = new WebRequestAgent(webRequestAgentHelper);
            agent.WebRequestAgentStart += OnWebRequestAgentStart;
            agent.WebRequestAgentSuccess += OnWebRequestAgentSuccess;
            agent.WebRequestAgentFailure += OnWebRequestAgentFailure;

            m_TaskPool.AddAgent(agent);
        }

        /// <summary>
        /// 根据 Web 请求任务的序列编号获取 Web 请求任务的信息。
        /// </summary>
        /// <param name="serialId">要获取信息的 Web 请求任务的序列编号。</param>
        /// <returns>Web 请求任务的信息。</returns>
        public TaskInfo GetWebRequestInfo(int serialId)
        {
            return m_TaskPool.GetTaskInfo(serialId);
        }

        /// <summary>
        /// 根据 Web 请求任务的标签获取 Web 请求任务的信息。
        /// </summary>
        /// <param name="tag">要获取信息的 Web 请求任务的标签。</param>
        /// <returns>Web 请求任务的信息。</returns>
        public TaskInfo[] GetWebRequestInfos(string tag)
        {
            return m_TaskPool.GetTaskInfos(tag);
        }

        /// <summary>
        /// 根据 Web 请求任务的标签获取 Web 请求任务的信息。
        /// </summary>
        /// <param name="tag">要获取信息的 Web 请求任务的标签。</param>
        /// <param name="results">Web 请求任务的信息。</param>
        public void GetAllWebRequestInfos(string tag, List<TaskInfo> results)
        {
            m_TaskPool.GetTaskInfos(tag, results);
        }

        /// <summary>
        /// 获取所有 Web 请求任务的信息。
        /// </summary>
        /// <returns>所有 Web 请求任务的信息。</returns>
        public TaskInfo[] GetAllWebRequestInfos()
        {
            return m_TaskPool.GetAllTaskInfos();
        }

        /// <summary>
        /// 获取所有 Web 请求任务的信息。
        /// </summary>
        /// <param name="results">所有 Web 请求任务的信息。</param>
        public void GetAllWebRequestInfos(List<TaskInfo> results)
        {
            m_TaskPool.GetAllTaskInfos(results);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri)
        {
            return AddWebRequest(webRequestUri, null, null, Constant.DefaultPriority, null);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="postData">要发送的数据流。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, byte[] postData)
        {
            return AddWebRequest(webRequestUri, postData, null, Constant.DefaultPriority, null);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="tag">Web 请求任务的标签。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, string tag)
        {
            return AddWebRequest(webRequestUri, null, tag, Constant.DefaultPriority, null);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="priority">Web 请求任务的优先级。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, int priority)
        {
            return AddWebRequest(webRequestUri, null, null, priority, null);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, object userData)
        {
            return AddWebRequest(webRequestUri, null, null, Constant.DefaultPriority, userData);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="postData">要发送的数据流。</param>
        /// <param name="tag">Web 请求任务的标签。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, byte[] postData, string tag)
        {
            return AddWebRequest(webRequestUri, postData, tag, Constant.DefaultPriority, null);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="postData">要发送的数据流。</param>
        /// <param name="priority">Web 请求任务的优先级。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, byte[] postData, int priority)
        {
            return AddWebRequest(webRequestUri, postData, null, priority, null);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="postData">要发送的数据流。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, byte[] postData, object userData)
        {
            return AddWebRequest(webRequestUri, postData, null, Constant.DefaultPriority, userData);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="tag">Web 请求任务的标签。</param>
        /// <param name="priority">Web 请求任务的优先级。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, string tag, int priority)
        {
            return AddWebRequest(webRequestUri, null, tag, priority, null);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="tag">Web 请求任务的标签。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, string tag, object userData)
        {
            return AddWebRequest(webRequestUri, null, tag, Constant.DefaultPriority, userData);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="priority">Web 请求任务的优先级。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, int priority, object userData)
        {
            return AddWebRequest(webRequestUri, null, null, priority, userData);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="postData">要发送的数据流。</param>
        /// <param name="tag">Web 请求任务的标签。</param>
        /// <param name="priority">Web 请求任务的优先级。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, byte[] postData, string tag, int priority)
        {
            return AddWebRequest(webRequestUri, postData, tag, priority, null);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="postData">要发送的数据流。</param>
        /// <param name="tag">Web 请求任务的标签。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, byte[] postData, string tag, object userData)
        {
            return AddWebRequest(webRequestUri, postData, tag, Constant.DefaultPriority, userData);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="postData">要发送的数据流。</param>
        /// <param name="priority">Web 请求任务的优先级。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, byte[] postData, int priority, object userData)
        {
            return AddWebRequest(webRequestUri, postData, null, priority, userData);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="tag">Web 请求任务的标签。</param>
        /// <param name="priority">Web 请求任务的优先级。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, string tag, int priority, object userData)
        {
            return AddWebRequest(webRequestUri, null, tag, priority, userData);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="postData">要发送的数据流。</param>
        /// <param name="tag">Web 请求任务的标签。</param>
        /// <param name="priority">Web 请求任务的优先级。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, byte[] postData, string tag, int priority, object userData)
        {
            if (string.IsNullOrEmpty(webRequestUri))
            {
                throw new GameFrameworkException("Web request uri is invalid.");
            }

            if (TotalAgentCount <= 0)
            {
                throw new GameFrameworkException("You must add web request agent first.");
            }

            WebRequestTask webRequestTask = WebRequestTask.Create(webRequestUri, postData, tag, priority, m_Timeout, userData);
            m_TaskPool.AddTask(webRequestTask);
            return webRequestTask.SerialId;
        }

        /// <summary>
        /// 根据 Web 请求任务的序列编号移除 Web 请求任务。
        /// </summary>
        /// <param name="serialId">要移除 Web 请求任务的序列编号。</param>
        /// <returns>是否移除 Web 请求任务成功。</returns>
        public bool RemoveWebRequest(int serialId)
        {
            return m_TaskPool.RemoveTask(serialId);
        }

        /// <summary>
        /// 根据 Web 请求任务的标签移除 Web 请求任务。
        /// </summary>
        /// <param name="tag">要移除 Web 请求任务的标签。</param>
        /// <returns>移除 Web 请求任务的数量。</returns>
        public int RemoveWebRequests(string tag)
        {
            return m_TaskPool.RemoveTasks(tag);
        }

        /// <summary>
        /// 移除所有 Web 请求任务。
        /// </summary>
        /// <returns>移除 Web 请求任务的数量。</returns>
        public int RemoveAllWebRequests()
        {
            return m_TaskPool.RemoveAllTasks();
        }

        private void OnWebRequestAgentStart(WebRequestAgent sender)
        {
            if (m_WebRequestStartEventHandler != null)
            {
                WebRequestStartEventArgs webRequestStartEventArgs = WebRequestStartEventArgs.Create(sender.Task.SerialId, sender.Task.WebRequestUri, sender.Task.UserData);
                m_WebRequestStartEventHandler(this, webRequestStartEventArgs);
                ReferencePool.Release(webRequestStartEventArgs);
            }
        }

        private void OnWebRequestAgentSuccess(WebRequestAgent sender, byte[] webResponseBytes)
        {
            if (m_WebRequestSuccessEventHandler != null)
            {
                WebRequestSuccessEventArgs webRequestSuccessEventArgs = WebRequestSuccessEventArgs.Create(sender.Task.SerialId, sender.Task.WebRequestUri, webResponseBytes, sender.Task.UserData);
                m_WebRequestSuccessEventHandler(this, webRequestSuccessEventArgs);
                ReferencePool.Release(webRequestSuccessEventArgs);
            }
        }

        private void OnWebRequestAgentFailure(WebRequestAgent sender, string errorMessage)
        {
            if (m_WebRequestFailureEventHandler != null)
            {
                WebRequestFailureEventArgs webRequestFailureEventArgs = WebRequestFailureEventArgs.Create(sender.Task.SerialId, sender.Task.WebRequestUri, errorMessage, sender.Task.UserData);
                m_WebRequestFailureEventHandler(this, webRequestFailureEventArgs);
                ReferencePool.Release(webRequestFailureEventArgs);
            }
        }
    }
}
