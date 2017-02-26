//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System;

namespace UnityGameFramework.Runtime
{
    internal sealed class ShowEntityInfo
    {
        private readonly Type m_EntityLogicType;
        private readonly object m_UserData;

        public ShowEntityInfo(Type entityLogicType, object userData)
        {
            m_EntityLogicType = entityLogicType;
            m_UserData = userData;
        }

        public Type EntityLogicType
        {
            get
            {
                return m_EntityLogicType;
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
