//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework.Resource;
using System;
using System.Collections.Generic;

namespace GameFramework.Localization
{
    /// <summary>
    /// 本地化管理器。
    /// </summary>
    internal sealed partial class LocalizationManager : GameFrameworkModule, ILocalizationManager
    {
        private readonly Dictionary<string, string> m_Dictionary;
        private readonly LoadAssetCallbacks m_LoadAssetCallbacks;
        private readonly LoadBinaryCallbacks m_LoadBinaryCallbacks;
        private IResourceManager m_ResourceManager;
        private ILocalizationHelper m_LocalizationHelper;
        private Language m_Language;
        private EventHandler<LoadDictionarySuccessEventArgs> m_LoadDictionarySuccessEventHandler;
        private EventHandler<LoadDictionaryFailureEventArgs> m_LoadDictionaryFailureEventHandler;
        private EventHandler<LoadDictionaryUpdateEventArgs> m_LoadDictionaryUpdateEventHandler;
        private EventHandler<LoadDictionaryDependencyAssetEventArgs> m_LoadDictionaryDependencyAssetEventHandler;

        /// <summary>
        /// 初始化本地化管理器的新实例。
        /// </summary>
        public LocalizationManager()
        {
            m_Dictionary = new Dictionary<string, string>();
            m_LoadAssetCallbacks = new LoadAssetCallbacks(LoadAssetSuccessCallback, LoadAssetOrBinaryFailureCallback, LoadAssetUpdateCallback, LoadAssetDependencyAssetCallback);
            m_LoadBinaryCallbacks = new LoadBinaryCallbacks(LoadBinarySuccessCallback, LoadAssetOrBinaryFailureCallback);
            m_ResourceManager = null;
            m_LocalizationHelper = null;
            m_Language = Language.Unspecified;
            m_LoadDictionarySuccessEventHandler = null;
            m_LoadDictionaryFailureEventHandler = null;
            m_LoadDictionaryUpdateEventHandler = null;
            m_LoadDictionaryDependencyAssetEventHandler = null;
        }

        /// <summary>
        /// 获取或设置本地化语言。
        /// </summary>
        public Language Language
        {
            get
            {
                return m_Language;
            }
            set
            {
                if (value == Language.Unspecified)
                {
                    throw new GameFrameworkException("Language is invalid.");
                }

                m_Language = value;
            }
        }

        /// <summary>
        /// 获取系统语言。
        /// </summary>
        public Language SystemLanguage
        {
            get
            {
                if (m_LocalizationHelper == null)
                {
                    throw new GameFrameworkException("You must set localization helper first.");
                }

                return m_LocalizationHelper.SystemLanguage;
            }
        }

        /// <summary>
        /// 获取字典数量。
        /// </summary>
        public int DictionaryCount
        {
            get
            {
                return m_Dictionary.Count;
            }
        }

        /// <summary>
        /// 加载字典成功事件。
        /// </summary>
        public event EventHandler<LoadDictionarySuccessEventArgs> LoadDictionarySuccess
        {
            add
            {
                m_LoadDictionarySuccessEventHandler += value;
            }
            remove
            {
                m_LoadDictionarySuccessEventHandler -= value;
            }
        }

        /// <summary>
        /// 加载字典失败事件。
        /// </summary>
        public event EventHandler<LoadDictionaryFailureEventArgs> LoadDictionaryFailure
        {
            add
            {
                m_LoadDictionaryFailureEventHandler += value;
            }
            remove
            {
                m_LoadDictionaryFailureEventHandler -= value;
            }
        }

        /// <summary>
        /// 加载字典更新事件。
        /// </summary>
        public event EventHandler<LoadDictionaryUpdateEventArgs> LoadDictionaryUpdate
        {
            add
            {
                m_LoadDictionaryUpdateEventHandler += value;
            }
            remove
            {
                m_LoadDictionaryUpdateEventHandler -= value;
            }
        }

        /// <summary>
        /// 加载字典时加载依赖资源事件。
        /// </summary>
        public event EventHandler<LoadDictionaryDependencyAssetEventArgs> LoadDictionaryDependencyAsset
        {
            add
            {
                m_LoadDictionaryDependencyAssetEventHandler += value;
            }
            remove
            {
                m_LoadDictionaryDependencyAssetEventHandler -= value;
            }
        }

        /// <summary>
        /// 本地化管理器轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {
        }

        /// <summary>
        /// 关闭并清理本地化管理器。
        /// </summary>
        internal override void Shutdown()
        {
        }

        /// <summary>
        /// 设置资源管理器。
        /// </summary>
        /// <param name="resourceManager">资源管理器。</param>
        public void SetResourceManager(IResourceManager resourceManager)
        {
            if (resourceManager == null)
            {
                throw new GameFrameworkException("Resource manager is invalid.");
            }

            m_ResourceManager = resourceManager;
        }

