//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
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
            private readonly IUIFormHelper m_UIFormHelper;

            public UIFormInstanceObject(string name, object target, IUIFormHelper uiFormHelper)
                : base(name, target)
            {
                if (uiFormHelper == null)
                {
                    throw new GameFrameworkException("UI form helper is invalid.");
                }

                m_UIFormHelper = uiFormHelper;
            }

            protected internal override void Release()
            {
                m_UIFormHelper.ReleaseUIFormInstance(Target);
            }
        }
    }
}
