//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.Event;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 关闭界面完成事件。
    /// </summary>
    public sealed class CloseUIFormCompleteEventArgs : GameEventArgs
    {
        /// <summary>
        /// 初始化关闭界面完成事件的新实例。
        /// </summary>
        /// <param name="e">内部事件。</param>
        public CloseUIFormCompleteEventArgs(GameFramework.UI.CloseUIFormCompleteEventArgs e)
        {
            UIFormTypeId = e.UIFormTypeId;
            UserData = e.UserData;
        }

        /// <summary>
        /// 获取关闭界面完成事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return (int)EventId.CloseUIFormComplete;
            }
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
