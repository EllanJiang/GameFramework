﻿//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.ObjectPool;
using System.Collections.Generic;

namespace GameFramework.Entity
{
    internal partial class EntityManager
    {
        /// <summary>
        /// 实体组。
        /// </summary>
        private sealed class EntityGroup : IEntityGroup
        {
            private readonly string m_Name;
            private readonly IEntityGroupHelper m_EntityGroupHelper;
            private readonly IObjectPool<EntityInstanceObject> m_InstancePool;
            private readonly LinkedList<IEntity> m_Entities;

            /// <summary>
            /// 初始化实体组的新实例。
            /// </summary>
            /// <param name="name">实体组名称。</param>
            /// <param name="instanceAutoReleaseInterval">实体实例对象池自动释放可释放对象的间隔秒数。</param>
            /// <param name="instanceCapacity">实体实例对象池容量。</param>
            /// <param name="instanceExpireTime">实体实例对象池对象过期秒数。</param>
            /// <param name="instancePriority">实体实例对象池的优先级。</param>
            /// <param name="entityGroupHelper">实体组辅助器。</param>
            /// <param name="objectPoolManager">对象池管理器。</param>
            public EntityGroup(string name, float instanceAutoReleaseInterval, int instanceCapacity, float instanceExpireTime, int instancePriority, IEntityGroupHelper entityGroupHelper, IObjectPoolManager objectPoolManager)
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new GameFrameworkException("Entity group name is invalid.");
                }

                if (entityGroupHelper == null)
                {
                    throw new GameFrameworkException("Entity group helper is invalid.");
                }

                m_Name = name;
                m_EntityGroupHelper = entityGroupHelper;
                m_InstancePool = objectPoolManager.CreateSingleSpawnObjectPool<EntityInstanceObject>(string.Format("Entity Instance Pool ({0})", name), instanceCapacity, instanceExpireTime, instancePriority);
                m_InstancePool.AutoReleaseInterval = instanceAutoReleaseInterval;
                m_Entities = new LinkedList<IEntity>();
            }

            /// <summary>
            /// 获取实体组名称。
            /// </summary>
            public string Name
            {
                get
                {
                    return m_Name;
                }
            }

            /// <summary>
            /// 获取实体组中实体数量。
            /// </summary>
            public int EntityCount
            {
                get
                {
                    return m_Entities.Count;
                }
            }

            /// <summary>
            /// 获取或设置实体组实例对象池自动释放可释放对象的间隔秒数。
            /// </summary>
            public float InstanceAutoReleaseInterval
            {
                get
                {
                    return m_InstancePool.AutoReleaseInterval;
                }
                set
                {
                    m_InstancePool.AutoReleaseInterval = value;
                }
            }

            /// <summary>
            /// 获取或设置实体组实例对象池的容量。
            /// </summary>
            public int InstanceCapacity
            {
                get
                {
                    return m_InstancePool.Capacity;
                }
                set
                {
                    m_InstancePool.Capacity = value;
                }
            }

            /// <summary>
            /// 获取或设置实体组实例对象池对象过期秒数。
            /// </summary>
            public float InstanceExpireTime
            {
                get
                {
                    return m_InstancePool.ExpireTime;
                }
                set
                {
                    m_InstancePool.ExpireTime = value;
                }
            }

            /// <summary>
            /// 获取或设置实体组实例对象池的优先级。
            /// </summary>
            public int InstancePriority
            {
                get
                {
                    return m_InstancePool.Priority;
                }
                set
                {
                    m_InstancePool.Priority = value;
                }
            }

            /// <summary>
            /// 获取实体组辅助器。
            /// </summary>
            public IEntityGroupHelper Helper
            {
                get
                {
                    return m_EntityGroupHelper;
                }
            }

            /// <summary>
            /// 实体组轮询。
            /// </summary>
            /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
            /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
            public void Update(float elapseSeconds, float realElapseSeconds)
            {
                LinkedListNode<IEntity> current = m_Entities.First;
                while (current != null)
                {
                    LinkedListNode<IEntity> next = current.Next;
                    current.Value.OnUpdate(elapseSeconds, realElapseSeconds);
                    current = next;
                }
            }

