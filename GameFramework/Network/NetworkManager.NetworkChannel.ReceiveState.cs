//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Network
{
    internal partial class NetworkManager
    {
        private partial class NetworkChannel
        {
            private sealed class ReceiveState
            {
                private readonly byte[] m_Buffer;
                private int m_Length;
                private int m_ReceivedLength;

                public ReceiveState(int bufferSize)
                {
                    if (bufferSize <= 0)
                    {
                        throw new GameFrameworkException("Buffer size is invalid.");
                    }

                    m_Buffer = new byte[bufferSize];
                }

                public int BufferSize
                {
                    get
                    {
                        return m_Buffer.Length;
                    }
                }

                public int Length
                {
                    get
                    {
                        return m_Length;
                    }
                    set
                    {
                        m_Length = value;
                    }
                }

                public int ReceivedLength
                {
                    get
                    {
                        return m_ReceivedLength;
                    }
                    set
                    {
                        m_ReceivedLength = value;
                    }
                }

                public void Reset(int packetHeaderLength)
                {
                    Length = packetHeaderLength;
                    ReceivedLength = 0;
                }

                public byte[] GetBuffer()
                {
                    return m_Buffer;
                }
            }
        }
    }
}
