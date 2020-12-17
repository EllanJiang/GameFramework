//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Network
{
    /// <summary>
    /// 网络错误码。
    /// </summary>
    public enum NetworkErrorCode : byte
    {
        /// <summary>
        /// 未知错误。
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// 地址族错误。
        /// </summary>
        AddressFamilyError,

        /// <summary>
        /// Socket 错误。
        /// </summary>
        SocketError,

        /// <summary>
        /// 连接错误。
        /// </summary>
        ConnectError,

        /// <summary>
        /// 发送错误。
        /// </summary>
        SendError,

        /// <summary>
        /// 接收错误。
        /// </summary>
        ReceiveError,

        /// <summary>
        /// 序列化错误。
        /// </summary>
        SerializeError,

        /// <summary>
        /// 反序列化消息包头错误。
        /// </summary>
        DeserializePacketHeaderError,

        /// <summary>
        /// 反序列化消息包错误。
        /// </summary>
        DeserializePacketError
    }
}
