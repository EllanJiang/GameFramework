//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Resource
{
    public partial struct ResourcePackVersionList
    {
        /// <summary>
        /// 资源。
        /// </summary>
        public struct Resource
        {
            private readonly string m_Name;
            private readonly string m_Variant;
            private readonly string m_Extension;
            private readonly byte m_LoadType;
            private readonly long m_Offset;
            private readonly int m_Length;
            private readonly int m_HashCode;
            private readonly int m_ZipLength;
            private readonly int m_ZipHashCode;

            /// <summary>
            /// 初始化资源的新实例。
            /// </summary>
            /// <param name="name">资源名称。</param>
            /// <param name="variant">资源变体名称。</param>
            /// <param name="extension">资源扩展名称。</param>
            /// <param name="loadType">资源加载方式。</param>
            /// <param name="offset">资源偏移。</param>
            /// <param name="length">资源长度。</param>
            /// <param name="hashCode">资源哈希值。</param>
            /// <param name="zipLength">资源压缩后长度。</param>
            /// <param name="zipHashCode">资源压缩后哈希值。</param>
            public Resource(string name, string variant, string extension, byte loadType, long offset, int length, int hashCode, int zipLength, int zipHashCode)
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new GameFrameworkException("Name is invalid.");
                }

                m_Name = name;
                m_Variant = variant;
                m_Extension = extension;
                m_LoadType = loadType;
                m_Offset = offset;
                m_Length = length;
                m_HashCode = hashCode;
                m_ZipLength = zipLength;
                m_ZipHashCode = zipHashCode;
            }

            /// <summary>
            /// 获取资源名称。
            /// </summary>
            public string Name
            {
                get
                {
                    return m_Name;
                }
            }

            /// <summary>
            /// 获取资源变体名称。
            /// </summary>
            public string Variant
            {
                get
                {
                    return m_Variant;
                }
            }

            /// <summary>
            /// 获取资源扩展名称。
            /// </summary>
            public string Extension
            {
                get
                {
                    return m_Extension;
                }
            }

            /// <summary>
            /// 获取资源加载方式。
            /// </summary>
            public byte LoadType
            {
                get
                {
                    return m_LoadType;
                }
            }

            /// <summary>
            /// 获取资源偏移。
            /// </summary>
            public long Offset
            {
                get
                {
                    return m_Offset;
                }
            }

            /// <summary>
            /// 获取资源长度。
            /// </summary>
            public int Length
            {
                get
                {
                    return m_Length;
                }
            }

            /// <summary>
            /// 获取资源哈希值。
            /// </summary>
            public int HashCode
            {
                get
                {
                    return m_HashCode;
                }
            }

            /// <summary>
            /// 获取资源压缩后长度。
            /// </summary>
            public int ZipLength
            {
                get
                {
                    return m_ZipLength;
                }
            }

            /// <summary>
            /// 获取资源压缩后哈希值。
            /// </summary>
            public int ZipHashCode
            {
                get
                {
                    return m_ZipHashCode;
                }
            }
        }
    }
}
