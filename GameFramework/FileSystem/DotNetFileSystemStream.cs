//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;
using System.IO;

namespace GameFramework.FileSystem
{
    /// <summary>
    /// 使用 .net 库实现的文件系统流。
    /// </summary>
    public sealed class DotNetFileSystemStream : FileSystemStream, IDisposable
    {
        private const int CachedBytesLength = 0x1000;
        private static readonly byte[] s_CachedBytes = new byte[CachedBytesLength];

        private readonly FileStream m_FileStream;

        /// <summary>
        /// 初始化使用 .net 库实现的文件系统流的新实例。
        /// </summary>
        /// <param name="fullPath">要加载的文件系统的完整路径。</param>
        /// <param name="access">要加载的文件系统的访问方式。</param>
        /// <param name="createNew">是否创建新的文件系统流。</param>
        public DotNetFileSystemStream(string fullPath, FileSystemAccess access, bool createNew)
        {
            if (string.IsNullOrEmpty(fullPath))
            {
                throw new GameFrameworkException("Full path is invalid.");
            }

            switch (access)
            {
                case FileSystemAccess.Read:
                    m_FileStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                    break;

                case FileSystemAccess.Write:
                    m_FileStream = new FileStream(fullPath, createNew ? FileMode.Create : FileMode.Open, FileAccess.Write, FileShare.None);
                    break;

                case FileSystemAccess.ReadWrite:
                    m_FileStream = new FileStream(fullPath, createNew ? FileMode.Create : FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                    break;

                default:
                    throw new GameFrameworkException("Access is invalid.");
            }
        }

        /// <summary>
        /// 获取或设置文件系统流位置。
        /// </summary>
        protected internal override long Position
        {
            get
            {
                return m_FileStream.Position;
            }
            set
            {
                m_FileStream.Position = value;
            }
        }

        /// <summary>
        /// 获取文件系统流长度。
        /// </summary>
        protected internal override long Length
        {
            get
            {
                return m_FileStream.Length;
            }
        }

        /// <summary>
        /// 设置文件系统流长度。
        /// </summary>
        /// <param name="length">要设置的文件系统流的长度。</param>
        protected internal override void SetLength(long length)
        {
            m_FileStream.SetLength(length);
        }

        /// <summary>
        /// 定位文件系统流位置。
        /// </summary>
        /// <param name="offset">要定位的文件系统流位置的偏移。</param>
        /// <param name="origin">要定位的文件系统流位置的方式。</param>
        protected internal override void Seek(long offset, SeekOrigin origin)
        {
            m_FileStream.Seek(offset, origin);
        }

        /// <summary>
        /// 从文件系统流中读取一个字节。
        /// </summary>
        /// <returns>读取的字节。</returns>
        protected internal override byte ReadByte()
        {
            return (byte)m_FileStream.ReadByte();
        }

        /// <summary>
        /// 从文件系统流中读取二进制流。
        /// </summary>
        /// <param name="buffer">存储读取文件内容的二进制流。</param>
        /// <param name="startIndex">存储读取文件内容的二进制流的起始位置。</param>
        /// <param name="length">存储读取文件内容的二进制流的长度。</param>
        /// <returns>实际读取了多少字节。</returns>
        protected internal override int Read(byte[] buffer, int startIndex, int length)
        {
            return m_FileStream.Read(buffer, startIndex, length);
        }

        /// <summary>
        /// 从文件系统流中读取二进制流。
        /// </summary>
        /// <param name="stream">存储读取文件内容的二进制流。</param>
        /// <param name="length">存储读取文件内容的二进制流的长度。</param>
        /// <returns>实际读取了多少字节。</returns>
        protected internal override int Read(Stream stream, int length)
        {
            int bytesRead = 0;
            int bytesLeft = length;
            while ((bytesRead = m_FileStream.Read(s_CachedBytes, 0, bytesLeft < CachedBytesLength ? bytesLeft : CachedBytesLength)) > 0)
            {
                bytesLeft -= bytesRead;
                stream.Write(s_CachedBytes, 0, bytesRead);
            }

            Array.Clear(s_CachedBytes, 0, CachedBytesLength);
            return length - bytesLeft;
        }

        /// <summary>
        /// 向文件系统流中写入一个字节。
        /// </summary>
        /// <param name="value">要写入的字节。</param>
        protected internal override void WriteByte(byte value)
        {
            m_FileStream.WriteByte(value);
        }

        /// <summary>
        /// 向文件系统流中写入二进制流。
        /// </summary>
        /// <param name="buffer">存储写入文件内容的二进制流。</param>
        /// <param name="startIndex">存储写入文件内容的二进制流的起始位置。</param>
        /// <param name="length">存储写入文件内容的二进制流的长度。</param>
        protected internal override void Write(byte[] buffer, int startIndex, int length)
        {
            m_FileStream.Write(buffer, startIndex, length);
        }

        /// <summary>
        /// 向文件系统流中写入二进制流。
        /// </summary>
        /// <param name="stream">存储写入文件内容的二进制流。</param>
        /// <param name="length">存储写入文件内容的二进制流的长度。</param>
        protected internal override void Write(Stream stream, int length)
        {
            int bytesRead = 0;
            int bytesLeft = length;
            while ((bytesRead = stream.Read(s_CachedBytes, 0, bytesLeft < CachedBytesLength ? bytesLeft : CachedBytesLength)) > 0)
            {
                bytesLeft -= bytesRead;
                m_FileStream.Write(s_CachedBytes, 0, bytesRead);
            }

            Array.Clear(s_CachedBytes, 0, CachedBytesLength);
        }

        /// <summary>
        /// 将文件系统流立刻更新到存储介质中。
        /// </summary>
        protected internal override void Flush()
        {
            m_FileStream.Flush();
        }

        /// <summary>
        /// 关闭文件系统流。
        /// </summary>
        protected internal override void Close()
        {
            m_FileStream.Close();
        }

        /// <summary>
        /// 销毁文件系统流。
        /// </summary>
        public void Dispose()
        {
            m_FileStream.Dispose();
        }
    }
}
