//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;

namespace GameFramework.Sound
{
    /// <summary>
    /// 声音代理辅助器接口。
    /// </summary>
    public interface ISoundAgentHelper
    {
        /// <summary>
        /// 获取当前是否正在播放。
        /// </summary>
        bool IsPlaying
        {
            get;
        }

        /// <summary>
        /// 获取声音长度。
        /// </summary>
        float Length
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
        /// 获取或设置音量大小。
        /// </summary>
        float Volume
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
        /// 获取或设置声音多普勒等级。
        /// </summary>
        float DopplerLevel
        {
            get;
            set;
        }

        /// <summary>
        /// 重置声音代理事件。
        /// </summary>
        event EventHandler<ResetSoundAgentEventArgs> ResetSoundAgent;

        /// <summary>
        /// 播放声音。
        /// </summary>
        /// <param name="fadeInSeconds">声音淡入时间，以秒为单位。</param>
        void Play(float fadeInSeconds);

        /// <summary>
        /// 停止播放声音。
        /// </summary>
        /// <param name="fadeOutSeconds">声音淡出时间，以秒为单位。</param>
        void Stop(float fadeOutSeconds);

        /// <summary>
        /// 暂停播放声音。
        /// </summary>
        /// <param name="fadeOutSeconds">声音淡出时间，以秒为单位。</param>
        void Pause(float fadeOutSeconds);

        /// <summary>
        /// 恢复播放声音。
        /// </summary>
        /// <param name="fadeInSeconds">声音淡入时间，以秒为单位。</param>
        void Resume(float fadeInSeconds);

        /// <summary>
        /// 重置声音代理辅助器。
        /// </summary>
        void Reset();

        /// <summary>
        /// 设置声音资源。
        /// </summary>
        /// <param name="soundAsset">声音资源。</param>
        /// <returns>是否设置声音资源成功。</returns>
        bool SetSoundAsset(object soundAsset);
    }
}
