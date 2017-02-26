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
            private sealed class LoadDependencyAssetTask : LoadResourceTaskBase
            {
                private readonly LoadResourceTaskBase m_MainTask;

                public LoadDependencyAssetTask(string assetName, ResourceInfo resourceInfo, string[] dependencyAssetNames, string[] scatteredDependencyAssetNames, string resourceChildName, LoadResourceTaskBase mainTask, object userData)
                    : base(assetName, resourceInfo, dependencyAssetNames, scatteredDependencyAssetNames, resourceChildName, userData)
                {
                    m_MainTask = mainTask;
                    m_MainTask.TotalDependencyAssetCount++;
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
                    m_MainTask.OnLoadDependencyAsset(agent, AssetName, asset);
                }

                public override void OnLoadFailure(LoadResourceAgent agent, LoadResourceStatus status, string errorMessage)
                {
                    base.OnLoadFailure(agent, status, errorMessage);
                    m_MainTask.OnLoadFailure(agent, LoadResourceStatus.DependencyError, string.Format("Can not load dependency asset '{0}', internal status '{1}', internal error message '{2}'.", AssetName, status.ToString(), errorMessage));
                }
            }
        }
    }
}
