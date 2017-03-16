//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.ObjectPool;
using GameFramework.Resource;
using System;
using System.Collections.Generic;

namespace GameFramework.UI
{
    /// <summary>
    /// 界面管理器。
    /// </summary>
    internal sealed partial class UIManager : GameFrameworkModule, IUIManager
    {
        private readonly IDictionary<string, UIGroup> m_UIGroups;
        private readonly LinkedList<RecycleNode> m_RecycleQueue;
        private readonly LoadAssetCallbacks m_LoadAssetCallbacks;
        private IObjectPoolManager m_ObjectPoolManager;
        private IResourceManager m_ResourceManager;
        private IObjectPool<UIFormInstanceObject> m_InstancePool;
        private IUIFormHelper m_UIFormHelper;
        private EventHandler<OpenUIFormSuccessEventArgs> m_OpenUIFormSuccessEventHandler;
        private EventHandler<OpenUIFormFailureEventArgs> m_OpenUIFormFailureEventHandler;
        private EventHandler<OpenUIFormUpdateEventArgs> m_OpenUIFormUpdateEventHandler;
        private EventHandler<OpenUIFormDependencyAssetEventArgs> m_OpenUIFormDependencyAssetEventHandler;
        private EventHandler<CloseUIFormCompleteEventArgs> m_CloseUIFormCompleteEventHandler;

        /// <summary>
        /// 初始化界面管理器的新实例。
        /// </summary>
        public UIManager()
        {
            m_UIGroups = new Dictionary<string, UIGroup>();
            m_RecycleQueue = new LinkedList<RecycleNode>();
            m_LoadAssetCallbacks = new LoadAssetCallbacks(LoadUIFormSuccessCallback, LoadUIFormFailureCallback, LoadUIFormUpdateCallback, LoadUIFormDependencyAssetCallback);
            m_ObjectPoolManager = null;
            m_ResourceManager = null;
            m_InstancePool = null;
            m_UIFormHelper = null;
            m_OpenUIFormSuccessEventHandler = null;
            m_OpenUIFormFailureEventHandler = null;
            m_OpenUIFormUpdateEventHandler = null;
            m_OpenUIFormDependencyAssetEventHandler = null;
            m_CloseUIFormCompleteEventHandler = null;
        }

        /// <summary>
        /// 获取界面组数量。
        /// </summary>
        public int UIGroupCount
        {
            get
            {
                return m_UIGroups.Count;
            }
        }

        /// <summary>
        /// 获取或设置界面实例对象池自动释放可释放对象的间隔秒数。
        /// </summary>
        public float InstanceAutoReleaseInterval
        {
            get
            {
                return m_InstancePool.AutoReleaseInterval;
            }
            set
            {
                m_InstancePool.AutoReleaseInterval = value;
            }
        }

        /// <summary>
        /// 获取或设置界面实例对象池的容量。
        /// </summary>
        public int InstanceCapacity
        {
            get
            {
                return m_InstancePool.Capacity;
            }
            set
            {
                m_InstancePool.Capacity = value;
            }
        }

        /// <summary>
        /// 获取或设置界面实例对象池对象过期秒数。
        /// </summary>
        public float InstanceExpireTime
        {
            get
            {
                return m_InstancePool.ExpireTime;
            }
            set
            {
                m_InstancePool.ExpireTime = value;
            }
        }

        /// <summary>
        /// 获取或设置界面实例对象池的优先级。
        /// </summary>
        public int InstancePriority
        {
            get
            {
                return m_InstancePool.Priority;
            }
            set
            {
                m_InstancePool.Priority = value;
            }
        }

        /// <summary>
        /// 打开界面成功事件。
        /// </summary>
        public event EventHandler<OpenUIFormSuccessEventArgs> OpenUIFormSuccess
        {
            add
            {
                m_OpenUIFormSuccessEventHandler += value;
            }
            remove
            {
                m_OpenUIFormSuccessEventHandler -= value;
            }
        }

        /// <summary>
        /// 打开界面失败事件。
        /// </summary>
        public event EventHandler<OpenUIFormFailureEventArgs> OpenUIFormFailure
        {
            add
            {
                m_OpenUIFormFailureEventHandler += value;
            }
            remove
            {
                m_OpenUIFormFailureEventHandler -= value;
            }
        }

