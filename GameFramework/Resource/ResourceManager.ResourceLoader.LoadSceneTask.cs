//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
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

                public LoadSceneTask(string sceneAssetName, int priority, ResourceInfo resourceInfo, string resourceChildName, string[] dependencyAssetNames, string[] scatteredDependencyAssetNames, LoadSceneCallbacks loadSceneCallbacks, object userData)
                    : base(sceneAssetName, null, priority, resourceInfo, resourceChildName, dependencyAssetNames, scatteredDependencyAssetNames, userData)
                {
                    m_LoadSceneCallbacks = loadSceneCallbacks;
                }

                public override bool IsScene
                {
                    get
                    {
                        return true;
                    }
                }

                public override void OnLoadAssetSuccess(LoadResourceAgent agent, object asset, float duration)
                {
                    base.OnLoadAssetSuccess(agent, asset, duration);
                    if (m_LoadSceneCallbacks.LoadSceneSuccessCallback != null)
                    {
                        m_LoadSceneCallbacks.LoadSceneSuccessCallback(AssetName, duration, UserData);
                    }
                }

                public override void OnLoadAssetFailure(LoadResourceAgent agent, LoadResourceStatus status, string errorMessage)
                {
                    base.OnLoadAssetFailure(agent, status, errorMessage);
                    if (m_LoadSceneCallbacks.LoadSceneFailureCallback != null)
                    {
                        m_LoadSceneCallbacks.LoadSceneFailureCallback(AssetName, status, errorMessage, UserData);
                    }
                }

                public override void OnLoadAssetUpdate(LoadResourceAgent agent, LoadResourceProgress type, float progress)
                {
                    base.OnLoadAssetUpdate(agent, type, progress);
                    if (type == LoadResourceProgress.LoadScene)
                    {
                        if (m_LoadSceneCallbacks.LoadSceneUpdateCallback != null)
                        {
                            m_LoadSceneCallbacks.LoadSceneUpdateCallback(AssetName, progress, UserData);
                        }
                    }
                }

                public override void OnLoadDependencyAsset(LoadResourceAgent agent, string dependencyAssetName, object dependencyAsset)
                {
                    base.OnLoadDependencyAsset(agent, dependencyAssetName, dependencyAsset);
                    if (m_LoadSceneCallbacks.LoadSceneDependencyAssetCallback != null)
                    {
                        m_LoadSceneCallbacks.LoadSceneDependencyAssetCallback(AssetName, dependencyAssetName, LoadedDependencyAssetCount, TotalDependencyAssetCount, UserData);
                    }
                }
            }
        }
    }
}
