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
        private readonly LinkedListNode<T> m_FirstNode;
        private readonly LinkedListNode<T> m_LastNode;

        /// <summary>
        /// 初始化游戏框架链表范围的新实例。
        /// </summary>
        /// <param name="onlyNode">链表范围的唯一结点。</param>
        public GameFrameworkLinkedListRange(LinkedListNode<T> onlyNode)
            : this(onlyNode, onlyNode)
        {
        }

        /// <summary>
        /// 初始化游戏框架链表范围的新实例。
        /// </summary>
        /// <param name="firstNode">链表范围的开始结点。</param>
        /// <param name="lastNode">链表范围的结束结点。</param>
        public GameFrameworkLinkedListRange(LinkedListNode<T> firstNode, LinkedListNode<T> lastNode)
        {
            m_FirstNode = firstNode;
            m_LastNode = lastNode;
        }

        /// <summary>
        /// 获取链表范围是否有效。
        /// </summary>
        public bool IsValid
        {
            get
            {
                return m_FirstNode != null && m_LastNode != null;
            }
        }

        /// <summary>
        /// 获取链表范围的开始结点。
        /// </summary>
        public LinkedListNode<T> FirstNode
        {
            get
            {
                return m_FirstNode;
            }
        }

        /// <summary>
        /// 获取链表范围的结束结点。
        /// </summary>
        public LinkedListNode<T> LastNode
        {
            get
            {
                return m_LastNode;
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
                for (LinkedListNode<T> currentNode = m_FirstNode; currentNode != null && currentNode != m_LastNode; currentNode = currentNode.Next)
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
            LinkedListNode<T> terminalNode = LastNode.Next;
            LinkedListNode<T> currentNode = FirstNode;
            while (currentNode != null && currentNode != terminalNode)
            {
                if (currentNode.Value.Equals(value))
                {
                    return true;
                }

                currentNode = currentNode.Next;
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
            private readonly LinkedListNode<T> m_TerminalNode;
            private LinkedListNode<T> m_CurrentNode;
            private T m_Current;

            internal Enumerator(GameFrameworkLinkedListRange<T> range)
            {
                if (!range.IsValid)
                {
                    throw new GameFrameworkException("Range is invalid.");
                }

                m_GameFrameworkLinkedListRange = range;
                m_TerminalNode = m_GameFrameworkLinkedListRange.LastNode.Next;
                m_CurrentNode = m_GameFrameworkLinkedListRange.FirstNode;
                m_Current = default(T);
            }

            /// <summary>
            /// 获取当前结点。
            /// </summary>
            public T Current
            {
                get
                {
                    return m_Current;
                }
            }

            /// <summary>
            /// 获取当前的枚举数。
            /// </summary>
            object IEnumerator.Current
            {
                get
                {
                    return m_Current;
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
                if (m_CurrentNode == null || m_CurrentNode == m_TerminalNode)
                {
                    return false;
                }

                m_Current = m_CurrentNode.Value;
                m_CurrentNode = m_CurrentNode.Next;
                return true;
            }

            /// <summary>
            /// 重置枚举数。
            /// </summary>
            void IEnumerator.Reset()
            {
                m_CurrentNode = m_GameFrameworkLinkedListRange.FirstNode;
                m_Current = default(T);
            }
        }
    }
}
