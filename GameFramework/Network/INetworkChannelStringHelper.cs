namespace GameFramework.Network
{
    /// <summary>
    /// 网络频道辅助器走字符串方式的接口。往往给http或者websocket等自定义频道使用
    /// </summary>
    public interface INetworkChannelStringHelper : INetworkChannelHelper
    {
        /// <summary>
        /// 序列化消息包。
        /// </summary>
        /// <typeparam name="T">消息包类型。</typeparam>
        /// <param name="packet">要序列化的消息包。</param>
        /// <param name="destination">要序列化的字符串。</param>
        /// <returns>是否序列化成功。</returns>
        bool Serialize<T>(T packet, out string destination) where T : Packet;

        /// <summary>
        /// 反序列化消息包头。 往往是不会调用这个方法，看自定义频道如何定义
        /// </summary>
        /// <param name="source">要反序列化的来源字符串。</param>
        /// <param name="customErrorData">用户自定义错误数据。</param>
        /// <returns>反序列化后的消息包头。</returns>
        IPacketHeader DeserializePacketHeader(string source, out object customErrorData);

        /// <summary>
        /// 反序列化消息包。
        /// </summary>
        /// <param name="packetHeader">消息包头。</param>
        /// <param name="source">要反序列化的来源字符串。</param>
        /// <param name="customErrorData">用户自定义错误数据。</param>
        /// <returns>反序列化后的消息包。</returns>
        Packet DeserializePacket(IPacketHeader packetHeader, string source, out object customErrorData);
    }
}