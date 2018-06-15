//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.Resource;
using System;
using System.Collections.Generic;

namespace GameFramework.Config
{
    /// <summary>
    /// 配置管理器。
    /// </summary>
    internal sealed partial class ConfigManager : GameFrameworkModule, IConfigManager
    {
        private readonly Dictionary<string, ConfigData> m_ConfigDatas;
        private readonly LoadAssetCallbacks m_LoadAssetCallbacks;
        private IResourceManager m_ResourceManager;
        private IConfigHelper m_ConfigHelper;
        private EventHandler<LoadConfigSuccessEventArgs> m_LoadConfigSuccessEventHandler;
        private EventHandler<LoadConfigFailureEventArgs> m_LoadConfigFailureEventHandler;
        private EventHandler<LoadConfigUpdateEventArgs> m_LoadConfigUpdateEventHandler;
        private EventHandler<LoadConfigDependencyAssetEventArgs> m_LoadConfigDependencyAssetEventHandler;

        /// <summary>
        /// 初始化配置管理器的新实例。
        /// </summary>
        public ConfigManager()
        {
            m_ConfigDatas = new Dictionary<string, ConfigData>();
            m_LoadAssetCallbacks = new LoadAssetCallbacks(LoadConfigSuccessCallback, LoadConfigFailureCallback, LoadConfigUpdateCallback, LoadConfigDependencyAssetCallback);
            m_ResourceManager = null;
            m_ConfigHelper = null;
            m_LoadConfigSuccessEventHandler = null;
            m_LoadConfigFailureEventHandler = null;
            m_LoadConfigUpdateEventHandler = null;
            m_LoadConfigDependencyAssetEventHandler = null;
        }

        /// <summary>
        /// 获取配置数量。
        /// </summary>
        public int ConfigCount
        {
            get
            {
                return m_ConfigDatas.Count;
            }
        }

        /// <summary>
        /// 加载配置成功事件。
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
        /// 加载配置失败事件。
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
        /// 加载配置更新事件。
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
        /// 加载配置时加载依赖资源事件。
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
        /// 配置管理器轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {

        }

        /// <summary>
        /// 关闭并清理配置管理器。
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
        /// 设置配置辅助器。
        /// </summary>
        /// <param name="configHelper">配置辅助器。</param>
        public void SetConfigHelper(IConfigHelper configHelper)
        {
            if (configHelper == null)
            {
                throw new GameFrameworkException("Config helper is invalid.");
            }

            m_ConfigHelper = configHelper;
        }

        /// <summary>
        /// 加载配置。
        /// </summary>
        /// <param name="configAssetName">配置资源名称。</param>
        public void LoadConfig(string configAssetName)
        {
            LoadConfig(configAssetName, Constant.DefaultPriority, null);
        }

        /// <summary>
        /// 加载配置。
        /// </summary>
        /// <param name="configAssetName">配置资源名称。</param>
        /// <param name="priority">加载配置资源的优先级。</param>
        public void LoadConfig(string configAssetName, int priority)
        {
            LoadConfig(configAssetName, priority, null);
        }

        /// <summary>
        /// 加载配置。
        /// </summary>
        /// <param name="configAssetName">配置资源名称。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void LoadConfig(string configAssetName, object userData)
        {
            LoadConfig(configAssetName, Constant.DefaultPriority, userData);
        }

        /// <summary>
        /// 加载配置。
        /// </summary>
        /// <param name="configAssetName">配置资源名称。</param>
        /// <param name="priority">加载配置资源的优先级。</param>
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

