//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Resource
{
    internal partial class ResourceManager
    {
        private partial class ResourceLoader
        {
            private sealed class LoadSceneTask : LoadResourceTaskBase
            {
                private readonly LoadSceneCallbacks m_LoadSceneCallbacks;

                public LoadSceneTask(string sceneAssetName, ResourceInfo resourceInfo, string[] dependencyAssetNames, string[] scatteredDependencyAssetNames, string resourceChildName, LoadSceneCallbacks loadSceneCallbacks, object userData)
                    : base(sceneAssetName, resourceInfo, dependencyAssetNames, scatteredDependencyAssetNames, resourceChildName, userData)
                {
                    m_LoadSceneCallbacks = loadSceneCallbacks;
                }

                public override bool IsInstantiate
                {
                    get
                    {
                        return false;
                    }
                }

                public override bool IsScene
                {
                    get
                    {
                        return true;
                    }
                }

                public override void OnLoadSuccess(LoadResourceAgent agent, object asset, object instance, float duration)
                {
                    base.OnLoadSuccess(agent, asset, instance, duration);
                    m_LoadSceneCallbacks.LoadSceneSuccessCallback?.Invoke(AssetName, duration, UserData);
                }

                public override void OnLoadFailure(LoadResourceAgent agent, LoadResourceStatus status, string errorMessage)
                {
                    base.OnLoadFailure(agent, status, errorMessage);
                    m_LoadSceneCallbacks.LoadSceneFailureCallback?.Invoke(AssetName, status, errorMessage, UserData);
                }

                public override void OnLoadUpdate(LoadResourceAgent agent, LoadResourceProgress type, float progress)
                {
                    base.OnLoadUpdate(agent, type, progress);
                    if (type == LoadResourceProgress.LoadScene)
                    {
                        m_LoadSceneCallbacks.LoadSceneUpdateCallback?.Invoke(AssetName, progress, UserData);
                    }
                }

                public override void OnLoadDependencyAsset(LoadResourceAgent agent, string dependencyAssetName, object dependencyAsset)
                {
                    base.OnLoadDependencyAsset(agent, dependencyAssetName, dependencyAsset);
                    m_LoadSceneCallbacks.LoadSceneDependencyAssetCallback?.Invoke(AssetName, dependencyAssetName, LoadedDependencyAssetCount, TotalDependencyAssetCount, UserData);
                }
            }
        }
    }
}
