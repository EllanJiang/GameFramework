//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace GameFramework.ObjectPool
{
    /// <summary>
    /// 对象池管理器。
    /// </summary>
    public interface IObjectPoolManager
    {
        /// <summary>
        /// 获取对象池数量。
        /// </summary>
        int Count
        {
            get;
        }

        /// <summary>
        /// 检查是否存在对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <returns>是否存在对象池。</returns>
        bool HasObjectPool<T>() where T : ObjectBase;

        /// <summary>
        /// 检查是否存在对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <returns>是否存在对象池。</returns>
        bool HasObjectPool(Type objectType);

        /// <summary>
        /// 检查是否存在对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="name">对象池名称。</param>
        /// <returns>是否存在对象池。</returns>
        bool HasObjectPool<T>(string name) where T : ObjectBase;

        /// <summary>
        /// 检查是否存在对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="name">对象池名称。</param>
        /// <returns>是否存在对象池。</returns>
        bool HasObjectPool(Type objectType, string name);

        /// <summary>
        /// 检查是否存在对象池。
        /// </summary>
        /// <param name="condition">要检查的条件。</param>
        /// <returns>是否存在对象池。</returns>
        bool HasObjectPool(Predicate<ObjectPoolBase> condition);

        /// <summary>
        /// 获取对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <returns>要获取的对象池。</returns>
        IObjectPool<T> GetObjectPool<T>() where T : ObjectBase;

        /// <summary>
        /// 获取对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <returns>要获取的对象池。</returns>
        ObjectPoolBase GetObjectPool(Type objectType);

        /// <summary>
        /// 获取对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="name">对象池名称。</param>
        /// <returns>要获取的对象池。</returns>
        IObjectPool<T> GetObjectPool<T>(string name) where T : ObjectBase;

        /// <summary>
        /// 获取对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="name">对象池名称。</param>
        /// <returns>要获取的对象池。</returns>
        ObjectPoolBase GetObjectPool(Type objectType, string name);

        /// <summary>
        /// 获取对象池。
        /// </summary>
        /// <param name="condition">要检查的条件。</param>
        /// <returns>要获取的对象池。</returns>
        ObjectPoolBase GetObjectPool(Predicate<ObjectPoolBase> condition);

        /// <summary>
        /// 获取对象池。
        /// </summary>
        /// <param name="condition">要检查的条件。</param>
        /// <returns>要获取的对象池。</returns>
        ObjectPoolBase[] GetObjectPools(Predicate<ObjectPoolBase> condition);

        /// <summary>
        /// 获取对象池。
        /// </summary>
        /// <param name="condition">要检查的条件。</param>
        /// <param name="results">要获取的对象池。</param>
        void GetObjectPools(Predicate<ObjectPoolBase> condition, List<ObjectPoolBase> results);

        /// <summary>
        /// 获取所有对象池。
        /// </summary>
        /// <returns>所有对象池。</returns>
        ObjectPoolBase[] GetAllObjectPools();

        /// <summary>
        /// 获取所有对象池。
        /// </summary>
        /// <param name="results">所有对象池。</param>
        void GetAllObjectPools(List<ObjectPoolBase> results);

        /// <summary>
        /// 获取所有对象池。
        /// </summary>
        /// <param name="sort">是否根据对象池的优先级排序。</param>
        /// <returns>所有对象池。</returns>
        ObjectPoolBase[] GetAllObjectPools(bool sort);

        /// <summary>
        /// 获取所有对象池。
        /// </summary>
        /// <param name="sort">是否根据对象池的优先级排序。</param>
        /// <param name="results">所有对象池。</param>
        void GetAllObjectPools(bool sort, List<ObjectPoolBase> results);

        /// <summary>
        /// 创建允许单次获取的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <returns>要创建的允许单次获取的对象池。</returns>
        IObjectPool<T> CreateSingleSpawnObjectPool<T>() where T : ObjectBase;

        /// <summary>
        /// 创建允许单次获取的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <returns>要创建的允许单次获取的对象池。</returns>
        ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType);

        /// <summary>
        /// 创建允许单次获取的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="name">对象池名称。</param>
        /// <returns>要创建的允许单次获取的对象池。</returns>
        IObjectPool<T> CreateSingleSpawnObjectPool<T>(string name) where T : ObjectBase;

        /// <summary>
        /// 创建允许单次获取的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="name">对象池名称。</param>
        /// <returns>要创建的允许单次获取的对象池。</returns>
        ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, string name);

        /// <summary>
        /// 创建允许单次获取的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="capacity">对象池的容量。</param>
        /// <returns>要创建的允许单次获取的对象池。</returns>
        IObjectPool<T> CreateSingleSpawnObjectPool<T>(int capacity) where T : ObjectBase;

        /// <summary>
        /// 创建允许单次获取的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="capacity">对象池的容量。</param>
        /// <returns>要创建的允许单次获取的对象池。</returns>
        ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, int capacity);

        /// <summary>
        /// 创建允许单次获取的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <returns>要创建的允许单次获取的对象池。</returns>
        IObjectPool<T> CreateSingleSpawnObjectPool<T>(float expireTime) where T : ObjectBase;

        /// <summary>
        /// 创建允许单次获取的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <returns>要创建的允许单次获取的对象池。</returns>
        ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, float expireTime);

        /// <summary>
        /// 创建允许单次获取的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="name">对象池名称。</param>
        /// <param name="capacity">对象池的容量。</param>
        /// <returns>要创建的允许单次获取的对象池。</returns>
        IObjectPool<T> CreateSingleSpawnObjectPool<T>(string name, int capacity) where T : ObjectBase;

        /// <summary>
        /// 创建允许单次获取的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="name">对象池名称。</param>
        /// <param name="capacity">对象池的容量。</param>
        /// <returns>要创建的允许单次获取的对象池。</returns>
        ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, string name, int capacity);

        /// <summary>
        /// 创建允许单次获取的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="name">对象池名称。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <returns>要创建的允许单次获取的对象池。</returns>
        IObjectPool<T> CreateSingleSpawnObjectPool<T>(string name, float expireTime) where T : ObjectBase;

        /// <summary>
        /// 创建允许单次获取的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="name">对象池名称。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <returns>要创建的允许单次获取的对象池。</returns>
        ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, string name, float expireTime);

        /// <summary>
        /// 创建允许单次获取的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="capacity">对象池的容量。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <returns>要创建的允许单次获取的对象池。</returns>
        IObjectPool<T> CreateSingleSpawnObjectPool<T>(int capacity, float expireTime) where T : ObjectBase;

        /// <summary>
        /// 创建允许单次获取的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="capacity">对象池的容量。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <returns>要创建的允许单次获取的对象池。</returns>
        ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, int capacity, float expireTime);

        /// <summary>
        /// 创建允许单次获取的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="capacity">对象池的容量。</param>
        /// <param name="priority">对象池的优先级。</param>
        /// <returns>要创建的允许单次获取的对象池。</returns>
        IObjectPool<T> CreateSingleSpawnObjectPool<T>(int capacity, int priority) where T : ObjectBase;

        /// <summary>
        /// 创建允许单次获取的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="capacity">对象池的容量。</param>
        /// <param name="priority">对象池的优先级。</param>
        /// <returns>要创建的允许单次获取的对象池。</returns>
        ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, int capacity, int priority);

        /// <summary>
        /// 创建允许单次获取的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <param name="priority">对象池的优先级。</param>
        /// <returns>要创建的允许单次获取的对象池。</returns>
        IObjectPool<T> CreateSingleSpawnObjectPool<T>(float expireTime, int priority) where T : ObjectBase;

        /// <summary>
        /// 创建允许单次获取的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <param name="priority">对象池的优先级。</param>
        /// <returns>要创建的允许单次获取的对象池。</returns>
        ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, float expireTime, int priority);

        /// <summary>
        /// 创建允许单次获取的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="name">对象池名称。</param>
        /// <param name="capacity">对象池的容量。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <returns>要创建的允许单次获取的对象池。</returns>
        IObjectPool<T> CreateSingleSpawnObjectPool<T>(string name, int capacity, float expireTime) where T : ObjectBase;

        /// <summary>
        /// 创建允许单次获取的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="name">对象池名称。</param>
        /// <param name="capacity">对象池的容量。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <returns>要创建的允许单次获取的对象池。</returns>
        ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, string name, int capacity, float expireTime);

        /// <summary>
        /// 创建允许单次获取的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="name">对象池名称。</param>
        /// <param name="capacity">对象池的容量。</param>
        /// <param name="priority">对象池的优先级。</param>
        /// <returns>要创建的允许单次获取的对象池。</returns>
        IObjectPool<T> CreateSingleSpawnObjectPool<T>(string name, int capacity, int priority) where T : ObjectBase;

        /// <summary>
        /// 创建允许单次获取的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="name">对象池名称。</param>
        /// <param name="capacity">对象池的容量。</param>
        /// <param name="priority">对象池的优先级。</param>
        /// <returns>要创建的允许单次获取的对象池。</returns>
        ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, string name, int capacity, int priority);

        /// <summary>
        /// 创建允许单次获取的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="name">对象池名称。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <param name="priority">对象池的优先级。</param>
        /// <returns>要创建的允许单次获取的对象池。</returns>
        IObjectPool<T> CreateSingleSpawnObjectPool<T>(string name, float expireTime, int priority) where T : ObjectBase;

        /// <summary>
        /// 创建允许单次获取的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="name">对象池名称。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <param name="priority">对象池的优先级。</param>
        /// <returns>要创建的允许单次获取的对象池。</returns>
        ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, string name, float expireTime, int priority);

        /// <summary>
        /// 创建允许单次获取的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="capacity">对象池的容量。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <param name="priority">对象池的优先级。</param>
        /// <returns>要创建的允许单次获取的对象池。</returns>
        IObjectPool<T> CreateSingleSpawnObjectPool<T>(int capacity, float expireTime, int priority) where T : ObjectBase;

        /// <summary>
        /// 创建允许单次获取的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="capacity">对象池的容量。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <param name="priority">对象池的优先级。</param>
        /// <returns>要创建的允许单次获取的对象池。</returns>
        ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, int capacity, float expireTime, int priority);

        /// <summary>
        /// 创建允许单次获取的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="name">对象池名称。</param>
        /// <param name="capacity">对象池的容量。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <param name="priority">对象池的优先级。</param>
        /// <returns>要创建的允许单次获取的对象池。</returns>
        IObjectPool<T> CreateSingleSpawnObjectPool<T>(string name, int capacity, float expireTime, int priority) where T : ObjectBase;

        /// <summary>
        /// 创建允许单次获取的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="name">对象池名称。</param>
        /// <param name="capacity">对象池的容量。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <param name="priority">对象池的优先级。</param>
        /// <returns>要创建的允许单次获取的对象池。</returns>
        ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, string name, int capacity, float expireTime, int priority);

        /// <summary>
        /// 创建允许单次获取的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="name">对象池名称。</param>
        /// <param name="autoReleaseInterval">对象池自动释放可释放对象的间隔秒数。</param>
        /// <param name="capacity">对象池的容量。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <param name="priority">对象池的优先级。</param>
        /// <returns>要创建的允许单次获取的对象池。</returns>
        IObjectPool<T> CreateSingleSpawnObjectPool<T>(string name, float autoReleaseInterval, int capacity, float expireTime, int priority) where T : ObjectBase;

        /// <summary>
        /// 创建允许单次获取的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="name">对象池名称。</param>
        /// <param name="autoReleaseInterval">对象池自动释放可释放对象的间隔秒数。</param>
        /// <param name="capacity">对象池的容量。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <param name="priority">对象池的优先级。</param>
        /// <returns>要创建的允许单次获取的对象池。</returns>
        ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, string name, float autoReleaseInterval, int capacity, float expireTime, int priority);

        /// <summary>
        /// 创建允许多次获取的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <returns>要创建的允许多次获取的对象池。</returns>
        IObjectPool<T> CreateMultiSpawnObjectPool<T>() where T : ObjectBase;

        /// <summary>
        /// 创建允许多次获取的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <returns>要创建的允许多次获取的对象池。</returns>
        ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType);

        /// <summary>
        /// 创建允许多次获取的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="name">对象池名称。</param>
        /// <returns>要创建的允许多次获取的对象池。</returns>
        IObjectPool<T> CreateMultiSpawnObjectPool<T>(string name) where T : ObjectBase;

        /// <summary>
        /// 创建允许多次获取的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="name">对象池名称。</param>
        /// <returns>要创建的允许多次获取的对象池。</returns>
        ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, string name);

        /// <summary>
        /// 创建允许多次获取的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="capacity">对象池的容量。</param>
        /// <returns>要创建的允许多次获取的对象池。</returns>
        IObjectPool<T> CreateMultiSpawnObjectPool<T>(int capacity) where T : ObjectBase;

        /// <summary>
        /// 创建允许多次获取的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="capacity">对象池的容量。</param>
        /// <returns>要创建的允许多次获取的对象池。</returns>
        ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, int capacity);

        /// <summary>
        /// 创建允许多次获取的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <returns>要创建的允许多次获取的对象池。</returns>
        IObjectPool<T> CreateMultiSpawnObjectPool<T>(float expireTime) where T : ObjectBase;

        /// <summary>
        /// 创建允许多次获取的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <returns>要创建的允许多次获取的对象池。</returns>
        ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, float expireTime);

        /// <summary>
        /// 创建允许多次获取的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="name">对象池名称。</param>
        /// <param name="capacity">对象池的容量。</param>
        /// <returns>要创建的允许多次获取的对象池。</returns>
        IObjectPool<T> CreateMultiSpawnObjectPool<T>(string name, int capacity) where T : ObjectBase;

        /// <summary>
        /// 创建允许多次获取的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="name">对象池名称。</param>
        /// <param name="capacity">对象池的容量。</param>
        /// <returns>要创建的允许多次获取的对象池。</returns>
        ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, string name, int capacity);

        /// <summary>
        /// 创建允许多次获取的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="name">对象池名称。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <returns>要创建的允许多次获取的对象池。</returns>
        IObjectPool<T> CreateMultiSpawnObjectPool<T>(string name, float expireTime) where T : ObjectBase;

        /// <summary>
        /// 创建允许多次获取的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="name">对象池名称。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <returns>要创建的允许多次获取的对象池。</returns>
        ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, string name, float expireTime);

        /// <summary>
        /// 创建允许多次获取的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="capacity">对象池的容量。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <returns>要创建的允许多次获取的对象池。</returns>
        IObjectPool<T> CreateMultiSpawnObjectPool<T>(int capacity, float expireTime) where T : ObjectBase;

        /// <summary>
        /// 创建允许多次获取的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="capacity">对象池的容量。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <returns>要创建的允许多次获取的对象池。</returns>
        ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, int capacity, float expireTime);

        /// <summary>
        /// 创建允许多次获取的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="capacity">对象池的容量。</param>
        /// <param name="priority">对象池的优先级。</param>
        /// <returns>要创建的允许多次获取的对象池。</returns>
        IObjectPool<T> CreateMultiSpawnObjectPool<T>(int capacity, int priority) where T : ObjectBase;

        /// <summary>
        /// 创建允许多次获取的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="capacity">对象池的容量。</param>
        /// <param name="priority">对象池的优先级。</param>
        /// <returns>要创建的允许多次获取的对象池。</returns>
        ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, int capacity, int priority);

        /// <summary>
        /// 创建允许多次获取的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <param name="priority">对象池的优先级。</param>
        /// <returns>要创建的允许多次获取的对象池。</returns>
        IObjectPool<T> CreateMultiSpawnObjectPool<T>(float expireTime, int priority) where T : ObjectBase;

        /// <summary>
        /// 创建允许多次获取的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <param name="priority">对象池的优先级。</param>
        /// <returns>要创建的允许多次获取的对象池。</returns>
        ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, float expireTime, int priority);

        /// <summary>
        /// 创建允许多次获取的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="name">对象池名称。</param>
        /// <param name="capacity">对象池的容量。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <returns>要创建的允许多次获取的对象池。</returns>
        IObjectPool<T> CreateMultiSpawnObjectPool<T>(string name, int capacity, float expireTime) where T : ObjectBase;

        /// <summary>
        /// 创建允许多次获取的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="name">对象池名称。</param>
        /// <param name="capacity">对象池的容量。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <returns>要创建的允许多次获取的对象池。</returns>
        ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, string name, int capacity, float expireTime);

        /// <summary>
        /// 创建允许多次获取的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="name">对象池名称。</param>
        /// <param name="capacity">对象池的容量。</param>
        /// <param name="priority">对象池的优先级。</param>
        /// <returns>要创建的允许多次获取的对象池。</returns>
        IObjectPool<T> CreateMultiSpawnObjectPool<T>(string name, int capacity, int priority) where T : ObjectBase;

        /// <summary>
        /// 创建允许多次获取的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="name">对象池名称。</param>
        /// <param name="capacity">对象池的容量。</param>
        /// <param name="priority">对象池的优先级。</param>
        /// <returns>要创建的允许多次获取的对象池。</returns>
        ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, string name, int capacity, int priority);

        /// <summary>
        /// 创建允许多次获取的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="name">对象池名称。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <param name="priority">对象池的优先级。</param>
        /// <returns>要创建的允许多次获取的对象池。</returns>
        IObjectPool<T> CreateMultiSpawnObjectPool<T>(string name, float expireTime, int priority) where T : ObjectBase;

        /// <summary>
        /// 创建允许多次获取的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="name">对象池名称。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <param name="priority">对象池的优先级。</param>
        /// <returns>要创建的允许多次获取的对象池。</returns>
        ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, string name, float expireTime, int priority);

        /// <summary>
        /// 创建允许多次获取的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="capacity">对象池的容量。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <param name="priority">对象池的优先级。</param>
        /// <returns>要创建的允许多次获取的对象池。</returns>
        IObjectPool<T> CreateMultiSpawnObjectPool<T>(int capacity, float expireTime, int priority) where T : ObjectBase;

        /// <summary>
        /// 创建允许多次获取的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="capacity">对象池的容量。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <param name="priority">对象池的优先级。</param>
        /// <returns>要创建的允许多次获取的对象池。</returns>
        ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, int capacity, float expireTime, int priority);

        /// <summary>
        /// 创建允许多次获取的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="name">对象池名称。</param>
        /// <param name="capacity">对象池的容量。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <param name="priority">对象池的优先级。</param>
        /// <returns>要创建的允许多次获取的对象池。</returns>
        IObjectPool<T> CreateMultiSpawnObjectPool<T>(string name, int capacity, float expireTime, int priority) where T : ObjectBase;

        /// <summary>
        /// 创建允许多次获取的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="name">对象池名称。</param>
        /// <param name="capacity">对象池的容量。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <param name="priority">对象池的优先级。</param>
        /// <returns>要创建的允许多次获取的对象池。</returns>
        ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, string name, int capacity, float expireTime, int priority);

        /// <summary>
        /// 创建允许多次获取的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="name">对象池名称。</param>
        /// <param name="autoReleaseInterval">对象池自动释放可释放对象的间隔秒数。</param>
        /// <param name="capacity">对象池的容量。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <param name="priority">对象池的优先级。</param>
        /// <returns>要创建的允许多次获取的对象池。</returns>
        IObjectPool<T> CreateMultiSpawnObjectPool<T>(string name, float autoReleaseInterval, int capacity, float expireTime, int priority) where T : ObjectBase;

        /// <summary>
        /// 创建允许多次获取的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="name">对象池名称。</param>
        /// <param name="autoReleaseInterval">对象池自动释放可释放对象的间隔秒数。</param>
        /// <param name="capacity">对象池的容量。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <param name="priority">对象池的优先级。</param>
        /// <returns>要创建的允许多次获取的对象池。</returns>
        ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, string name, float autoReleaseInterval, int capacity, float expireTime, int priority);

        /// <summary>
        /// 销毁对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <returns>是否销毁对象池成功。</returns>
        bool DestroyObjectPool<T>() where T : ObjectBase;

        /// <summary>
        /// 销毁对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <returns>是否销毁对象池成功。</returns>
        bool DestroyObjectPool(Type objectType);

        /// <summary>
        /// 销毁对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="name">要销毁的对象池名称。</param>
        /// <returns>是否销毁对象池成功。</returns>
        bool DestroyObjectPool<T>(string name) where T : ObjectBase;

        /// <summary>
        /// 销毁对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="name">要销毁的对象池名称。</param>
        /// <returns>是否销毁对象池成功。</returns>
        bool DestroyObjectPool(Type objectType, string name);

        /// <summary>
        /// 销毁对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="objectPool">要销毁的对象池。</param>
        /// <returns>是否销毁对象池成功。</returns>
        bool DestroyObjectPool<T>(IObjectPool<T> objectPool) where T : ObjectBase;

        /// <summary>
        /// 销毁对象池。
        /// </summary>
        /// <param name="objectPool">要销毁的对象池。</param>
        /// <returns>是否销毁对象池成功。</returns>
        bool DestroyObjectPool(ObjectPoolBase objectPool);

        /// <summary>
        /// 释放对象池中的可释放对象。
        /// </summary>
        void Release();

        /// <summary>
        /// 释放对象池中的所有未使用对象。
        /// </summary>
        void ReleaseAllUnused();
    }
}
