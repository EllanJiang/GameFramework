//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

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
        /// 将要解析的数据表文本分割为数据表行文本。
        /// </summary>
        /// <param name="text">要解析的数据表文本。</param>
        /// <returns>数据表行文本。</returns>
        string[] SplitToDataRows(string text);

        /// <summary>
        /// 释放数据表资源。
        /// </summary>
        /// <param name="dataTableAsset">要释放的数据表资源。</param>
        void ReleaseDataTableAsset(object dataTableAsset);
    }
}
