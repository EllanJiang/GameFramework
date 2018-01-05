//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Resource
{
    internal partial class ResourceManager
    {
        private partial class ResourceChecker
        {
            private partial class CheckInfo
            {
                /// <summary>
                /// 资源检查状态。
                /// </summary>
                public enum CheckStatus
                {
                    /// <summary>
                    /// 状态未知。
                    /// </summary>
                    Unknown = 0,

                    /// <summary>
                    /// 需要更新。
                    /// </summary>
                    NeedUpdate,

                    /// <summary>
                    /// 存在最新且已存放于只读区中。
                    /// </summary>
                    StorageInReadOnly,

                    /// <summary>
                    /// 存在最新且已存放于读写区中。
                    /// </summary>
                    StorageInReadWrite,

                    /// <summary>
                    /// 不适用于当前变体。
                    /// </summary>
                    Unavailable,

                    /// <summary>
                    /// 已废弃。
                    /// </summary>
                    Disuse
                }
            }
        }
    }
}
