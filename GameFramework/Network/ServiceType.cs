//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Network
{
    /// <summary>
    /// 网络服务类型。
    /// </summary>
    public enum ServiceType : byte
    {
        /// <summary>
        /// TCP 网络服务。
        /// </summary>
        Tcp = 0,

        /// <summary>
        /// 使用同步接收的 TCP 网络服务。
        /// </summary>
        TcpWithSyncReceive
    }
}
