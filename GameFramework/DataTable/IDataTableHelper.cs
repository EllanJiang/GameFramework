//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;

namespace GameFramework.DataTable
{
    /// <summary>
    /// 数据表辅助器接口。
    /// </summary>
    public interface IDataTableHelper
    {
        /// <summary>
        /// 加载数据表。
        /// </summary>
        /// <param name="dataTableAsset">数据表资源。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>加载是否成功。</returns>
        bool LoadDataTable(object dataTableAsset, object userData);

        /// <summary>
        /// 获取数据表行文本。
        /// </summary>
        /// <param name="text">要解析的数据表文本。</param>
        /// <returns>数据表行文本。</returns>
        IEnumerable<string> GetSplitedDataRows(string text);

        /// <summary>
        /// 获取数据表行二进制流。
        /// </summary>
        /// <param name="bytes">要解析的数据表二进制流。</param>
        /// <returns>数据表行二进制流。</returns>
        IEnumerable<ArraySegment<byte>> GetSplitedDataRows(byte[] bytes);

        /// <summary>
        /// 获取数据表行描述数据，包含每个数据表行的偏移和长度。
        /// </summary>
        /// <param name="stream">要解析的数据表二进制流。</param>
        /// <returns>数据表行描述数据。</returns>
        IEnumerable<KeyValuePair<int, int>> GetSplitedDataRows(Stream stream);

        /// <summary>
        /// 释放数据表资源。
        /// </summary>
        /// <param name="dataTableAsset">要释放的数据表资源。</param>
        void ReleaseDataTableAsset(object dataTableAsset);
    }
}
