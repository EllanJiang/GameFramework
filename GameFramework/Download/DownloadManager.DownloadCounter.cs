//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Download
{
    internal sealed partial class DownloadManager : GameFrameworkModule, IDownloadManager
    {
        private sealed partial class DownloadCounter
        {
            private readonly GameFrameworkLinkedList<DownloadCounterNode> m_DownloadCounterNodes;
            private float m_UpdateInterval;
            private float m_RecordInterval;
            private float m_CurrentSpeed;
            private float m_Accumulator;
            private float m_TimeLeft;

            public DownloadCounter(float updateInterval, float recordInterval)
            {
                if (updateInterval <= 0f)
                {
                    throw new GameFrameworkException("Update interval is invalid.");
                }

                if (recordInterval <= 0f)
                {
                    throw new GameFrameworkException("Record interval is invalid.");
                }

                m_DownloadCounterNodes = new GameFrameworkLinkedList<DownloadCounterNode>();
                m_UpdateInterval = updateInterval;
                m_RecordInterval = recordInterval;
                Reset();
            }

            public float UpdateInterval
            {
                get
                {
                    return m_UpdateInterval;
                }
                set
                {
                    if (value <= 0f)
                    {
                        throw new GameFrameworkException("Update interval is invalid.");
                    }

                    m_UpdateInterval = value;
                    Reset();
                }
            }

            public float RecordInterval
            {
                get
                {
                    return m_RecordInterval;
                }
                set
                {
                    if (value <= 0f)
                    {
                        throw new GameFrameworkException("Record interval is invalid.");
                    }

                    m_RecordInterval = value;
                    Reset();
                }
            }

            public float CurrentSpeed
            {
                get
                {
                    return m_CurrentSpeed;
                }
            }

            public void Shutdown()
            {
                Reset();
            }

            public void Update(float elapseSeconds, float realElapseSeconds)
            {
                if (m_DownloadCounterNodes.Count <= 0)
                {
                    return;
                }

                m_Accumulator += realElapseSeconds;
                if (m_Accumulator > m_RecordInterval)
                {
                    m_Accumulator = m_RecordInterval;
                }

                m_TimeLeft -= realElapseSeconds;
                foreach (DownloadCounterNode downloadCounterNode in m_DownloadCounterNodes)
                {
                    downloadCounterNode.Update(elapseSeconds, realElapseSeconds);
                }

                while (m_DownloadCounterNodes.Count > 0)
                {
                    DownloadCounterNode downloadCounterNode = m_DownloadCounterNodes.First.Value;
                    if (downloadCounterNode.ElapseSeconds < m_RecordInterval)
                    {
                        break;
                    }

                    ReferencePool.Release(downloadCounterNode);
                    m_DownloadCounterNodes.RemoveFirst();
                }

                if (m_DownloadCounterNodes.Count <= 0)
                {
                    Reset();
                    return;
                }

                if (m_TimeLeft <= 0f)
                {
                    int totalDownloadLength = 0;
                    foreach (DownloadCounterNode downloadCounterNode in m_DownloadCounterNodes)
                    {
                        totalDownloadLength += downloadCounterNode.DownloadedLength;
                    }

                    m_CurrentSpeed = m_Accumulator > 0f ? totalDownloadLength / m_Accumulator : 0f;
                    m_TimeLeft += m_UpdateInterval;
                }
            }

            public void RecordDownloadedLength(int downloadedLength)
            {
                if (downloadedLength <= 0)
                {
                    return;
                }

                if (m_DownloadCounterNodes.Count > 0)
                {
                    DownloadCounterNode downloadCounterNode = m_DownloadCounterNodes.Last.Value;
                    if (downloadCounterNode.ElapseSeconds < m_UpdateInterval)
                    {
                        downloadCounterNode.AddDownloadedLength(downloadedLength);
                        return;
                    }
                }

                m_DownloadCounterNodes.AddLast(DownloadCounterNode.Create(downloadedLength));
            }

            private void Reset()
            {
                m_DownloadCounterNodes.Clear();
                m_CurrentSpeed = 0f;
                m_Accumulator = 0f;
                m_TimeLeft = 0f;
            }
        }
    }
}
