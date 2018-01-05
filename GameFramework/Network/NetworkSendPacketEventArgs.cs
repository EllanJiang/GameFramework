//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Network
{
    /// <summary>
    /// 发送网络消息包事件。
    /// </summary>
    public sealed class NetworkSendPacketEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化发送网络消息包事件的新实例。
        /// </summary>
        /// <param name="networkChannel">网络频道。</param>
        /// <param name="bytesSent">已发送字节数。</param>
        /// <param name="userData">用户自定义数据。</param>
        public NetworkSendPacketEventArgs(INetworkChannel networkChannel, int bytesSent, object userData)
        {
            NetworkChannel = networkChannel;
            BytesSent = bytesSent;
            UserData = userData;
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
        /// 获取已发送字节数。
        /// </summary>
        public int BytesSent
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取用户自定义数据。
        /// </summary>
        public object UserData
        {
            get;
            private set;
        }
    }
}
