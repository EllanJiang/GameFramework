//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Sound
{
    /// <summary>
    /// 播放声音错误码。
    /// </summary>
    public enum PlaySoundErrorCode : byte
    {
        /// <summary>
        /// 未知错误。
        /// </summary>
        Unknown = 0,

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
        SetSoundAssetFailure
    }
}
