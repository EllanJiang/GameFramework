//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.Entity;
using GameFramework.ObjectPool;
using GameFramework.Resource;
using System;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 实体组件。
    /// </summary>
    [AddComponentMenu("Game Framework/Entity")]
    public sealed partial class EntityComponent : GameFrameworkComponent
    {
        private IEntityManager m_EntityManager = null;
        private EventComponent m_EventComponent = null;

        [SerializeField]
        private bool m_EnableShowEntitySuccessEvent = true;

        [SerializeField]
        private bool m_EnableShowEntityFailureEvent = true;

        [SerializeField]
        private bool m_EnableShowEntityUpdateEvent = false;

        [SerializeField]
        private bool m_EnableShowEntityDependencyAssetEvent = false;

        [SerializeField]
        private bool m_EnableHideEntityCompleteEvent = true;

        [SerializeField]
        private Transform m_InstanceRoot = null;

        [SerializeField]
        private string m_EntityHelperTypeName = "UnityGameFramework.Runtime.DefaultEntityHelper";

        [SerializeField]
        private EntityHelperBase m_CustomEntityHelper = null;

        [SerializeField]
        private string m_EntityGroupHelperTypeName = "UnityGameFramework.Runtime.DefaultEntityGroupHelper";

        [SerializeField]
        private EntityGroupHelperBase m_CustomEntityGroupHelper = null;

        [SerializeField]
        private EntityGroup[] m_EntityGroups = null;

        /// <summary>
        /// 获取实体数量。
        /// </summary>
        public int EntityCount
        {
            get
            {
                return m_EntityManager.EntityCount;
            }
        }

        /// <summary>
        /// 获取实体组数量。
        /// </summary>
        public int EntityGroupCount
        {
            get
            {
                return m_EntityManager.EntityGroupCount;
            }
        }

        /// <summary>
        /// 游戏框架组件初始化。
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            m_EntityManager = GameFrameworkEntry.GetModule<IEntityManager>();
            if (m_EntityManager == null)
            {
                Log.Fatal("Entity manager is invalid.");
                return;
            }

            m_EntityManager.ShowEntitySuccess += OnShowEntitySuccess;
            m_EntityManager.ShowEntityFailure += OnShowEntityFailure;
            m_EntityManager.ShowEntityUpdate += OnShowEntityUpdate;
            m_EntityManager.ShowEntityDependencyAsset += OnShowEntityDependencyAsset;
            m_EntityManager.HideEntityComplete += OnHideEntityComplete;
        }

        private void Start()
        {
            BaseComponent baseComponent = GameEntry.GetComponent<BaseComponent>();
            if (baseComponent == null)
            {
                Log.Fatal("Base component is invalid.");
                return;
            }

            m_EventComponent = GameEntry.GetComponent<EventComponent>();
            if (m_EventComponent == null)
            {
                Log.Fatal("Event component is invalid.");
                return;
            }

            if (baseComponent.EditorResourceMode)
            {
                m_EntityManager.SetResourceManager(baseComponent.EditorResourceHelper);
            }
            else
            {
                m_EntityManager.SetResourceManager(GameFrameworkEntry.GetModule<IResourceManager>());
            }

            m_EntityManager.SetObjectPoolManager(GameFrameworkEntry.GetModule<IObjectPoolManager>());

            EntityHelperBase entityHelper = Utility.Helper.CreateHelper(m_EntityHelperTypeName, m_CustomEntityHelper);
            if (entityHelper == null)
            {
                Log.Error("Can not create entity helper.");
                return;
            }

            entityHelper.name = string.Format("Entity Helper");
            Transform transform = entityHelper.transform;
            transform.SetParent(this.transform);
            transform.localScale = Vector3.one;

            m_EntityManager.SetEntityHelper(entityHelper);

            if (m_InstanceRoot == null)
            {
                m_InstanceRoot = (new GameObject("Entity Instances")).transform;
                m_InstanceRoot.SetParent(gameObject.transform);
                m_InstanceRoot.localScale = Vector3.one;
            }

            for (int i = 0; i < m_EntityGroups.Length; i++)
            {
                if (!AddEntityGroup(m_EntityGroups[i].Name, m_EntityGroups[i].InstanceAutoReleaseInterval, m_EntityGroups[i].InstanceCapacity, m_EntityGroups[i].InstanceExpireTime, m_EntityGroups[i].InstancePriority))
                {
                    Log.Warning("Add entity group '{0}' failure.", m_EntityGroups[i].Name);
                    continue;
                }
            }
        }

        /// <summary>
        /// 是否存在实体组。
        /// </summary>
        /// <param name="entityGroupName">实体组名称。</param>
        /// <returns>是否存在实体组。</returns>
        public bool HasEntityGroup(string entityGroupName)
        {
            return m_EntityManager.HasEntityGroup(entityGroupName);
        }

        /// <summary>
        /// 获取实体组。
        /// </summary>
        /// <param name="entityGroupName">实体组名称。</param>
        /// <returns>要获取的实体组。</returns>
        public IEntityGroup GetEntityGroup(string entityGroupName)
        {
            return m_EntityManager.GetEntityGroup(entityGroupName);
        }

        /// <summary>
        /// 获取所有实体组。
        /// </summary>
        /// <returns>所有实体组。</returns>
        public IEntityGroup[] GetAllEntityGroups()
        {
            return m_EntityManager.GetAllEntityGroups();
        }

        /// <summary>
        /// 增加实体组。
        /// </summary>
        /// <param name="entityGroupName">实体组名称。</param>
        /// <param name="instanceAutoReleaseInterval">实体实例对象池自动释放可释放对象的间隔秒数。</param>
        /// <param name="instanceCapacity">实体实例对象池容量。</param>
        /// <param name="instanceExpireTime">实体实例对象池对象过期秒数。</param>
        /// <param name="instancePriority">实体实例对象池的优先级。</param>
        /// <returns>是否增加实体组成功。</returns>
        public bool AddEntityGroup(string entityGroupName, float instanceAutoReleaseInterval, int instanceCapacity, float instanceExpireTime, int instancePriority)
        {
            if (m_EntityManager.HasEntityGroup(entityGroupName))
            {
                return false;
            }

            EntityGroupHelperBase entityGroupHelper = Utility.Helper.CreateHelper(m_EntityGroupHelperTypeName, m_CustomEntityGroupHelper, EntityGroupCount);
            if (entityGroupHelper == null)
            {
                Log.Error("Can not create entity group helper.");
                return false;
            }

            entityGroupHelper.name = string.Format("Entity Group - {0}", entityGroupName);
            Transform transform = entityGroupHelper.transform;
            transform.SetParent(m_InstanceRoot);
            transform.localScale = Vector3.one;

            return m_EntityManager.AddEntityGroup(entityGroupName, instanceAutoReleaseInterval, instanceCapacity, instanceExpireTime, instancePriority, entityGroupHelper);
        }

        /// <summary>
        /// 是否存在实体。
        /// </summary>
        /// <param name="entityId">实体编号。</param>
        /// <returns>是否存在实体。</returns>
        public bool HasEntity(int entityId)
        {
            return m_EntityManager.HasEntity(entityId);
        }

        /// <summary>
        /// 获取实体。
        /// </summary>
        /// <param name="entityId">实体编号。</param>
        /// <returns>实体。</returns>
        public Entity GetEntity(int entityId)
        {
            return (Entity)m_EntityManager.GetEntity(entityId);
        }

        /// <summary>
        /// 获取所有实体。
        /// </summary>
        /// <returns>所有实体。</returns>
        public Entity[] GetAllEntities()
        {
            IEntity[] entities = m_EntityManager.GetAllEntities();
            Entity[] entityImpls = new Entity[entities.Length];
            for (int i = 0; i < entities.Length; i++)
            {
                entityImpls[i] = (Entity)entities[i];
            }

            return entityImpls;
        }

        /// <summary>
        /// 是否正在加载实体。
        /// </summary>
        /// <param name="entityId">实体编号。</param>
        /// <returns>是否正在加载实体。</returns>
        public bool IsLoadingEntity(int entityId)
        {
            return m_EntityManager.IsLoadingEntity(entityId);
        }

        /// <summary>
        /// 是否是合法的实体。
        /// </summary>
        /// <param name="entity">实体。</param>
        /// <returns>实体是否合法。</returns>
        public bool IsValidEntity(Entity entity)
        {
            if (entity == null)
            {
                return false;
            }

            return HasEntity(entity.Id);
        }

        /// <summary>
        /// 显示实体。
        /// </summary>
        /// <typeparam name="T">实体逻辑类型。</typeparam>
        /// <param name="entityId">实体编号。</param>
        /// <param name="entityAssetName">实体资源名称。</param>
        /// <param name="entityGroupName">实体组名称。</param>
        public void ShowEntity<T>(int entityId, string entityAssetName, string entityGroupName) where T : EntityLogic
        {
            ShowEntity(entityId, typeof(T), entityAssetName, entityGroupName, null);
        }

        /// <summary>
        /// 显示实体。
        /// </summary>
        /// <param name="entityId">实体编号。</param>
        /// <param name="entityLogicType">实体逻辑类型。</param>
        /// <param name="entityAssetName">实体资源名称。</param>
        /// <param name="entityGroupName">实体组名称。</param>
        public void ShowEntity(int entityId, Type entityLogicType, string entityAssetName, string entityGroupName)
        {
            ShowEntity(entityId, entityLogicType, entityAssetName, entityGroupName, null);
        }

        /// <summary>
        /// 显示实体。
        /// </summary>
        /// <typeparam name="T">实体逻辑类型。</typeparam>
        /// <param name="entityId">实体编号。</param>
        /// <param name="entityAssetName">实体资源名称。</param>
        /// <param name="entityGroupName">实体组名称。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void ShowEntity<T>(int entityId, string entityAssetName, string entityGroupName, object userData) where T : EntityLogic
        {
            ShowEntity(entityId, typeof(T), entityAssetName, entityGroupName, userData);
        }

        /// <summary>
        /// 显示实体。
        /// </summary>
        /// <param name="entityId">实体编号。</param>
        /// <param name="entityLogicType">实体逻辑类型。</param>
        /// <param name="entityAssetName">实体资源名称。</param>
        /// <param name="entityGroupName">实体组名称。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void ShowEntity(int entityId, Type entityLogicType, string entityAssetName, string entityGroupName, object userData)
        {
            if (entityLogicType == null)
            {
                Log.Error("Entity type is invalid.");
                return;
            }

            m_EntityManager.ShowEntity(entityId, entityAssetName, entityGroupName, new ShowEntityInfo(entityLogicType, userData));
        }

        /// <summary>
        /// 隐藏实体。
        /// </summary>
        /// <param name="entityId">实体编号。</param>
        public void HideEntity(int entityId)
        {
            m_EntityManager.HideEntity(entityId);
        }

        /// <summary>
        /// 隐藏实体。
        /// </summary>
        /// <param name="entityId">实体编号。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void HideEntity(int entityId, object userData)
        {
            m_EntityManager.HideEntity(entityId, userData);
        }

        /// <summary>
        /// 隐藏实体。
        /// </summary>
        /// <param name="entity">实体。</param>
        public void HideEntity(Entity entity)
        {
            m_EntityManager.HideEntity(entity);
        }

        /// <summary>
        /// 隐藏实体。
        /// </summary>
        /// <param name="entity">实体。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void HideEntity(Entity entity, object userData)
        {
            m_EntityManager.HideEntity(entity, userData);
        }

        /// <summary>
        /// 隐藏全部实体。
        /// </summary>
        public void HideAllEntities()
        {
            m_EntityManager.HideAllEntities();
        }

        /// <summary>
        /// 隐藏全部实体。
        /// </summary>
        /// <param name="userData">用户自定义数据。</param>
        public void HideAllEntities(object userData)
        {
            m_EntityManager.HideAllEntities(userData);
        }

        /// <summary>
        /// 获取父实体。
        /// </summary>
        /// <param name="childEntityId">要获取父实体的子实体的实体编号。</param>
        /// <returns>子实体的父实体。</returns>
        public Entity GetParentEntity(int childEntityId)
        {
            return (Entity)m_EntityManager.GetParentEntity(childEntityId);
        }

        /// <summary>
        /// 获取父实体。
        /// </summary>
        /// <param name="childEntity">要获取父实体的子实体。</param>
        /// <returns>子实体的父实体。</returns>
        public Entity GetParentEntity(Entity childEntity)
        {
            return (Entity)m_EntityManager.GetParentEntity(childEntity);
        }

        /// <summary>
        /// 获取子实体。
        /// </summary>
        /// <param name="parentEntityId">要获取子实体的父实体的实体编号。</param>
        /// <returns>子实体数组。</returns>
        public Entity[] GetChildEntities(int parentEntityId)
        {
            IEntity[] entities = m_EntityManager.GetChildEntities(parentEntityId);
            Entity[] entityImpls = new Entity[entities.Length];
            for (int i = 0; i < entities.Length; i++)
            {
                entityImpls[i] = (Entity)entities[i];
            }

            return entityImpls;
        }

        /// <summary>
        /// 获取子实体。
        /// </summary>
        /// <param name="parentEntity">要获取子实体的父实体。</param>
        /// <returns>子实体数组。</returns>
        public Entity[] GetChildEntities(Entity parentEntity)
        {
            IEntity[] entities = m_EntityManager.GetChildEntities(parentEntity);
            Entity[] entityImpls = new Entity[entities.Length];
            for (int i = 0; i < entities.Length; i++)
            {
                entityImpls[i] = (Entity)entities[i];
            }

            return entityImpls;
        }

        /// <summary>
        /// 附加子实体。
        /// </summary>
        /// <param name="childEntityId">要附加的子实体的实体编号。</param>
        /// <param name="parentEntityId">被附加的父实体的实体编号。</param>
        public void AttachEntity(int childEntityId, int parentEntityId)
        {
            AttachEntity(GetEntity(childEntityId), GetEntity(parentEntityId), string.Empty, null);
        }

        /// <summary>
        /// 附加子实体。
        /// </summary>
        /// <param name="childEntityId">要附加的子实体的实体编号。</param>
        /// <param name="parentEntity">被附加的父实体。</param>
        public void AttachEntity(int childEntityId, Entity parentEntity)
        {
            AttachEntity(GetEntity(childEntityId), parentEntity, string.Empty, null);
        }

        /// <summary>
        /// 附加子实体。
        /// </summary>
        /// <param name="childEntity">要附加的子实体。</param>
        /// <param name="parentEntityId">被附加的父实体的实体编号。</param>
        public void AttachEntity(Entity childEntity, int parentEntityId)
        {
            AttachEntity(childEntity, GetEntity(parentEntityId), string.Empty, null);
        }

        /// <summary>
        /// 附加子实体。
        /// </summary>
        /// <param name="childEntity">要附加的子实体。</param>
        /// <param name="parentEntity">被附加的父实体。</param>
        public void AttachEntity(Entity childEntity, Entity parentEntity)
        {
            AttachEntity(childEntity, parentEntity, string.Empty, null);
        }

        /// <summary>
        /// 附加子实体。
        /// </summary>
        /// <param name="childEntityId">要附加的子实体的实体编号。</param>
        /// <param name="parentEntityId">被附加的父实体的实体编号。</param>
        /// <param name="parentTransformPath">相对于被附加父实体的位置。</param>
        public void AttachEntity(int childEntityId, int parentEntityId, string parentTransformPath)
        {
            AttachEntity(GetEntity(childEntityId), GetEntity(parentEntityId), parentTransformPath, null);
        }

        /// <summary>
        /// 附加子实体。
        /// </summary>
        /// <param name="childEntityId">要附加的子实体的实体编号。</param>
        /// <param name="parentEntity">被附加的父实体。</param>
        /// <param name="parentTransformPath">相对于被附加父实体的位置。</param>
        public void AttachEntity(int childEntityId, Entity parentEntity, string parentTransformPath)
        {
            AttachEntity(GetEntity(childEntityId), parentEntity, parentTransformPath, null);
        }

        /// <summary>
        /// 附加子实体。
        /// </summary>
        /// <param name="childEntity">要附加的子实体。</param>
        /// <param name="parentEntityId">被附加的父实体的实体编号。</param>
        /// <param name="parentTransformPath">相对于被附加父实体的位置。</param>
        public void AttachEntity(Entity childEntity, int parentEntityId, string parentTransformPath)
        {
            AttachEntity(childEntity, GetEntity(parentEntityId), parentTransformPath, null);
        }

        /// <summary>
        /// 附加子实体。
        /// </summary>
        /// <param name="childEntity">要附加的子实体。</param>
        /// <param name="parentEntity">被附加的父实体。</param>
        /// <param name="parentTransformPath">相对于被附加父实体的位置。</param>
        public void AttachEntity(Entity childEntity, Entity parentEntity, string parentTransformPath)
        {
            AttachEntity(childEntity, parentEntity, parentTransformPath, null);
        }

        /// <summary>
        /// 附加子实体。
        /// </summary>
        /// <param name="childEntityId">要附加的子实体的实体编号。</param>
        /// <param name="parentEntityId">被附加的父实体的实体编号。</param>
        /// <param name="parentTransform">相对于被附加父实体的位置。</param>
        public void AttachEntity(int childEntityId, int parentEntityId, Transform parentTransform)
        {
            AttachEntity(GetEntity(childEntityId), GetEntity(parentEntityId), parentTransform, null);
        }

        /// <summary>
        /// 附加子实体。
        /// </summary>
        /// <param name="childEntityId">要附加的子实体的实体编号。</param>
        /// <param name="parentEntity">被附加的父实体。</param>
        /// <param name="parentTransform">相对于被附加父实体的位置。</param>
        public void AttachEntity(int childEntityId, Entity parentEntity, Transform parentTransform)
        {
            AttachEntity(GetEntity(childEntityId), parentEntity, parentTransform, null);
        }

        /// <summary>
        /// 附加子实体。
        /// </summary>
        /// <param name="childEntity">要附加的子实体。</param>
        /// <param name="parentEntityId">被附加的父实体的实体编号。</param>
        /// <param name="parentTransform">相对于被附加父实体的位置。</param>
        public void AttachEntity(Entity childEntity, int parentEntityId, Transform parentTransform)
        {
            AttachEntity(childEntity, GetEntity(parentEntityId), parentTransform, null);
        }

        /// <summary>
        /// 附加子实体。
        /// </summary>
        /// <param name="childEntity">要附加的子实体。</param>
        /// <param name="parentEntity">被附加的父实体。</param>
        /// <param name="parentTransform">相对于被附加父实体的位置。</param>
        public void AttachEntity(Entity childEntity, Entity parentEntity, Transform parentTransform)
        {
            AttachEntity(childEntity, parentEntity, parentTransform, null);
        }

        /// <summary>
        /// 附加子实体。
        /// </summary>
        /// <param name="childEntityId">要附加的子实体的实体编号。</param>
        /// <param name="parentEntityId">被附加的父实体的实体编号。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void AttachEntity(int childEntityId, int parentEntityId, object userData)
        {
            AttachEntity(GetEntity(childEntityId), GetEntity(parentEntityId), string.Empty, userData);
        }

        /// <summary>
        /// 附加子实体。
        /// </summary>
        /// <param name="childEntityId">要附加的子实体的实体编号。</param>
        /// <param name="parentEntity">被附加的父实体。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void AttachEntity(int childEntityId, Entity parentEntity, object userData)
        {
            AttachEntity(GetEntity(childEntityId), parentEntity, string.Empty, userData);
        }

        /// <summary>
        /// 附加子实体。
        /// </summary>
        /// <param name="childEntity">要附加的子实体。</param>
        /// <param name="parentEntityId">被附加的父实体的实体编号。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void AttachEntity(Entity childEntity, int parentEntityId, object userData)
        {
            AttachEntity(childEntity, GetEntity(parentEntityId), string.Empty, userData);
        }

        /// <summary>
        /// 附加子实体。
        /// </summary>
        /// <param name="childEntity">要附加的子实体。</param>
        /// <param name="parentEntity">被附加的父实体。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void AttachEntity(Entity childEntity, Entity parentEntity, object userData)
        {
            AttachEntity(childEntity, parentEntity, string.Empty, userData);
        }

        /// <summary>
        /// 附加子实体。
        /// </summary>
        /// <param name="childEntityId">要附加的子实体的实体编号。</param>
        /// <param name="parentEntityId">被附加的父实体的实体编号。</param>
        /// <param name="parentTransformPath">相对于被附加父实体的位置。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void AttachEntity(int childEntityId, int parentEntityId, string parentTransformPath, object userData)
        {
            AttachEntity(GetEntity(childEntityId), GetEntity(parentEntityId), parentTransformPath, userData);
        }

        /// <summary>
        /// 附加子实体。
        /// </summary>
        /// <param name="childEntityId">要附加的子实体的实体编号。</param>
        /// <param name="parentEntity">被附加的父实体。</param>
        /// <param name="parentTransformPath">相对于被附加父实体的位置。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void AttachEntity(int childEntityId, Entity parentEntity, string parentTransformPath, object userData)
        {
            AttachEntity(GetEntity(childEntityId), parentEntity, parentTransformPath, userData);
        }

        /// <summary>
        /// 附加子实体。
        /// </summary>
        /// <param name="childEntity">要附加的子实体。</param>
        /// <param name="parentEntityId">被附加的父实体的实体编号。</param>
        /// <param name="parentTransformPath">相对于被附加父实体的位置。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void AttachEntity(Entity childEntity, int parentEntityId, string parentTransformPath, object userData)
        {
            AttachEntity(childEntity, GetEntity(parentEntityId), parentTransformPath, userData);
        }

        /// <summary>
        /// 附加子实体。
        /// </summary>
        /// <param name="childEntity">要附加的子实体。</param>
        /// <param name="parentEntity">被附加的父实体。</param>
        /// <param name="parentTransformPath">相对于被附加父实体的位置。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void AttachEntity(Entity childEntity, Entity parentEntity, string parentTransformPath, object userData)
        {
            if (childEntity == null)
            {
                Log.Warning("Child entity is invalid.");
                return;
            }

            if (parentEntity == null)
            {
                Log.Warning("Parent entity is invalid.");
                return;
            }

            Transform parentTransform = null;
            if (string.IsNullOrEmpty(parentTransformPath))
            {
                parentTransform = parentEntity.Logic.CachedTransform;
            }
            else
            {
                parentTransform = parentEntity.Logic.CachedTransform.Find(parentTransformPath);
                if (parentTransform == null)
                {
                    Log.Warning("Can not find transform path '{0}' from parent entity '{1}'.", parentTransformPath, parentEntity.Logic.Name);
                    parentTransform = parentEntity.Logic.CachedTransform;
                }
            }

            AttachEntity(childEntity, parentEntity, parentTransform, userData);
        }

        /// <summary>
        /// 附加子实体。
        /// </summary>
        /// <param name="childEntityId">要附加的子实体的实体编号。</param>
        /// <param name="parentEntityId">被附加的父实体的实体编号。</param>
        /// <param name="parentTransform">相对于被附加父实体的位置。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void AttachEntity(int childEntityId, int parentEntityId, Transform parentTransform, object userData)
        {
            AttachEntity(GetEntity(childEntityId), GetEntity(parentEntityId), parentTransform, userData);
        }

        /// <summary>
        /// 附加子实体。
        /// </summary>
        /// <param name="childEntityId">要附加的子实体的实体编号。</param>
        /// <param name="parentEntity">被附加的父实体。</param>
        /// <param name="parentTransform">相对于被附加父实体的位置。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void AttachEntity(int childEntityId, Entity parentEntity, Transform parentTransform, object userData)
        {
            AttachEntity(GetEntity(childEntityId), parentEntity, parentTransform, userData);
        }

        /// <summary>
        /// 附加子实体。
        /// </summary>
        /// <param name="childEntity">要附加的子实体。</param>
        /// <param name="parentEntityId">被附加的父实体的实体编号。</param>
        /// <param name="parentTransform">相对于被附加父实体的位置。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void AttachEntity(Entity childEntity, int parentEntityId, Transform parentTransform, object userData)
        {
            AttachEntity(childEntity, GetEntity(parentEntityId), parentTransform, userData);
        }

        /// <summary>
        /// 附加子实体。
        /// </summary>
        /// <param name="childEntity">要附加的子实体。</param>
        /// <param name="parentEntity">被附加的父实体。</param>
        /// <param name="parentTransform">相对于被附加父实体的位置。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void AttachEntity(Entity childEntity, Entity parentEntity, Transform parentTransform, object userData)
        {
            if (childEntity == null)
            {
                Log.Warning("Child entity is invalid.");
                return;
            }

            if (parentEntity == null)
            {
                Log.Warning("Parent entity is invalid.");
                return;
            }

            if (parentTransform == null)
            {
                parentTransform = parentEntity.Logic.CachedTransform;
            }

            m_EntityManager.AttachEntity(childEntity, parentEntity, new AttachEntityInfo(parentTransform, userData));
        }

        /// <summary>
        /// 解除子实体。
        /// </summary>
        /// <param name="childEntityId">要解除的子实体的实体编号。</param>
        public void DetachEntity(int childEntityId)
        {
            m_EntityManager.DetachEntity(childEntityId);
        }

        /// <summary>
        /// 解除子实体。
        /// </summary>
        /// <param name="childEntityId">要解除的子实体的实体编号。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void DetachEntity(int childEntityId, object userData)
        {
            m_EntityManager.DetachEntity(childEntityId, userData);
        }

        /// <summary>
        /// 解除子实体。
        /// </summary>
        /// <param name="childEntity">要解除的子实体。</param>
        public void DetachEntity(Entity childEntity)
        {
            m_EntityManager.DetachEntity(childEntity);
        }

        /// <summary>
        /// 解除子实体。
        /// </summary>
        /// <param name="childEntity">要解除的子实体。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void DetachEntity(Entity childEntity, object userData)
        {
            m_EntityManager.DetachEntity(childEntity, userData);
        }

        /// <summary>
        /// 解除所有子实体。
        /// </summary>
        /// <param name="parentEntityId">被解除的父实体的实体编号。</param>
        public void DetachChildEntities(int parentEntityId)
        {
            m_EntityManager.DetachChildEntities(parentEntityId);
        }

        /// <summary>
        /// 解除所有子实体。
        /// </summary>
        /// <param name="parentEntityId">被解除的父实体的实体编号。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void DetachChildEntities(int parentEntityId, object userData)
        {
            m_EntityManager.DetachChildEntities(parentEntityId, userData);
        }

        /// <summary>
        /// 解除所有子实体。
        /// </summary>
        /// <param name="parentEntity">被解除的父实体。</param>
        public void DetachChildEntities(Entity parentEntity)
        {
            m_EntityManager.DetachChildEntities(parentEntity);
        }

        /// <summary>
        /// 解除所有子实体。
        /// </summary>
        /// <param name="parentEntity">被解除的父实体。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void DetachChildEntities(Entity parentEntity, object userData)
        {
            m_EntityManager.DetachChildEntities(parentEntity, userData);
        }

        /// <summary>
        /// 设置实体实例是否被加锁。
        /// </summary>
        /// <param name="entity">实体。</param>
        /// <param name="locked">实体实例是否被加锁。</param>
        public void SetInstanceLocked(Entity entity, bool locked)
        {
            m_EntityManager.SetInstanceLocked(entity, locked);
        }

        /// <summary>
        /// 设置实体实例的优先级。
        /// </summary>
        /// <param name="entity">实体。</param>
        /// <param name="priority">实体实例优先级。</param>
        public void SetInstancePriority(Entity entity, int priority)
        {
            m_EntityManager.SetInstancePriority(entity, priority);
        }

        private void OnShowEntitySuccess(object sender, GameFramework.Entity.ShowEntitySuccessEventArgs e)
        {
            if (m_EnableShowEntitySuccessEvent)
            {
                m_EventComponent.Fire(this, new ShowEntitySuccessEventArgs(e));
            }
        }

        private void OnShowEntityFailure(object sender, GameFramework.Entity.ShowEntityFailureEventArgs e)
        {
            Log.Warning("Show entity failure, entity id '{0}', asset name '{1}', entity group name '{2}', error message '{3}'.", e.EntityId.ToString(), e.EntityAssetName, e.EntityGroupName, e.ErrorMessage);
            if (m_EnableShowEntityFailureEvent)
            {
                m_EventComponent.Fire(this, new ShowEntityFailureEventArgs(e));
            }
        }

        private void OnShowEntityUpdate(object sender, GameFramework.Entity.ShowEntityUpdateEventArgs e)
        {
            if (m_EnableShowEntityUpdateEvent)
            {
                m_EventComponent.Fire(this, new ShowEntityUpdateEventArgs(e));
            }
        }

        private void OnShowEntityDependencyAsset(object sender, GameFramework.Entity.ShowEntityDependencyAssetEventArgs e)
        {
            if (m_EnableShowEntityDependencyAssetEvent)
            {
                m_EventComponent.Fire(this, new ShowEntityDependencyAssetEventArgs(e));
            }
        }

        private void OnHideEntityComplete(object sender, GameFramework.Entity.HideEntityCompleteEventArgs e)
        {
            if (m_EnableHideEntityCompleteEvent)
            {
                m_EventComponent.Fire(this, new HideEntityCompleteEventArgs(e));
            }
        }
    }
}
