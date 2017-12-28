//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

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
        /// <param name="errorMessage">错误信息。</param>
        public NetworkErrorEventArgs(INetworkChannel networkChannel, NetworkErrorCode errorCode, string errorMessage)
        {
            NetworkChannel = networkChannel;
            ErrorCode = errorCode;
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
        /// 获取错误信息。
        /// </summary>
        public string ErrorMessage
        {
            get;
            private set;
        }
    }
}
