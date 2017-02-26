//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

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
    }
}
