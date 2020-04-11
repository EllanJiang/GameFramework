//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Resource
{
    /// <summary>
    /// 本地版本资源列表。
    /// </summary>
    public partial struct LocalVersionList
    {
        private static readonly Resource[] EmptyResourceArray = new Resource[] { };
        private static readonly Binary[] EmptyBinaryArray = new Binary[] { };

        private readonly bool m_IsValid;
        private readonly Resource[] m_Resources;
        private readonly Binary[] m_Binaries;

        /// <summary>
        /// 初始化本地版本资源列表的新实例。
        /// </summary>
        /// <param name="resources">包含的普通资源集合。</param>
        /// <param name="binaries">包含的二进制资源集合。</param>
        public LocalVersionList(Resource[] resources, Binary[] binaries)
        {
            m_IsValid = true;
            m_Resources = resources ?? EmptyResourceArray;
            m_Binaries = binaries ?? EmptyBinaryArray;
        }

        /// <summary>
        /// 获取本地版本资源列表是否有效。
        /// </summary>
        public bool IsValid
        {
            get
            {
                return m_IsValid;
            }
        }

        /// <summary>
        /// 获取普通资源集合。
        /// </summary>
        /// <returns></returns>
        public Resource[] GetResources()
        {
            return m_Resources;
        }

        /// <summary>
        /// 获取二进制资源集合。
        /// </summary>
        /// <returns></returns>
        public Binary[] GetBinaries()
        {
            return m_Binaries;
        }
    }
}
