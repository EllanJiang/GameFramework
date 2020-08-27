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
        private sealed partial class UIGroup : IUIGroup
        {
            /// <summary>
            /// 界面组界面信息。
            /// </summary>
            private sealed class UIFormInfo : IReference
            {
                private IUIForm m_UIForm;
                private bool m_Paused;
                private bool m_Covered;

                public UIFormInfo()
                {
                    m_UIForm = null;
                    m_Paused = false;
                    m_Covered = false;
                }

                public IUIForm UIForm
                {
                    get
                    {
                        return m_UIForm;
                    }
                }

                public bool Paused
                {
                    get
                    {
                        return m_Paused;
                    }
                    set
                    {
                        m_Paused = value;
                    }
                }

                public bool Covered
                {
                    get
                    {
                        return m_Covered;
                    }
                    set
                    {
                        m_Covered = value;
                    }
                }

                public static UIFormInfo Create(IUIForm uiForm)
                {
                    if (uiForm == null)
                    {
                        throw new GameFrameworkException("UI form is invalid.");
                    }

                    UIFormInfo uiFormInfo = ReferencePool.Acquire<UIFormInfo>();
                    uiFormInfo.m_UIForm = uiForm;
                    uiFormInfo.m_Paused = true;
                    uiFormInfo.m_Covered = true;
                    return uiFormInfo;
                }

                public void Clear()
                {
                    m_UIForm = null;
                    m_Paused = false;
                    m_Covered = false;
                }
            }
        }
    }
}
