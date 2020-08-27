//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;

namespace GameFramework.ObjectPool
{
    /// <summary>
    /// 对象池基类。
    /// </summary>
    public abstract class ObjectPoolBase
    {
        private readonly string m_Name;

        /// <summary>
        /// 初始化对象池基类的新实例。
        /// </summary>
        public ObjectPoolBase()
            : this(null)
        {
        }

        /// <summary>
        /// 初始化对象池基类的新实例。
        /// </summary>
        /// <param name="name">对象池名称。</param>
        public ObjectPoolBase(string name)
        {
            m_Name = name ?? string.Empty;
        }

        /// <summary>
        /// 获取对象池名称。
        /// </summary>
        public string Name
        {
            get
            {
                return m_Name;
            }
        }

        /// <summary>
        /// 获取对象池完整名称。
        /// </summary>
        public string FullName
        {
            get
            {
                return new TypeNamePair(ObjectType, m_Name).ToString();
            }
        }

        /// <summary>
        /// 获取对象池对象类型。
        /// </summary>
        public abstract Type ObjectType
        {
            get;
        }

        /// <summary>
        /// 获取对象池中对象的数量。
        /// </summary>
        public abstract int Count
        {
            get;
        }

        /// <summary>
        /// 获取对象池中能被释放的对象的数量。
        /// </summary>
        public abstract int CanReleaseCount
        {
            get;
        }

        /// <summary>
        /// 获取是否允许对象被多次获取。
        /// </summary>
        public abstract bool AllowMultiSpawn
        {
            get;
        }

        /// <summary>
        /// 获取或设置对象池自动释放可释放对象的间隔秒数。
        /// </summary>
        public abstract float AutoReleaseInterval
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置对象池的容量。
        /// </summary>
        public abstract int Capacity
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置对象池对象过期秒数。
        /// </summary>
        public abstract float ExpireTime
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置对象池的优先级。
        /// </summary>
        public abstract int Priority
        {
            get;
            set;
        }

        /// <summary>
        /// 释放对象池中的可释放对象。
        /// </summary>
        public abstract void Release();

        /// <summary>
        /// 释放对象池中的可释放对象。
        /// </summary>
        /// <param name="toReleaseCount">尝试释放对象数量。</param>
        public abstract void Release(int toReleaseCount);

        /// <summary>
        /// 释放对象池中的所有未使用对象。
        /// </summary>
        public abstract void ReleaseAllUnused();

        /// <summary>
        /// 获取所有对象信息。
        /// </summary>
        /// <returns>所有对象信息。</returns>
        public abstract ObjectInfo[] GetAllObjectInfos();

        internal abstract void Update(float elapseSeconds, float realElapseSeconds);

        internal abstract void Shutdown();
    }
}
