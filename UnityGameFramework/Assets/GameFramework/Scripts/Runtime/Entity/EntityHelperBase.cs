//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.Entity;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 实体辅助器基类。
    /// </summary>
    public abstract class EntityHelperBase : MonoBehaviour, IEntityHelper
    {
        /// <summary>
        /// 创建实体。
        /// </summary>
        /// <param name="entityInstance">实体实例。</param>
        /// <param name="entityGroup">实体所属的实体组。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>实体。</returns>
        public abstract IEntity CreateEntity(object entityInstance, IEntityGroup entityGroup, object userData);

        /// <summary>
        /// 释放实体实例。
        /// </summary>
        /// <param name="entityInstance">要释放的实体实例。</param>
        public abstract void ReleaseEntityInstance(object entityInstance);
    }
}
