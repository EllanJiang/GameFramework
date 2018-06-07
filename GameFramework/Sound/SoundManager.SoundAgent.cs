//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System;

namespace GameFramework.Sound
{
    internal partial class SoundManager
    {
        /// <summary>
        /// 声音代理。
        /// </summary>
        private sealed class SoundAgent : ISoundAgent
        {
            private readonly SoundGroup m_SoundGroup;
            private readonly ISoundHelper m_SoundHelper;
            private readonly ISoundAgentHelper m_SoundAgentHelper;
            private int m_SerialId;
            private object m_SoundAsset;
            private DateTime m_SetSoundAssetTime;
            private bool m_MuteInSoundGroup;
            private float m_VolumeInSoundGroup;

            /// <summary>
            /// 初始化声音代理的新实例。
            /// </summary>
            /// <param name="soundGroup">所在的声音组。</param>
            /// <param name="soundHelper">声音辅助器接口。</param>
            /// <param name="soundAgentHelper">声音代理辅助器接口。</param>
            public SoundAgent(SoundGroup soundGroup, ISoundHelper soundHelper, ISoundAgentHelper soundAgentHelper)
            {
                if (soundGroup == null)
                {
                    throw new GameFrameworkException("Sound group is invalid.");
                }

                if (soundHelper == null)
                {
                    throw new GameFrameworkException("Sound helper is invalid.");
                }

                if (soundAgentHelper == null)
                {
                    throw new GameFrameworkException("Sound agent helper is invalid.");
                }

                m_SoundGroup = soundGroup;
                m_SoundHelper = soundHelper;
                m_SoundAgentHelper = soundAgentHelper;
                m_SoundAgentHelper.ResetSoundAgent += OnResetSoundAgent;
                m_SerialId = 0;
                m_SoundAsset = null;
                Reset();
            }

            /// <summary>
            /// 获取所在的声音组。
            /// </summary>
            public ISoundGroup SoundGroup
            {
                get
                {
                    return m_SoundGroup;
                }
            }

            /// <summary>
            /// 获取或设置声音的序列编号。
            /// </summary>
            public int SerialId
            {
                get
                {
                    return m_SerialId;
                }
                set
                {
                    m_SerialId = value;
                }
            }

            /// <summary>
            /// 获取当前是否正在播放。
            /// </summary>
            public bool IsPlaying
            {
                get
                {
                    return m_SoundAgentHelper.IsPlaying;
                }
            }

            /// <summary>
            /// 获取或设置播放位置。
            /// </summary>
            public float Time
            {
                get
                {
                    return m_SoundAgentHelper.Time;
                }
                set
                {
                    m_SoundAgentHelper.Time = value;
                }
            }

            /// <summary>
            /// 获取是否静音。
            /// </summary>
            public bool Mute
            {
                get
                {
                    return m_SoundAgentHelper.Mute;
                }
            }

            /// <summary>
            /// 获取或设置在声音组内是否静音。
            /// </summary>
            public bool MuteInSoundGroup
            {
                get
                {
                    return m_MuteInSoundGroup;
                }
                set
                {
                    m_MuteInSoundGroup = value;
                    RefreshMute();
                }
            }

            /// <summary>
            /// 获取或设置是否循环播放。
            /// </summary>
            public bool Loop
            {
                get
                {
                    return m_SoundAgentHelper.Loop;
                }
                set
                {
                    m_SoundAgentHelper.Loop = value;
                }
            }

            /// <summary>
            /// 获取或设置声音优先级。
            /// </summary>
            public int Priority
            {
                get
                {
                    return m_SoundAgentHelper.Priority;
                }
                set
                {
                    m_SoundAgentHelper.Priority = value;
                }
            }

            /// <summary>
            /// 获取音量大小。
            /// </summary>
            public float Volume
            {
                get
                {
                    return m_SoundAgentHelper.Volume;
                }
            }

            /// <summary>
            /// 获取或设置在声音组内音量大小。
            /// </summary>
            public float VolumeInSoundGroup
            {
                get
                {
                    return m_VolumeInSoundGroup;
                }
                set
                {
                    m_VolumeInSoundGroup = value;
                    RefreshVolume();
                }
            }

            /// <summary>
            /// 获取或设置声音音调。
            /// </summary>
            public float Pitch
            {
                get
                {
                    return m_SoundAgentHelper.Pitch;
                }
                set
                {
                    m_SoundAgentHelper.Pitch = value;
                }
            }

            /// <summary>
            /// 获取或设置声音立体声声相。
            /// </summary>
            public float PanStereo
            {
                get
                {
                    return m_SoundAgentHelper.PanStereo;
                }
                set
                {
                    m_SoundAgentHelper.PanStereo = value;
                }
            }

