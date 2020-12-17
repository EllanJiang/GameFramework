//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework.FileSystem;
using System;

namespace GameFramework.Resource
{
    /// <summary>
    /// 加载资源代理辅助器接口。
    /// </summary>
    public interface ILoadResourceAgentHelper
    {
        /// <summary>
        /// 加载资源代理辅助器异步加载资源更新事件。
        /// </summary>
        event EventHandler<LoadResourceAgentHelperUpdateEventArgs> LoadResourceAgentHelperUpdate;

        /// <summary>
        /// 加载资源代理辅助器异步读取资源文件完成事件。
        /// </summary>
        event EventHandler<LoadResourceAgentHelperReadFileCompleteEventArgs> LoadResourceAgentHelperReadFileComplete;

        /// <summary>
        /// 加载资源代理辅助器异步读取资源二进制流完成事件。
        /// </summary>
        event EventHandler<LoadResourceAgentHelperReadBytesCompleteEventArgs> LoadResourceAgentHelperReadBytesComplete;

        /// <summary>
        /// 加载资源代理辅助器异步将资源二进制流转换为加载对象完成事件。
        /// </summary>
        event EventHandler<LoadResourceAgentHelperParseBytesCompleteEventArgs> LoadResourceAgentHelperParseBytesComplete;

        /// <summary>
        /// 加载资源代理辅助器异步加载资源完成事件。
        /// </summary>
        event EventHandler<LoadResourceAgentHelperLoadCompleteEventArgs> LoadResourceAgentHelperLoadComplete;

        /// <summary>
        /// 加载资源代理辅助器错误事件。
        /// </summary>
        event EventHandler<LoadResourceAgentHelperErrorEventArgs> LoadResourceAgentHelperError;

        /// <summary>
        /// 通过加载资源代理辅助器开始异步读取资源文件。
        /// </summary>
        /// <param name="fullPath">要加载资源的完整路径名。</param>
        void ReadFile(string fullPath);

        /// <summary>
        /// 通过加载资源代理辅助器开始异步读取资源文件。
        /// </summary>
        /// <param name="fileSystem">要加载资源的文件系统。</param>
        /// <param name="name">要加载资源的名称。</param>
        void ReadFile(IFileSystem fileSystem, string name);

        /// <summary>
        /// 通过加载资源代理辅助器开始异步读取资源二进制流。
        /// </summary>
        /// <param name="fullPath">要加载资源的完整路径名。</param>
        void ReadBytes(string fullPath);

        /// <summary>
        /// 通过加载资源代理辅助器开始异步读取资源二进制流。
        /// </summary>
        /// <param name="fileSystem">要加载资源的文件系统。</param>
        /// <param name="name">要加载资源的名称。</param>
        void ReadBytes(IFileSystem fileSystem, string name);

        /// <summary>
        /// 通过加载资源代理辅助器开始异步将资源二进制流转换为加载对象。
        /// </summary>
        /// <param name="bytes">要加载资源的二进制流。</param>
        void ParseBytes(byte[] bytes);

        /// <summary>
        /// 通过加载资源代理辅助器开始异步加载资源。
        /// </summary>
        /// <param name="resource">资源。</param>
        /// <param name="assetName">要加载的资源名称。</param>
        /// <param name="assetType">要加载资源的类型。</param>
        /// <param name="isScene">要加载的资源是否是场景。</param>
        void LoadAsset(object resource, string assetName, Type assetType, bool isScene);

        /// <summary>
        /// 重置加载资源代理辅助器。
        /// </summary>
        void Reset();
    }
}
