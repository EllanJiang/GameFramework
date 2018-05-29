//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.ObjectPool;
using System.Collections.Generic;

namespace GameFramework.Resource
{
    internal partial class ResourceManager
    {
        private partial class ResourceLoader
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
                private readonly Dictionary<object, int> m_DependencyCount;

                public AssetObject(string name, object target, object[] dependencyAssets, object resource, IObjectPool<AssetObject> assetPool, IObjectPool<ResourceObject> resourcePool, IResourceHelper resourceHelper, Dictionary<object, int> dependencyCount)
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

                    if (dependencyCount == null)
                    {
                        throw new GameFrameworkException("Dependency count is invalid.");
                    }

                    m_DependencyAssets = dependencyAssets;
                    m_Resource = resource;
                    m_AssetPool = assetPool;
                    m_ResourcePool = resourcePool;
                    m_ResourceHelper = resourceHelper;
                    m_DependencyCount = dependencyCount;

                    foreach (object dependencyAsset in m_DependencyAssets)
                    {
                        int referenceCount = 0;
                        if (m_DependencyCount.TryGetValue(dependencyAsset, out referenceCount))
                        {
                            m_DependencyCount[dependencyAsset] = referenceCount + 1;
                        }
                        else
                        {
                            m_DependencyCount.Add(dependencyAsset, 1);
                        }
                    }
                }

                public override bool CustomCanReleaseFlag
                {
                    get
                    {
                        int targetReferenceCount = 0;
                        m_DependencyCount.TryGetValue(Target, out targetReferenceCount);
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
                        if (m_DependencyCount.TryGetValue(Target, out targetReferenceCount) && targetReferenceCount > 0)
                        {
                            throw new GameFrameworkException(string.Format("Target '{0}' dependency asset reference count is '{1}' larger than 0.", Name, targetReferenceCount.ToString()));
                        }

                        foreach (object dependencyAsset in m_DependencyAssets)
                        {
                            int referenceCount = 0;
                            if (m_DependencyCount.TryGetValue(dependencyAsset, out referenceCount))
                            {
                                m_DependencyCount[dependencyAsset] = referenceCount - 1;
                            }
                            else
                            {
                                throw new GameFrameworkException(string.Format("Target '{0}' dependency asset reference count is invalid.", Name));
                            }
                        }
                    }

                    m_DependencyCount.Remove(Target);
                    m_ResourceHelper.Release(Target);
                    m_ResourcePool.Unspawn(m_Resource);
                }
            }
        }
    }
}
