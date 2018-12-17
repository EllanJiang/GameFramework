//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System.IO;

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
        /// 数据表行文本解析器。
        /// </summary>
        /// <param name="dataRowSegment">要解析的数据表行片段。</param>
        /// <returns>是否解析数据表行成功。</returns>
        bool ParseDataRow(GameFrameworkSegment<string> dataRowSegment);

        /// <summary>
        /// 数据表行二进制流解析器。
        /// </summary>
        /// <param name="dataRowSegment">要解析的数据表行片段。</param>
        /// <returns>是否解析数据表行成功。</returns>
        bool ParseDataRow(GameFrameworkSegment<byte[]> dataRowSegment);

        /// <summary>
        /// 数据表行二进制流解析器。
        /// </summary>
        /// <param name="dataRowSegment">要解析的数据表行片段。</param>
        /// <returns>是否解析数据表行成功。</returns>
        bool ParseDataRow(GameFrameworkSegment<Stream> dataRowSegment);
    }
}
