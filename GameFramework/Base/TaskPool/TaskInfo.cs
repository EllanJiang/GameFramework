//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System.Runtime.InteropServices;

namespace GameFramework
{
    /// <summary>
    /// 任务信息。
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct TaskInfo
    {
        private readonly bool m_IsValid;
        private readonly int m_SerialId;
        private readonly string m_Tag;
        private readonly int m_Priority;
        private readonly object m_UserData;
        private readonly TaskStatus m_Status;
        private readonly string m_Description;

        /// <summary>
        /// 初始化任务信息的新实例。
        /// </summary>
        /// <param name="serialId">任务的序列编号。</param>
        /// <param name="tag">任务的标签。</param>
        /// <param name="priority">任务的优先级。</param>
        /// <param name="userData">任务的用户自定义数据。</param>
        /// <param name="status">任务状态。</param>
        /// <param name="description">任务描述。</param>
        public TaskInfo(int serialId, string tag, int priority, object userData, TaskStatus status, string description)
        {
            m_IsValid = true;
            m_SerialId = serialId;
            m_Tag = tag;
            m_Priority = priority;
            m_UserData = userData;
            m_Status = status;
            m_Description = description;
        }

        /// <summary>
        /// 获取任务信息是否有效。
        /// </summary>
        public bool IsValid
        {
            get
            {
                return m_IsValid;
            }
        }

        /// <summary>
        /// 获取任务的序列编号。
        /// </summary>
        public int SerialId
        {
            get
            {
                if (!m_IsValid)
                {
                    throw new GameFrameworkException("Data is invalid.");
                }

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
                if (!m_IsValid)
                {
                    throw new GameFrameworkException("Data is invalid.");
                }

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
                if (!m_IsValid)
                {
                    throw new GameFrameworkException("Data is invalid.");
                }

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
                if (!m_IsValid)
                {
                    throw new GameFrameworkException("Data is invalid.");
                }

                return m_UserData;
            }
        }

        /// <summary>
        /// 获取任务状态。
        /// </summary>
        public TaskStatus Status
        {
            get
            {
                if (!m_IsValid)
                {
                    throw new GameFrameworkException("Data is invalid.");
                }

                return m_Status;
            }
        }

        /// <summary>
        /// 获取任务描述。
        /// </summary>
        public string Description
        {
            get
            {
                if (!m_IsValid)
                {
                    throw new GameFrameworkException("Data is invalid.");
                }

                return m_Description;
            }
        }
    }
}
