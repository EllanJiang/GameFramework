//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.DataNode
{
    /// <summary>
    /// 数据结点管理器接口。
    /// </summary>
    public interface IDataNodeManager
    {
        /// <summary>
        /// 获取根数据结点。
        /// </summary>
        IDataNode Root
        {
            get;
        }

        /// <summary>
        /// 根据类型获取数据结点的数据。
        /// </summary>
        /// <typeparam name="T">要获取的数据类型。</typeparam>
        /// <param name="path">相对于 node 的查找路径。</param>
        /// <returns>指定类型的数据。</returns>
        T GetData<T>(string path) where T : Variable;

        /// <summary>
        /// 获取数据结点的数据。
        /// </summary>
        /// <param name="path">相对于 node 的查找路径。</param>
        /// <returns>数据结点的数据。</returns>
        Variable GetData(string path);

        /// <summary>
        /// 根据类型获取数据结点的数据。
        /// </summary>
        /// <typeparam name="T">要获取的数据类型。</typeparam>
        /// <param name="path">相对于 node 的查找路径。</param>
        /// <param name="node">查找起始结点。</param>
        /// <returns>指定类型的数据。</returns>
        T GetData<T>(string path, IDataNode node) where T : Variable;

        /// <summary>
        /// 获取数据结点的数据。
        /// </summary>
        /// <param name="path">相对于 node 的查找路径。</param>
        /// <param name="node">查找起始结点。</param>
        /// <returns>数据结点的数据。</returns>
        Variable GetData(string path, IDataNode node);

        /// <summary>
        /// 设置数据结点的数据。
        /// </summary>
        /// <typeparam name="T">要设置的数据类型。</typeparam>
        /// <param name="path">相对于 node 的查找路径。</param>
        /// <param name="data">要设置的数据。</param>
        void SetData<T>(string path, T data) where T : Variable;

        /// <summary>
        /// 设置数据结点的数据。
        /// </summary>
        /// <param name="path">相对于 node 的查找路径。</param>
        /// <param name="data">要设置的数据。</param>
        void SetData(string path, Variable data);

        /// <summary>
        /// 设置数据结点的数据。
        /// </summary>
        /// <typeparam name="T">要设置的数据类型。</typeparam>
        /// <param name="path">相对于 node 的查找路径。</param>
        /// <param name="data">要设置的数据。</param>
        /// <param name="node">查找起始结点。</param>
        void SetData<T>(string path, T data, IDataNode node) where T : Variable;

        /// <summary>
        /// 设置数据结点的数据。
        /// </summary>
        /// <param name="path">相对于 node 的查找路径。</param>
        /// <param name="data">要设置的数据。</param>
        /// <param name="node">查找起始结点。</param>
        void SetData(string path, Variable data, IDataNode node);

        /// <summary>
        /// 获取数据结点。
        /// </summary>
        /// <param name="path">相对于 node 的查找路径。</param>
        /// <returns>指定位置的数据结点，如果没有找到，则返回空。</returns>
        IDataNode GetNode(string path);

        /// <summary>
        /// 获取数据结点。
        /// </summary>
        /// <param name="path">相对于 node 的查找路径。</param>
        /// <param name="node">查找起始结点。</param>
        /// <returns>指定位置的数据结点，如果没有找到，则返回空。</returns>
        IDataNode GetNode(string path, IDataNode node);

        /// <summary>
        /// 获取或增加数据结点。
        /// </summary>
        /// <param name="path">相对于 node 的查找路径。</param>
        /// <returns>指定位置的数据结点，如果没有找到，则创建相应的数据结点。</returns>
        IDataNode GetOrAddNode(string path);

        /// <summary>
        /// 获取或增加数据结点。
        /// </summary>
        /// <param name="path">相对于 node 的查找路径。</param>
        /// <param name="node">查找起始结点。</param>
        /// <returns>指定位置的数据结点，如果没有找到，则创建相应的数据结点。</returns>
        IDataNode GetOrAddNode(string path, IDataNode node);

        /// <summary>
        /// 移除数据结点。
        /// </summary>
        /// <param name="path">相对于 node 的查找路径。</param>
        void RemoveNode(string path);

        /// <summary>
        /// 移除数据结点。
        /// </summary>
        /// <param name="path">相对于 node 的查找路径。</param>
        /// <param name="node">查找起始结点。</param>
        void RemoveNode(string path, IDataNode node);

        /// <summary>
        /// 移除所有数据结点。
        /// </summary>
        void Clear();
    }
}