            m_ResourceManager.LoadAsset(configAssetName, priority, m_LoadAssetCallbacks, userData);
        }

        /// <summary>
        /// 解析配置。
        /// </summary>
        /// <param name="text">要解析的配置文本。</param>
        /// <returns>是否解析配置成功。</returns>
        public bool ParseConfig(string text)
        {
            return ParseConfig(text, null);
        }

        /// <summary>
        /// 解析配置。
        /// </summary>
        /// <param name="text">要解析的配置文本。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否解析配置成功。</returns>
        public bool ParseConfig(string text, object userData)
        {
            if (m_ConfigHelper == null)
            {
                throw new GameFrameworkException("You must set config helper first.");
            }

            return m_ConfigHelper.ParseConfig(text, userData);
        }

        /// <summary>
        /// 检查是否存在指定配置项。
        /// </summary>
        /// <param name="configName">要检查配置项的名称。</param>
        /// <returns>指定的配置项是否存在。</returns>
        public bool HasConfig(string configName)
        {
            return GetConfigData(configName).HasValue;
        }

        /// <summary>
        /// 增加指定配置项。
        /// </summary>
        /// <param name="configName">要增加配置项的名称。</param>
        /// <param name="boolValue">配置项布尔值。</param>
        /// <param name="intValue">配置项整数值。</param>
        /// <param name="floatValue">配置项浮点数值。</param>
        /// <param name="stringValue">配置项字符串值。</param>
        /// <returns>是否增加配置项成功。</returns>
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
        /// 移除指定配置项。
        /// </summary>
        /// <param name="configName">要移除配置项的名称。</param>
        public void RemoveConfig(string configName)
        {
            m_ConfigDatas.Remove(configName);
        }

        /// <summary>
        /// 清空所有配置项。
        /// </summary>
        public void RemoveAllConfigs()
        {
            m_ConfigDatas.Clear();
        }

        /// <summary>
        /// 从指定配置项中读取布尔值。
        /// </summary>
        /// <param name="configName">要获取配置项的名称。</param>
        /// <returns>读取的布尔值。</returns>
        public bool GetBool(string configName)
        {
            ConfigData? configData = GetConfigData(configName);
            if (!configData.HasValue)
            {
                throw new GameFrameworkException(string.Format("Config name '{0}' is not exist.", configName));
            }

            return configData.Value.BoolValue;
        }

        /// <summary>
        /// 从指定配置项中读取布尔值。
        /// </summary>
        /// <param name="configName">要获取配置项的名称。</param>
        /// <param name="defaultValue">当指定的配置项不存在时，返回此默认值。</param>
        /// <returns>读取的布尔值。</returns>
        public bool GetBool(string configName, bool defaultValue)
        {
            ConfigData? configData = GetConfigData(configName);
            return configData.HasValue ? configData.Value.BoolValue : defaultValue;
        }

        /// <summary>
        /// 从指定配置项中读取整数值。
        /// </summary>
        /// <param name="configName">要获取配置项的名称。</param>
        /// <returns>读取的整数值。</returns>
        public int GetInt(string configName)
        {
            ConfigData? configData = GetConfigData(configName);
            if (!configData.HasValue)
            {
                throw new GameFrameworkException(string.Format("Config name '{0}' is not exist.", configName));
            }

            return configData.Value.IntValue;
        }

        /// <summary>
        /// 从指定配置项中读取整数值。
        /// </summary>
        /// <param name="configName">要获取配置项的名称。</param>
        /// <param name="defaultValue">当指定的配置项不存在时，返回此默认值。</param>
        /// <returns>读取的整数值。</returns>
        public int GetInt(string configName, int defaultValue)
        {
            ConfigData? configData = GetConfigData(configName);
            return configData.HasValue ? configData.Value.IntValue : defaultValue;
        }

        /// <summary>
        /// 从指定配置项中读取浮点数值。
        /// </summary>
        /// <param name="configName">要获取配置项的名称。</param>
        /// <returns>读取的浮点数值。</returns>
        public float GetFloat(string configName)
        {
            ConfigData? configData = GetConfigData(configName);
            if (!configData.HasValue)
            {
                throw new GameFrameworkException(string.Format("Config name '{0}' is not exist.", configName));
            }

            return configData.Value.FloatValue;
        }

        /// <summary>
        /// 从指定配置项中读取浮点数值。
        /// </summary>
        /// <param name="configName">要获取配置项的名称。</param>
        /// <param name="defaultValue">当指定的配置项不存在时，返回此默认值。</param>
        /// <returns>读取的浮点数值。</returns>
        public float GetFloat(string configName, float defaultValue)
        {
            ConfigData? configData = GetConfigData(configName);
            return configData.HasValue ? configData.Value.FloatValue : defaultValue;
        }

        /// <summary>
        /// 从指定配置项中读取字符串值。
        /// </summary>
        /// <param name="configName">要获取配置项的名称。</param>
        /// <returns>读取的字符串值。</returns>
        public string GetString(string configName)
        {
            ConfigData? configData = GetConfigData(configName);
            if (!configData.HasValue)
            {
                throw new GameFrameworkException(string.Format("Config name '{0}' is not exist.", configName));
            }

            return configData.Value.StringValue;
        }

        /// <summary>
        /// 从指定配置项中读取字符串值。
        /// </summary>
        /// <param name="configName">要获取配置项的名称。</param>
        /// <param name="defaultValue">当指定的配置项不存在时，返回此默认值。</param>
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

        private void LoadConfigSuccessCallback(string configAssetName, object configAsset, float duration, object userData)
        {
            try
            {
                if (!m_ConfigHelper.LoadConfig(configAsset, userData))
                {
                    throw new GameFrameworkException(string.Format("Load config failure in helper, asset name '{0}'.", configAssetName));
                }
            }
            catch (Exception exception)
            {
                if (m_LoadConfigFailureEventHandler != null)
                {
                    m_LoadConfigFailureEventHandler(this, new LoadConfigFailureEventArgs(configAssetName, exception.ToString(), userData));
                    return;
                }

                throw;
            }
            finally
            {
                m_ConfigHelper.ReleaseConfigAsset(configAsset);
            }

            if (m_LoadConfigSuccessEventHandler != null)
            {
                m_LoadConfigSuccessEventHandler(this, new LoadConfigSuccessEventArgs(configAssetName, duration, userData));
            }
        }

        private void LoadConfigFailureCallback(string configAssetName, LoadResourceStatus status, string errorMessage, object userData)
        {
            string appendErrorMessage = string.Format("Load config failure, asset name '{0}', status '{1}', error message '{2}'.", configAssetName, status.ToString(), errorMessage);
            if (m_LoadConfigFailureEventHandler != null)
            {
                m_LoadConfigFailureEventHandler(this, new LoadConfigFailureEventArgs(configAssetName, appendErrorMessage, userData));
                return;
            }

            throw new GameFrameworkException(appendErrorMessage);
        }

        private void LoadConfigUpdateCallback(string configAssetName, float progress, object userData)
        {
            if (m_LoadConfigUpdateEventHandler != null)
            {
                m_LoadConfigUpdateEventHandler(this, new LoadConfigUpdateEventArgs(configAssetName, progress, userData));
            }
        }

        private void LoadConfigDependencyAssetCallback(string configAssetName, string dependencyAssetName, int loadedCount, int totalCount, object userData)
        {
            if (m_LoadConfigDependencyAssetEventHandler != null)
            {
                m_LoadConfigDependencyAssetEventHandler(this, new LoadConfigDependencyAssetEventArgs(configAssetName, dependencyAssetName, loadedCount, totalCount, userData));
            }
        }
    }
}
