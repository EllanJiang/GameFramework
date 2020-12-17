//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System.Runtime.InteropServices;

namespace GameFramework.FileSystem
{
    /// <summary>
    /// 文件信息。
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct FileInfo
    {
        private readonly string m_Name;
        private readonly long m_Offset;
        private readonly int m_Length;

        /// <summary>
        /// 初始化文件信息的新实例。
        /// </summary>
        /// <param name="name">文件名称。</param>
        /// <param name="offset">文件偏移。</param>
        /// <param name="length">文件长度。</param>
        public FileInfo(string name, long offset, int length)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new GameFrameworkException("Name is invalid.");
            }

            if (offset < 0L)
            {
                throw new GameFrameworkException("Offset is invalid.");
            }

            if (length < 0)
            {
                throw new GameFrameworkException("Length is invalid.");
            }

            m_Name = name;
            m_Offset = offset;
            m_Length = length;
        }

        /// <summary>
        /// 获取文件信息是否有效。
        /// </summary>
        public bool IsValid
        {
            get
            {
                return !string.IsNullOrEmpty(m_Name) && m_Offset >= 0L && m_Length >= 0;
            }
        }

        /// <summary>
        /// 获取文件名称。
        /// </summary>
        public string Name
        {
            get
            {
                return m_Name;
            }
        }

        /// <summary>
        /// 获取文件偏移。
        /// </summary>
        public long Offset
        {
            get
            {
                return m_Offset;
            }
        }

        /// <summary>
        /// 获取文件长度。
        /// </summary>
        public int Length
        {
            get
            {
                return m_Length;
            }
        }
    }
}
