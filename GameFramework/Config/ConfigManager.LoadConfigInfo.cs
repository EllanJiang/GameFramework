//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Config
{
    internal sealed partial class ConfigManager : GameFrameworkModule, IConfigManager
    {
        private sealed class LoadConfigInfo : IReference
        {
            private LoadType m_LoadType;
            private object m_UserData;

            public LoadConfigInfo()
            {
                m_LoadType = LoadType.Text;
                m_UserData = null;
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

            public static LoadConfigInfo Create(LoadType loadType, object userData)
            {
                LoadConfigInfo loadConfigInfo = ReferencePool.Acquire<LoadConfigInfo>();
                loadConfigInfo.m_LoadType = loadType;
                loadConfigInfo.m_UserData = userData;
                return loadConfigInfo;
            }

            public void Clear()
            {
                m_LoadType = LoadType.Text;
                m_UserData = null;
            }
        }
    }
}