        /// <summary>
        /// 设置本地化辅助器。
        /// </summary>
        /// <param name="localizationHelper">本地化辅助器。</param>
        public void SetLocalizationHelper(ILocalizationHelper localizationHelper)
        {
            if (localizationHelper == null)
            {
                throw new GameFrameworkException("Localization helper is invalid.");
            }

            m_LocalizationHelper = localizationHelper;
        }

        /// <summary>
        /// 加载字典。
        /// </summary>
        /// <param name="dictionaryAssetName">字典资源名称。</param>
        public void LoadDictionary(string dictionaryAssetName)
        {
            LoadDictionary(dictionaryAssetName, Constant.DefaultPriority, null);
        }

        /// <summary>
        /// 加载字典。
        /// </summary>
        /// <param name="dictionaryAssetName">字典资源名称。</param>
        /// <param name="priority">加载字典资源的优先级。</param>
        public void LoadDictionary(string dictionaryAssetName, int priority)
        {
            LoadDictionary(dictionaryAssetName, priority, null);
        }

        /// <summary>
        /// 加载字典。
        /// </summary>
        /// <param name="dictionaryAssetName">字典资源名称。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void LoadDictionary(string dictionaryAssetName, object userData)
        {
            LoadDictionary(dictionaryAssetName, Constant.DefaultPriority, userData);
        }

        /// <summary>
        /// 加载字典。
        /// </summary>
        /// <param name="dictionaryAssetName">字典资源名称。</param>
        /// <param name="priority">加载字典资源的优先级。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void LoadDictionary(string dictionaryAssetName, int priority, object userData)
        {
            if (m_ResourceManager == null)
            {
                throw new GameFrameworkException("You must set resource manager first.");
            }

            if (m_LocalizationHelper == null)
            {
                throw new GameFrameworkException("You must set localization helper first.");
            }

            HasAssetResult result = m_ResourceManager.HasAsset(dictionaryAssetName);
            switch (result)
            {
                case HasAssetResult.Asset:
                    m_ResourceManager.LoadAsset(dictionaryAssetName, priority, m_LoadAssetCallbacks, userData);
                    break;

                case HasAssetResult.Binary:
                    m_ResourceManager.LoadBinary(dictionaryAssetName, m_LoadBinaryCallbacks, userData);
                    break;

                default:
                    throw new GameFrameworkException(Utility.Text.Format("Dictionary asset '{0}' is '{1}'.", dictionaryAssetName, result.ToString()));
            }
        }

        /// <summary>
        /// 解析字典。
        /// </summary>
        /// <param name="dictionaryData">要解析的字典数据。</param>
        /// <returns>是否解析字典成功。</returns>
        public bool ParseDictionary(object dictionaryData)
        {
            return ParseDictionary(dictionaryData, null);
        }

        /// <summary>
        /// 解析字典。
        /// </summary>
        /// <param name="dictionaryData">要解析的字典数据。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否解析字典成功。</returns>
        public bool ParseDictionary(object dictionaryData, object userData)
        {
            if (m_LocalizationHelper == null)
            {
                throw new GameFrameworkException("You must set localization helper first.");
            }

            try
            {
                return m_LocalizationHelper.ParseDictionary(dictionaryData, userData);
            }
            catch (Exception exception)
            {
                if (exception is GameFrameworkException)
                {
                    throw;
                }

                throw new GameFrameworkException(Utility.Text.Format("Can not parse dictionary with exception '{0}'.", exception.ToString()), exception);
            }
        }

        /// <summary>
        /// 根据字典主键获取字典内容字符串。
        /// </summary>
        /// <param name="key">字典主键。</param>
        /// <returns>要获取的字典内容字符串。</returns>
        public string GetString(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new GameFrameworkException("Key is invalid.");
            }

            string value = null;
            if (!m_Dictionary.TryGetValue(key, out value))
            {
                return Utility.Text.Format("<NoKey>{0}", key);
            }

