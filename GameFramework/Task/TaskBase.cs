//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Task
{
    /// <summary>
    /// 任务基类。
    /// </summary>
    public abstract class TaskBase : IReference
    {
        /// <summary>
        /// 任务默认优先级。
        /// </summary>
        public const int DefaultPriority = 0;

        private int m_SerialId;
        private int m_Priority;
        private TaskStatus m_Status;
        private object m_UserData;

        /// <summary>
        /// 初始化任务基类的新实例。
        /// </summary>
        public TaskBase()
        {
            m_SerialId = 0;
            m_Priority = DefaultPriority;
            m_Status = TaskStatus.Free;
            m_UserData = null;
        }

        /// <summary>
        /// 获取任务的序列编号。
        /// </summary>
        public int SerialId
        {
            get
            {
                return m_SerialId;
            }
        }

        /// <summary>
        /// 获取任务的优先级。
        /// </summary>
        public int Priority
        {
            get
            {
                return m_Priority;
            }
        }

        /// <summary>
        /// 获取任务的状态。
        /// </summary>
        public TaskStatus Status
        {
            get
            {
                return m_Status;
            }
        }

        /// <summary>
        /// 获取或设置用户自定义数据。
        /// </summary>
        public object UserData
        {
            get
            {
                return m_UserData;
            }
            set
            {
                m_UserData = value;
            }
        }

        /// <summary>
        /// 初始化任务基类。
        /// </summary>
        /// <param name="serialId">任务的序列编号。</param>
        /// <param name="priority">任务的优先级。</param>
        internal void Initialize(int serialId, int priority)
        {
            m_SerialId = serialId;
            m_Priority = priority;
        }

        /// <summary>
        /// 清理任务基类。
        /// </summary>
        public virtual void Clear()
        {
            m_SerialId = 0;
            m_Priority = DefaultPriority;
            m_Status = TaskStatus.Free;
            m_UserData = null;
        }

        /// <summary>
        /// 任务生成时调用。
        /// </summary>
        protected internal virtual void OnGenerate()
        {
            m_Status = TaskStatus.Waiting;
        }

        /// <summary>
        /// 任务开始时调用。
        /// </summary>
        protected internal virtual void OnStart()
        {
            m_Status = TaskStatus.Running;
        }

        /// <summary>
        /// 任务轮询时调用。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        protected internal virtual void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
        }

        /// <summary>
        /// 任务完成时调用。
        /// </summary>
        /// <param name="reason">任务完成的原因。</param>
        protected internal virtual void OnComplete(string reason)
        {
            m_Status = TaskStatus.Completed;
        }

        /// <summary>
        /// 任务失败时调用。
        /// </summary>
        /// <param name="reason">任务失败的原因。</param>
        protected internal virtual void OnFailure(string reason)
        {
            m_Status = TaskStatus.Failed;
        }

        /// <summary>
        /// 任务取消时调用。
        /// </summary>
        /// <param name="reason">任务取消的原因。</param>
        protected internal virtual void OnCancel(string reason)
        {
            m_Status = TaskStatus.Canceled;
        }
    }
}
