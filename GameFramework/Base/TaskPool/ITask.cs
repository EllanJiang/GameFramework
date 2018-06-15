//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework
{
    /// <summary>
    /// 任务接口。
    /// </summary>
    internal interface ITask
    {
        /// <summary>
        /// 获取任务的序列编号。
        /// </summary>
        int SerialId
        {
            get;
        }

        /// <summary>
        /// 获取任务的优先级。
        /// </summary>
        int Priority
        {
            get;
        }

        /// <summary>
        /// 获取任务是否完成。
        /// </summary>
        bool Done
        {
            get;
        }
    }
}
