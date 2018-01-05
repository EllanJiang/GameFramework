//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Entity
{
    /// <summary>
    /// 隐藏实体完成事件。
    /// </summary>
    public sealed class HideEntityCompleteEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化隐藏实体完成事件的新实例。
        /// </summary>
        /// <param name="entityId">实体编号。</param>
        /// <param name="entityAssetName">实体资源名称。</param>
        /// <param name="entityGroup">实体所属的实体组。</param>
        /// <param name="userData">用户自定义数据。</param>
        public HideEntityCompleteEventArgs(int entityId, string entityAssetName, IEntityGroup entityGroup, object userData)
        {
            EntityId = entityId;
            EntityAssetName = entityAssetName;
            EntityGroup = entityGroup;
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
        /// 获取实体所属的实体组。
        /// </summary>
        public IEntityGroup EntityGroup
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