            return value;
        }

        /// <summary>
        /// 根据字典主键获取字典内容字符串。
        /// </summary>
        /// <param name="key">字典主键。</param>
        /// <param name="arg0">字典参数 0。</param>
        /// <returns>要获取的字典内容字符串。</returns>
        public string GetString(string key, object arg0)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new GameFrameworkException("Key is invalid.");
            }

            string value = null;
            if (!m_Dictionary.TryGetValue(key, out value))
            {
                return Utility.Text.Format("<NoKey>{0}", key);
            }

            try
            {
                return Utility.Text.Format(value, arg0);
            }
            catch (Exception exception)
            {
                return Utility.Text.Format("<Error>{0},{1},{2},{3}", key, value, arg0, exception.ToString());
            }
        }

        /// <summary>
        /// 根据字典主键获取字典内容字符串。
        /// </summary>
        /// <param name="key">字典主键。</param>
        /// <param name="arg0">字典参数 0。</param>
        /// <param name="arg1">字典参数 1。</param>
        /// <returns>要获取的字典内容字符串。</returns>
        public string GetString(string key, object arg0, object arg1)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new GameFrameworkException("Key is invalid.");
            }

            string value = null;
            if (!m_Dictionary.TryGetValue(key, out value))
            {
                return Utility.Text.Format("<NoKey>{0}", key);
            }

            try
            {
                return Utility.Text.Format(value, arg0, arg1);
            }
            catch (Exception exception)
            {
                return Utility.Text.Format("<Error>{0},{1},{2},{3},{4}", key, value, arg0, arg1, exception.ToString());
            }
        }

        /// <summary>
        /// 根据字典主键获取字典内容字符串。
        /// </summary>
        /// <param name="key">字典主键。</param>
        /// <param name="arg0">字典参数 0。</param>
        /// <param name="arg1">字典参数 1。</param>
        /// <param name="arg2">字典参数 2。</param>
        /// <returns>要获取的字典内容字符串。</returns>
        public string GetString(string key, object arg0, object arg1, object arg2)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new GameFrameworkException("Key is invalid.");
            }

            string value = null;
            if (!m_Dictionary.TryGetValue(key, out value))
            {
                return Utility.Text.Format("<NoKey>{0}", key);
            }

            try
            {
                return Utility.Text.Format(value, arg0, arg1, arg2);
            }
            catch (Exception exception)
            {
                return Utility.Text.Format("<Error>{0},{1},{2},{3},{4},{5}", key, value, arg0, arg1, arg2, exception.ToString());
            }
        }

        /// <summary>
        /// 根据字典主键获取字典内容字符串。
        /// </summary>
        /// <param name="key">字典主键。</param>
        /// <param name="args">字典参数。</param>
        /// <returns>要获取的字典内容字符串。</returns>
        public string GetString(string key, params object[] args)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new GameFrameworkException("Key is invalid.");
            }

            string value = null;
            if (!m_Dictionary.TryGetValue(key, out value))
            {
                return Utility.Text.Format("<NoKey>{0}", key);
            }

            try
            {
                return Utility.Text.Format(value, args);
            }
            catch (Exception exception)
            {
                string errorString = Utility.Text.Format("<Error>{0},{1}", key, value);
                if (args != null)
                {
                    foreach (object arg in args)
                    {
                        errorString += "," + arg.ToString();
                    }
                }

                errorString += "," + exception.ToString();
                return errorString;
            }
        }

        /// <summary>
        /// 是否存在字典。
        /// </summary>
        /// <param name="key">字典主键。</param>
        /// <returns>是否存在字典。</returns>
        public bool HasRawString(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new GameFrameworkException("Key is invalid.");
            }

            return m_Dictionary.ContainsKey(key);
        }

        /// <summary>
        /// 根据字典主键获取字典值。
        /// </summary>
        /// <param name="key">字典主键。</param>
        /// <returns>字典值。</returns>
        public string GetRawString(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new GameFrameworkException("Key is invalid.");
            }

            string value = null;
            if (m_Dictionary.TryGetValue(key, out value))
            {
                return value;
            }

            return Utility.Text.Format("<NoKey>{0}", key);
        }

        /// <summary>
        /// 增加字典。
        /// </summary>
        /// <param name="key">字典主键。</param>
        /// <param name="value">字典内容。</param>
        /// <returns>是否增加字典成功。</returns>
        public bool AddRawString(string key, string value)
        {
            if (HasRawString(key))
            {
                return false;
            }

            m_Dictionary.Add(key, value ?? string.Empty);
            return true;
        }

        /// <summary>
        /// 移除字典。
        /// </summary>
        /// <param name="key">字典主键。</param>
        /// <returns>是否移除字典成功。</returns>
        public bool RemoveRawString(string key)
        {
            if (!HasRawString(key))
            {
                return false;
            }

            return m_Dictionary.Remove(key);
        }

        /// <summary>
        /// 清空所有字典。
        /// </summary>
        public void RemoveAllRawStrings()
        {
            m_Dictionary.Clear();
        }

        private void LoadAssetSuccessCallback(string dictionaryAssetName, object dictionaryAsset, float duration, object userData)
        {
            try
            {
                if (!m_LocalizationHelper.LoadDictionary(dictionaryAssetName, dictionaryAsset, userData))
                {
                    throw new GameFrameworkException(Utility.Text.Format("Load dictionary failure in helper, asset name '{0}'.", dictionaryAssetName));
                }

                if (m_LoadDictionarySuccessEventHandler != null)
                {
                    LoadDictionarySuccessEventArgs loadDictionarySuccessEventArgs = LoadDictionarySuccessEventArgs.Create(dictionaryAssetName, duration, userData);
                    m_LoadDictionarySuccessEventHandler(this, loadDictionarySuccessEventArgs);
                    ReferencePool.Release(loadDictionarySuccessEventArgs);
                }
            }
            catch (Exception exception)
            {
                if (m_LoadDictionaryFailureEventHandler != null)
                {
                    LoadDictionaryFailureEventArgs loadDictionaryFailureEventArgs = LoadDictionaryFailureEventArgs.Create(dictionaryAssetName, exception.ToString(), userData);
                    m_LoadDictionaryFailureEventHandler(this, loadDictionaryFailureEventArgs);
                    ReferencePool.Release(loadDictionaryFailureEventArgs);
                    return;
                }

                throw;
            }
            finally
            {
                m_LocalizationHelper.ReleaseDictionaryAsset(dictionaryAsset);
            }
        }

        private void LoadAssetOrBinaryFailureCallback(string dictionaryAssetName, LoadResourceStatus status, string errorMessage, object userData)
        {
            string appendErrorMessage = Utility.Text.Format("Load dictionary failure, asset name '{0}', status '{1}', error message '{2}'.", dictionaryAssetName, status.ToString(), errorMessage);
            if (m_LoadDictionaryFailureEventHandler != null)
            {
                LoadDictionaryFailureEventArgs loadDictionaryFailureEventArgs = LoadDictionaryFailureEventArgs.Create(dictionaryAssetName, appendErrorMessage, userData);
                m_LoadDictionaryFailureEventHandler(this, loadDictionaryFailureEventArgs);
                ReferencePool.Release(loadDictionaryFailureEventArgs);
                return;
            }

            throw new GameFrameworkException(appendErrorMessage);
        }

        private void LoadAssetUpdateCallback(string dictionaryAssetName, float progress, object userData)
        {
            if (m_LoadDictionaryUpdateEventHandler != null)
            {
                LoadDictionaryUpdateEventArgs loadDictionaryUpdateEventArgs = LoadDictionaryUpdateEventArgs.Create(dictionaryAssetName, progress, userData);
                m_LoadDictionaryUpdateEventHandler(this, loadDictionaryUpdateEventArgs);
                ReferencePool.Release(loadDictionaryUpdateEventArgs);
            }
        }

        private void LoadAssetDependencyAssetCallback(string dictionaryAssetName, string dependencyAssetName, int loadedCount, int totalCount, object userData)
        {
            if (m_LoadDictionaryDependencyAssetEventHandler != null)
            {
                LoadDictionaryDependencyAssetEventArgs loadDictionaryDependencyAssetEventArgs = LoadDictionaryDependencyAssetEventArgs.Create(dictionaryAssetName, dependencyAssetName, loadedCount, totalCount, userData);
                m_LoadDictionaryDependencyAssetEventHandler(this, loadDictionaryDependencyAssetEventArgs);
                ReferencePool.Release(loadDictionaryDependencyAssetEventArgs);
            }
        }

        private void LoadBinarySuccessCallback(string dictionaryAssetName, byte[] dictionaryBytes, float duration, object userData)
        {
            try
            {
                if (!m_LocalizationHelper.LoadDictionary(dictionaryAssetName, dictionaryBytes, userData))
                {
                    throw new GameFrameworkException(Utility.Text.Format("Load dictionary failure in helper, asset name '{0}'.", dictionaryAssetName));
                }

                if (m_LoadDictionarySuccessEventHandler != null)
                {
                    LoadDictionarySuccessEventArgs loadDictionarySuccessEventArgs = LoadDictionarySuccessEventArgs.Create(dictionaryAssetName, duration, userData);
                    m_LoadDictionarySuccessEventHandler(this, loadDictionarySuccessEventArgs);
                    ReferencePool.Release(loadDictionarySuccessEventArgs);
                }
            }
            catch (Exception exception)
            {
                if (m_LoadDictionaryFailureEventHandler != null)
                {
                    LoadDictionaryFailureEventArgs loadDictionaryFailureEventArgs = LoadDictionaryFailureEventArgs.Create(dictionaryAssetName, exception.ToString(), userData);
                    m_LoadDictionaryFailureEventHandler(this, loadDictionaryFailureEventArgs);
                    ReferencePool.Release(loadDictionaryFailureEventArgs);
                    return;
                }

                throw;
            }
        }
    }
}
