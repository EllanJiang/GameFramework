using System;
using System.Collections.Generic;

namespace GameFramework
{
    /// <summary>
    /// 引用池。
    /// </summary>
    public static class ReferencePool
    {
        private static readonly IDictionary<string, Queue<IReference>> s_ReferencePool = new Dictionary<string, Queue<IReference>>();

        /// <summary>
        /// 清除所有引用池。
        /// </summary>
        public static void ClearAll()
        {
            lock (s_ReferencePool)
            {
                s_ReferencePool.Clear();
            }
        }

        /// <summary>
        /// 清除引用池。
        /// </summary>
        /// <typeparam name="T">引用类型。</typeparam>
        public static void Clear<T>() where T : class, IReference
        {
            lock (s_ReferencePool)
            {
                GetReferencePool(typeof(T).FullName).Clear();
            }
        }

        /// <summary>
        /// 清除引用池。
        /// </summary>
        /// <param name="referenceType">引用类型。</param>
        public static void Clear(Type referenceType)
        {
            if (referenceType == null)
            {
                throw new GameFrameworkException("Reference type is invalid.");
            }

            if (!referenceType.IsClass || referenceType.IsAbstract)
            {
                throw new GameFrameworkException("Reference type is not a non-abstract class type.");
            }

            if (!typeof(IReference).IsAssignableFrom(referenceType))
            {
                throw new GameFrameworkException(string.Format("Reference type '{0}' is invalid.", referenceType.FullName));
            }

            lock (s_ReferencePool)
            {
                GetReferencePool(referenceType.FullName).Clear();
            }
        }

        /// <summary>
        /// 获取引用池的数量。
        /// </summary>
        /// <returns>引用池的数量。</returns>
        public static int Count()
        {
            lock (s_ReferencePool)
            {
                return s_ReferencePool.Count;
            }
        }

        /// <summary>
        /// 获取引用池中引用的数量。
        /// </summary>
        /// <typeparam name="T">引用类型。</typeparam>
        /// <returns>引用池中引用的数量。</returns>
        public static int Count<T>()
        {
            lock (s_ReferencePool)
            {
                return GetReferencePool(typeof(T).FullName).Count;
            }
        }

        /// <summary>
        /// 获取引用池中引用的数量。
        /// </summary>
        /// <param name="referenceType">引用类型。</param>
        /// <returns>引用池中引用的数量。</returns>
        public static int Count(Type referenceType)
        {
            if (referenceType == null)
            {
                throw new GameFrameworkException("Reference type is invalid.");
            }

            if (!referenceType.IsClass || referenceType.IsAbstract)
            {
                throw new GameFrameworkException("Reference type is not a non-abstract class type.");
            }

            if (!typeof(IReference).IsAssignableFrom(referenceType))
            {
                throw new GameFrameworkException(string.Format("Reference type '{0}' is invalid.", referenceType.FullName));
            }

            lock (s_ReferencePool)
            {
                return GetReferencePool(referenceType.FullName).Count;
            }
        }

        /// <summary>
        /// 从引用池获取引用。
        /// </summary>
        /// <typeparam name="T">引用类型。</typeparam>
        public static T Acquire<T>() where T : class, IReference, new()
        {
            lock (s_ReferencePool)
            {
                Queue<IReference> referencePool = GetReferencePool(typeof(T).FullName);
                if (referencePool.Count > 0)
                {
                    return (T)referencePool.Dequeue();
                }
            }

            return new T();
        }

        /// <summary>
        /// 从引用池获取引用。
        /// </summary>
        /// <param name="referenceType">引用类型。</param>
        /// <returns></returns>
        public static IReference Acquire(Type referenceType)
        {
            if (referenceType == null)
            {
                throw new GameFrameworkException("Reference type is invalid.");
            }

            if (!referenceType.IsClass || referenceType.IsAbstract)
            {
                throw new GameFrameworkException("Reference type is not a non-abstract class type.");
            }

            if (!typeof(IReference).IsAssignableFrom(referenceType))
            {
                throw new GameFrameworkException(string.Format("Reference type '{0}' is invalid.", referenceType.FullName));
            }

            lock (s_ReferencePool)
            {
                Queue<IReference> referencePool = GetReferencePool(referenceType.FullName);
                if (referencePool.Count > 0)
                {
                    return referencePool.Dequeue();
                }
            }

            return (IReference)Activator.CreateInstance(referenceType);
        }

