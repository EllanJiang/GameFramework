﻿//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.UI
{
    /// <summary>
    /// 界面组接口。
    /// </summary>
    public interface IUIGroup
    {
        /// <summary>
        /// 获取界面组名称。
        /// </summary>
        string Name
        {
            get;
        }

        /// <summary>
        /// 获取或设置界面组深度。
        /// </summary>
        int Depth
        {
            get;
            set;
        }

        /// <summary>
        /// 获取界面组中界面数量。
        /// </summary>
        int UIFormCount
        {
            get;
        }

        /// <summary>
        /// 获取当前界面。
        /// </summary>
        IUIForm CurrentUIForm
        {
            get;
        }

        /// <summary>
        /// 获取界面组辅助器。
        /// </summary>
        IUIGroupHelper Helper
        {
            get;
        }

        /// <summary>
        /// 界面组中是否存在界面。
        /// </summary>
        /// <param name="serialId">界面序列编号。</param>
        /// <returns>界面组中是否存在界面。</returns>
        bool HasUIForm(int serialId);

        /// <summary>
        /// 界面组中是否存在界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <returns>界面组中是否存在界面。</returns>
        bool HasUIForm(string uiFormAssetName);

        /// <summary>
        /// 从界面组中获取界面。
        /// </summary>
        /// <param name="serialId">界面序列编号。</param>
        /// <returns>要获取的界面。</returns>
        IUIForm GetUIForm(int serialId);

        /// <summary>
        /// 从界面组中获取界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <returns>要获取的界面。</returns>
        IUIForm GetUIForm(string uiFormAssetName);

        /// <summary>
        /// 从界面组中获取界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <returns>要获取的界面。</returns>
        IUIForm[] GetUIForms(string uiFormAssetName);

        /// <summary>
        /// 从界面组中获取所有界面。
        /// </summary>
        /// <returns>界面组中的所有界面。</returns>
        IUIForm[] GetAllUIForms();
    }
}
