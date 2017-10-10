//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System.IO;

namespace GameFramework.Network
{
    internal partial class NetworkManager
    {
        private partial class NetworkChannel
        {
            private sealed class ReceiveState
            {
                private const int DefaultBufferLength = 1024 * 64;
                private readonly MemoryStream m_Stream;
                private bool m_IsPacket;

                public ReceiveState()
                {
                    m_Stream = new MemoryStream(DefaultBufferLength);
                    m_IsPacket = false;
                }

                public MemoryStream Stream
                {
                    get
                    {
                        return m_Stream;
                    }
                }

                public bool IsPacket
                {
                    get
                    {
                        return m_IsPacket;
                    }
                }

                public void PrepareForPacketHeader(int packetHeaderLength)
                {
                    Reset(packetHeaderLength, false);
                }

                public void PrepareForPacket(int packetLength)
                {
                    Reset(packetLength, true);
                }

                private void Reset(int targetLength, bool isPacket)
                {
                    if (targetLength < 0)
                    {
                        throw new GameFrameworkException("Target length is invalid.");
                    }

                    m_Stream.Position = 0L;
                    m_Stream.SetLength(targetLength);
                    m_IsPacket = isPacket;
                }
            }
        }
    }
}
