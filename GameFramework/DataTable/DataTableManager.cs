//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.Resource;
using System;
using System.Collections.Generic;
using System.IO;

namespace GameFramework.DataTable
{
    /// <summary>
    /// 数据表管理器。
    /// </summary>
    internal sealed partial class DataTableManager : GameFrameworkModule, IDataTableManager
    {
        private readonly Dictionary<string, DataTableBase> m_DataTables;
        private readonly LoadAssetCallbacks m_LoadAssetCallbacks;
        private IResourceManager m_ResourceManager;
        private IDataTableHelper m_DataTableHelper;
        private EventHandler<LoadDataTableSuccessEventArgs> m_LoadDataTableSuccessEventHandler;
        private EventHandler<LoadDataTableFailureEventArgs> m_LoadDataTableFailureEventHandler;
        private EventHandler<LoadDataTableUpdateEventArgs> m_LoadDataTableUpdateEventHandler;
        private EventHandler<LoadDataTableDependencyAssetEventArgs> m_LoadDataTableDependencyAssetEventHandler;

        /// <summary>
        /// 初始化数据表管理器的新实例。
        /// </summary>
        public DataTableManager()
        {
            m_DataTables = new Dictionary<string, DataTableBase>();
            m_LoadAssetCallbacks = new LoadAssetCallbacks(LoadDataTableSuccessCallback, LoadDataTableFailureCallback, LoadDataTableUpdateCallback, LoadDataTableDependencyAssetCallback);
            m_ResourceManager = null;
            m_DataTableHelper = null;
            m_LoadDataTableSuccessEventHandler = null;
            m_LoadDataTableFailureEventHandler = null;
            m_LoadDataTableUpdateEventHandler = null;
            m_LoadDataTableDependencyAssetEventHandler = null;
        }

        /// <summary>
        /// 获取数据表数量。
        /// </summary>
        public int Count
        {
            get
            {
                return m_DataTables.Count;
            }
        }

        /// <summary>
        /// 加载数据表成功事件。
        /// </summary>
        public event EventHandler<LoadDataTableSuccessEventArgs> LoadDataTableSuccess
        {
            add
            {
                m_LoadDataTableSuccessEventHandler += value;
            }
            remove
            {
                m_LoadDataTableSuccessEventHandler -= value;
            }
        }

        /// <summary>
        /// 加载数据表失败事件。
        /// </summary>
        public event EventHandler<LoadDataTableFailureEventArgs> LoadDataTableFailure
        {
            add
            {
                m_LoadDataTableFailureEventHandler += value;
            }
            remove
            {
                m_LoadDataTableFailureEventHandler -= value;
            }
        }

        /// <summary>
        /// 加载数据表更新事件。
        /// </summary>
        public event EventHandler<LoadDataTableUpdateEventArgs> LoadDataTableUpdate
        {
            add
            {
                m_LoadDataTableUpdateEventHandler += value;
            }
            remove
            {
                m_LoadDataTableUpdateEventHandler -= value;
            }
        }

        /// <summary>
        /// 加载数据表时加载依赖资源事件。
        /// </summary>
        public event EventHandler<LoadDataTableDependencyAssetEventArgs> LoadDataTableDependencyAsset
        {
            add
            {
                m_LoadDataTableDependencyAssetEventHandler += value;
            }
            remove
            {
                m_LoadDataTableDependencyAssetEventHandler -= value;
            }
        }

        /// <summary>
        /// 数据表管理器轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {
        }

        /// <summary>
        /// 关闭并清理数据表管理器。
        /// </summary>
        internal override void Shutdown()
        {
            foreach (KeyValuePair<string, DataTableBase> dataTable in m_DataTables)
            {
                dataTable.Value.Shutdown();
            }

            m_DataTables.Clear();
        }

        /// <summary>
        /// 设置资源管理器。
        /// </summary>
        /// <param name="resourceManager">资源管理器。</param>
        public void SetResourceManager(IResourceManager resourceManager)
        {
            if (resourceManager == null)
            {
                throw new GameFrameworkException("Resource manager is invalid.");
            }

            m_ResourceManager = resourceManager;
        }

