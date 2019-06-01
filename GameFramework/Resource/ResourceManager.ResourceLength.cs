//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Resource
{
    internal sealed partial class ResourceManager : GameFrameworkModule, IResourceManager
    {
        /// <summary>
        /// 资源大小数据。
        /// </summary>
        private struct ResourceLength
        {
            private readonly ResourceName m_ResourceName;
            private readonly int m_Length;
            private readonly int m_ZipLength;

            /// <summary>
            /// 初始化资源大小数据的新实例。
            /// </summary>
            /// <param name="resourceName">资源名称。</param>
            /// <param name="length">资源大小。</param>
            /// <param name="zipLength">资源压缩后的大小。</param>
            public ResourceLength(ResourceName resourceName, int length, int zipLength)
            {
                if (resourceName == null)
                {
                    throw new GameFrameworkException("Resource name is invalid.");
                }

                m_ResourceName = resourceName;
                m_Length = length;
                m_ZipLength = zipLength;
            }

            /// <summary>
            /// 获取资源名称。
            /// </summary>
            public ResourceName ResourceName
            {
                get
                {
                    return m_ResourceName;
                }
            }

            /// <summary>
            /// 获取资源大小。
            /// </summary>
            public int Length
            {
                get
                {
                    return m_Length;
                }
            }

            /// <summary>
            /// 获取资源压缩后的大小。
            /// </summary>
            public int ZipLength
            {
                get
                {
                    return m_ZipLength;
                }
            }
        }
    }
}
