//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework.Resource;
using System;

namespace GameFramework.Config
{
    /// <summary>
    /// 全局配置管理器接口。
    /// </summary>
    public interface IConfigManager
    {
        /// <summary>
        /// 获取全局配置项数量。
        /// </summary>
        int Count
        {
            get;
        }

        /// <summary>
        /// 加载全局配置成功事件。
        /// </summary>
        event EventHandler<LoadConfigSuccessEventArgs> LoadConfigSuccess;

        /// <summary>
        /// 加载全局配置失败事件。
        /// </summary>
        event EventHandler<LoadConfigFailureEventArgs> LoadConfigFailure;

        /// <summary>
        /// 加载全局配置更新事件。
        /// </summary>
        event EventHandler<LoadConfigUpdateEventArgs> LoadConfigUpdate;

        /// <summary>
        /// 加载全局配置时加载依赖资源事件。
        /// </summary>
        event EventHandler<LoadConfigDependencyAssetEventArgs> LoadConfigDependencyAsset;

        /// <summary>
        /// 设置资源管理器。
        /// </summary>
        /// <param name="resourceManager">资源管理器。</param>
        void SetResourceManager(IResourceManager resourceManager);

        /// <summary>
        /// 设置全局配置辅助器。
        /// </summary>
        /// <param name="configHelper">全局配置辅助器。</param>
        void SetConfigHelper(IConfigHelper configHelper);

        /// <summary>
        /// 加载全局配置。
        /// </summary>
        /// <param name="configAssetName">全局配置资源名称。</param>
        void LoadConfig(string configAssetName);

        /// <summary>
        /// 加载全局配置。
        /// </summary>
        /// <param name="configAssetName">全局配置资源名称。</param>
        /// <param name="priority">加载全局配置资源的优先级。</param>
        void LoadConfig(string configAssetName, int priority);

        /// <summary>
        /// 加载全局配置。
        /// </summary>
        /// <param name="configAssetName">全局配置资源名称。</param>
        /// <param name="userData">用户自定义数据。</param>
        void LoadConfig(string configAssetName, object userData);

        /// <summary>
        /// 加载全局配置。
        /// </summary>
        /// <param name="configAssetName">全局配置资源名称。</param>
        /// <param name="priority">加载全局配置资源的优先级。</param>
        /// <param name="userData">用户自定义数据。</param>
        void LoadConfig(string configAssetName, int priority, object userData);

        /// <summary>
        /// 解析全局配置。
        /// </summary>
        /// <param name="configData">要解析的全局配置数据。</param>
        /// <returns>是否解析全局配置成功。</returns>
        bool ParseConfig(object configData);

        /// <summary>
        /// 解析全局配置。
        /// </summary>
        /// <param name="configData">要解析的全局配置数据。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否解析全局配置成功。</returns>
        bool ParseConfig(object configData, object userData);

        /// <summary>
        /// 检查是否存在指定全局配置项。
        /// </summary>
        /// <param name="configName">要检查全局配置项的名称。</param>
        /// <returns>指定的全局配置项是否存在。</returns>
        bool HasConfig(string configName);

        /// <summary>
        /// 增加指定全局配置项。
        /// </summary>
        /// <param name="configName">要增加全局配置项的名称。</param>
        /// <param name="boolValue">全局配置项布尔值。</param>
        /// <param name="intValue">全局配置项整数值。</param>
        /// <param name="floatValue">全局配置项浮点数值。</param>
        /// <param name="stringValue">全局配置项字符串值。</param>
        /// <returns>是否增加全局配置项成功。</returns>
        bool AddConfig(string configName, bool boolValue, int intValue, float floatValue, string stringValue);

        /// <summary>
        /// 移除指定全局配置项。
        /// </summary>
        /// <param name="configName">要移除全局配置项的名称。</param>
        /// <returns>是否移除全局配置项成功。</returns>
        bool RemoveConfig(string configName);

        /// <summary>
        /// 清空所有全局配置项。
        /// </summary>
        void RemoveAllConfigs();

        /// <summary>
        /// 从指定全局配置项中读取布尔值。
        /// </summary>
        /// <param name="configName">要获取全局配置项的名称。</param>
        /// <returns>读取的布尔值。</returns>
        bool GetBool(string configName);

        /// <summary>
        /// 从指定全局配置项中读取布尔值。
        /// </summary>
        /// <param name="configName">要获取全局配置项的名称。</param>
        /// <param name="defaultValue">当指定的全局配置项不存在时，返回此默认值。</param>
        /// <returns>读取的布尔值。</returns>
        bool GetBool(string configName, bool defaultValue);

        /// <summary>
        /// 从指定全局配置项中读取整数值。
        /// </summary>
        /// <param name="configName">要获取全局配置项的名称。</param>
        /// <returns>读取的整数值。</returns>
        int GetInt(string configName);

        /// <summary>
        /// 从指定全局配置项中读取整数值。
        /// </summary>
        /// <param name="configName">要获取全局配置项的名称。</param>
        /// <param name="defaultValue">当指定的全局配置项不存在时，返回此默认值。</param>
        /// <returns>读取的整数值。</returns>
        int GetInt(string configName, int defaultValue);

        /// <summary>
        /// 从指定全局配置项中读取浮点数值。
        /// </summary>
        /// <param name="configName">要获取全局配置项的名称。</param>
        /// <returns>读取的浮点数值。</returns>
        float GetFloat(string configName);

        /// <summary>
        /// 从指定全局配置项中读取浮点数值。
        /// </summary>
        /// <param name="configName">要获取全局配置项的名称。</param>
        /// <param name="defaultValue">当指定的全局配置项不存在时，返回此默认值。</param>
        /// <returns>读取的浮点数值。</returns>
        float GetFloat(string configName, float defaultValue);

        /// <summary>
        /// 从指定全局配置项中读取字符串值。
        /// </summary>
        /// <param name="configName">要获取全局配置项的名称。</param>
        /// <returns>读取的字符串值。</returns>
        string GetString(string configName);

        /// <summary>
        /// 从指定全局配置项中读取字符串值。
        /// </summary>
        /// <param name="configName">要获取全局配置项的名称。</param>
        /// <param name="defaultValue">当指定的全局配置项不存在时，返回此默认值。</param>
        /// <returns>读取的字符串值。</returns>
        string GetString(string configName, string defaultValue);
    }
}
