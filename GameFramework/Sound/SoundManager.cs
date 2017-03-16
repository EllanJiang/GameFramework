//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.Resource;
using System;
using System.Collections.Generic;

namespace GameFramework.Sound
{
    /// <summary>
    /// 声音管理器。
    /// </summary>
    internal sealed partial class SoundManager : GameFrameworkModule, ISoundManager
    {
        private readonly IDictionary<string, SoundGroup> m_SoundGroups;
        private readonly LoadAssetCallbacks m_LoadAssetCallbacks;
        private IResourceManager m_ResourceManager;
        private ISoundHelper m_SoundHelper;
        private int m_Serial;
        private EventHandler<PlaySoundSuccessEventArgs> m_PlaySoundSuccessEventHandler;
        private EventHandler<PlaySoundFailureEventArgs> m_PlaySoundFailureEventHandler;
        private EventHandler<PlaySoundUpdateEventArgs> m_PlaySoundUpdateEventHandler;
        private EventHandler<PlaySoundDependencyAssetEventArgs> m_PlaySoundDependencyAssetEventHandler;

        /// <summary>
        /// 初始化声音管理器的新实例。
        /// </summary>
        public SoundManager()
        {
            m_SoundGroups = new Dictionary<string, SoundGroup>();
            m_LoadAssetCallbacks = new LoadAssetCallbacks(LoadSoundSuccessCallback, LoadSoundFailureCallback, LoadSoundUpdateCallback, LoadSoundDependencyAssetCallback);
            m_ResourceManager = null;
            m_SoundHelper = null;
            m_Serial = 0;
            m_PlaySoundSuccessEventHandler = null;
            m_PlaySoundFailureEventHandler = null;
            m_PlaySoundUpdateEventHandler = null;
            m_PlaySoundDependencyAssetEventHandler = null;
        }

        /// <summary>
        /// 获取声音组数量。
        /// </summary>
        public int SoundGroupCount
        {
            get
            {
                return m_SoundGroups.Count;
            }
        }

        /// <summary>
        /// 播放声音成功事件。
        /// </summary>
        public event EventHandler<PlaySoundSuccessEventArgs> PlaySoundSuccess
        {
            add
            {
                m_PlaySoundSuccessEventHandler += value;
            }
            remove
            {
                m_PlaySoundSuccessEventHandler -= value;
            }
        }

        /// <summary>
        /// 播放声音失败事件。
        /// </summary>
        public event EventHandler<PlaySoundFailureEventArgs> PlaySoundFailure
        {
            add
            {
                m_PlaySoundFailureEventHandler += value;
            }
            remove
            {
                m_PlaySoundFailureEventHandler -= value;
            }
        }

        /// <summary>
        /// 播放声音更新事件。
        /// </summary>
        public event EventHandler<PlaySoundUpdateEventArgs> PlaySoundUpdate
        {
            add
            {
                m_PlaySoundUpdateEventHandler += value;
            }
            remove
            {
                m_PlaySoundUpdateEventHandler -= value;
            }
        }

        /// <summary>
        /// 播放声音时加载依赖资源事件。
        /// </summary>
        public event EventHandler<PlaySoundDependencyAssetEventArgs> PlaySoundDependencyAsset
        {
            add
            {
                m_PlaySoundDependencyAssetEventHandler += value;
            }
            remove
            {
                m_PlaySoundDependencyAssetEventHandler -= value;
            }
        }

        /// <summary>
        /// 声音管理器轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {

        }

        /// <summary>
        /// 关闭并清理声音管理器。
        /// </summary>
        internal override void Shutdown()
        {

        }

        /// <summary>
        /// 设置资源管理器。
        /// </summary>
        /// <param name="resourceManager">资源管理器。</param>
        public void SetResourceManager(IResourceManager resourceManager)
        {
            if (resourceManager == null)
            {
                throw new GameFrameworkException("Resource manager is invalid.");
            }

            m_ResourceManager = resourceManager;
        }

