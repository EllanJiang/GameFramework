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
        /// 资源组。
        /// </summary>
        public struct ResourceGroup
        {
            private static readonly int[] EmptyArray = new int[] { };

            private readonly string m_Name;
            private readonly int[] m_ResourceIndexes;
            private readonly int[] m_BinaryIndexes;

            /// <summary>
            /// 初始化资源组的新实例。
            /// </summary>
            /// <param name="name">资源组名称。</param>
            /// <param name="resourceIndexes">资源组包含的普通资源索引集合。</param>
            /// <param name="binaryIndexes">资源组包含的二进制资源索引集合。</param>
            public ResourceGroup(string name, int[] resourceIndexes, int[] binaryIndexes)
            {
                if (name == null)
                {
                    throw new GameFrameworkException("Name is invalid.");
                }

                m_Name = name;
                m_ResourceIndexes = resourceIndexes ?? EmptyArray;
                m_BinaryIndexes = binaryIndexes ?? EmptyArray;
            }

            /// <summary>
            /// 获取资源组名称。
            /// </summary>
            public string Name
            {
                get
                {
                    return m_Name;
                }
            }

            /// <summary>
            /// 获取资源组包含的普通资源索引集合。
            /// </summary>
            /// <returns>资源组包含的普通资源索引集合。</returns>
            public int[] GetResourceIndexes()
            {
                return m_ResourceIndexes;
            }

            /// <summary>
            /// 获取资源组包含的二进制资源索引集合。
            /// </summary>
            /// <returns>资源组包含的二进制资源索引集合。</returns>
            public int[] GetBinaryIndexes()
            {
                return m_BinaryIndexes;
            }
        }
    }
}