            /// <summary>
            /// 获取或设置声音空间混合量。
            /// </summary>
            public float SpatialBlend
            {
                get
                {
                    return m_SoundAgentHelper.SpatialBlend;
                }
                set
                {
                    m_SoundAgentHelper.SpatialBlend = value;
                }
            }

            /// <summary>
            /// 获取或设置声音最大距离。
            /// </summary>
            public float MaxDistance
            {
                get
                {
                    return m_SoundAgentHelper.MaxDistance;
                }
                set
                {
                    m_SoundAgentHelper.MaxDistance = value;
                }
            }

            /// <summary>
            /// 获取或设置声音多普勒等级。
            /// </summary>
            public float DopplerLevel
            {
                get
                {
                    return m_SoundAgentHelper.DopplerLevel;
                }
                set
                {
                    m_SoundAgentHelper.DopplerLevel = value;
                }
            }

            /// <summary>
            /// 获取声音代理辅助器。
            /// </summary>
            public ISoundAgentHelper Helper
            {
                get
                {
                    return m_SoundAgentHelper;
                }
            }

            /// <summary>
            /// 获取声音创建时间。
            /// </summary>
            internal DateTime SetSoundAssetTime
            {
                get
                {
                    return m_SetSoundAssetTime;
                }
            }

            /// <summary>
            /// 播放声音。
            /// </summary>
            public void Play()
            {
                m_SoundAgentHelper.Play(Constant.DefaultFadeInSeconds);
            }

            /// <summary>
            /// 播放声音。
            /// </summary>
            /// <param name="fadeInSeconds">声音淡入时间，以秒为单位。</param>
            public void Play(float fadeInSeconds)
            {
                m_SoundAgentHelper.Play(fadeInSeconds);
            }

            /// <summary>
            /// 停止播放声音。
            /// </summary>
            public void Stop()
            {
                m_SoundAgentHelper.Stop(Constant.DefaultFadeOutSeconds);
            }

            /// <summary>
            /// 停止播放声音。
            /// </summary>
            /// <param name="fadeOutSeconds">声音淡出时间，以秒为单位。</param>
            public void Stop(float fadeOutSeconds)
            {
                m_SoundAgentHelper.Stop(fadeOutSeconds);
            }

            /// <summary>
            /// 暂停播放声音。
            /// </summary>
            public void Pause()
            {
                m_SoundAgentHelper.Pause(Constant.DefaultFadeOutSeconds);
            }

            /// <summary>
            /// 暂停播放声音。
            /// </summary>
            /// <param name="fadeOutSeconds">声音淡出时间，以秒为单位。</param>
            public void Pause(float fadeOutSeconds)
            {
                m_SoundAgentHelper.Pause(fadeOutSeconds);
            }

            /// <summary>
            /// 恢复播放声音。
            /// </summary>
            public void Resume()
            {
                m_SoundAgentHelper.Resume(Constant.DefaultFadeInSeconds);
            }

            /// <summary>
            /// 恢复播放声音。
            /// </summary>
            /// <param name="fadeInSeconds">声音淡入时间，以秒为单位。</param>
            public void Resume(float fadeInSeconds)
            {
                m_SoundAgentHelper.Resume(fadeInSeconds);
            }

            /// <summary>
            /// 重置声音代理。
            /// </summary>
            public void Reset()
            {
                if (m_SoundAsset != null)
                {
                    m_SoundHelper.ReleaseSoundAsset(m_SoundAsset);
                    m_SoundAsset = null;
                }

                m_SetSoundAssetTime = DateTime.MinValue;
                Time = Constant.DefaultTime;
                MuteInSoundGroup = Constant.DefaultMute;
                Loop = Constant.DefaultLoop;
                Priority = Constant.DefaultPriority;
                VolumeInSoundGroup = Constant.DefaultVolume;
                Pitch = Constant.DefaultPitch;
                PanStereo = Constant.DefaultPanStereo;
                SpatialBlend = Constant.DefaultSpatialBlend;
                MaxDistance = Constant.DefaultMaxDistance;
                DopplerLevel = Constant.DefaultDopplerLevel;
                m_SoundAgentHelper.Reset();
            }

            internal bool SetSoundAsset(object soundAsset)
            {
                Reset();
                m_SoundAsset = soundAsset;
                m_SetSoundAssetTime = DateTime.Now;
                return m_SoundAgentHelper.SetSoundAsset(soundAsset);
            }

            internal void RefreshMute()
            {
                m_SoundAgentHelper.Mute = m_SoundGroup.Mute || m_MuteInSoundGroup;
            }

            internal void RefreshVolume()
            {
                m_SoundAgentHelper.Volume = m_SoundGroup.Volume * m_VolumeInSoundGroup;
            }

            private void OnResetSoundAgent(object sender, ResetSoundAgentEventArgs e)
            {
                Reset();
            }
        }
    }
}