        /// <summary>
        /// 设置声音辅助器。
        /// </summary>
        /// <param name="soundHelper">声音辅助器。</param>
        public void SetSoundHelper(ISoundHelper soundHelper)
        {
            if (soundHelper == null)
            {
                throw new GameFrameworkException("Sound helper is invalid.");
            }

            m_SoundHelper = soundHelper;
        }

        /// <summary>
        /// 是否存在指定声音组。
        /// </summary>
        /// <param name="soundGroupName">声音组名称。</param>
        /// <returns>指定声音组是否存在。</returns>
        public bool HasSoundGroup(string soundGroupName)
        {
            if (string.IsNullOrEmpty(soundGroupName))
            {
                throw new GameFrameworkException("Sound group name is invalid.");
            }

            return m_SoundGroups.ContainsKey(soundGroupName);
        }

        /// <summary>
        /// 获取指定声音组。
        /// </summary>
        /// <param name="soundGroupName">声音组名称。</param>
        /// <returns>要获取的声音组。</returns>
        public ISoundGroup GetSoundGroup(string soundGroupName)
        {
            if (string.IsNullOrEmpty(soundGroupName))
            {
                throw new GameFrameworkException("Sound group name is invalid.");
            }

            SoundGroup soundGroup = null;
            if (m_SoundGroups.TryGetValue(soundGroupName, out soundGroup))
            {
                return soundGroup;
            }

            return null;
        }

        /// <summary>
        /// 获取所有声音组。
        /// </summary>
        /// <returns>所有声音组。</returns>
        public ISoundGroup[] GetAllSoundGroups()
        {
            int index = 0;
            ISoundGroup[] soundGroups = new ISoundGroup[m_SoundGroups.Count];
            foreach (KeyValuePair<string, SoundGroup> soundGroup in m_SoundGroups)
            {
                soundGroups[index++] = soundGroup.Value;
            }

            return soundGroups;
        }

        /// <summary>
        /// 增加声音组。
        /// </summary>
        /// <param name="soundGroupName">声音组名称。</param>
        /// <param name="soundGroupHelper">声音组辅助器。</param>
        /// <returns>是否增加声音组成功。</returns>
        public bool AddSoundGroup(string soundGroupName, ISoundGroupHelper soundGroupHelper)
        {
            return AddSoundGroup(soundGroupName, false, Constant.DefaultMute, Constant.DefaultVolume, soundGroupHelper);
        }

        /// <summary>
        /// 增加声音组。
        /// </summary>
        /// <param name="soundGroupName">声音组名称。</param>
        /// <param name="soundGroupAvoidBeingReplacedBySamePriority">声音组中的声音是否避免被同优先级声音替换。</param>
        /// <param name="soundGroupMute">声音组是否静音。</param>
        /// <param name="soundGroupVolume">声音组音量。</param>
        /// <param name="soundGroupHelper">声音组辅助器。</param>
        /// <returns>是否增加声音组成功。</returns>
        public bool AddSoundGroup(string soundGroupName, bool soundGroupAvoidBeingReplacedBySamePriority, bool soundGroupMute, float soundGroupVolume, ISoundGroupHelper soundGroupHelper)
        {
            if (string.IsNullOrEmpty(soundGroupName))
            {
                throw new GameFrameworkException("Sound group name is invalid.");
            }

            if (soundGroupHelper == null)
            {
                throw new GameFrameworkException("Sound group helper is invalid.");
            }

            if (HasSoundGroup(soundGroupName))
            {
                return false;
            }

            SoundGroup soundGroup = new SoundGroup(soundGroupName, soundGroupHelper);
            soundGroup.AvoidBeingReplacedBySamePriority = soundGroupAvoidBeingReplacedBySamePriority;
            soundGroup.Mute = soundGroupMute;
            soundGroup.Volume = soundGroupVolume;
            m_SoundGroups.Add(soundGroupName, soundGroup);

            return true;
        }

