//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework.ObjectPool;

namespace GameFramework.Entity
{
    internal sealed partial class EntityManager : GameFrameworkModule, IEntityManager
    {
        /// <summary>
        /// 实体实例对象。
        /// </summary>
        private sealed class EntityInstanceObject : ObjectBase
        {
            private object m_EntityAsset;
            private IEntityHelper m_EntityHelper;

            public EntityInstanceObject()
            {
                m_EntityAsset = null;
                m_EntityHelper = null;
            }

            public static EntityInstanceObject Create(string name, object entityAsset, object entityInstance, IEntityHelper entityHelper)
            {
                if (entityAsset == null)
                {
                    throw new GameFrameworkException("Entity asset is invalid.");
                }

                if (entityHelper == null)
                {
                    throw new GameFrameworkException("Entity helper is invalid.");
                }

                EntityInstanceObject entityInstanceObject = ReferencePool.Acquire<EntityInstanceObject>();
                entityInstanceObject.Initialize(name, entityInstance);
                entityInstanceObject.m_EntityAsset = entityAsset;
                entityInstanceObject.m_EntityHelper = entityHelper;
                return entityInstanceObject;
            }

            public override void Clear()
            {
                base.Clear();
                m_EntityAsset = null;
                m_EntityHelper = null;
            }

            protected internal override void Release(bool isShutdown)
            {
                m_EntityHelper.ReleaseEntity(m_EntityAsset, Target);
            }
        }
    }
}
