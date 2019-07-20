//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Resource
{
    /// <summary>
    /// 资源组接口。
    /// </summary>
    public interface IResourceGroup
    {
        /// <summary>
        /// 获取资源组名称。
        /// </summary>
        string Name
        {
            get;
        }

        /// <summary>
        /// 获取资源组是否准备完毕。
        /// </summary>
        bool Ready
        {
            get;
        }

        /// <summary>
        /// 获取资源组包含资源数量。
        /// </summary>
        int TotalCount
        {
            get;
        }

        /// <summary>
        /// 获取资源组中已准备完成资源数量。
        /// </summary>
        int ReadyCount
        {
            get;
        }

        /// <summary>
        /// 获取资源组包含资源的总大小。
        /// </summary>
        long TotalLength
        {
            get;
        }

        /// <summary>
        /// 获取资源组包含资源压缩后的总大小。
        /// </summary>
        long TotalZipLength
        {
            get;
        }

        /// <summary>
        /// 获取资源组中已准备完成资源的总大小。
        /// </summary>
        long ReadyLength
        {
            get;
        }

        /// <summary>
        /// 获取资源组的完成进度。
        /// </summary>
        float Progress
        {
            get;
        }

        /// <summary>
        /// 获取资源组包含的资源名称列表。
        /// </summary>
        /// <returns>资源组包含的资源名称列表。</returns>
        string[] GetResourceNames();
    }
}
