//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace UnityGameFramework.Runtime
{
    internal sealed class PlaySoundInfo
    {
        private readonly Entity m_BindingEntity;
        private readonly object m_UserData;

        public PlaySoundInfo(Entity bindingEntity, object userData)
        {
            m_BindingEntity = bindingEntity;
            m_UserData = userData;
        }

        public Entity BindingEntity
        {
            get
            {
                return m_BindingEntity;
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
