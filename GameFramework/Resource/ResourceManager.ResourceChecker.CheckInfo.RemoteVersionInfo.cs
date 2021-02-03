//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System.Runtime.InteropServices;

namespace GameFramework.Resource
{
    internal sealed partial class ResourceManager : GameFrameworkModule, IResourceManager
    {
        private sealed partial class ResourceChecker
        {
            private sealed partial class CheckInfo
            {
                /// <summary>
                /// 远程资源状态信息。
                /// </summary>
                [StructLayout(LayoutKind.Auto)]
                private struct RemoteVersionInfo
                {
                    private readonly bool m_Exist;
                    private readonly string m_FileSystemName;
                    private readonly LoadType m_LoadType;
                    private readonly int m_Length;
                    private readonly int m_HashCode;
                    private readonly int m_CompressedLength;
                    private readonly int m_CompressedHashCode;

                    public RemoteVersionInfo(string fileSystemName, LoadType loadType, int length, int hashCode, int compressedLength, int compressedHashCode)
                    {
                        m_Exist = true;
                        m_FileSystemName = fileSystemName;
                        m_LoadType = loadType;
                        m_Length = length;
                        m_HashCode = hashCode;
                        m_CompressedLength = compressedLength;
                        m_CompressedHashCode = compressedHashCode;
                    }

                    public bool Exist
                    {
                        get
                        {
                            return m_Exist;
                        }
                    }

                    public bool UseFileSystem
                    {
                        get
                        {
                            return !string.IsNullOrEmpty(m_FileSystemName);
                        }
                    }

                    public string FileSystemName
                    {
                        get
                        {
                            return m_FileSystemName;
                        }
                    }

                    public LoadType LoadType
                    {
                        get
                        {
                            return m_LoadType;
                        }
                    }

                    public int Length
                    {
                        get
                        {
                            return m_Length;
                        }
                    }

                    public int HashCode
                    {
                        get
                        {
                            return m_HashCode;
                        }
                    }

                    public int CompressedLength
                    {
                        get
                        {
                            return m_CompressedLength;
                        }
                    }

                    public int CompressedHashCode
                    {
                        get
                        {
                            return m_CompressedHashCode;
                        }
                    }
                }
            }
        }
    }
}
