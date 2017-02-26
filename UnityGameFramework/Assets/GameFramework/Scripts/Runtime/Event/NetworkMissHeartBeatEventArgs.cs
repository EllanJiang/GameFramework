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
    /// 网络心跳包丢失事件。
    /// </summary>
    public sealed class NetworkMissHeartBeatEventArgs : GameEventArgs
    {
        /// <summary>
        /// 初始化网络心跳包丢失事件的新实例。
        /// </summary>
        /// <param name="e">内部事件。</param>
        public NetworkMissHeartBeatEventArgs(GameFramework.Network.NetworkMissHeartBeatEventArgs e)
        {
            NetworkChannel = e.NetworkChannel;
            MissCount = e.MissCount;
        }

        /// <summary>
        /// 获取心跳包丢失事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return (int)EventId.NetworkMissHeartBeat;
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
        /// 获取心跳包已丢失次数。
        /// </summary>
        public int MissCount
        {
            get;
            private set;
        }
    }
}
