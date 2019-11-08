//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System.Collections.Generic;

namespace GameFramework
{
    /// <summary>
    /// 任务池。
    /// </summary>
    /// <typeparam name="T">任务类型。</typeparam>
    internal sealed class TaskPool<T> where T : TaskBase
    {
        private readonly Stack<ITaskAgent<T>> m_FreeAgents;
        private readonly GameFrameworkLinkedList<ITaskAgent<T>> m_WorkingAgents;
        private readonly GameFrameworkLinkedList<T> m_WaitingTasks;
        private bool m_Paused;

        /// <summary>
        /// 初始化任务池的新实例。
        /// </summary>
        public TaskPool()
        {
            m_FreeAgents = new Stack<ITaskAgent<T>>();
            m_WorkingAgents = new GameFrameworkLinkedList<ITaskAgent<T>>();
            m_WaitingTasks = new GameFrameworkLinkedList<T>();
            m_Paused = false;
        }

        /// <summary>
        /// 获取或设置任务池是否被暂停。
        /// </summary>
        public bool Paused
        {
            get
            {
                return m_Paused;
            }
            set
            {
                m_Paused = value;
            }
        }

        /// <summary>
        /// 获取任务代理总数量。
        /// </summary>
        public int TotalAgentCount
        {
            get
            {
                return FreeAgentCount + WorkingAgentCount;
            }
        }

        /// <summary>
        /// 获取可用任务代理数量。
        /// </summary>
        public int FreeAgentCount
        {
            get
            {
                return m_FreeAgents.Count;
            }
        }

        /// <summary>
        /// 获取工作中任务代理数量。
        /// </summary>
        public int WorkingAgentCount
        {
            get
            {
                return m_WorkingAgents.Count;
            }
        }

        /// <summary>
        /// 获取等待任务数量。
        /// </summary>
        public int WaitingTaskCount
        {
            get
            {
                return m_WaitingTasks.Count;
            }
        }

        /// <summary>
        /// 任务池轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        public void Update(float elapseSeconds, float realElapseSeconds)
        {
            if (m_Paused)
            {
                return;
            }

            ProcessRunningTasks(elapseSeconds, realElapseSeconds);
            ProcessWaitingTasks(elapseSeconds, realElapseSeconds);
        }

        /// <summary>
        /// 关闭并清理任务池。
        /// </summary>
        public void Shutdown()
        {
            RemoveAllTasks();

            while (FreeAgentCount > 0)
            {
                m_FreeAgents.Pop().Shutdown();
            }
        }

        /// <summary>
        /// 增加任务代理。
        /// </summary>
        /// <param name="agent">要增加的任务代理。</param>
        public void AddAgent(ITaskAgent<T> agent)
        {
            if (agent == null)
            {
                throw new GameFrameworkException("Task agent is invalid.");
            }

            agent.Initialize();
            m_FreeAgents.Push(agent);
        }

        /// <summary>
        /// 增加任务。
        /// </summary>
        /// <param name="task">要增加的任务。</param>
        public void AddTask(T task)
        {
            LinkedListNode<T> current = m_WaitingTasks.Last;
            while (current != null)
            {
                if (task.Priority <= current.Value.Priority)
                {
                    break;
                }

                current = current.Previous;
            }

            if (current != null)
            {
                m_WaitingTasks.AddAfter(current, task);
            }
            else
            {
                m_WaitingTasks.AddFirst(task);
            }
        }

        /// <summary>
        /// 移除任务。
        /// </summary>
        /// <param name="serialId">要移除任务的序列编号。</param>
        /// <returns>移除任务是否成功。</returns>
        public bool RemoveTask(int serialId)
        {
            foreach (T task in m_WaitingTasks)
            {
                if (task.SerialId == serialId)
                {
                    m_WaitingTasks.Remove(task);
                    ReferencePool.Release(task);
                    return true;
                }
            }

            foreach (ITaskAgent<T> workingAgent in m_WorkingAgents)
            {
                if (workingAgent.Task.SerialId == serialId)
                {
                    T task = workingAgent.Task;
                    workingAgent.Reset();
                    m_FreeAgents.Push(workingAgent);
                    m_WorkingAgents.Remove(workingAgent);
                    ReferencePool.Release(task);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 移除所有任务。
        /// </summary>
        public void RemoveAllTasks()
        {
            foreach (T task in m_WaitingTasks)
            {
                ReferencePool.Release(task);
            }

            m_WaitingTasks.Clear();

            foreach (ITaskAgent<T> workingAgent in m_WorkingAgents)
            {
                T task = workingAgent.Task;
                workingAgent.Reset();
                m_FreeAgents.Push(workingAgent);
                ReferencePool.Release(task);
            }

            m_WorkingAgents.Clear();
        }

        public TaskInfo[] GetAllTaskInfos()
        {
            List<TaskInfo> results = new List<TaskInfo>();
            foreach (ITaskAgent<T> workingAgent in m_WorkingAgents)
            {
                T workingTask = workingAgent.Task;
                results.Add(new TaskInfo(workingTask.SerialId, workingTask.Priority, workingTask.Done ? TaskStatus.Done : TaskStatus.Doing, workingTask.Description));
            }

            foreach (T waitingTask in m_WaitingTasks)
            {
                results.Add(new TaskInfo(waitingTask.SerialId, waitingTask.Priority, TaskStatus.Todo, waitingTask.Description));
            }

            return results.ToArray();
        }

        private void ProcessRunningTasks(float elapseSeconds, float realElapseSeconds)
        {
            LinkedListNode<ITaskAgent<T>> current = m_WorkingAgents.First;
            while (current != null)
            {
                T task = current.Value.Task;
                if (!task.Done)
                {
                    current.Value.Update(elapseSeconds, realElapseSeconds);
                    current = current.Next;
                    continue;
                }

                LinkedListNode<ITaskAgent<T>> next = current.Next;
                current.Value.Reset();
                m_FreeAgents.Push(current.Value);
                m_WorkingAgents.Remove(current);
                ReferencePool.Release(task);
                current = next;
            }
        }

        private void ProcessWaitingTasks(float elapseSeconds, float realElapseSeconds)
        {
            LinkedListNode<T> current = m_WaitingTasks.First;
            while (current != null && FreeAgentCount > 0)
            {
                ITaskAgent<T> agent = m_FreeAgents.Pop();
                LinkedListNode<ITaskAgent<T>> agentNode = m_WorkingAgents.AddLast(agent);
                T task = current.Value;
                LinkedListNode<T> next = current.Next;
                StartTaskStatus status = agent.Start(task);
                if (status == StartTaskStatus.Done || status == StartTaskStatus.HasToWait || status == StartTaskStatus.UnknownError)
                {
                    agent.Reset();
                    m_FreeAgents.Push(agent);
                    m_WorkingAgents.Remove(agentNode);
                }

                if (status == StartTaskStatus.Done || status == StartTaskStatus.CanResume || status == StartTaskStatus.UnknownError)
                {
                    m_WaitingTasks.Remove(current);
                }

                if (status == StartTaskStatus.Done || status == StartTaskStatus.UnknownError)
                {
                    ReferencePool.Release(task);
                }

                current = next;
            }
        }
    }
}
