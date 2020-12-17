//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework.Resource;

namespace GameFramework.Localization
{
    /// <summary>
    /// 本地化管理器接口。
    /// </summary>
    public interface ILocalizationManager : IDataProvider<ILocalizationManager>
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
        /// 获取缓冲二进制流的大小。
        /// </summary>
        int CachedBytesSize
        {
            get;
        }

        /// <summary>
        /// 设置资源管理器。
        /// </summary>
        /// <param name="resourceManager">资源管理器。</param>
        void SetResourceManager(IResourceManager resourceManager);

        /// <summary>
        /// 设置本地化数据提供者辅助器。
        /// </summary>
        /// <param name="dataProviderHelper">本地化数据提供者辅助器。</param>
        void SetDataProviderHelper(IDataProviderHelper<ILocalizationManager> dataProviderHelper);

        /// <summary>
        /// 设置本地化辅助器。
        /// </summary>
        /// <param name="localizationHelper">本地化辅助器。</param>
        void SetLocalizationHelper(ILocalizationHelper localizationHelper);

        /// <summary>
        /// 确保二进制流缓存分配足够大小的内存并缓存。
        /// </summary>
        /// <param name="ensureSize">要确保二进制流缓存分配内存的大小。</param>
        void EnsureCachedBytesSize(int ensureSize);

        /// <summary>
        /// 释放缓存的二进制流。
        /// </summary>
        void FreeCachedBytes();

        /// <summary>
        /// 根据字典主键获取字典内容字符串。
        /// </summary>
        /// <param name="key">字典主键。</param>
        /// <returns>要获取的字典内容字符串。</returns>
        string GetString(string key);

        /// <summary>
        /// 根据字典主键获取字典内容字符串。
        /// </summary>
        /// <param name="key">字典主键。</param>
        /// <param name="arg0">字典参数 0。</param>
        /// <returns>要获取的字典内容字符串。</returns>
        string GetString(string key, object arg0);

        /// <summary>
        /// 根据字典主键获取字典内容字符串。
        /// </summary>
        /// <param name="key">字典主键。</param>
        /// <param name="arg0">字典参数 0。</param>
        /// <param name="arg1">字典参数 1。</param>
        /// <returns>要获取的字典内容字符串。</returns>
        string GetString(string key, object arg0, object arg1);

        /// <summary>
        /// 根据字典主键获取字典内容字符串。
        /// </summary>
        /// <param name="key">字典主键。</param>
        /// <param name="arg0">字典参数 0。</param>
        /// <param name="arg1">字典参数 1。</param>
        /// <param name="arg2">字典参数 2。</param>
        /// <returns>要获取的字典内容字符串。</returns>
        string GetString(string key, object arg0, object arg1, object arg2);

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

        /// <summary>
        /// 清空所有字典。
        /// </summary>
        void RemoveAllRawStrings();
    }
}
