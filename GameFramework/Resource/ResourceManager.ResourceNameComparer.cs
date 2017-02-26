//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System.Collections.Generic;

namespace GameFramework.Resource
{
    internal partial class ResourceManager
    {
        /// <summary>
        /// 资源名称比较器。
        /// </summary>
        private struct ResourceNameComparer : IComparer<ResourceName>
        {
            public int Compare(ResourceName x, ResourceName y)
            {
                return x.FullName.CompareTo(y.FullName);
            }
        }
    }
}
