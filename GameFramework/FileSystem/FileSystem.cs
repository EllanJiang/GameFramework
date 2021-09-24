//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace GameFramework.FileSystem
{
    /// <summary>
    /// 文件系统。
    /// </summary>
    internal sealed partial class FileSystem : IFileSystem
    {
        private const int ClusterSize = 1024 * 4;
        private const int CachedBytesLength = 0x1000;

        private static readonly string[] EmptyStringArray = new string[] { };
        private static readonly byte[] s_CachedBytes = new byte[CachedBytesLength];

        private static readonly int HeaderDataSize = Marshal.SizeOf(typeof(HeaderData));
        private static readonly int BlockDataSize = Marshal.SizeOf(typeof(BlockData));
        private static readonly int StringDataSize = Marshal.SizeOf(typeof(StringData));

        private readonly string m_FullPath;
        private readonly FileSystemAccess m_Access;
        private readonly FileSystemStream m_Stream;
        private readonly Dictionary<string, int> m_FileDatas;
        private readonly List<BlockData> m_BlockDatas;
        private readonly GameFrameworkMultiDictionary<int, int> m_FreeBlockIndexes;
        private readonly SortedDictionary<int, StringData> m_StringDatas;
        private readonly Queue<int> m_FreeStringIndexes;
        private readonly Queue<StringData> m_FreeStringDatas;

        private HeaderData m_HeaderData;
        private int m_BlockDataOffset;
        private int m_StringDataOffset;
        private int m_FileDataOffset;

        /// <summary>
        /// 初始化文件系统的新实例。
        /// </summary>
        /// <param name="fullPath">文件系统完整路径。</param>
        /// <param name="access">文件系统访问方式。</param>
        /// <param name="stream">文件系统流。</param>
        private FileSystem(string fullPath, FileSystemAccess access, FileSystemStream stream)
        {
            if (string.IsNullOrEmpty(fullPath))
            {
                throw new GameFrameworkException("Full path is invalid.");
            }

            if (access == FileSystemAccess.Unspecified)
            {
                throw new GameFrameworkException("Access is invalid.");
            }

            if (stream == null)
            {
                throw new GameFrameworkException("Stream is invalid.");
            }

            m_FullPath = fullPath;
            m_Access = access;
            m_Stream = stream;
            m_FileDatas = new Dictionary<string, int>(StringComparer.Ordinal);
            m_BlockDatas = new List<BlockData>();
            m_FreeBlockIndexes = new GameFrameworkMultiDictionary<int, int>();
            m_StringDatas = new SortedDictionary<int, StringData>();
            m_FreeStringIndexes = new Queue<int>();
            m_FreeStringDatas = new Queue<StringData>();

            m_HeaderData = default(HeaderData);
            m_BlockDataOffset = 0;
            m_StringDataOffset = 0;
            m_FileDataOffset = 0;

            Utility.Marshal.EnsureCachedHGlobalSize(CachedBytesLength);
        }

        /// <summary>
        /// 获取文件系统完整路径。
        /// </summary>
        public string FullPath
        {
            get
            {
                return m_FullPath;
            }
        }

        /// <summary>
        /// 获取文件系统访问方式。
        /// </summary>
        public FileSystemAccess Access
        {
            get
            {
                return m_Access;
            }
        }

        /// <summary>
        /// 获取文件数量。
        /// </summary>
        public int FileCount
        {
            get
            {
                return m_FileDatas.Count;
            }
        }

        /// <summary>
        /// 获取最大文件数量。
        /// </summary>
        public int MaxFileCount
        {
            get
            {
                return m_HeaderData.MaxFileCount;
            }
        }

        /// <summary>
        /// 创建文件系统。
        /// </summary>
        /// <param name="fullPath">要创建的文件系统的完整路径。</param>
        /// <param name="access">要创建的文件系统的访问方式。</param>
        /// <param name="stream">要创建的文件系统的文件系统流。</param>
        /// <param name="maxFileCount">要创建的文件系统的最大文件数量。</param>
        /// <param name="maxBlockCount">要创建的文件系统的最大块数据数量。</param>
        /// <returns>创建的文件系统。</returns>
        public static FileSystem Create(string fullPath, FileSystemAccess access, FileSystemStream stream, int maxFileCount, int maxBlockCount)
        {
            if (maxFileCount <= 0)
            {
                throw new GameFrameworkException("Max file count is invalid.");
            }

            if (maxBlockCount <= 0)
            {
                throw new GameFrameworkException("Max block count is invalid.");
            }

            if (maxFileCount > maxBlockCount)
            {
                throw new GameFrameworkException("Max file count can not larger than max block count.");
            }

            FileSystem fileSystem = new FileSystem(fullPath, access, stream);
            fileSystem.m_HeaderData = new HeaderData(maxFileCount, maxBlockCount);
            CalcOffsets(fileSystem);
            Utility.Marshal.StructureToBytes(fileSystem.m_HeaderData, HeaderDataSize, s_CachedBytes);

            try
            {
                stream.Write(s_CachedBytes, 0, HeaderDataSize);
                stream.SetLength(fileSystem.m_FileDataOffset);
                return fileSystem;
            }
            catch
            {
                fileSystem.Shutdown();
                return null;
            }
        }

        /// <summary>
        /// 加载文件系统。
        /// </summary>
        /// <param name="fullPath">要加载的文件系统的完整路径。</param>
        /// <param name="access">要加载的文件系统的访问方式。</param>
        /// <param name="stream">要加载的文件系统的文件系统流。</param>
        /// <returns>加载的文件系统。</returns>
        public static FileSystem Load(string fullPath, FileSystemAccess access, FileSystemStream stream)
        {
            FileSystem fileSystem = new FileSystem(fullPath, access, stream);

            stream.Read(s_CachedBytes, 0, HeaderDataSize);
            fileSystem.m_HeaderData = Utility.Marshal.BytesToStructure<HeaderData>(HeaderDataSize, s_CachedBytes);
            if (!fileSystem.m_HeaderData.IsValid)
            {
                return null;
            }

            CalcOffsets(fileSystem);

            if (fileSystem.m_BlockDatas.Capacity < fileSystem.m_HeaderData.BlockCount)
            {
                fileSystem.m_BlockDatas.Capacity = fileSystem.m_HeaderData.BlockCount;
            }

            for (int i = 0; i < fileSystem.m_HeaderData.BlockCount; i++)
            {
                stream.Read(s_CachedBytes, 0, BlockDataSize);
                BlockData blockData = Utility.Marshal.BytesToStructure<BlockData>(BlockDataSize, s_CachedBytes);
                fileSystem.m_BlockDatas.Add(blockData);
            }

            for (int i = 0; i < fileSystem.m_BlockDatas.Count; i++)
            {
                BlockData blockData = fileSystem.m_BlockDatas[i];
                if (blockData.Using)
                {
                    StringData stringData = fileSystem.ReadStringData(blockData.StringIndex);
                    fileSystem.m_StringDatas.Add(blockData.StringIndex, stringData);
                    fileSystem.m_FileDatas.Add(stringData.GetString(fileSystem.m_HeaderData.GetEncryptBytes()), i);
                }
                else
                {
                    fileSystem.m_FreeBlockIndexes.Add(blockData.Length, i);
                }
            }

            int index = 0;
            foreach (KeyValuePair<int, StringData> i in fileSystem.m_StringDatas)
            {
                while (index < i.Key)
                {
                    fileSystem.m_FreeStringIndexes.Enqueue(index++);
                }

                index++;
            }

            return fileSystem;
        }

        /// <summary>
        /// 关闭并清理文件系统。
        /// </summary>
        public void Shutdown()
        {
            m_Stream.Close();

            m_FileDatas.Clear();
            m_BlockDatas.Clear();
            m_FreeBlockIndexes.Clear();
            m_StringDatas.Clear();
            m_FreeStringIndexes.Clear();
            m_FreeStringDatas.Clear();

            m_BlockDataOffset = 0;
            m_StringDataOffset = 0;
            m_FileDataOffset = 0;
        }

        /// <summary>
        /// 获取文件信息。
        /// </summary>
        /// <param name="name">要获取文件信息的文件名称。</param>
        /// <returns>获取的文件信息。</returns>
        public FileInfo GetFileInfo(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new GameFrameworkException("Name is invalid.");
            }

            int blockIndex = 0;
            if (!m_FileDatas.TryGetValue(name, out blockIndex))
            {
                return default(FileInfo);
            }

            BlockData blockData = m_BlockDatas[blockIndex];
            return new FileInfo(name, GetClusterOffset(blockData.ClusterIndex), blockData.Length);
        }

        /// <summary>
        /// 获取所有文件信息。
        /// </summary>
        /// <returns>获取的所有文件信息。</returns>
        public FileInfo[] GetAllFileInfos()
        {
            int index = 0;
            FileInfo[] results = new FileInfo[m_FileDatas.Count];
            foreach (KeyValuePair<string, int> fileData in m_FileDatas)
            {
                BlockData blockData = m_BlockDatas[fileData.Value];
                results[index++] = new FileInfo(fileData.Key, GetClusterOffset(blockData.ClusterIndex), blockData.Length);
            }

            return results;
        }

        /// <summary>
        /// 获取所有文件信息。
        /// </summary>
        /// <param name="results">获取的所有文件信息。</param>
        public void GetAllFileInfos(List<FileInfo> results)
        {
            if (results == null)
            {
                throw new GameFrameworkException("Results is invalid.");
            }

            results.Clear();
            foreach (KeyValuePair<string, int> fileData in m_FileDatas)
            {
                BlockData blockData = m_BlockDatas[fileData.Value];
                results.Add(new FileInfo(fileData.Key, GetClusterOffset(blockData.ClusterIndex), blockData.Length));
            }
        }

        /// <summary>
        /// 检查是否存在指定文件。
        /// </summary>
        /// <param name="name">要检查的文件名称。</param>
        /// <returns>是否存在指定文件。</returns>
        public bool HasFile(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new GameFrameworkException("Name is invalid.");
            }

            return m_FileDatas.ContainsKey(name);
        }

        /// <summary>
        /// 读取指定文件。
        /// </summary>
        /// <param name="name">要读取的文件名称。</param>
        /// <returns>存储读取文件内容的二进制流。</returns>
        public byte[] ReadFile(string name)
        {
            if (m_Access != FileSystemAccess.Read && m_Access != FileSystemAccess.ReadWrite)
            {
                throw new GameFrameworkException("File system is not readable.");
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new GameFrameworkException("Name is invalid.");
            }

            FileInfo fileInfo = GetFileInfo(name);
            if (!fileInfo.IsValid)
            {
                return null;
            }

            int length = fileInfo.Length;
            byte[] buffer = new byte[length];
            if (length > 0)
            {
                m_Stream.Position = fileInfo.Offset;
                m_Stream.Read(buffer, 0, length);
            }

            return buffer;
        }

        /// <summary>
        /// 读取指定文件。
        /// </summary>
        /// <param name="name">要读取的文件名称。</param>
        /// <param name="buffer">存储读取文件内容的二进制流。</param>
        /// <returns>实际读取了多少字节。</returns>
        public int ReadFile(string name, byte[] buffer)
        {
            if (buffer == null)
            {
                throw new GameFrameworkException("Buffer is invalid.");
            }

            return ReadFile(name, buffer, 0, buffer.Length);
        }

        /// <summary>
        /// 读取指定文件。
        /// </summary>
        /// <param name="name">要读取的文件名称。</param>
        /// <param name="buffer">存储读取文件内容的二进制流。</param>
        /// <param name="startIndex">存储读取文件内容的二进制流的起始位置。</param>
        /// <returns>实际读取了多少字节。</returns>
        public int ReadFile(string name, byte[] buffer, int startIndex)
        {
            if (buffer == null)
            {
                throw new GameFrameworkException("Buffer is invalid.");
            }

            return ReadFile(name, buffer, startIndex, buffer.Length - startIndex);
        }

        /// <summary>
        /// 读取指定文件。
        /// </summary>
        /// <param name="name">要读取的文件名称。</param>
        /// <param name="buffer">存储读取文件内容的二进制流。</param>
        /// <param name="startIndex">存储读取文件内容的二进制流的起始位置。</param>
        /// <param name="length">存储读取文件内容的二进制流的长度。</param>
        /// <returns>实际读取了多少字节。</returns>
        public int ReadFile(string name, byte[] buffer, int startIndex, int length)
        {
            if (m_Access != FileSystemAccess.Read && m_Access != FileSystemAccess.ReadWrite)
            {
                throw new GameFrameworkException("File system is not readable.");
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new GameFrameworkException("Name is invalid.");
            }

            if (buffer == null)
            {
                throw new GameFrameworkException("Buffer is invalid.");
            }

            if (startIndex < 0 || length < 0 || startIndex + length > buffer.Length)
            {
                throw new GameFrameworkException("Start index or length is invalid.");
            }

            FileInfo fileInfo = GetFileInfo(name);
            if (!fileInfo.IsValid)
            {
                return 0;
            }

            m_Stream.Position = fileInfo.Offset;
            if (length > fileInfo.Length)
            {
                length = fileInfo.Length;
            }

            if (length > 0)
            {
                return m_Stream.Read(buffer, startIndex, length);
            }

            return 0;
        }

        /// <summary>
        /// 读取指定文件。
        /// </summary>
        /// <param name="name">要读取的文件名称。</param>
        /// <param name="stream">存储读取文件内容的二进制流。</param>
        /// <returns>实际读取了多少字节。</returns>
        public int ReadFile(string name, Stream stream)
        {
            if (m_Access != FileSystemAccess.Read && m_Access != FileSystemAccess.ReadWrite)
            {
                throw new GameFrameworkException("File system is not readable.");
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new GameFrameworkException("Name is invalid.");
            }

            if (stream == null)
            {
                throw new GameFrameworkException("Stream is invalid.");
            }

            if (!stream.CanWrite)
            {
                throw new GameFrameworkException("Stream is not writable.");
            }

            FileInfo fileInfo = GetFileInfo(name);
            if (!fileInfo.IsValid)
            {
                return 0;
            }

            int length = fileInfo.Length;
            if (length > 0)
            {
                m_Stream.Position = fileInfo.Offset;
                return m_Stream.Read(stream, length);
            }

            return 0;
        }

        /// <summary>
        /// 读取指定文件的指定片段。
        /// </summary>
        /// <param name="name">要读取片段的文件名称。</param>
        /// <param name="length">要读取片段的长度。</param>
        /// <returns>存储读取文件片段内容的二进制流。</returns>
        public byte[] ReadFileSegment(string name, int length)
        {
            return ReadFileSegment(name, 0, length);
        }

        /// <summary>
        /// 读取指定文件的指定片段。
        /// </summary>
        /// <param name="name">要读取片段的文件名称。</param>
        /// <param name="offset">要读取片段的偏移。</param>
        /// <param name="length">要读取片段的长度。</param>
        /// <returns>存储读取文件片段内容的二进制流。</returns>
        public byte[] ReadFileSegment(string name, int offset, int length)
        {
            if (m_Access != FileSystemAccess.Read && m_Access != FileSystemAccess.ReadWrite)
            {
                throw new GameFrameworkException("File system is not readable.");
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new GameFrameworkException("Name is invalid.");
            }

            if (offset < 0)
            {
                throw new GameFrameworkException("Index is invalid.");
            }

            if (length < 0)
            {
                throw new GameFrameworkException("Length is invalid.");
            }

            FileInfo fileInfo = GetFileInfo(name);
            if (!fileInfo.IsValid)
            {
                return null;
            }

            if (offset > fileInfo.Length)
            {
                offset = fileInfo.Length;
            }

            int leftLength = fileInfo.Length - offset;
            if (length > leftLength)
            {
                length = leftLength;
            }

            byte[] buffer = new byte[length];
            if (length > 0)
            {
                m_Stream.Position = fileInfo.Offset + offset;
                m_Stream.Read(buffer, 0, length);
            }

            return buffer;
        }

        /// <summary>
        /// 读取指定文件的指定片段。
        /// </summary>
        /// <param name="name">要读取片段的文件名称。</param>
        /// <param name="buffer">存储读取文件片段内容的二进制流。</param>
        /// <returns>实际读取了多少字节。</returns>
        public int ReadFileSegment(string name, byte[] buffer)
        {
            if (buffer == null)
            {
                throw new GameFrameworkException("Buffer is invalid.");
            }

            return ReadFileSegment(name, 0, buffer, 0, buffer.Length);
        }

        /// <summary>
        /// 读取指定文件的指定片段。
        /// </summary>
        /// <param name="name">要读取片段的文件名称。</param>
        /// <param name="buffer">存储读取文件片段内容的二进制流。</param>
        /// <param name="length">要读取片段的长度。</param>
        /// <returns>实际读取了多少字节。</returns>
        public int ReadFileSegment(string name, byte[] buffer, int length)
        {
            return ReadFileSegment(name, 0, buffer, 0, length);
        }

        /// <summary>
        /// 读取指定文件的指定片段。
        /// </summary>
        /// <param name="name">要读取片段的文件名称。</param>
        /// <param name="buffer">存储读取文件片段内容的二进制流。</param>
        /// <param name="startIndex">存储读取文件片段内容的二进制流的起始位置。</param>
        /// <param name="length">要读取片段的长度。</param>
        /// <returns>实际读取了多少字节。</returns>
        public int ReadFileSegment(string name, byte[] buffer, int startIndex, int length)
        {
            return ReadFileSegment(name, 0, buffer, startIndex, length);
        }

        /// <summary>
        /// 读取指定文件的指定片段。
        /// </summary>
        /// <param name="name">要读取片段的文件名称。</param>
        /// <param name="offset">要读取片段的偏移。</param>
        /// <param name="buffer">存储读取文件片段内容的二进制流。</param>
        /// <returns>实际读取了多少字节。</returns>
        public int ReadFileSegment(string name, int offset, byte[] buffer)
        {
            if (buffer == null)
            {
                throw new GameFrameworkException("Buffer is invalid.");
            }

            return ReadFileSegment(name, offset, buffer, 0, buffer.Length);
        }

        /// <summary>
        /// 读取指定文件的指定片段。
        /// </summary>
        /// <param name="name">要读取片段的文件名称。</param>
        /// <param name="offset">要读取片段的偏移。</param>
        /// <param name="buffer">存储读取文件片段内容的二进制流。</param>
        /// <param name="length">要读取片段的长度。</param>
        /// <returns>实际读取了多少字节。</returns>
        public int ReadFileSegment(string name, int offset, byte[] buffer, int length)
        {
            return ReadFileSegment(name, offset, buffer, 0, length);
        }

        /// <summary>
        /// 读取指定文件的指定片段。
        /// </summary>
        /// <param name="name">要读取片段的文件名称。</param>
        /// <param name="offset">要读取片段的偏移。</param>
        /// <param name="buffer">存储读取文件片段内容的二进制流。</param>
        /// <param name="startIndex">存储读取文件片段内容的二进制流的起始位置。</param>
        /// <param name="length">要读取片段的长度。</param>
        /// <returns>实际读取了多少字节。</returns>
        public int ReadFileSegment(string name, int offset, byte[] buffer, int startIndex, int length)
        {
            if (m_Access != FileSystemAccess.Read && m_Access != FileSystemAccess.ReadWrite)
            {
                throw new GameFrameworkException("File system is not readable.");
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new GameFrameworkException("Name is invalid.");
            }

            if (offset < 0)
            {
                throw new GameFrameworkException("Index is invalid.");
            }

            if (buffer == null)
            {
                throw new GameFrameworkException("Buffer is invalid.");
            }

            if (startIndex < 0 || length < 0 || startIndex + length > buffer.Length)
            {
                throw new GameFrameworkException("Start index or length is invalid.");
            }

            FileInfo fileInfo = GetFileInfo(name);
            if (!fileInfo.IsValid)
            {
                return 0;
            }

            if (offset > fileInfo.Length)
            {
                offset = fileInfo.Length;
            }

            int leftLength = fileInfo.Length - offset;
            if (length > leftLength)
            {
                length = leftLength;
            }

            if (length > 0)
            {
                m_Stream.Position = fileInfo.Offset + offset;
                return m_Stream.Read(buffer, startIndex, length);
            }

            return 0;
        }

        /// <summary>
        /// 读取指定文件的指定片段。
        /// </summary>
        /// <param name="name">要读取片段的文件名称。</param>
        /// <param name="stream">存储读取文件片段内容的二进制流。</param>
        /// <param name="length">要读取片段的长度。</param>
        /// <returns>实际读取了多少字节。</returns>
        public int ReadFileSegment(string name, Stream stream, int length)
        {
            return ReadFileSegment(name, 0, stream, length);
        }

        /// <summary>
        /// 读取指定文件的指定片段。
        /// </summary>
        /// <param name="name">要读取片段的文件名称。</param>
        /// <param name="offset">要读取片段的偏移。</param>
        /// <param name="stream">存储读取文件片段内容的二进制流。</param>
        /// <param name="length">要读取片段的长度。</param>
        /// <returns>实际读取了多少字节。</returns>
        public int ReadFileSegment(string name, int offset, Stream stream, int length)
        {
            if (m_Access != FileSystemAccess.Read && m_Access != FileSystemAccess.ReadWrite)
            {
                throw new GameFrameworkException("File system is not readable.");
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new GameFrameworkException("Name is invalid.");
            }

            if (offset < 0)
            {
                throw new GameFrameworkException("Index is invalid.");
            }

            if (stream == null)
            {
                throw new GameFrameworkException("Stream is invalid.");
            }

            if (!stream.CanWrite)
            {
                throw new GameFrameworkException("Stream is not writable.");
            }

            if (length < 0)
            {
                throw new GameFrameworkException("Length is invalid.");
            }

            FileInfo fileInfo = GetFileInfo(name);
            if (!fileInfo.IsValid)
            {
                return 0;
            }

            if (offset > fileInfo.Length)
            {
                offset = fileInfo.Length;
            }

            int leftLength = fileInfo.Length - offset;
            if (length > leftLength)
            {
                length = leftLength;
            }

            if (length > 0)
            {
                m_Stream.Position = fileInfo.Offset + offset;
                return m_Stream.Read(stream, length);
            }

            return 0;
        }

        /// <summary>
        /// 写入指定文件。
        /// </summary>
        /// <param name="name">要写入的文件名称。</param>
        /// <param name="buffer">存储写入文件内容的二进制流。</param>
        /// <returns>是否写入指定文件成功。</returns>
        public bool WriteFile(string name, byte[] buffer)
        {
            if (buffer == null)
            {
                throw new GameFrameworkException("Buffer is invalid.");
            }

            return WriteFile(name, buffer, 0, buffer.Length);
        }

        /// <summary>
        /// 写入指定文件。
        /// </summary>
        /// <param name="name">要写入的文件名称。</param>
        /// <param name="buffer">存储写入文件内容的二进制流。</param>
        /// <param name="startIndex">存储写入文件内容的二进制流的起始位置。</param>
        /// <returns>是否写入指定文件成功。</returns>
        public bool WriteFile(string name, byte[] buffer, int startIndex)
        {
            if (buffer == null)
            {
                throw new GameFrameworkException("Buffer is invalid.");
            }

            return WriteFile(name, buffer, startIndex, buffer.Length - startIndex);
        }

        /// <summary>
        /// 写入指定文件。
        /// </summary>
        /// <param name="name">要写入的文件名称。</param>
        /// <param name="buffer">存储写入文件内容的二进制流。</param>
        /// <param name="startIndex">存储写入文件内容的二进制流的起始位置。</param>
        /// <param name="length">存储写入文件内容的二进制流的长度。</param>
        /// <returns>是否写入指定文件成功。</returns>
        public bool WriteFile(string name, byte[] buffer, int startIndex, int length)
        {
            if (m_Access != FileSystemAccess.Write && m_Access != FileSystemAccess.ReadWrite)
            {
                throw new GameFrameworkException("File system is not writable.");
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new GameFrameworkException("Name is invalid.");
            }

            if (name.Length > byte.MaxValue)
            {
                throw new GameFrameworkException(Utility.Text.Format("Name '{0}' is too long.", name));
            }

            if (buffer == null)
            {
                throw new GameFrameworkException("Buffer is invalid.");
            }

            if (startIndex < 0 || length < 0 || startIndex + length > buffer.Length)
            {
                throw new GameFrameworkException("Start index or length is invalid.");
            }

            bool hasFile = false;
            int oldBlockIndex = -1;
            if (m_FileDatas.TryGetValue(name, out oldBlockIndex))
            {
                hasFile = true;
            }

            if (!hasFile && m_FileDatas.Count >= m_HeaderData.MaxFileCount)
            {
                return false;
            }

            int blockIndex = AllocBlock(length);
            if (blockIndex < 0)
            {
                return false;
            }

            if (length > 0)
            {
                m_Stream.Position = GetClusterOffset(m_BlockDatas[blockIndex].ClusterIndex);
                m_Stream.Write(buffer, startIndex, length);
            }

            ProcessWriteFile(name, hasFile, oldBlockIndex, blockIndex, length);
            m_Stream.Flush();
            return true;
        }

        /// <summary>
        /// 写入指定文件。
        /// </summary>
        /// <param name="name">要写入的文件名称。</param>
        /// <param name="stream">存储写入文件内容的二进制流。</param>
        /// <returns>是否写入指定文件成功。</returns>
        public bool WriteFile(string name, Stream stream)
        {
            if (m_Access != FileSystemAccess.Write && m_Access != FileSystemAccess.ReadWrite)
            {
                throw new GameFrameworkException("File system is not writable.");
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new GameFrameworkException("Name is invalid.");
            }

            if (name.Length > byte.MaxValue)
            {
                throw new GameFrameworkException(Utility.Text.Format("Name '{0}' is too long.", name));
            }

            if (stream == null)
            {
                throw new GameFrameworkException("Stream is invalid.");
            }

            if (!stream.CanRead)
            {
                throw new GameFrameworkException("Stream is not readable.");
            }

            bool hasFile = false;
            int oldBlockIndex = -1;
            if (m_FileDatas.TryGetValue(name, out oldBlockIndex))
            {
                hasFile = true;
            }

            if (!hasFile && m_FileDatas.Count >= m_HeaderData.MaxFileCount)
            {
                return false;
            }

            int length = (int)(stream.Length - stream.Position);
            int blockIndex = AllocBlock(length);
            if (blockIndex < 0)
            {
                return false;
            }

            if (length > 0)
            {
                m_Stream.Position = GetClusterOffset(m_BlockDatas[blockIndex].ClusterIndex);
                m_Stream.Write(stream, length);
            }

            ProcessWriteFile(name, hasFile, oldBlockIndex, blockIndex, length);
            m_Stream.Flush();
            return true;
        }

        /// <summary>
        /// 写入指定文件。
        /// </summary>
        /// <param name="name">要写入的文件名称。</param>
        /// <param name="filePath">存储写入文件内容的文件路径。</param>
        /// <returns>是否写入指定文件成功。</returns>
        public bool WriteFile(string name, string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new GameFrameworkException("File path is invalid");
            }

            if (!File.Exists(filePath))
            {
                return false;
            }

            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return WriteFile(name, fileStream);
            }
        }

        /// <summary>
        /// 将指定文件另存为物理文件。
        /// </summary>
        /// <param name="name">要另存为的文件名称。</param>
        /// <param name="filePath">存储写入文件内容的文件路径。</param>
        /// <returns>是否将指定文件另存为物理文件成功。</returns>
        public bool SaveAsFile(string name, string filePath)
        {
            if (m_Access != FileSystemAccess.Read && m_Access != FileSystemAccess.ReadWrite)
            {
                throw new GameFrameworkException("File system is not readable.");
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new GameFrameworkException("Name is invalid.");
            }

            if (string.IsNullOrEmpty(filePath))
            {
                throw new GameFrameworkException("File path is invalid");
            }

            FileInfo fileInfo = GetFileInfo(name);
            if (!fileInfo.IsValid)
            {
                return false;
            }

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            string directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                int length = fileInfo.Length;
                if (length > 0)
                {
                    m_Stream.Position = fileInfo.Offset;
                    return m_Stream.Read(fileStream, length) == length;
                }

                return true;
            }
        }

        /// <summary>
        /// 重命名指定文件。
        /// </summary>
        /// <param name="oldName">要重命名的文件名称。</param>
        /// <param name="newName">重命名后的文件名称。</param>
        /// <returns>是否重命名指定文件成功。</returns>
        public bool RenameFile(string oldName, string newName)
        {
            if (m_Access != FileSystemAccess.Write && m_Access != FileSystemAccess.ReadWrite)
            {
                throw new GameFrameworkException("File system is not writable.");
            }

            if (string.IsNullOrEmpty(oldName))
            {
                throw new GameFrameworkException("Old name is invalid.");
            }

            if (string.IsNullOrEmpty(newName))
            {
                throw new GameFrameworkException("New name is invalid.");
            }

            if (newName.Length > byte.MaxValue)
            {
                throw new GameFrameworkException(Utility.Text.Format("New name '{0}' is too long.", newName));
            }

            if (oldName == newName)
            {
                return true;
            }

            if (m_FileDatas.ContainsKey(newName))
            {
                return false;
            }

            int blockIndex = 0;
            if (!m_FileDatas.TryGetValue(oldName, out blockIndex))
            {
                return false;
            }

            int stringIndex = m_BlockDatas[blockIndex].StringIndex;
            StringData stringData = m_StringDatas[stringIndex].SetString(newName, m_HeaderData.GetEncryptBytes());
            m_StringDatas[stringIndex] = stringData;
            WriteStringData(stringIndex, stringData);
            m_FileDatas.Add(newName, blockIndex);
            m_FileDatas.Remove(oldName);
            m_Stream.Flush();
            return true;
        }

        /// <summary>
        /// 删除指定文件。
        /// </summary>
        /// <param name="name">要删除的文件名称。</param>
        /// <returns>是否删除指定文件成功。</returns>
        public bool DeleteFile(string name)
        {
            if (m_Access != FileSystemAccess.Write && m_Access != FileSystemAccess.ReadWrite)
            {
                throw new GameFrameworkException("File system is not writable.");
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new GameFrameworkException("Name is invalid.");
            }

            int blockIndex = 0;
            if (!m_FileDatas.TryGetValue(name, out blockIndex))
            {
                return false;
            }

            m_FileDatas.Remove(name);

            BlockData blockData = m_BlockDatas[blockIndex];
            int stringIndex = blockData.StringIndex;
            StringData stringData = m_StringDatas[stringIndex].Clear();
            m_FreeStringIndexes.Enqueue(stringIndex);
            m_FreeStringDatas.Enqueue(stringData);
            m_StringDatas.Remove(stringIndex);
            WriteStringData(stringIndex, stringData);

            blockData = blockData.Free();
            m_BlockDatas[blockIndex] = blockData;
            if (!TryCombineFreeBlocks(blockIndex))
            {
                m_FreeBlockIndexes.Add(blockData.Length, blockIndex);
                WriteBlockData(blockIndex);
            }

            m_Stream.Flush();
            return true;
        }

        private void ProcessWriteFile(string name, bool hasFile, int oldBlockIndex, int blockIndex, int length)
        {
            BlockData blockData = m_BlockDatas[blockIndex];
            if (hasFile)
            {
                BlockData oldBlockData = m_BlockDatas[oldBlockIndex];
                blockData = new BlockData(oldBlockData.StringIndex, blockData.ClusterIndex, length);
                m_BlockDatas[blockIndex] = blockData;
                WriteBlockData(blockIndex);

                oldBlockData = oldBlockData.Free();
                m_BlockDatas[oldBlockIndex] = oldBlockData;
                if (!TryCombineFreeBlocks(oldBlockIndex))
                {
                    m_FreeBlockIndexes.Add(oldBlockData.Length, oldBlockIndex);
                    WriteBlockData(oldBlockIndex);
                }
            }
            else
            {
                int stringIndex = AllocString(name);
                blockData = new BlockData(stringIndex, blockData.ClusterIndex, length);
                m_BlockDatas[blockIndex] = blockData;
                WriteBlockData(blockIndex);
            }

            if (hasFile)
            {
                m_FileDatas[name] = blockIndex;
            }
            else
            {
                m_FileDatas.Add(name, blockIndex);
            }
        }

        private bool TryCombineFreeBlocks(int freeBlockIndex)
        {
            BlockData freeBlockData = m_BlockDatas[freeBlockIndex];
            if (freeBlockData.Length <= 0)
            {
                return false;
            }

            int previousFreeBlockIndex = -1;
            int nextFreeBlockIndex = -1;
            int nextBlockDataClusterIndex = freeBlockData.ClusterIndex + GetUpBoundClusterCount(freeBlockData.Length);
            foreach (KeyValuePair<int, GameFrameworkLinkedListRange<int>> blockIndexes in m_FreeBlockIndexes)
            {
                if (blockIndexes.Key <= 0)
                {
                    continue;
                }

                int blockDataClusterCount = GetUpBoundClusterCount(blockIndexes.Key);
                foreach (int blockIndex in blockIndexes.Value)
                {
                    BlockData blockData = m_BlockDatas[blockIndex];
                    if (blockData.ClusterIndex + blockDataClusterCount == freeBlockData.ClusterIndex)
                    {
                        previousFreeBlockIndex = blockIndex;
                    }
                    else if (blockData.ClusterIndex == nextBlockDataClusterIndex)
                    {
                        nextFreeBlockIndex = blockIndex;
                    }
                }
            }

            if (previousFreeBlockIndex < 0 && nextFreeBlockIndex < 0)
            {
                return false;
            }

            m_FreeBlockIndexes.Remove(freeBlockData.Length, freeBlockIndex);
            if (previousFreeBlockIndex >= 0)
            {
                BlockData previousFreeBlockData = m_BlockDatas[previousFreeBlockIndex];
                m_FreeBlockIndexes.Remove(previousFreeBlockData.Length, previousFreeBlockIndex);
                freeBlockData = new BlockData(previousFreeBlockData.ClusterIndex, previousFreeBlockData.Length + freeBlockData.Length);
                m_BlockDatas[previousFreeBlockIndex] = BlockData.Empty;
                m_FreeBlockIndexes.Add(0, previousFreeBlockIndex);
                WriteBlockData(previousFreeBlockIndex);
            }

            if (nextFreeBlockIndex >= 0)
            {
                BlockData nextFreeBlockData = m_BlockDatas[nextFreeBlockIndex];
                m_FreeBlockIndexes.Remove(nextFreeBlockData.Length, nextFreeBlockIndex);
                freeBlockData = new BlockData(freeBlockData.ClusterIndex, freeBlockData.Length + nextFreeBlockData.Length);
                m_BlockDatas[nextFreeBlockIndex] = BlockData.Empty;
                m_FreeBlockIndexes.Add(0, nextFreeBlockIndex);
                WriteBlockData(nextFreeBlockIndex);
            }

            m_BlockDatas[freeBlockIndex] = freeBlockData;
            m_FreeBlockIndexes.Add(freeBlockData.Length, freeBlockIndex);
            WriteBlockData(freeBlockIndex);
            return true;
        }

        private int GetEmptyBlockIndex()
        {
            GameFrameworkLinkedListRange<int> lengthRange = default(GameFrameworkLinkedListRange<int>);
            if (m_FreeBlockIndexes.TryGetValue(0, out lengthRange))
            {
                int blockIndex = lengthRange.First.Value;
                m_FreeBlockIndexes.Remove(0, blockIndex);
                return blockIndex;
            }

            if (m_BlockDatas.Count < m_HeaderData.MaxBlockCount)
            {
                int blockIndex = m_BlockDatas.Count;
                m_BlockDatas.Add(BlockData.Empty);
                WriteHeaderData();
                return blockIndex;
            }

            return -1;
        }

        private int AllocBlock(int length)
        {
            if (length <= 0)
            {
                return GetEmptyBlockIndex();
            }

            length = (int)GetUpBoundClusterOffset(length);

            int lengthFound = -1;
            GameFrameworkLinkedListRange<int> lengthRange = default(GameFrameworkLinkedListRange<int>);
            foreach (KeyValuePair<int, GameFrameworkLinkedListRange<int>> i in m_FreeBlockIndexes)
            {
                if (i.Key < length)
                {
                    continue;
                }

                if (lengthFound >= 0 && lengthFound < i.Key)
                {
                    continue;
                }

                lengthFound = i.Key;
                lengthRange = i.Value;
            }

            if (lengthFound >= 0)
            {
                if (lengthFound > length && m_BlockDatas.Count >= m_HeaderData.MaxBlockCount)
                {
                    return -1;
                }

                int blockIndex = lengthRange.First.Value;
                m_FreeBlockIndexes.Remove(lengthFound, blockIndex);
                if (lengthFound > length)
                {
                    BlockData blockData = m_BlockDatas[blockIndex];
                    m_BlockDatas[blockIndex] = new BlockData(blockData.ClusterIndex, length);
                    WriteBlockData(blockIndex);

                    int deltaLength = lengthFound - length;
                    int anotherBlockIndex = GetEmptyBlockIndex();
                    m_BlockDatas[anotherBlockIndex] = new BlockData(blockData.ClusterIndex + GetUpBoundClusterCount(length), deltaLength);
                    m_FreeBlockIndexes.Add(deltaLength, anotherBlockIndex);
                    WriteBlockData(anotherBlockIndex);
                }

                return blockIndex;
            }
            else
            {
                int blockIndex = GetEmptyBlockIndex();
                if (blockIndex < 0)
                {
                    return -1;
                }

                long fileLength = m_Stream.Length;
                try
                {
                    m_Stream.SetLength(fileLength + length);
                }
                catch
                {
                    return -1;
                }

                m_BlockDatas[blockIndex] = new BlockData(GetUpBoundClusterCount(fileLength), length);
                WriteBlockData(blockIndex);
                return blockIndex;
            }
        }

        private int AllocString(string value)
        {
            int stringIndex = -1;
            StringData stringData = default(StringData);

            if (m_FreeStringIndexes.Count > 0)
            {
                stringIndex = m_FreeStringIndexes.Dequeue();
            }
            else
            {
                stringIndex = m_StringDatas.Count;
            }

            if (m_FreeStringDatas.Count > 0)
            {
                stringData = m_FreeStringDatas.Dequeue();
            }
            else
            {
                byte[] bytes = new byte[byte.MaxValue];
                Utility.Random.GetRandomBytes(bytes);
                stringData = new StringData(0, bytes);
            }

            stringData = stringData.SetString(value, m_HeaderData.GetEncryptBytes());
            m_StringDatas.Add(stringIndex, stringData);
            WriteStringData(stringIndex, stringData);
            return stringIndex;
        }

        private void WriteHeaderData()
        {
            m_HeaderData = m_HeaderData.SetBlockCount(m_BlockDatas.Count);
            Utility.Marshal.StructureToBytes(m_HeaderData, HeaderDataSize, s_CachedBytes);
            m_Stream.Position = 0L;
            m_Stream.Write(s_CachedBytes, 0, HeaderDataSize);
        }

        private void WriteBlockData(int blockIndex)
        {
            Utility.Marshal.StructureToBytes(m_BlockDatas[blockIndex], BlockDataSize, s_CachedBytes);
            m_Stream.Position = m_BlockDataOffset + BlockDataSize * blockIndex;
            m_Stream.Write(s_CachedBytes, 0, BlockDataSize);
        }

        private StringData ReadStringData(int stringIndex)
        {
            m_Stream.Position = m_StringDataOffset + StringDataSize * stringIndex;
            m_Stream.Read(s_CachedBytes, 0, StringDataSize);
            return Utility.Marshal.BytesToStructure<StringData>(StringDataSize, s_CachedBytes);
        }

        private void WriteStringData(int stringIndex, StringData stringData)
        {
            Utility.Marshal.StructureToBytes(stringData, StringDataSize, s_CachedBytes);
            m_Stream.Position = m_StringDataOffset + StringDataSize * stringIndex;
            m_Stream.Write(s_CachedBytes, 0, StringDataSize);
        }

        private static void CalcOffsets(FileSystem fileSystem)
        {
            fileSystem.m_BlockDataOffset = HeaderDataSize;
            fileSystem.m_StringDataOffset = fileSystem.m_BlockDataOffset + BlockDataSize * fileSystem.m_HeaderData.MaxBlockCount;
            fileSystem.m_FileDataOffset = (int)GetUpBoundClusterOffset(fileSystem.m_StringDataOffset + StringDataSize * fileSystem.m_HeaderData.MaxFileCount);
        }

        private static long GetUpBoundClusterOffset(long offset)
        {
            return (offset - 1L + ClusterSize) / ClusterSize * ClusterSize;
        }

        private static int GetUpBoundClusterCount(long length)
        {
            return (int)((length - 1L + ClusterSize) / ClusterSize);
        }

        private static long GetClusterOffset(int clusterIndex)
        {
            return (long)ClusterSize * clusterIndex;
        }
    }
}
