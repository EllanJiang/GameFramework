//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.Entity;
using GameFramework.Sound;
using System;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 默认声音代理辅助器。
    /// </summary>
    public class DefaultSoundAgentHelper : SoundAgentHelperBase
    {
        private Transform m_CachedTransform = null;
        private AudioSource m_AudioSource = null;
        private EntityLogic m_BindingEntityLogic = null;
        private EventHandler<ResetSoundAgentEventArgs> m_ResetSoundAgentEventHandler = null;

        /// <summary>
        /// 获取当前是否正在播放。
        /// </summary>
        public override bool IsPlaying
        {
            get
            {
                return m_AudioSource.isPlaying;
            }
        }

        /// <summary>
        /// 获取或设置播放位置。
        /// </summary>
        public override float Time
        {
            get
            {
                return m_AudioSource.time;
            }
            set
            {
                m_AudioSource.time = value;
            }
        }

        /// <summary>
        /// 获取或设置是否静音。
        /// </summary>
        public override bool Mute
        {
            get
            {
                return m_AudioSource.mute;
            }
            set
            {
                m_AudioSource.mute = value;
            }
        }

        /// <summary>
        /// 获取或设置是否循环播放。
        /// </summary>
        public override bool Loop
        {
            get
            {
                return m_AudioSource.loop;
            }
            set
            {
                m_AudioSource.loop = value;
            }
        }

        /// <summary>
        /// 获取或设置声音优先级。
        /// </summary>
        public override int Priority
        {
            get
            {
                return 128 - m_AudioSource.priority;
            }
            set
            {
                m_AudioSource.priority = 128 - value;
            }
        }

        /// <summary>
        /// 获取或设置音量大小。
        /// </summary>
        public override float Volume
        {
            get
            {
                return m_AudioSource.volume;
            }
            set
            {
                m_AudioSource.volume = value;
            }
        }

        /// <summary>
        /// 获取或设置声音音调。
        /// </summary>
        public override float Pitch
        {
            get
            {
                return m_AudioSource.pitch;
            }
            set
            {
                m_AudioSource.pitch = value;
            }
        }

        /// <summary>
        /// 获取或设置声音立体声声相。
        /// </summary>
        public override float PanStereo
        {
            get
            {
                return m_AudioSource.panStereo;
            }
            set
            {
                m_AudioSource.panStereo = value;
            }
        }

        /// <summary>
        /// 获取或设置声音空间混合量。
        /// </summary>
        public override float SpatialBlend
        {
            get
            {
                return m_AudioSource.spatialBlend;
            }
            set
            {
                m_AudioSource.spatialBlend = value;
            }
        }

        /// <summary>
        /// 获取或设置声音最大距离。
        /// </summary>
        public override float MaxDistance
        {
            get
            {
                return m_AudioSource.maxDistance;
            }

            set
            {
                m_AudioSource.maxDistance = value;
            }
        }

        /// <summary>
        /// 重置声音代理事件。
        /// </summary>
        public override event EventHandler<ResetSoundAgentEventArgs> ResetSoundAgent
        {
            add
            {
                m_ResetSoundAgentEventHandler += value;
            }
            remove
            {
                m_ResetSoundAgentEventHandler -= value;
            }
        }

        /// <summary>
        /// 播放声音。
        /// </summary>
        public override void Play()
        {
            m_AudioSource.Play();
        }

        /// <summary>
        /// 停止播放声音。
        /// </summary>
        public override void Stop()
        {
            m_AudioSource.Stop();
        }

        /// <summary>
        /// 暂停播放声音。
        /// </summary>
        public override void Pause()
        {
            m_AudioSource.Pause();
        }

        /// <summary>
        /// 恢复播放声音。
        /// </summary>
        public override void Resume()
        {
            m_AudioSource.UnPause();
        }

        /// <summary>
        /// 重置声音代理辅助器。
        /// </summary>
        public override void Reset()
        {
            m_CachedTransform.localPosition = Vector3.zero;
            m_AudioSource.clip = null;
            m_BindingEntityLogic = null;
        }

        /// <summary>
        /// 设置声音资源。
        /// </summary>
        /// <param name="soundAsset">声音资源。</param>
        /// <returns>设置声音资源是否成功。</returns>
        public override bool SetSoundAsset(object soundAsset)
        {
            AudioClip audioClip = soundAsset as AudioClip;
            if (audioClip == null)
            {
                return false;
            }

            m_AudioSource.clip = audioClip;
            return true;
        }

        /// <summary>
        /// 设置声音绑定的实体。
        /// </summary>
        /// <param name="bindingEntity">声音绑定的实体。</param>
        public override void SetBindingEntity(IEntity bindingEntity)
        {
            m_BindingEntityLogic = ((Entity)bindingEntity).Logic;
            if (m_BindingEntityLogic == null && m_ResetSoundAgentEventHandler != null)
            {
                m_ResetSoundAgentEventHandler(this, new ResetSoundAgentEventArgs());
                return;
            }

            UpdateAgentPosition();
        }

        private void Awake()
        {
            m_CachedTransform = transform;
            m_AudioSource = gameObject.GetOrAddComponent<AudioSource>();
            m_AudioSource.playOnAwake = false;
            m_AudioSource.rolloffMode = AudioRolloffMode.Custom;
        }

        private void Update()
        {
            if (!IsPlaying && m_AudioSource.clip != null && m_ResetSoundAgentEventHandler != null)
            {
                m_ResetSoundAgentEventHandler(this, new ResetSoundAgentEventArgs());
                return;
            }

            if (m_BindingEntityLogic == null)
            {
                return;
            }

            UpdateAgentPosition();
        }

        private void UpdateAgentPosition()
        {
            if (!m_BindingEntityLogic.IsAvailable && m_ResetSoundAgentEventHandler != null)
            {
                m_ResetSoundAgentEventHandler(this, new ResetSoundAgentEventArgs());
                return;
            }

            m_CachedTransform.position = m_BindingEntityLogic.CachedTransform.position;
        }
    }
}
