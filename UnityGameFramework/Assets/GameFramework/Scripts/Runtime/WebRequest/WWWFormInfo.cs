//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using UnityEngine;

namespace UnityGameFramework.Runtime
{
    internal sealed class WWWFormInfo
    {
        private readonly WWWForm m_WWWForm;
        private readonly object m_UserData;

        public WWWFormInfo(WWWForm wwwForm, object userData)
        {
            m_WWWForm = wwwForm;
            m_UserData = userData;
        }

        public WWWForm WWWForm
        {
            get
            {
                return m_WWWForm;
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
