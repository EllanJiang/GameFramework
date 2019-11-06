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
    /// 游戏框架多值字典类。
    /// </summary>
    /// <typeparam name="TKey">指定多值字典的主键类型。</typeparam>
    /// <typeparam name="TValue">指定多值字典的值类型。</typeparam>
    public sealed class GameFrameworkMultiDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, GameFrameworkLinkedListRange<TValue>>>, IEnumerable
    {
        private readonly GameFrameworkLinkedList<TValue> m_LinkedList;
        private readonly Dictionary<TKey, GameFrameworkLinkedListRange<TValue>> m_Dictionary;

        /// <summary>
        /// 初始化游戏框架多值字典类的新实例。
        /// </summary>
        public GameFrameworkMultiDictionary()
        {
            m_LinkedList = new GameFrameworkLinkedList<TValue>();
            m_Dictionary = new Dictionary<TKey, GameFrameworkLinkedListRange<TValue>>();
        }

        /// <summary>
        /// 获取多值字典中实际包含的主键数量。
        /// </summary>
        public int Count
        {
            get
            {
                return m_Dictionary.Count;
            }
        }

        /// <summary>
        /// 获取多值字典中指定主键的范围。
        /// </summary>
        /// <param name="key">指定的主键。</param>
        /// <returns>指定主键的范围。</returns>
        public GameFrameworkLinkedListRange<TValue> this[TKey key]
        {
            get
            {
                GameFrameworkLinkedListRange<TValue> range = default(GameFrameworkLinkedListRange<TValue>);
                m_Dictionary.TryGetValue(key, out range);
                return range;
            }
        }

        /// <summary>
        /// 清理多值字典。
        /// </summary>
        public void Clear()
        {
            m_Dictionary.Clear();
            m_LinkedList.Clear();
        }

        /// <summary>
        /// 检查多值字典中是否包含指定主键。
        /// </summary>
        /// <param name="key">要检查的主键。</param>
        /// <returns>多值字典中是否包含指定主键。</returns>
        public bool Contains(TKey key)
        {
            return m_Dictionary.ContainsKey(key);
        }

        /// <summary>
        /// 检查多值字典中是否包含指定值。
        /// </summary>
        /// <param name="key">要检查的主键。</param>
        /// <param name="value">要检查的值。</param>
        /// <returns>多值字典中是否包含指定值。</returns>
        public bool Contains(TKey key, TValue value)
        {
            GameFrameworkLinkedListRange<TValue> range = default(GameFrameworkLinkedListRange<TValue>);
            if (m_Dictionary.TryGetValue(key, out range))
            {
                return range.Contains(value);
            }

            return false;
        }

        /// <summary>
        /// 尝试获取多值字典中指定主键的范围。
        /// </summary>
        /// <param name="key">指定的主键。</param>
        /// <param name="range">指定主键的范围。</param>
        /// <returns>是否获取成功。</returns>
        public bool TryGetValue(TKey key, out GameFrameworkLinkedListRange<TValue> range)
        {
            return m_Dictionary.TryGetValue(key, out range);
        }

        /// <summary>
        /// 向指定的主键增加指定的值。
        /// </summary>
        /// <param name="key">指定的主键。</param>
        /// <param name="value">指定的值。</param>
        public void Add(TKey key, TValue value)
        {
            GameFrameworkLinkedListRange<TValue> range = default(GameFrameworkLinkedListRange<TValue>);
            if (m_Dictionary.TryGetValue(key, out range))
            {
                LinkedListNode<TValue> last = m_LinkedList.AddAfter(range.Last, value);
                m_Dictionary[key] = new GameFrameworkLinkedListRange<TValue>(range.First, last);
            }
            else
            {
                LinkedListNode<TValue> node = m_LinkedList.AddLast(value);
                m_Dictionary.Add(key, new GameFrameworkLinkedListRange<TValue>(node));
            }
        }

        /// <summary>
        /// 从指定的主键中移除指定的值。
        /// </summary>
        /// <param name="key">指定的主键。</param>
        /// <param name="value">指定的值。</param>
        /// <returns>是否移除成功。</returns>
        public bool Remove(TKey key, TValue value)
        {
            GameFrameworkLinkedListRange<TValue> range = default(GameFrameworkLinkedListRange<TValue>);
            if (m_Dictionary.TryGetValue(key, out range))
            {
                LinkedListNode<TValue> current = range.First;
                while (current != null)
                {
                    if (current.Value.Equals(value))
                    {
                        if (current == range.First)
                        {
                            if (current == range.Last)
                            {
                                m_Dictionary.Remove(key);
                            }
                            else
                            {
                                m_Dictionary[key] = new GameFrameworkLinkedListRange<TValue>(current.Next, range.Last);
                            }
                        }
                        else if (current == range.Last)
                        {
                            m_Dictionary[key] = new GameFrameworkLinkedListRange<TValue>(range.First, current.Previous);
                        }

                        m_LinkedList.Remove(current);
                        return true;
                    }

                    current = current != range.Last ? current.Next : null;
                }
            }

            return false;
        }

        /// <summary>
        /// 从指定的主键中移除所有的值。
        /// </summary>
        /// <param name="key">指定的主键。</param>
        /// <returns>是否移除成功。</returns>
        public bool RemoveAll(TKey key)
        {
            GameFrameworkLinkedListRange<TValue> range = default(GameFrameworkLinkedListRange<TValue>);
            if (m_Dictionary.TryGetValue(key, out range))
            {
                m_Dictionary.Remove(key);

                LinkedListNode<TValue> current = range.First;
                LinkedListNode<TValue> next = null;
                while (current != null)
                {
                    next = current != range.Last ? current.Next : null;
                    m_LinkedList.Remove(current);
                    current = next;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// 返回循环访问集合的枚举数。
        /// </summary>
        /// <returns>循环访问集合的枚举数。</returns>
        public Enumerator GetEnumerator()
        {
            return new Enumerator(m_Dictionary);
        }

        IEnumerator<KeyValuePair<TKey, GameFrameworkLinkedListRange<TValue>>> IEnumerable<KeyValuePair<TKey, GameFrameworkLinkedListRange<TValue>>>.GetEnumerator()
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
        public struct Enumerator : IEnumerator<KeyValuePair<TKey, GameFrameworkLinkedListRange<TValue>>>, IEnumerator
        {
            private Dictionary<TKey, GameFrameworkLinkedListRange<TValue>>.Enumerator m_Enumerator;

            internal Enumerator(Dictionary<TKey, GameFrameworkLinkedListRange<TValue>> dictionary)
            {
                if (dictionary == null)
                {
                    throw new GameFrameworkException("Dictionary is invalid.");
                }

                m_Enumerator = dictionary.GetEnumerator();
            }

            /// <summary>
            /// 获取当前结点。
            /// </summary>
            public KeyValuePair<TKey, GameFrameworkLinkedListRange<TValue>> Current
            {
                get
                {
                    return m_Enumerator.Current;
                }
            }

            /// <summary>
            /// 获取当前的枚举数。
            /// </summary>
            object IEnumerator.Current
            {
                get
                {
                    return m_Enumerator.Current;
                }
            }

            /// <summary>
            /// 清理枚举数。
            /// </summary>
            public void Dispose()
            {
                m_Enumerator.Dispose();
            }

            /// <summary>
            /// 获取下一个结点。
            /// </summary>
            /// <returns>返回下一个结点。</returns>
            public bool MoveNext()
            {
                return m_Enumerator.MoveNext();
            }

            /// <summary>
            /// 重置枚举数。
            /// </summary>
            void IEnumerator.Reset()
            {
                ((IEnumerator<KeyValuePair<TKey, GameFrameworkLinkedListRange<TValue>>>)m_Enumerator).Reset();
            }
        }
    }
}
