//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.DataTable
{
    internal sealed partial class DataTableManager : GameFrameworkModule, IDataTableManager
    {
        private sealed class LoadDataTableInfo : IReference
        {
            private LoadType m_LoadType;
            private object m_UserData;

            public LoadDataTableInfo()
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

            public static LoadDataTableInfo Create(LoadType loadType, object userData)
            {
                LoadDataTableInfo loadDataTableInfo = ReferencePool.Acquire<LoadDataTableInfo>();
                loadDataTableInfo.m_LoadType = loadType;
                loadDataTableInfo.m_UserData = userData;
                return loadDataTableInfo;
            }

            public void Clear()
            {
                m_LoadType = LoadType.Text;
                m_UserData = null;
            }
        }
    }
}
