//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.Entity;

namespace GameFramework.Sound
{
    /// <summary>
    /// 声音代理接口。
    /// </summary>
    public interface ISoundAgent
    {
        /// <summary>
        /// 获取所在的声音组。
        /// </summary>
        ISoundGroup SoundGroup
        {
            get;
        }

        /// <summary>
        /// 获取声音的序列编号。
        /// </summary>
        int SerialId
        {
            get;
        }

        /// <summary>
        /// 获取当前是否正在播放。
        /// </summary>
        bool IsPlaying
        {
            get;
        }

        /// <summary>
        /// 获取或设置播放位置。
        /// </summary>
        float Time
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置是否静音。
        /// </summary>
        bool Mute
        {
            get;
        }

        /// <summary>
        /// 获取或设置在声音组内是否静音。
        /// </summary>
        bool MuteInSoundGroup
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置是否循环播放。
        /// </summary>
        bool Loop
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置声音优先级。
        /// </summary>
        int Priority
        {
            get;
            set;
        }

        /// <summary>
        /// 获取音量大小。
        /// </summary>
        float Volume
        {
            get;
        }

        /// <summary>
        /// 获取或设置在声音组内音量大小。
        /// </summary>
        float VolumeInSoundGroup
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置声音音调。
        /// </summary>
        float Pitch
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置声音立体声声相。
        /// </summary>
        float PanStereo
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置声音空间混合量。
        /// </summary>
        float SpatialBlend
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置声音最大距离。
        /// </summary>
        float MaxDistance
        {
            get;
            set;
        }

        /// <summary>
        /// 播放声音。
        /// </summary>
        void Play();

        /// <summary>
        /// 停止播放声音。
        /// </summary>
        void Stop();

        /// <summary>
        /// 暂停播放声音。
        /// </summary>
        void Pause();

        /// <summary>
        /// 恢复播放声音。
        /// </summary>
        void Resume();

        /// <summary>
        /// 重置声音代理。
        /// </summary>
        void Reset();

        /// <summary>
        /// 设置声音绑定的实体。
        /// </summary>
        /// <param name="bindingEntity">声音绑定的实体。</param>
        void SetBindingEntity(IEntity bindingEntity);
    }
}
