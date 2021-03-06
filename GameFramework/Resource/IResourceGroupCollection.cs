//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System.Collections.Generic;

namespace GameFramework.Resource
{
    /// <summary>
    /// 资源组集合接口。
    /// </summary>
    public interface IResourceGroupCollection
    {
        /// <summary>
        /// 获取资源组集合是否准备完毕。
        /// </summary>
        bool Ready
        {
            get;
        }

        /// <summary>
        /// 获取资源组集合包含资源数量。
        /// </summary>
        int TotalCount
        {
            get;
        }

        /// <summary>
        /// 获取资源组集合中已准备完成资源数量。
        /// </summary>
        int ReadyCount
        {
            get;
        }

        /// <summary>
        /// 获取资源组集合包含资源的总大小。
        /// </summary>
        long TotalLength
        {
            get;
        }

        /// <summary>
        /// 获取资源组集合包含资源压缩后的总大小。
        /// </summary>
        long TotalCompressedLength
        {
            get;
        }

        /// <summary>
        /// 获取资源组集合中已准备完成资源的总大小。
        /// </summary>
        long ReadyLength
        {
            get;
        }

        /// <summary>
        /// 获取资源组集合中已准备完成资源压缩后的总大小。
        /// </summary>
        long ReadyCompressedLength
        {
            get;
        }

        /// <summary>
        /// 获取资源组集合的完成进度。
        /// </summary>
        float Progress
        {
            get;
        }

        /// <summary>
        /// 获取资源组集合包含的资源组列表。
        /// </summary>
        /// <returns>资源组包含的资源名称列表。</returns>
        IResourceGroup[] GetResourceGroups();

        /// <summary>
        /// 获取资源组集合包含的资源名称列表。
        /// </summary>
        /// <returns>资源组包含的资源名称列表。</returns>
        string[] GetResourceNames();

        /// <summary>
        /// 获取资源组集合包含的资源名称列表。
        /// </summary>
        /// <param name="results">资源组包含的资源名称列表。</param>
        void GetResourceNames(List<string> results);
    }
}
