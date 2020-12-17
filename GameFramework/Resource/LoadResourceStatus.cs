//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Resource
{
    /// <summary>
    /// 加载资源状态。
    /// </summary>
    public enum LoadResourceStatus : byte
    {
        /// <summary>
        /// 加载资源完成。
        /// </summary>
        Success = 0,

        /// <summary>
        /// 资源不存在。
        /// </summary>
        NotExist,

        /// <summary>
        /// 资源尚未准备完毕。
        /// </summary>
        NotReady,

        /// <summary>
        /// 依赖资源错误。
        /// </summary>
        DependencyError,

        /// <summary>
        /// 资源类型错误。
        /// </summary>
        TypeError,

        /// <summary>
        /// 加载资源错误。
        /// </summary>
        AssetError
    }
}
