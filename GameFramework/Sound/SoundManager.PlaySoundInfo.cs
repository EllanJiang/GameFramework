//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Sound
{
    internal partial class SoundManager
    {
        private sealed class PlaySoundInfo
        {
            private readonly int m_SerialId;
            private readonly SoundGroup m_SoundGroup;
            private readonly PlaySoundParams m_PlaySoundParams;
            private readonly object m_UserData;

            public PlaySoundInfo(int serialId, SoundGroup soundGroup, PlaySoundParams playSoundParams, object userData)
            {
                m_SerialId = serialId;
                m_SoundGroup = soundGroup;
                m_PlaySoundParams = playSoundParams;
                m_UserData = userData;
            }

            public int SerialId
            {
                get
                {
                    return m_SerialId;
                }
            }

            public SoundGroup SoundGroup
            {
                get
                {
                    return m_SoundGroup;
                }
            }

            public PlaySoundParams PlaySoundParams
            {
                get
                {
                    return m_PlaySoundParams;
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
