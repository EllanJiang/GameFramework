//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Entity
{
    internal partial class EntityManager
    {
        private sealed class RecycleNode
        {
            private readonly EntityInfo m_EntityInfo;
            private int m_TickCount;

            public RecycleNode(EntityInfo entityInfo)
            {
                m_EntityInfo = entityInfo;
                m_TickCount = 0;
            }

            public EntityInfo EntityInfo
            {
                get
                {
                    return m_EntityInfo;
                }
            }

            public int TickCount
            {
                get
                {
                    return m_TickCount;
                }
                set
                {
                    m_TickCount = value;
                }
            }
        }
    }
}
