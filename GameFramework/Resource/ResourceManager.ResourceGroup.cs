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
        /// 资源组。
        /// </summary>
        private sealed class ResourceGroup : IResourceGroup
        {
            private readonly string m_Name;
            private readonly Dictionary<ResourceName, ResourceInfo> m_ResourceInfos;
            private readonly HashSet<ResourceName> m_ResourceNames;
            private long m_TotalLength;
            private long m_TotalCompressedLength;

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
                m_ResourceNames = new HashSet<ResourceName>();
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
            public long TotalCompressedLength
            {
                get
                {
                    return m_TotalCompressedLength;
                }
            }

            /// <summary>
            /// 获取资源组中已准备完成资源的总大小。
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
            /// 获取资源组中已准备完成资源压缩后的总大小。
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
                int index = 0;
                string[] resourceNames = new string[m_ResourceNames.Count];
                foreach (ResourceName resourceName in m_ResourceNames)
                {
                    resourceNames[index++] = resourceName.FullName;
                }

                return resourceNames;
            }

            /// <summary>
            /// 获取资源组包含的资源名称列表。
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

            /// <summary>
            /// 获取资源组包含的资源名称列表。
            /// </summary>
            /// <returns>资源组包含的资源名称列表。</returns>
            public ResourceName[] InternalGetResourceNames()
            {
                int index = 0;
                ResourceName[] resourceNames = new ResourceName[m_ResourceNames.Count];
                foreach (ResourceName resourceName in m_ResourceNames)
                {
                    resourceNames[index++] = resourceName;
                }

                return resourceNames;
            }

            /// <summary>
            /// 获取资源组包含的资源名称列表。
            /// </summary>
            /// <param name="results">资源组包含的资源名称列表。</param>
            public void InternalGetResourceNames(List<ResourceName> results)
            {
                if (results == null)
                {
                    throw new GameFrameworkException("Results is invalid.");
                }

                results.Clear();
                foreach (ResourceName resourceName in m_ResourceNames)
                {
                    results.Add(resourceName);
                }
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
            /// <param name="compressedLength">资源压缩后的大小。</param>
            public void AddResource(ResourceName resourceName, int length, int compressedLength)
            {
                m_ResourceNames.Add(resourceName);
                m_TotalLength += length;
                m_TotalCompressedLength += compressedLength;
            }
        }
    }
}
