//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Resource
{
    /// <summary>
    /// 检查资源是否存在的结果。
    /// </summary>
    public enum HasAssetResult : byte
    {
        /// <summary>
        /// 不存在资源。
        /// </summary>
        NotExist = 0,

        /// <summary>
        /// 存在资源。
        /// </summary>
        Asset,

        /// <summary>
        /// 存在二进制资源。
        /// </summary>
        Binary
    }
}
