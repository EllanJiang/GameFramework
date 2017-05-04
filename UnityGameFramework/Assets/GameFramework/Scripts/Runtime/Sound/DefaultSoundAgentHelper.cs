//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.Sound;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

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
        private float m_VolumeWhenPause = 0f;
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
        /// 获取或设置声音代理辅助器所在的混音组。
        /// </summary>
        public override AudioMixerGroup AudioMixerGroup
        {
            get
            {
                return m_AudioSource.outputAudioMixerGroup;
            }
            set
            {
                m_AudioSource.outputAudioMixerGroup = value;
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
        /// <param name="fadeInSeconds">声音淡入时间，以秒为单位。</param>
        public override void Play(float fadeInSeconds)
        {
            StopAllCoroutines();

            m_AudioSource.Play();
            if (fadeInSeconds > 0f)
            {
                float volume = m_AudioSource.volume;
                m_AudioSource.volume = 0f;
                StartCoroutine(FadeToVolume(m_AudioSource, volume, fadeInSeconds));
            }
        }

        /// <summary>
        /// 停止播放声音。
        /// </summary>
        /// <param name="fadeOutSeconds">声音淡出时间，以秒为单位。</param>
        public override void Stop(float fadeOutSeconds)
        {
            StopAllCoroutines();

            if (fadeOutSeconds > 0f)
            {
                StartCoroutine(StopCo(fadeOutSeconds));
            }
            else
            {
                m_AudioSource.Stop();
            }
        }

        /// <summary>
        /// 暂停播放声音。
        /// </summary>
        /// <param name="fadeOutSeconds">声音淡出时间，以秒为单位。</param>
        public override void Pause(float fadeOutSeconds)
        {
            StopAllCoroutines();

            m_VolumeWhenPause = m_AudioSource.volume;
            if (fadeOutSeconds > 0f)
            {
                StartCoroutine(PauseCo(fadeOutSeconds));
            }
            else
            {
                m_AudioSource.Pause();
            }
        }

        /// <summary>
        /// 恢复播放声音。
        /// </summary>
        /// <param name="fadeInSeconds">声音淡入时间，以秒为单位。</param>
        public override void Resume(float fadeInSeconds)
        {
            StopAllCoroutines();

            m_AudioSource.UnPause();
            if (fadeInSeconds > 0f)
            {
                StartCoroutine(FadeToVolume(m_AudioSource, m_VolumeWhenPause, fadeInSeconds));
            }
            else
            {
                m_AudioSource.volume = m_VolumeWhenPause;
            }
        }

        /// <summary>
        /// 重置声音代理辅助器。
        /// </summary>
        public override void Reset()
        {
            m_CachedTransform.localPosition = Vector3.zero;
            m_AudioSource.clip = null;
            m_BindingEntityLogic = null;
            m_VolumeWhenPause = 0f;
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
        public override void SetBindingEntity(Entity bindingEntity)
        {
            m_BindingEntityLogic = bindingEntity.Logic;
            if (m_BindingEntityLogic != null)
            {
                UpdateAgentPosition();
                return;
            }

            if (m_ResetSoundAgentEventHandler != null)
            {
                m_ResetSoundAgentEventHandler(this, new ResetSoundAgentEventArgs());
            }
        }

        /// <summary>
        /// 设置声音所在的世界坐标。
        /// </summary>
        /// <param name="worldPosition">声音所在的世界坐标。</param>
        public override void SetWorldPosition(Vector3 worldPosition)
        {
            m_CachedTransform.position = worldPosition;
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

            if (m_BindingEntityLogic != null)
            {
                UpdateAgentPosition();
            }
        }

        private void UpdateAgentPosition()
        {
            if (m_BindingEntityLogic.IsAvailable)
            {
                m_CachedTransform.position = m_BindingEntityLogic.CachedTransform.position;
                return;
            }

            if (m_ResetSoundAgentEventHandler != null)
            {
                m_ResetSoundAgentEventHandler(this, new ResetSoundAgentEventArgs());
            }
        }

        private IEnumerator StopCo(float fadeOutSeconds)
        {
            yield return FadeToVolume(m_AudioSource, 0f, fadeOutSeconds);
            m_AudioSource.Stop();
        }

        private IEnumerator PauseCo(float fadeOutSeconds)
        {
            yield return FadeToVolume(m_AudioSource, 0f, fadeOutSeconds);
            m_AudioSource.Pause();
        }

        private IEnumerator FadeToVolume(AudioSource audioSource, float volume, float duration)
        {
            float time = 0f;
            float originalVolume = audioSource.volume;
            while (time < duration)
            {
                time += UnityEngine.Time.deltaTime;
                audioSource.volume = Mathf.Lerp(originalVolume, volume, time / duration);
                yield return new WaitForEndOfFrame();
            }

            audioSource.volume = volume;
        }
    }
}
