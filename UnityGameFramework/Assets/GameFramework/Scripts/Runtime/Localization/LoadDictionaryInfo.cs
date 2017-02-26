//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace UnityGameFramework.Runtime
{
    internal sealed class LoadDictionaryInfo
    {
        private readonly string m_DictionaryName;
        private readonly object m_UserData;

        public LoadDictionaryInfo(string dictionaryName, object userData)
        {
            m_DictionaryName = dictionaryName;
            m_UserData = userData;
        }

        public string DictionaryName
        {
            get
            {
                return m_DictionaryName;
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
