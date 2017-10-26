using System;
using System.Collections.Generic;

namespace GameFramework
{
    /// <summary>
    /// 引用池。
    /// </summary>
    public static class ReferencePool
    {
        private readonly static IDictionary<string, Queue<IReference>> s_ReferencePool = new Dictionary<string, Queue<IReference>>();

        /// <summary>
        /// 清除所有引用池。
        /// </summary>
        public static void ClearAll()
        {
            s_ReferencePool.Clear();
        }

        /// <summary>
        /// 清除引用池。
        /// </summary>
        /// <typeparam name="T">引用类型。</typeparam>
        public static void Clear<T>() where T : class, IReference
        {
            GetReferencePool(typeof(T).FullName).Clear();
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

            GetReferencePool(referenceType.FullName).Clear();
        }

        /// <summary>
        /// 获取引用池中引用的数量。
        /// </summary>
        /// <typeparam name="T">引用类型。</typeparam>
        /// <returns>引用池中引用的数量。</returns>
        public static int Count<T>()
        {
            return GetReferencePool(typeof(T).FullName).Count;
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

            return GetReferencePool(referenceType.FullName).Count;
        }

        /// <summary>
        /// 从引用池获取引用。
        /// </summary>
        /// <typeparam name="T">引用类型。</typeparam>
        public static T Acquire<T>() where T : class, IReference, new()
        {
            Queue<IReference> referencePool = GetReferencePool(typeof(T).FullName);
            if (referencePool.Count > 0)
            {
                return (T)referencePool.Dequeue();
            }
            else
            {
                return new T();
            }
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

            Queue<IReference> referencePool = GetReferencePool(referenceType.FullName);
            if (referencePool.Count > 0)
            {
                return referencePool.Dequeue();
            }
            else
            {
                return (IReference)Activator.CreateInstance(referenceType);
            }
        }

        /// <summary>
        /// 将引用归还引用池。
        /// </summary>
        /// <typeparam name="T">引用类型。</typeparam>
        /// <param name="reference">引用。</param>
        public static void Return<T>(T reference) where T : class, IReference
        {
            if (reference == null)
            {
                throw new GameFrameworkException("Reference is invalid.");
            }

            reference.Clear();
            GetReferencePool(typeof(T).FullName).Enqueue(reference);
        }

        /// <summary>
        /// 将引用归还引用池。
        /// </summary>
        /// <param name="referenceType">引用类型。</param>
        /// <param name="reference">引用。</param>
        public static void Return(Type referenceType, IReference reference)
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
            GetReferencePool(referenceType.FullName).Enqueue(reference);
        }

        /// <summary>
        /// 向引用池中追加指定数量的引用。
        /// </summary>
        /// <typeparam name="T">引用类型。</typeparam>
        /// <param name="count">追加数量。</param>
        public static void Add<T>(int count) where T : class, IReference, new()
        {
            Queue<IReference> referencePool = GetReferencePool(typeof(T).FullName);
            while (count-- > 0)
            {
                referencePool.Enqueue(new T());
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

            Queue<IReference> referencePool = GetReferencePool(referenceType.FullName);
            while (count-- > 0)
            {
                referencePool.Enqueue((IReference)Activator.CreateInstance(referenceType));
            }
        }

        /// <summary>
        /// 从引用池中移除指定数量的引用。
        /// </summary>
        /// <typeparam name="T">引用类型。</typeparam>
        /// <param name="count">移除数量。</param>
        public static void Remove<T>(int count) where T : class, IReference
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
