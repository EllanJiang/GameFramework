//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System;

namespace GameFramework
{
    /// <summary>
    /// 事件池模式。
    /// </summary>
    [Flags]
    internal enum EventPoolMode
    {
        /// <summary>
        /// 默认事件池模式，即必须存在有且只有一个事件处理函数。
        /// </summary>
        Default = 0,

        /// <summary>
        /// 允许不存在事件处理函数。
        /// </summary>
        AllowNoHandler = 1,

        /// <summary>
        /// 允许存在多个事件处理函数。
        /// </summary>
        AllowMultiHandler = 2,

        /// <summary>
        /// 允许存在重复的事件处理函数。
        /// </summary>
        AllowDuplicateHandler = 4,
    }
}
