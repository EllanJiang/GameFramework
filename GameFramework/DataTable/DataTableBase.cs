//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;

namespace GameFramework.DataTable
{
    /// <summary>
    /// 数据表基类。
    /// </summary>
    public abstract class DataTableBase
    {
        private readonly string m_Name;

        /// <summary>
        /// 初始化数据表基类的新实例。
        /// </summary>
        public DataTableBase()
            : this(null)
        {
        }

        /// <summary>
        /// 初始化数据表基类的新实例。
        /// </summary>
        /// <param name="name">数据表名称。</param>
        public DataTableBase(string name)
        {
            m_Name = name ?? string.Empty;
        }

        /// <summary>
        /// 获取数据表名称。
        /// </summary>
        public string Name
        {
            get
            {
                return m_Name;
            }
        }

        /// <summary>
        /// 获取数据表完整名称。
        /// </summary>
        public string FullName
        {
            get
            {
                return new TypeNamePair(Type, m_Name).ToString();
            }
        }

        /// <summary>
        /// 获取数据表行的类型。
        /// </summary>
        public abstract Type Type
        {
            get;
        }

        /// <summary>
        /// 获取数据表行数。
        /// </summary>
        public abstract int Count
        {
            get;
        }

        /// <summary>
        /// 增加数据表行。
        /// </summary>
        /// <param name="dataRowSegment">要解析的数据表行片段。</param>
        /// <param name="dataTableUserData">数据表用户自定义数据。</param>
        /// <returns>是否增加数据表行成功。</returns>
        public abstract bool AddDataRow(GameFrameworkDataSegment dataRowSegment, object dataTableUserData);

        /// <summary>
        /// 关闭并清理数据表。
        /// </summary>
        internal abstract void Shutdown();
    }
}