        /// <summary>
        /// 设置数据表辅助器。
        /// </summary>
        /// <param name="dataTableHelper">数据表辅助器。</param>
        public void SetDataTableHelper(IDataTableHelper dataTableHelper)
        {
            if (dataTableHelper == null)
            {
                throw new GameFrameworkException("Data table helper is invalid.");
            }

            m_DataTableHelper = dataTableHelper;
        }

        /// <summary>
        /// 加载数据表。
        /// </summary>
        /// <param name="dataTableAssetName">数据表资源名称。</param>
        /// <param name="loadType">数据表加载方式。</param>
        public void LoadDataTable(string dataTableAssetName, LoadType loadType)
        {
            LoadDataTable(dataTableAssetName, loadType, Constant.DefaultPriority, null);
        }

        /// <summary>
        /// 加载数据表。
        /// </summary>
        /// <param name="dataTableAssetName">数据表资源名称。</param>
        /// <param name="loadType">数据表加载方式。</param>
        /// <param name="priority">加载数据表资源的优先级。</param>
        public void LoadDataTable(string dataTableAssetName, LoadType loadType, int priority)
        {
            LoadDataTable(dataTableAssetName, loadType, priority, null);
        }

        /// <summary>
        /// 加载数据表。
        /// </summary>
        /// <param name="dataTableAssetName">数据表资源名称。</param>
        /// <param name="loadType">数据表加载方式。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void LoadDataTable(string dataTableAssetName, LoadType loadType, object userData)
        {
            LoadDataTable(dataTableAssetName, loadType, Constant.DefaultPriority, userData);
        }

        /// <summary>
        /// 加载数据表。
        /// </summary>
        /// <param name="dataTableAssetName">数据表资源名称。</param>
        /// <param name="loadType">数据表加载方式。</param>
        /// <param name="priority">加载数据表资源的优先级。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void LoadDataTable(string dataTableAssetName, LoadType loadType, int priority, object userData)
        {
            if (m_ResourceManager == null)
            {
                throw new GameFrameworkException("You must set resource manager first.");
            }

            if (m_DataTableHelper == null)
            {
                throw new GameFrameworkException("You must set data table helper first.");
            }

            m_ResourceManager.LoadAsset(dataTableAssetName, priority, m_LoadAssetCallbacks, new LoadDataTableInfo(loadType, userData));
        }

        /// <summary>
        /// 是否存在数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <returns>是否存在数据表。</returns>
        public bool HasDataTable<T>() where T : IDataRow
        {
            return InternalHasDataTable(Utility.Text.GetFullName<T>(string.Empty));
        }

        /// <summary>
        /// 是否存在数据表。
        /// </summary>
        /// <param name="dataRowType">数据表行的类型。</param>
        /// <returns>是否存在数据表。</returns>
        public bool HasDataTable(Type dataRowType)
        {
            if (dataRowType == null)
            {
                throw new GameFrameworkException("Data row type is invalid.");
            }

            if (!typeof(IDataRow).IsAssignableFrom(dataRowType))
            {
                throw new GameFrameworkException(Utility.Text.Format("Data row type '{0}' is invalid.", dataRowType.FullName));
            }

            return InternalHasDataTable(Utility.Text.GetFullName(dataRowType, string.Empty));
        }

        /// <summary>
        /// 是否存在数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <param name="name">数据表名称。</param>
        /// <returns>是否存在数据表。</returns>
        public bool HasDataTable<T>(string name) where T : IDataRow
        {
            return InternalHasDataTable(Utility.Text.GetFullName<T>(name));
        }

        /// <summary>
        /// 是否存在数据表。
        /// </summary>
        /// <param name="dataRowType">数据表行的类型。</param>
        /// <param name="name">数据表名称。</param>
        /// <returns>是否存在数据表。</returns>
        public bool HasDataTable(Type dataRowType, string name)
        {
            if (dataRowType == null)
            {
                throw new GameFrameworkException("Data row type is invalid.");
            }

            if (!typeof(IDataRow).IsAssignableFrom(dataRowType))
            {
                throw new GameFrameworkException(Utility.Text.Format("Data row type '{0}' is invalid.", dataRowType.FullName));
            }

            return InternalHasDataTable(Utility.Text.GetFullName(dataRowType, name));
        }

