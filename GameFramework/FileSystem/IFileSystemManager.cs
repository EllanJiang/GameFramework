//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System.Collections.Generic;

namespace GameFramework.FileSystem
{
    /// <summary>
    /// 文件系统管理器。
    /// </summary>
    public interface IFileSystemManager
    {
        /// <summary>
        /// 获取文件系统数量。
        /// </summary>
        int Count
        {
            get;
        }

        /// <summary>
        /// 设置文件系统辅助器。
        /// </summary>
        /// <param name="fileSystemHelper">文件系统辅助器。</param>
        void SetFileSystemHelper(IFileSystemHelper fileSystemHelper);

        /// <summary>
        /// 检查是否存在文件系统。
        /// </summary>
        /// <param name="fullPath">要检查的文件系统的完整路径。</param>
        /// <returns>是否存在文件系统。</returns>
        bool HasFileSystem(string fullPath);

        /// <summary>
        /// 获取文件系统。
        /// </summary>
        /// <param name="fullPath">要获取的文件系统的完整路径。</param>
        /// <returns>获取的文件系统。</returns>
        IFileSystem GetFileSystem(string fullPath);

        /// <summary>
        /// 创建文件系统。
        /// </summary>
        /// <param name="fullPath">要创建的文件系统的完整路径。</param>
        /// <param name="access">要创建的文件系统的访问方式。</param>
        /// <param name="maxFileCount">要创建的文件系统的最大文件数量。</param>
        /// <param name="maxBlockCount">要创建的文件系统的最大块数据数量。</param>
        /// <returns>创建的文件系统。</returns>
        IFileSystem CreateFileSystem(string fullPath, FileSystemAccess access, int maxFileCount, int maxBlockCount);

        /// <summary>
        /// 加载文件系统。
        /// </summary>
        /// <param name="fullPath">要加载的文件系统的完整路径。</param>
        /// <param name="access">要加载的文件系统的访问方式。</param>
        /// <returns>加载的文件系统。</returns>
        IFileSystem LoadFileSystem(string fullPath, FileSystemAccess access);

        /// <summary>
        /// 销毁文件系统。
        /// </summary>
        /// <param name="fileSystem">要销毁的文件系统。</param>
        /// <param name="deletePhysicalFile">是否删除文件系统对应的物理文件。</param>
        void DestroyFileSystem(IFileSystem fileSystem, bool deletePhysicalFile);

        /// <summary>
        /// 获取所有文件系统集合。
        /// </summary>
        /// <returns>获取的所有文件系统集合。</returns>
        IFileSystem[] GetAllFileSystems();

        /// <summary>
        /// 获取所有文件系统集合。
        /// </summary>
        /// <param name="results">获取的所有文件系统集合。</param>
        void GetAllFileSystems(List<IFileSystem> results);
    }
}
