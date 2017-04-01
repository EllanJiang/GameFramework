//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace UnityGameFramework.Editor.AssetBundleTools
{
    internal partial class AssetBundleBuilderController
    {
        private sealed class AssetData
        {
            private readonly string m_Guid;
            private readonly string m_Name;
            private readonly int m_Length;
            private readonly int m_HashCode;
            private readonly string[] m_DependencyAssetNames;

            public AssetData(string guid, string name, int length, int hashCode, string[] dependencyAssetNames)
            {
                m_Guid = guid;
                m_Name = name;
                m_Length = length;
                m_HashCode = hashCode;
                m_DependencyAssetNames = dependencyAssetNames;
            }

            public string Guid
            {
                get
                {
                    return m_Guid;
                }
            }

            public string Name
            {
                get
                {
                    return m_Name;
                }
            }

            public int Length
            {
                get
                {
                    return m_Length;
                }
            }

            public int HashCode
            {
                get
                {
                    return m_HashCode;
                }
            }

            public string[] GetDependencyAssetNames()
            {
                return m_DependencyAssetNames;
            }
        }
    }
}
