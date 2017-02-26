//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.Event;
using GameFramework.Network;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 网络错误事件。
    /// </summary>
    public sealed class NetworkErrorEventArgs : GameEventArgs
    {
        /// <summary>
        /// 初始化网络错误事件的新实例。
        /// </summary>
        /// <param name="e">内部事件。</param>
        public NetworkErrorEventArgs(GameFramework.Network.NetworkErrorEventArgs e)
        {
            NetworkChannel = e.NetworkChannel;
            ErrorCode = e.ErrorCode;
            ErrorMessage = e.ErrorMessage;
        }

        /// <summary>
        /// 获取连接错误事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return (int)EventId.NetworkError;
            }
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
