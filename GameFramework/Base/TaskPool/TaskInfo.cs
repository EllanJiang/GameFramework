//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System;

namespace GameFramework
{
    /// <summary>
    /// 任务信息。
    /// </summary>
    public struct TaskInfo
    {
        private readonly int m_SerialId;
        private readonly int m_Priority;
        private readonly bool m_Done;
        private readonly bool m_Working;
        private readonly string m_Description;

        /// <summary>
        /// 初始化任务信息的新实例。
        /// </summary>
        /// <param name="serialId">任务的序列编号。</param>
        /// <param name="priority">任务的优先级。</param>
        /// <param name="done">任务是否完成。</param>
        /// <param name="working">任务是否正在进行。</param>
        /// <param name="description">任务描述。</param>
        public TaskInfo(int serialId, int priority, bool done, bool working, string description)
        {
            m_SerialId = serialId;
            m_Priority = priority;
            m_Done = done;
            m_Working = working;
            m_Description = description;
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
        /// 获取任务是否完成。
        /// </summary>
        public bool Done
        {
            get
            {
                return m_Done;
            }
        }

        /// <summary>
        /// 获取任务是否正在进行。
        /// </summary>
        public bool Working
        {
            get
            {
                return m_Working;
            }
        }

        /// <summary>
        /// 获取任务描述。
        /// </summary>
        public string Description
        {
            get
            {
                return m_Description;
            }
        }
    }
}
