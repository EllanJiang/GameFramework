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
        /// <summary>
        /// 资源信息。
        /// </summary>
        private sealed class ResourceInfo
        {
            private readonly ResourceName m_ResourceName;
            private readonly string m_FileSystemName;
            private readonly LoadType m_LoadType;
            private readonly int m_Length;
            private readonly int m_HashCode;
            private readonly int m_CompressedLength;
            private readonly bool m_StorageInReadOnly;
            private bool m_Ready;

            /// <summary>
            /// 初始化资源信息的新实例。
            /// </summary>
            /// <param name="resourceName">资源名称。</param>
            /// <param name="fileSystemName">文件系统名称。</param>
            /// <param name="loadType">资源加载方式。</param>
            /// <param name="length">资源大小。</param>
            /// <param name="hashCode">资源哈希值。</param>
            /// <param name="compressedLength">压缩后资源大小。</param>
            /// <param name="storageInReadOnly">资源是否在只读区。</param>
            /// <param name="ready">资源是否准备完毕。</param>
            public ResourceInfo(ResourceName resourceName, string fileSystemName, LoadType loadType, int length, int hashCode, int compressedLength, bool storageInReadOnly, bool ready)
            {
                m_ResourceName = resourceName;
                m_FileSystemName = fileSystemName;
                m_LoadType = loadType;
                m_Length = length;
                m_HashCode = hashCode;
                m_CompressedLength = compressedLength;
                m_StorageInReadOnly = storageInReadOnly;
                m_Ready = ready;
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
            /// 获取文件系统名称。
            /// </summary>
            public string FileSystemName
            {
                get
                {
                    return m_FileSystemName;
                }
            }

            /// <summary>
            /// 获取资源是否通过二进制方式加载。
            /// </summary>
            public bool IsLoadFromBinary
            {
                get
                {
                    return m_LoadType == LoadType.LoadFromBinary || m_LoadType == LoadType.LoadFromBinaryAndQuickDecrypt || m_LoadType == LoadType.LoadFromBinaryAndDecrypt;
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
            /// 获取压缩后资源大小。
            /// </summary>
            public int CompressedLength
            {
                get
                {
                    return m_CompressedLength;
                }
            }

            /// <summary>
            /// 获取资源是否在只读区。
            /// </summary>
            public bool StorageInReadOnly
            {
                get
                {
                    return m_StorageInReadOnly;
                }
            }

            /// <summary>
            /// 获取资源是否准备完毕。
            /// </summary>
            public bool Ready
            {
                get
                {
                    return m_Ready;
                }
            }

            /// <summary>
            /// 标记资源准备完毕。
            /// </summary>
            public void MarkReady()
            {
                m_Ready = true;
            }
        }
    }
}
