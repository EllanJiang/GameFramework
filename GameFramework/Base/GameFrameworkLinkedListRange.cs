//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;

namespace GameFramework
{
    /// <summary>
    /// 游戏框架链表范围。
    /// </summary>
    /// <typeparam name="T">指定链表范围的元素类型。</typeparam>
    public struct GameFrameworkLinkedListRange<T> : IEnumerable<T>, IEnumerable
    {
        private readonly LinkedListNode<T> m_First;
        private readonly LinkedListNode<T> m_Last;

        /// <summary>
        /// 初始化游戏框架链表范围的新实例。
        /// </summary>
        /// <param name="node">链表范围的唯一结点。</param>
        public GameFrameworkLinkedListRange(LinkedListNode<T> node)
            : this(node, node)
        {
        }

        /// <summary>
        /// 初始化游戏框架链表范围的新实例。
        /// </summary>
        /// <param name="first">链表范围的开始结点。</param>
        /// <param name="last">链表范围的结束结点。</param>
        public GameFrameworkLinkedListRange(LinkedListNode<T> first, LinkedListNode<T> last)
        {
            m_First = first;
            m_Last = last;
        }

        /// <summary>
        /// 获取链表范围是否有效。
        /// </summary>
        public bool IsValid
        {
            get
            {
                return m_First != null && m_Last != null;
            }
        }

        /// <summary>
        /// 获取链表范围的开始结点。
        /// </summary>
        public LinkedListNode<T> First
        {
            get
            {
                return m_First;
            }
        }

        /// <summary>
        /// 获取链表范围的结束结点。
        /// </summary>
        public LinkedListNode<T> Last
        {
            get
            {
                return m_Last;
            }
        }

        /// <summary>
        /// 获取链表范围的结点数量。
        /// </summary>
        public int Count
        {
            get
            {
                if (!IsValid)
                {
                    return 0;
                }

                int count = 1;
                for (LinkedListNode<T> current = m_First; current != null && current != m_Last; current = current.Next)
                {
                    count++;
                }

                return count;
            }
        }

        /// <summary>
        /// 检查是否包含指定值。
        /// </summary>
        /// <param name="value">要检查的值。</param>
        /// <returns>是否包含指定值。</returns>
        public bool Contains(T value)
        {
            LinkedListNode<T> terminal = Last.Next;
            LinkedListNode<T> current = First;
            while (current != null && current != terminal)
            {
                if (current.Value.Equals(value))
                {
                    return true;
                }

                current = current.Next;
            }

            return false;
        }

        /// <summary>
        /// 返回循环访问集合的枚举数。
        /// </summary>
        /// <returns>循环访问集合的枚举数。</returns>
        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// 循环访问集合的枚举数。
        /// </summary>
        public struct Enumerator : IEnumerator<T>, IEnumerator
        {
            private readonly GameFrameworkLinkedListRange<T> m_GameFrameworkLinkedListRange;
            private readonly LinkedListNode<T> m_Terminal;
            private LinkedListNode<T> m_Current;
            private T m_CurrentValue;

            internal Enumerator(GameFrameworkLinkedListRange<T> range)
            {
                if (!range.IsValid)
                {
                    throw new GameFrameworkException("Range is invalid.");
                }

                m_GameFrameworkLinkedListRange = range;
                m_Terminal = m_GameFrameworkLinkedListRange.Last.Next;
                m_Current = m_GameFrameworkLinkedListRange.First;
                m_CurrentValue = default(T);
            }

            /// <summary>
            /// 获取当前结点。
            /// </summary>
            public T Current
            {
                get
                {
                    return m_CurrentValue;
                }
            }

            /// <summary>
            /// 获取当前的枚举数。
            /// </summary>
            object IEnumerator.Current
            {
                get
                {
                    return m_CurrentValue;
                }
            }

            /// <summary>
            /// 清理枚举数。
            /// </summary>
            public void Dispose()
            {
            }

            /// <summary>
            /// 获取下一个结点。
            /// </summary>
            /// <returns>返回下一个结点。</returns>
            public bool MoveNext()
            {
                if (m_Current == null || m_Current == m_Terminal)
                {
                    return false;
                }

                m_CurrentValue = m_Current.Value;
                m_Current = m_Current.Next;
                return true;
            }

            /// <summary>
            /// 重置枚举数。
            /// </summary>
            void IEnumerator.Reset()
            {
                m_Current = m_GameFrameworkLinkedListRange.First;
                m_CurrentValue = default(T);
            }
        }
    }
}
