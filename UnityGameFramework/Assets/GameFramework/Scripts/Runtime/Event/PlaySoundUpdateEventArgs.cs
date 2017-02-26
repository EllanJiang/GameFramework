//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.Event;
using GameFramework.Sound;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 播放声音更新事件。
    /// </summary>
    public sealed class PlaySoundUpdateEventArgs : GameEventArgs
    {
        /// <summary>
        /// 初始化播放声音更新事件的新实例。
        /// </summary>
        /// <param name="e">内部事件。</param>
        public PlaySoundUpdateEventArgs(GameFramework.Sound.PlaySoundUpdateEventArgs e)
        {
            PlaySoundInfo playSoundInfo = (PlaySoundInfo)e.UserData;
            SerialId = e.SerialId;
            SoundAssetName = e.SoundAssetName;
            SoundGroupName = e.SoundGroupName;
            PlaySoundParams = e.PlaySoundParams;
            Progress = e.Progress;
            BindingEntity = playSoundInfo.BindingEntity;
            UserData = playSoundInfo.UserData;
        }

        /// <summary>
        /// 获取播放声音更新事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return (int)EventId.PlaySoundUpdate;
            }
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
        /// 获取声音绑定的实体。
        /// </summary>
        public Entity BindingEntity
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
