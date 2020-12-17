//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System.Runtime.InteropServices;

namespace GameFramework.Resource
{
    public partial struct UpdatableVersionList
    {
        /// <summary>
        /// 资源。
        /// </summary>
        [StructLayout(LayoutKind.Auto)]
        public struct Asset
        {
            private static readonly int[] EmptyIntArray = new int[] { };

            private readonly string m_Name;
            private readonly int[] m_DependencyAssetIndexes;

            /// <summary>
            /// 初始化资源的新实例。
            /// </summary>
            /// <param name="name">资源名称。</param>
            /// <param name="dependencyAssetIndexes">资源包含的依赖资源索引集合。</param>
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
            /// 获取资源名称。
            /// </summary>
            public string Name
            {
                get
                {
                    return m_Name;
                }
            }

            /// <summary>
            /// 获取资源包含的依赖资源索引集合。
            /// </summary>
            /// <returns>资源包含的依赖资源索引集合。</returns>
            public int[] GetDependencyAssetIndexes()
            {
                return m_DependencyAssetIndexes;
            }
        }
    }
}