        /// <summary>
        /// 获取数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <returns>要获取的数据表。</returns>
        public IDataTable<T> GetDataTable<T>() where T : IDataRow
        {
            return (IDataTable<T>)InternalGetDataTable(Utility.Text.GetFullName<T>(string.Empty));
        }

        /// <summary>
        /// 获取数据表。
        /// </summary>
        /// <param name="dataRowType">数据表行的类型。</param>
        /// <returns>要获取的数据表。</returns>
        public DataTableBase GetDataTable(Type dataRowType)
        {
            if (dataRowType == null)
            {
                throw new GameFrameworkException("Data row type is invalid.");
            }

            if (!typeof(IDataRow).IsAssignableFrom(dataRowType))
            {
                throw new GameFrameworkException(Utility.Text.Format("Data row type '{0}' is invalid.", dataRowType.FullName));
            }

            return InternalGetDataTable(Utility.Text.GetFullName(dataRowType, string.Empty));
        }

        /// <summary>
        /// 获取数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <param name="name">数据表名称。</param>
        /// <returns>要获取的数据表。</returns>
        public IDataTable<T> GetDataTable<T>(string name) where T : IDataRow
        {
            return (IDataTable<T>)InternalGetDataTable(Utility.Text.GetFullName<T>(name));
        }

        /// <summary>
        /// 获取数据表。
        /// </summary>
        /// <param name="dataRowType">数据表行的类型。</param>
        /// <param name="name">数据表名称。</param>
        /// <returns>要获取的数据表。</returns>
        public DataTableBase GetDataTable(Type dataRowType, string name)
        {
            if (dataRowType == null)
            {
                throw new GameFrameworkException("Data row type is invalid.");
            }

            if (!typeof(IDataRow).IsAssignableFrom(dataRowType))
            {
                throw new GameFrameworkException(Utility.Text.Format("Data row type '{0}' is invalid.", dataRowType.FullName));
            }

            return InternalGetDataTable(Utility.Text.GetFullName(dataRowType, name));
        }

        /// <summary>
        /// 获取所有数据表。
        /// </summary>
        /// <returns>所有数据表。</returns>
        public DataTableBase[] GetAllDataTables()
        {
            int index = 0;
            DataTableBase[] results = new DataTableBase[m_DataTables.Count];
            foreach (KeyValuePair<string, DataTableBase> dataTable in m_DataTables)
            {
                results[index++] = dataTable.Value;
            }

            return results;
        }

        /// <summary>
        /// 获取所有数据表。
        /// </summary>
        /// <param name="results">所有数据表。</param>
        public void GetAllDataTables(List<DataTableBase> results)
        {
            if (results == null)
            {
                throw new GameFrameworkException("Results is invalid.");
            }

            results.Clear();
            foreach (KeyValuePair<string, DataTableBase> dataTable in m_DataTables)
            {
                results.Add(dataTable.Value);
            }
        }

        /// <summary>
        /// 创建数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <param name="text">要解析的数据表文本。</param>
        /// <returns>要创建的数据表。</returns>
        public IDataTable<T> CreateDataTable<T>(string text) where T : class, IDataRow, new()
        {
            return CreateDataTable<T>(string.Empty, text);
        }

        /// <summary>
        /// 创建数据表。
        /// </summary>
        /// <param name="dataRowType">数据表行的类型。</param>
        /// <param name="text">要解析的数据表文本。</param>
        /// <returns>要创建的数据表。</returns>
        public DataTableBase CreateDataTable(Type dataRowType, string text)
        {
            return CreateDataTable(dataRowType, string.Empty, text);
        }

