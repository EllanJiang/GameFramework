//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Sound
{
    /// <summary>
    /// 播放声音错误码。
    /// </summary>
    public enum PlaySoundErrorCode
    {
        /// <summary>
        /// 声音组不存在。
        /// </summary>
        SoundGroupNotExist,

        /// <summary>
        /// 声音组没有声音代理。
        /// </summary>
        SoundGroupHasNoAgent,

        /// <summary>
        /// 加载资源失败。
        /// </summary>
        LoadAssetFailure,

        /// <summary>
        /// 播放声音因优先级低被忽略。
        /// </summary>
        IgnoredDueToLowPriority,

        /// <summary>
        /// 设置声音资源失败。
        /// </summary>
        SetSoundAssetFailure,
    }
}
