//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System.Collections.Generic;

namespace GameFramework.Entity
{
    /// <summary>
    /// 实体组接口。
    /// </summary>
    public interface IEntityGroup
    {
        /// <summary>
        /// 获取实体组名称。
        /// </summary>
        string Name
        {
            get;
        }

        /// <summary>
        /// 获取实体组中实体数量。
        /// </summary>
        int EntityCount
        {
            get;
        }

        /// <summary>
        /// 获取或设置实体组实例对象池自动释放可释放对象的间隔秒数。
        /// </summary>
        float InstanceAutoReleaseInterval
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置实体组实例对象池的容量。
        /// </summary>
        int InstanceCapacity
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置实体组实例对象池对象过期秒数。
        /// </summary>
        float InstanceExpireTime
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置实体组实例对象池的优先级。
        /// </summary>
        int InstancePriority
        {
            get;
            set;
        }

        /// <summary>
        /// 获取实体组辅助器。
        /// </summary>
        IEntityGroupHelper Helper
        {
            get;
        }

        /// <summary>
        /// 实体组中是否存在实体。
        /// </summary>
        /// <param name="entityId">实体序列编号。</param>
        /// <returns>实体组中是否存在实体。</returns>
        bool HasEntity(int entityId);

        /// <summary>
        /// 实体组中是否存在实体。
        /// </summary>
        /// <param name="entityAssetName">实体资源名称。</param>
        /// <returns>实体组中是否存在实体。</returns>
        bool HasEntity(string entityAssetName);

        /// <summary>
        /// 从实体组中获取实体。
        /// </summary>
        /// <param name="entityId">实体序列编号。</param>
        /// <returns>要获取的实体。</returns>
        IEntity GetEntity(int entityId);

        /// <summary>
        /// 从实体组中获取实体。
        /// </summary>
        /// <param name="entityAssetName">实体资源名称。</param>
        /// <returns>要获取的实体。</returns>
        IEntity GetEntity(string entityAssetName);

        /// <summary>
        /// 从实体组中获取实体。
        /// </summary>
        /// <param name="entityAssetName">实体资源名称。</param>
        /// <returns>要获取的实体。</returns>
        IEntity[] GetEntities(string entityAssetName);

        /// <summary>
        /// 从实体组中获取实体。
        /// </summary>
        /// <param name="entityAssetName">实体资源名称。</param>
        /// <param name="results">要获取的实体。</param>
        void GetEntities(string entityAssetName, List<IEntity> results);

        /// <summary>
        /// 从实体组中获取所有实体。
        /// </summary>
        /// <returns>实体组中的所有实体。</returns>
        IEntity[] GetAllEntities();

        /// <summary>
        /// 从实体组中获取所有实体。
        /// </summary>
        /// <param name="results">实体组中的所有实体。</param>
        void GetAllEntities(List<IEntity> results);

        /// <summary>
        /// 设置实体实例是否被加锁。
        /// </summary>
        /// <param name="entityInstance">实体实例。</param>
        /// <param name="locked">实体实例是否被加锁。</param>
        void SetEntityInstanceLocked(object entityInstance, bool locked);

        /// <summary>
        /// 设置实体实例的优先级。
        /// </summary>
        /// <param name="entityInstance">实体实例。</param>
        /// <param name="priority">实体实例优先级。</param>
        void SetEntityInstancePriority(object entityInstance, int priority);
    }
}
