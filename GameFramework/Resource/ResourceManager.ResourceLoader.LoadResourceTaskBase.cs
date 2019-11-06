//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace GameFramework.Resource
{
    internal sealed partial class ResourceManager : GameFrameworkModule, IResourceManager
    {
        private sealed partial class ResourceLoader
        {
            private abstract class LoadResourceTaskBase : TaskBase
            {
                private static int s_Serial = 0;

                private string m_AssetName;
                private Type m_AssetType;
                private ResourceInfo m_ResourceInfo;
                private string[] m_DependencyAssetNames;
                private object m_UserData;
                private readonly List<object> m_DependencyAssets;
                private ResourceObject m_ResourceObject;
                private DateTime m_StartTime;
                private int m_TotalDependencyAssetCount;

                public LoadResourceTaskBase()
                {
                    m_AssetName = null;
                    m_AssetType = null;
                    m_ResourceInfo = default(ResourceInfo);
                    m_DependencyAssetNames = null;
                    m_UserData = null;
                    m_DependencyAssets = new List<object>();
                    m_ResourceObject = null;
                    m_StartTime = default(DateTime);
                    m_TotalDependencyAssetCount = 0;
                }

                public string AssetName
                {
                    get
                    {
                        return m_AssetName;
                    }
                }

                public Type AssetType
                {
                    get
                    {
                        return m_AssetType;
                    }
                }

                public ResourceInfo ResourceInfo
                {
                    get
                    {
                        return m_ResourceInfo;
                    }
                }

                public ResourceObject ResourceObject
                {
                    get
                    {
                        return m_ResourceObject;
                    }
                }

                public abstract bool IsScene
                {
                    get;
                }

                public object UserData
                {
                    get
                    {
                        return m_UserData;
                    }
                }

                public DateTime StartTime
                {
                    get
                    {
                        return m_StartTime;
                    }
                    set
                    {
                        m_StartTime = value;
                    }
                }

                public int LoadedDependencyAssetCount
                {
                    get
                    {
                        return m_DependencyAssets.Count;
                    }
                }

                public int TotalDependencyAssetCount
                {
                    get
                    {
                        return m_TotalDependencyAssetCount;
                    }
                    set
                    {
                        m_TotalDependencyAssetCount = value;
                    }
                }

                public override string Description
                {
                    get
                    {
                        return m_AssetName;
                    }
                }

                public override void Clear()
                {
                    base.Clear();
                    m_AssetName = null;
                    m_AssetType = null;
                    m_ResourceInfo = default(ResourceInfo);
                    m_DependencyAssetNames = null;
                    m_UserData = null;
                    m_DependencyAssets.Clear();
                    m_ResourceObject = null;
                    m_StartTime = default(DateTime);
                    m_TotalDependencyAssetCount = 0;
                }

                public string[] GetDependencyAssetNames()
                {
                    return m_DependencyAssetNames;
                }

                public List<object> GetDependencyAssets()
                {
                    return m_DependencyAssets;
                }

                public void LoadMain(LoadResourceAgent agent, ResourceObject resourceObject)
                {
                    m_ResourceObject = resourceObject;
                    agent.Helper.LoadAsset(resourceObject.Target, AssetName, AssetType, IsScene);
                }

                public virtual void OnLoadAssetSuccess(LoadResourceAgent agent, object asset, float duration)
                {
                }

                public virtual void OnLoadAssetFailure(LoadResourceAgent agent, LoadResourceStatus status, string errorMessage)
                {
                }

                public virtual void OnLoadAssetUpdate(LoadResourceAgent agent, LoadResourceProgress type, float progress)
                {
                }

                public virtual void OnLoadDependencyAsset(LoadResourceAgent agent, string dependencyAssetName, object dependencyAsset)
                {
                    m_DependencyAssets.Add(dependencyAsset);
                }

                protected void Initialize(string assetName, Type assetType, int priority, ResourceInfo resourceInfo, string[] dependencyAssetNames, object userData)
                {
                    Initialize(++s_Serial, priority);
                    m_AssetName = assetName;
                    m_AssetType = assetType;
                    m_ResourceInfo = resourceInfo;
                    m_DependencyAssetNames = dependencyAssetNames;
                    m_UserData = userData;
                }
            }
        }
    }
}
