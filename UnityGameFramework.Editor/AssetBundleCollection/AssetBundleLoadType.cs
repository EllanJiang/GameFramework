//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace UnityGameFramework.Editor.AssetBundleTools
{
    /// <summary>
    /// 资源加载方式类型。
    /// </summary>
    internal enum AssetBundleLoadType
    {
        /// <summary>
        /// 从文件加载。
        /// </summary>
        LoadFromFile = 0,

        /// <summary>
        /// 从内存加载。
        /// </summary>
        LoadFromMemory,

        /// <summary>
        /// 从内存快速解密加载。
        /// </summary>
        LoadFromMemoryAndQuickDecrypt,

        /// <summary>
        /// 从内存解密加载。
        /// </summary>
        LoadFromMemoryAndDecrypt,
    }
}
