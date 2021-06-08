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
        /// <typeparam name="T">字典参数的类型。</typeparam>
        /// <param name="key">字典主键。</param>
        /// <param name="arg">字典参数。</param>
        /// <returns>要获取的字典内容字符串。</returns>
        string GetString<T>(string key, T arg);

        /// <summary>
        /// 根据字典主键获取字典内容字符串。
        /// </summary>
        /// <typeparam name="T1">字典参数 1 的类型。</typeparam>
        /// <typeparam name="T2">字典参数 2 的类型。</typeparam>
        /// <param name="key">字典主键。</param>
        /// <param name="arg1">字典参数 1。</param>
        /// <param name="arg2">字典参数 2。</param>
        /// <returns>要获取的字典内容字符串。</returns>
        string GetString<T1, T2>(string key, T1 arg1, T2 arg2);

        /// <summary>
        /// 根据字典主键获取字典内容字符串。
        /// </summary>
        /// <typeparam name="T1">字典参数 1 的类型。</typeparam>
        /// <typeparam name="T2">字典参数 2 的类型。</typeparam>
        /// <typeparam name="T3">字典参数 3 的类型。</typeparam>
        /// <param name="key">字典主键。</param>
        /// <param name="arg1">字典参数 1。</param>
        /// <param name="arg2">字典参数 2。</param>
        /// <param name="arg3">字典参数 3。</param>
        /// <returns>要获取的字典内容字符串。</returns>
        string GetString<T1, T2, T3>(string key, T1 arg1, T2 arg2, T3 arg3);

        /// <summary>
        /// 根据字典主键获取字典内容字符串。
        /// </summary>
        /// <typeparam name="T1">字典参数 1 的类型。</typeparam>
        /// <typeparam name="T2">字典参数 2 的类型。</typeparam>
        /// <typeparam name="T3">字典参数 3 的类型。</typeparam>
        /// <typeparam name="T4">字典参数 4 的类型。</typeparam>
        /// <param name="key">字典主键。</param>
        /// <param name="arg1">字典参数 1。</param>
        /// <param name="arg2">字典参数 2。</param>
        /// <param name="arg3">字典参数 3。</param>
        /// <param name="arg4">字典参数 4。</param>
        /// <returns>要获取的字典内容字符串。</returns>
        string GetString<T1, T2, T3, T4>(string key, T1 arg1, T2 arg2, T3 arg3, T4 arg4);

        /// <summary>
        /// 根据字典主键获取字典内容字符串。
        /// </summary>
        /// <typeparam name="T1">字典参数 1 的类型。</typeparam>
        /// <typeparam name="T2">字典参数 2 的类型。</typeparam>
        /// <typeparam name="T3">字典参数 3 的类型。</typeparam>
        /// <typeparam name="T4">字典参数 4 的类型。</typeparam>
        /// <typeparam name="T5">字典参数 5 的类型。</typeparam>
        /// <param name="key">字典主键。</param>
        /// <param name="arg1">字典参数 1。</param>
        /// <param name="arg2">字典参数 2。</param>
        /// <param name="arg3">字典参数 3。</param>
        /// <param name="arg4">字典参数 4。</param>
        /// <param name="arg5">字典参数 5。</param>
        /// <returns>要获取的字典内容字符串。</returns>
        string GetString<T1, T2, T3, T4, T5>(string key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);

        /// <summary>
        /// 根据字典主键获取字典内容字符串。
        /// </summary>
        /// <typeparam name="T1">字典参数 1 的类型。</typeparam>
        /// <typeparam name="T2">字典参数 2 的类型。</typeparam>
        /// <typeparam name="T3">字典参数 3 的类型。</typeparam>
        /// <typeparam name="T4">字典参数 4 的类型。</typeparam>
        /// <typeparam name="T5">字典参数 5 的类型。</typeparam>
        /// <typeparam name="T6">字典参数 6 的类型。</typeparam>
        /// <param name="key">字典主键。</param>
        /// <param name="arg1">字典参数 1。</param>
        /// <param name="arg2">字典参数 2。</param>
        /// <param name="arg3">字典参数 3。</param>
        /// <param name="arg4">字典参数 4。</param>
        /// <param name="arg5">字典参数 5。</param>
        /// <param name="arg6">字典参数 6。</param>
        /// <returns>要获取的字典内容字符串。</returns>
        string GetString<T1, T2, T3, T4, T5, T6>(string key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);

        /// <summary>
        /// 根据字典主键获取字典内容字符串。
        /// </summary>
        /// <typeparam name="T1">字典参数 1 的类型。</typeparam>
        /// <typeparam name="T2">字典参数 2 的类型。</typeparam>
        /// <typeparam name="T3">字典参数 3 的类型。</typeparam>
        /// <typeparam name="T4">字典参数 4 的类型。</typeparam>
        /// <typeparam name="T5">字典参数 5 的类型。</typeparam>
        /// <typeparam name="T6">字典参数 6 的类型。</typeparam>
        /// <typeparam name="T7">字典参数 7 的类型。</typeparam>
        /// <param name="key">字典主键。</param>
        /// <param name="arg1">字典参数 1。</param>
        /// <param name="arg2">字典参数 2。</param>
        /// <param name="arg3">字典参数 3。</param>
        /// <param name="arg4">字典参数 4。</param>
        /// <param name="arg5">字典参数 5。</param>
        /// <param name="arg6">字典参数 6。</param>
        /// <param name="arg7">字典参数 7。</param>
        /// <returns>要获取的字典内容字符串。</returns>
        string GetString<T1, T2, T3, T4, T5, T6, T7>(string key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);

        /// <summary>
        /// 根据字典主键获取字典内容字符串。
        /// </summary>
        /// <typeparam name="T1">字典参数 1 的类型。</typeparam>
        /// <typeparam name="T2">字典参数 2 的类型。</typeparam>
        /// <typeparam name="T3">字典参数 3 的类型。</typeparam>
        /// <typeparam name="T4">字典参数 4 的类型。</typeparam>
        /// <typeparam name="T5">字典参数 5 的类型。</typeparam>
        /// <typeparam name="T6">字典参数 6 的类型。</typeparam>
        /// <typeparam name="T7">字典参数 7 的类型。</typeparam>
        /// <typeparam name="T8">字典参数 8 的类型。</typeparam>
        /// <param name="key">字典主键。</param>
        /// <param name="arg1">字典参数 1。</param>
        /// <param name="arg2">字典参数 2。</param>
        /// <param name="arg3">字典参数 3。</param>
        /// <param name="arg4">字典参数 4。</param>
        /// <param name="arg5">字典参数 5。</param>
        /// <param name="arg6">字典参数 6。</param>
        /// <param name="arg7">字典参数 7。</param>
        /// <param name="arg8">字典参数 8。</param>
        /// <returns>要获取的字典内容字符串。</returns>
        string GetString<T1, T2, T3, T4, T5, T6, T7, T8>(string key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8);

        /// <summary>
        /// 根据字典主键获取字典内容字符串。
        /// </summary>
        /// <typeparam name="T1">字典参数 1 的类型。</typeparam>
        /// <typeparam name="T2">字典参数 2 的类型。</typeparam>
        /// <typeparam name="T3">字典参数 3 的类型。</typeparam>
        /// <typeparam name="T4">字典参数 4 的类型。</typeparam>
        /// <typeparam name="T5">字典参数 5 的类型。</typeparam>
        /// <typeparam name="T6">字典参数 6 的类型。</typeparam>
        /// <typeparam name="T7">字典参数 7 的类型。</typeparam>
        /// <typeparam name="T8">字典参数 8 的类型。</typeparam>
        /// <typeparam name="T9">字典参数 9 的类型。</typeparam>
        /// <param name="key">字典主键。</param>
        /// <param name="arg1">字典参数 1。</param>
        /// <param name="arg2">字典参数 2。</param>
        /// <param name="arg3">字典参数 3。</param>
        /// <param name="arg4">字典参数 4。</param>
        /// <param name="arg5">字典参数 5。</param>
        /// <param name="arg6">字典参数 6。</param>
        /// <param name="arg7">字典参数 7。</param>
        /// <param name="arg8">字典参数 8。</param>
        /// <param name="arg9">字典参数 9。</param>
        /// <returns>要获取的字典内容字符串。</returns>
        string GetString<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9);

        /// <summary>
        /// 根据字典主键获取字典内容字符串。
        /// </summary>
        /// <typeparam name="T1">字典参数 1 的类型。</typeparam>
        /// <typeparam name="T2">字典参数 2 的类型。</typeparam>
        /// <typeparam name="T3">字典参数 3 的类型。</typeparam>
        /// <typeparam name="T4">字典参数 4 的类型。</typeparam>
        /// <typeparam name="T5">字典参数 5 的类型。</typeparam>
        /// <typeparam name="T6">字典参数 6 的类型。</typeparam>
        /// <typeparam name="T7">字典参数 7 的类型。</typeparam>
        /// <typeparam name="T8">字典参数 8 的类型。</typeparam>
        /// <typeparam name="T9">字典参数 9 的类型。</typeparam>
        /// <typeparam name="T10">字典参数 10 的类型。</typeparam>
        /// <param name="key">字典主键。</param>
        /// <param name="arg1">字典参数 1。</param>
        /// <param name="arg2">字典参数 2。</param>
        /// <param name="arg3">字典参数 3。</param>
        /// <param name="arg4">字典参数 4。</param>
        /// <param name="arg5">字典参数 5。</param>
        /// <param name="arg6">字典参数 6。</param>
        /// <param name="arg7">字典参数 7。</param>
        /// <param name="arg8">字典参数 8。</param>
        /// <param name="arg9">字典参数 9。</param>
        /// <param name="arg10">字典参数 10。</param>
        /// <returns>要获取的字典内容字符串。</returns>
        string GetString<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10);

        /// <summary>
        /// 根据字典主键获取字典内容字符串。
        /// </summary>
        /// <typeparam name="T1">字典参数 1 的类型。</typeparam>
        /// <typeparam name="T2">字典参数 2 的类型。</typeparam>
        /// <typeparam name="T3">字典参数 3 的类型。</typeparam>
        /// <typeparam name="T4">字典参数 4 的类型。</typeparam>
        /// <typeparam name="T5">字典参数 5 的类型。</typeparam>
        /// <typeparam name="T6">字典参数 6 的类型。</typeparam>
        /// <typeparam name="T7">字典参数 7 的类型。</typeparam>
        /// <typeparam name="T8">字典参数 8 的类型。</typeparam>
        /// <typeparam name="T9">字典参数 9 的类型。</typeparam>
        /// <typeparam name="T10">字典参数 10 的类型。</typeparam>
        /// <typeparam name="T11">字典参数 11 的类型。</typeparam>
        /// <param name="key">字典主键。</param>
        /// <param name="arg1">字典参数 1。</param>
        /// <param name="arg2">字典参数 2。</param>
        /// <param name="arg3">字典参数 3。</param>
        /// <param name="arg4">字典参数 4。</param>
        /// <param name="arg5">字典参数 5。</param>
        /// <param name="arg6">字典参数 6。</param>
        /// <param name="arg7">字典参数 7。</param>
        /// <param name="arg8">字典参数 8。</param>
        /// <param name="arg9">字典参数 9。</param>
        /// <param name="arg10">字典参数 10。</param>
        /// <param name="arg11">字典参数 11。</param>
        /// <returns>要获取的字典内容字符串。</returns>
        string GetString<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11);

        /// <summary>
        /// 根据字典主键获取字典内容字符串。
        /// </summary>
        /// <typeparam name="T1">字典参数 1 的类型。</typeparam>
        /// <typeparam name="T2">字典参数 2 的类型。</typeparam>
        /// <typeparam name="T3">字典参数 3 的类型。</typeparam>
        /// <typeparam name="T4">字典参数 4 的类型。</typeparam>
        /// <typeparam name="T5">字典参数 5 的类型。</typeparam>
        /// <typeparam name="T6">字典参数 6 的类型。</typeparam>
        /// <typeparam name="T7">字典参数 7 的类型。</typeparam>
        /// <typeparam name="T8">字典参数 8 的类型。</typeparam>
        /// <typeparam name="T9">字典参数 9 的类型。</typeparam>
        /// <typeparam name="T10">字典参数 10 的类型。</typeparam>
        /// <typeparam name="T11">字典参数 11 的类型。</typeparam>
        /// <typeparam name="T12">字典参数 12 的类型。</typeparam>
        /// <param name="key">字典主键。</param>
        /// <param name="arg1">字典参数 1。</param>
        /// <param name="arg2">字典参数 2。</param>
        /// <param name="arg3">字典参数 3。</param>
        /// <param name="arg4">字典参数 4。</param>
        /// <param name="arg5">字典参数 5。</param>
        /// <param name="arg6">字典参数 6。</param>
        /// <param name="arg7">字典参数 7。</param>
        /// <param name="arg8">字典参数 8。</param>
        /// <param name="arg9">字典参数 9。</param>
        /// <param name="arg10">字典参数 10。</param>
        /// <param name="arg11">字典参数 11。</param>
        /// <param name="arg12">字典参数 12。</param>
        /// <returns>要获取的字典内容字符串。</returns>
        string GetString<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12);

        /// <summary>
        /// 根据字典主键获取字典内容字符串。
        /// </summary>
        /// <typeparam name="T1">字典参数 1 的类型。</typeparam>
        /// <typeparam name="T2">字典参数 2 的类型。</typeparam>
        /// <typeparam name="T3">字典参数 3 的类型。</typeparam>
        /// <typeparam name="T4">字典参数 4 的类型。</typeparam>
        /// <typeparam name="T5">字典参数 5 的类型。</typeparam>
        /// <typeparam name="T6">字典参数 6 的类型。</typeparam>
        /// <typeparam name="T7">字典参数 7 的类型。</typeparam>
        /// <typeparam name="T8">字典参数 8 的类型。</typeparam>
        /// <typeparam name="T9">字典参数 9 的类型。</typeparam>
        /// <typeparam name="T10">字典参数 10 的类型。</typeparam>
        /// <typeparam name="T11">字典参数 11 的类型。</typeparam>
        /// <typeparam name="T12">字典参数 12 的类型。</typeparam>
        /// <typeparam name="T13">字典参数 13 的类型。</typeparam>
        /// <param name="key">字典主键。</param>
        /// <param name="arg1">字典参数 1。</param>
        /// <param name="arg2">字典参数 2。</param>
        /// <param name="arg3">字典参数 3。</param>
        /// <param name="arg4">字典参数 4。</param>
        /// <param name="arg5">字典参数 5。</param>
        /// <param name="arg6">字典参数 6。</param>
        /// <param name="arg7">字典参数 7。</param>
        /// <param name="arg8">字典参数 8。</param>
        /// <param name="arg9">字典参数 9。</param>
        /// <param name="arg10">字典参数 10。</param>
        /// <param name="arg11">字典参数 11。</param>
        /// <param name="arg12">字典参数 12。</param>
        /// <param name="arg13">字典参数 13。</param>
        /// <returns>要获取的字典内容字符串。</returns>
        string GetString<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(string key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13);

        /// <summary>
        /// 根据字典主键获取字典内容字符串。
        /// </summary>
        /// <typeparam name="T1">字典参数 1 的类型。</typeparam>
        /// <typeparam name="T2">字典参数 2 的类型。</typeparam>
        /// <typeparam name="T3">字典参数 3 的类型。</typeparam>
        /// <typeparam name="T4">字典参数 4 的类型。</typeparam>
        /// <typeparam name="T5">字典参数 5 的类型。</typeparam>
        /// <typeparam name="T6">字典参数 6 的类型。</typeparam>
        /// <typeparam name="T7">字典参数 7 的类型。</typeparam>
        /// <typeparam name="T8">字典参数 8 的类型。</typeparam>
        /// <typeparam name="T9">字典参数 9 的类型。</typeparam>
        /// <typeparam name="T10">字典参数 10 的类型。</typeparam>
        /// <typeparam name="T11">字典参数 11 的类型。</typeparam>
        /// <typeparam name="T12">字典参数 12 的类型。</typeparam>
        /// <typeparam name="T13">字典参数 13 的类型。</typeparam>
        /// <typeparam name="T14">字典参数 14 的类型。</typeparam>
        /// <param name="key">字典主键。</param>
        /// <param name="arg1">字典参数 1。</param>
        /// <param name="arg2">字典参数 2。</param>
        /// <param name="arg3">字典参数 3。</param>
        /// <param name="arg4">字典参数 4。</param>
        /// <param name="arg5">字典参数 5。</param>
        /// <param name="arg6">字典参数 6。</param>
        /// <param name="arg7">字典参数 7。</param>
        /// <param name="arg8">字典参数 8。</param>
        /// <param name="arg9">字典参数 9。</param>
        /// <param name="arg10">字典参数 10。</param>
        /// <param name="arg11">字典参数 11。</param>
        /// <param name="arg12">字典参数 12。</param>
        /// <param name="arg13">字典参数 13。</param>
        /// <param name="arg14">字典参数 14。</param>
        /// <returns>要获取的字典内容字符串。</returns>
        string GetString<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(string key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14);

        /// <summary>
        /// 根据字典主键获取字典内容字符串。
        /// </summary>
        /// <typeparam name="T1">字典参数 1 的类型。</typeparam>
        /// <typeparam name="T2">字典参数 2 的类型。</typeparam>
        /// <typeparam name="T3">字典参数 3 的类型。</typeparam>
        /// <typeparam name="T4">字典参数 4 的类型。</typeparam>
        /// <typeparam name="T5">字典参数 5 的类型。</typeparam>
        /// <typeparam name="T6">字典参数 6 的类型。</typeparam>
        /// <typeparam name="T7">字典参数 7 的类型。</typeparam>
        /// <typeparam name="T8">字典参数 8 的类型。</typeparam>
        /// <typeparam name="T9">字典参数 9 的类型。</typeparam>
        /// <typeparam name="T10">字典参数 10 的类型。</typeparam>
        /// <typeparam name="T11">字典参数 11 的类型。</typeparam>
        /// <typeparam name="T12">字典参数 12 的类型。</typeparam>
        /// <typeparam name="T13">字典参数 13 的类型。</typeparam>
        /// <typeparam name="T14">字典参数 14 的类型。</typeparam>
        /// <typeparam name="T15">字典参数 15 的类型。</typeparam>
        /// <param name="key">字典主键。</param>
        /// <param name="arg1">字典参数 1。</param>
        /// <param name="arg2">字典参数 2。</param>
        /// <param name="arg3">字典参数 3。</param>
        /// <param name="arg4">字典参数 4。</param>
        /// <param name="arg5">字典参数 5。</param>
        /// <param name="arg6">字典参数 6。</param>
        /// <param name="arg7">字典参数 7。</param>
        /// <param name="arg8">字典参数 8。</param>
        /// <param name="arg9">字典参数 9。</param>
        /// <param name="arg10">字典参数 10。</param>
        /// <param name="arg11">字典参数 11。</param>
        /// <param name="arg12">字典参数 12。</param>
        /// <param name="arg13">字典参数 13。</param>
        /// <param name="arg14">字典参数 14。</param>
        /// <param name="arg15">字典参数 15。</param>
        /// <returns>要获取的字典内容字符串。</returns>
        string GetString<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(string key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15);

        /// <summary>
        /// 根据字典主键获取字典内容字符串。
        /// </summary>
        /// <typeparam name="T1">字典参数 1 的类型。</typeparam>
        /// <typeparam name="T2">字典参数 2 的类型。</typeparam>
        /// <typeparam name="T3">字典参数 3 的类型。</typeparam>
        /// <typeparam name="T4">字典参数 4 的类型。</typeparam>
        /// <typeparam name="T5">字典参数 5 的类型。</typeparam>
        /// <typeparam name="T6">字典参数 6 的类型。</typeparam>
        /// <typeparam name="T7">字典参数 7 的类型。</typeparam>
        /// <typeparam name="T8">字典参数 8 的类型。</typeparam>
        /// <typeparam name="T9">字典参数 9 的类型。</typeparam>
        /// <typeparam name="T10">字典参数 10 的类型。</typeparam>
        /// <typeparam name="T11">字典参数 11 的类型。</typeparam>
        /// <typeparam name="T12">字典参数 12 的类型。</typeparam>
        /// <typeparam name="T13">字典参数 13 的类型。</typeparam>
        /// <typeparam name="T14">字典参数 14 的类型。</typeparam>
        /// <typeparam name="T15">字典参数 15 的类型。</typeparam>
        /// <typeparam name="T16">字典参数 16 的类型。</typeparam>
        /// <param name="key">字典主键。</param>
        /// <param name="arg1">字典参数 1。</param>
        /// <param name="arg2">字典参数 2。</param>
        /// <param name="arg3">字典参数 3。</param>
        /// <param name="arg4">字典参数 4。</param>
        /// <param name="arg5">字典参数 5。</param>
        /// <param name="arg6">字典参数 6。</param>
        /// <param name="arg7">字典参数 7。</param>
        /// <param name="arg8">字典参数 8。</param>
        /// <param name="arg9">字典参数 9。</param>
        /// <param name="arg10">字典参数 10。</param>
        /// <param name="arg11">字典参数 11。</param>
        /// <param name="arg12">字典参数 12。</param>
        /// <param name="arg13">字典参数 13。</param>
        /// <param name="arg14">字典参数 14。</param>
        /// <param name="arg15">字典参数 15。</param>
        /// <param name="arg16">字典参数 16。</param>
        /// <returns>要获取的字典内容字符串。</returns>
        string GetString<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(string key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16);

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