        /// <summary>
        /// 打开界面更新事件。
        /// </summary>
        public event EventHandler<OpenUIFormUpdateEventArgs> OpenUIFormUpdate
        {
            add
            {
                m_OpenUIFormUpdateEventHandler += value;
            }
            remove
            {
                m_OpenUIFormUpdateEventHandler -= value;
            }
        }

        /// <summary>
        /// 打开界面时加载依赖资源事件。
        /// </summary>
        public event EventHandler<OpenUIFormDependencyAssetEventArgs> OpenUIFormDependencyAsset
        {
            add
            {
                m_OpenUIFormDependencyAssetEventHandler += value;
            }
            remove
            {
                m_OpenUIFormDependencyAssetEventHandler -= value;
            }
        }

        /// <summary>
        /// 关闭界面完成事件。
        /// </summary>
        public event EventHandler<CloseUIFormCompleteEventArgs> CloseUIFormComplete
        {
            add
            {
                m_CloseUIFormCompleteEventHandler += value;
            }
            remove
            {
                m_CloseUIFormCompleteEventHandler -= value;
            }
        }

        /// <summary>
        /// 界面管理器轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {
            while (m_RecycleQueue.Count > 0)
            {
                RecycleNode recycleNode = m_RecycleQueue.First.Value;
                recycleNode.TickCount++;
                if (recycleNode.TickCount <= 1)
                {
                    break;
                }

                m_RecycleQueue.RemoveFirst();
                IUIForm uiForm = recycleNode.UIForm;
                uiForm.OnRecycle();
                m_InstancePool.Unspawn(uiForm.Handle);
            }

            foreach (KeyValuePair<string, UIGroup> uiGroup in (Dictionary<string, UIGroup>)m_UIGroups)
            {
                uiGroup.Value.Update(elapseSeconds, realElapseSeconds);
            }
        }

        /// <summary>
        /// 关闭并清理界面管理器。
        /// </summary>
        internal override void Shutdown()
        {
            m_UIGroups.Clear();
            m_RecycleQueue.Clear();
        }

        /// <summary>
        /// 设置对象池管理器。
        /// </summary>
        /// <param name="objectPoolManager">对象池管理器。</param>
        public void SetObjectPoolManager(IObjectPoolManager objectPoolManager)
        {
            if (objectPoolManager == null)
            {
                throw new GameFrameworkException("Object pool manager is invalid.");
            }

            m_ObjectPoolManager = objectPoolManager;
            m_InstancePool = m_ObjectPoolManager.CreateSingleSpawnObjectPool<UIFormInstanceObject>("UI Instance Pool");
        }

        /// <summary>
        /// 设置资源管理器。
        /// </summary>
        /// <param name="resourceManager">资源管理器。</param>
        public void SetResourceManager(IResourceManager resourceManager)
        {
            if (resourceManager == null)
            {
                throw new GameFrameworkException("Resource manager is invalid.");
            }

            m_ResourceManager = resourceManager;
        }

        /// <summary>
        /// 设置界面辅助器。
        /// </summary>
        /// <param name="uiFormHelper">界面辅助器。</param>
        public void SetUIFormHelper(IUIFormHelper uiFormHelper)
        {
            if (uiFormHelper == null)
            {
                throw new GameFrameworkException("UI form helper is invalid.");
            }

            m_UIFormHelper = uiFormHelper;
        }

        /// <summary>
        /// 是否存在界面组。
        /// </summary>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <returns>是否存在界面组。</returns>
        public bool HasUIGroup(string uiGroupName)
        {
            if (string.IsNullOrEmpty(uiGroupName))
            {
                throw new GameFrameworkException("UI group name is invalid.");
            }

            return m_UIGroups.ContainsKey(uiGroupName);
        }

        /// <summary>
        /// 获取界面组。
        /// </summary>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <returns>要获取的界面组。</returns>
        public IUIGroup GetUIGroup(string uiGroupName)
        {
            if (string.IsNullOrEmpty(uiGroupName))
            {
                throw new GameFrameworkException("UI group name is invalid.");
            }

            UIGroup uiGroup = null;
            if (m_UIGroups.TryGetValue(uiGroupName, out uiGroup))
            {
                return uiGroup;
            }

            return null;
        }

