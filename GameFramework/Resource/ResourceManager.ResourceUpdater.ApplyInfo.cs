//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Resource
{
    internal sealed partial class ResourceManager : GameFrameworkModule, IResourceManager
    {
        private sealed partial class ResourceUpdater
        {
            /// <summary>
            /// 应用信息。
            /// </summary>
            private sealed class ApplyInfo
            {
                private readonly ResourceName m_ResourceName;
                private readonly LoadType m_LoadType;
                private readonly long m_Offset;
                private readonly int m_Length;
                private readonly int m_HashCode;
                private readonly int m_ZipLength;
                private readonly int m_ZipHashCode;
                private readonly string m_ResourcePath;

                /// <summary>
                /// 初始化应用信息的新实例。
                /// </summary>
                /// <param name="resourceName">资源名称。</param>
                /// <param name="loadType">资源加载方式。</param>
                /// <param name="offset">资源偏移。</param>
                /// <param name="length">资源大小。</param>
                /// <param name="hashCode">资源哈希值。</param>
                /// <param name="zipLength">压缩后大小。</param>
                /// <param name="zipHashCode">压缩后哈希值。</param>
                /// <param name="resourcePath">资源路径。</param>
                public ApplyInfo(ResourceName resourceName, LoadType loadType, long offset, int length, int hashCode, int zipLength, int zipHashCode, string resourcePath)
                {
                    m_ResourceName = resourceName;
                    m_LoadType = loadType;
                    m_Offset = offset;
                    m_Length = length;
                    m_HashCode = hashCode;
                    m_ZipLength = zipLength;
                    m_ZipHashCode = zipHashCode;
                    m_ResourcePath = resourcePath;
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
                /// 获取资源加载方式。
                /// </summary>
                public LoadType LoadType
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
                /// 获取压缩后大小。
                /// </summary>
                public int ZipLength
                {
                    get
                    {
                        return m_ZipLength;
                    }
                }

                /// <summary>
                /// 获取压缩后哈希值。
                /// </summary>
                public int ZipHashCode
                {
                    get
                    {
                        return m_ZipHashCode;
                    }
                }

                /// <summary>
                /// 获取资源路径。
                /// </summary>
                public string ResourcePath
                {
                    get
                    {
                        return m_ResourcePath;
                    }
                }
            }
        }
    }
}
