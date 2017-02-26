//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Resource
{
    internal partial class ResourceManager
    {
        /// <summary>
        /// 资源名称。
        /// </summary>
        private struct ResourceName
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

            /// <summary>
            /// 获取资源完整名称。
            /// </summary>
            public string FullName
            {
                get
                {
                    return IsVariant ? string.Format("{0}.{1}", m_Name, m_Variant) : m_Name;
                }
            }

            /// <summary>
            /// 获取资源完整名称。
            /// </summary>
            /// <returns>资源完整名称。</returns>
            public override string ToString()
            {
                return FullName;
            }
        }
    }
}
