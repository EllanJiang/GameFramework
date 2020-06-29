//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System.Collections.Generic;
using System.IO;

namespace GameFramework.FileSystem
{
    /// <summary>
    /// 文件系统管理器。
    /// </summary>
    internal sealed class FileSystemManager : GameFrameworkModule, IFileSystemManager
    {
        private readonly Dictionary<string, FileSystem> m_FileSystems;
        private readonly GameFrameworkMultiDictionary<string, FileSystem> m_RegisteredFileSystems;

        /// <summary>
        /// 初始化文件系统管理器的新实例。
        /// </summary>
        public FileSystemManager()
        {
            m_FileSystems = new Dictionary<string, FileSystem>();
            m_RegisteredFileSystems = new GameFrameworkMultiDictionary<string, FileSystem>();
        }

        /// <summary>
        /// 获取文件系统数量。
        /// </summary>
        public int Count
        {
            get
            {
                return m_FileSystems.Count;
            }
        }

        /// <summary>
        /// 文件系统管理器轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {
        }

        /// <summary>
        /// 关闭并清理文件系统管理器。
        /// </summary>
        internal override void Shutdown()
        {
            m_RegisteredFileSystems.Clear();
            while (m_FileSystems.Count > 0)
            {
                foreach (KeyValuePair<string, FileSystem> fileSystem in m_FileSystems)
                {
                    DestroyFileSystem(fileSystem.Value, false);
                    break;
                }
            }
        }

        /// <summary>
        /// 创建文件系统。
        /// </summary>
        /// <param name="fullPath">要创建的文件系统的完整路径。</param>
        /// <param name="access">要创建的文件系统的访问方式。</param>
        /// <param name="maxFileCount">要创建的文件系统的最大文件数量。</param>
        /// <param name="maxBlockCount">要创建的文件系统的最大块数据数量。</param>
        /// <returns>创建的文件系统。</returns>
        public IFileSystem CreateFileSystem(string fullPath, FileSystemAccess access, int maxFileCount, int maxBlockCount)
        {
            if (string.IsNullOrEmpty(fullPath))
            {
                throw new GameFrameworkException("Full path is invalid.");
            }

            if (access == FileSystemAccess.Unspecified)
            {
                throw new GameFrameworkException("Access is invalid.");
            }

            if (access == FileSystemAccess.Read)
            {
                throw new GameFrameworkException("Access read is invalid.");
            }

            fullPath = Utility.Path.GetRegularPath(fullPath);
            if (m_FileSystems.ContainsKey(fullPath))
            {
                throw new GameFrameworkException(Utility.Text.Format("File system '{0}' is already exist.", fullPath));
            }

            FileSystemStream fileSystemStream = new DotNetFileSystemStream(fullPath, access, true);
            FileSystem fileSystem = FileSystem.Create(fullPath, access, fileSystemStream, maxFileCount, maxBlockCount);
            if (fileSystem == null)
            {
                throw new GameFrameworkException(Utility.Text.Format("Create file system '{0}' failure.", fullPath));
            }

            m_FileSystems.Add(fullPath, fileSystem);
            return fileSystem;
        }

        /// <summary>
        /// 加载文件系统。
        /// </summary>
        /// <param name="fullPath">要加载的文件系统的完整路径。</param>
        /// <param name="access">要加载的文件系统的访问方式。</param>
        /// <returns>加载的文件系统。</returns>
        public IFileSystem LoadFileSystem(string fullPath, FileSystemAccess access)
        {
            if (string.IsNullOrEmpty(fullPath))
            {
                throw new GameFrameworkException("Full path is invalid.");
            }

            if (access == FileSystemAccess.Unspecified)
            {
                throw new GameFrameworkException("Access is invalid.");
            }

            fullPath = Utility.Path.GetRegularPath(fullPath);
            if (m_FileSystems.ContainsKey(fullPath))
            {
                throw new GameFrameworkException(Utility.Text.Format("File system '{0}' is already exist.", fullPath));
            }

            FileSystemStream fileSystemStream = new DotNetFileSystemStream(fullPath, access, false);
            FileSystem fileSystem = FileSystem.Load(fullPath, access, fileSystemStream);
            if (fileSystem == null)
            {
                throw new GameFrameworkException(Utility.Text.Format("Load file system '{0}' failure.", fullPath));
            }

            m_FileSystems.Add(fullPath, fileSystem);
            return fileSystem;
        }

        /// <summary>
        /// 销毁文件系统。
        /// </summary>
        /// <param name="fileSystem">要销毁的文件系统。</param>
        /// <param name="deletePhysicalFile">是否删除文件系统对应的物理文件。</param>
        public void DestroyFileSystem(IFileSystem fileSystem, bool deletePhysicalFile)
        {
            if (fileSystem == null)
            {
                throw new GameFrameworkException("File system is invalid.");
            }

            FileSystem fileSystemImpl = (FileSystem)fileSystem;
            string[] names = fileSystemImpl.GetNames();
            foreach (string name in names)
            {
                UnregisterFileSystem(name, fileSystemImpl);
            }

            string fullPath = fileSystemImpl.FullPath;
            fileSystemImpl.Shutdown();
            m_FileSystems.Remove(fullPath);

            if (deletePhysicalFile && File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }

        /// <summary>
        /// 注册文件系统。
        /// </summary>
        /// <param name="name">要注册的文件系统的名称。</param>
        /// <param name="fileSystem">要注册的文件系统。</param>
        /// <returns>注册的文件系统是否成功。</returns>
        public bool RegisterFileSystem(string name, IFileSystem fileSystem)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new GameFrameworkException("Name is invalid.");
            }