        /// <summary>
        /// 创建数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <param name="name">数据表名称。</param>
        /// <param name="text">要解析的数据表文本。</param>
        /// <returns>要创建的数据表。</returns>
        public IDataTable<T> CreateDataTable<T>(string name, string text) where T : class, IDataRow, new()
        {
            if (HasDataTable<T>(name))
            {
                throw new GameFrameworkException(Utility.Text.Format("Already exist data table '{0}'.", Utility.Text.GetFullName<T>(name)));
            }

            DataTable<T> dataTable = new DataTable<T>(name);
            InternalCreateDataTable(dataTable, text);
            m_DataTables.Add(Utility.Text.GetFullName<T>(name), dataTable);
            return dataTable;
        }

        /// <summary>
        /// 创建数据表。
        /// </summary>
        /// <param name="dataRowType">数据表行的类型。</param>
        /// <param name="name">数据表名称。</param>
        /// <param name="text">要解析的数据表文本。</param>
        /// <returns>要创建的数据表。</returns>
        public DataTableBase CreateDataTable(Type dataRowType, string name, string text)
        {
            if (dataRowType == null)
            {
                throw new GameFrameworkException("Data row type is invalid.");
            }

            if (!typeof(IDataRow).IsAssignableFrom(dataRowType))
            {
                throw new GameFrameworkException(Utility.Text.Format("Data row type '{0}' is invalid.", dataRowType.FullName));
            }

            if (HasDataTable(dataRowType, name))
            {
                throw new GameFrameworkException(Utility.Text.Format("Already exist data table '{0}'.", Utility.Text.GetFullName(dataRowType, name)));
            }

            Type dataTableType = typeof(DataTable<>).MakeGenericType(dataRowType);
            DataTableBase dataTable = (DataTableBase)Activator.CreateInstance(dataTableType, name);
            InternalCreateDataTable(dataTable, text);
            m_DataTables.Add(Utility.Text.GetFullName(dataRowType, name), dataTable);
            return dataTable;
        }

        /// <summary>
        /// 创建数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <param name="bytes">要解析的数据表二进制流。</param>
        /// <returns>要创建的数据表。</returns>
        public IDataTable<T> CreateDataTable<T>(byte[] bytes) where T : class, IDataRow, new()
        {
            return CreateDataTable<T>(string.Empty, bytes);
        }

        /// <summary>
        /// 创建数据表。
        /// </summary>
        /// <param name="dataRowType">数据表行的类型。</param>
        /// <param name="bytes">要解析的数据表二进制流。</param>
        /// <returns>要创建的数据表。</returns>
        public DataTableBase CreateDataTable(Type dataRowType, byte[] bytes)
        {
            return CreateDataTable(dataRowType, string.Empty, bytes);
        }

        /// <summary>
        /// 创建数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <param name="name">数据表名称。</param>
        /// <param name="bytes">要解析的数据表二进制流。</param>
        /// <returns>要创建的数据表。</returns>
        public IDataTable<T> CreateDataTable<T>(string name, byte[] bytes) where T : class, IDataRow, new()
        {
            if (HasDataTable<T>(name))
            {
                throw new GameFrameworkException(Utility.Text.Format("Already exist data table '{0}'.", Utility.Text.GetFullName<T>(name)));
            }

            DataTable<T> dataTable = new DataTable<T>(name);
            InternalCreateDataTable(dataTable, bytes);
            m_DataTables.Add(Utility.Text.GetFullName<T>(name), dataTable);
            return dataTable;
        }

