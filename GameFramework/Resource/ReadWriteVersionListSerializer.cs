//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Resource
{
    /// <summary>
    /// 本地读写区版本资源列表序列化器。
    /// </summary>
    public sealed class ReadWriteVersionListSerializer : GameFrameworkSerializer<LocalVersionList>
    {
        private static readonly byte[] Header = new byte[] { (byte)'G', (byte)'F', (byte)'W' };

        /// <summary>
        /// 初始化本地读写区版本资源列表序列化器的新实例。
        /// </summary>
        public ReadWriteVersionListSerializer()
        {
        }

        /// <summary>
        /// 获取本地读写区版本资源列表头标识。
        /// </summary>
        /// <returns>本地读写区版本资源列表头标识。</returns>
        protected override byte[] GetHeader()
        {
            return Header;
        }
    }
}
