//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework.Resource;

namespace GameFramework
{
    /// <summary>
    /// 数据提供者创建器。
    /// </summary>
    public static class DataProviderCreator
    {
        /// <summary>
        /// 获取缓冲二进制流的大小。
        /// </summary>
        public static int CachedBytesSize
        {
            get
            {
                return DataProvider<object>.CachedBytesSize;
            }
        }

        /// <summary>
        /// 创建数据提供者。
        /// </summary>
        /// <typeparam name="T">数据提供者的持有者的类型。</typeparam>
        /// <param name="owner">数据提供者的持有者。</param>
        /// <param name="resourceManager">资源管理器。</param>
        /// <param name="dataProviderHelper">数据提供者辅助器。</param>
        /// <returns></returns>
        public static IDataProvider<T> Create<T>(T owner, IResourceManager resourceManager, IDataProviderHelper<T> dataProviderHelper)
        {
            if (owner == null)
            {
                throw new GameFrameworkException("Owner is invalid.");
            }

            if (resourceManager == null)
            {
                throw new GameFrameworkException("Resource manager is invalid.");
            }

            if (dataProviderHelper == null)
            {
                throw new GameFrameworkException("Data provider helper is invalid.");
            }

            DataProvider<T> dataProvider = new DataProvider<T>(owner);
            dataProvider.SetResourceManager(resourceManager);
            dataProvider.SetDataProviderHelper(dataProviderHelper);
            return dataProvider;
        }
    }
}
