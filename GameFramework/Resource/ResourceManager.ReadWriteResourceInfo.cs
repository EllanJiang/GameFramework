//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Resource
{
    internal partial class ResourceManager
    {
        private struct ReadWriteResourceInfo
        {
            private readonly LoadType m_LoadType;
            private readonly int m_Length;
            private readonly int m_HashCode;

            public ReadWriteResourceInfo(LoadType loadType, int length, int hashCode)
            {
                m_LoadType = loadType;
                m_Length = length;
                m_HashCode = hashCode;
            }

            public LoadType LoadType
            {
                get
                {
                    return m_LoadType;
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
        }
    }
}
