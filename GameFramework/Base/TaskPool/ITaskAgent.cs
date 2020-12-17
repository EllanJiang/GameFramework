//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework
{
    /// <summary>
    /// 任务代理接口。
    /// </summary>
    /// <typeparam name="T">任务类型。</typeparam>
    internal interface ITaskAgent<T> where T : TaskBase
    {
        /// <summary>
        /// 获取任务。
        /// </summary>
        T Task
        {
            get;
        }

        /// <summary>
        /// 初始化任务代理。
        /// </summary>
        void Initialize();

        /// <summary>
        /// 任务代理轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        void Update(float elapseSeconds, float realElapseSeconds);

        /// <summary>
        /// 关闭并清理任务代理。
        /// </summary>
        void Shutdown();

        /// <summary>
        /// 开始处理任务。
        /// </summary>
        /// <param name="task">要处理的任务。</param>
        /// <returns>开始处理任务的状态。</returns>
        StartTaskStatus Start(T task);

        /// <summary>
        /// 停止正在处理的任务并重置任务代理。
        /// </summary>
        void Reset();
    }
}
