//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.ObjectPool;

namespace GameFramework.Entity
{
    internal partial class EntityManager
    {
        /// <summary>
        /// 实体实例对象。
        /// </summary>
        private sealed class EntityInstanceObject : ObjectBase
        {
            private readonly IEntityHelper m_EntityHelper;

            public EntityInstanceObject(string name, object target, IEntityHelper entityHelper)
                : base(name, target)
            {
                if (entityHelper == null)
                {
                    throw new GameFrameworkException("Entity helper is invalid.");
                }

                m_EntityHelper = entityHelper;
            }

            protected internal override void Release()
            {
                m_EntityHelper.ReleaseEntityInstance(Target);
            }
        }
    }
}
