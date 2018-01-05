//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Sound
{
    /// <summary>
    /// 声音辅助器接口。
    /// </summary>
    public interface ISoundHelper
    {
        /// <summary>
        /// 释放声音资源。
        /// </summary>
        /// <param name="soundAsset">要释放的声音资源。</param>
        void ReleaseSoundAsset(object soundAsset);
    }
}
