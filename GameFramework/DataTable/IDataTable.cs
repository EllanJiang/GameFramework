//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace GameFramework.DataTable
{
    /// <summary>
    /// 数据表接口。
    /// </summary>
    /// <typeparam name="T">数据表行的类型。</typeparam>
    public interface IDataTable<T> : IEnumerable<T> where T : IDataRow
    {
        /// <summary>
        /// 获取数据表名称。
        /// </summary>
        string Name
        {
            get;
        }

        /// <summary>
        /// 获取数据表行的类型。
        /// </summary>
        Type Type
        {
            get;
        }

        /// <summary>
        /// 获取数据表行数。
        /// </summary>
        int Count
        {
            get;
        }

        /// <summary>
        /// 获取数据表行。
        /// </summary>
        /// <param name="id">数据表行的编号。</param>
        /// <returns>数据表行。</returns>
        T this[int id]
        {
            get;
        }

        /// <summary>
        /// 获取编号最小的数据表行。
        /// </summary>
        T MinIdDataRow
        {
            get;
        }

        /// <summary>
        /// 获取编号最大的数据表行。
        /// </summary>
        T MaxIdDataRow
        {
            get;
        }

        /// <summary>
        /// 检查是否存在数据表行。
        /// </summary>
        /// <param name="id">数据表行的编号。</param>
        /// <returns>是否存在数据表行。</returns>
        bool HasDataRow(int id);

        /// <summary>
        /// 检查是否存在数据表行。
        /// </summary>
        /// <param name="condition">要检查的条件。</param>
        /// <returns>是否存在数据表行。</returns>
        bool HasDataRow(Predicate<T> condition);

        /// <summary>
        /// 获取数据表行。
        /// </summary>
        /// <param name="id">数据表行的编号。</param>
        /// <returns>数据表行。</returns>
        T GetDataRow(int id);

        /// <summary>
        /// 获取符合条件的数据表行。
        /// </summary>
        /// <param name="condition">要检查的条件。</param>
        /// <returns>符合条件的数据表行。</returns>
        /// <remarks>当存在多个符合条件的数据表行时，仅返回第一个符合条件的数据表行。</remarks>
        T GetDataRow(Predicate<T> condition);

        /// <summary>
        /// 获取所有数据表行。
        /// </summary>
        /// <returns>所有数据表行。</returns>
        T[] GetAllDataRows();

        /// <summary>
        /// 获取所有数据表行。
        /// </summary>
        /// <param name="results">所有数据表行。</param>
        void GetAllDataRows(List<T> results);

        /// <summary>
        /// 获取所有符合条件的数据表行。
        /// </summary>
        /// <param name="condition">要检查的条件。</param>
        /// <returns>所有符合条件的数据表行。</returns>
        T[] GetAllDataRows(Predicate<T> condition);

        /// <summary>
        /// 获取所有符合条件的数据表行。
        /// </summary>
        /// <param name="condition">要检查的条件。</param>
        /// <param name="results">所有符合条件的数据表行。</param>
        void GetAllDataRows(Predicate<T> condition, List<T> results);

        /// <summary>
        /// 获取所有排序后的数据表行。
        /// </summary>
        /// <param name="comparison">要排序的条件。</param>
        /// <returns>所有排序后的数据表行。</returns>
        T[] GetAllDataRows(Comparison<T> comparison);

        /// <summary>
        /// 获取所有排序后的数据表行。
        /// </summary>
        /// <param name="comparison">要排序的条件。</param>
        /// <param name="results">所有排序后的数据表行。</param>
        void GetAllDataRows(Comparison<T> comparison, List<T> results);

        /// <summary>
        /// 获取所有排序后的符合条件的数据表行。
        /// </summary>
        /// <param name="condition">要检查的条件。</param>
        /// <param name="comparison">要排序的条件。</param>
        /// <returns>所有排序后的符合条件的数据表行。</returns>
        T[] GetAllDataRows(Predicate<T> condition, Comparison<T> comparison);

        /// <summary>
        /// 获取所有排序后的符合条件的数据表行。
        /// </summary>
        /// <param name="condition">要检查的条件。</param>
        /// <param name="comparison">要排序的条件。</param>
        /// <param name="results">所有排序后的符合条件的数据表行。</param>
        void GetAllDataRows(Predicate<T> condition, Comparison<T> comparison, List<T> results);
    }
}
