//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Entity
{
    /// <summary>
    /// 显示实体失败事件。
    /// </summary>
    public sealed class ShowEntityFailureEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化显示实体失败事件的新实例。
        /// </summary>
        /// <param name="entityId">实体编号。</param>
        /// <param name="entityAssetName">实体资源名称。</param>
        /// <param name="entityGroupName">实体组名称。</param>
        /// <param name="errorMessage">错误信息。</param>
        /// <param name="userData">用户自定义数据。</param>
        public ShowEntityFailureEventArgs(int entityId, string entityAssetName, string entityGroupName, string errorMessage, object userData)
        {
            EntityId = entityId;
            EntityAssetName = entityAssetName;
            EntityGroupName = entityGroupName;
            ErrorMessage = errorMessage;
            UserData = userData;
        }

        /// <summary>
        /// 获取实体编号。
        /// </summary>
        public int EntityId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取实体资源名称。
        /// </summary>
        public string EntityAssetName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取实体组名称。
        /// </summary>
        public string EntityGroupName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取错误信息。
        /// </summary>
        public string ErrorMessage
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
