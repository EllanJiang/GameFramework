//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using UnityEditor;

namespace UnityGameFramework.Editor.AssetBundleTools
{
    internal partial class AssetBundleBuilderController
    {
        private sealed class AssetBundleCode
        {
            private readonly BuildTarget m_BuildTarget;
            private readonly int m_Length;
            private readonly int m_HashCode;
            private readonly int m_ZipLength;
            private readonly int m_ZipHashCode;

            public AssetBundleCode(BuildTarget buildTarget, int length, int hashCode, int zipLength, int zipHashCode)
            {
                m_BuildTarget = buildTarget;
                m_Length = length;
                m_HashCode = hashCode;
                m_ZipLength = zipLength;
                m_ZipHashCode = zipHashCode;
            }

            public BuildTarget BuildTarget
            {
                get
                {
                    return m_BuildTarget;
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

            public int ZipLength
            {
                get
                {
                    return m_ZipLength;
                }
            }

            public int ZipHashCode
            {
                get
                {
                    return m_ZipHashCode;
                }
            }
        }
    }
}
