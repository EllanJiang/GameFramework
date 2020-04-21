//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Resource
{
    /// <summary>
    /// 单机模式版本资源列表。
    /// </summary>
    public partial struct PackageVersionList
    {
        private static readonly Asset[] EmptyAssetArray = new Asset[] { };
        private static readonly Resource[] EmptyResourceArray = new Resource[] { };
        private static readonly ResourceGroup[] EmptyResourceGroupArray = new ResourceGroup[] { };

        private readonly bool m_IsValid;
        private readonly string m_ApplicableGameVersion;
        private readonly int m_InternalResourceVersion;
        private readonly Asset[] m_Assets;
        private readonly Resource[] m_Resources;
        private readonly ResourceGroup[] m_ResourceGroups;

        /// <summary>
        /// 初始化单机模式版本资源列表的新实例。
        /// </summary>
        /// <param name="applicableGameVersion">适配的游戏版本号。</param>
        /// <param name="internalResourceVersion">内部资源版本号。</param>
        /// <param name="assets">包含的资源集合。</param>
        /// <param name="resources">包含的资源集合。</param>
        /// <param name="resourceGroups">包含的资源组集合。</param>
        public PackageVersionList(string applicableGameVersion, int internalResourceVersion, Asset[] assets, Resource[] resources, ResourceGroup[] resourceGroups)
        {
            m_IsValid = true;
            m_ApplicableGameVersion = applicableGameVersion;
            m_InternalResourceVersion = internalResourceVersion;
            m_Assets = assets ?? EmptyAssetArray;
            m_Resources = resources ?? EmptyResourceArray;
            m_ResourceGroups = resourceGroups ?? EmptyResourceGroupArray;
        }

        /// <summary>
        /// 获取单机模式版本资源列表是否有效。
        /// </summary>
        public bool IsValid
        {
            get
            {
                return m_IsValid;
            }
        }

        /// <summary>
        /// 获取适配的游戏版本号。
        /// </summary>
        public string ApplicableGameVersion
        {
            get
            {
                return m_ApplicableGameVersion;
            }
        }

        /// <summary>
        /// 获取内部资源版本号。
        /// </summary>
        public int InternalResourceVersion
        {
            get
            {
                return m_InternalResourceVersion;
            }
        }

        /// <summary>
        /// 获取包含的资源集合。
        /// </summary>
        /// <returns></returns>
        public Asset[] GetAssets()
        {
            return m_Assets;
        }

        /// <summary>
        /// 获取包含的资源集合。
        /// </summary>
        /// <returns></returns>
        public Resource[] GetResources()
        {
            return m_Resources;
        }

        /// <summary>
        /// 获取包含的资源组集合。
        /// </summary>
        /// <returns></returns>
        public ResourceGroup[] GetResourceGroups()
        {
            return m_ResourceGroups;
        }
    }
}
