//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Resource
{
    /// <summary>
    /// 资源包版本资源列表。
    /// </summary>
    public partial struct ResourcePackVersionList
    {
        private static readonly Resource[] EmptyResourceArray = new Resource[] { };

        private readonly bool m_IsValid;
        private readonly int m_Offset;
        private readonly long m_Length;
        private readonly int m_HashCode;
        private readonly Resource[] m_Resources;

        /// <summary>
        /// 初始化资源包版本资源列表的新实例。
        /// </summary>
        /// <param name="offset">资源数据偏移。</param>
        /// <param name="length">资源数据长度。</param>
        /// <param name="hashCode">资源数据哈希值。</param>
        /// <param name="resources">包含的资源集合。</param>
        public ResourcePackVersionList(int offset, long length, int hashCode, Resource[] resources)
        {
            m_IsValid = true;
            m_Offset = offset;
            m_Length = length;
            m_HashCode = hashCode;
            m_Resources = resources ?? EmptyResourceArray;
        }

        /// <summary>
        /// 获取资源包版本资源列表是否有效。
        /// </summary>
        public bool IsValid
        {
            get
            {
                return m_IsValid;
            }
        }

        /// <summary>
        /// 获取资源数据偏移。
        /// </summary>
        public int Offset
        {
            get
            {
                return m_Offset;
            }
        }

        /// <summary>
        /// 获取资源数据长度。
        /// </summary>
        public long Length
        {
            get
            {
                return m_Length;
            }
        }

        /// <summary>
        /// 获取资源数据哈希值。
        /// </summary>
        public int HashCode
        {
            get
            {
                return m_HashCode;
            }
        }

        /// <summary>
        /// 获取资源集合。
        /// </summary>
        /// <returns></returns>
        public Resource[] GetResources()
        {
            return m_Resources;
        }
    }
}
