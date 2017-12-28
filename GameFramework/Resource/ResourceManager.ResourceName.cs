//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System;

namespace GameFramework.Resource
{
    internal partial class ResourceManager
    {
        /// <summary>
        /// 资源名称。
        /// </summary>
        private struct ResourceName : IComparable, IComparable<ResourceName>, IEquatable<ResourceName>
        {
            private readonly string m_Name;
            private readonly string m_Variant;

            /// <summary>
            /// 初始化资源名称的新实例。
            /// </summary>
            /// <param name="name">资源名称。</param>
            /// <param name="variant">变体名称。</param>
            public ResourceName(string name, string variant)
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new GameFrameworkException("Resource name is invalid.");
                }

                m_Name = name;
                m_Variant = variant;
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
            /// 获取变体名称。
            /// </summary>
            public string Variant
            {
                get
                {
                    return m_Variant;
                }
            }

            /// <summary>
            /// 获取是否变体。
            /// </summary>
            public bool IsVariant
            {
                get
                {
                    return m_Variant != null;
                }
            }

            public string FullName
            {
                get
                {
                    return IsVariant ? string.Format("{0}.{1}", m_Name, m_Variant) : m_Name;
                }
            }

            public override string ToString()
            {
                return FullName;
            }

            public override int GetHashCode()
            {
                if (m_Variant == null)
                {
                    return m_Name.GetHashCode();
                }

                return (m_Name.GetHashCode() ^ m_Variant.GetHashCode());
            }

            public override bool Equals(object value)
            {
                return (value is ResourceName) && (this == (ResourceName)value);
            }

            public bool Equals(ResourceName resourceName)
            {
                return (this == resourceName);
            }

            public static bool operator ==(ResourceName resourceName1, ResourceName resourceName2)
            {
                return resourceName1.CompareTo(resourceName2) == 0;
            }

            public static bool operator !=(ResourceName resourceName1, ResourceName resourceName2)
            {
                return resourceName1.CompareTo(resourceName2) != 0;
            }

            public int CompareTo(object value)
            {
                if (value == null)
                {
                    return 1;
                }

                if (!(value is ResourceName))
                {
                    throw new GameFrameworkException("Type of value is invalid.");
                }

                return CompareTo((ResourceName)value);
            }

            public int CompareTo(ResourceName resourceName)
            {
                int result = string.Compare(m_Name, resourceName.m_Name);
                if (result != 0)
                {
                    return result;
                }

                return string.Compare(m_Variant, resourceName.m_Variant);
            }
        }
    }
}