        /// <summary>
        /// 将引用归还引用池。
        /// </summary>
        /// <typeparam name="T">引用类型。</typeparam>
        /// <param name="reference">引用。</param>
        public static void Release<T>(T reference) where T : class, IReference
        {
            if (reference == null)
            {
                throw new GameFrameworkException("Reference is invalid.");
            }

            reference.Clear();
            lock (s_ReferencePool)
            {
                GetReferencePool(typeof(T).FullName).Enqueue(reference);
            }
        }

        /// <summary>
        /// 将引用归还引用池。
        /// </summary>
        /// <param name="referenceType">引用类型。</param>
        /// <param name="reference">引用。</param>
        public static void Release(Type referenceType, IReference reference)
        {
            if (referenceType == null)
            {
                throw new GameFrameworkException("Reference type is invalid.");
            }

            if (!referenceType.IsClass || referenceType.IsAbstract)
            {
                throw new GameFrameworkException("Reference type is not a non-abstract class type.");
            }

            if (reference == null)
            {
                throw new GameFrameworkException("Reference is invalid.");
            }

            Type type = reference.GetType();
            if (referenceType != type)
            {
                throw new GameFrameworkException(string.Format("Reference type '{0}' not equals to reference's type '{1}'.", referenceType.FullName, type.FullName));
            }

            reference.Clear();
            lock (s_ReferencePool)
            {
                GetReferencePool(referenceType.FullName).Enqueue(reference);
            }
        }

        /// <summary>
        /// 向引用池中追加指定数量的引用。
        /// </summary>
        /// <typeparam name="T">引用类型。</typeparam>
        /// <param name="count">追加数量。</param>
        public static void Add<T>(int count) where T : class, IReference, new()
        {
            lock (s_ReferencePool)
            {
                Queue<IReference> referencePool = GetReferencePool(typeof(T).FullName);
                while (count-- > 0)
                {
                    referencePool.Enqueue(new T());
                }
            }
        }

        /// <summary>
        /// 向引用池中追加指定数量的引用。
        /// </summary>
        /// <param name="referenceType">引用类型。</param>
        /// <param name="count">追加数量。</param>
        public static void Add(Type referenceType, int count)
        {
            if (referenceType == null)
            {
                throw new GameFrameworkException("Reference type is invalid.");
            }

            if (!referenceType.IsClass || referenceType.IsAbstract)
            {
                throw new GameFrameworkException("Reference type is not a non-abstract class type.");
            }

            if (!typeof(IReference).IsAssignableFrom(referenceType))
            {
                throw new GameFrameworkException(string.Format("Reference type '{0}' is invalid.", referenceType.FullName));
            }

            lock (s_ReferencePool)
            {
                Queue<IReference> referencePool = GetReferencePool(referenceType.FullName);
                while (count-- > 0)
                {
                    referencePool.Enqueue((IReference)Activator.CreateInstance(referenceType));
                }
            }
        }

        /// <summary>
        /// 从引用池中移除指定数量的引用。
        /// </summary>
        /// <typeparam name="T">引用类型。</typeparam>
        /// <param name="count">移除数量。</param>
        public static void Remove<T>(int count) where T : class, IReference
        {
            lock (s_ReferencePool)
            {
                Queue<IReference> referencePool = GetReferencePool(typeof(T).FullName);
                if (referencePool.Count < count)
                {
                    count = referencePool.Count;
                }

                while (count-- > 0)
                {
                    referencePool.Dequeue();
                }
            }
        }

        /// <summary>
        /// 从引用池中移除指定数量的引用。
        /// </summary>
        /// <param name="referenceType">引用类型。</param>
        /// <param name="count">移除数量。</param>
        public static void Remove(Type referenceType, int count)
        {
            if (referenceType == null)
            {
                throw new GameFrameworkException("Reference type is invalid.");
            }

            if (!referenceType.IsClass || referenceType.IsAbstract)
            {
                throw new GameFrameworkException("Reference type is not a non-abstract class type.");
            }

            if (!typeof(IReference).IsAssignableFrom(referenceType))
            {
                throw new GameFrameworkException(string.Format("Reference type '{0}' is invalid.", referenceType.FullName));
            }

            lock (s_ReferencePool)
            {
                Queue<IReference> referencePool = GetReferencePool(referenceType.FullName);
                if (referencePool.Count < count)
                {
                    count = referencePool.Count;
                }

                while (count-- > 0)
                {
                    referencePool.Dequeue();
                }
            }
        }

        private static Queue<IReference> GetReferencePool(string fullName)
        {
            Queue<IReference> referencePool = null;
            if (!s_ReferencePool.TryGetValue(fullName, out referencePool))
            {
                referencePool = new Queue<IReference>();
                s_ReferencePool.Add(fullName, referencePool);
            }

            return referencePool;
        }
    }
}
