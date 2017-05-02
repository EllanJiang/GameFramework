//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.Resource;
#if UNITY_5_3
using GameFramework.Scene;
#endif
using GameFramework.Sound;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 声音组件。
    /// </summary>
    [AddComponentMenu("Game Framework/Sound")]
    public sealed partial class SoundComponent : GameFrameworkComponent
    {
        private ISoundManager m_SoundManager = null;
        private EventComponent m_EventComponent = null;
        private AudioListener m_AudioListener = null;

        [SerializeField]
        private bool m_EnablePlaySoundSuccessEvent = true;

        [SerializeField]
        private bool m_EnablePlaySoundFailureEvent = true;

        [SerializeField]
        private bool m_EnablePlaySoundUpdateEvent = false;

        [SerializeField]
        private bool m_EnablePlaySoundDependencyAssetEvent = false;

        [SerializeField]
        private Transform m_InstanceRoot = null;

        [SerializeField]
        private AudioMixer m_AudioMixer = null;

        [SerializeField]
        private string m_SoundHelperTypeName = "UnityGameFramework.Runtime.DefaultSoundHelper";

        [SerializeField]
        private SoundHelperBase m_CustomSoundHelper = null;

        [SerializeField]
        private string m_SoundGroupHelperTypeName = "UnityGameFramework.Runtime.DefaultSoundGroupHelper";

        [SerializeField]
        private SoundGroupHelperBase m_CustomSoundGroupHelper = null;

        [SerializeField]
        private string m_SoundAgentHelperTypeName = "UnityGameFramework.Runtime.DefaultSoundAgentHelper";

        [SerializeField]
        private SoundAgentHelperBase m_CustomSoundAgentHelper = null;

        [SerializeField]
        private SoundGroup[] m_SoundGroups = null;

        /// <summary>
        /// 获取声音组数量。
        /// </summary>
        public int SoundGroupCount
        {
            get
            {
                return m_SoundManager.SoundGroupCount;
            }
        }

        /// <summary>
        /// 获取声音混响器。
        /// </summary>
        public AudioMixer AudioMixer
        {
            get
            {
                return m_AudioMixer;
            }
        }

        /// <summary>
        /// 游戏框架组件初始化。
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            m_SoundManager = GameFrameworkEntry.GetModule<ISoundManager>();
            if (m_SoundManager == null)
            {
                Log.Fatal("Sound manager is invalid.");
                return;
            }

            m_SoundManager.PlaySoundSuccess += OnPlaySoundSuccess;
            m_SoundManager.PlaySoundFailure += OnPlaySoundFailure;
            m_SoundManager.PlaySoundUpdate += OnPlaySoundUpdate;
            m_SoundManager.PlaySoundDependencyAsset += OnPlaySoundDependencyAsset;

            m_AudioListener = gameObject.GetOrAddComponent<AudioListener>();

#if UNITY_5_4_OR_NEWER
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
#else
            ISceneManager sceneManager = GameFrameworkEntry.GetModule<ISceneManager>();
            if (sceneManager == null)
            {
                Log.Fatal("Scene manager is invalid.");
                return;
            }

            sceneManager.LoadSceneSuccess += OnLoadSceneSuccess;
            sceneManager.LoadSceneFailure += OnLoadSceneFailure;
            sceneManager.UnloadSceneSuccess += OnUnloadSceneSuccess;
            sceneManager.UnloadSceneFailure += OnUnloadSceneFailure;
#endif
        }

        private void Start()
        {
            BaseComponent baseComponent = GameEntry.GetComponent<BaseComponent>();
            if (baseComponent == null)
            {
                Log.Fatal("Base component is invalid.");
                return;
            }

            m_EventComponent = GameEntry.GetComponent<EventComponent>();
            if (m_EventComponent == null)
            {
                Log.Fatal("Event component is invalid.");
                return;
            }

            if (baseComponent.EditorResourceMode)
            {
                m_SoundManager.SetResourceManager(baseComponent.EditorResourceHelper);
            }
            else
            {
                m_SoundManager.SetResourceManager(GameFrameworkEntry.GetModule<IResourceManager>());
            }

            SoundHelperBase soundHelper = Utility.Helper.CreateHelper(m_SoundHelperTypeName, m_CustomSoundHelper);
            if (soundHelper == null)
            {
                Log.Error("Can not create sound helper.");
                return;
            }

            soundHelper.name = string.Format("Sound Helper");
            Transform transform = soundHelper.transform;
            transform.SetParent(this.transform);
            transform.localScale = Vector3.one;

            m_SoundManager.SetSoundHelper(soundHelper);

            if (m_InstanceRoot == null)
            {
                m_InstanceRoot = (new GameObject("Sound Instances")).transform;
                m_InstanceRoot.SetParent(gameObject.transform);
                m_InstanceRoot.localScale = Vector3.one;
            }

            for (int i = 0; i < m_SoundGroups.Length; i++)
            {
                if (!AddSoundGroup(m_SoundGroups[i].Name, m_SoundGroups[i].AvoidBeingReplacedBySamePriority, m_SoundGroups[i].Mute, m_SoundGroups[i].Volume, m_SoundGroups[i].AgentHelperCount))
                {
                    Log.Warning("Add sound group '{0}' failure.", m_SoundGroups[i].Name);
                    continue;
                }
            }
        }

        private void OnDestroy()
        {
#if UNITY_5_4_OR_NEWER
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
#endif
        }

        /// <summary>
        /// 是否存在指定声音组。
        /// </summary>
        /// <param name="soundGroupName">声音组名称。</param>
        /// <returns>指定声音组是否存在。</returns>
        public bool HasSoundGroup(string soundGroupName)
        {
            return m_SoundManager.HasSoundGroup(soundGroupName);
        }

        /// <summary>
        /// 获取指定声音组。
        /// </summary>
        /// <param name="soundGroupName">声音组名称。</param>
        /// <returns>要获取的声音组。</returns>
        public ISoundGroup GetSoundGroup(string soundGroupName)
        {
            return m_SoundManager.GetSoundGroup(soundGroupName);
        }

        /// <summary>
        /// 获取所有声音组。
        /// </summary>
        /// <returns>所有声音组。</returns>
        public ISoundGroup[] GetAllSoundGroups()
        {
            return m_SoundManager.GetAllSoundGroups();
        }

        /// <summary>
        /// 增加声音组。
        /// </summary>
        /// <param name="soundGroupName">声音组名称。</param>
        /// <param name="soundAgentHelperCount">声音代理辅助器数量。</param>
        /// <returns>是否增加声音组成功。</returns>
        public bool AddSoundGroup(string soundGroupName, int soundAgentHelperCount)
        {
            return AddSoundGroup(soundGroupName, false, false, 1f, soundAgentHelperCount);
        }

        /// <summary>
        /// 增加声音组。
        /// </summary>
        /// <param name="soundGroupName">声音组名称。</param>
        /// <param name="soundGroupAvoidBeingReplacedBySamePriority">声音组中的声音是否避免被同优先级声音替换。</param>
        /// <param name="soundGroupMute">声音组是否静音。</param>
        /// <param name="soundGroupVolume">声音组音量。</param>
        /// <param name="soundAgentHelperCount">声音代理辅助器数量。</param>
        /// <returns>是否增加声音组成功。</returns>
        public bool AddSoundGroup(string soundGroupName, bool soundGroupAvoidBeingReplacedBySamePriority, bool soundGroupMute, float soundGroupVolume, int soundAgentHelperCount)
        {
            if (m_SoundManager.HasSoundGroup(soundGroupName))
            {
                return false;
            }

            SoundGroupHelperBase soundGroupHelper = Utility.Helper.CreateHelper(m_SoundGroupHelperTypeName, m_CustomSoundGroupHelper, SoundGroupCount);
            if (soundGroupHelper == null)
            {
                Log.Error("Can not create sound group helper.");
                return false;
            }

            soundGroupHelper.name = string.Format("Sound Group - {0}", soundGroupName);
            Transform transform = soundGroupHelper.transform;
            transform.SetParent(m_InstanceRoot);
            transform.localScale = Vector3.one;

            if (m_AudioMixer != null)
            {
                AudioMixerGroup[] audioMixerGroups = m_AudioMixer.FindMatchingGroups(string.Format("Master/{0}", soundGroupName));
                if (audioMixerGroups.Length > 0)
                {
                    soundGroupHelper.AudioMixerGroup = audioMixerGroups[0];
                }
                else
                {
                    soundGroupHelper.AudioMixerGroup = m_AudioMixer.FindMatchingGroups("Master")[0];
                }
            }

            if (!m_SoundManager.AddSoundGroup(soundGroupName, soundGroupAvoidBeingReplacedBySamePriority, soundGroupMute, soundGroupVolume, soundGroupHelper))
            {
                return false;
            }

            for (int i = 0; i < soundAgentHelperCount; i++)
            {
                if (!AddSoundAgentHelper(soundGroupName, soundGroupHelper, i))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 播放声音。
        /// </summary>
        /// <param name="soundAssetName">声音资源名称。</param>
        /// <param name="soundGroupName">声音组名称。</param>
        /// <returns>声音的序列编号。</returns>
        public int PlaySound(string soundAssetName, string soundGroupName)
        {
            return PlaySound(soundAssetName, soundGroupName, null, null, null);
        }

        /// <summary>
        /// 播放声音。
        /// </summary>
        /// <param name="soundAssetName">声音资源名称。</param>
        /// <param name="soundGroupName">声音组名称。</param>
        /// <param name="playSoundParams">播放声音参数。</param>
        /// <returns>声音的序列编号。</returns>
        public int PlaySound(string soundAssetName, string soundGroupName, PlaySoundParams playSoundParams)
        {
            return PlaySound(soundAssetName, soundGroupName, playSoundParams, null, null);
        }

        /// <summary>
        /// 播放声音。
        /// </summary>
        /// <param name="soundAssetName">声音资源名称。</param>
        /// <param name="soundGroupName">声音组名称。</param>
        /// <param name="bindingEntity">声音绑定的实体。</param>
        /// <returns>声音的序列编号。</returns>
        public int PlaySound(string soundAssetName, string soundGroupName, Entity bindingEntity)
        {
            return PlaySound(soundAssetName, soundGroupName, null, bindingEntity, null);
        }

        /// <summary>
        /// 播放声音。
        /// </summary>
        /// <param name="soundAssetName">声音资源名称。</param>
        /// <param name="soundGroupName">声音组名称。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>声音的序列编号。</returns>
        public int PlaySound(string soundAssetName, string soundGroupName, object userData)
        {
            return PlaySound(soundAssetName, soundGroupName, null, null, userData);
        }

        /// <summary>
        /// 播放声音。
        /// </summary>
        /// <param name="soundAssetName">声音资源名称。</param>
        /// <param name="soundGroupName">声音组名称。</param>
        /// <param name="playSoundParams">播放声音参数。</param>
        /// <param name="bindingEntity">声音绑定的实体。</param>
        /// <returns>声音的序列编号。</returns>
        public int PlaySound(string soundAssetName, string soundGroupName, PlaySoundParams playSoundParams, Entity bindingEntity)
        {
            return PlaySound(soundAssetName, soundGroupName, playSoundParams, bindingEntity, null);
        }

        /// <summary>
        /// 播放声音。
        /// </summary>
        /// <param name="soundAssetName">声音资源名称。</param>
        /// <param name="soundGroupName">声音组名称。</param>
        /// <param name="playSoundParams">播放声音参数。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>声音的序列编号。</returns>
        public int PlaySound(string soundAssetName, string soundGroupName, PlaySoundParams playSoundParams, object userData)
        {
            return PlaySound(soundAssetName, soundGroupName, playSoundParams, null, userData);
        }

        /// <summary>
        /// 播放声音。
        /// </summary>
        /// <param name="soundAssetName">声音资源名称。</param>
        /// <param name="soundGroupName">声音组名称。</param>
        /// <param name="bindingEntity">声音绑定的实体。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>声音的序列编号。</returns>
        public int PlaySound(string soundAssetName, string soundGroupName, Entity bindingEntity, object userData)
        {
            return PlaySound(soundAssetName, soundGroupName, null, bindingEntity, userData);
        }

        /// <summary>
        /// 播放声音。
        /// </summary>
        /// <param name="soundAssetName">声音资源名称。</param>
        /// <param name="soundGroupName">声音组名称。</param>
        /// <param name="playSoundParams">播放声音参数。</param>
        /// <param name="bindingEntity">声音绑定的实体。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>声音的序列编号。</returns>
        public int PlaySound(string soundAssetName, string soundGroupName, PlaySoundParams playSoundParams, Entity bindingEntity, object userData)
        {
            return m_SoundManager.PlaySound(soundAssetName, soundGroupName, playSoundParams, new PlaySoundInfo(bindingEntity, Vector3.zero, userData));
        }

        /// <summary>
        /// 播放声音。
        /// </summary>
        /// <param name="soundAssetName">声音资源名称。</param>
        /// <param name="soundGroupName">声音组名称。</param>
        /// <param name="playSoundParams">播放声音参数。</param>
        /// <param name="worldPosition">声音所在的世界坐标。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>声音的序列编号。</returns>
        public int PlaySound(string soundAssetName, string soundGroupName, Vector3 worldPosition)
        {
            return PlaySound(soundAssetName, soundGroupName, null, worldPosition, null);
        }

        /// <summary>
        /// 播放声音。
        /// </summary>
        /// <param name="soundAssetName">声音资源名称。</param>
        /// <param name="soundGroupName">声音组名称。</param>
        /// <param name="playSoundParams">播放声音参数。</param>
        /// <param name="worldPosition">声音所在的世界坐标。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>声音的序列编号。</returns>
        public int PlaySound(string soundAssetName, string soundGroupName, PlaySoundParams playSoundParams, Vector3 worldPosition)
        {
            return PlaySound(soundAssetName, soundGroupName, playSoundParams, worldPosition, null);
        }

        /// <summary>
        /// 播放声音。
        /// </summary>
        /// <param name="soundAssetName">声音资源名称。</param>
        /// <param name="soundGroupName">声音组名称。</param>
        /// <param name="playSoundParams">播放声音参数。</param>
        /// <param name="worldPosition">声音所在的世界坐标。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>声音的序列编号。</returns>
        public int PlaySound(string soundAssetName, string soundGroupName, Vector3 worldPosition, object userData)
        {
            return PlaySound(soundAssetName, soundGroupName, null, worldPosition, userData);
        }

        /// <summary>
        /// 播放声音。
        /// </summary>
        /// <param name="soundAssetName">声音资源名称。</param>
        /// <param name="soundGroupName">声音组名称。</param>
        /// <param name="playSoundParams">播放声音参数。</param>
        /// <param name="worldPosition">声音所在的世界坐标。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>声音的序列编号。</returns>
        public int PlaySound(string soundAssetName, string soundGroupName, PlaySoundParams playSoundParams, Vector3 worldPosition, object userData)
        {
            return m_SoundManager.PlaySound(soundAssetName, soundGroupName, playSoundParams, new PlaySoundInfo(null, worldPosition, userData));
        }

        /// <summary>
        /// 停止播放声音。
        /// </summary>
        /// <param name="serialId">要停止播放声音的序列编号。</param>
        /// <returns>是否停止播放声音成功。</returns>
        public bool StopSound(int serialId)
        {
            return m_SoundManager.StopSound(serialId);
        }

        /// <summary>
        /// 停止播放声音。
        /// </summary>
        /// <param name="serialId">要停止播放声音的序列编号。</param>
        /// <param name="fadeOutSeconds">声音淡出时间，以秒为单位。</param>
        /// <returns>是否停止播放声音成功。</returns>
        public bool StopSound(int serialId, float fadeOutSeconds)
        {
            return m_SoundManager.StopSound(serialId, fadeOutSeconds);
        }

        /// <summary>
        /// 暂停播放声音。
        /// </summary>
        /// <param name="serialId">要暂停播放声音的序列编号。</param>
        /// <returns>是否暂停播放声音成功。</returns>
        public bool PauseSound(int serialId)
        {
            return m_SoundManager.PauseSound(serialId);
        }

        /// <summary>
        /// 暂停播放声音。
        /// </summary>
        /// <param name="serialId">要暂停播放声音的序列编号。</param>
        /// <param name="fadeOutSeconds">声音淡出时间，以秒为单位。</param>
        /// <returns>是否暂停播放声音成功。</returns>
        public bool PauseSound(int serialId, float fadeOutSeconds)
        {
            return m_SoundManager.PauseSound(serialId, fadeOutSeconds);
        }

        /// <summary>
        /// 恢复播放声音。
        /// </summary>
        /// <param name="serialId">要恢复播放声音的序列编号。</param>
        /// <returns>是否恢复播放声音成功。</returns>
        public bool ResumeSound(int serialId)
        {
            return m_SoundManager.ResumeSound(serialId);
        }

        /// <summary>
        /// 恢复播放声音。
        /// </summary>
        /// <param name="serialId">要恢复播放声音的序列编号。</param>
        /// <param name="fadeInSeconds">声音淡入时间，以秒为单位。</param>
        /// <returns>是否恢复播放声音成功。</returns>
        public bool ResumeSound(int serialId, float fadeInSeconds)
        {
            return m_SoundManager.ResumeSound(serialId, fadeInSeconds);
        }

        /// <summary>
        /// 停止所有声音。
        /// </summary>
        /// <param name="soundGroupName">声音组名称。</param>
        public void StopAllSounds(string soundGroupName)
        {
            m_SoundManager.StopAllSounds(soundGroupName);
        }

        /// <summary>
        /// 停止所有声音。
        /// </summary>
        /// <param name="soundGroupName">声音组名称。</param>
        /// <param name="fadeOutSeconds">声音淡出时间，以秒为单位。</param>
        public void StopAllSounds(string soundGroupName, float fadeOutSeconds)
        {
            m_SoundManager.StopAllSounds(soundGroupName, fadeOutSeconds);
        }

        /// <summary>
        /// 停止所有声音。
        /// </summary>
        public void StopAllSounds()
        {
            m_SoundManager.StopAllSounds();
        }

        /// <summary>
        /// 停止所有声音。
        /// </summary>
        /// <param name="fadeOutSeconds">声音淡出时间，以秒为单位。</param>
        public void StopAllSounds(float fadeOutSeconds)
        {
            m_SoundManager.StopAllSounds(fadeOutSeconds);
        }

        /// <summary>
        /// 增加声音代理辅助器。
        /// </summary>
        /// <param name="soundGroupName">声音组名称。</param>
        /// <param name="soundGroupHelper">声音组辅助器。</param>
        /// <param name="index">声音代理辅助器索引。</param>
        /// <returns>是否增加声音代理辅助器成功。</returns>
        private bool AddSoundAgentHelper(string soundGroupName, SoundGroupHelperBase soundGroupHelper, int index)
        {
            SoundAgentHelperBase soundAgentHelper = Utility.Helper.CreateHelper(m_SoundAgentHelperTypeName, m_CustomSoundAgentHelper, index);
            if (soundAgentHelper == null)
            {
                Log.Error("Can not create sound agent helper.");
                return false;
            }

            soundAgentHelper.name = string.Format("Sound Agent Helper - {0} - {1}", soundGroupName, index.ToString());
            Transform transform = soundAgentHelper.transform;
            transform.SetParent(soundGroupHelper.transform);
            transform.localScale = Vector3.one;

            if (m_AudioMixer != null)
            {
                AudioMixerGroup[] audioMixerGroups = m_AudioMixer.FindMatchingGroups(string.Format("Master/{0}/{1}", soundGroupName, index.ToString()));
                if (audioMixerGroups.Length > 0)
                {
                    soundAgentHelper.AudioMixerGroup = audioMixerGroups[0];
                }
                else
                {
                    soundAgentHelper.AudioMixerGroup = soundGroupHelper.AudioMixerGroup;
                }
            }

            m_SoundManager.AddSoundAgentHelper(soundGroupName, soundAgentHelper);

            return true;
        }

        private void OnPlaySoundSuccess(object sender, GameFramework.Sound.PlaySoundSuccessEventArgs e)
        {
            PlaySoundInfo playSoundInfo = (PlaySoundInfo)e.UserData;
            if (playSoundInfo != null)
            {
                SoundAgentHelperBase soundAgentHelper = (SoundAgentHelperBase)e.SoundAgent.Helper;
                if (playSoundInfo.BindingEntity != null)
                {
                    soundAgentHelper.SetBindingEntity(playSoundInfo.BindingEntity);
                }
                else
                {
                    soundAgentHelper.SetWorldPosition(playSoundInfo.WorldPosition);
                }
            }

            if (m_EnablePlaySoundSuccessEvent)
            {
                m_EventComponent.Fire(this, new PlaySoundSuccessEventArgs(e));
            }
        }

        private void OnPlaySoundFailure(object sender, GameFramework.Sound.PlaySoundFailureEventArgs e)
        {
            string logMessage = string.Format("Play sound failure, asset name '{0}', sound group name '{1}', error code '{2}', error message '{3}'.", e.SoundAssetName, e.SoundGroupName, e.ErrorCode.ToString(), e.ErrorMessage);
            if (e.ErrorCode == PlaySoundErrorCode.IgnoredDueToLowPriority)
            {
                Log.Info(logMessage);
            }
            else
            {
                Log.Warning(logMessage);
            }

            if (m_EnablePlaySoundFailureEvent)
            {
                m_EventComponent.Fire(this, new PlaySoundFailureEventArgs(e));
            }
        }

        private void OnPlaySoundUpdate(object sender, GameFramework.Sound.PlaySoundUpdateEventArgs e)
        {
            if (m_EnablePlaySoundUpdateEvent)
            {
                m_EventComponent.Fire(this, new PlaySoundUpdateEventArgs(e));
            }
        }

        private void OnPlaySoundDependencyAsset(object sender, GameFramework.Sound.PlaySoundDependencyAssetEventArgs e)
        {
            if (m_EnablePlaySoundDependencyAssetEvent)
            {
                m_EventComponent.Fire(this, new PlaySoundDependencyAssetEventArgs(e));
            }
        }

        private void OnLoadSceneSuccess(object sender, GameFramework.Scene.LoadSceneSuccessEventArgs e)
        {
            RefreshAudioListener();
        }

        private void OnLoadSceneFailure(object sender, GameFramework.Scene.LoadSceneFailureEventArgs e)
        {
            RefreshAudioListener();
        }

        private void OnUnloadSceneSuccess(object sender, GameFramework.Scene.UnloadSceneSuccessEventArgs e)
        {
            RefreshAudioListener();
        }

        private void OnUnloadSceneFailure(object sender, GameFramework.Scene.UnloadSceneFailureEventArgs e)
        {
            RefreshAudioListener();
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            RefreshAudioListener();
        }

        private void OnSceneUnloaded(Scene scene)
        {
            RefreshAudioListener();
        }

        private void RefreshAudioListener()
        {
            m_AudioListener.enabled = FindObjectsOfType<AudioListener>().Length <= 1;
        }
    }
}
