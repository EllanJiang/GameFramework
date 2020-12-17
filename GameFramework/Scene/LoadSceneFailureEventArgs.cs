//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Scene
{
    /// <summary>
    /// 加载场景失败事件。
    /// </summary>
    public sealed class LoadSceneFailureEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化加载场景失败事件的新实例。
        /// </summary>
        public LoadSceneFailureEventArgs()
        {
            SceneAssetName = null;
            ErrorMessage = null;
            UserData = null;
        }

        /// <summary>
        /// 获取场景资源名称。
        /// </summary>
        public string SceneAssetName
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
        /// 创建加载场景失败事件。
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称。</param>
        /// <param name="errorMessage">错误信息。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>创建的加载场景失败事件。</returns>
        public static LoadSceneFailureEventArgs Create(string sceneAssetName, string errorMessage, object userData)
        {
            LoadSceneFailureEventArgs loadSceneFailureEventArgs = ReferencePool.Acquire<LoadSceneFailureEventArgs>();
            loadSceneFailureEventArgs.SceneAssetName = sceneAssetName;
            loadSceneFailureEventArgs.ErrorMessage = errorMessage;
            loadSceneFailureEventArgs.UserData = userData;
            return loadSceneFailureEventArgs;
        }

        /// <summary>
        /// 清理加载场景失败事件。
        /// </summary>
        public override void Clear()
        {
            SceneAssetName = null;
            ErrorMessage = null;
            UserData = null;
        }
    }
}
