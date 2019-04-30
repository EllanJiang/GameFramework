//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System.Net.Sockets;

namespace GameFramework.Network
{
    /// <summary>
    /// 网络错误事件。
    /// </summary>
    public sealed class NetworkErrorEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化网络错误事件的新实例。
        /// </summary>
        /// <param name="networkChannel">网络频道。</param>
        /// <param name="errorCode">错误码。</param>
        /// <param name="socketErrorCode">Socket 错误码。</param>
        /// <param name="errorMessage">错误信息。</param>
        public NetworkErrorEventArgs(INetworkChannel networkChannel, NetworkErrorCode errorCode, SocketError socketErrorCode, string errorMessage)
        {
            NetworkChannel = networkChannel;
            ErrorCode = errorCode;
            SocketErrorCode = socketErrorCode;
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// 获取网络频道。
        /// </summary>
        public INetworkChannel NetworkChannel
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取错误码。
        /// </summary>
        public NetworkErrorCode ErrorCode
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取 Socket 错误码。
        /// </summary>
        public SocketError SocketErrorCode
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取错误信息。
        /// </summary>
        public string ErrorMessage
        {
            get;
            private set;
        }
    }
}
