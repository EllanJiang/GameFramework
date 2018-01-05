//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Sound
{
    /// <summary>
    /// 声音组接口。
    /// </summary>
    public interface ISoundGroup
    {
        /// <summary>
        /// 获取声音组名称。
        /// </summary>
        string Name
        {
            get;
        }

        /// <summary>
        /// 获取声音代理数。
        /// </summary>
        int SoundAgentCount
        {
            get;
        }

        /// <summary>
        /// 获取或设置声音组中的声音是否避免被同优先级声音替换。
        /// </summary>
        bool AvoidBeingReplacedBySamePriority
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置声音组静音。
        /// </summary>
        bool Mute
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置声音组音量。
        /// </summary>
        float Volume
        {
            get;
            set;
        }

        /// <summary>
        /// 获取声音组辅助器。
        /// </summary>
        ISoundGroupHelper Helper
        {
            get;
        }

        /// <summary>
        /// 停止所有已加载的声音。
        /// </summary>
        void StopAllLoadedSounds();

        /// <summary>
        /// 停止所有已加载的声音。
        /// </summary>
        /// <param name="fadeOutSeconds">声音淡出时间，以秒为单位。</param>
        void StopAllLoadedSounds(float fadeOutSeconds);
    }
}