        /// <summary>
        /// 获取所有界面组。
        /// </summary>
        /// <returns>所有界面组。</returns>
        public IUIGroup[] GetAllUIGroups()
        {
            int index = 0;
            IUIGroup[] uiGroups = new IUIGroup[m_UIGroups.Count];
            foreach (KeyValuePair<string, UIGroup> uiGroup in m_UIGroups)
            {
                uiGroups[index++] = uiGroup.Value;
            }

            return uiGroups;
        }

        /// <summary>
        /// 增加界面组。
        /// </summary>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="uiGroupHelper">界面组辅助器。</param>
        /// <returns>是否增加界面组成功。</returns>
        public bool AddUIGroup(string uiGroupName, IUIGroupHelper uiGroupHelper)
        {
            return AddUIGroup(uiGroupName, 0, uiGroupHelper);
        }

        /// <summary>
        /// 增加界面组。
        /// </summary>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="uiGroupDepth">界面组深度。</param>
        /// <param name="uiGroupHelper">界面组辅助器。</param>
        /// <returns>是否增加界面组成功。</returns>
        public bool AddUIGroup(string uiGroupName, int uiGroupDepth, IUIGroupHelper uiGroupHelper)
        {
            if (string.IsNullOrEmpty(uiGroupName))
            {
                throw new GameFrameworkException("UI group name is invalid.");
            }

            if (uiGroupHelper == null)
            {
                throw new GameFrameworkException("UI group helper is invalid.");
            }

            if (HasUIGroup(uiGroupName))
            {
                return false;
            }

            m_UIGroups.Add(uiGroupName, new UIGroup(uiGroupName, uiGroupDepth, uiGroupHelper));

            return true;
        }

        /// <summary>
        /// 界面组中是否存在界面。
        /// </summary>
        /// <param name="uiFormTypeId">界面类型编号。</param>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <returns>界面组中是否存在界面。</returns>
        public bool HasUIForm(int uiFormTypeId, string uiGroupName)
        {
            IUIGroup uiGroup = GetUIGroup(uiGroupName);
            if (uiGroup == null)
            {
                throw new GameFrameworkException(string.Format("UI group '{0}' is not exist.", uiGroupName));
            }

            return uiGroup.HasUIForm(uiFormTypeId);
        }

        /// <summary>
        /// 从界面组中获取界面。
        /// </summary>
        /// <param name="uiFormTypeId">界面类型编号。</param>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <returns>要获取的界面。</returns>
        public IUIForm GetUIForm(int uiFormTypeId, string uiGroupName)
        {
            IUIGroup uiGroup = GetUIGroup(uiGroupName);
            if (uiGroup == null)
            {
                throw new GameFrameworkException(string.Format("UI group '{0}' is not exist.", uiGroupName));
            }

            return uiGroup.GetUIForm(uiFormTypeId);
        }

        /// <summary>
        /// 从界面组中获取界面。
        /// </summary>
        /// <param name="uiFormTypeId">界面类型编号。</param>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <returns>要获取的界面。</returns>
        public IUIForm[] GetUIForms(int uiFormTypeId, string uiGroupName)
        {
            IUIGroup uiGroup = GetUIGroup(uiGroupName);
            if (uiGroup == null)
            {
                throw new GameFrameworkException(string.Format("UI group '{0}' is not exist.", uiGroupName));
            }

            return uiGroup.GetUIForms(uiFormTypeId);
        }

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiFormTypeId">界面类型编号。</param>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <param name="uiGroupName">界面组名称。</param>
        public void OpenUIForm(int uiFormTypeId, string uiFormAssetName, string uiGroupName)
        {
            OpenUIForm(uiFormTypeId, uiFormAssetName, uiGroupName, false, null);
        }

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiFormTypeId">界面类型编号。</param>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="pauseCoveredUIForm">是否暂停被覆盖的界面。</param>
        public void OpenUIForm(int uiFormTypeId, string uiFormAssetName, string uiGroupName, bool pauseCoveredUIForm)
        {
            OpenUIForm(uiFormTypeId, uiFormAssetName, uiGroupName, pauseCoveredUIForm, null);
        }

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiFormTypeId">界面类型编号。</param>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void OpenUIForm(int uiFormTypeId, string uiFormAssetName, string uiGroupName, object userData)
        {
            OpenUIForm(uiFormTypeId, uiFormAssetName, uiGroupName, false, userData);
        }

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiFormTypeId">界面类型编号。</param>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="pauseCoveredUIForm">是否暂停被覆盖的界面。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void OpenUIForm(int uiFormTypeId, string uiFormAssetName, string uiGroupName, bool pauseCoveredUIForm, object userData)
        {
            if (m_ResourceManager == null)
            {
                throw new GameFrameworkException("You must set resource manager first.");
            }

            if (m_UIFormHelper == null)
            {
                throw new GameFrameworkException("You must set UI form helper first.");
            }

            UIGroup uiGroup = (UIGroup)GetUIGroup(uiGroupName);
            if (uiGroup == null)
            {
                throw new GameFrameworkException(string.Format("UI group '{0}' is not exist.", uiGroupName));
            }

            UIFormInstanceObject uiFormInstanceObject = m_InstancePool.Spawn(uiFormAssetName);
            if (uiFormInstanceObject == null)
            {
                m_ResourceManager.LoadAsset(uiFormAssetName, m_LoadAssetCallbacks, new OpenUIFormInfo(uiFormTypeId, uiGroup, pauseCoveredUIForm, userData));
                return;
            }

            InternalOpenUIForm(uiFormTypeId, uiFormAssetName, uiGroup, uiFormInstanceObject.Target, pauseCoveredUIForm, false, 0f, userData);
        }

