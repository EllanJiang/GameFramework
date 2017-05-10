//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.Event;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 加载字典时加载依赖资源事件。
    /// </summary>
    public sealed class LoadDictionaryDependencyAssetEventArgs : GameEventArgs
    {
        /// <summary>
        /// 初始化加载字典时加载依赖资源事件的新实例。
        /// </summary>
        /// <param name="e">内部事件。</param>
        public LoadDictionaryDependencyAssetEventArgs(GameFramework.Localization.LoadDictionaryDependencyAssetEventArgs e)
        {
            LoadDictionaryInfo loadDictionaryInfo = (LoadDictionaryInfo)e.UserData;
            DictionaryName = loadDictionaryInfo.DictionaryName;
            DictionaryAssetName = e.DictionaryAssetName;
            DependencyAssetName = e.DependencyAssetName;
            LoadedCount = e.LoadedCount;
            TotalCount = e.TotalCount;
            UserData = loadDictionaryInfo.UserData;
        }

        /// <summary>
        /// 获取加载字典失败事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return (int)EventId.LoadDictionaryDependencyAsset;
            }
        }

        /// <summary>
        /// 获取字典名称。
        /// </summary>
        public string DictionaryName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取字典资源名称。
        /// </summary>
        public string DictionaryAssetName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取被加载的依赖资源名称。
        /// </summary>
        public string DependencyAssetName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取当前已加载依赖资源数量。
        /// </summary>
        public int LoadedCount
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取总共加载依赖资源数量。
        /// </summary>
        public int TotalCount
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
