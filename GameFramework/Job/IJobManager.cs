//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Job
{
    /// <summary>
    /// 任务管理器。
    /// </summary>
    public interface IJobManager
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
        T GenerateJob<T>() where T : JobBase, new();

        /// <summary>
        /// 生成任务。
        /// </summary>
        /// <typeparam name="T">任务的类型。</typeparam>
        /// <param name="priority">任务的优先级。</param>
        /// <returns>生成的指定类型的任务。</returns>
        T GenerateJob<T>(int priority) where T : JobBase, new();

        /// <summary>
        /// 取消任务。
        /// </summary>
        /// <param name="serialId">要取消的任务的序列编号。</param>
        /// <returns>取消任务是否成功。</returns>
        bool CancelJob(int serialId);

        /// <summary>
        /// 取消任务。
        /// </summary>
        /// <param name="serialId">要取消的任务的序列编号。</param>
        /// <param name="reason">任务取消的原因。</param>
        /// <returns>取消任务是否成功。</returns>
        bool CancelJob(int serialId, string reason);

        /// <summary>
        /// 取消任务。
        /// </summary>
        /// <param name="job">要取消的任务。</param>
        /// <returns>取消任务是否成功。</returns>
        bool CancelJob(JobBase job);

        /// <summary>
        /// 取消任务。
        /// </summary>
        /// <param name="job">要取消的任务。</param>
        /// <param name="reason">任务取消的原因。</param>
        /// <returns>取消任务是否成功。</returns>
        bool CancelJob(JobBase job, string reason);

        /// <summary>
        /// 取消所有任务。
        /// </summary>
        /// <param name="reason">任务取消的原因。</param>
        void CancelAllJobs(string reason);
    }
}
