//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.ObjectPool;
using System.Collections.Generic;

namespace GameFramework.Resource
{
    internal sealed partial class ResourceManager : GameFrameworkModule, IResourceManager
    {
        private sealed partial class ResourceLoader
        {
            /// <summary>
            /// 资源对象。
            /// </summary>
            private sealed class AssetObject : ObjectBase
            {
                private readonly object[] m_DependencyAssets;
                private readonly object m_Resource;
                private readonly IObjectPool<AssetObject> m_AssetPool;
                private readonly IObjectPool<ResourceObject> m_ResourcePool;
                private readonly IResourceHelper m_ResourceHelper;
                private readonly Dictionary<object, int> m_AssetDependencyCount;

                public AssetObject(string name, object target, object[] dependencyAssets, object resource, IObjectPool<AssetObject> assetPool, IObjectPool<ResourceObject> resourcePool, IResourceHelper resourceHelper, Dictionary<object, int> assetDependencyCount)
                    : base(name, target)
                {
                    if (dependencyAssets == null)
                    {
                        throw new GameFrameworkException("Dependency assets is invalid.");
                    }

                    if (resource == null)
                    {
                        throw new GameFrameworkException("Resource is invalid.");
                    }

                    if (assetPool == null)
                    {
                        throw new GameFrameworkException("Asset pool is invalid.");
                    }

                    if (resourcePool == null)
                    {
                        throw new GameFrameworkException("Resource pool is invalid.");
                    }

                    if (resourceHelper == null)
                    {
                        throw new GameFrameworkException("Resource helper is invalid.");
                    }

                    if (assetDependencyCount == null)
                    {
                        throw new GameFrameworkException("Asset dependency count is invalid.");
                    }

                    m_DependencyAssets = dependencyAssets;
                    m_Resource = resource;
                    m_AssetPool = assetPool;
                    m_ResourcePool = resourcePool;
                    m_ResourceHelper = resourceHelper;
                    m_AssetDependencyCount = assetDependencyCount;

                    foreach (object dependencyAsset in m_DependencyAssets)
                    {
                        int referenceCount = 0;
                        if (m_AssetDependencyCount.TryGetValue(dependencyAsset, out referenceCount))
                        {
                            m_AssetDependencyCount[dependencyAsset] = referenceCount + 1;
                        }
                        else
                        {
                            m_AssetDependencyCount.Add(dependencyAsset, 1);
                        }
                    }
                }

                public override bool CustomCanReleaseFlag
                {
                    get
                    {
                        int targetReferenceCount = 0;
                        m_AssetDependencyCount.TryGetValue(Target, out targetReferenceCount);
                        return base.CustomCanReleaseFlag && targetReferenceCount <= 0;
                    }
                }

                protected internal override void OnUnspawn()
                {
                    base.OnUnspawn();
                    foreach (object dependencyAsset in m_DependencyAssets)
                    {
                        m_AssetPool.Unspawn(dependencyAsset);
                    }
                }

                protected internal override void Release(bool isShutdown)
                {
                    if (!isShutdown)
                    {
                        int targetReferenceCount = 0;
                        if (m_AssetDependencyCount.TryGetValue(Target, out targetReferenceCount) && targetReferenceCount > 0)
                        {
                            throw new GameFrameworkException(Utility.Text.Format("Asset target '{0}' reference count is '{1}' larger than 0.", Name, targetReferenceCount.ToString()));
                        }

                        foreach (object dependencyAsset in m_DependencyAssets)
                        {
                            int referenceCount = 0;
                            if (m_AssetDependencyCount.TryGetValue(dependencyAsset, out referenceCount))
                            {
                                m_AssetDependencyCount[dependencyAsset] = referenceCount - 1;
                            }
                            else
                            {
                                throw new GameFrameworkException(Utility.Text.Format("Asset target '{0}' dependency asset reference count is invalid.", Name));
                            }
                        }

                        m_ResourcePool.Unspawn(m_Resource);
                    }

                    m_AssetDependencyCount.Remove(Target);
                    m_ResourceHelper.Release(Target);
                }
            }
        }
    }
}
