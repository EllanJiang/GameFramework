//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.Resource;
using System;

namespace GameFramework.Localization
{
    /// <summary>
    /// 本地化管理器接口。
    /// </summary>
    public interface ILocalizationManager
    {
        /// <summary>
        /// 获取或设置本地化语言。
        /// </summary>
        Language Language
        {
            get;
            set;
        }

        /// <summary>
        /// 获取系统语言。
        /// </summary>
        Language SystemLanguage
        {
            get;
        }

        /// <summary>
        /// 获取字典数量。
        /// </summary>
        int DictionaryCount
        {
            get;
        }

        /// <summary>
        /// 加载字典成功事件。
        /// </summary>
        event EventHandler<LoadDictionarySuccessEventArgs> LoadDictionarySuccess;

        /// <summary>
        /// 加载字典失败事件。
        /// </summary>
        event EventHandler<LoadDictionaryFailureEventArgs> LoadDictionaryFailure;

        /// <summary>
        /// 加载字典更新事件。
        /// </summary>
        event EventHandler<LoadDictionaryUpdateEventArgs> LoadDictionaryUpdate;

        /// <summary>
        /// 加载字典时加载依赖资源事件。
        /// </summary>
        event EventHandler<LoadDictionaryDependencyAssetEventArgs> LoadDictionaryDependencyAsset;

        /// <summary>
        /// 设置资源管理器。
        /// </summary>
        /// <param name="resourceManager">资源管理器。</param>
        void SetResourceManager(IResourceManager resourceManager);

        /// <summary>
        /// 设置本地化辅助器。
        /// </summary>
        /// <param name="localizationHelper">本地化辅助器。</param>
        void SetLocalizationHelper(ILocalizationHelper localizationHelper);

        /// <summary>
        /// 加载字典。
        /// </summary>
        /// <param name="dictionaryAssetName">字典资源名称。</param>
        void LoadDictionary(string dictionaryAssetName);

        /// <summary>
        /// 加载字典。
        /// </summary>
        /// <param name="dictionaryAssetName">字典资源名称。</param>
        /// <param name="priority">加载字典资源的优先级。</param>
        void LoadDictionary(string dictionaryAssetName, int priority);

        /// <summary>
        /// 加载字典。
        /// </summary>
        /// <param name="dictionaryAssetName">字典资源名称。</param>
        /// <param name="userData">用户自定义数据。</param>
        void LoadDictionary(string dictionaryAssetName, object userData);

        /// <summary>
        /// 加载字典。
        /// </summary>
        /// <param name="dictionaryAssetName">字典资源名称。</param>
        /// <param name="priority">加载字典资源的优先级。</param>
        /// <param name="userData">用户自定义数据。</param>
        void LoadDictionary(string dictionaryAssetName, int priority, object userData);

        /// <summary>
        /// 解析字典。
        /// </summary>
        /// <param name="text">要解析的字典文本。</param>
        /// <returns>是否解析字典成功。</returns>
        bool ParseDictionary(string text);

        /// <summary>
        /// 解析字典。
        /// </summary>
        /// <param name="text">要解析的字典文本。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否解析字典成功。</returns>
        bool ParseDictionary(string text, object userData);

        /// <summary>
        /// 根据字典主键获取字典内容字符串。
        /// </summary>
        /// <param name="key">字典主键。</param>
        /// <param name="args">字典参数。</param>
        /// <returns>要获取的字典内容字符串。</returns>
        string GetString(string key, params object[] args);

        /// <summary>
        /// 是否存在字典。
        /// </summary>
        /// <param name="key">字典主键。</param>
        /// <returns>是否存在字典。</returns>
        bool HasRawString(string key);

        /// <summary>
        /// 根据字典主键获取字典值。
        /// </summary>
        /// <param name="key">字典主键。</param>
        /// <returns>字典值。</returns>
        string GetRawString(string key);

        /// <summary>
        /// 增加字典。
        /// </summary>
        /// <param name="key">字典主键。</param>
        /// <param name="value">字典内容。</param>
        /// <returns>是否增加字典成功。</returns>
        bool AddRawString(string key, string value);

        /// <summary>
        /// 移除字典。
        /// </summary>
        /// <param name="key">字典主键。</param>
        /// <returns>是否移除字典成功。</returns>
        bool RemoveRawString(string key);
    }
}
