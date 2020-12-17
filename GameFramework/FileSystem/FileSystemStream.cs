//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;
using System.IO;

namespace GameFramework.FileSystem
{
    /// <summary>
    /// 文件系统流。
    /// </summary>
    public abstract class FileSystemStream
    {
        /// <summary>
        /// 缓存二进制流的长度。
        /// </summary>
        protected const int CachedBytesLength = 0x1000;

        /// <summary>
        /// 缓存二进制流。
        /// </summary>
        protected static readonly byte[] s_CachedBytes = new byte[CachedBytesLength];

        /// <summary>
        /// 获取或设置文件系统流位置。
        /// </summary>
        protected internal abstract long Position
        {
            get;
            set;
        }

        /// <summary>
        /// 获取文件系统流长度。
        /// </summary>
        protected internal abstract long Length
        {
            get;
        }

        /// <summary>
        /// 设置文件系统流长度。
        /// </summary>
        /// <param name="length">要设置的文件系统流的长度。</param>
        protected internal abstract void SetLength(long length);

        /// <summary>
        /// 定位文件系统流位置。
        /// </summary>
        /// <param name="offset">要定位的文件系统流位置的偏移。</param>
        /// <param name="origin">要定位的文件系统流位置的方式。</param>
        protected internal abstract void Seek(long offset, SeekOrigin origin);

        /// <summary>
        /// 从文件系统流中读取一个字节。
        /// </summary>
        /// <returns>读取的字节，若已经到达文件结尾，则返回 -1。</returns>
        protected internal abstract int ReadByte();

        /// <summary>
        /// 从文件系统流中读取二进制流。
        /// </summary>
        /// <param name="buffer">存储读取文件内容的二进制流。</param>
        /// <param name="startIndex">存储读取文件内容的二进制流的起始位置。</param>
        /// <param name="length">存储读取文件内容的二进制流的长度。</param>
        /// <returns>实际读取了多少字节。</returns>
        protected internal abstract int Read(byte[] buffer, int startIndex, int length);

        /// <summary>
        /// 从文件系统流中读取二进制流。
        /// </summary>
        /// <param name="stream">存储读取文件内容的二进制流。</param>
        /// <param name="length">存储读取文件内容的二进制流的长度。</param>
        /// <returns>实际读取了多少字节。</returns>
        protected internal int Read(Stream stream, int length)
        {
            int bytesRead = 0;
            int bytesLeft = length;
            while ((bytesRead = Read(s_CachedBytes, 0, bytesLeft < CachedBytesLength ? bytesLeft : CachedBytesLength)) > 0)
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
        protected internal abstract void WriteByte(byte value);

        /// <summary>
        /// 向文件系统流中写入二进制流。
        /// </summary>
        /// <param name="buffer">存储写入文件内容的二进制流。</param>
        /// <param name="startIndex">存储写入文件内容的二进制流的起始位置。</param>
        /// <param name="length">存储写入文件内容的二进制流的长度。</param>
        protected internal abstract void Write(byte[] buffer, int startIndex, int length);

        /// <summary>
        /// 向文件系统流中写入二进制流。
        /// </summary>
        /// <param name="stream">存储写入文件内容的二进制流。</param>
        /// <param name="length">存储写入文件内容的二进制流的长度。</param>
        protected internal void Write(Stream stream, int length)
        {
            int bytesRead = 0;
            int bytesLeft = length;
            while ((bytesRead = stream.Read(s_CachedBytes, 0, bytesLeft < CachedBytesLength ? bytesLeft : CachedBytesLength)) > 0)
            {
                bytesLeft -= bytesRead;
                Write(s_CachedBytes, 0, bytesRead);
            }

            Array.Clear(s_CachedBytes, 0, CachedBytesLength);
        }

        /// <summary>
        /// 将文件系统流立刻更新到存储介质中。
        /// </summary>
        protected internal abstract void Flush();

        /// <summary>
        /// 关闭文件系统流。
        /// </summary>
        protected internal abstract void Close();
    }
}
