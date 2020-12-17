//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System.Runtime.InteropServices;

namespace GameFramework.FileSystem
{
    internal sealed partial class FileSystem : IFileSystem
    {
        /// <summary>
        /// 块数据。
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct BlockData
        {
            public static readonly BlockData Empty = new BlockData(0, 0);

            private readonly int m_StringIndex;
            private readonly int m_ClusterIndex;
            private readonly int m_Length;

            public BlockData(int clusterIndex, int length)
                : this(-1, clusterIndex, length)
            {
            }

            public BlockData(int stringIndex, int clusterIndex, int length)
            {
                m_StringIndex = stringIndex;
                m_ClusterIndex = clusterIndex;
                m_Length = length;
            }

            public bool Using
            {
                get
                {
                    return m_StringIndex >= 0;
                }
            }

            public int StringIndex
            {
                get
                {
                    return m_StringIndex;
                }
            }

            public int ClusterIndex
            {
                get
                {
                    return m_ClusterIndex;
                }
            }

            public int Length
            {
                get
                {
                    return m_Length;
                }
            }

            public BlockData Free()
            {
                return new BlockData(m_ClusterIndex, (int)GetUpBoundClusterOffset(m_Length));
            }
        }
    }
}
