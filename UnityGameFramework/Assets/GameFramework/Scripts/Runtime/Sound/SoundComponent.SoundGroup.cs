//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    public partial class SoundComponent
    {
        [Serializable]
        private sealed class SoundGroup
        {
            [SerializeField]
            private string m_Name = null;

            [SerializeField]
            private bool m_AvoidBeingReplacedBySamePriority = false;

            [SerializeField]
            private bool m_Mute = false;

            [SerializeField, Range(0f, 1f)]
            private float m_Volume = 1f;

            [SerializeField]
            private int m_AgentHelperCount = 1;

            public string Name
            {
                get
                {
                    return m_Name;
                }
            }

            public bool AvoidBeingReplacedBySamePriority
            {
                get
                {
                    return m_AvoidBeingReplacedBySamePriority;
                }
            }

            public bool Mute
            {
                get
                {
                    return m_Mute;
                }
            }

            public float Volume
            {
                get
                {
                    return m_Volume;
                }
            }

            public int AgentHelperCount
            {
                get
                {
                    return m_AgentHelperCount;
                }
            }
        }
    }
}