        /// <summary>
        /// 增加声音代理辅助器。
        /// </summary>
        /// <param name="soundGroupName">声音组名称。</param>
        /// <param name="soundAgentHelper">要增加的声音代理辅助器。</param>
        public void AddSoundAgentHelper(string soundGroupName, ISoundAgentHelper soundAgentHelper)
        {
            if (m_SoundHelper == null)
            {
                throw new GameFrameworkException("You must set sound helper first.");
            }

            SoundGroup soundGroup = (SoundGroup)GetSoundGroup(soundGroupName);
            if (soundGroup == null)
            {
                throw new GameFrameworkException(string.Format("Sound group '{0}' is not exist.", soundGroupName));
            }

            soundGroup.AddSoundAgentHelper(m_SoundHelper, soundAgentHelper);
        }

        /// <summary>
        /// 播放声音。
        /// </summary>
        /// <param name="soundAssetName">声音资源名称。</param>
        /// <param name="soundGroupName">声音组名称。</param>
        /// <returns>声音的序列编号。</returns>
        public int PlaySound(string soundAssetName, string soundGroupName)
        {
            return PlaySound(soundAssetName, soundGroupName, null, null);
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
            return PlaySound(soundAssetName, soundGroupName, playSoundParams, null);
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
            return PlaySound(soundAssetName, soundGroupName, null, userData);
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
            if (m_ResourceManager == null)
            {
                throw new GameFrameworkException("You must set resource manager first.");
            }

            if (m_SoundHelper == null)
            {
                throw new GameFrameworkException("You must set sound helper first.");
            }

            if (playSoundParams == null)
            {
                playSoundParams = new PlaySoundParams();
            }

            int serialId = m_Serial++;
            PlaySoundErrorCode? errorCode = null;
            string errorMessage = null;
            SoundGroup soundGroup = (SoundGroup)GetSoundGroup(soundGroupName);
            if (soundGroup == null)
            {
                errorCode = PlaySoundErrorCode.SoundGroupNotExist;
                errorMessage = string.Format("Sound group '{0}' is not exist.", soundGroupName);
            }
            else if (soundGroup.SoundAgentCount <= 0)
            {
                errorCode = PlaySoundErrorCode.SoundGroupHasNoAgent;
                errorMessage = string.Format("Sound group '{0}' is have no sound agent.", soundGroupName);
            }

            if (errorCode.HasValue)
            {
                if (m_PlaySoundFailureEventHandler != null)
                {
                    m_PlaySoundFailureEventHandler(this, new PlaySoundFailureEventArgs(serialId, soundAssetName, soundGroupName, playSoundParams, errorCode.Value, errorMessage, userData));
                    return serialId;
                }

                throw new GameFrameworkException(errorMessage);
            }

            m_ResourceManager.LoadAsset(soundAssetName, m_LoadAssetCallbacks, new PlaySoundInfo(serialId, soundGroup, playSoundParams, userData));
            return serialId;
        }

