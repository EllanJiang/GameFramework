//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Network
{
    internal partial class NetworkManager
    {
        private partial class NetworkChannel
        {
            private sealed class SendState
            {
                private byte[] m_PacketBytes;
                private int m_Offset;
                private int m_Length;

                public SendState()
                {
                    m_PacketBytes = null;
                    m_Offset = 0;
                    m_Length = 0;
                }

                public bool IsFree
                {
                    get
                    {
                        return m_PacketBytes == null && m_Offset == 0 && m_Length == 0;
                    }
                }

                public int Offset
                {
                    get
                    {
                        return m_Offset;
                    }
                    set
                    {
                        m_Offset = value;
                    }
                }

                public int Length
                {
                    get
                    {
                        return m_Length;
                    }
                }

                public byte[] GetPacketBytes()
                {
                    return m_PacketBytes;
                }

                public void SetPacket(byte[] packetBytes)
                {
                    m_PacketBytes = packetBytes;
                    m_Offset = 0;
                    m_Length = packetBytes.Length;
                }

                public void Reset()
                {
                    m_PacketBytes = null;
                    m_Offset = 0;
                    m_Length = 0;
                }
            }
        }
    }
}
