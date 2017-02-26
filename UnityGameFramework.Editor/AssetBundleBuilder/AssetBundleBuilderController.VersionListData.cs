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
        private sealed class VersionListData
        {
            public VersionListData(string path, int length, int hashCode, int zipLength, int zipHashCode)
            {
                Path = path;
                Length = length;
                HashCode = hashCode;
                ZipLength = zipLength;
                ZipHashCode = zipHashCode;
            }

            public string Path
            {
                get;
                private set;
            }

            public int Length
            {
                get;
                private set;
            }

            public int HashCode
            {
                get;
                private set;
            }

            public int ZipLength
            {
                get;
                private set;
            }

            public int ZipHashCode
            {
                get;
                private set;
            }
        }
    }
}
