//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System.IO;

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
        /// <param name="configAsset">全局配置资源。</param>
        /// <param name="loadType">全局配置加载方式。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否加载成功。</returns>
        bool LoadConfig(object configAsset, LoadType loadType, object userData);

        /// <summary>
        /// 解析全局配置。
        /// </summary>
        /// <param name="text">要解析的全局配置文本。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否解析全局配置成功。</returns>
        bool ParseConfig(string text, object userData);

        /// <summary>
        /// 解析全局配置。
        /// </summary>
        /// <param name="bytes">要解析的全局配置二进制流。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否解析全局配置成功。</returns>
        bool ParseConfig(byte[] bytes, object userData);

        /// <summary>
        /// 解析全局配置。
        /// </summary>
        /// <param name="stream">要解析的全局配置二进制流。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否解析全局配置成功。</returns>
        bool ParseConfig(Stream stream, object userData);

        /// <summary>
        /// 释放全局配置资源。
        /// </summary>
        /// <param name="configAsset">要释放的全局配置资源。</param>
        void ReleaseConfigAsset(object configAsset);
    }
}
