//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.Event;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 事件组件。
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Game Framework/Event")]
    public sealed class EventComponent : GameFrameworkComponent
    {
        private IEventManager m_EventManager = null;

        /// <summary>
        /// 获取事件数量。
        /// </summary>
        public int Count
        {
            get
            {
                return m_EventManager.Count;
            }
        }

        /// <summary>
        /// 游戏框架组件初始化。
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            m_EventManager = GameFrameworkEntry.GetModule<IEventManager>();
            if (m_EventManager == null)
            {
                Log.Fatal("Event manager is invalid.");
                return;
            }
        }

        private void Start()
        {

        }

        /// <summary>
        /// 检查订阅事件处理回调函数。
        /// </summary>
        /// <param name="id">事件类型编号。</param>
        /// <param name="handler">要检查的事件处理回调函数。</param>
        /// <returns>是否存在事件处理回调函数。</returns>
        public bool Check(EventId id, EventHandler<GameEventArgs> handler)
        {
            return m_EventManager.Check((int)id, handler);
        }

        /// <summary>
        /// 订阅事件处理回调函数。
        /// </summary>
        /// <param name="id">事件类型编号。</param>
        /// <param name="handler">要订阅的事件处理回调函数。</param>
        public void Subscribe(EventId id, EventHandler<GameEventArgs> handler)
        {
            m_EventManager.Subscribe((int)id, handler);
        }

        /// <summary>
        /// 取消订阅事件处理回调函数。
        /// </summary>
        /// <param name="id">事件类型编号。</param>
        /// <param name="handler">要取消订阅的事件处理回调函数。</param>
        public void Unsubscribe(EventId id, EventHandler<GameEventArgs> handler)
        {
            m_EventManager.Unsubscribe((int)id, handler);
        }

        /// <summary>
        /// 抛出事件，这个操作是线程安全的，即使不在主线程中抛出，也可保证在主线程中回调事件处理函数，但事件会在抛出后的下一帧分发。
        /// </summary>
        /// <param name="sender">事件发送者。</param>
        /// <param name="e">事件内容。</param>
        public void Fire(object sender, GameEventArgs e)
        {
            m_EventManager.Fire(sender, e);
        }

        /// <summary>
        /// 抛出事件立即模式，这个操作不是线程安全的，事件会立刻分发。
        /// </summary>
        /// <param name="sender">事件发送者。</param>
        /// <param name="e">事件内容。</param>
        public void FireNow(object sender, GameEventArgs e)
        {
            m_EventManager.FireNow(sender, e);
        }

        private void AvoidJIT()
        {
            new Dictionary<int, EventHandler<GameEventArgs>>();
        }
    }
}