        /// <summary>
        /// 关闭界面。
        /// </summary>
        /// <param name="uiForm">要关闭的界面。</param>
        public void CloseUIForm(IUIForm uiForm)
        {
            CloseUIForm(uiForm, null);
        }

        /// <summary>
        /// 关闭界面。
        /// </summary>
        /// <param name="uiForm">要关闭的界面。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void CloseUIForm(IUIForm uiForm, object userData)
        {
            if (uiForm == null)
            {
                throw new GameFrameworkException("UI form is invalid.");
            }

            UIGroup uiGroup = (UIGroup)uiForm.UIGroup;
            if (uiGroup == null)
            {
                throw new GameFrameworkException("UI group is invalid.");
            }

            uiGroup.RemoveUIForm(uiForm);
            uiForm.OnClose(userData);
            uiGroup.Refresh();

            if (m_CloseUIFormCompleteEventHandler != null)
            {
                m_CloseUIFormCompleteEventHandler(this, new CloseUIFormCompleteEventArgs(uiForm.TypeId, userData));
            }

            m_RecycleQueue.AddLast(new RecycleNode(uiForm));
        }

        /// <summary>
        /// 激活界面。
        /// </summary>
        /// <param name="uiForm">要激活的界面。</param>
        public void RefocusUIForm(IUIForm uiForm)
        {
            RefocusUIForm(uiForm, null);
        }

        /// <summary>
        /// 激活界面。
        /// </summary>
        /// <param name="uiForm">要激活的界面。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void RefocusUIForm(IUIForm uiForm, object userData)
        {
            if (uiForm == null)
            {
                throw new GameFrameworkException("UI form is invalid.");
            }

            UIGroup uiGroup = (UIGroup)uiForm.UIGroup;
            if (uiGroup == null)
            {
                throw new GameFrameworkException("UI group is invalid.");
            }

            uiGroup.RefocusUIForm(uiForm, userData);
            uiGroup.Refresh();
            uiForm.OnRefocus(userData);
        }

        /// <summary>
        /// 设置界面是否被加锁。
        /// </summary>
        /// <param name="uiForm">界面。</param>
        /// <param name="locked">界面是否被加锁。</param>
        public void SetUIFormLocked(IUIForm uiForm, bool locked)
        {
            if (uiForm == null)
            {
                throw new GameFrameworkException("UI form is invalid.");
            }

            m_InstancePool.SetLocked(uiForm, locked);
        }

        /// <summary>
        /// 设置界面的优先级。
        /// </summary>
        /// <param name="uiForm">界面。</param>
        /// <param name="priority">界面优先级。</param>
        public void SetUIFormPriority(IUIForm uiForm, int priority)
        {
            if (uiForm == null)
            {
                throw new GameFrameworkException("UI form is invalid.");
            }

            m_InstancePool.SetPriority(uiForm, priority);
        }