        /// <summary>
        /// 创建数据表。
        /// </summary>
        /// <param name="dataRowType">数据表行的类型。</param>
        /// <param name="name">数据表名称。</param>
        /// <param name="bytes">要解析的数据表二进制流。</param>
        /// <returns>要创建的数据表。</returns>
        public DataTableBase CreateDataTable(Type dataRowType, string name, byte[] bytes)
        {
            if (dataRowType == null)
            {
                throw new GameFrameworkException("Data row type is invalid.");
            }

            if (!typeof(IDataRow).IsAssignableFrom(dataRowType))
            {
                throw new GameFrameworkException(Utility.Text.Format("Data row type '{0}' is invalid.", dataRowType.FullName));
            }

            if (HasDataTable(dataRowType, name))
            {
                throw new GameFrameworkException(Utility.Text.Format("Already exist data table '{0}'.", Utility.Text.GetFullName(dataRowType, name)));
            }

            Type dataTableType = typeof(DataTable<>).MakeGenericType(dataRowType);
            DataTableBase dataTable = (DataTableBase)Activator.CreateInstance(dataTableType, name);
            InternalCreateDataTable(dataTable, bytes);
            m_DataTables.Add(Utility.Text.GetFullName(dataRowType, name), dataTable);
            return dataTable;
        }

        /// <summary>
        /// 创建数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <param name="stream">要解析的数据表二进制流。</param>
        /// <returns>要创建的数据表。</returns>
        public IDataTable<T> CreateDataTable<T>(Stream stream) where T : class, IDataRow, new()
        {
            return CreateDataTable<T>(string.Empty, stream);
        }

        /// <summary>
        /// 创建数据表。
        /// </summary>
        /// <param name="dataRowType">数据表行的类型。</param>
        /// <param name="stream">要解析的数据表二进制流。</param>
        /// <returns>要创建的数据表。</returns>
        public DataTableBase CreateDataTable(Type dataRowType, Stream stream)
        {
            return CreateDataTable(dataRowType, string.Empty, stream);
        }

        /// <summary>
        /// 创建数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <param name="name">数据表名称。</param>
        /// <param name="stream">要解析的数据表二进制流。</param>
        /// <returns>要创建的数据表。</returns>
        public IDataTable<T> CreateDataTable<T>(string name, Stream stream) where T : class, IDataRow, new()
        {
            if (HasDataTable<T>(name))
            {
                throw new GameFrameworkException(Utility.Text.Format("Already exist data table '{0}'.", Utility.Text.GetFullName<T>(name)));
            }

            DataTable<T> dataTable = new DataTable<T>(name);
            InternalCreateDataTable(dataTable, stream);
            m_DataTables.Add(Utility.Text.GetFullName<T>(name), dataTable);
            return dataTable;
        }

        /// <summary>
        /// 创建数据表。
        /// </summary>
        /// <param name="dataRowType">数据表行的类型。</param>
        /// <param name="name">数据表名称。</param>
        /// <param name="stream">要解析的数据表二进制流。</param>
        /// <returns>要创建的数据表。</returns>
        public DataTableBase CreateDataTable(Type dataRowType, string name, Stream stream)
        {
            if (dataRowType == null)
            {
                throw new GameFrameworkException("Data row type is invalid.");
            }

            if (!typeof(IDataRow).IsAssignableFrom(dataRowType))
            {
                throw new GameFrameworkException(Utility.Text.Format("Data row type '{0}' is invalid.", dataRowType.FullName));
            }

            if (HasDataTable(dataRowType, name))
            {
                throw new GameFrameworkException(Utility.Text.Format("Already exist data table '{0}'.", Utility.Text.GetFullName(dataRowType, name)));
            }

            Type dataTableType = typeof(DataTable<>).MakeGenericType(dataRowType);
            DataTableBase dataTable = (DataTableBase)Activator.CreateInstance(dataTableType, name);
            InternalCreateDataTable(dataTable, stream);
            m_DataTables.Add(Utility.Text.GetFullName(dataRowType, name), dataTable);
            return dataTable;
        }

        /// <summary>
        /// 销毁数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        public bool DestroyDataTable<T>() where T : IDataRow
        {
            return InternalDestroyDataTable(Utility.Text.GetFullName<T>(string.Empty));
        }

        /// <summary>
        /// 销毁数据表。
        /// </summary>
        /// <param name="dataRowType">数据表行的类型。</param>
        /// <returns>是否销毁数据表成功。</returns>
        public bool DestroyDataTable(Type dataRowType)
        {
            if (dataRowType == null)
            {
                throw new GameFrameworkException("Data row type is invalid.");
            }

            if (!typeof(IDataRow).IsAssignableFrom(dataRowType))
            {
                throw new GameFrameworkException(Utility.Text.Format("Data row type '{0}' is invalid.", dataRowType.FullName));
            }

            return InternalDestroyDataTable(Utility.Text.GetFullName(dataRowType, string.Empty));
        }

