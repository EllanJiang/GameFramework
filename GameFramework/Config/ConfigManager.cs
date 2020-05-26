//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework.Resource;
using System;
using System.Collections.Generic;

namespace GameFramework.Config
{
    /// <summary>
    /// 全局配置管理器。
    /// </summary>
    internal sealed partial class ConfigManager : GameFrameworkModule, IConfigManager
    {
        private readonly Dictionary<string, ConfigData> m_ConfigDatas;
        private readonly LoadAssetCallbacks m_LoadAssetCallbacks;
        private readonly LoadBinaryCallbacks m_LoadBinaryCallbacks;
        private IResourceManager m_ResourceManager;
        private IConfigHelper m_ConfigHelper;
        private EventHandler<LoadConfigSuccessEventArgs> m_LoadConfigSuccessEventHandler;
        private EventHandler<LoadConfigFailureEventArgs> m_LoadConfigFailureEventHandler;
        private EventHandler<LoadConfigUpdateEventArgs> m_LoadConfigUpdateEventHandler;
        private EventHandler<LoadConfigDependencyAssetEventArgs> m_LoadConfigDependencyAssetEventHandler;

        /// <summary>
        /// 初始化全局配置管理器的新实例。
        /// </summary>
        public ConfigManager()
        {
            m_ConfigDatas = new Dictionary<string, ConfigData>();
            m_LoadAssetCallbacks = new LoadAssetCallbacks(LoadAssetSuccessCallback, LoadAssetOrBinaryFailureCallback, LoadAssetUpdateCallback, LoadAssetDependencyAssetCallback);
            m_LoadBinaryCallbacks = new LoadBinaryCallbacks(LoadBinarySuccessCallback, LoadAssetOrBinaryFailureCallback);
            m_ResourceManager = null;
            m_ConfigHelper = null;
            m_LoadConfigSuccessEventHandler = null;
            m_LoadConfigFailureEventHandler = null;
            m_LoadConfigUpdateEventHandler = null;
            m_LoadConfigDependencyAssetEventHandler = null;
        }

        /// <summary>
        /// 获取全局配置项数量。
        /// </summary>
        public int Count
        {
            get
            {
                return m_ConfigDatas.Count;
            }
        }

        /// <summary>
        /// 加载全局配置成功事件。
        /// </summary>
        public event EventHandler<LoadConfigSuccessEventArgs> LoadConfigSuccess
        {
            add
            {
                m_LoadConfigSuccessEventHandler += value;
            }
            remove
            {
                m_LoadConfigSuccessEventHandler -= value;
            }
        }

        /// <summary>
        /// 加载全局配置失败事件。
        /// </summary>
        public event EventHandler<LoadConfigFailureEventArgs> LoadConfigFailure
        {
            add
            {
                m_LoadConfigFailureEventHandler += value;
            }
            remove
            {
                m_LoadConfigFailureEventHandler -= value;
            }
        }

        /// <summary>
        /// 加载全局配置更新事件。
        /// </summary>
        public event EventHandler<LoadConfigUpdateEventArgs> LoadConfigUpdate
        {
            add
            {
                m_LoadConfigUpdateEventHandler += value;
            }
            remove
            {
                m_LoadConfigUpdateEventHandler -= value;
            }
        }

        /// <summary>
        /// 加载全局配置时加载依赖资源事件。
        /// </summary>
        public event EventHandler<LoadConfigDependencyAssetEventArgs> LoadConfigDependencyAsset
        {
            add
            {
                m_LoadConfigDependencyAssetEventHandler += value;
            }
            remove
            {
                m_LoadConfigDependencyAssetEventHandler -= value;
            }
        }

        /// <summary>
        /// 全局配置管理器轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {
        }

