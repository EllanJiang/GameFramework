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
    /// 打开界面成功事件。
    /// </summary>
    public sealed class OpenUIFormSuccessEventArgs : GameEventArgs
    {
        /// <summary>
        /// 初始化打开界面成功事件的新实例。
        /// </summary>
        /// <param name="e">内部事件。</param>
        public OpenUIFormSuccessEventArgs(GameFramework.UI.OpenUIFormSuccessEventArgs e)
        {
            UIForm = (UIForm)e.UIForm;
            Duration = e.Duration;
            UserData = e.UserData;
        }

        /// <summary>
        /// 获取打开界面成功事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return (int)EventId.OpenUIFormSuccess;
            }
        }

        /// <summary>
        /// 获取打开成功的界面。
        /// </summary>
        public UIForm UIForm
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取加载持续时间。
        /// </summary>
        public float Duration
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
