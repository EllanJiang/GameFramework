//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.UI
{
    /// <summary>
    /// 界面辅助器接口。
    /// </summary>
    public interface IUIFormHelper
    {
        /// <summary>
        /// 创建界面。
        /// </summary>
        /// <param name="uiFormInstance">界面实例。</param>
        /// <param name="uiGroup">界面所属的界面组。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>界面。</returns>
        IUIForm CreateUIForm(object uiFormInstance, IUIGroup uiGroup, object userData);

        /// <summary>
        /// 释放界面实例。
        /// </summary>
        /// <param name="uiFormInstance">要释放的界面实例。</param>
        void ReleaseUIFormInstance(object uiFormInstance);
    }
}
