//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Resource
{
    /// <summary>
    /// 资源模式。
    /// </summary>
    public enum ResourceMode
    {
        /// <summary>
        /// 未指定。
        /// </summary>
        Unspecified = 0,

        /// <summary>
        /// 单机模式。
        /// </summary>
        Package,

        /// <summary>
        /// 可更新模式。
        /// </summary>
        Updatable,
    }
}
