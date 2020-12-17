//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System.Collections.Generic;
using System.IO;

namespace GameFramework.FileSystem
{
    /// <summary>
    /// 文件系统接口。
    /// </summary>
    public interface IFileSystem
    {
        /// <summary>
        /// 获取文件系统完整路径。
        /// </summary>
        string FullPath
        {
            get;
        }

        /// <summary>
        /// 获取文件系统访问方式。
        /// </summary>
        FileSystemAccess Access
        {
            get;
        }

        /// <summary>
        /// 获取文件数量。
        /// </summary>
        int FileCount
        {
            get;
        }

        /// <summary>
        /// 获取最大文件数量。
        /// </summary>
        int MaxFileCount
        {
            get;
        }

        /// <summary>
        /// 获取文件信息。
        /// </summary>
        /// <param name="name">要获取文件信息的文件名称。</param>
        /// <returns>获取的文件信息。</returns>
        FileInfo GetFileInfo(string name);

        /// <summary>
        /// 获取所有文件信息。
        /// </summary>
        /// <returns>获取的所有文件信息。</returns>
        FileInfo[] GetAllFileInfos();

        /// <summary>
        /// 获取所有文件信息。
        /// </summary>
        /// <param name="results">获取的所有文件信息。</param>
        void GetAllFileInfos(List<FileInfo> results);

        /// <summary>
        /// 检查是否存在指定文件。
        /// </summary>
        /// <param name="name">要检查的文件名称。</param>
        /// <returns>是否存在指定文件。</returns>
        bool HasFile(string name);

        /// <summary>
        /// 读取指定文件。
        /// </summary>
        /// <param name="name">要读取的文件名称。</param>
        /// <returns>存储读取文件内容的二进制流。</returns>
        byte[] ReadFile(string name);

        /// <summary>
        /// 读取指定文件。
        /// </summary>
        /// <param name="name">要读取的文件名称。</param>
        /// <param name="buffer">存储读取文件内容的二进制流。</param>
        /// <returns>实际读取了多少字节。</returns>
        int ReadFile(string name, byte[] buffer);

        /// <summary>
        /// 读取指定文件。
        /// </summary>
        /// <param name="name">要读取的文件名称。</param>
        /// <param name="buffer">存储读取文件内容的二进制流。</param>
        /// <param name="startIndex">存储读取文件内容的二进制流的起始位置。</param>
        /// <returns>实际读取了多少字节。</returns>
        int ReadFile(string name, byte[] buffer, int startIndex);

        /// <summary>
        /// 读取指定文件。
        /// </summary>
        /// <param name="name">要读取的文件名称。</param>
        /// <param name="buffer">存储读取文件内容的二进制流。</param>
        /// <param name="startIndex">存储读取文件内容的二进制流的起始位置。</param>
        /// <param name="length">存储读取文件内容的二进制流的长度。</param>
        /// <returns>实际读取了多少字节。</returns>
        int ReadFile(string name, byte[] buffer, int startIndex, int length);

        /// <summary>
        /// 读取指定文件。
        /// </summary>
        /// <param name="name">要读取的文件名称。</param>
        /// <param name="stream">存储读取文件内容的二进制流。</param>
        /// <returns>实际读取了多少字节。</returns>
        int ReadFile(string name, Stream stream);

        /// <summary>
        /// 读取指定文件的指定片段。
        /// </summary>
        /// <param name="name">要读取片段的文件名称。</param>
        /// <param name="length">要读取片段的长度。</param>
        /// <returns>存储读取文件片段内容的二进制流。</returns>
        byte[] ReadFileSegment(string name, int length);

        /// <summary>
        /// 读取指定文件的指定片段。
        /// </summary>
        /// <param name="name">要读取片段的文件名称。</param>
        /// <param name="offset">要读取片段的偏移。</param>
        /// <param name="length">要读取片段的长度。</param>
        /// <returns>存储读取文件片段内容的二进制流。</returns>
        byte[] ReadFileSegment(string name, int offset, int length);

        /// <summary>
        /// 读取指定文件的指定片段。
        /// </summary>
        /// <param name="name">要读取片段的文件名称。</param>
        /// <param name="buffer">存储读取文件片段内容的二进制流。</param>
        /// <returns>实际读取了多少字节。</returns>
        int ReadFileSegment(string name, byte[] buffer);

        /// <summary>
        /// 读取指定文件的指定片段。
        /// </summary>
        /// <param name="name">要读取片段的文件名称。</param>
        /// <param name="buffer">存储读取文件片段内容的二进制流。</param>
        /// <param name="length">要读取片段的长度。</param>
        /// <returns>实际读取了多少字节。</returns>
        int ReadFileSegment(string name, byte[] buffer, int length);

        /// <summary>
        /// 读取指定文件的指定片段。
        /// </summary>
        /// <param name="name">要读取片段的文件名称。</param>
        /// <param name="buffer">存储读取文件片段内容的二进制流。</param>
        /// <param name="startIndex">存储读取文件片段内容的二进制流的起始位置。</param>
        /// <param name="length">要读取片段的长度。</param>
        /// <returns>实际读取了多少字节。</returns>
        int ReadFileSegment(string name, byte[] buffer, int startIndex, int length);

