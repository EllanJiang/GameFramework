//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Entity
{
    internal partial class EntityManager
    {
        /// <summary>
        /// 实体状态。
        /// </summary>
        private enum EntityStatus
        {
            WillInit,
            Inited,
            WillShow,
            Showed,
            WillHide,
            Hidden,
            WillRecycle,
            Recycled,
        }
    }
}
