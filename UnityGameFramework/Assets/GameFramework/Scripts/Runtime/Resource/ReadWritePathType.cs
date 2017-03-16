//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 读写区路径类型。
    /// </summary>
    public enum ReadWritePathType
    {
        /// <summary>
        /// 未指定。
        /// </summary>
        Unspecified = 0,

        /// <summary>
        /// 临时缓存。
        /// </summary>
        TemporaryCache,

        /// <summary>
        /// 持久化数据。
        /// </summary>
        PersistentData,
    }
}
