//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Resource
{
    internal sealed partial class ResourceManager : GameFrameworkModule, IResourceManager
    {
        /// <summary>
        /// 资源信息。
        /// </summary>
        private sealed class AssetInfo
        {
            private readonly string m_AssetName;
            private readonly ResourceName m_ResourceName;
            private readonly string[] m_DependencyAssetNames;

            /// <summary>
            /// 初始化资源信息的新实例。
            /// </summary>
            /// <param name="assetName">资源名称。</param>
            /// <param name="resourceName">所在资源名称。</param>
            /// <param name="dependencyAssetNames">依赖资源名称。</param>
            public AssetInfo(string assetName, ResourceName resourceName, string[] dependencyAssetNames)
            {
                m_AssetName = assetName;
                m_ResourceName = resourceName;
                m_DependencyAssetNames = dependencyAssetNames;
            }

            /// <summary>
            /// 获取资源名称。
            /// </summary>
            public string AssetName
            {
                get
                {
                    return m_AssetName;
                }
            }

            /// <summary>
            /// 获取所在资源名称。
            /// </summary>
            public ResourceName ResourceName
            {
                get
                {
                    return m_ResourceName;
                }
            }

            /// <summary>
            /// 获取依赖资源名称。
            /// </summary>
            /// <returns>依赖资源名称。</returns>
            public string[] GetDependencyAssetNames()
            {
                return m_DependencyAssetNames;
            }
        }
    }
}
