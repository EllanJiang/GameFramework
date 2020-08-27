//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Task
{
    /// <summary>
    /// 任务管理器。
    /// </summary>
    public interface ITaskManager
    {
        /// <summary>
        /// 获取任务数量。
        /// </summary>
        int Count
        {
            get;
        }

        /// <summary>
        /// 生成任务。
        /// </summary>
        /// <typeparam name="T">任务的类型。</typeparam>
        /// <returns>生成的指定类型的任务。</returns>
        T GenerateTask<T>() where T : TaskBase, new();

        /// <summary>
        /// 生成任务。
        /// </summary>
        /// <typeparam name="T">任务的类型。</typeparam>
        /// <param name="priority">任务的优先级。</param>
        /// <returns>生成的指定类型的任务。</returns>
        T GenerateTask<T>(int priority) where T : TaskBase, new();

        /// <summary>
        /// 取消任务。
        /// </summary>
        /// <param name="serialId">要取消的任务的序列编号。</param>
        /// <returns>是否取消任务成功。</returns>
        bool CancelTask(int serialId);

        /// <summary>
        /// 取消任务。
        /// </summary>
        /// <param name="serialId">要取消的任务的序列编号。</param>
        /// <param name="reason">任务取消的原因。</param>
        /// <returns>是否取消任务成功。</returns>
        bool CancelTask(int serialId, string reason);

        /// <summary>
        /// 取消任务。
        /// </summary>
        /// <param name="task">要取消的任务。</param>
        /// <returns>是否取消任务成功。</returns>
        bool CancelTask(TaskBase task);

        /// <summary>
        /// 取消任务。
        /// </summary>
        /// <param name="task">要取消的任务。</param>
        /// <param name="reason">任务取消的原因。</param>
        /// <returns>是否取消任务成功。</returns>
        bool CancelTask(TaskBase task, string reason);

        /// <summary>
        /// 取消所有任务。
        /// </summary>
        /// <param name="reason">任务取消的原因。</param>
        void CancelAllTasks(string reason);
    }
}
