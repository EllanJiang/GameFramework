//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System.Collections.Generic;

namespace GameFramework.Entity
{
    internal partial class EntityManager
    {
        /// <summary>
        /// 实体信息。
        /// </summary>
        private sealed class EntityInfo
        {
            private static readonly IEntity[] EmptyArray = new IEntity[] { };

            private readonly IEntity m_Entity;
            private EntityStatus m_Status;
            private IEntity m_ParentEntity;
            private List<IEntity> m_ChildEntities;

            public EntityInfo(IEntity entity)
            {
                if (entity == null)
                {
                    throw new GameFrameworkException("Entity is invalid.");
                }

                m_Entity = entity;
                m_Status = EntityStatus.WillInit;
                m_ParentEntity = null;
                m_ChildEntities = null;
            }

            public IEntity Entity
            {
                get
                {
                    return m_Entity;
                }
            }

            public EntityStatus Status
            {
                get
                {
                    return m_Status;
                }
                set
                {
                    m_Status = value;
                }
            }

            public IEntity ParentEntity
            {
                get
                {
                    return m_ParentEntity;
                }
                set
                {
                    m_ParentEntity = value;
                }
            }

            public IEntity[] GetChildEntities()
            {
                if (m_ChildEntities == null)
                {
                    return EmptyArray;
                }

                return m_ChildEntities.ToArray();
            }

            public void GetChildEntities(List<IEntity> results)
            {
                if (results == null)
                {
                    throw new GameFrameworkException("Results is invalid.");
                }

                results.Clear();
                if (m_ChildEntities == null)
                {
                    return;
                }

                foreach (IEntity childEntity in m_ChildEntities)
                {
                    results.Add(childEntity);
                }
            }

            public void AddChildEntity(IEntity childEntity)
            {
                if (m_ChildEntities == null)
                {
                    m_ChildEntities = new List<IEntity>();
                }

                if (m_ChildEntities.Contains(childEntity))
                {
                    throw new GameFrameworkException("Can not add child entity which is already exist.");
                }

                m_ChildEntities.Add(childEntity);
            }

            public void RemoveChildEntity(IEntity childEntity)
            {
                if (m_ChildEntities == null || !m_ChildEntities.Remove(childEntity))
                {
                    throw new GameFrameworkException("Can not remove child entity which is not exist.");
                }
            }
        }
    }
}
