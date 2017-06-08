﻿//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System;

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
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri)
        {
            return AddWebRequest(webRequestUri, null, null);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="postData">要发送的数据流。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, byte[] postData)
        {
            return AddWebRequest(webRequestUri, postData, null);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, object userData)
        {
            return AddWebRequest(webRequestUri, null, userData);
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
            if (string.IsNullOrEmpty(webRequestUri))
            {
                throw new GameFrameworkException("Web request uri is invalid.");
            }

            if (TotalAgentCount <= 0)
            {
                throw new GameFrameworkException("You must add web request agent first.");
            }

            WebRequestTask webRequestTask = new WebRequestTask(webRequestUri, postData, m_Timeout, userData);
            m_TaskPool.AddTask(webRequestTask);

            return webRequestTask.SerialId;
        }

        /// <summary>
        /// 移除 Web 请求任务。
        /// </summary>
        /// <param name="serialId">要移除 Web 请求任务的序列编号。</param>
        /// <returns>是否移除 Web 请求任务成功。</returns>
        public bool RemoveWebRequest(int serialId)
        {
            return m_TaskPool.RemoveTask(serialId) != null;
        }

        /// <summary>
        /// 移除所有 Web 请求任务。
        /// </summary>
        public void RemoveAllWebRequests()
        {
            m_TaskPool.RemoveAllTasks();
        }

        private void OnWebRequestAgentStart(WebRequestAgent sender)
        {
            if (m_WebRequestStartEventHandler != null)
            {
                m_WebRequestStartEventHandler(this, new WebRequestStartEventArgs(sender.Task.SerialId, sender.Task.WebRequestUri, sender.Task.UserData));
            }
        }

        private void OnWebRequestAgentSuccess(WebRequestAgent sender, byte[] webResponseBytes)
        {
            if (m_WebRequestSuccessEventHandler != null)
            {
                m_WebRequestSuccessEventHandler(this, new WebRequestSuccessEventArgs(sender.Task.SerialId, sender.Task.WebRequestUri, webResponseBytes, sender.Task.UserData));
            }
        }

        private void OnWebRequestAgentFailure(WebRequestAgent sender, string errorMessage)
        {
            if (m_WebRequestFailureEventHandler != null)
            {
                m_WebRequestFailureEventHandler(this, new WebRequestFailureEventArgs(sender.Task.SerialId, sender.Task.WebRequestUri, errorMessage, sender.Task.UserData));
            }
        }
    }
}
