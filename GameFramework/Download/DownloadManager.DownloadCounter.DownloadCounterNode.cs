//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Download
{
    internal sealed partial class DownloadManager : GameFrameworkModule, IDownloadManager
    {
        private sealed partial class DownloadCounter
        {
            private sealed class DownloadCounterNode : IReference
            {
                private int m_DownloadedLength;
                private float m_ElapseSeconds;

                public DownloadCounterNode()
                {
                    m_DownloadedLength = 0;
                    m_ElapseSeconds = 0f;
                }

                public int DownloadedLength
                {
                    get
                    {
                        return m_DownloadedLength;
                    }
                    set
                    {
                        m_DownloadedLength = value;
                    }
                }

                public float ElapseSeconds
                {
                    get
                    {
                        return m_ElapseSeconds;
                    }
                }

                public void Update(float elapseSeconds, float realElapseSeconds)
                {
                    m_ElapseSeconds += realElapseSeconds;
                }

                public void Clear()
                {
                    m_DownloadedLength = 0;
                    m_ElapseSeconds = 0f;
                }
            }
        }
    }
}
