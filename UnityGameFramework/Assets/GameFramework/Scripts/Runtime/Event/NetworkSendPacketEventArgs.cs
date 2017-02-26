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
    /// 发送网络消息包事件。
    /// </summary>
    public sealed class NetworkSendPacketEventArgs : GameEventArgs
    {
        /// <summary>
        /// 初始化发送网络消息包事件的新实例。
        /// </summary>
        /// <param name="e">内部事件。</param>
        public NetworkSendPacketEventArgs(GameFramework.Network.NetworkSendPacketEventArgs e)
        {
            NetworkChannel = e.NetworkChannel;
            BytesSent = e.BytesSent;
            UserData = e.UserData;
        }

        /// <summary>
        /// 获取发送网络消息包事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return (int)EventId.NetworkSendPacket;
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