            if (fileSystem == null)
            {
                throw new GameFrameworkException("File system is invalid.");
            }

            FileSystem fileSystemImpl = (FileSystem)fileSystem;
            if (!fileSystemImpl.AddName(name))
            {
                return false;
            }

            m_RegisteredFileSystems.Add(name, fileSystemImpl);
            return true;
        }

        /// <summary>
        /// 解除注册文件系统。
        /// </summary>
        /// <param name="name">要解除注册的文件系统的名称。</param>
        /// <param name="fileSystem">要解除注册的文件系统。</param>
        /// <returns>解除注册的文件系统是否成功。</returns>
        public bool UnregisterFileSystem(string name, IFileSystem fileSystem)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new GameFrameworkException("Name is invalid.");
            }

            if (fileSystem == null)
            {
                throw new GameFrameworkException("File system is invalid.");
            }

            FileSystem fileSystemImpl = (FileSystem)fileSystem;
            if (!fileSystemImpl.RemoveName(name))
            {
                return false;
            }

            return m_RegisteredFileSystems.Remove(name, fileSystemImpl);
        }

        /// <summary>
        /// 获取文件系统。
        /// </summary>
        /// <param name="name">要获取的文件系统的名称。</param>
        /// <returns>获取的文件系统。</returns>
        public IFileSystem GetFileSystem(string name)
        {
            return GetFileSystem(name, FileSystemAccess.Unspecified);
        }

        /// <summary>
        /// 获取文件系统。
        /// </summary>
        /// <param name="name">要获取的文件系统的名称。</param>
        /// <param name="access">要获取的文件系统的访问方式。</param>
        /// <returns>获取的文件系统。</returns>
        public IFileSystem GetFileSystem(string name, FileSystemAccess access)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new GameFrameworkException("Name is invalid.");
            }

            GameFrameworkLinkedListRange<FileSystem> range = default(GameFrameworkLinkedListRange<FileSystem>);
            if (m_RegisteredFileSystems.TryGetValue(name, out range))
            {
                foreach (FileSystem fileSystem in range)
                {
                    if (access != FileSystemAccess.Unspecified && (fileSystem.Access & access) != access)
                    {
                        continue;
                    }

                    return fileSystem;
                }
            }

            return null;
        }

        /// <summary>
        /// 获取文件系统集合。
        /// </summary>
        /// <param name="name">要获取的文件系统的名称。</param>
        /// <returns>获取的文件系统集合。</returns>
        public IFileSystem[] GetFileSystems(string name)
        {
            List<IFileSystem> results = new List<IFileSystem>();
            GetFileSystems(name, FileSystemAccess.Unspecified, results);
            return results.ToArray();
        }

        /// <summary>
        /// 获取文件系统集合。
        /// </summary>
        /// <param name="name">要获取的文件系统的名称。</param>
        /// <param name="access">要获取的文件系统的访问方式。</param>
        /// <returns>获取的文件系统集合。</returns>
        public IFileSystem[] GetFileSystems(string name, FileSystemAccess access)
        {
            List<IFileSystem> results = new List<IFileSystem>();
            GetFileSystems(name, access, results);
            return results.ToArray();
        }

        /// <summary>
        /// 获取文件系统集合。
        /// </summary>
        /// <param name="name">要获取的文件系统的名称。</param>
        /// <param name="results">获取的文件系统集合。</param>
        public void GetFileSystems(string name, List<IFileSystem> results)
        {
            GetFileSystems(name, FileSystemAccess.Unspecified, results);
        }

        /// <summary>
        /// 获取文件系统集合。
        /// </summary>
        /// <param name="name">要获取的文件系统的名称。</param>
        /// <param name="access">要获取的文件系统的访问方式。</param>
        /// <param name="results">获取的文件系统集合。</param>
        public void GetFileSystems(string name, FileSystemAccess access, List<IFileSystem> results)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new GameFrameworkException("Name is invalid.");
            }

            if (results == null)
            {
                throw new GameFrameworkException("Results is invalid.");
            }

            results.Clear();
            GameFrameworkLinkedListRange<FileSystem> range = default(GameFrameworkLinkedListRange<FileSystem>);
            if (m_RegisteredFileSystems.TryGetValue(name, out range))
            {
                foreach (FileSystem fileSystem in range)
                {
                    if (access != FileSystemAccess.Unspecified && (fileSystem.Access & access) != access)
                    {
                        continue;
                    }

                    results.Add(fileSystem);
                }
            }
        }

        /// <summary>
        /// 获取所有文件系统集合。
        /// </summary>
        /// <returns>获取的所有文件系统集合。</returns>
        public IFileSystem[] GetAllFileSystems()
        {
            int index = 0;
            IFileSystem[] results = new IFileSystem[m_FileSystems.Count];
            foreach (KeyValuePair<string, FileSystem> fileSystem in m_FileSystems)
            {
                results[index++] = fileSystem.Value;
            }

            return results;
        }

        /// <summary>
        /// 获取所有文件系统集合。
        /// </summary>
        /// <param name="results">获取的所有文件系统集合。</param>
        public void GetAllFileSystems(List<IFileSystem> results)
        {
            if (results == null)
            {
                throw new GameFrameworkException("Results is invalid.");
            }

            results.Clear();
            foreach (KeyValuePair<string, FileSystem> fileSystem in m_FileSystems)
            {
                results.Add(fileSystem.Value);
            }
        }
    }
}
