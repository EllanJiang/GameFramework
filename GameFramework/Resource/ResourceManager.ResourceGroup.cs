//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System.Collections.Generic;

namespace GameFramework.Resource
{
    internal sealed partial class ResourceManager : GameFrameworkModule, IResourceManager
    {
        /// <summary>
        /// 资源组。
        /// </summary>
        private sealed class ResourceGroup : IResourceGroup
        {
            private readonly string m_Name;
            private readonly Dictionary<ResourceName, ResourceInfo> m_ResourceInfos;
            private readonly List<ResourceName> m_ResourceNames;
            private long m_TotalLength;
            private long m_TotalZipLength;

            /// <summary>
            /// 初始化资源组的新实例。
            /// </summary>
            /// <param name="name">资源组名称。</param>
            /// <param name="resourceInfos">资源信息引用。</param>
            public ResourceGroup(string name, Dictionary<ResourceName, ResourceInfo> resourceInfos)
            {
                if (name == null)
                {
                    throw new GameFrameworkException("Name is invalid.");
                }

                if (resourceInfos == null)
                {
                    throw new GameFrameworkException("Resource infos is invalid.");
                }

                m_Name = name;
                m_ResourceInfos = resourceInfos;
                m_ResourceNames = new List<ResourceName>();
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
            /// 获取资源组是否准备完毕。
            /// </summary>
            public bool Ready
            {
                get
                {
                    return ReadyCount >= TotalCount;
                }
            }

            /// <summary>
            /// 获取资源组包含资源数量。
            /// </summary>
            public int TotalCount
            {
                get
                {
                    return m_ResourceNames.Count;
                }
            }

            /// <summary>
            /// 获取资源组中已准备完成资源数量。
            /// </summary>
            public int ReadyCount
            {
                get
                {
                    int readyCount = 0;
                    foreach (ResourceName resourceName in m_ResourceNames)
                    {
                        if (m_ResourceInfos.ContainsKey(resourceName))
                        {
                            readyCount++;
                        }
                    }

                    return readyCount;
                }
            }

            /// <summary>
            /// 获取资源组包含资源的总大小。
            /// </summary>
            public long TotalLength
            {
                get
                {
                    return m_TotalLength;
                }
            }

            /// <summary>
            /// 获取资源组包含资源压缩后的总大小。
            /// </summary>
            public long TotalZipLength
            {
                get
                {
                    return m_TotalZipLength;
                }
            }

            /// <summary>
            /// 获取资源组中已准备完成资源的总大小。
            /// </summary>
            public long ReadyLength
            {
                get
                {
                    long totalReadyLength = 0L;
                    foreach (ResourceName resourceName in m_ResourceNames)
                    {
                        ResourceInfo resourceInfo = default(ResourceInfo);
                        if (m_ResourceInfos.TryGetValue(resourceName, out resourceInfo))
                        {
                            totalReadyLength += resourceInfo.Length;
                        }
                    }

                    return totalReadyLength;
                }
            }

            /// <summary>
            /// 获取资源组的完成进度。
            /// </summary>
            public float Progress
            {
                get
                {
                    return m_TotalLength > 0L ? (float)ReadyLength / m_TotalLength : 1f;
                }
            }

            /// <summary>
            /// 获取资源组包含的资源名称列表。
            /// </summary>
            /// <returns>资源组包含的资源名称列表。</returns>
            public string[] GetResourceNames()
            {
                string[] resourceNames = new string[m_ResourceNames.Count];
                for (int i = 0; i < m_ResourceNames.Count; i++)
                {
                    resourceNames[i] = m_ResourceNames[i].FullName;
                }

                return resourceNames;
            }

            /// <summary>
            /// 检查指定资源是否属于资源组。
            /// </summary>
            /// <param name="resourceName">要检查的资源的名称。</param>
            /// <returns>指定资源是否属于资源组。</returns>
            public bool HasResource(ResourceName resourceName)
            {
                return m_ResourceNames.Contains(resourceName);
            }

            /// <summary>
            /// 向资源组中增加资源。
            /// </summary>
            /// <param name="resourceName">资源名称。</param>
            /// <param name="length">资源大小。</param>
            /// <param name="zipLength">资源压缩后的大小。</param>
            public void AddResource(ResourceName resourceName, int length, int zipLength)
            {
                m_ResourceNames.Add(resourceName);
                m_TotalLength += length;
                m_TotalZipLength += zipLength;
            }
        }
    }
}