        /// <summary>
        /// 停止播放声音。
        /// </summary>
        /// <param name="serialId">要停止播放声音的序列编号。</param>
        /// <returns>是否停止播放声音成功。</returns>
        public bool StopSound(int serialId)
        {
            foreach (KeyValuePair<string, SoundGroup> soundGroup in m_SoundGroups)
            {
                if (soundGroup.Value.StopSound(serialId))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 停止所有声音。
        /// </summary>
        /// <param name="soundGroupName">声音组名称。</param>
        public void StopAllSounds(string soundGroupName)
        {
            SoundGroup soundGroup = (SoundGroup)GetSoundGroup(soundGroupName);
            if (soundGroup == null)
            {
                throw new GameFrameworkException(string.Format("Sound group '{0}' is not exist.", soundGroupName));
            }

            soundGroup.StopAllSounds();
        }

        /// <summary>
        /// 停止所有声音。
        /// </summary>
        public void StopAllSounds()
        {
            foreach (KeyValuePair<string, SoundGroup> soundGroup in m_SoundGroups)
            {
                soundGroup.Value.StopAllSounds();
            }
        }

        private void LoadSoundSuccessCallback(string soundAssetName, object soundAsset, float duration, object userData)
        {
            PlaySoundInfo playSoundInfo = (PlaySoundInfo)userData;
            if (playSoundInfo == null)
            {
                throw new GameFrameworkException("Play sound info is invalid.");
            }

            PlaySoundErrorCode? errorCode = null;
            ISoundAgent soundAgent = playSoundInfo.SoundGroup.PlaySound(playSoundInfo.SerialId, soundAsset, playSoundInfo.PlaySoundParams, out errorCode);
            if (soundAgent != null)
            {
                if (m_PlaySoundSuccessEventHandler != null)
                {
                    m_PlaySoundSuccessEventHandler(this, new PlaySoundSuccessEventArgs(playSoundInfo.SerialId, soundAssetName, soundAgent, duration, playSoundInfo.UserData));
                }
            }
            else
            {
                m_SoundHelper.ReleaseSoundAsset(soundAsset);
                string errorMessage = string.Format("Sound group '{0}' play sound '{1}' failure.", playSoundInfo.SoundGroup.Name, soundAssetName);
                if (m_PlaySoundFailureEventHandler != null)
                {
                    m_PlaySoundFailureEventHandler(this, new PlaySoundFailureEventArgs(playSoundInfo.SerialId, soundAssetName, playSoundInfo.SoundGroup.Name, playSoundInfo.PlaySoundParams, errorCode.Value, errorMessage, playSoundInfo.UserData));
                    return;
                }

                throw new GameFrameworkException(errorMessage);
            }
        }

        private void LoadSoundFailureCallback(string soundAssetName, LoadResourceStatus status, string errorMessage, object userData)
        {
            PlaySoundInfo playSoundInfo = (PlaySoundInfo)userData;
            if (playSoundInfo == null)
            {
                throw new GameFrameworkException("Play sound info is invalid.");
            }

            string appendErrorMessage = string.Format("Load sound failure, asset name '{0}', status '{1}', error message '{2}'.", soundAssetName, status.ToString(), errorMessage);
            if (m_PlaySoundFailureEventHandler != null)
            {
                m_PlaySoundFailureEventHandler(this, new PlaySoundFailureEventArgs(playSoundInfo.SerialId, soundAssetName, playSoundInfo.SoundGroup.Name, playSoundInfo.PlaySoundParams, PlaySoundErrorCode.LoadAssetFailure, appendErrorMessage, playSoundInfo.UserData));
                return;
            }

            throw new GameFrameworkException(appendErrorMessage);
        }

        private void LoadSoundUpdateCallback(string soundAssetName, float progress, object userData)
        {
            PlaySoundInfo playSoundInfo = (PlaySoundInfo)userData;
            if (playSoundInfo == null)
            {
                throw new GameFrameworkException("Play sound info is invalid.");
            }

            if (m_PlaySoundUpdateEventHandler != null)
            {
                m_PlaySoundUpdateEventHandler(this, new PlaySoundUpdateEventArgs(playSoundInfo.SerialId, soundAssetName, playSoundInfo.SoundGroup.Name, playSoundInfo.PlaySoundParams, progress, playSoundInfo.UserData));
            }
        }

        private void LoadSoundDependencyAssetCallback(string soundAssetName, string dependencyAssetName, int loadedCount, int totalCount, object userData)
        {
            PlaySoundInfo playSoundInfo = (PlaySoundInfo)userData;
            if (playSoundInfo == null)
            {
                throw new GameFrameworkException("Play sound info is invalid.");
            }

            if (m_PlaySoundDependencyAssetEventHandler != null)
            {
                m_PlaySoundDependencyAssetEventHandler(this, new PlaySoundDependencyAssetEventArgs(playSoundInfo.SerialId, soundAssetName, playSoundInfo.SoundGroup.Name, playSoundInfo.PlaySoundParams, dependencyAssetName, loadedCount, totalCount, playSoundInfo.UserData));
            }
        }
    }
}
