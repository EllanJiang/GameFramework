//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using UnityEngine;

namespace UnityGameFramework.Runtime
{
    internal sealed class PlaySoundInfo
    {
        private readonly Entity m_BindingEntity;
        private readonly Vector3 m_WorldPosition;
        private readonly object m_UserData;

        public PlaySoundInfo(Entity bindingEntity, Vector3 worldPosition, object userData)
        {
            m_BindingEntity = bindingEntity;
            m_WorldPosition = worldPosition;
            m_UserData = userData;
        }

        public Entity BindingEntity
        {
            get
            {
                return m_BindingEntity;
            }
        }

        public Vector3 WorldPosition
        {
            get
            {
                return m_WorldPosition;
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
