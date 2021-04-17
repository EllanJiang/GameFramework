//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
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
            /// 资源更新信息。
            /// </summary>
            private sealed class UpdateInfo
            {
                private readonly ResourceName m_ResourceName;
                private readonly string m_FileSystemName;
                private readonly LoadType m_LoadType;
                private readonly int m_Length;
                private readonly int m_HashCode;
                private readonly int m_CompressedLength;
                private readonly int m_CompressedHashCode;
                private readonly string m_ResourcePath;
                private bool m_Downloading;
                private int m_RetryCount;

                /// <summary>
                /// 初始化资源更新信息的新实例。
                /// </summary>
                /// <param name="resourceName">资源名称。</param>
                /// <param name="fileSystemName">资源所在的文件系统名称。</param>
                /// <param name="loadType">资源加载方式。</param>
                /// <param name="length">资源大小。</param>
                /// <param name="hashCode">资源哈希值。</param>
                /// <param name="compressedLength">压缩后大小。</param>
                /// <param name="compressedHashCode">压缩后哈希值。</param>
                /// <param name="resourcePath">资源路径。</param>
                public UpdateInfo(ResourceName resourceName, string fileSystemName, LoadType loadType, int length, int hashCode, int compressedLength, int compressedHashCode, string resourcePath)
                {
                    m_ResourceName = resourceName;
                    m_FileSystemName = fileSystemName;
                    m_LoadType = loadType;
                    m_Length = length;
                    m_HashCode = hashCode;
                    m_CompressedLength = compressedLength;
                    m_CompressedHashCode = compressedHashCode;
                    m_ResourcePath = resourcePath;
                    m_Downloading = false;
                    m_RetryCount = 0;
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
                /// 获取资源是否使用文件系统。
                /// </summary>
                public bool UseFileSystem
                {
                    get
                    {
                        return !string.IsNullOrEmpty(m_FileSystemName);
                    }
                }

                /// <summary>
                /// 获取资源所在的文件系统名称。
                /// </summary>
                public string FileSystemName
                {
                    get
                    {
                        return m_FileSystemName;
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
                public int CompressedLength
                {
                    get
                    {
                        return m_CompressedLength;
                    }
                }

                /// <summary>
                /// 获取压缩后哈希值。
                /// </summary>
                public int CompressedHashCode
                {
                    get
                    {
                        return m_CompressedHashCode;
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

                /// <summary>
                /// 获取或设置下载状态。
                /// </summary>
                public bool Downloading
                {
                    get
                    {
                        return m_Downloading;
                    }
                    set
                    {
                        m_Downloading = value;
                    }
                }

                /// <summary>
                /// 获取或设置已重试次数。
                /// </summary>
                public int RetryCount
                {
                    get
                    {
                        return m_RetryCount;
                    }
                    set
                    {
                        m_RetryCount = value;
                    }
                }
            }
        }
    }
}
