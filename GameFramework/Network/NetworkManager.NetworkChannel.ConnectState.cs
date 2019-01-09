//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System;
using System.Net.Sockets;

namespace GameFramework.Network
{
    internal sealed partial class NetworkManager : GameFrameworkModule, INetworkManager
    {
        private sealed partial class NetworkChannel : INetworkChannel, IDisposable
        {
            private sealed class ConnectState
            {
                private readonly Socket m_Socket;
                private readonly object m_UserData;

                public ConnectState(Socket socket, object userData)
                {
                    m_Socket = socket;
                    m_UserData = userData;
                }

                public Socket Socket
                {
                    get
                    {
                        return m_Socket;
                    }
                }

                public object UserData
                {
                    get
                    {
                        return m_UserData;
                    }
                }
            }
        }
    }
}