        /// <summary>
        /// 销毁数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <param name="name">数据表名称。</param>
        public bool DestroyDataTable<T>(string name) where T : IDataRow
        {
            return InternalDestroyDataTable(Utility.Text.GetFullName<T>(name));
        }

        /// <summary>
        /// 销毁数据表。
        /// </summary>
        /// <param name="dataRowType">数据表行的类型。</param>
        /// <param name="name">数据表名称。</param>
        /// <returns>是否销毁数据表成功。</returns>
        public bool DestroyDataTable(Type dataRowType, string name)
        {
            if (dataRowType == null)
            {
                throw new GameFrameworkException("Data row type is invalid.");
            }

            if (!typeof(IDataRow).IsAssignableFrom(dataRowType))
            {
                throw new GameFrameworkException(Utility.Text.Format("Data row type '{0}' is invalid.", dataRowType.FullName));
            }

            return InternalDestroyDataTable(Utility.Text.GetFullName(dataRowType, name));
        }

        private bool InternalHasDataTable(string fullName)
        {
            return m_DataTables.ContainsKey(fullName);
        }

        private DataTableBase InternalGetDataTable(string fullName)
        {
            DataTableBase dataTable = null;
            if (m_DataTables.TryGetValue(fullName, out dataTable))
            {
                return dataTable;
            }

            return null;
        }

        private void InternalCreateDataTable(DataTableBase dataTable, string text)
        {
            IEnumerable<GameFrameworkSegment<string>> dataRowSegments = null;
            try
            {
                dataRowSegments = m_DataTableHelper.GetDataRowSegments(text);
            }
            catch (Exception exception)
            {
                if (exception is GameFrameworkException)
                {
                    throw;
                }

                throw new GameFrameworkException(Utility.Text.Format("Can not get data row segments with exception '{0}'.", exception.ToString()), exception);
            }

            if (dataRowSegments == null)
            {
                throw new GameFrameworkException("Data row segments is invalid.");
            }

            foreach (GameFrameworkSegment<string> dataRowSegment in dataRowSegments)
            {
                if (!dataTable.AddDataRow(dataRowSegment))
                {
                    throw new GameFrameworkException("Add data row failure.");
                }
            }
        }

        private void InternalCreateDataTable(DataTableBase dataTable, byte[] bytes)
        {
            IEnumerable<GameFrameworkSegment<byte[]>> dataRowSegments = null;
            try
            {
                dataRowSegments = m_DataTableHelper.GetDataRowSegments(bytes);
            }
            catch (Exception exception)
            {
                if (exception is GameFrameworkException)
                {
                    throw;
                }

                throw new GameFrameworkException(Utility.Text.Format("Can not get data row segments with exception '{0}'.", exception.ToString()), exception);
            }

            if (dataRowSegments == null)
            {
                throw new GameFrameworkException("Data row segments is invalid.");
            }

            foreach (GameFrameworkSegment<byte[]> dataRowSegment in dataRowSegments)
            {
                if (!dataTable.AddDataRow(dataRowSegment))
                {
                    throw new GameFrameworkException("Add data row failure.");
                }
            }
        }

        private void InternalCreateDataTable(DataTableBase dataTable, Stream stream)
        {
            IEnumerable<GameFrameworkSegment<Stream>> dataRowSegments = null;
            try
            {
                dataRowSegments = m_DataTableHelper.GetDataRowSegments(stream);
            }
            catch (Exception exception)
            {
                if (exception is GameFrameworkException)
                {
                    throw;
                }

                throw new GameFrameworkException(Utility.Text.Format("Can not get data row segments with exception '{0}'.", exception.ToString()), exception);
            }

            if (dataRowSegments == null)
            {
                throw new GameFrameworkException("Data row segments is invalid.");
            }

            foreach (GameFrameworkSegment<Stream> dataRowSegment in dataRowSegments)
            {
                if (!dataTable.AddDataRow(dataRowSegment))
                {
                    throw new GameFrameworkException("Add data row failure.");
                }
            }
        }

