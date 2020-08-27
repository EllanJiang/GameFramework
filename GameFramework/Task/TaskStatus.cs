//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Task
{
    /// <summary>
    /// 任务状态。
    /// </summary>
    public enum TaskStatus : byte
    {
        /// <summary>
        /// 空闲。
        /// </summary>
        Free = 0,

        /// <summary>
        /// 等待中。
        /// </summary>
        Waiting,

        /// <summary>
        /// 运行中。
        /// </summary>
        Running,

        /// <summary>
        /// 已完成。
        /// </summary>
        Completed,

        /// <summary>
        /// 已失败。
        /// </summary>
        Failed,

        /// <summary>
        /// 已取消。
        /// </summary>
        Canceled
    }
}