            /// <summary>
            /// 实体组中是否存在实体。
            /// </summary>
            /// <param name="entityId">实体序列编号。</param>
            /// <returns>实体组中是否存在实体。</returns>
            public bool HasEntity(int entityId)
            {
                foreach (IEntity entity in m_Entities)
                {
                    if (entity.Id == entityId)
                    {
                        return true;
                    }
                }

                return false;
            }

            /// <summary>
            /// 实体组中是否存在实体。
            /// </summary>
            /// <param name="entityAssetName">实体资源名称。</param>
            /// <returns>实体组中是否存在实体。</returns>
            public bool HasEntity(string entityAssetName)
            {
                if (string.IsNullOrEmpty(entityAssetName))
                {
                    throw new GameFrameworkException("Entity asset name is invalid.");
                }

                foreach (IEntity entity in m_Entities)
                {
                    if (entity.EntityAssetName == entityAssetName)
                    {
                        return true;
                    }
                }

                return false;
            }

            /// <summary>
            /// 从实体组中获取实体。
            /// </summary>
            /// <param name="entityId">实体序列编号。</param>
            /// <returns>要获取的实体。</returns>
            public IEntity GetEntity(int entityId)
            {
                foreach (IEntity entity in m_Entities)
                {
                    if (entity.Id == entityId)
                    {
                        return entity;
                    }
                }

                return null;
            }

            /// <summary>
            /// 从实体组中获取实体。
            /// </summary>
            /// <param name="entityAssetName">实体资源名称。</param>
            /// <returns>要获取的实体。</returns>
            public IEntity GetEntity(string entityAssetName)
            {
                if (string.IsNullOrEmpty(entityAssetName))
                {
                    throw new GameFrameworkException("Entity asset name is invalid.");
                }

                foreach (IEntity entity in m_Entities)
                {
                    if (entity.EntityAssetName == entityAssetName)
                    {
                        return entity;
                    }
                }

                return null;
            }

            /// <summary>
            /// 从实体组中获取实体。
            /// </summary>
            /// <param name="entityAssetName">实体资源名称。</param>
            /// <returns>要获取的实体。</returns>
            public IEntity[] GetEntities(string entityAssetName)
            {
                if (string.IsNullOrEmpty(entityAssetName))
                {
                    throw new GameFrameworkException("Entity asset name is invalid.");
                }

                List<IEntity> entities = new List<IEntity>();
                foreach (IEntity entity in m_Entities)
                {
                    if (entity.EntityAssetName == entityAssetName)
                    {
                        entities.Add(entity);
                    }
                }

                return entities.ToArray();
            }

            /// <summary>
            /// 从实体组中获取所有实体。
            /// </summary>
            /// <returns>实体组中的所有实体。</returns>
            public IEntity[] GetAllEntities()
            {
                List<IEntity> entities = new List<IEntity>();
                foreach (IEntity entity in m_Entities)
                {
                    entities.Add(entity);
                }

                return entities.ToArray();
            }

            /// <summary>
            /// 往实体组增加实体。
            /// </summary>
            /// <param name="entity">要增加的实体。</param>
            public void AddEntity(IEntity entity)
            {
                m_Entities.AddLast(entity);
            }

            /// <summary>
            /// 从实体组移除实体。
            /// </summary>
            /// <param name="entity">要移除的实体。</param>
            public void RemoveEntity(IEntity entity)
            {
                m_Entities.Remove(entity);
            }

            public void RegisterEntityInstanceObject(EntityInstanceObject obj, bool spawned)
            {
                m_InstancePool.Register(obj, spawned);
            }

            public EntityInstanceObject SpawnEntityInstanceObject(string name)
            {
                return m_InstancePool.Spawn(name);
            }

            public void UnspawnEntity(IEntity entity)
            {
                m_InstancePool.Unspawn(entity.Handle);
            }

            public void SetInstanceLocked(IEntity entity, bool locked)
            {
                m_InstancePool.SetLocked(entity.Handle, locked);
            }

            public void SetInstancePriority(IEntity entity, int priority)
            {
                m_InstancePool.SetPriority(entity, priority);
            }
        }
    }
}
