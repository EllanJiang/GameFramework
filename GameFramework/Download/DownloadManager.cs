//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System;

namespace GameFramework.Download
{
    /// <summary>
    /// 下载管理器。
    /// </summary>
    internal sealed partial class DownloadManager : GameFrameworkModule, IDownloadManager
    {
        private readonly TaskPool<DownloadTask> m_TaskPool;
        private readonly DownloadCounter m_DownloadCounter;
        private int m_FlushSize;
        private float m_Timeout;
        private EventHandler<DownloadStartEventArgs> m_DownloadStartEventHandler;
        private EventHandler<DownloadUpdateEventArgs> m_DownloadUpdateEventHandler;
        private EventHandler<DownloadSuccessEventArgs> m_DownloadSuccessEventHandler;
        private EventHandler<DownloadFailureEventArgs> m_DownloadFailureEventHandler;

        /// <summary>
        /// 初始化下载管理器的新实例。
        /// </summary>
        public DownloadManager()
        {
            m_TaskPool = new TaskPool<DownloadTask>();
            m_DownloadCounter = new DownloadCounter(1f, 30f);
            m_FlushSize = 1024 * 1024;
            m_Timeout = 30f;
            m_DownloadStartEventHandler = null;
            m_DownloadUpdateEventHandler = null;
            m_DownloadSuccessEventHandler = null;
            m_DownloadFailureEventHandler = null;
        }

        /// <summary>
        /// 获取游戏框架模块优先级。
        /// </summary>
        /// <remarks>优先级较高的模块会优先轮询，并且关闭操作会后进行。</remarks>
        internal override int Priority
        {
            get
            {
                return 80;
            }
        }

        /// <summary>
        /// 获取下载代理总数量。
        /// </summary>
        public int TotalAgentCount
        {
            get
            {
                return m_TaskPool.TotalAgentCount;
            }
        }

        /// <summary>
        /// 获取可用下载代理数量。
        /// </summary>
        public int FreeAgentCount
        {
            get
            {
                return m_TaskPool.FreeAgentCount;
            }
        }

        /// <summary>
        /// 获取工作中下载代理数量。
        /// </summary>
        public int WorkingAgentCount
        {
            get
            {
                return m_TaskPool.WorkingAgentCount;
            }
        }

        /// <summary>
        /// 获取等待下载任务数量。
        /// </summary>
        public int WaitingTaskCount
        {
            get
            {
                return m_TaskPool.WaitingTaskCount;
            }
        }

        /// <summary>
        /// 获取或设置将缓冲区写入磁盘的临界大小。
        /// </summary>
        public int FlushSize
        {
            get
            {
                return m_FlushSize;
            }
            set
            {
                m_FlushSize = value;
            }
        }

        /// <summary>
        /// 获取或设置下载超时时长，以秒为单位。
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
        /// 获取当前下载速度。
        /// </summary>
        public float CurrentSpeed
        {
            get
            {
                return m_DownloadCounter.CurrentSpeed;
            }
        }

        /// <summary>
        /// 下载开始事件。
        /// </summary>
        public event EventHandler<DownloadStartEventArgs> DownloadStart
        {
            add
            {
                m_DownloadStartEventHandler += value;
            }
            remove
            {
                m_DownloadStartEventHandler -= value;
            }
        }

        /// <summary>
        /// 下载更新事件。
        /// </summary>
        public event EventHandler<DownloadUpdateEventArgs> DownloadUpdate
        {
            add
            {
                m_DownloadUpdateEventHandler += value;
            }
            remove
            {
                m_DownloadUpdateEventHandler -= value;
            }
        }

        /// <summary>
        /// 下载成功事件。
        /// </summary>
        public event EventHandler<DownloadSuccessEventArgs> DownloadSuccess
        {
            add
            {
                m_DownloadSuccessEventHandler += value;
            }
            remove
            {
                m_DownloadSuccessEventHandler -= value;
            }
        }

        /// <summary>
        /// 下载失败事件。
        /// </summary>
        public event EventHandler<DownloadFailureEventArgs> DownloadFailure
        {
            add
            {
                m_DownloadFailureEventHandler += value;
            }
            remove
            {
                m_DownloadFailureEventHandler -= value;
            }
        }

        /// <summary>
        /// 下载管理器轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {
            m_TaskPool.Update(elapseSeconds, realElapseSeconds);
            m_DownloadCounter.Update(elapseSeconds, realElapseSeconds);
        }

        /// <summary>
        /// 关闭并清理下载管理器。
        /// </summary>
        internal override void Shutdown()
        {
            m_TaskPool.Shutdown();
            m_DownloadCounter.Shutdown();
        }

