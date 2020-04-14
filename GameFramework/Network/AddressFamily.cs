//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Network
{
    /// <summary>
    /// 网络地址类型。
    /// </summary>
    public enum AddressFamily : byte
    {
        /// <summary>
        /// 未知。
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// IP 版本 4。
        /// </summary>
        IPv4,

        /// <summary>
        /// IP 版本 6。
        /// </summary>
        IPv6
    }
}
