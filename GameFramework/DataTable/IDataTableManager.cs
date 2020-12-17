//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework.Resource;
using System;
using System.Collections.Generic;

namespace GameFramework.DataTable
{
    /// <summary>
    /// 数据表管理器接口。
    /// </summary>
    public interface IDataTableManager
    {
        /// <summary>
        /// 获取数据表数量。
        /// </summary>
        int Count
        {
            get;
        }

        /// <summary>
        /// 获取缓冲二进制流的大小。
        /// </summary>
        int CachedBytesSize
        {
            get;
        }

        /// <summary>
        /// 设置资源管理器。
        /// </summary>
        /// <param name="resourceManager">资源管理器。</param>
        void SetResourceManager(IResourceManager resourceManager);

        /// <summary>
        /// 设置数据表数据提供者辅助器。
        /// </summary>
        /// <param name="dataProviderHelper">数据表数据提供者辅助器。</param>
        void SetDataProviderHelper(IDataProviderHelper<DataTableBase> dataProviderHelper);

        /// <summary>
        /// 设置数据表辅助器。
        /// </summary>
        /// <param name="dataTableHelper">数据表辅助器。</param>
        void SetDataTableHelper(IDataTableHelper dataTableHelper);

        /// <summary>
        /// 确保二进制流缓存分配足够大小的内存并缓存。
        /// </summary>
        /// <param name="ensureSize">要确保二进制流缓存分配内存的大小。</param>
        void EnsureCachedBytesSize(int ensureSize);

        /// <summary>
        /// 释放缓存的二进制流。
        /// </summary>
        void FreeCachedBytes();

        /// <summary>
        /// 是否存在数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <returns>是否存在数据表。</returns>
        bool HasDataTable<T>() where T : IDataRow;

        /// <summary>
        /// 是否存在数据表。
        /// </summary>
        /// <param name="dataRowType">数据表行的类型。</param>
        /// <returns>是否存在数据表。</returns>
        bool HasDataTable(Type dataRowType);

        /// <summary>
        /// 是否存在数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <param name="name">数据表名称。</param>
        /// <returns>是否存在数据表。</returns>
        bool HasDataTable<T>(string name) where T : IDataRow;

        /// <summary>
        /// 是否存在数据表。
        /// </summary>
        /// <param name="dataRowType">数据表行的类型。</param>
        /// <param name="name">数据表名称。</param>
        /// <returns>是否存在数据表。</returns>
        bool HasDataTable(Type dataRowType, string name);

        /// <summary>
        /// 获取数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <returns>要获取的数据表。</returns>
        IDataTable<T> GetDataTable<T>() where T : IDataRow;

        /// <summary>
        /// 获取数据表。
        /// </summary>
        /// <param name="dataRowType">数据表行的类型。</param>
        /// <returns>要获取的数据表。</returns>
        DataTableBase GetDataTable(Type dataRowType);

        /// <summary>
        /// 获取数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <param name="name">数据表名称。</param>
        /// <returns>要获取的数据表。</returns>
        IDataTable<T> GetDataTable<T>(string name) where T : IDataRow;

        /// <summary>
        /// 获取数据表。
        /// </summary>
        /// <param name="dataRowType">数据表行的类型。</param>
        /// <param name="name">数据表名称。</param>
        /// <returns>要获取的数据表。</returns>
        DataTableBase GetDataTable(Type dataRowType, string name);

        /// <summary>
        /// 获取所有数据表。
        /// </summary>
        /// <returns>所有数据表。</returns>
        DataTableBase[] GetAllDataTables();

        /// <summary>
        /// 获取所有数据表。
        /// </summary>
        /// <param name="results">所有数据表。</param>
        void GetAllDataTables(List<DataTableBase> results);

        /// <summary>
        /// 创建数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <returns>要创建的数据表。</returns>
        IDataTable<T> CreateDataTable<T>() where T : class, IDataRow, new();

        /// <summary>
        /// 创建数据表。
        /// </summary>
        /// <param name="dataRowType">数据表行的类型。</param>
        /// <returns>要创建的数据表。</returns>
        DataTableBase CreateDataTable(Type dataRowType);

        /// <summary>
        /// 创建数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <param name="name">数据表名称。</param>
        /// <returns>要创建的数据表。</returns>
        IDataTable<T> CreateDataTable<T>(string name) where T : class, IDataRow, new();

        /// <summary>
        /// 创建数据表。
        /// </summary>
        /// <param name="dataRowType">数据表行的类型。</param>
        /// <param name="name">数据表名称。</param>
        /// <returns>要创建的数据表。</returns>
        DataTableBase CreateDataTable(Type dataRowType, string name);

        /// <summary>
        /// 销毁数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <returns>是否销毁数据表成功。</returns>
        bool DestroyDataTable<T>() where T : IDataRow;

        /// <summary>
        /// 销毁数据表。
        /// </summary>
        /// <param name="dataRowType">数据表行的类型。</param>
        /// <returns>是否销毁数据表成功。</returns>
        bool DestroyDataTable(Type dataRowType);

        /// <summary>
        /// 销毁数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <param name="name">数据表名称。</param>
        /// <returns>是否销毁数据表成功。</returns>
        bool DestroyDataTable<T>(string name) where T : IDataRow;

        /// <summary>
        /// 销毁数据表。
        /// </summary>
        /// <param name="dataRowType">数据表行的类型。</param>
        /// <param name="name">数据表名称。</param>
        /// <returns>是否销毁数据表成功。</returns>
        bool DestroyDataTable(Type dataRowType, string name);

        /// <summary>
        /// 销毁数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <param name="dataTable">要销毁的数据表。</param>
        /// <returns>是否销毁数据表成功。</returns>
        bool DestroyDataTable<T>(IDataTable<T> dataTable) where T : IDataRow;

        /// <summary>
        /// 销毁数据表。
        /// </summary>
        /// <param name="dataTable">要销毁的数据表。</param>
        /// <returns>是否销毁数据表成功。</returns>
        bool DestroyDataTable(DataTableBase dataTable);
    }
}
