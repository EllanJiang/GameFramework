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
    /// 网络连接关闭事件。
    /// </summary>
    public sealed class NetworkClosedEventArgs : GameEventArgs
    {
        /// <summary>
        /// 初始化网络连接关闭事件的新实例。
        /// </summary>
        /// <param name="e">内部事件。</param>
        public NetworkClosedEventArgs(GameFramework.Network.NetworkClosedEventArgs e)
        {
            NetworkChannel = e.NetworkChannel;
        }

        /// <summary>
        /// 获取连接成功事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return (int)EventId.NetworkClosed;
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
    }
}
