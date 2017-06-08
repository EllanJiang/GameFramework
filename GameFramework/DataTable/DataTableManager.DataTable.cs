﻿//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;

namespace GameFramework.DataTable
{
    internal partial class DataTableManager
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
            /// 关闭并清理数据表。
            /// </summary>
            internal override void Shutdown()
            {
                m_DataSet.Clear();
            }

            /// <summary>
            /// 检查是否存在数据表行。
            /// </summary>
            /// <param name="id">数据表行的编号。</param>
            /// <returns>是否存在数据表行。</returns>
            public bool HasDataRow(int id)
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
                    T dr = dataRow.Value;
                    if (condition(dr))
                    {
                        return dr;
                    }
                }

                return null;
            }

            /// <summary>
            /// 增加数据表行。
            /// </summary>
            /// <param name="dataRowText">要解析的数据表行文本。</param>
            public void AddDataRow(string dataRowText)
            {
                T dataRow = new T();
                try
                {
                    dataRow.ParseDataRow(dataRowText);
                }
                catch (Exception exception)
                {
                    if (exception is GameFrameworkException)
                    {
                        throw;
                    }

                    throw new GameFrameworkException(string.Format("Can not parse data table '{0}' at '{1}' with exception '{2}'.", Utility.Text.GetFullName<T>(Name), dataRowText, exception.ToString()), exception);
                }

                if (HasDataRow(dataRow.Id))
                {
                    throw new GameFrameworkException(string.Format("Already exist '{0}' in data table '{1}'.", dataRow.Id.ToString(), Utility.Text.GetFullName<T>(Name)));
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

            /// <summary>
            /// 获取所有数据表行。
            /// </summary>
            /// <returns>所有数据表行。</returns>
            public T[] GetAllDataRows()
            {
                int index = 0;
                T[] allDataRows = new T[m_DataSet.Count];
                foreach (KeyValuePair<int, T> dataRow in m_DataSet)
                {
                    allDataRows[index++] = dataRow.Value;
                }

                return allDataRows;
            }

            /// <summary>
            /// 获取所有符合条件的数据表行。
            /// </summary>
            /// <param name="condition">要检查的条件。</param>
            /// <returns>所有符合条件的数据表行。</returns>
            public T[] GetAllDataRows(Predicate<T> condition)
            {
                if (condition == null)
                {
                    throw new GameFrameworkException("Condition is invalid.");
                }

                List<T> results = new List<T>();
                foreach (KeyValuePair<int, T> dataRow in m_DataSet)
                {
                    T dr = dataRow.Value;
                    if (condition(dr))
                    {
                        results.Add(dr);
                    }
                }

                return results.ToArray();
            }

            /// <summary>
            /// 获取所有排序后的数据表行。
            /// </summary>
            /// <param name="comparison">要排序的条件。</param>
            /// <returns>所有排序后的数据表行。</returns>
            public T[] GetAllDataRows(Comparison<T> comparison)
            {
                if (comparison == null)
                {
                    throw new GameFrameworkException("Comparison is invalid.");
                }

                List<T> allDataRows = new List<T>();
                foreach (KeyValuePair<int, T> dataRow in m_DataSet)
                {
                    allDataRows.Add(dataRow.Value);
                }

                allDataRows.Sort(comparison);
                return allDataRows.ToArray();
            }

            /// <summary>
            /// 获取所有排序后的符合条件的数据表行。
            /// </summary>
            /// <param name="condition">要检查的条件。</param>
            /// <param name="comparison">要排序的条件。</param>
            /// <returns>所有排序后的符合条件的数据表行。</returns>
            public T[] GetAllDataRows(Predicate<T> condition, Comparison<T> comparison)
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
                    T dr = dataRow.Value;
                    if (condition(dr))
                    {
                        results.Add(dr);
                    }
                }

                results.Sort(comparison);
                return results.ToArray();
            }

            /// <summary>
            /// 返回一个循环访问数据表的枚举器。
            /// </summary>
            /// <returns>可用于循环访问数据表的对象。</returns>
            public IEnumerator<T> GetEnumerator()
            {
                return m_DataSet.Values.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return m_DataSet.Values.GetEnumerator();
            }
        }
    }
}
