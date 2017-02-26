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
    /// 显示实体时加载依赖资源事件。
    /// </summary>
    public sealed class ShowEntityDependencyAssetEventArgs : GameEventArgs
    {
        /// <summary>
        /// 初始化显示实体时加载依赖资源事件的新实例。
        /// </summary>
        /// <param name="e">内部事件。</param>
        public ShowEntityDependencyAssetEventArgs(GameFramework.Entity.ShowEntityDependencyAssetEventArgs e)
        {
            ShowEntityInfo showEntityInfo = (ShowEntityInfo)e.UserData;
            EntityId = e.EntityId;
            EntityLogicType = showEntityInfo.EntityLogicType;
            EntityAssetName = e.EntityAssetName;
            EntityGroupName = e.EntityGroupName;
            DependencyAssetName = e.DependencyAssetName;
            LoadedCount = e.LoadedCount;
            TotalCount = e.TotalCount;
            UserData = showEntityInfo.UserData;
        }

        /// <summary>
        /// 获取显示实体时加载依赖资源事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return (int)EventId.ShowEntityDependencyAsset;
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
        /// 获取被加载的依赖资源名称。
        /// </summary>
        public string DependencyAssetName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取当前已加载依赖资源数量。
        /// </summary>
        public int LoadedCount
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取总共加载依赖资源数量。
        /// </summary>
        public int TotalCount
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