        /// <summary>
        /// 关闭并清理全局配置管理器。
        /// </summary>
        internal override void Shutdown()
        {
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
        /// 设置全局配置辅助器。
        /// </summary>
        /// <param name="configHelper">全局配置辅助器。</param>
        public void SetConfigHelper(IConfigHelper configHelper)
        {
            if (configHelper == null)
            {
                throw new GameFrameworkException("Config helper is invalid.");
            }

            m_ConfigHelper = configHelper;
        }

        /// <summary>
        /// 加载全局配置。
        /// </summary>
        /// <param name="configAssetName">全局配置资源名称。</param>
        public void LoadConfig(string configAssetName)
        {
            LoadConfig(configAssetName, Constant.DefaultPriority, null);
        }

        /// <summary>
        /// 加载全局配置。
        /// </summary>
        /// <param name="configAssetName">全局配置资源名称。</param>
        /// <param name="priority">加载全局配置资源的优先级。</param>
        public void LoadConfig(string configAssetName, int priority)
        {
            LoadConfig(configAssetName, priority, null);
        }

        /// <summary>
        /// 加载全局配置。
        /// </summary>
        /// <param name="configAssetName">全局配置资源名称。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void LoadConfig(string configAssetName, object userData)
        {
            LoadConfig(configAssetName, Constant.DefaultPriority, userData);
        }

        /// <summary>
        /// 加载全局配置。
        /// </summary>
        /// <param name="configAssetName">全局配置资源名称。</param>
        /// <param name="priority">加载全局配置资源的优先级。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void LoadConfig(string configAssetName, int priority, object userData)
        {
            if (m_ResourceManager == null)
            {
                throw new GameFrameworkException("You must set resource manager first.");
            }

            if (m_ConfigHelper == null)
            {
                throw new GameFrameworkException("You must set config helper first.");
            }

            HasAssetResult result = m_ResourceManager.HasAsset(configAssetName);
            switch (result)
            {
                case HasAssetResult.Asset:
                    m_ResourceManager.LoadAsset(configAssetName, priority, m_LoadAssetCallbacks, userData);
                    break;

                case HasAssetResult.Binary:
                    m_ResourceManager.LoadBinary(configAssetName, m_LoadBinaryCallbacks, userData);
                    break;

                default:
                    throw new GameFrameworkException(Utility.Text.Format("Config asset '{0}' is '{1}'.", configAssetName, result.ToString()));
            }
        }

        /// <summary>
        /// 解析全局配置。
        /// </summary>
        /// <param name="configData">要解析的全局配置数据。</param>
        /// <returns>是否解析全局配置成功。</returns>
        public bool ParseConfig(object configData)
        {
            return ParseConfig(configData, null);
        }

        /// <summary>
        /// 解析全局配置。
        /// </summary>
        /// <param name="configData">要解析的全局配置数据。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否解析全局配置成功。</returns>
        public bool ParseConfig(object configData, object userData)
        {
            if (m_ConfigHelper == null)
            {
                throw new GameFrameworkException("You must set config helper first.");
            }

            try
            {
                return m_ConfigHelper.ParseConfig(configData, userData);
            }
            catch (Exception exception)
            {
                if (exception is GameFrameworkException)
                {
                    throw;
                }

                throw new GameFrameworkException(Utility.Text.Format("Can not parse config with exception '{0}'.", exception.ToString()), exception);
            }
        }

        /// <summary>
        /// 检查是否存在指定全局配置项。
        /// </summary>
        /// <param name="configName">要检查全局配置项的名称。</param>
        /// <returns>指定的全局配置项是否存在。</returns>
        public bool HasConfig(string configName)
        {
            return GetConfigData(configName).HasValue;
        }

        /// <summary>
        /// 增加指定全局配置项。
        /// </summary>
        /// <param name="configName">要增加全局配置项的名称。</param>
        /// <param name="boolValue">全局配置项布尔值。</param>
        /// <param name="intValue">全局配置项整数值。</param>
        /// <param name="floatValue">全局配置项浮点数值。</param>
        /// <param name="stringValue">全局配置项字符串值。</param>
        /// <returns>是否增加全局配置项成功。</returns>
        public bool AddConfig(string configName, bool boolValue, int intValue, float floatValue, string stringValue)
        {
            if (HasConfig(configName))
            {
                return false;
            }

            m_ConfigDatas.Add(configName, new ConfigData(boolValue, intValue, floatValue, stringValue));
            return true;
        }

        /// <summary>
        /// 移除指定全局配置项。
        /// </summary>
        /// <param name="configName">要移除全局配置项的名称。</param>
        public bool RemoveConfig(string configName)
        {
            if (!HasConfig(configName))
            {
                return false;
            }

            return m_ConfigDatas.Remove(configName);
        }

        /// <summary>
        /// 清空所有全局配置项。
        /// </summary>
        public void RemoveAllConfigs()
        {
            m_ConfigDatas.Clear();
        }

        /// <summary>
        /// 从指定全局配置项中读取布尔值。
        /// </summary>
        /// <param name="configName">要获取全局配置项的名称。</param>
        /// <returns>读取的布尔值。</returns>
        public bool GetBool(string configName)
        {
            ConfigData? configData = GetConfigData(configName);
            if (!configData.HasValue)
            {
                throw new GameFrameworkException(Utility.Text.Format("Config name '{0}' is not exist.", configName));
            }

            return configData.Value.BoolValue;
        }

        /// <summary>
        /// 从指定全局配置项中读取布尔值。
        /// </summary>
        /// <param name="configName">要获取全局配置项的名称。</param>
        /// <param name="defaultValue">当指定的全局配置项不存在时，返回此默认值。</param>
        /// <returns>读取的布尔值。</returns>
        public bool GetBool(string configName, bool defaultValue)
        {
            ConfigData? configData = GetConfigData(configName);
            return configData.HasValue ? configData.Value.BoolValue : defaultValue;
        }

        /// <summary>
        /// 从指定全局配置项中读取整数值。
        /// </summary>
        /// <param name="configName">要获取全局配置项的名称。</param>
        /// <returns>读取的整数值。</returns>
        public int GetInt(string configName)
        {
            ConfigData? configData = GetConfigData(configName);
            if (!configData.HasValue)
            {
                throw new GameFrameworkException(Utility.Text.Format("Config name '{0}' is not exist.", configName));
            }

            return configData.Value.IntValue;
        }

        /// <summary>
        /// 从指定全局配置项中读取整数值。
        /// </summary>
        /// <param name="configName">要获取全局配置项的名称。</param>
        /// <param name="defaultValue">当指定的全局配置项不存在时，返回此默认值。</param>
        /// <returns>读取的整数值。</returns>
        public int GetInt(string configName, int defaultValue)
        {
            ConfigData? configData = GetConfigData(configName);
            return configData.HasValue ? configData.Value.IntValue : defaultValue;
        }

        /// <summary>
        /// 从指定全局配置项中读取浮点数值。
        /// </summary>
        /// <param name="configName">要获取全局配置项的名称。</param>
        /// <returns>读取的浮点数值。</returns>
        public float GetFloat(string configName)
        {
            ConfigData? configData = GetConfigData(configName);
            if (!configData.HasValue)
            {
                throw new GameFrameworkException(Utility.Text.Format("Config name '{0}' is not exist.", configName));
            }

            return configData.Value.FloatValue;
        }

        /// <summary>
        /// 从指定全局配置项中读取浮点数值。
        /// </summary>
        /// <param name="configName">要获取全局配置项的名称。</param>
        /// <param name="defaultValue">当指定的全局配置项不存在时，返回此默认值。</param>
        /// <returns>读取的浮点数值。</returns>
        public float GetFloat(string configName, float defaultValue)
        {
            ConfigData? configData = GetConfigData(configName);
            return configData.HasValue ? configData.Value.FloatValue : defaultValue;
        }

        /// <summary>
        /// 从指定全局配置项中读取字符串值。
        /// </summary>
        /// <param name="configName">要获取全局配置项的名称。</param>
        /// <returns>读取的字符串值。</returns>
        public string GetString(string configName)
        {
            ConfigData? configData = GetConfigData(configName);
            if (!configData.HasValue)
            {
                throw new GameFrameworkException(Utility.Text.Format("Config name '{0}' is not exist.", configName));
            }

            return configData.Value.StringValue;
        }

        /// <summary>
        /// 从指定全局配置项中读取字符串值。
        /// </summary>
        /// <param name="configName">要获取全局配置项的名称。</param>
        /// <param name="defaultValue">当指定的全局配置项不存在时，返回此默认值。</param>
        /// <returns>读取的字符串值。</returns>
        public string GetString(string configName, string defaultValue)
        {
            ConfigData? configData = GetConfigData(configName);
            return configData.HasValue ? configData.Value.StringValue : defaultValue;
        }

        private ConfigData? GetConfigData(string configName)
        {
            if (string.IsNullOrEmpty(configName))
            {
                throw new GameFrameworkException("Config name is invalid.");
            }

            ConfigData configData = default(ConfigData);
            if (m_ConfigDatas.TryGetValue(configName, out configData))
            {
                return configData;
            }

            return null;
        }

        private void LoadAssetSuccessCallback(string configAssetName, object configAsset, float duration, object userData)
        {
            try
            {
                if (!m_ConfigHelper.LoadConfig(configAssetName, configAsset, userData))
                {
                    throw new GameFrameworkException(Utility.Text.Format("Load config failure in helper, asset name '{0}'.", configAssetName));
                }

                if (m_LoadConfigSuccessEventHandler != null)
                {
                    LoadConfigSuccessEventArgs loadConfigSuccessEventArgs = LoadConfigSuccessEventArgs.Create(configAssetName, duration, userData);
                    m_LoadConfigSuccessEventHandler(this, loadConfigSuccessEventArgs);
                    ReferencePool.Release(loadConfigSuccessEventArgs);
                }
            }
            catch (Exception exception)
            {
                if (m_LoadConfigFailureEventHandler != null)
                {
                    LoadConfigFailureEventArgs loadConfigFailureEventArgs = LoadConfigFailureEventArgs.Create(configAssetName, exception.ToString(), userData);
                    m_LoadConfigFailureEventHandler(this, loadConfigFailureEventArgs);
                    ReferencePool.Release(loadConfigFailureEventArgs);
                    return;
                }

                throw;
            }
            finally
            {
                m_ConfigHelper.ReleaseConfigAsset(configAsset);
            }
        }

        private void LoadAssetOrBinaryFailureCallback(string configAssetName, LoadResourceStatus status, string errorMessage, object userData)
        {
            string appendErrorMessage = Utility.Text.Format("Load config failure, asset name '{0}', status '{1}', error message '{2}'.", configAssetName, status.ToString(), errorMessage);
            if (m_LoadConfigFailureEventHandler != null)
            {
                LoadConfigFailureEventArgs loadConfigFailureEventArgs = LoadConfigFailureEventArgs.Create(configAssetName, appendErrorMessage, userData);
                m_LoadConfigFailureEventHandler(this, loadConfigFailureEventArgs);
                ReferencePool.Release(loadConfigFailureEventArgs);
                return;
            }

            throw new GameFrameworkException(appendErrorMessage);
        }

        private void LoadAssetUpdateCallback(string configAssetName, float progress, object userData)
        {
            if (m_LoadConfigUpdateEventHandler != null)
            {
                LoadConfigUpdateEventArgs loadConfigUpdateEventArgs = LoadConfigUpdateEventArgs.Create(configAssetName, progress, userData);
                m_LoadConfigUpdateEventHandler(this, loadConfigUpdateEventArgs);
                ReferencePool.Release(loadConfigUpdateEventArgs);
            }
        }

        private void LoadAssetDependencyAssetCallback(string configAssetName, string dependencyAssetName, int loadedCount, int totalCount, object userData)
        {
            if (m_LoadConfigDependencyAssetEventHandler != null)
            {
                LoadConfigDependencyAssetEventArgs loadConfigDependencyAssetEventArgs = LoadConfigDependencyAssetEventArgs.Create(configAssetName, dependencyAssetName, loadedCount, totalCount, userData);
                m_LoadConfigDependencyAssetEventHandler(this, loadConfigDependencyAssetEventArgs);
                ReferencePool.Release(loadConfigDependencyAssetEventArgs);
            }
        }

        private void LoadBinarySuccessCallback(string configAssetName, byte[] configBytes, float duration, object userData)
        {
            try
            {
                if (!m_ConfigHelper.LoadConfig(configAssetName, configBytes, userData))
                {
                    throw new GameFrameworkException(Utility.Text.Format("Load config failure in helper, asset name '{0}'.", configAssetName));
                }

                if (m_LoadConfigSuccessEventHandler != null)
                {
                    LoadConfigSuccessEventArgs loadConfigSuccessEventArgs = LoadConfigSuccessEventArgs.Create(configAssetName, duration, userData);
                    m_LoadConfigSuccessEventHandler(this, loadConfigSuccessEventArgs);
                    ReferencePool.Release(loadConfigSuccessEventArgs);
                }
            }
            catch (Exception exception)
            {
                if (m_LoadConfigFailureEventHandler != null)
                {
                    LoadConfigFailureEventArgs loadConfigFailureEventArgs = LoadConfigFailureEventArgs.Create(configAssetName, exception.ToString(), userData);
                    m_LoadConfigFailureEventHandler(this, loadConfigFailureEventArgs);
                    ReferencePool.Release(loadConfigFailureEventArgs);
                    return;
                }

                throw;
            }
        }
    }
}
