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
        private sealed partial class ResourceVerifier
        {
            /// <summary>
            /// 资源校验信息。
            /// </summary>
            private struct VerifyInfo
            {
                private readonly ResourceName m_ResourceName;
                private readonly string m_FileSystemName;
                private readonly LoadType m_LoadType;
                private readonly int m_Length;
                private readonly int m_HashCode;

                /// <summary>
                /// 初始化资源校验信息的新实例。
                /// </summary>
                /// <param name="resourceName">资源名称。</param>
                /// <param name="fileSystemName">资源所在的文件系统名称。</param>
                /// <param name="loadType">资源加载方式。</param>
                /// <param name="length">资源大小。</param>
                /// <param name="hashCode">资源哈希值。</param>
                public VerifyInfo(ResourceName resourceName, string fileSystemName, LoadType loadType, int length, int hashCode)
                {
                    m_ResourceName = resourceName;
                    m_FileSystemName = fileSystemName;
                    m_LoadType = loadType;
                    m_Length = length;
                    m_HashCode = hashCode;
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
            }
        }
    }
}
