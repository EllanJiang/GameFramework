//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.Event;
using System;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 加载数据表更新事件。
    /// </summary>
    public sealed class LoadDataTableUpdateEventArgs : GameEventArgs
    {
        /// <summary>
        /// 初始化加载数据表更新事件的新实例。
        /// </summary>
        /// <param name="e">内部事件。</param>
        public LoadDataTableUpdateEventArgs(GameFramework.DataTable.LoadDataTableUpdateEventArgs e)
        {
            LoadDataTableInfo loadDataTableInfo = (LoadDataTableInfo)e.UserData;
            DataTableName = loadDataTableInfo.DataTableName;
            DataTableType = loadDataTableInfo.DataTableType;
            DataTableAssetName = e.DataTableAssetName;
            Progress = e.Progress;
            UserData = loadDataTableInfo.UserData;
        }

        /// <summary>
        /// 获取加载数据表失败事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return (int)EventId.LoadDataTableUpdate;
            }
        }

        /// <summary>
        /// 获取数据表名称。
        /// </summary>
        public string DataTableName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取数据表类型。
        /// </summary>
        public Type DataTableType
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取数据表资源名称。
        /// </summary>
        public string DataTableAssetName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取加载数据表进度。
        /// </summary>
        public float Progress
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取用户自定义数据。
        /// </summary>
        public object UserData
        {
            get;
            private set;
        }
    }
}