        private void InternalOpenUIForm(int uiFormTypeId, string uiFormAssetName, UIGroup uiGroup, object uiFormInstance, bool pauseCoveredUIForm, bool isNewInstance, float duration, object userData)
        {
            try
            {
                IUIForm uiForm = m_UIFormHelper.CreateUIForm(uiFormInstance, uiGroup, userData);
                if (uiForm == null)
                {
                    throw new GameFrameworkException("Can not create UI form in helper.");
                }

                uiForm.OnInit(uiFormTypeId, uiGroup, pauseCoveredUIForm, isNewInstance, userData);
                uiGroup.AddUIForm(uiForm);
                uiForm.OnOpen(userData);
                uiGroup.Refresh();

                if (m_OpenUIFormSuccessEventHandler != null)
                {
                    m_OpenUIFormSuccessEventHandler(this, new OpenUIFormSuccessEventArgs(uiFormAssetName, uiForm, duration, userData));
                }
            }
            catch (Exception exception)
            {
                if (m_OpenUIFormFailureEventHandler != null)
                {
                    m_OpenUIFormFailureEventHandler(this, new OpenUIFormFailureEventArgs(uiFormTypeId, uiFormAssetName, uiGroup.Name, pauseCoveredUIForm, exception.ToString(), userData));
                    return;
                }

                throw;
            }
        }

        private void LoadUIFormSuccessCallback(string uiFormAssetName, object uiFormAsset, float duration, object userData)
        {
            UIFormInstanceObject uiFormInstanceObject = new UIFormInstanceObject(uiFormAssetName, uiFormAsset, m_UIFormHelper.InstantiateUIForm(uiFormAsset), m_UIFormHelper);
            m_InstancePool.Register(uiFormInstanceObject, true);

            OpenUIFormInfo openUIFormInfo = (OpenUIFormInfo)userData;
            if (openUIFormInfo == null)
            {
                throw new GameFrameworkException("Open UI form info is invalid.");
            }

            InternalOpenUIForm(openUIFormInfo.UIFormTypeId, uiFormAssetName, openUIFormInfo.UIGroup, uiFormInstanceObject.Target, openUIFormInfo.PauseCoveredUIForm, true, duration, openUIFormInfo.UserData);
        }

        private void LoadUIFormFailureCallback(string uiFormAssetName, LoadResourceStatus status, string errorMessage, object userData)
        {
            OpenUIFormInfo openUIFormInfo = (OpenUIFormInfo)userData;
            if (openUIFormInfo == null)
            {
                throw new GameFrameworkException("Open UI form info is invalid.");
            }

            string appendErrorMessage = string.Format("Load UI form failure, asset name '{0}', status '{1}', error message '{2}'.", uiFormAssetName, status.ToString(), errorMessage);
            if (m_OpenUIFormFailureEventHandler != null)
            {
                m_OpenUIFormFailureEventHandler(this, new OpenUIFormFailureEventArgs(openUIFormInfo.UIFormTypeId, uiFormAssetName, openUIFormInfo.UIGroup.Name, openUIFormInfo.PauseCoveredUIForm, appendErrorMessage, openUIFormInfo.UserData));
                return;
            }

            throw new GameFrameworkException(appendErrorMessage);
        }

        private void LoadUIFormUpdateCallback(string uiFormAssetName, float progress, object userData)
        {
            OpenUIFormInfo openUIFormInfo = (OpenUIFormInfo)userData;
            if (openUIFormInfo == null)
            {
                throw new GameFrameworkException("Open UI form info is invalid.");
            }

            if (m_OpenUIFormUpdateEventHandler != null)
            {
                m_OpenUIFormUpdateEventHandler(this, new OpenUIFormUpdateEventArgs(openUIFormInfo.UIFormTypeId, uiFormAssetName, openUIFormInfo.UIGroup.Name, openUIFormInfo.PauseCoveredUIForm, progress, openUIFormInfo.UserData));
            }
        }

        private void LoadUIFormDependencyAssetCallback(string uiFormAssetName, string dependencyAssetName, int loadedCount, int totalCount, object userData)
        {
            OpenUIFormInfo openUIFormInfo = (OpenUIFormInfo)userData;
            if (openUIFormInfo == null)
            {
                throw new GameFrameworkException("Open UI form info is invalid.");
            }

            if (m_OpenUIFormDependencyAssetEventHandler != null)
            {
                m_OpenUIFormDependencyAssetEventHandler(this, new OpenUIFormDependencyAssetEventArgs(openUIFormInfo.UIFormTypeId, uiFormAssetName, openUIFormInfo.UIGroup.Name, openUIFormInfo.PauseCoveredUIForm, dependencyAssetName, loadedCount, totalCount, openUIFormInfo.UserData));
            }
        }
    }
}
