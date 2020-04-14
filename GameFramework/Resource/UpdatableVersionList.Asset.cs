//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Resource
{
    public partial struct UpdatableVersionList
    {
        /// <summary>
        /// 资产。
        /// </summary>
        public struct Asset
        {
            private static readonly int[] EmptyIntArray = new int[] { };

            private readonly string m_Name;
            private readonly int[] m_DependencyAssetIndexes;

            /// <summary>
            /// 初始化资产的新实例。
            /// </summary>
            /// <param name="name">资产名称。</param>
            /// <param name="dependencyAssetIndexes">资产包含的依赖资产索引集合。</param>
            public Asset(string name, int[] dependencyAssetIndexes)
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new GameFrameworkException("Name is invalid.");
                }

                m_Name = name;
                m_DependencyAssetIndexes = dependencyAssetIndexes ?? EmptyIntArray;
            }

            /// <summary>
            /// 获取资产名称。
            /// </summary>
            public string Name
            {
                get
                {
                    return m_Name;
                }
            }

            /// <summary>
            /// 获取资产包含的依赖资产索引集合。
            /// </summary>
            /// <returns></returns>
            public int[] GetDependencyAssetIndexes()
            {
                return m_DependencyAssetIndexes;
            }
        }
    }
}