        /// <summary>
        /// 增加下载代理辅助器。
        /// </summary>
        /// <param name="downloadAgentHelper">要增加的下载代理辅助器。</param>
        public void AddDownloadAgentHelper(IDownloadAgentHelper downloadAgentHelper)
        {
            DownloadAgent agent = new DownloadAgent(downloadAgentHelper);
            agent.DownloadAgentStart += OnDownloadAgentStart;
            agent.DownloadAgentUpdate += OnDownloadAgentUpdate;
            agent.DownloadAgentSuccess += OnDownloadAgentSuccess;
            agent.DownloadAgentFailure += OnDownloadAgentFailure;

            m_TaskPool.AddAgent(agent);
        }

        /// <summary>
        /// 增加下载任务。
        /// </summary>
        /// <param name="downloadPath">下载后存放路径。</param>
        /// <param name="downloadUri">原始下载地址。</param>
        /// <returns>新增下载任务的序列编号。</returns>
        public int AddDownload(string downloadPath, string downloadUri)
        {
            return AddDownload(downloadPath, downloadUri, Constant.DefaultPriority, null);
        }

        /// <summary>
        /// 增加下载任务。
        /// </summary>
        /// <param name="downloadPath">下载后存放路径。</param>
        /// <param name="downloadUri">原始下载地址。</param>
        /// <param name="priority">下载任务的优先级。</param>
        /// <returns>新增下载任务的序列编号。</returns>
        public int AddDownload(string downloadPath, string downloadUri, int priority)
        {
            return AddDownload(downloadPath, downloadUri, priority, null);
        }

        /// <summary>
        /// 增加下载任务。
        /// </summary>
        /// <param name="downloadPath">下载后存放路径。</param>
        /// <param name="downloadUri">原始下载地址。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>新增下载任务的序列编号。</returns>
        public int AddDownload(string downloadPath, string downloadUri, object userData)
        {
            return AddDownload(downloadPath, downloadUri, Constant.DefaultPriority, userData);
        }

        /// <summary>
        /// 增加下载任务。
        /// </summary>
        /// <param name="downloadPath">下载后存放路径。</param>
        /// <param name="downloadUri">原始下载地址。</param>
        /// <param name="priority">下载任务的优先级。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>新增下载任务的序列编号。</returns>
        public int AddDownload(string downloadPath, string downloadUri, int priority, object userData)
        {
            if (string.IsNullOrEmpty(downloadPath))
            {
                throw new GameFrameworkException("Download path is invalid.");
            }

            if (string.IsNullOrEmpty(downloadUri))
            {
                throw new GameFrameworkException("Download uri is invalid.");
            }

            if (TotalAgentCount <= 0)
            {
                throw new GameFrameworkException("You must add download agent first.");
            }

            DownloadTask downloadTask = new DownloadTask(downloadPath, downloadUri, priority, m_FlushSize, m_Timeout, userData);
            m_TaskPool.AddTask(downloadTask);

            return downloadTask.SerialId;
        }

        /// <summary>
        /// 移除下载任务。
        /// </summary>
        /// <param name="serialId">要移除下载任务的序列编号。</param>
        /// <returns>是否移除下载任务成功。</returns>
        public bool RemoveDownload(int serialId)
        {
            return m_TaskPool.RemoveTask(serialId) != null;
        }

        /// <summary>
        /// 移除所有下载任务。
        /// </summary>
        public void RemoveAllDownload()
        {
            m_TaskPool.RemoveAllTasks();
        }

        private void OnDownloadAgentStart(DownloadAgent sender)
        {
            if (m_DownloadStartEventHandler != null)
            {
                m_DownloadStartEventHandler(this, new DownloadStartEventArgs(sender.Task.SerialId, sender.Task.DownloadPath, sender.Task.DownloadUri, sender.CurrentLength, sender.Task.UserData));
            }
        }

        private void OnDownloadAgentUpdate(DownloadAgent sender, int lastDownloadedLength)
        {
            m_DownloadCounter.RecordDownloadedLength(lastDownloadedLength);
            if (m_DownloadUpdateEventHandler != null)
            {
                m_DownloadUpdateEventHandler(this, new DownloadUpdateEventArgs(sender.Task.SerialId, sender.Task.DownloadPath, sender.Task.DownloadUri, sender.CurrentLength, sender.Task.UserData));
            }
        }

        private void OnDownloadAgentSuccess(DownloadAgent sender, int lastDownloadedLength)
        {
            m_DownloadCounter.RecordDownloadedLength(lastDownloadedLength);
            if (m_DownloadSuccessEventHandler != null)
            {
                m_DownloadSuccessEventHandler(this, new DownloadSuccessEventArgs(sender.Task.SerialId, sender.Task.DownloadPath, sender.Task.DownloadUri, sender.CurrentLength, sender.Task.UserData));
            }
        }

        private void OnDownloadAgentFailure(DownloadAgent sender, string errorMessage)
        {
            if (m_DownloadFailureEventHandler != null)
            {
                m_DownloadFailureEventHandler(this, new DownloadFailureEventArgs(sender.Task.SerialId, sender.Task.DownloadPath, sender.Task.DownloadUri, errorMessage, sender.Task.UserData));
            }
        }
    }
}
