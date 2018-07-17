//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
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
        /// 加载数据表成功事件。
        /// </summary>
        event EventHandler<LoadDataTableSuccessEventArgs> LoadDataTableSuccess;

        /// <summary>
        /// 加载数据表失败事件。
        /// </summary>
        event EventHandler<LoadDataTableFailureEventArgs> LoadDataTableFailure;

        /// <summary>
        /// 加载数据表更新事件。
        /// </summary>
        event EventHandler<LoadDataTableUpdateEventArgs> LoadDataTableUpdate;

        /// <summary>
        /// 加载数据表时加载依赖资源事件。
        /// </summary>
        event EventHandler<LoadDataTableDependencyAssetEventArgs> LoadDataTableDependencyAsset;

        /// <summary>
        /// 设置资源管理器。
        /// </summary>
        /// <param name="resourceManager">资源管理器。</param>
        void SetResourceManager(IResourceManager resourceManager);

        /// <summary>
        /// 设置数据表辅助器。
        /// </summary>
        /// <param name="dataTableHelper">数据表辅助器。</param>
        void SetDataTableHelper(IDataTableHelper dataTableHelper);

        /// <summary>
        /// 加载数据表。
        /// </summary>
        /// <param name="dataTableAssetName">数据表资源名称。</param>
        void LoadDataTable(string dataTableAssetName);

        /// <summary>
        /// 加载数据表。
        /// </summary>
        /// <param name="dataTableAssetName">数据表资源名称。</param>
        /// <param name="priority">加载数据表资源的优先级。</param>
        void LoadDataTable(string dataTableAssetName, int priority);

        /// <summary>
        /// 加载数据表。
        /// </summary>
        /// <param name="dataTableAssetName">数据表资源名称。</param>
        /// <param name="userData">用户自定义数据。</param>
        void LoadDataTable(string dataTableAssetName, object userData);

        /// <summary>
        /// 加载数据表。
        /// </summary>
        /// <param name="dataTableAssetName">数据表资源名称。</param>
        /// <param name="priority">加载数据表资源的优先级。</param>
        /// <param name="userData">用户自定义数据。</param>
        void LoadDataTable(string dataTableAssetName, int priority, object userData);

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
        /// <param name="text">要解析的数据表文本。</param>
        /// <returns>要创建的数据表。</returns>
        IDataTable<T> CreateDataTable<T>(string text) where T : class, IDataRow, new();

        /// <summary>
        /// 创建数据表。
        /// </summary>
        /// <param name="dataRowType">数据表行的类型。</param>
        /// <param name="text">要解析的数据表文本。</param>
        /// <returns>要创建的数据表。</returns>
        DataTableBase CreateDataTable(Type dataRowType, string text);

        /// <summary>
        /// 创建数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <param name="name">数据表名称。</param>
        /// <param name="text">要解析的数据表文本。</param>
        /// <returns>要创建的数据表。</returns>
        IDataTable<T> CreateDataTable<T>(string name, string text) where T : class, IDataRow, new();

        /// <summary>
        /// 创建数据表。
        /// </summary>
        /// <param name="dataRowType">数据表行的类型。</param>
        /// <param name="name">数据表名称。</param>
        /// <param name="text">要解析的数据表文本。</param>
        /// <returns>要创建的数据表。</returns>
        DataTableBase CreateDataTable(Type dataRowType, string name, string text);

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
    }
}
