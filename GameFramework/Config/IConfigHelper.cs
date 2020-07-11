//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Config
{
    /// <summary>
    /// 全局配置辅助器接口。
    /// </summary>
    public interface IConfigHelper
    {
        /// <summary>
        /// 加载全局配置。
        /// </summary>
        /// <param name="configAssetName">全局配置资源名称。</param>
        /// <param name="configAsset">全局配置资源。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否加载成功。</returns>
        bool LoadConfig(string configAssetName, object configAsset, object userData);

        /// <summary>
        /// 加载全局配置。
        /// </summary>
        /// <param name="configAssetName">全局配置资源名称。</param>
        /// <param name="configBytes">全局配置二进制流。</param>
        /// <param name="startIndex">全局配置二进制流的起始位置。</param>
        /// <param name="length">全局配置二进制流的长度。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否加载成功。</returns>
        bool LoadConfig(string configAssetName, byte[] configBytes, int startIndex, int length, object userData);

        /// <summary>
        /// 解析全局配置。
        /// </summary>
        /// <param name="configString">要解析的全局配置字符串。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否解析全局配置成功。</returns>
        bool ParseConfig(string configString, object userData);

        /// <summary>
        /// 解析全局配置。
        /// </summary>
        /// <param name="configBytes">要解析的全局配置二进制流。</param>
        /// <param name="startIndex">全局配置二进制流的起始位置。</param>
        /// <param name="length">全局配置二进制流的长度。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否解析全局配置成功。</returns>
        bool ParseConfig(byte[] configBytes, int startIndex, int length, object userData);

        /// <summary>
        /// 释放全局配置资源。
        /// </summary>
        /// <param name="configAsset">要释放的全局配置资源。</param>
        void ReleaseConfigAsset(object configAsset);
    }
}
