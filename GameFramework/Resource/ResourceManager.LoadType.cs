//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Resource
{
    internal sealed partial class ResourceManager : GameFrameworkModule, IResourceManager
    {
        /// <summary>
        /// 资源加载方式类型。
        /// </summary>
        private enum LoadType : byte
        {
            /// <summary>
            /// 按二进制直接加载。
            /// </summary>
            LoadFromBinary = 0,

            /// <summary>
            /// 使用文件方式加载。
            /// </summary>
            LoadFromFile,

            /// <summary>
            /// 使用内存方式加载。
            /// </summary>
            LoadFromMemory,

            /// <summary>
            /// 使用内存快速解密方式加载。
            /// </summary>
            LoadFromMemoryAndQuickDecrypt,

            /// <summary>
            /// 使用内存解密方式加载。
            /// </summary>
            LoadFromMemoryAndDecrypt
        }
    }
}
