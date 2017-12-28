//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
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
        /// <param name="serialId">界面序列编号。</param>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <param name="uiGroup">界面所属的界面组。</param>
        /// <param name="userData">用户自定义数据。</param>
        public CloseUIFormCompleteEventArgs(int serialId, string uiFormAssetName, IUIGroup uiGroup, object userData)
        {
            SerialId = serialId;
            UIFormAssetName = uiFormAssetName;
            UIGroup = uiGroup;
            UserData = userData;
        }

        /// <summary>
        /// 获取界面序列编号。
        /// </summary>
        public int SerialId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取界面资源名称。
        /// </summary>
        public string UIFormAssetName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取界面所属的界面组。
        /// </summary>
        public IUIGroup UIGroup
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
