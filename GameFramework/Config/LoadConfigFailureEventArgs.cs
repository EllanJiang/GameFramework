//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Config
{
    /// <summary>
    /// 加载配置失败事件。
    /// </summary>
    public sealed class LoadConfigFailureEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化加载配置失败事件的新实例。
        /// </summary>
        /// <param name="configAssetName">配置资源名称。</param>
        /// <param name="loadType">配置加载方式。</param>
        /// <param name="errorMessage">错误信息。</param>
        /// <param name="userData">用户自定义数据。</param>
        public LoadConfigFailureEventArgs(string configAssetName, LoadType loadType, string errorMessage, object userData)
        {
            ConfigAssetName = configAssetName;
            LoadType = loadType;
            ErrorMessage = errorMessage;
            UserData = userData;
        }

        /// <summary>
        /// 获取配置资源名称。
        /// </summary>
        public string ConfigAssetName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取配置加载方式。
        /// </summary>
        public LoadType LoadType
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取错误信息。
        /// </summary>
        public string ErrorMessage
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
