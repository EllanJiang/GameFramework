//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Network
{
    /// <summary>
    /// 网络心跳包丢失事件。
    /// </summary>
    public sealed class NetworkMissHeartBeatEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化网络心跳包丢失事件的新实例。
        /// </summary>
        /// <param name="networkChannel">网络频道。</param>
        /// <param name="missCount">心跳包已丢失次数。</param>
        public NetworkMissHeartBeatEventArgs(INetworkChannel networkChannel, int missCount)
        {
            NetworkChannel = networkChannel;
            MissCount = missCount;
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
