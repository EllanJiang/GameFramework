//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.UI
{
    internal partial class UIManager
    {
        private sealed class RecycleNode
        {
            private readonly IUIForm m_UIForm;
            private int m_TickCount;

            public RecycleNode(IUIForm uiForm)
            {
                m_UIForm = uiForm;
                m_TickCount = 0;
            }

            public IUIForm UIForm
            {
                get
                {
                    return m_UIForm;
                }
            }

            public int TickCount
            {
                get
                {
                    return m_TickCount;
                }
                set
                {
                    m_TickCount = value;
                }
            }
        }
    }
}
