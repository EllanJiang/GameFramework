//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.DataTable
{
    /// <summary>
    /// 数据表行接口。
    /// </summary>
    public interface IDataRow
    {
        /// <summary>
        /// 获取数据表行的编号。
        /// </summary>
        int Id
        {
            get;
        }

        /// <summary>
        /// 数据表行解析器。
        /// </summary>
        /// <param name="dataRowSegment">要解析的数据表行片段。</param>
        /// <param name="dataTableUserData">数据表用户自定义数据。</param>
        /// <returns>是否解析数据表行成功。</returns>
        bool ParseDataRow(GameFrameworkDataSegment dataRowSegment, object dataTableUserData);
    }
}
