//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.ObjectPool;
using GameFramework.Resource;
using System;

namespace GameFramework.UI
{
    /// <summary>
    /// 界面管理器接口。
    /// </summary>
    public interface IUIManager
    {
        /// <summary>
        /// 获取界面组数量。
        /// </summary>
        int UIGroupCount
        {
            get;
        }

        /// <summary>
        /// 获取或设置界面实例对象池自动释放可释放对象的间隔秒数。
        /// </summary>
        float InstanceAutoReleaseInterval
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置界面实例对象池的容量。
        /// </summary>
        int InstanceCapacity
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置界面实例对象池对象过期秒数。
        /// </summary>
        float InstanceExpireTime
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置界面实例对象池的优先级。
        /// </summary>
        int InstancePriority
        {
            get;
            set;
        }

        /// <summary>
        /// 打开界面成功事件。
        /// </summary>
        event EventHandler<OpenUIFormSuccessEventArgs> OpenUIFormSuccess;

        /// <summary>
        /// 打开界面失败事件。
        /// </summary>
        event EventHandler<OpenUIFormFailureEventArgs> OpenUIFormFailure;

        /// <summary>
        /// 打开界面更新事件。
        /// </summary>
        event EventHandler<OpenUIFormUpdateEventArgs> OpenUIFormUpdate;

        /// <summary>
        /// 打开界面时加载依赖资源事件。
        /// </summary>
        event EventHandler<OpenUIFormDependencyAssetEventArgs> OpenUIFormDependencyAsset;

        /// <summary>
        /// 关闭界面完成事件。
        /// </summary>
        event EventHandler<CloseUIFormCompleteEventArgs> CloseUIFormComplete;

        /// <summary>
        /// 设置对象池管理器。
        /// </summary>
        /// <param name="objectPoolManager">对象池管理器。</param>
        void SetObjectPoolManager(IObjectPoolManager objectPoolManager);

        /// <summary>
        /// 设置资源管理器。
        /// </summary>
        /// <param name="resourceManager">资源管理器。</param>
        void SetResourceManager(IResourceManager resourceManager);

        /// <summary>
        /// 设置界面辅助器。
        /// </summary>
        /// <param name="uiFormHelper">界面辅助器。</param>
        void SetUIFormHelper(IUIFormHelper uiFormHelper);

        /// <summary>
        /// 是否存在界面组。
        /// </summary>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <returns>是否存在界面组。</returns>
        bool HasUIGroup(string uiGroupName);

        /// <summary>
        /// 获取界面组。
        /// </summary>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <returns>要获取的界面组。</returns>
        IUIGroup GetUIGroup(string uiGroupName);

        /// <summary>
        /// 获取所有界面组。
        /// </summary>
        /// <returns>所有界面组。</returns>
        IUIGroup[] GetAllUIGroups();

        /// <summary>
        /// 增加界面组。
        /// </summary>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="uiGroupHelper">界面组辅助器。</param>
        /// <returns>是否增加界面组成功。</returns>
        bool AddUIGroup(string uiGroupName, IUIGroupHelper uiGroupHelper);

        /// <summary>
        /// 增加界面组。
        /// </summary>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="uiGroupDepth">界面组深度。</param>
        /// <param name="uiGroupHelper">界面组辅助器。</param>
        /// <returns>是否增加界面组成功。</returns>
        bool AddUIGroup(string uiGroupName, int uiGroupDepth, IUIGroupHelper uiGroupHelper);

        /// <summary>
        /// 界面组中是否存在界面。
        /// </summary>
        /// <param name="uiFormTypeId">界面类型编号。</param>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <returns>界面组中是否存在界面。</returns>
        bool HasUIForm(int uiFormTypeId, string uiGroupName);

        /// <summary>
        /// 从界面组中获取界面。
        /// </summary>
        /// <param name="uiFormTypeId">界面类型编号。</param>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <returns>要获取的界面。</returns>
        IUIForm GetUIForm(int uiFormTypeId, string uiGroupName);

        /// <summary>
        /// 从界面组中获取界面。
        /// </summary>
        /// <param name="uiFormTypeId">界面类型编号。</param>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <returns>要获取的界面。</returns>
        IUIForm[] GetUIForms(int uiFormTypeId, string uiGroupName);

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiFormTypeId">界面类型编号。</param>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <param name="uiGroupName">界面组名称。</param>
        void OpenUIForm(int uiFormTypeId, string uiFormAssetName, string uiGroupName);

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiFormTypeId">界面类型编号。</param>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="pauseCoveredUIForm">是否暂停被覆盖的界面。</param>
        void OpenUIForm(int uiFormTypeId, string uiFormAssetName, string uiGroupName, bool pauseCoveredUIForm);

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiFormTypeId">界面类型编号。</param>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="userData">用户自定义数据。</param>
        void OpenUIForm(int uiFormTypeId, string uiFormAssetName, string uiGroupName, object userData);

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiFormTypeId">界面类型编号。</param>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="pauseCoveredUIForm">是否暂停被覆盖的界面。</param>
        /// <param name="userData">用户自定义数据。</param>
        void OpenUIForm(int uiFormTypeId, string uiFormAssetName, string uiGroupName, bool pauseCoveredUIForm, object userData);

        /// <summary>
        /// 关闭界面。
        /// </summary>
        /// <param name="uiForm">要关闭的界面。</param>
        void CloseUIForm(IUIForm uiForm);

        /// <summary>
        /// 关闭界面。
        /// </summary>
        /// <param name="uiForm">要关闭的界面。</param>
        /// <param name="userData">用户自定义数据。</param>
        void CloseUIForm(IUIForm uiForm, object userData);

        /// <summary>
        /// 激活界面。
        /// </summary>
        /// <param name="uiForm">要激活的界面。</param>
        void RefocusUIForm(IUIForm uiForm);

        /// <summary>
        /// 激活界面。
        /// </summary>
        /// <param name="uiForm">要激活的界面。</param>
        /// <param name="userData">用户自定义数据。</param>
        void RefocusUIForm(IUIForm uiForm, object userData);

        /// <summary>
        /// 设置界面是否被加锁。
        /// </summary>
        /// <param name="uiForm">界面。</param>
        /// <param name="locked">界面是否被加锁。</param>
        void SetUIFormLocked(IUIForm uiForm, bool locked);

        /// <summary>
        /// 设置界面的优先级。
        /// </summary>
        /// <param name="uiForm">界面。</param>
        /// <param name="priority">界面优先级。</param>
        void SetUIFormPriority(IUIForm uiForm, int priority);
    }
}
