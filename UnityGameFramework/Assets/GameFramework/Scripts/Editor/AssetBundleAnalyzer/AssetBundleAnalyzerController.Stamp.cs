//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace UnityGameFramework.Editor.AssetBundleTools
{
    internal sealed partial class AssetBundleAnalyzerController
    {
        private struct Stamp
        {
            private readonly string m_AssetName;
            private readonly string m_HostAssetName;

            public Stamp(string assetName, string hostAssetName)
            {
                m_AssetName = assetName;
                m_HostAssetName = hostAssetName;
            }

            public string AssetName
            {
                get
                {
                    return m_AssetName;
                }
            }

            public string HostAssetName
            {
                get
                {
                    return m_HostAssetName;
                }
            }
        }
    }
}
