//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework.Resource;
using System;
using System.Collections.Generic;

namespace GameFramework.Scene
{
    /// <summary>
    /// 场景管理器接口。
    /// </summary>
    public interface ISceneManager
    {
        /// <summary>
        /// 加载场景成功事件。
        /// </summary>
        event EventHandler<LoadSceneSuccessEventArgs> LoadSceneSuccess;

        /// <summary>
        /// 加载场景失败事件。
        /// </summary>
        event EventHandler<LoadSceneFailureEventArgs> LoadSceneFailure;

        /// <summary>
        /// 加载场景更新事件。
        /// </summary>
        event EventHandler<LoadSceneUpdateEventArgs> LoadSceneUpdate;

        /// <summary>
        /// 加载场景时加载依赖资源事件。
        /// </summary>
        event EventHandler<LoadSceneDependencyAssetEventArgs> LoadSceneDependencyAsset;

        /// <summary>
        /// 卸载场景成功事件。
        /// </summary>
        event EventHandler<UnloadSceneSuccessEventArgs> UnloadSceneSuccess;

        /// <summary>
        /// 卸载场景失败事件。
        /// </summary>
        event EventHandler<UnloadSceneFailureEventArgs> UnloadSceneFailure;

        /// <summary>
        /// 设置资源管理器。
        /// </summary>
        /// <param name="resourceManager">资源管理器。</param>
        void SetResourceManager(IResourceManager resourceManager);

        /// <summary>
        /// 获取场景是否已加载。
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称。</param>
        /// <returns>场景是否已加载。</returns>
        bool SceneIsLoaded(string sceneAssetName);

        /// <summary>
        /// 获取已加载场景的资源名称。
        /// </summary>
        /// <returns>已加载场景的资源名称。</returns>
        string[] GetLoadedSceneAssetNames();

        /// <summary>
        /// 获取已加载场景的资源名称。
        /// </summary>
        /// <param name="results">已加载场景的资源名称。</param>
        void GetLoadedSceneAssetNames(List<string> results);

        /// <summary>
        /// 获取场景是否正在加载。
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称。</param>
        /// <returns>场景是否正在加载。</returns>
        bool SceneIsLoading(string sceneAssetName);

        /// <summary>
        /// 获取正在加载场景的资源名称。
        /// </summary>
        /// <returns>正在加载场景的资源名称。</returns>
        string[] GetLoadingSceneAssetNames();

        /// <summary>
        /// 获取正在加载场景的资源名称。
        /// </summary>
        /// <param name="results">正在加载场景的资源名称。</param>
        void GetLoadingSceneAssetNames(List<string> results);

        /// <summary>
        /// 获取场景是否正在卸载。
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称。</param>
        /// <returns>场景是否正在卸载。</returns>
        bool SceneIsUnloading(string sceneAssetName);

        /// <summary>
        /// 获取正在卸载场景的资源名称。
        /// </summary>
        /// <returns>正在卸载场景的资源名称。</returns>
        string[] GetUnloadingSceneAssetNames();

        /// <summary>
        /// 获取正在卸载场景的资源名称。
        /// </summary>
        /// <param name="results">正在卸载场景的资源名称。</param>
        void GetUnloadingSceneAssetNames(List<string> results);

        /// <summary>
        /// 检查场景资源是否存在。
        /// </summary>
        /// <param name="sceneAssetName">要检查场景资源的名称。</param>
        /// <returns>场景资源是否存在。</returns>
        bool HasScene(string sceneAssetName);

        /// <summary>
        /// 加载场景。
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称。</param>
        void LoadScene(string sceneAssetName);

        /// <summary>
        /// 加载场景。
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称。</param>
        /// <param name="priority">加载场景资源的优先级。</param>
        void LoadScene(string sceneAssetName, int priority);

        /// <summary>
        /// 加载场景。
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称。</param>
        /// <param name="userData">用户自定义数据。</param>
        void LoadScene(string sceneAssetName, object userData);

        /// <summary>
        /// 加载场景。
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称。</param>
        /// <param name="priority">加载场景资源的优先级。</param>
        /// <param name="userData">用户自定义数据。</param>
        void LoadScene(string sceneAssetName, int priority, object userData);

        /// <summary>
        /// 卸载场景。
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称。</param>
        void UnloadScene(string sceneAssetName);

        /// <summary>
        /// 卸载场景。
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称。</param>
        /// <param name="userData">用户自定义数据。</param>
        void UnloadScene(string sceneAssetName, object userData);
    }
}
