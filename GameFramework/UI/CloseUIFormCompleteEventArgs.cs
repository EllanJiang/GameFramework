//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.UI
{
    /// <summary>
    /// 关闭界面完成事件。
    /// </summary>
    public sealed class CloseUIFormCompleteEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化关闭界面完成事件的新实例。
        /// </summary>
        /// <param name="uiFormTypeId">界面类型编号。</param>
        /// <param name="userData">用户自定义数据。</param>
        public CloseUIFormCompleteEventArgs(int uiFormTypeId, object userData)
        {
            UIFormTypeId = uiFormTypeId;
            UserData = userData;
        }

        /// <summary>
        /// 获取界面类型编号。
        /// </summary>
        public int UIFormTypeId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取用户自定义数据。
        /// </summary>
        public object UserData
        {
            get;
            private set;
        }
    }
}
