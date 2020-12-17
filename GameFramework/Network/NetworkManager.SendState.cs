//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;
using System.IO;

namespace GameFramework.Network
{
    internal sealed partial class NetworkManager : GameFrameworkModule, INetworkManager
    {
        private sealed class SendState : IDisposable
        {
            private const int DefaultBufferLength = 1024 * 64;
            private MemoryStream m_Stream;
            private bool m_Disposed;

            public SendState()
            {
                m_Stream = new MemoryStream(DefaultBufferLength);
                m_Disposed = false;
            }

            public MemoryStream Stream
            {
                get
                {
                    return m_Stream;
                }
            }

            public void Reset()
            {
                m_Stream.Position = 0L;
                m_Stream.SetLength(0L);
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            private void Dispose(bool disposing)
            {
                if (m_Disposed)
                {
                    return;
                }

                if (disposing)
                {
                    if (m_Stream != null)
                    {
                        m_Stream.Dispose();
                        m_Stream = null;
                    }
                }

                m_Disposed = true;
            }
        }
    }
}
