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
                private RemoteVersionInfo m_VersionInfo;
                private LocalVersionInfo m_ReadOnlyInfo;
                private LocalVersionInfo m_ReadWriteInfo;

                /// <summary>
                /// 初始化资源检查信息的新实例。
                /// </summary>
                /// <param name="resourceName">资源名称。</param>
                public CheckInfo(ResourceName resourceName)
                {
                    m_ResourceName = resourceName;
                    m_Status = CheckStatus.Unknown;
                    m_NeedRemove = false;
                    m_VersionInfo = default(RemoteVersionInfo);
                    m_ReadOnlyInfo = default(LocalVersionInfo);
                    m_ReadWriteInfo = default(LocalVersionInfo);
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
                public int ZipLength
                {
                    get
                    {
                        return m_VersionInfo.ZipLength;
                    }
                }

                /// <summary>
                /// 获取压缩后哈希值。
                /// </summary>
                public int ZipHashCode
                {
                    get
                    {
                        return m_VersionInfo.ZipHashCode;
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
                /// 获取资源是否可以从读写区移除。
                /// </summary>
                public bool NeedRemove
                {
                    get
                    {
                        return m_NeedRemove;
                    }
                }

                /// <summary>
                /// 设置资源在版本中的信息。
                /// </summary>
                /// <param name="loadType">资源加载方式。</param>
                /// <param name="length">资源大小。</param>
                /// <param name="hashCode">资源哈希值。</param>
                /// <param name="zipLength">压缩后大小。</param>
                /// <param name="zipHashCode">压缩后哈希值。</param>
                public void SetVersionInfo(LoadType loadType, int length, int hashCode, int zipLength, int zipHashCode)
                {
                    if (m_VersionInfo.Exist)
                    {
                        throw new GameFrameworkException(Utility.Text.Format("You must set version info of '{0}' only once.", m_ResourceName.FullName));
                    }

                    m_VersionInfo = new RemoteVersionInfo(loadType, length, hashCode, zipLength, zipHashCode);
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
                        throw new GameFrameworkException(Utility.Text.Format("You must set readonly info of '{0}' only once.", m_ResourceName.FullName));
                    }

                    m_ReadOnlyInfo = new LocalVersionInfo(loadType, length, hashCode);
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

                    m_ReadWriteInfo = new LocalVersionInfo(loadType, length, hashCode);
                }

                /// <summary>
                /// 刷新资源信息状态。
                /// </summary>
                /// <param name="currentVariant">当前变体。</param>
                public void RefreshStatus(string currentVariant)
                {
                    if (!m_VersionInfo.Exist)
                    {
                        m_Status = CheckStatus.Disuse;
                        m_NeedRemove = m_ReadWriteInfo.Exist;
                        return;
                    }

                    if (m_ResourceName.Variant == null || m_ResourceName.Variant == currentVariant)
                    {
                        if (m_ReadOnlyInfo.Exist && m_ReadOnlyInfo.LoadType == m_VersionInfo.LoadType && m_ReadOnlyInfo.Length == m_VersionInfo.Length && m_ReadOnlyInfo.HashCode == m_VersionInfo.HashCode)
                        {
                            m_Status = CheckStatus.StorageInReadOnly;
                            m_NeedRemove = m_ReadWriteInfo.Exist;
                        }
                        else if (m_ReadWriteInfo.Exist && m_ReadWriteInfo.LoadType == m_VersionInfo.LoadType && m_ReadWriteInfo.Length == m_VersionInfo.Length && m_ReadWriteInfo.HashCode == m_VersionInfo.HashCode)
                        {
                            m_Status = CheckStatus.StorageInReadWrite;
                            m_NeedRemove = false;
                        }
                        else
                        {
                            m_Status = CheckStatus.NeedUpdate;
                            m_NeedRemove = m_ReadWriteInfo.Exist;
                        }
                    }
                    else
                    {
                        m_Status = CheckStatus.Unavailable;
                        if (m_ReadOnlyInfo.Exist && m_ReadOnlyInfo.LoadType == m_VersionInfo.LoadType && m_ReadOnlyInfo.Length == m_VersionInfo.Length && m_ReadOnlyInfo.HashCode == m_VersionInfo.HashCode)
                        {
                            m_NeedRemove = m_ReadWriteInfo.Exist;
                        }
                        else if (m_ReadWriteInfo.Exist && m_ReadWriteInfo.LoadType == m_VersionInfo.LoadType && m_ReadWriteInfo.Length == m_VersionInfo.Length && m_ReadWriteInfo.HashCode == m_VersionInfo.HashCode)
                        {
                            m_NeedRemove = false;
                        }
                        else
                        {
                            m_NeedRemove = m_ReadWriteInfo.Exist;
                        }
                    }
                }
            }
        }
    }
}
