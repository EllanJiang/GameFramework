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
            private sealed class LoadAssetTask : LoadResourceTaskBase
            {
                private readonly LoadAssetCallbacks m_LoadAssetCallbacks;

                public LoadAssetTask(string assetName, ResourceInfo resourceInfo, string[] dependencyAssetNames, string[] scatteredDependencyAssetNames, string resourceChildName, LoadAssetCallbacks loadAssetCallbacks, object userData)
                    : base(assetName, resourceInfo, dependencyAssetNames, scatteredDependencyAssetNames, resourceChildName, userData)
                {
                    m_LoadAssetCallbacks = loadAssetCallbacks;
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
                        return false;
                    }
                }

                public override void OnLoadSuccess(LoadResourceAgent agent, object asset, object instance, float duration)
                {
                    base.OnLoadSuccess(agent, asset, instance, duration);
                    m_LoadAssetCallbacks.LoadAssetSuccessCallback?.Invoke(AssetName, asset, duration, UserData);
                }

                public override void OnLoadFailure(LoadResourceAgent agent, LoadResourceStatus status, string errorMessage)
                {
                    base.OnLoadFailure(agent, status, errorMessage);
                    m_LoadAssetCallbacks.LoadAssetFailureCallback?.Invoke(AssetName, status, errorMessage, UserData);
                }

                public override void OnLoadUpdate(LoadResourceAgent agent, LoadResourceProgress type, float progress)
                {
                    base.OnLoadUpdate(agent, type, progress);
                    if (type == LoadResourceProgress.LoadAsset)
                    {
                        m_LoadAssetCallbacks.LoadAssetUpdateCallback?.Invoke(AssetName, progress, UserData);
                    }
                }

                public override void OnLoadDependencyAsset(LoadResourceAgent agent, string dependencyAssetName, object dependencyAsset)
                {
                    base.OnLoadDependencyAsset(agent, dependencyAssetName, dependencyAsset);
                    m_LoadAssetCallbacks.LoadAssetDependencyAssetCallback?.Invoke(AssetName, dependencyAssetName, LoadedDependencyAssetCount, TotalDependencyAssetCount, UserData);
                }
            }
        }
    }
}
