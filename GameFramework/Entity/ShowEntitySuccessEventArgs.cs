//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Entity
{
    /// <summary>
    /// 显示实体成功事件。
    /// </summary>
    public sealed class ShowEntitySuccessEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化显示实体成功事件的新实例。
        /// </summary>
        /// <param name="entity">加载成功的实体。</param>
        /// <param name="duration">加载持续时间。</param>
        /// <param name="userData">用户自定义数据。</param>
        public ShowEntitySuccessEventArgs(IEntity entity, float duration, object userData)
        {
            Entity = entity;
            Duration = duration;
            UserData = userData;
        }

        /// <summary>
        /// 获取显示成功的实体。
        /// </summary>
        public IEntity Entity
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取加载持续时间。
        /// </summary>
        public float Duration
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取用户自定义数据。
        /// </summary>
        public object UserData
        {
            get;
            private set;
        }
    }
}
