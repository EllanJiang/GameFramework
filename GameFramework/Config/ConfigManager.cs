//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
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
        private readonly DataProvider<IConfigManager> m_DataProvider;
        private IConfigHelper m_ConfigHelper;

        /// <summary>
        /// 初始化全局配置管理器的新实例。
        /// </summary>
        public ConfigManager()
        {
            m_ConfigDatas = new Dictionary<string, ConfigData>(StringComparer.Ordinal);
            m_DataProvider = new DataProvider<IConfigManager>(this);
            m_ConfigHelper = null;
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
        /// 获取缓冲二进制流的大小。
        /// </summary>
        public int CachedBytesSize
        {
            get
            {
                return DataProvider<IConfigManager>.CachedBytesSize;
            }
        }

        /// <summary>
        /// 读取全局配置成功事件。
        /// </summary>
        public event EventHandler<ReadDataSuccessEventArgs> ReadDataSuccess
        {
            add
            {
                m_DataProvider.ReadDataSuccess += value;
            }
            remove
            {
                m_DataProvider.ReadDataSuccess -= value;
            }
        }

        /// <summary>
        /// 读取全局配置失败事件。
        /// </summary>
        public event EventHandler<ReadDataFailureEventArgs> ReadDataFailure
        {
            add
            {
                m_DataProvider.ReadDataFailure += value;
            }
            remove
            {
                m_DataProvider.ReadDataFailure -= value;
            }
        }

        /// <summary>
        /// 读取全局配置更新事件。
        /// </summary>
        public event EventHandler<ReadDataUpdateEventArgs> ReadDataUpdate
        {
            add
            {
                m_DataProvider.ReadDataUpdate += value;
            }
            remove
            {
                m_DataProvider.ReadDataUpdate -= value;
            }
        }

        /// <summary>
        /// 读取全局配置时加载依赖资源事件。
        /// </summary>
        public event EventHandler<ReadDataDependencyAssetEventArgs> ReadDataDependencyAsset
        {
            add
            {
                m_DataProvider.ReadDataDependencyAsset += value;
            }
            remove
            {
                m_DataProvider.ReadDataDependencyAsset -= value;
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
            m_DataProvider.SetResourceManager(resourceManager);
        }

        /// <summary>
        /// 设置全局配置数据提供者辅助器。
        /// </summary>
        /// <param name="dataProviderHelper">全局配置数据提供者辅助器。</param>
        public void SetDataProviderHelper(IDataProviderHelper<IConfigManager> dataProviderHelper)
        {
            m_DataProvider.SetDataProviderHelper(dataProviderHelper);
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
        /// 确保二进制流缓存分配足够大小的内存并缓存。
        /// </summary>
        /// <param name="ensureSize">要确保二进制流缓存分配内存的大小。</param>
        public void EnsureCachedBytesSize(int ensureSize)
        {
            DataProvider<IConfigManager>.EnsureCachedBytesSize(ensureSize);
        }

        /// <summary>
        /// 释放缓存的二进制流。
        /// </summary>
        public void FreeCachedBytes()
        {
            DataProvider<IConfigManager>.FreeCachedBytes();
        }

        /// <summary>
        /// 读取全局配置。
        /// </summary>
        /// <param name="configAssetName">全局配置资源名称。</param>
        public void ReadData(string configAssetName)
        {
            m_DataProvider.ReadData(configAssetName);
        }

        /// <summary>
        /// 读取全局配置。
        /// </summary>
        /// <param name="configAssetName">全局配置资源名称。</param>
        /// <param name="priority">加载全局配置资源的优先级。</param>
        public void ReadData(string configAssetName, int priority)
        {
            m_DataProvider.ReadData(configAssetName, priority);
        }

        /// <summary>
        /// 读取全局配置。
        /// </summary>
        /// <param name="configAssetName">全局配置资源名称。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void ReadData(string configAssetName, object userData)
        {
            m_DataProvider.ReadData(configAssetName, userData);
        }

        /// <summary>
        /// 读取全局配置。
        /// </summary>
        /// <param name="configAssetName">全局配置资源名称。</param>
        /// <param name="priority">加载全局配置资源的优先级。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void ReadData(string configAssetName, int priority, object userData)
        {
            m_DataProvider.ReadData(configAssetName, priority, userData);
        }

        /// <summary>
        /// 解析全局配置。
        /// </summary>
        /// <param name="configString">要解析的全局配置字符串。</param>
        /// <returns>是否解析全局配置成功。</returns>
        public bool ParseData(string configString)
        {
            return m_DataProvider.ParseData(configString);
        }

        /// <summary>
        /// 解析全局配置。
        /// </summary>
        /// <param name="configString">要解析的全局配置字符串。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否解析全局配置成功。</returns>
        public bool ParseData(string configString, object userData)
        {
            return m_DataProvider.ParseData(configString, userData);
        }

        /// <summary>
        /// 解析全局配置。
        /// </summary>
        /// <param name="configBytes">要解析的全局配置二进制流。</param>
        /// <returns>是否解析全局配置成功。</returns>
        public bool ParseData(byte[] configBytes)
        {
            return m_DataProvider.ParseData(configBytes);
        }

        /// <summary>
        /// 解析全局配置。
        /// </summary>
        /// <param name="configBytes">要解析的全局配置二进制流。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否解析全局配置成功。</returns>
        public bool ParseData(byte[] configBytes, object userData)
        {
            return m_DataProvider.ParseData(configBytes, userData);
        }

        /// <summary>
        /// 解析全局配置。
        /// </summary>
        /// <param name="configBytes">要解析的全局配置二进制流。</param>
        /// <param name="startIndex">全局配置二进制流的起始位置。</param>
        /// <param name="length">全局配置二进制流的长度。</param>
        /// <returns>是否解析全局配置成功。</returns>
        public bool ParseData(byte[] configBytes, int startIndex, int length)
        {
            return m_DataProvider.ParseData(configBytes, startIndex, length);
        }

        /// <summary>
        /// 解析全局配置。
        /// </summary>
        /// <param name="configBytes">要解析的全局配置二进制流。</param>
        /// <param name="startIndex">全局配置二进制流的起始位置。</param>
        /// <param name="length">全局配置二进制流的长度。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否解析全局配置成功。</returns>
        public bool ParseData(byte[] configBytes, int startIndex, int length, object userData)
        {
            return m_DataProvider.ParseData(configBytes, startIndex, length, userData);
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

        /// <summary>
        /// 增加指定全局配置项。
        /// </summary>
        /// <param name="configName">要增加全局配置项的名称。</param>
        /// <param name="configValue">全局配置项的值。</param>
        /// <returns>是否增加全局配置项成功。</returns>
        public bool AddConfig(string configName, string configValue)
        {
            bool boolValue = false;
            bool.TryParse(configValue, out boolValue);

            int intValue = 0;
            int.TryParse(configValue, out intValue);

            float floatValue = 0f;
            float.TryParse(configValue, out floatValue);

            return AddConfig(configName, boolValue, intValue, floatValue, configValue);
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
    }
}
