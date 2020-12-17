//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Sound
{
    /// <summary>
    /// 播放声音更新事件。
    /// </summary>
    public sealed class PlaySoundUpdateEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化播放声音更新事件的新实例。
        /// </summary>
        public PlaySoundUpdateEventArgs()
        {
            SerialId = 0;
            SoundAssetName = null;
            SoundGroupName = null;
            PlaySoundParams = null;
            Progress = 0f;
            UserData = null;
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
        /// 获取声音组名称。
        /// </summary>
        public string SoundGroupName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取播放声音参数。
        /// </summary>
        public PlaySoundParams PlaySoundParams
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取加载声音进度。
        /// </summary>
        public float Progress
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

        /// <summary>
        /// 创建播放声音更新事件。
        /// </summary>
        /// <param name="serialId">声音的序列编号。</param>
        /// <param name="soundAssetName">声音资源名称。</param>
        /// <param name="soundGroupName">声音组名称。</param>
        /// <param name="playSoundParams">播放声音参数。</param>
        /// <param name="progress">加载声音进度。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>创建的播放声音更新事件。</returns>
        public static PlaySoundUpdateEventArgs Create(int serialId, string soundAssetName, string soundGroupName, PlaySoundParams playSoundParams, float progress, object userData)
        {
            PlaySoundUpdateEventArgs playSoundUpdateEventArgs = ReferencePool.Acquire<PlaySoundUpdateEventArgs>();
            playSoundUpdateEventArgs.SerialId = serialId;
            playSoundUpdateEventArgs.SoundAssetName = soundAssetName;
            playSoundUpdateEventArgs.SoundGroupName = soundGroupName;
            playSoundUpdateEventArgs.PlaySoundParams = playSoundParams;
            playSoundUpdateEventArgs.Progress = progress;
            playSoundUpdateEventArgs.UserData = userData;
            return playSoundUpdateEventArgs;
        }

        /// <summary>
        /// 清理播放声音更新事件。
        /// </summary>
        public override void Clear()
        {
            SerialId = 0;
            SoundAssetName = null;
            SoundGroupName = null;
            PlaySoundParams = null;
            Progress = 0f;
            UserData = null;
        }
    }
}
