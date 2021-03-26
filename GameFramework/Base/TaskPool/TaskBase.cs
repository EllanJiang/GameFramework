//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework
{
    /// <summary>
    /// 任务基类。
    /// </summary>
    internal abstract class TaskBase : IReference
    {
        /// <summary>
        /// 任务默认优先级。
        /// </summary>
        public const int DefaultPriority = 0;

        private int m_SerialId;
        private string m_Tag;
        private int m_Priority;
        private object m_UserData;

        private bool m_Done;

        /// <summary>
        /// 初始化任务基类的新实例。
        /// </summary>
        public TaskBase()
        {
            m_SerialId = 0;
            m_Tag = null;
            m_Priority = DefaultPriority;
            m_Done = false;
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
        /// 获取任务的标签。
        /// </summary>
        public string Tag
        {
            get
            {
                return m_Tag;
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
        /// 获取任务的用户自定义数据。
        /// </summary>
        public object UserData
        {
            get
            {
                return m_UserData;
            }
        }

        /// <summary>
        /// 获取或设置任务是否完成。
        /// </summary>
        public bool Done
        {
            get
            {
                return m_Done;
            }
            set
            {
                m_Done = value;
            }
        }

        /// <summary>
        /// 获取任务描述。
        /// </summary>
        public virtual string Description
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// 初始化任务基类。
        /// </summary>
        /// <param name="serialId">任务的序列编号。</param>
        /// <param name="tag">任务的标签。</param>
        /// <param name="priority">任务的优先级。</param>
        /// <param name="userData">任务的用户自定义数据。</param>
        internal void Initialize(int serialId, string tag, int priority, object userData)
        {
            m_SerialId = serialId;
            m_Tag = tag;
            m_Priority = priority;
            m_UserData = userData;
            m_Done = false;
        }

        /// <summary>
        /// 清理任务基类。
        /// </summary>
        public virtual void Clear()
        {
            m_SerialId = 0;
            m_Tag = null;
            m_Priority = DefaultPriority;
            m_UserData = null;
            m_Done = false;
        }
    }
}
