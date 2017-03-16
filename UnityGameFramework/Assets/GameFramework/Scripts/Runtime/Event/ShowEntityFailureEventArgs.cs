//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.Event;
using System;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 显示实体失败事件。
    /// </summary>
    public sealed class ShowEntityFailureEventArgs : GameEventArgs
    {
        /// <summary>
        /// 初始化显示实体失败事件的新实例。
        /// </summary>
        /// <param name="e">内部事件。</param>
        public ShowEntityFailureEventArgs(GameFramework.Entity.ShowEntityFailureEventArgs e)
        {
            ShowEntityInfo showEntityInfo = (ShowEntityInfo)e.UserData;
            EntityId = e.EntityId;
            EntityLogicType = showEntityInfo.EntityLogicType;
            EntityAssetName = e.EntityAssetName;
            EntityGroupName = e.EntityGroupName;
            ErrorMessage = e.ErrorMessage;
            UserData = showEntityInfo.UserData;
        }

        /// <summary>
        /// 获取显示实体失败事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return (int)EventId.ShowEntityFailure;
            }
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
        /// 获取实体逻辑类型。
        /// </summary>
        public Type EntityLogicType
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
