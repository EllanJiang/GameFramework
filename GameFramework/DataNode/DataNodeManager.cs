//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;

namespace GameFramework.DataNode
{
    /// <summary>
    /// 数据结点管理器。
    /// </summary>
    internal sealed partial class DataNodeManager : GameFrameworkModule, IDataNodeManager
    {
        private static readonly string[] EmptyStringArray = new string[] { };
        private static readonly string[] PathSplitSeparator = new string[] { ".", "/", "\\" };

        private const string RootName = "<Root>";
        private DataNode m_Root;

        /// <summary>
        /// 初始化数据结点管理器的新实例。
        /// </summary>
        public DataNodeManager()
        {
            m_Root = DataNode.Create(RootName, null);
        }

        /// <summary>
        /// 获取根数据结点。
        /// </summary>
        public IDataNode Root
        {
            get
            {
                return m_Root;
            }
        }

        /// <summary>
        /// 数据结点管理器轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {
        }

        /// <summary>
        /// 关闭并清理数据结点管理器。
        /// </summary>
        internal override void Shutdown()
        {
            ReferencePool.Release(m_Root);
            m_Root = null;
        }

        /// <summary>
        /// 根据类型获取数据结点的数据。
        /// </summary>
        /// <typeparam name="T">要获取的数据类型。</typeparam>
        /// <param name="path">相对于 node 的查找路径。</param>
        /// <returns>指定类型的数据。</returns>
        public T GetData<T>(string path) where T : Variable
        {
            return GetData<T>(path, null);
        }

        /// <summary>
        /// 获取数据结点的数据。
        /// </summary>
        /// <param name="path">相对于 node 的查找路径。</param>
        /// <returns>数据结点的数据。</returns>
        public Variable GetData(string path)
        {
            return GetData(path, null);
        }

        /// <summary>
        /// 根据类型获取数据结点的数据。
        /// </summary>
        /// <typeparam name="T">要获取的数据类型。</typeparam>
        /// <param name="path">相对于 node 的查找路径。</param>
        /// <param name="node">查找起始结点。</param>
        /// <returns>指定类型的数据。</returns>
        public T GetData<T>(string path, IDataNode node) where T : Variable
        {
            IDataNode current = GetNode(path, node);
            if (current == null)
            {
                throw new GameFrameworkException(Utility.Text.Format("Data node is not exist, path '{0}', node '{1}'.", path, node != null ? node.FullName : string.Empty));
            }

            return current.GetData<T>();
        }

        /// <summary>
        /// 获取数据结点的数据。
        /// </summary>
        /// <param name="path">相对于 node 的查找路径。</param>
        /// <param name="node">查找起始结点。</param>
        /// <returns>数据结点的数据。</returns>
        public Variable GetData(string path, IDataNode node)
        {
            IDataNode current = GetNode(path, node);
            if (current == null)
            {
                throw new GameFrameworkException(Utility.Text.Format("Data node is not exist, path '{0}', node '{1}'.", path, node != null ? node.FullName : string.Empty));
            }

            return current.GetData();
        }

        /// <summary>
        /// 设置数据结点的数据。
        /// </summary>
        /// <typeparam name="T">要设置的数据类型。</typeparam>
        /// <param name="path">相对于 node 的查找路径。</param>
        /// <param name="data">要设置的数据。</param>
        public void SetData<T>(string path, T data) where T : Variable
        {
            SetData(path, data, null);
        }

        /// <summary>
        /// 设置数据结点的数据。
        /// </summary>
        /// <param name="path">相对于 node 的查找路径。</param>
        /// <param name="data">要设置的数据。</param>
        public void SetData(string path, Variable data)
        {
            SetData(path, data, null);
        }

        /// <summary>
        /// 设置数据结点的数据。
        /// </summary>
        /// <typeparam name="T">要设置的数据类型。</typeparam>
        /// <param name="path">相对于 node 的查找路径。</param>
        /// <param name="data">要设置的数据。</param>
        /// <param name="node">查找起始结点。</param>
        public void SetData<T>(string path, T data, IDataNode node) where T : Variable
        {
            IDataNode current = GetOrAddNode(path, node);
            current.SetData(data);
        }

        /// <summary>
        /// 设置数据结点的数据。
        /// </summary>
        /// <param name="path">相对于 node 的查找路径。</param>
        /// <param name="data">要设置的数据。</param>
        /// <param name="node">查找起始结点。</param>
        public void SetData(string path, Variable data, IDataNode node)
        {
            IDataNode current = GetOrAddNode(path, node);
            current.SetData(data);
        }

        /// <summary>
        /// 获取数据结点。
        /// </summary>
        /// <param name="path">相对于 node 的查找路径。</param>
        /// <returns>指定位置的数据结点，如果没有找到，则返回空。</returns>
        public IDataNode GetNode(string path)
        {
            return GetNode(path, null);
        }

        /// <summary>
        /// 获取数据结点。
        /// </summary>
        /// <param name="path">相对于 node 的查找路径。</param>
        /// <param name="node">查找起始结点。</param>
        /// <returns>指定位置的数据结点，如果没有找到，则返回空。</returns>
        public IDataNode GetNode(string path, IDataNode node)
        {
            IDataNode current = node ?? m_Root;
            string[] splitedPath = GetSplitedPath(path);
            foreach (string i in splitedPath)
            {
                current = current.GetChild(i);
                if (current == null)
                {
                    return null;
                }
            }

            return current;
        }

        /// <summary>
        /// 获取或增加数据结点。
        /// </summary>
        /// <param name="path">相对于 node 的查找路径。</param>
        /// <returns>指定位置的数据结点，如果没有找到，则创建相应的数据结点。</returns>
        public IDataNode GetOrAddNode(string path)
        {
            return GetOrAddNode(path, null);
        }

        /// <summary>
        /// 获取或增加数据结点。
        /// </summary>
        /// <param name="path">相对于 node 的查找路径。</param>
        /// <param name="node">查找起始结点。</param>
        /// <returns>指定位置的数据结点，如果没有找到，则增加相应的数据结点。</returns>
        public IDataNode GetOrAddNode(string path, IDataNode node)
        {
            IDataNode current = node ?? m_Root;
            string[] splitedPath = GetSplitedPath(path);
            foreach (string i in splitedPath)
            {
                current = current.GetOrAddChild(i);
            }

            return current;
        }

        /// <summary>
        /// 移除数据结点。
        /// </summary>
        /// <param name="path">相对于 node 的查找路径。</param>
        public void RemoveNode(string path)
        {
            RemoveNode(path, null);
        }

        /// <summary>
        /// 移除数据结点。
        /// </summary>
        /// <param name="path">相对于 node 的查找路径。</param>
        /// <param name="node">查找起始结点。</param>
        public void RemoveNode(string path, IDataNode node)
        {
            IDataNode current = node ?? m_Root;
            IDataNode parent = current.Parent;
            string[] splitedPath = GetSplitedPath(path);
            foreach (string i in splitedPath)
            {
                parent = current;
                current = current.GetChild(i);
                if (current == null)
                {
                    return;
                }
            }

            if (parent != null)
            {
                parent.RemoveChild(current.Name);
            }
        }

        /// <summary>
        /// 移除所有数据结点。
        /// </summary>
        public void Clear()
        {
            m_Root.Clear();
        }

        /// <summary>
        /// 数据结点路径切分工具函数。
        /// </summary>
        /// <param name="path">要切分的数据结点路径。</param>
        /// <returns>切分后的字符串数组。</returns>
        private static string[] GetSplitedPath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return EmptyStringArray;
            }

            return path.Split(PathSplitSeparator, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
