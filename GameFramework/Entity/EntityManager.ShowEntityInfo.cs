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
        private sealed class ShowEntityInfo
        {
            private readonly int m_EntityId;
            private readonly string m_EntityGroupName;
            private readonly object m_UserData;

            public ShowEntityInfo(int entityId, string entityGroupName, object userData)
            {
                m_EntityId = entityId;
                m_EntityGroupName = entityGroupName;
                m_UserData = userData;
            }

            public int EntityId
            {
                get
                {
                    return m_EntityId;
                }
            }

            public string EntityGroupName
            {
                get
                {
                    return m_EntityGroupName;
                }
            }

            public object UserData
            {
                get
                {
                    return m_UserData;
                }
            }
        }
    }
}
