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
        public LoadConfigFailureEventArgs()
        {
            ConfigAssetName = null;
            LoadType = LoadType.Text;
            ErrorMessage = null;
            UserData = null;
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

        /// <summary>
        /// 创建加载配置失败事件。
        /// </summary>
        /// <param name="configAssetName">配置资源名称。</param>
        /// <param name="loadType">配置加载方式。</param>
        /// <param name="errorMessage">错误信息。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>创建的加载配置失败事件。</returns>
        public static LoadConfigFailureEventArgs Create(string configAssetName, LoadType loadType, string errorMessage, object userData)
        {
            LoadConfigFailureEventArgs loadConfigFailureEventArgs = ReferencePool.Acquire<LoadConfigFailureEventArgs>();
            loadConfigFailureEventArgs.ConfigAssetName = configAssetName;
            loadConfigFailureEventArgs.LoadType = loadType;
            loadConfigFailureEventArgs.ErrorMessage = errorMessage;
            loadConfigFailureEventArgs.UserData = userData;
            return loadConfigFailureEventArgs;
        }

        /// <summary>
        /// 清理加载配置失败事件。
        /// </summary>
        public override void Clear()
        {
            ConfigAssetName = null;
            LoadType = LoadType.Text;
            ErrorMessage = null;
            UserData = null;
        }
    }
}
