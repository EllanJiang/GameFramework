//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Resource
{
    /// <summary>
    /// 资源包版本资源列表序列化器。
    /// </summary>
    public sealed class ResourcePackVersionListSerializer : GameFrameworkSerializer<ResourcePackVersionList>
    {
        private static readonly byte[] Header = new byte[] { (byte)'G', (byte)'F', (byte)'K' };

        /// <summary>
        /// 初始化资源包版本资源列表序列化器的新实例。
        /// </summary>
        public ResourcePackVersionListSerializer()
        {
        }

        /// <summary>
        /// 获取资源包版本资源列表头标识。
        /// </summary>
        /// <returns>资源包版本资源列表头标识。</returns>
        protected override byte[] GetHeader()
        {
            return Header;
        }
    }
}
