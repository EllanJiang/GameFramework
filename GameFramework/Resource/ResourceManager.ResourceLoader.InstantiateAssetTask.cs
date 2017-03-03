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
            private sealed class InstantiateAssetTask : LoadResourceTaskBase
            {
                private readonly InstantiateAssetCallbacks m_InstantiateAssetCallbacks;

                public InstantiateAssetTask(string assetName, ResourceInfo resourceInfo, string[] dependencyAssetNames, string[] scatteredDependencyAssetNames, string resourceChildName, InstantiateAssetCallbacks instantiateAssetCallbacks, object userData)
                    : base(assetName, resourceInfo, dependencyAssetNames, scatteredDependencyAssetNames, resourceChildName, userData)
                {
                    m_InstantiateAssetCallbacks = instantiateAssetCallbacks;
                }

                public override bool IsInstantiate
                {
                    get
                    {
                        return true;
                    }
                }

                public override bool IsScene
                {
                    get
                    {
                        return false;
                    }
                }

                public override void OnLoadSuccess(LoadResourceAgent agent, object asset, object instance, float duration)
                {
                    base.OnLoadSuccess(agent, asset, instance, duration);
                    if (m_InstantiateAssetCallbacks.InstantiateAssetSuccessCallback != null)
                    {
                        m_InstantiateAssetCallbacks.InstantiateAssetSuccessCallback(AssetName, instance, duration, UserData);
                    }
                }

                public override void OnLoadFailure(LoadResourceAgent agent, LoadResourceStatus status, string errorMessage)
                {
                    base.OnLoadFailure(agent, status, errorMessage);
                    if (m_InstantiateAssetCallbacks.InstantiateAssetFailureCallback != null)
                    {
                        m_InstantiateAssetCallbacks.InstantiateAssetFailureCallback(AssetName, status, errorMessage, UserData);
                    }
                }

                public override void OnLoadUpdate(LoadResourceAgent agent, LoadResourceProgress type, float progress)
                {
                    base.OnLoadUpdate(agent, type, progress);
                    if (type == LoadResourceProgress.LoadAsset)
                    {
                        if (m_InstantiateAssetCallbacks.InstantiateAssetUpdateCallback != null)
                        {
                            m_InstantiateAssetCallbacks.InstantiateAssetUpdateCallback(AssetName, progress, UserData);
                        }
                    }
                }

                public override void OnLoadDependencyAsset(LoadResourceAgent agent, string dependencyAssetName, object dependencyAsset)
                {
                    base.OnLoadDependencyAsset(agent, dependencyAssetName, dependencyAsset);
                    if (m_InstantiateAssetCallbacks.InstantiateAssetDependencyAssetCallback != null)
                    {
                        m_InstantiateAssetCallbacks.InstantiateAssetDependencyAssetCallback(AssetName, dependencyAssetName, LoadedDependencyAssetCount, TotalDependencyAssetCount, UserData);
                    }
                }
            }
        }
    }
}
