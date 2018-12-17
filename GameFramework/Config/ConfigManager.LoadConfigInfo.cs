//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Config
{
    internal partial class ConfigManager
    {
        private sealed class LoadConfigInfo
        {
            private readonly LoadType m_LoadType;
            private readonly object m_UserData;

            public LoadConfigInfo(LoadType loadType, object userData)
            {
                m_LoadType = loadType;
                m_UserData = userData;
            }

            public LoadType LoadType
            {
                get
                {
                    return m_LoadType;
                }
            }

            public object UserData
            {
                get
                {
                    return m_UserData;
                }
            }
        }
    }
}
