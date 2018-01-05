//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.UI
{
    internal partial class UIManager
    {
        private partial class UIGroup
        {
            /// <summary>
            /// 界面组界面信息。
            /// </summary>
            private sealed class UIFormInfo
            {
                private readonly IUIForm m_UIForm;
                private bool m_Paused;
                private bool m_Covered;

                /// <summary>
                /// 初始化界面组界面信息的新实例。
                /// </summary>
                /// <param name="uiForm">界面。</param>
                public UIFormInfo(IUIForm uiForm)
                {
                    if (uiForm == null)
                    {
                        throw new GameFrameworkException("UI form is invalid.");
                    }

                    m_UIForm = uiForm;
                    m_Paused = true;
                    m_Covered = true;
                }

                /// <summary>
                /// 获取界面。
                /// </summary>
                public IUIForm UIForm
                {
                    get
                    {
                        return m_UIForm;
                    }
                }

                /// <summary>
                /// 获取或设置界面是否暂停。
                /// </summary>
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

                /// <summary>
                /// 获取或设置界面是否遮挡。
                /// </summary>
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
            }
        }
    }
}
