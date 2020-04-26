//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
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
        /// <param name="dataTableAssetName">数据表资源名称。</param>
        /// <param name="dataTableObject">数据表对象。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否加载成功。</returns>
        bool LoadDataTable(string dataTableAssetName, object dataTableObject, object userData);

        /// <summary>
        /// 获取数据表行片段。
        /// </summary>
        /// <param name="dataTableData">要解析的数据表数据。</param>
        /// <returns>数据表行片段。</returns>
        GameFrameworkDataSegment[] GetDataRowSegments(object dataTableData);

        /// <summary>
        /// 获取数据表用户自定义数据。
        /// </summary>
        /// <param name="dataTableData">要解析的数据表数据。</param>
        /// <returns>数据表用户自定义数据。</returns>
        object GetDataTableUserData(object dataTableData);

        /// <summary>
        /// 释放数据表资源。
        /// </summary>
        /// <param name="dataTableAsset">要释放的数据表资源。</param>
        void ReleaseDataTableAsset(object dataTableAsset);
    }
}
