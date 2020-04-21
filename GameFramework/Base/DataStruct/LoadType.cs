//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework
{
    /// <summary>
    /// 加载方式。
    /// </summary>
    public enum LoadType : byte
    {
        /// <summary>
        /// 按文本从资源加载。
        /// </summary>
        TextFromAsset = 0,

        /// <summary>
        /// 按二进制流从资源加载。
        /// </summary>
        BytesFromAsset,

        /// <summary>
        /// 按二进制流从资源加载。
        /// </summary>
        StreamFromAsset,

        /// <summary>
        /// 按文本从二进制资源加载。
        /// </summary>
        TextFromBinary,

        /// <summary>
        /// 按二进制流从二进制资源加载。
        /// </summary>
        BytesFromBinary,

        /// <summary>
        /// 按二进制流从二进制资源加载。
        /// </summary>
        StreamFromBinary
    }
}
