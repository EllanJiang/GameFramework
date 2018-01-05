//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.DataTable
{
    /// <summary>
    /// 加载数据表成功事件。
    /// </summary>
    public sealed class LoadDataTableSuccessEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化加载数据表成功事件的新实例。
        /// </summary>
        /// <param name="dataTableAssetName">数据表资源名称。</param>
        /// <param name="duration">加载持续时间。</param>
        /// <param name="userData">用户自定义数据。</param>
        public LoadDataTableSuccessEventArgs(string dataTableAssetName, float duration, object userData)
        {
            DataTableAssetName = dataTableAssetName;
            Duration = duration;
            UserData = userData;
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
        /// 获取加载持续时间。
        /// </summary>
        public float Duration
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
