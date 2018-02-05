using System;
using System.Collections.Generic;

namespace GameFramework
{
    public static partial class ReferencePool
    {
        private sealed class ReferenceCollection
        {
            private readonly Queue<IReference> m_References;
            private int m_UsingReferenceCount;
            private int m_AcquireReferenceCount;
            private int m_ReleaseReferenceCount;
            private int m_AddReferenceCount;
            private int m_RemoveReferenceCount;

            public ReferenceCollection()
            {
                m_References = new Queue<IReference>();
                m_UsingReferenceCount = 0;
                m_AcquireReferenceCount = 0;
                m_ReleaseReferenceCount = 0;
                m_AddReferenceCount = 0;
                m_RemoveReferenceCount = 0;
            }

            public int UnusedReferenceCount
            {
                get
                {
                    return m_References.Count;
                }
            }

            public int UsingReferenceCount
            {
                get
                {
                    return m_UsingReferenceCount;
                }
            }

            public int AcquireReferenceCount
            {
                get
                {
                    return m_AcquireReferenceCount;
                }
            }

            public int ReleaseReferenceCount
            {
                get
                {
                    return m_ReleaseReferenceCount;
                }
            }

            public int AddReferenceCount
            {
                get
                {
                    return m_AddReferenceCount;
                }
            }

            public int RemoveReferenceCount
            {
                get
                {
                    return m_RemoveReferenceCount;
                }
            }

            public T Acquire<T>() where T : class, IReference, new()
            {
                m_UsingReferenceCount++;
                m_AcquireReferenceCount++;
                lock (m_References)
                {
                    if (m_References.Count > 0)
                    {
                        return (T)m_References.Dequeue();
                    }
                }

                m_AddReferenceCount++;
                return new T();
            }

            public IReference Acquire(Type referenceType)
            {
                m_UsingReferenceCount++;
                m_AcquireReferenceCount++;
                lock (m_References)
                {
                    if (m_References.Count > 0)
                    {
                        return m_References.Dequeue();
                    }
                }

                m_AddReferenceCount++;
                return (IReference)Activator.CreateInstance(referenceType);
            }

            public void Release<T>(T reference) where T : class, IReference
            {
                reference.Clear();
                lock (m_References)
                {
                    m_References.Enqueue(reference);
                }

                m_ReleaseReferenceCount++;
                m_UsingReferenceCount--;
            }

            public void Release(Type referenceType, IReference reference)
            {
                reference.Clear();
                lock (m_References)
                {
                    m_References.Enqueue(reference);
                }

                m_ReleaseReferenceCount++;
                m_UsingReferenceCount--;
            }

            public void Add<T>(int count) where T : class, IReference, new()
            {
                lock (m_References)
                {
                    m_AddReferenceCount += count;
                    while (count-- > 0)
                    {
                        m_References.Enqueue(new T());
                    }
                }
            }

            public void Add(Type referenceType, int count)
            {
                lock (m_References)
                {
                    m_AddReferenceCount += count;
                    while (count-- > 0)
                    {
                        m_References.Enqueue((IReference)Activator.CreateInstance(referenceType));
                    }
                }
            }

            public void Remove<T>(int count) where T : class, IReference
            {
                lock (m_References)
                {
                    if (count > m_References.Count)
                    {
                        count = m_References.Count;
                    }

                    m_RemoveReferenceCount += count;
                    while (count-- > 0)
                    {
                        m_References.Dequeue();
                    }
                }
            }

            public void Remove(Type referenceType, int count)
            {
                lock (m_References)
                {
                    if (count > m_References.Count)
                    {
                        count = m_References.Count;
                    }

                    m_RemoveReferenceCount += count;
                    while (count-- > 0)
                    {
                        m_References.Dequeue();
                    }
                }
            }

            public void RemoveAll()
            {
                lock (m_References)
                {
                    m_RemoveReferenceCount += m_References.Count;
                    m_References.Clear();
                }
            }
        }
    }
}
