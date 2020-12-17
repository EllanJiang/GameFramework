//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Entity
{
    /// <summary>
    /// 实体接口。
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// 获取实体编号。
        /// </summary>
        int Id
        {
            get;
        }

        /// <summary>
        /// 获取实体资源名称。
        /// </summary>
        string EntityAssetName
        {
            get;
        }

        /// <summary>
        /// 获取实体实例。
        /// </summary>
        object Handle
        {
            get;
        }

        /// <summary>
        /// 获取实体所属的实体组。
        /// </summary>
        IEntityGroup EntityGroup
        {
            get;
        }

        /// <summary>
        /// 实体初始化。
        /// </summary>
        /// <param name="entityId">实体编号。</param>
        /// <param name="entityAssetName">实体资源名称。</param>
        /// <param name="entityGroup">实体所属的实体组。</param>
        /// <param name="isNewInstance">是否是新实例。</param>
        /// <param name="userData">用户自定义数据。</param>
        void OnInit(int entityId, string entityAssetName, IEntityGroup entityGroup, bool isNewInstance, object userData);

        /// <summary>
        /// 实体回收。
        /// </summary>
        void OnRecycle();

        /// <summary>
        /// 实体显示。
        /// </summary>
        /// <param name="userData">用户自定义数据。</param>
        void OnShow(object userData);

        /// <summary>
        /// 实体隐藏。
        /// </summary>
        /// <param name="isShutdown">是否是关闭实体管理器时触发。</param>
        /// <param name="userData">用户自定义数据。</param>
        void OnHide(bool isShutdown, object userData);

        /// <summary>
        /// 实体附加子实体。
        /// </summary>
        /// <param name="childEntity">附加的子实体。</param>
        /// <param name="userData">用户自定义数据。</param>
        void OnAttached(IEntity childEntity, object userData);

        /// <summary>
        /// 实体解除子实体。
        /// </summary>
        /// <param name="childEntity">解除的子实体。</param>
        /// <param name="userData">用户自定义数据。</param>
        void OnDetached(IEntity childEntity, object userData);

        /// <summary>
        /// 实体附加子实体。
        /// </summary>
        /// <param name="parentEntity">被附加的父实体。</param>
        /// <param name="userData">用户自定义数据。</param>
        void OnAttachTo(IEntity parentEntity, object userData);

        /// <summary>
        /// 实体解除子实体。
        /// </summary>
        /// <param name="parentEntity">被解除的父实体。</param>
        /// <param name="userData">用户自定义数据。</param>
        void OnDetachFrom(IEntity parentEntity, object userData);

        /// <summary>
        /// 实体轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        void OnUpdate(float elapseSeconds, float realElapseSeconds);
    }
}
