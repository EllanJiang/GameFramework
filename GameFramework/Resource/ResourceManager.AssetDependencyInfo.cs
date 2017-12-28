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
        /// <summary>
        /// 资源依赖信息。
        /// </summary>
        private struct AssetDependencyInfo
        {
            private readonly string m_AssetName;
            private readonly string[] m_DependencyAssetNames;
            private readonly string[] m_ScatteredDependencyAssetNames;

            /// <summary>
            /// 初始化资源依赖信息的新实例。
            /// </summary>
            /// <param name="assetName">资源名称。</param>
            /// <param name="dependencyAssetNames">依赖资源名称。</param>
            /// <param name="scatteredDependencyAssetNames">依赖零散资源名称。</param>
            public AssetDependencyInfo(string assetName, string[] dependencyAssetNames, string[] scatteredDependencyAssetNames)
            {
                m_AssetName = assetName;
                m_DependencyAssetNames = dependencyAssetNames;
                m_ScatteredDependencyAssetNames = scatteredDependencyAssetNames;
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
            /// 获取依赖资源名称。
            /// </summary>
            /// <returns>依赖资源名称。</returns>
            public string[] GetDependencyAssetNames()
            {
                return m_DependencyAssetNames;
            }

            /// <summary>
            /// 获取依赖零散资源名称。
            /// </summary>
            /// <returns>依赖零散资源名称。</returns>
            public string[] GetScatteredDependencyAssetNames()
            {
                return m_ScatteredDependencyAssetNames;
            }
        }
    }
}
