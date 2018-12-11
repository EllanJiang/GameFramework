//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System;
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
        /// <param name="dataRowText">要解析的数据表行文本。</param>
        void ParseDataRow(string dataRowText);

        /// <summary>
        /// 数据表行二进制流解析器。
        /// </summary>
        /// <param name="dataRowBytes">要解析的数据表行二进制流。</param>
        void ParseDataRow(ArraySegment<byte> dataRowBytes);

        /// <summary>
        /// 数据表行二进制流解析器。
        /// </summary>
        /// <param name="stream">数据表二进制流。</param>
        /// <param name="dataRowOffset">要解析的数据表行的偏移。</param>
        /// <param name="dataRowLength">要解析的数据表行的长度。</param>
        void ParseDataRow(Stream stream, int dataRowOffset, int dataRowLength);
    }
}
