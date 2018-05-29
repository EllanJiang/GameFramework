//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.ObjectPool;

namespace GameFramework.Resource
{
    internal partial class ResourceManager
    {
        private partial class ResourceLoader
        {
            /// <summary>
            /// 资源对象。
            /// </summary>
            private sealed class ResourceObject : ObjectBase
            {
                private readonly IResourceHelper m_ResourceHelper;

                public ResourceObject(string name, object target, IResourceHelper resourceHelper)
                    : base(name, target)
                {
                    if (resourceHelper == null)
                    {
                        throw new GameFrameworkException("Resource helper is invalid.");
                    }

                    m_ResourceHelper = resourceHelper;
                }

                protected internal override void Release(bool isShutdown)
                {
                    m_ResourceHelper.Release(Target);
                }
            }
        }
    }
}
