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
        private sealed partial class ResourceChecker
        {
            /// <summary>
            /// 资源检查信息。
            /// </summary>
            private sealed partial class CheckInfo
            {
                private readonly ResourceName m_ResourceName;
                private CheckStatus m_Status;
                private bool m_NeedRemove;
                private bool m_NeedMoveToDisk;
                private bool m_NeedMoveToFileSystem;
                private RemoteVersionInfo m_VersionInfo;
                private LocalVersionInfo m_ReadOnlyInfo;
                private LocalVersionInfo m_ReadWriteInfo;
                private string m_CachedFileSystemName;

                /// <summary>
                /// 初始化资源检查信息的新实例。
                /// </summary>
                /// <param name="resourceName">资源名称。</param>
                public CheckInfo(ResourceName resourceName)
                {
                    m_ResourceName = resourceName;
                    m_Status = CheckStatus.Unknown;
                    m_NeedRemove = false;
                    m_NeedMoveToDisk = false;
                    m_NeedMoveToFileSystem = false;
                    m_VersionInfo = default(RemoteVersionInfo);
                    m_ReadOnlyInfo = default(LocalVersionInfo);
                    m_ReadWriteInfo = default(LocalVersionInfo);
                    m_CachedFileSystemName = null;
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
                /// 获取资源检查状态。
                /// </summary>
                public CheckStatus Status
                {
                    get
                    {
                        return m_Status;
                    }
                }

                /// <summary>
                /// 获取是否需要移除读写区的资源。
                /// </summary>
                public bool NeedRemove
                {
                    get
                    {
                        return m_NeedRemove;
                    }
                }

                /// <summary>
                /// 获取是否需要将读写区的资源移动到磁盘。
                /// </summary>
                public bool NeedMoveToDisk
                {
                    get
                    {
                        return m_NeedMoveToDisk;
                    }
                }

                /// <summary>
                /// 获取是否需要将读写区的资源移动到文件系统。
                /// </summary>
                public bool NeedMoveToFileSystem
                {
                    get
                    {
                        return m_NeedMoveToFileSystem;
                    }
                }

                /// <summary>
                /// 获取资源所在的文件系统名称。
                /// </summary>
                public string FileSystemName
                {
                    get
                    {
                        return m_VersionInfo.FileSystemName;
                    }
                }

                /// <summary>
                /// 获取资源是否使用文件系统。
                /// </summary>
                public bool ReadWriteUseFileSystem
                {
                    get
                    {
                        return m_ReadWriteInfo.UseFileSystem;
                    }
                }

                /// <summary>
                /// 获取读写资源所在的文件系统名称。
                /// </summary>
                public string ReadWriteFileSystemName
                {
                    get
                    {
                        return m_ReadWriteInfo.FileSystemName;
                    }
                }

                /// <summary>
                /// 获取资源加载方式。
                /// </summary>
                public LoadType LoadType
                {
                    get
                    {
                        return m_VersionInfo.LoadType;
                    }
                }

                /// <summary>
                /// 获取资源大小。
                /// </summary>
                public int Length
                {
                    get
                    {
                        return m_VersionInfo.Length;
                    }
                }

                /// <summary>
                /// 获取资源哈希值。
                /// </summary>
                public int HashCode
                {
                    get
                    {
                        return m_VersionInfo.HashCode;
                    }
                }

                /// <summary>
                /// 获取压缩后大小。
                /// </summary>
                public int CompressedLength
                {
                    get
                    {
                        return m_VersionInfo.CompressedLength;
                    }
                }

                /// <summary>
                /// 获取压缩后哈希值。
                /// </summary>
                public int CompressedHashCode
                {
                    get
                    {
                        return m_VersionInfo.CompressedHashCode;
                    }
                }

                /// <summary>
                /// 临时缓存资源所在的文件系统名称。
                /// </summary>
                /// <param name="fileSystemName">资源所在的文件系统名称。</param>
                public void SetCachedFileSystemName(string fileSystemName)
                {
                    m_CachedFileSystemName = fileSystemName;
                }

                /// <summary>
                /// 设置资源在版本中的信息。
                /// </summary>
                /// <param name="loadType">资源加载方式。</param>
                /// <param name="length">资源大小。</param>
                /// <param name="hashCode">资源哈希值。</param>
                /// <param name="compressedLength">压缩后大小。</param>
                /// <param name="compressedHashCode">压缩后哈希值。</param>
                public void SetVersionInfo(LoadType loadType, int length, int hashCode, int compressedLength, int compressedHashCode)
                {
                    if (m_VersionInfo.Exist)
                    {
                        throw new GameFrameworkException(Utility.Text.Format("You must set version info of '{0}' only once.", m_ResourceName.FullName));
                    }

                    m_VersionInfo = new RemoteVersionInfo(m_CachedFileSystemName, loadType, length, hashCode, compressedLength, compressedHashCode);
                    m_CachedFileSystemName = null;
                }

                /// <summary>
                /// 设置资源在只读区中的信息。
                /// </summary>
                /// <param name="loadType">资源加载方式。</param>
                /// <param name="length">资源大小。</param>
                /// <param name="hashCode">资源哈希值。</param>
                public void SetReadOnlyInfo(LoadType loadType, int length, int hashCode)
                {
                    if (m_ReadOnlyInfo.Exist)
                    {
                        throw new GameFrameworkException(Utility.Text.Format("You must set read-only info of '{0}' only once.", m_ResourceName.FullName));
                    }

                    m_ReadOnlyInfo = new LocalVersionInfo(m_CachedFileSystemName, loadType, length, hashCode);
                    m_CachedFileSystemName = null;
                }

                /// <summary>
                /// 设置资源在读写区中的信息。
                /// </summary>
                /// <param name="loadType">资源加载方式。</param>
                /// <param name="length">资源大小。</param>
                /// <param name="hashCode">资源哈希值。</param>
                public void SetReadWriteInfo(LoadType loadType, int length, int hashCode)
                {
                    if (m_ReadWriteInfo.Exist)
                    {
                        throw new GameFrameworkException(Utility.Text.Format("You must set read-write info of '{0}' only once.", m_ResourceName.FullName));
                    }

                    m_ReadWriteInfo = new LocalVersionInfo(m_CachedFileSystemName, loadType, length, hashCode);
                    m_CachedFileSystemName = null;
                }

                /// <summary>
                /// 刷新资源信息状态。
                /// </summary>
                /// <param name="currentVariant">当前变体。</param>
                /// <param name="ignoreOtherVariant">是否忽略处理其它变体的资源，若不忽略则移除。</param>
                public void RefreshStatus(string currentVariant, bool ignoreOtherVariant)
                {
                    if (!m_VersionInfo.Exist)
                    {
                        m_Status = CheckStatus.Disuse;
                        m_NeedRemove = m_ReadWriteInfo.Exist;
                        return;
                    }

                    if (m_ResourceName.Variant == null || m_ResourceName.Variant == currentVariant)
                    {
                        if (m_ReadOnlyInfo.Exist && m_ReadOnlyInfo.FileSystemName == m_VersionInfo.FileSystemName && m_ReadOnlyInfo.LoadType == m_VersionInfo.LoadType && m_ReadOnlyInfo.Length == m_VersionInfo.Length && m_ReadOnlyInfo.HashCode == m_VersionInfo.HashCode)
                        {
                            m_Status = CheckStatus.StorageInReadOnly;
                            m_NeedRemove = m_ReadWriteInfo.Exist;
                        }
                        else if (m_ReadWriteInfo.Exist && m_ReadWriteInfo.LoadType == m_VersionInfo.LoadType && m_ReadWriteInfo.Length == m_VersionInfo.Length && m_ReadWriteInfo.HashCode == m_VersionInfo.HashCode)
                        {
                            bool differentFileSystem = m_ReadWriteInfo.FileSystemName != m_VersionInfo.FileSystemName;
                            m_Status = CheckStatus.StorageInReadWrite;
                            m_NeedMoveToDisk = m_ReadWriteInfo.UseFileSystem && differentFileSystem;
                            m_NeedMoveToFileSystem = m_VersionInfo.UseFileSystem && differentFileSystem;
                        }
                        else
                        {
                            m_Status = CheckStatus.Update;
                            m_NeedRemove = m_ReadWriteInfo.Exist;
                        }
                    }
                    else
                    {
                        m_Status = CheckStatus.Unavailable;
                        m_NeedRemove = !ignoreOtherVariant && m_ReadWriteInfo.Exist;
                    }
                }
            }
        }
    }
}
