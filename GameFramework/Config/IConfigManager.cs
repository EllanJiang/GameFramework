//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.Resource;
using System;

namespace GameFramework.Config
{
    /// <summary>
    /// 配置管理器接口。
    /// </summary>
    public interface IConfigManager
    {
        /// <summary>
        /// 获取配置数量。
        /// </summary>
        int ConfigCount
        {
            get;
        }

        /// <summary>
        /// 加载配置成功事件。
        /// </summary>
        event EventHandler<LoadConfigSuccessEventArgs> LoadConfigSuccess;

        /// <summary>
        /// 加载配置失败事件。
        /// </summary>
        event EventHandler<LoadConfigFailureEventArgs> LoadConfigFailure;

        /// <summary>
        /// 加载配置更新事件。
        /// </summary>
        event EventHandler<LoadConfigUpdateEventArgs> LoadConfigUpdate;

        /// <summary>
        /// 加载配置时加载依赖资源事件。
        /// </summary>
        event EventHandler<LoadConfigDependencyAssetEventArgs> LoadConfigDependencyAsset;

        /// <summary>
        /// 设置资源管理器。
        /// </summary>
        /// <param name="resourceManager">资源管理器。</param>
        void SetResourceManager(IResourceManager resourceManager);

        /// <summary>
        /// 设置配置辅助器。
        /// </summary>
        /// <param name="configHelper">配置辅助器。</param>
        void SetConfigHelper(IConfigHelper configHelper);

        /// <summary>
        /// 加载配置。
        /// </summary>
        /// <param name="configAssetName">配置资源名称。</param>
        void LoadConfig(string configAssetName);

        /// <summary>
        /// 加载配置。
        /// </summary>
        /// <param name="configAssetName">配置资源名称。</param>
        /// <param name="priority">加载配置资源的优先级。</param>
        void LoadConfig(string configAssetName, int priority);

        /// <summary>
        /// 加载配置。
        /// </summary>
        /// <param name="configAssetName">配置资源名称。</param>
        /// <param name="userData">用户自定义数据。</param>
        void LoadConfig(string configAssetName, object userData);

        /// <summary>
        /// 加载配置。
        /// </summary>
        /// <param name="configAssetName">配置资源名称。</param>
        /// <param name="priority">加载配置资源的优先级。</param>
        /// <param name="userData">用户自定义数据。</param>
        void LoadConfig(string configAssetName, int priority, object userData);

        /// <summary>
        /// 解析配置。
        /// </summary>
        /// <param name="text">要解析的配置文本。</param>
        /// <returns>是否解析配置成功。</returns>
        bool ParseConfig(string text);

        /// <summary>
        /// 解析配置。
        /// </summary>
        /// <param name="text">要解析的配置文本。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否解析配置成功。</returns>
        bool ParseConfig(string text, object userData);

        /// <summary>
        /// 检查是否存在指定配置项。
        /// </summary>
        /// <param name="configName">要检查配置项的名称。</param>
        /// <returns>指定的配置项是否存在。</returns>
        bool HasConfig(string configName);

        /// <summary>
        /// 增加指定配置项。
        /// </summary>
        /// <param name="configName">要增加配置项的名称。</param>
        /// <param name="boolValue">配置项布尔值。</param>
        /// <param name="intValue">配置项整数值。</param>
        /// <param name="floatValue">配置项浮点数值。</param>
        /// <param name="stringValue">配置项字符串值。</param>
        /// <returns>是否增加配置项成功。</returns>
        bool AddConfig(string configName, bool boolValue, int intValue, float floatValue, string stringValue);

        /// <summary>
        /// 移除指定配置项。
        /// </summary>
        /// <param name="configName">要移除配置项的名称。</param>
        void RemoveConfig(string configName);

        /// <summary>
        /// 清空所有配置项。
        /// </summary>
        void RemoveAllConfigs();

        /// <summary>
        /// 从指定配置项中读取布尔值。
        /// </summary>
        /// <param name="configName">要获取配置项的名称。</param>
        /// <returns>读取的布尔值。</returns>
        bool GetBool(string configName);

        /// <summary>
        /// 从指定配置项中读取布尔值。
        /// </summary>
        /// <param name="configName">要获取配置项的名称。</param>
        /// <param name="defaultValue">当指定的配置项不存在时，返回此默认值。</param>
        /// <returns>读取的布尔值。</returns>
        bool GetBool(string configName, bool defaultValue);

        /// <summary>
        /// 从指定配置项中读取整数值。
        /// </summary>
        /// <param name="configName">要获取配置项的名称。</param>
        /// <returns>读取的整数值。</returns>
        int GetInt(string configName);

        /// <summary>
        /// 从指定配置项中读取整数值。
        /// </summary>
        /// <param name="configName">要获取配置项的名称。</param>
        /// <param name="defaultValue">当指定的配置项不存在时，返回此默认值。</param>
        /// <returns>读取的整数值。</returns>
        int GetInt(string configName, int defaultValue);

        /// <summary>
        /// 从指定配置项中读取浮点数值。
        /// </summary>
        /// <param name="configName">要获取配置项的名称。</param>
        /// <returns>读取的浮点数值。</returns>
        float GetFloat(string configName);

        /// <summary>
        /// 从指定配置项中读取浮点数值。
        /// </summary>
        /// <param name="configName">要获取配置项的名称。</param>
        /// <param name="defaultValue">当指定的配置项不存在时，返回此默认值。</param>
        /// <returns>读取的浮点数值。</returns>
        float GetFloat(string configName, float defaultValue);

        /// <summary>
        /// 从指定配置项中读取字符串值。
        /// </summary>
        /// <param name="configName">要获取配置项的名称。</param>
        /// <returns>读取的字符串值。</returns>
        string GetString(string configName);

        /// <summary>
        /// 从指定配置项中读取字符串值。
        /// </summary>
        /// <param name="configName">要获取配置项的名称。</param>
        /// <param name="defaultValue">当指定的配置项不存在时，返回此默认值。</param>
        /// <returns>读取的字符串值。</returns>
        string GetString(string configName, string defaultValue);
    }
}
