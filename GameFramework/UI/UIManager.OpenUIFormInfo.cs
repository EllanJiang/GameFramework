//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.UI
{
    internal sealed partial class UIManager : GameFrameworkModule, IUIManager
    {
        private sealed class OpenUIFormInfo : IReference
        {
            private int m_SerialId;
            private UIGroup m_UIGroup;
            private bool m_PauseCoveredUIForm;
            private object m_UserData;

            public OpenUIFormInfo()
            {
                m_SerialId = 0;
                m_UIGroup = null;
                m_PauseCoveredUIForm = false;
                m_UserData = null;
            }

            public int SerialId
            {
                get
                {
                    return m_SerialId;
                }
            }

            public UIGroup UIGroup
            {
                get
                {
                    return m_UIGroup;
                }
            }

            public bool PauseCoveredUIForm
            {
                get
                {
                    return m_PauseCoveredUIForm;
                }
            }

            public object UserData
            {
                get
                {
                    return m_UserData;
                }
            }

            public static OpenUIFormInfo Create(int serialId, UIGroup uiGroup, bool pauseCoveredUIForm, object userData)
            {
                OpenUIFormInfo openUIFormInfo = ReferencePool.Acquire<OpenUIFormInfo>();
                openUIFormInfo.m_SerialId = serialId;
                openUIFormInfo.m_UIGroup = uiGroup;
                openUIFormInfo.m_PauseCoveredUIForm = pauseCoveredUIForm;
                openUIFormInfo.m_UserData = userData;
                return openUIFormInfo;
            }

            public void Clear()
            {
                m_SerialId = 0;
                m_UIGroup = null;
                m_PauseCoveredUIForm = false;
                m_UserData = null;
            }
        }
    }
}
