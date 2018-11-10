//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System.Collections.Generic;

namespace GameFramework.Job
{
    /// <summary>
    /// 任务管理器。
    /// </summary>
    internal sealed class JobManager : GameFrameworkModule, IJobManager
    {
        private readonly LinkedList<JobBase> m_Jobs;
        private int m_Serial;

        /// <summary>
        /// 初始化任务管理器的新实例。
        /// </summary>
        public JobManager()
        {
            m_Jobs = new LinkedList<JobBase>();
            m_Serial = 0;
        }

        /// <summary>
        /// 获取任务数量。
        /// </summary>
        public int Count
        {
            get
            {
                return m_Jobs.Count;
            }
        }

        /// <summary>
        /// 任务管理器轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {
            LinkedListNode<JobBase> current = m_Jobs.First;
            while (current != null)
            {
                JobBase job = current.Value;
                if (job.Status == JobStatus.Free)
                {
                    throw new GameFrameworkException("Job status is invalid.");
                }

                if (job.Status == JobStatus.Waiting)
                {
                    job.OnStart();
                }

                if (job.Status == JobStatus.Running)
                {
                    job.OnUpdate(elapseSeconds, realElapseSeconds);
                    current = current.Next;
                }
                else
                {
                    LinkedListNode<JobBase> next = current.Next;
                    m_Jobs.Remove(current);
                    ReferencePool.Release((IReference)job);
                    current = next;
                }
            }
        }

        /// <summary>
        /// 关闭并清理任务管理器。
        /// </summary>
        internal override void Shutdown()
        {
            CancelAllJobs(null);

            foreach (JobBase job in m_Jobs)
            {
                ReferencePool.Release((IReference)job);
            }

            m_Jobs.Clear();
        }

        /// <summary>
        /// 生成任务。
        /// </summary>
        /// <typeparam name="T">任务的类型。</typeparam>
        /// <returns>生成的指定类型的任务。</returns>
        public T GenerateJob<T>() where T : JobBase, new()
        {
            return GenerateJob<T>(JobBase.DefaultPriority);
        }

        /// <summary>
        /// 生成任务。
        /// </summary>
        /// <typeparam name="T">任务的类型。</typeparam>
        /// <param name="priority">任务的优先级。</param>
        /// <returns>生成的指定类型的任务。</returns>
        public T GenerateJob<T>(int priority) where T : JobBase, new()
        {
            T job = ReferencePool.Acquire<T>();
            job.SerialId = m_Serial++;
            job.Priority = priority;
            job.OnGenerate();

            LinkedListNode<JobBase> current = m_Jobs.First;
            while (current != null)
            {
                if (job.Priority > current.Value.Priority)
                {
                    break;
                }

                current = current.Next;
            }

            if (current != null)
            {
                m_Jobs.AddBefore(current, job);
            }
            else
            {
                m_Jobs.AddLast(job);
            }

            return job;
        }

        /// <summary>
        /// 取消任务。
        /// </summary>
        /// <param name="serialId">要取消的任务的序列编号。</param>
        /// <returns>取消任务是否成功。</returns>
        public bool CancelJob(int serialId)
        {
            return CancelJob(serialId, null);
        }

        /// <summary>
        /// 取消任务。
        /// </summary>
        /// <param name="serialId">要取消的任务的序列编号。</param>
        /// <param name="reason">任务取消的原因。</param>
        /// <returns>取消任务是否成功。</returns>
        public bool CancelJob(int serialId, string reason)
        {
            foreach (JobBase job in m_Jobs)
            {
                if (job.SerialId != serialId)
                {
                    continue;
                }

                if (job.Status != JobStatus.Waiting && job.Status != JobStatus.Running)
                {
                    return false;
                }

                job.OnCancel(reason);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 取消任务。
        /// </summary>
        /// <param name="job">要取消的任务。</param>
        /// <returns>取消任务是否成功。</returns>
        public bool CancelJob(JobBase job)
        {
            if (job == null)
            {
                throw new GameFrameworkException("Job is invalid.");
            }

            return CancelJob(job.SerialId, null);
        }

        /// <summary>
        /// 取消任务。
        /// </summary>
        /// <param name="job">要取消的任务。</param>
        /// <param name="reason">任务取消的原因。</param>
        /// <returns>取消任务是否成功。</returns>
        public bool CancelJob(JobBase job, string reason)
        {
            if (job == null)
            {
                throw new GameFrameworkException("Job is invalid.");
            }

            return CancelJob(job.SerialId, reason);
        }

        /// <summary>
        /// 取消所有任务。
        /// </summary>
        /// <param name="reason">任务取消的原因。</param>
        public void CancelAllJobs(string reason)
        {
            foreach (JobBase job in m_Jobs)
            {
                if (job.Status != JobStatus.Waiting && job.Status != JobStatus.Running)
                {
                    continue;
                }

                job.OnCancel(reason);
            }
        }
    }
}
