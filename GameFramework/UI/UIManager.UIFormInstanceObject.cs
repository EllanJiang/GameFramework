//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.ObjectPool;

namespace GameFramework.UI
{
    internal partial class UIManager
    {
        /// <summary>
        /// 界面实例对象。
        /// </summary>
        private sealed class UIFormInstanceObject : ObjectBase
        {
            private readonly object m_UIFormAsset;
            private readonly IUIFormHelper m_UIFormHelper;

            public UIFormInstanceObject(string name, object uiFormAsset, object uiFormInstance, IUIFormHelper uiFormHelper)
                : base(name, uiFormInstance)
            {
                if (uiFormAsset == null)
                {
                    throw new GameFrameworkException("UI form asset is invalid.");
                }

                if (uiFormHelper == null)
                {
                    throw new GameFrameworkException("UI form helper is invalid.");
                }

                m_UIFormAsset = uiFormAsset;
                m_UIFormHelper = uiFormHelper;
            }

            protected internal override void Release(bool isShutdown)
            {
                m_UIFormHelper.ReleaseUIForm(m_UIFormAsset, Target);
            }
        }
    }
}
