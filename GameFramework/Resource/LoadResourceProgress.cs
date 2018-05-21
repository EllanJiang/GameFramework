//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Resource
{
    /// <summary>
    /// 加载资源进度类型。
    /// </summary>
    public enum LoadResourceProgress
    {
        /// <summary>
        /// 读取资源包。
        /// </summary>
        ReadResource,

        /// <summary>
        /// 加载资源包。
        /// </summary>
        LoadResource,

        /// <summary>
        /// 加载资源。
        /// </summary>
        LoadAsset,

        /// <summary>
        /// 加载场景。
        /// </summary>
        LoadScene,
    }
}
