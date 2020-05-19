//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Resource
{
    /// <summary>
    /// 本地只读区版本资源列表序列化器。
    /// </summary>
    public sealed class ReadOnlyVersionListSerializer : GameFrameworkSerializer<LocalVersionList>
    {
        private static readonly byte[] Header = new byte[] { (byte)'G', (byte)'F', (byte)'R' };

        /// <summary>
        /// 初始化本地只读区版本资源列表序列化器的新实例。
        /// </summary>
        public ReadOnlyVersionListSerializer()
        {
        }

        /// <summary>
        /// 获取本地只读区版本资源列表头标识。
        /// </summary>
        /// <returns>本地只读区版本资源列表头标识。</returns>
        protected override byte[] GetHeader()
        {
            return Header;
        }
    }
}
