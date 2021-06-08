//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System.Collections.Generic;

namespace GameFramework.Resource
{
    internal sealed partial class ResourceManager : GameFrameworkModule, IResourceManager
    {
        /// <summary>
        /// 资源组集合。
        /// </summary>
        private sealed class ResourceGroupCollection : IResourceGroupCollection
        {
            private readonly ResourceGroup[] m_ResourceGroups;
            private readonly Dictionary<ResourceName, ResourceInfo> m_ResourceInfos;
            private readonly HashSet<ResourceName> m_ResourceNames;
            private long m_TotalLength;
            private long m_TotalCompressedLength;

            /// <summary>
            /// 初始化资源组集合的新实例。
            /// </summary>
            /// <param name="resourceGroups">资源组集合。</param>
            /// <param name="resourceInfos">资源信息引用。</param>
            public ResourceGroupCollection(ResourceGroup[] resourceGroups, Dictionary<ResourceName, ResourceInfo> resourceInfos)
            {
                if (resourceGroups == null || resourceGroups.Length < 1)
                {
                    throw new GameFrameworkException("Resource groups is invalid.");
                }

                if (resourceInfos == null)
                {
                    throw new GameFrameworkException("Resource infos is invalid.");
                }

                int lastIndex = resourceGroups.Length - 1;
                for (int i = 0; i < lastIndex; i++)
                {
                    if (resourceGroups[i] == null)
                    {
                        throw new GameFrameworkException(Utility.Text.Format("Resource group index '{0}' is invalid.", i));
                    }

                    for (int j = i + 1; j < resourceGroups.Length; j++)
                    {
                        if (resourceGroups[i] == resourceGroups[j])
                        {
                            throw new GameFrameworkException(Utility.Text.Format("Resource group '{0}' duplicated.", resourceGroups[i].Name));
                        }
                    }
                }

                if (resourceGroups[lastIndex] == null)
                {
                    throw new GameFrameworkException(Utility.Text.Format("Resource group index '{0}' is invalid.", lastIndex));
                }

                m_ResourceGroups = resourceGroups;
                m_ResourceInfos = resourceInfos;
                m_ResourceNames = new HashSet<ResourceName>();
                m_TotalLength = 0L;
                m_TotalCompressedLength = 0L;

                List<ResourceName> cachedResourceNames = new List<ResourceName>();
                foreach (ResourceGroup resourceGroup in m_ResourceGroups)
                {
                    resourceGroup.InternalGetResourceNames(cachedResourceNames);
                    foreach (ResourceName resourceName in cachedResourceNames)
                    {
                        ResourceInfo resourceInfo = null;
                        if (!m_ResourceInfos.TryGetValue(resourceName, out resourceInfo))
                        {
                            throw new GameFrameworkException(Utility.Text.Format("Resource info '{0}' is invalid.", resourceName.FullName));
                        }

                        if (m_ResourceNames.Add(resourceName))
                        {
                            m_TotalLength += resourceInfo.Length;
                            m_TotalCompressedLength += resourceInfo.CompressedLength;
                        }
                    }
                }
            }

            /// <summary>
            /// 获取资源组集合是否准备完毕。
            /// </summary>
            public bool Ready
            {
                get
                {
                    return ReadyCount >= TotalCount;
                }
            }

            /// <summary>
            /// 获取资源组集合包含资源数量。
            /// </summary>
            public int TotalCount
            {
                get
                {
                    return m_ResourceNames.Count;
                }
            }

            /// <summary>
            /// 获取资源组集合中已准备完成资源数量。
            /// </summary>
            public int ReadyCount
            {
                get
                {
                    int readyCount = 0;
                    foreach (ResourceName resourceName in m_ResourceNames)
                    {
                        ResourceInfo resourceInfo = null;
                        if (m_ResourceInfos.TryGetValue(resourceName, out resourceInfo) && resourceInfo.Ready)
                        {
                            readyCount++;
                        }
                    }

                    return readyCount;
                }
            }

            /// <summary>
            /// 获取资源组集合包含资源的总大小。
            /// </summary>
            public long TotalLength
            {
                get
                {
                    return m_TotalLength;
                }
            }

            /// <summary>
            /// 获取资源组集合包含资源压缩后的总大小。
            /// </summary>
            public long TotalCompressedLength
            {
                get
                {
                    return m_TotalCompressedLength;
                }
            }

            /// <summary>
            /// 获取资源组集合中已准备完成资源的总大小。
            /// </summary>
            public long ReadyLength
            {
                get
                {
                    long readyLength = 0L;
                    foreach (ResourceName resourceName in m_ResourceNames)
                    {
                        ResourceInfo resourceInfo = null;
                        if (m_ResourceInfos.TryGetValue(resourceName, out resourceInfo) && resourceInfo.Ready)
                        {
                            readyLength += resourceInfo.Length;
                        }
                    }

                    return readyLength;
                }
            }

            /// <summary>
            /// 获取资源组集合中已准备完成资源压缩后的总大小。
            /// </summary>
            public long ReadyCompressedLength
            {
                get
                {
                    long readyCompressedLength = 0L;
                    foreach (ResourceName resourceName in m_ResourceNames)
                    {
                        ResourceInfo resourceInfo = null;
                        if (m_ResourceInfos.TryGetValue(resourceName, out resourceInfo) && resourceInfo.Ready)
                        {
                            readyCompressedLength += resourceInfo.CompressedLength;
                        }
                    }

                    return readyCompressedLength;
                }
            }

            /// <summary>
            /// 获取资源组集合的完成进度。
            /// </summary>
            public float Progress
            {
                get
                {
                    return m_TotalLength > 0L ? (float)ReadyLength / m_TotalLength : 1f;
                }
            }

            /// <summary>
            /// 获取资源组集合包含的资源组列表。
            /// </summary>
            /// <returns>资源组包含的资源名称列表。</returns>
            public IResourceGroup[] GetResourceGroups()
            {
                return m_ResourceGroups;
            }

            /// <summary>
            /// 获取资源组集合包含的资源名称列表。
            /// </summary>
            /// <returns>资源组包含的资源名称列表。</returns>
            public string[] GetResourceNames()
            {
                int index = 0;
                string[] resourceNames = new string[m_ResourceNames.Count];
                foreach (ResourceName resourceName in m_ResourceNames)
                {
                    resourceNames[index++] = resourceName.FullName;
                }

                return resourceNames;
            }

            /// <summary>
            /// 获取资源组集合包含的资源名称列表。
            /// </summary>
            /// <param name="results">资源组包含的资源名称列表。</param>
            public void GetResourceNames(List<string> results)
            {
                if (results == null)
                {
                    throw new GameFrameworkException("Results is invalid.");
                }

                results.Clear();
                foreach (ResourceName resourceName in m_ResourceNames)
                {
                    results.Add(resourceName.FullName);
                }
            }
        }
    }
}
