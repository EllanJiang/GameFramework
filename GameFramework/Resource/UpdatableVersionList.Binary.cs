//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Resource
{
    public partial struct UpdatableVersionList
    {
        /// <summary>
        /// 二进制资源。
        /// </summary>
        public struct Binary
        {
            private readonly string m_Name;
            private readonly string m_Variant;
            private readonly int m_Length;
            private readonly int m_HashCode;
            private readonly int m_ZipLength;
            private readonly int m_ZipHashCode;

            /// <summary>
            /// 初始化二进制资源的新实例。
            /// </summary>
            /// <param name="name">二进制资源名称。</param>
            /// <param name="variant">二进制资源变体名称。</param>
            /// <param name="length">二进制资源长度。</param>
            /// <param name="hashCode">二进制资源哈希值。</param>
            /// <param name="zipLength">二进制资源压缩后长度。</param>
            /// <param name="zipHashCode">二进制资源压缩后哈希值。</param>
            public Binary(string name, string variant, int length, int hashCode, int zipLength, int zipHashCode)
            {
                if (name == null)
                {
                    throw new GameFrameworkException("Name is invalid.");
                }

                m_Name = name;
                m_Variant = variant;
                m_Length = length;
                m_HashCode = hashCode;
                m_ZipLength = zipLength;
                m_ZipHashCode = zipHashCode;
            }

            /// <summary>
            /// 获取二进制资源名称。
            /// </summary>
            public string Name
            {
                get
                {
                    return m_Name;
                }
            }

            /// <summary>
            /// 获取二进制资源变体名称。
            /// </summary>
            public string Variant
            {
                get
                {
                    return m_Variant;
                }
            }

            /// <summary>
            /// 获取二进制资源长度。
            /// </summary>
            public int Length
            {
                get
                {
                    return m_Length;
                }
            }

            /// <summary>
            /// 获取二进制资源哈希值。
            /// </summary>
            public int HashCode
            {
                get
                {
                    return m_HashCode;
                }
            }

            /// <summary>
            /// 获取二进制资源压缩后长度。
            /// </summary>
            public int ZipLength
            {
                get
                {
                    return m_ZipLength;
                }
            }

            /// <summary>
            /// 获取二进制资源压缩后哈希值。
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