        /// <summary>
        /// 读取指定文件的指定片段。
        /// </summary>
        /// <param name="name">要读取片段的文件名称。</param>
        /// <param name="offset">要读取片段的偏移。</param>
        /// <param name="buffer">存储读取文件片段内容的二进制流。</param>
        /// <returns>实际读取了多少字节。</returns>
        int ReadFileSegment(string name, int offset, byte[] buffer);

        /// <summary>
        /// 读取指定文件的指定片段。
        /// </summary>
        /// <param name="name">要读取片段的文件名称。</param>
        /// <param name="offset">要读取片段的偏移。</param>
        /// <param name="buffer">存储读取文件片段内容的二进制流。</param>
        /// <param name="length">要读取片段的长度。</param>
        /// <returns>实际读取了多少字节。</returns>
        int ReadFileSegment(string name, int offset, byte[] buffer, int length);

        /// <summary>
        /// 读取指定文件的指定片段。
        /// </summary>
        /// <param name="name">要读取片段的文件名称。</param>
        /// <param name="offset">要读取片段的偏移。</param>
        /// <param name="buffer">存储读取文件片段内容的二进制流。</param>
        /// <param name="startIndex">存储读取文件片段内容的二进制流的起始位置。</param>
        /// <param name="length">要读取片段的长度。</param>
        /// <returns>实际读取了多少字节。</returns>
        int ReadFileSegment(string name, int offset, byte[] buffer, int startIndex, int length);

        /// <summary>
        /// 读取指定文件的指定片段。
        /// </summary>
        /// <param name="name">要读取片段的文件名称。</param>
        /// <param name="stream">存储读取文件片段内容的二进制流。</param>
        /// <param name="length">要读取片段的长度。</param>
        /// <returns>实际读取了多少字节。</returns>
        int ReadFileSegment(string name, Stream stream, int length);

        /// <summary>
        /// 读取指定文件的指定片段。
        /// </summary>
        /// <param name="name">要读取片段的文件名称。</param>
        /// <param name="offset">要读取片段的偏移。</param>
        /// <param name="stream">存储读取文件片段内容的二进制流。</param>
        /// <param name="length">要读取片段的长度。</param>
        /// <returns>实际读取了多少字节。</returns>
        int ReadFileSegment(string name, int offset, Stream stream, int length);

        /// <summary>
        /// 写入指定文件。
        /// </summary>
        /// <param name="name">要写入的文件名称。</param>
        /// <param name="buffer">存储写入文件内容的二进制流。</param>
        /// <returns>是否写入指定文件成功。</returns>
        bool WriteFile(string name, byte[] buffer);

        /// <summary>
        /// 写入指定文件。
        /// </summary>
        /// <param name="name">要写入的文件名称。</param>
        /// <param name="buffer">存储写入文件内容的二进制流。</param>
        /// <param name="startIndex">存储写入文件内容的二进制流的起始位置。</param>
        /// <returns>是否写入指定文件成功。</returns>
        bool WriteFile(string name, byte[] buffer, int startIndex);

        /// <summary>
        /// 写入指定文件。
        /// </summary>
        /// <param name="name">要写入的文件名称。</param>
        /// <param name="buffer">存储写入文件内容的二进制流。</param>
        /// <param name="startIndex">存储写入文件内容的二进制流的起始位置。</param>
        /// <param name="length">存储写入文件内容的二进制流的长度。</param>
        /// <returns>是否写入指定文件成功。</returns>
        bool WriteFile(string name, byte[] buffer, int startIndex, int length);

        /// <summary>
        /// 写入指定文件。
        /// </summary>
        /// <param name="name">要写入的文件名称。</param>
        /// <param name="stream">存储写入文件内容的二进制流。</param>
        /// <returns>是否写入指定文件成功。</returns>
        bool WriteFile(string name, Stream stream);

        /// <summary>
        /// 写入指定文件。
        /// </summary>
        /// <param name="name">要写入的文件名称。</param>
        /// <param name="filePath">存储写入文件内容的文件路径。</param>
        /// <returns>是否写入指定文件成功。</returns>
        bool WriteFile(string name, string filePath);

        /// <summary>
        /// 将指定文件另存为物理文件。
        /// </summary>
        /// <param name="name">要另存为的文件名称。</param>
        /// <param name="filePath">存储写入文件内容的文件路径。</param>
        /// <returns>是否将指定文件另存为物理文件成功。</returns>
        bool SaveAsFile(string name, string filePath);

        /// <summary>
        /// 重命名指定文件。
        /// </summary>
        /// <param name="oldName">要重命名的文件名称。</param>
        /// <param name="newName">重命名后的文件名称。</param>
        /// <returns>是否重命名指定文件成功。</returns>
        bool RenameFile(string oldName, string newName);

        /// <summary>
        /// 删除指定文件。
        /// </summary>
        /// <param name="name">要删除的文件名称。</param>
        /// <returns>是否删除指定文件成功。</returns>
        bool DeleteFile(string name);
    }
}
