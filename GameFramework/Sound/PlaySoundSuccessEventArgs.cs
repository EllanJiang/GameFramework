//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Sound
{
    /// <summary>
    /// 播放声音成功事件。
    /// </summary>
    public sealed class PlaySoundSuccessEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化播放声音成功事件的新实例。
        /// </summary>
        /// <param name="serialId">声音的序列编号。</param>
        /// <param name="soundAssetName">声音资源名称。</param>
        /// <param name="soundAgent">用于播放的声音代理。</param>
        /// <param name="duration">加载持续时间。</param>
        /// <param name="userData">用户自定义数据。</param>
        public PlaySoundSuccessEventArgs(int serialId, string soundAssetName, ISoundAgent soundAgent, float duration, object userData)
        {
            SerialId = serialId;
            SoundAssetName = soundAssetName;
            SoundAgent = soundAgent;
            Duration = duration;
            UserData = userData;
        }

        /// <summary>
        /// 获取声音的序列编号。
        /// </summary>
        public int SerialId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取声音资源名称。
        /// </summary>
        public string SoundAssetName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取用于播放的声音代理。
        /// </summary>
        public ISoundAgent SoundAgent
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取加载持续时间。
        /// </summary>
        public float Duration
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取用户自定义数据。
        /// </summary>
        public object UserData
        {
            get;
            private set;
        }
    }
}
