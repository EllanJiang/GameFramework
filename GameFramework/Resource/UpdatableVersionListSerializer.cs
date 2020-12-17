//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Resource
{
    /// <summary>
    /// 可更新模式版本资源列表序列化器。
    /// </summary>
    public sealed class UpdatableVersionListSerializer : GameFrameworkSerializer<UpdatableVersionList>
    {
        private static readonly byte[] Header = new byte[] { (byte)'G', (byte)'F', (byte)'U' };

        /// <summary>
        /// 初始化可更新模式版本资源列表序列化器的新实例。
        /// </summary>
        public UpdatableVersionListSerializer()
        {
        }

        /// <summary>
        /// 获取可更新模式版本资源列表头标识。
        /// </summary>
        /// <returns>可更新模式版本资源列表头标识。</returns>
        protected override byte[] GetHeader()
        {
            return Header;
        }
    }
}
