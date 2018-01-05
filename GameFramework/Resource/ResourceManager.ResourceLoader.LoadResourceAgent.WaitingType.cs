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
        private partial class ResourceLoader
        {
            private partial class LoadResourceAgent
            {
                private enum WaitingType
                {
                    None = 0,
                    WaitForAsset,
                    WaitForDependencyAsset,
                    WaitForResource,
                }
            }
        }
    }
}
