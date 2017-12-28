//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
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
                private const int DefaultBufferLength = 1024 * 8;
                private readonly MemoryStream m_Stream;
                private IPacketHeader m_PacketHeader;

                public ReceiveState()
                {
                    m_Stream = new MemoryStream(DefaultBufferLength);
                    m_PacketHeader = null;
                }

                public MemoryStream Stream
                {
                    get
                    {
                        return m_Stream;
                    }
                }

                public IPacketHeader PacketHeader
                {
                    get
                    {
                        return m_PacketHeader;
                    }
                }

                public void PrepareForPacketHeader(int packetHeaderLength)
                {
                    Reset(packetHeaderLength, null);
                }

                public void PrepareForPacket(IPacketHeader packetHeader)
                {
                    if (packetHeader == null)
                    {
                        throw new GameFrameworkException("Packet header is invalid.");
                    }

                    Reset(packetHeader.PacketLength, packetHeader);
                }

                private void Reset(int targetLength, IPacketHeader packetHeader)
                {
                    if (targetLength < 0)
                    {
                        throw new GameFrameworkException("Target length is invalid.");
                    }

                    m_Stream.Position = 0L;
                    m_Stream.SetLength(targetLength);
                    m_PacketHeader = packetHeader;
                }
            }
        }
    }
}