        private bool InternalDestroyDataTable(string fullName)
        {
            DataTableBase dataTable = null;
            if (m_DataTables.TryGetValue(fullName, out dataTable))
            {
                dataTable.Shutdown();
                return m_DataTables.Remove(fullName);
            }

            return false;
        }

        private void LoadDataTableSuccessCallback(string dataTableAssetName, object dataTableAsset, float duration, object userData)
        {
            LoadDataTableInfo loadDataTableInfo = (LoadDataTableInfo)userData;
            if (loadDataTableInfo == null)
            {
                throw new GameFrameworkException("Load data table info is invalid.");
            }

            try
            {
                if (!m_DataTableHelper.LoadDataTable(dataTableAsset, loadDataTableInfo.LoadType, loadDataTableInfo.UserData))
                {
                    throw new GameFrameworkException(Utility.Text.Format("Load data table failure in helper, asset name '{0}'.", dataTableAssetName));
                }
            }
            catch (Exception exception)
            {
                if (m_LoadDataTableFailureEventHandler != null)
                {
                    m_LoadDataTableFailureEventHandler(this, new LoadDataTableFailureEventArgs(dataTableAssetName, exception.ToString(), loadDataTableInfo.UserData));
                    return;
                }

                throw;
            }
            finally
            {
                m_DataTableHelper.ReleaseDataTableAsset(dataTableAsset);
            }

            if (m_LoadDataTableSuccessEventHandler != null)
            {
                m_LoadDataTableSuccessEventHandler(this, new LoadDataTableSuccessEventArgs(dataTableAssetName, duration, loadDataTableInfo.UserData));
            }
        }

        private void LoadDataTableFailureCallback(string dataTableAssetName, LoadResourceStatus status, string errorMessage, object userData)
        {
            LoadDataTableInfo loadDataTableInfo = (LoadDataTableInfo)userData;
            if (loadDataTableInfo == null)
            {
                throw new GameFrameworkException("Load data table info is invalid.");
            }

            string appendErrorMessage = Utility.Text.Format("Load data table failure, asset name '{0}', status '{1}', error message '{2}'.", dataTableAssetName, status.ToString(), errorMessage);
            if (m_LoadDataTableFailureEventHandler != null)
            {
                m_LoadDataTableFailureEventHandler(this, new LoadDataTableFailureEventArgs(dataTableAssetName, appendErrorMessage, loadDataTableInfo.UserData));
                return;
            }

            throw new GameFrameworkException(appendErrorMessage);
        }

        private void LoadDataTableUpdateCallback(string dataTableAssetName, float progress, object userData)
        {
            LoadDataTableInfo loadDataTableInfo = (LoadDataTableInfo)userData;
            if (loadDataTableInfo == null)
            {
                throw new GameFrameworkException("Load data table info is invalid.");
            }

            if (m_LoadDataTableUpdateEventHandler != null)
            {
                m_LoadDataTableUpdateEventHandler(this, new LoadDataTableUpdateEventArgs(dataTableAssetName, progress, loadDataTableInfo.UserData));
            }
        }

        private void LoadDataTableDependencyAssetCallback(string dataTableAssetName, string dependencyAssetName, int loadedCount, int totalCount, object userData)
        {
            LoadDataTableInfo loadDataTableInfo = (LoadDataTableInfo)userData;
            if (loadDataTableInfo == null)
            {
                throw new GameFrameworkException("Load data table info is invalid.");
            }

            if (m_LoadDataTableDependencyAssetEventHandler != null)
            {
                m_LoadDataTableDependencyAssetEventHandler(this, new LoadDataTableDependencyAssetEventArgs(dataTableAssetName, dependencyAssetName, loadedCount, totalCount, loadDataTableInfo.UserData));
            }
        }
    }
}
