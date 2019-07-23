//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Resource
{
    internal sealed partial class ResourceManager : GameFrameworkModule, IResourceManager
    {
        private sealed partial class ResourceLoader
        {
            private sealed class LoadDependencyAssetTask : LoadResourceTaskBase
            {
                private LoadResourceTaskBase m_MainTask;

                public LoadDependencyAssetTask()
                {
                    m_MainTask = null;
                }

                public override bool IsScene
                {
                    get
                    {
                        return false;
                    }
                }

                public void Initialize(string assetName, int priority, ResourceInfo resourceInfo, string[] dependencyAssetNames, LoadResourceTaskBase mainTask, object userData)
                {
                    base.Initialize(assetName, null, priority, resourceInfo, dependencyAssetNames, userData);
                    m_MainTask = mainTask;
                    m_MainTask.TotalDependencyAssetCount++;
                }

                public override void Clear()
                {
                    base.Clear();
                    m_MainTask = null;
                }

                public override void OnLoadAssetSuccess(LoadResourceAgent agent, object asset, float duration)
                {
                    base.OnLoadAssetSuccess(agent, asset, duration);
                    m_MainTask.OnLoadDependencyAsset(agent, AssetName, asset);
                }

                public override void OnLoadAssetFailure(LoadResourceAgent agent, LoadResourceStatus status, string errorMessage)
                {
                    base.OnLoadAssetFailure(agent, status, errorMessage);
                    m_MainTask.OnLoadAssetFailure(agent, LoadResourceStatus.DependencyError, Utility.Text.Format("Can not load dependency asset '{0}', internal status '{1}', internal error message '{2}'.", AssetName, status.ToString(), errorMessage));
                }
            }
        }
    }
}
