//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;

namespace GameFramework.DataTable
{
    internal sealed partial class DataTableManager : GameFrameworkModule, IDataTableManager
    {
        /// <summary>
        /// 数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        private sealed class DataTable<T> : DataTableBase, IDataTable<T> where T : class, IDataRow, new()
        {
            private readonly Dictionary<int, T> m_DataSet;
            private T m_MinIdDataRow;
            private T m_MaxIdDataRow;

            /// <summary>
            /// 初始化数据表的新实例。
            /// </summary>
            /// <param name="name">数据表名称。</param>
            public DataTable(string name)
                : base(name)
            {
                m_DataSet = new Dictionary<int, T>();
                m_MinIdDataRow = null;
                m_MaxIdDataRow = null;
            }

            /// <summary>
            /// 获取数据表行的类型。
            /// </summary>
            public override Type Type
            {
                get
                {
                    return typeof(T);
                }
            }

            /// <summary>
            /// 获取数据表行数。
            /// </summary>
            public override int Count
            {
                get
                {
                    return m_DataSet.Count;
                }
            }

            /// <summary>
            /// 获取数据表行。
            /// </summary>
            /// <param name="id">数据表行的编号。</param>
            /// <returns>数据表行。</returns>
            public T this[int id]
            {
                get
                {
                    return GetDataRow(id);
                }
            }

            /// <summary>
            /// 获取编号最小的数据表行。
            /// </summary>
            public T MinIdDataRow
            {
                get
                {
                    return m_MinIdDataRow;
                }
            }

            /// <summary>
            /// 获取编号最大的数据表行。
            /// </summary>
            public T MaxIdDataRow
            {
                get
                {
                    return m_MaxIdDataRow;
                }
            }

            /// <summary>
            /// 检查是否存在数据表行。
            /// </summary>
            /// <param name="id">数据表行的编号。</param>
            /// <returns>是否存在数据表行。</returns>
            public override bool HasDataRow(int id)
            {
                return m_DataSet.ContainsKey(id);
            }

            /// <summary>
            /// 检查是否存在数据表行。
            /// </summary>
            /// <param name="condition">要检查的条件。</param>
            /// <returns>是否存在数据表行。</returns>
            public bool HasDataRow(Predicate<T> condition)
            {
                if (condition == null)
                {
                    throw new GameFrameworkException("Condition is invalid.");
                }

                foreach (KeyValuePair<int, T> dataRow in m_DataSet)
                {
                    if (condition(dataRow.Value))
                    {
                        return true;
                    }
                }

                return false;
            }

            /// <summary>
            /// 获取数据表行。
            /// </summary>
            /// <param name="id">数据表行的编号。</param>
            /// <returns>数据表行。</returns>
            public T GetDataRow(int id)
            {
                T dataRow = null;
                if (m_DataSet.TryGetValue(id, out dataRow))
                {
                    return dataRow;
                }

                return null;
            }

            /// <summary>
            /// 获取符合条件的数据表行。
            /// </summary>
            /// <param name="condition">要检查的条件。</param>
            /// <returns>符合条件的数据表行。</returns>
            /// <remarks>当存在多个符合条件的数据表行时，仅返回第一个符合条件的数据表行。</remarks>
            public T GetDataRow(Predicate<T> condition)
            {
                if (condition == null)
                {
                    throw new GameFrameworkException("Condition is invalid.");
                }

                foreach (KeyValuePair<int, T> dataRow in m_DataSet)
                {
                    if (condition(dataRow.Value))
                    {
                        return dataRow.Value;
                    }
                }

                return null;
            }

            /// <summary>
            /// 获取符合条件的数据表行。
            /// </summary>
            /// <param name="condition">要检查的条件。</param>
            /// <returns>符合条件的数据表行。</returns>
            public T[] GetDataRows(Predicate<T> condition)
            {
                if (condition == null)
                {
                    throw new GameFrameworkException("Condition is invalid.");
                }

                List<T> results = new List<T>();
                foreach (KeyValuePair<int, T> dataRow in m_DataSet)
                {
                    if (condition(dataRow.Value))
                    {
                        results.Add(dataRow.Value);
                    }
                }

                return results.ToArray();
            }

            /// <summary>
            /// 获取符合条件的数据表行。
            /// </summary>
            /// <param name="condition">要检查的条件。</param>
            /// <param name="results">符合条件的数据表行。</param>
            public void GetDataRows(Predicate<T> condition, List<T> results)
            {
                if (condition == null)
                {
                    throw new GameFrameworkException("Condition is invalid.");
                }

                if (results == null)
                {
                    throw new GameFrameworkException("Results is invalid.");
                }

                results.Clear();
                foreach (KeyValuePair<int, T> dataRow in m_DataSet)
                {
                    if (condition(dataRow.Value))
                    {
                        results.Add(dataRow.Value);
                    }
                }
            }

            /// <summary>
            /// 获取排序后的数据表行。
            /// </summary>
            /// <param name="comparison">要排序的条件。</param>
            /// <returns>排序后的数据表行。</returns>
            public T[] GetDataRows(Comparison<T> comparison)
            {
                if (comparison == null)
                {
                    throw new GameFrameworkException("Comparison is invalid.");
                }

                List<T> results = new List<T>();
                foreach (KeyValuePair<int, T> dataRow in m_DataSet)
                {
                    results.Add(dataRow.Value);
                }

                results.Sort(comparison);
                return results.ToArray();
            }

            /// <summary>
            /// 获取排序后的数据表行。
            /// </summary>
            /// <param name="comparison">要排序的条件。</param>
            /// <param name="results">排序后的数据表行。</param>
            public void GetDataRows(Comparison<T> comparison, List<T> results)
            {
                if (comparison == null)
                {
                    throw new GameFrameworkException("Comparison is invalid.");
                }

                if (results == null)
                {
                    throw new GameFrameworkException("Results is invalid.");
                }

                results.Clear();
                foreach (KeyValuePair<int, T> dataRow in m_DataSet)
                {
                    results.Add(dataRow.Value);
                }

                results.Sort(comparison);
            }

            /// <summary>
            /// 获取排序后的符合条件的数据表行。
            /// </summary>
            /// <param name="condition">要检查的条件。</param>
            /// <param name="comparison">要排序的条件。</param>
            /// <returns>排序后的符合条件的数据表行。</returns>
            public T[] GetDataRows(Predicate<T> condition, Comparison<T> comparison)
            {
                if (condition == null)
                {
                    throw new GameFrameworkException("Condition is invalid.");
                }

                if (comparison == null)
                {
                    throw new GameFrameworkException("Comparison is invalid.");
                }

                List<T> results = new List<T>();
                foreach (KeyValuePair<int, T> dataRow in m_DataSet)
                {
                    if (condition(dataRow.Value))
                    {
                        results.Add(dataRow.Value);
                    }
                }

                results.Sort(comparison);
                return results.ToArray();
            }

            /// <summary>
            /// 获取排序后的符合条件的数据表行。
            /// </summary>
            /// <param name="condition">要检查的条件。</param>
            /// <param name="comparison">要排序的条件。</param>
            /// <param name="results">排序后的符合条件的数据表行。</param>
            public void GetDataRows(Predicate<T> condition, Comparison<T> comparison, List<T> results)
            {
                if (condition == null)
                {
                    throw new GameFrameworkException("Condition is invalid.");
                }

                if (comparison == null)
                {
                    throw new GameFrameworkException("Comparison is invalid.");
                }

                if (results == null)
                {
                    throw new GameFrameworkException("Results is invalid.");
                }

                results.Clear();
                foreach (KeyValuePair<int, T> dataRow in m_DataSet)
                {
                    if (condition(dataRow.Value))
                    {
                        results.Add(dataRow.Value);
                    }
                }

                results.Sort(comparison);
            }

            /// <summary>
            /// 获取所有数据表行。
            /// </summary>
            /// <returns>所有数据表行。</returns>
            public T[] GetAllDataRows()
            {
                int index = 0;
                T[] results = new T[m_DataSet.Count];
                foreach (KeyValuePair<int, T> dataRow in m_DataSet)
                {
                    results[index++] = dataRow.Value;
                }

                return results;
            }

            /// <summary>
            /// 获取所有数据表行。
            /// </summary>
            /// <param name="results">所有数据表行。</param>
            public void GetAllDataRows(List<T> results)
            {
                if (results == null)
                {
                    throw new GameFrameworkException("Results is invalid.");
                }

                results.Clear();
                foreach (KeyValuePair<int, T> dataRow in m_DataSet)
                {
                    results.Add(dataRow.Value);
                }
            }

            /// <summary>
            /// 增加数据表行。
            /// </summary>
            /// <param name="dataRowString">要解析的数据表行字符串。</param>
            /// <param name="userData">用户自定义数据。</param>
            /// <returns>是否增加数据表行成功。</returns>
            public override bool AddDataRow(string dataRowString, object userData)
            {
                try
                {
                    T dataRow = new T();
                    if (!dataRow.ParseDataRow(dataRowString, userData))
                    {
                        return false;
                    }

                    InternalAddDataRow(dataRow);
                    return true;
                }
                catch (Exception exception)
                {
                    if (exception is GameFrameworkException)
                    {
                        throw;
                    }

                    throw new GameFrameworkException(Utility.Text.Format("Can not parse data row string for data table '{0}' with exception '{1}'.", new TypeNamePair(typeof(T), Name), exception), exception);
                }
            }

            /// <summary>
            /// 增加数据表行。
            /// </summary>
            /// <param name="dataRowBytes">要解析的数据表行二进制流。</param>
            /// <param name="startIndex">数据表行二进制流的起始位置。</param>
            /// <param name="length">数据表行二进制流的长度。</param>
            /// <param name="userData">用户自定义数据。</param>
            /// <returns>是否增加数据表行成功。</returns>
            public override bool AddDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
            {
                try
                {
                    T dataRow = new T();
                    if (!dataRow.ParseDataRow(dataRowBytes, startIndex, length, userData))
                    {
                        return false;
                    }

                    InternalAddDataRow(dataRow);
                    return true;
                }
                catch (Exception exception)
                {
                    if (exception is GameFrameworkException)
                    {
                        throw;
                    }

                    throw new GameFrameworkException(Utility.Text.Format("Can not parse data row bytes for data table '{0}' with exception '{1}'.", new TypeNamePair(typeof(T), Name), exception), exception);
                }
            }

            /// <summary>
            /// 移除指定数据表行。
            /// </summary>
            /// <param name="id">要移除数据表行的编号。</param>
            /// <returns>是否移除数据表行成功。</returns>
            public override bool RemoveDataRow(int id)
            {
                if (!HasDataRow(id))
                {
                    return false;
                }

                if (!m_DataSet.Remove(id))
                {
                    return false;
                }

                if (m_MinIdDataRow != null && m_MinIdDataRow.Id == id || m_MaxIdDataRow != null && m_MaxIdDataRow.Id == id)
                {
                    m_MinIdDataRow = null;
                    m_MaxIdDataRow = null;
                    foreach (KeyValuePair<int, T> dataRow in m_DataSet)
                    {
                        if (m_MinIdDataRow == null || m_MinIdDataRow.Id > dataRow.Key)
                        {
                            m_MinIdDataRow = dataRow.Value;
                        }

                        if (m_MaxIdDataRow == null || m_MaxIdDataRow.Id < dataRow.Key)
                        {
                            m_MaxIdDataRow = dataRow.Value;
                        }
                    }
                }

                return true;
            }

            /// <summary>
            /// 清空所有数据表行。
            /// </summary>
            public override void RemoveAllDataRows()
            {
                m_DataSet.Clear();
                m_MinIdDataRow = null;
                m_MaxIdDataRow = null;
            }

            /// <summary>
            /// 返回循环访问集合的枚举数。
            /// </summary>
            /// <returns>循环访问集合的枚举数。</returns>
            public IEnumerator<T> GetEnumerator()
            {
                return m_DataSet.Values.GetEnumerator();
            }

            /// <summary>
            /// 返回循环访问集合的枚举数。
            /// </summary>
            /// <returns>循环访问集合的枚举数。</returns>
            IEnumerator IEnumerable.GetEnumerator()
            {
                return m_DataSet.Values.GetEnumerator();
            }

            /// <summary>
            /// 关闭并清理数据表。
            /// </summary>
            internal override void Shutdown()
            {
                m_DataSet.Clear();
            }

            private void InternalAddDataRow(T dataRow)
            {
                if (m_DataSet.ContainsKey(dataRow.Id))
                {
                    throw new GameFrameworkException(Utility.Text.Format("Already exist '{0}' in data table '{1}'.", dataRow.Id, new TypeNamePair(typeof(T), Name)));
                }

                m_DataSet.Add(dataRow.Id, dataRow);

                if (m_MinIdDataRow == null || m_MinIdDataRow.Id > dataRow.Id)
                {
                    m_MinIdDataRow = dataRow;
                }

                if (m_MaxIdDataRow == null || m_MaxIdDataRow.Id < dataRow.Id)
                {
                    m_MaxIdDataRow = dataRow;
                }
            }
        }
    }
}
