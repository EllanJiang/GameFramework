//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;

namespace GameFramework.Fsm
{
    /// <summary>
    /// 有限状态机基类。
    /// </summary>
    public abstract class FsmBase
    {
        private string m_Name;

        /// <summary>
        /// 初始化有限状态机基类的新实例。
        /// </summary>
        public FsmBase()
        {
            m_Name = string.Empty;
        }

        /// <summary>
        /// 获取有限状态机名称。
        /// </summary>
        public string Name
        {
            get
            {
                return m_Name;
            }
            protected set
            {
                m_Name = value ?? string.Empty;
            }
        }

        /// <summary>
        /// 获取有限状态机完整名称。
        /// </summary>
        public string FullName
        {
            get
            {
                return new TypeNamePair(OwnerType, m_Name).ToString();
            }
        }

        /// <summary>
        /// 获取有限状态机持有者类型。
        /// </summary>
        public abstract Type OwnerType
        {
            get;
        }

        /// <summary>
        /// 获取有限状态机中状态的数量。
        /// </summary>
        public abstract int FsmStateCount
        {
            get;
        }

        /// <summary>
        /// 获取有限状态机是否正在运行。
        /// </summary>
        public abstract bool IsRunning
        {
            get;
        }

        /// <summary>
        /// 获取有限状态机是否被销毁。
        /// </summary>
        public abstract bool IsDestroyed
        {
            get;
        }

        /// <summary>
        /// 获取当前有限状态机状态名称。
        /// </summary>
        public abstract string CurrentStateName
        {
            get;
        }

        /// <summary>
        /// 获取当前有限状态机状态持续时间。
        /// </summary>
        public abstract float CurrentStateTime
        {
            get;
        }

        /// <summary>
        /// 有限状态机轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">当前已流逝时间，以秒为单位。</param>
        internal abstract void Update(float elapseSeconds, float realElapseSeconds);

        /// <summary>
        /// 关闭并清理有限状态机。
        /// </summary>
        internal abstract void Shutdown();
    }
}
