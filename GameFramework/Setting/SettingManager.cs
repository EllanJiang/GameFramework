//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System;

namespace GameFramework.Setting
{
    /// <summary>
    /// 配置管理器。
    /// </summary>
    internal sealed class SettingManager : GameFrameworkModule, ISettingManager
    {
        private ISettingHelper m_SettingHelper;

        /// <summary>
        /// 初始化配置管理器的新实例。
        /// </summary>
        public SettingManager()
        {
            m_SettingHelper = null;
        }

        /// <summary>
        /// 配置管理器轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {

        }

        /// <summary>
        /// 关闭并清理配置管理器。
        /// </summary>
        internal override void Shutdown()
        {

        }

        /// <summary>
        /// 设置配置管理器辅助器。
        /// </summary>
        /// <param name="settingHelper">配置管理器辅助器。</param>
        public void SetSettingHelper(ISettingHelper settingHelper)
        {
            if (settingHelper == null)
            {
                throw new GameFrameworkException("Setting helper is invalid.");
            }

            m_SettingHelper = settingHelper;
        }

        /// <summary>
        /// 加载配置。
        /// </summary>
        /// <returns>是否加载配置成功。</returns>
        public bool Load()
        {
            return m_SettingHelper.Load();
        }

        /// <summary>
        /// 保存配置。
        /// </summary>
        /// <returns>是否保存配置成功。</returns>
        public bool Save()
        {
            return m_SettingHelper.Save();
        }

        /// <summary>
        /// 检查是否存在指定配置项。
        /// </summary>
        /// <param name="key">要检查配置项的名称。</param>
        /// <returns>指定的配置项是否存在。</returns>
        public bool HasKey(string key)
        {
            return m_SettingHelper.HasKey(key);
        }

        /// <summary>
        /// 移除指定配置项。
        /// </summary>
        /// <param name="key">要移除配置项的名称。</param>
        public void RemoveKey(string key)
        {
            m_SettingHelper.RemoveKey(key);
        }

        /// <summary>
        /// 清空所有配置项。
        /// </summary>
        public void RemoveAllKeys()
        {
            m_SettingHelper.RemoveAllKeys();
        }

        /// <summary>
        /// 从指定配置项中读取布尔值。
        /// </summary>
        /// <param name="key">要获取配置项的名称。</param>
        /// <returns>读取的布尔值。</returns>
        public bool GetBool(string key)
        {
            return m_SettingHelper.GetBool(key);
        }

        /// <summary>
        /// 从指定配置项中读取布尔值。
        /// </summary>
        /// <param name="key">要获取配置项的名称。</param>
        /// <param name="defaultValue">当指定的配置项不存在时，返回此默认值。</param>
        /// <returns>读取的布尔值。</returns>
        public bool GetBool(string key, bool defaultValue)
        {
            return m_SettingHelper.GetBool(key, defaultValue);
        }

        /// <summary>
        /// 向指定配置项写入布尔值。
        /// </summary>
        /// <param name="key">要写入配置项的名称。</param>
        /// <param name="value">要写入的布尔值。</param>
        public void SetBool(string key, bool value)
        {
            m_SettingHelper.SetBool(key, value);
        }

        /// <summary>
        /// 从指定配置项中读取整数值。
        /// </summary>
        /// <param name="key">要获取配置项的名称。</param>
        /// <returns>读取的整数值。</returns>
        public int GetInt(string key)
        {
            return m_SettingHelper.GetInt(key);
        }

        /// <summary>
        /// 从指定配置项中读取整数值。
        /// </summary>
        /// <param name="key">要获取配置项的名称。</param>
        /// <param name="defaultValue">当指定的配置项不存在时，返回此默认值。</param>
        /// <returns>读取的整数值。</returns>
        public int GetInt(string key, int defaultValue)
        {
            return m_SettingHelper.GetInt(key, defaultValue);
        }

        /// <summary>
        /// 向指定配置项写入整数值。
        /// </summary>
        /// <param name="key">要写入配置项的名称。</param>
        /// <param name="value">要写入的整数值。</param>
        public void SetInt(string key, int value)
        {
            m_SettingHelper.SetInt(key, value);
        }

        /// <summary>
        /// 从指定配置项中读取浮点数值。
        /// </summary>
        /// <param name="key">要获取配置项的名称。</param>
        /// <returns>读取的浮点数值。</returns>
        public float GetFloat(string key)
        {
            return m_SettingHelper.GetFloat(key);
        }

        /// <summary>
        /// 从指定配置项中读取浮点数值。
        /// </summary>
        /// <param name="key">要获取配置项的名称。</param>
        /// <param name="defaultValue">当指定的配置项不存在时，返回此默认值。</param>
        /// <returns>读取的浮点数值。</returns>
        public float GetFloat(string key, float defaultValue)
        {
            return m_SettingHelper.GetFloat(key, defaultValue);
        }

        /// <summary>
        /// 向指定配置项写入浮点数值。
        /// </summary>
        /// <param name="key">要写入配置项的名称。</param>
        /// <param name="value">要写入的浮点数值。</param>
        public void SetFloat(string key, float value)
        {
            m_SettingHelper.SetFloat(key, value);
        }

        /// <summary>
        /// 从指定配置项中读取字符串值。
        /// </summary>
        /// <param name="key">要获取配置项的名称。</param>
        /// <returns>读取的字符串值。</returns>
        public string GetString(string key)
        {
            return m_SettingHelper.GetString(key);
        }

        /// <summary>
        /// 从指定配置项中读取字符串值。
        /// </summary>
        /// <param name="key">要获取配置项的名称。</param>
        /// <param name="defaultValue">当指定的配置项不存在时，返回此默认值。</param>
        /// <returns>读取的字符串值。</returns>
        public string GetString(string key, string defaultValue)
        {
            return m_SettingHelper.GetString(key, defaultValue);
        }

        /// <summary>
        /// 向指定配置项写入字符串值。
        /// </summary>
        /// <param name="key">要写入配置项的名称。</param>
        /// <param name="value">要写入的字符串值。</param>
        public void SetString(string key, string value)
        {
            m_SettingHelper.SetString(key, value);
        }

        /// <summary>
        /// 从指定配置项中读取对象。
        /// </summary>
        /// <typeparam name="T">要读取对象的类型。</typeparam>
        /// <param name="key">要获取配置项的名称。</param>
        /// <returns>读取的对象。</returns>
        public T GetObject<T>(string key)
        {
            return m_SettingHelper.GetObject<T>(key);
        }

        /// <summary>
        /// 从指定配置项中读取对象。
        /// </summary>
        /// <param name="objectType">要读取对象的类型。</param>
        /// <param name="key">要获取配置项的名称。</param>
        /// <returns></returns>
        public object GetObject(Type objectType, string key)
        {
            if (objectType == null)
            {
                throw new GameFrameworkException("Object type is invalid.");
            }

            return m_SettingHelper.GetObject(objectType, key);
        }

        /// <summary>
        /// 从指定配置项中读取对象。
        /// </summary>
        /// <typeparam name="T">要读取对象的类型。</typeparam>
        /// <param name="key">要获取配置项的名称。</param>
        /// <param name="defaultObj">当指定的配置项不存在时，返回此默认对象。</param>
        /// <returns>读取的对象。</returns>
        public T GetObject<T>(string key, T defaultObj)
        {
            return m_SettingHelper.GetObject(key, defaultObj);
        }

        /// <summary>
        /// 从指定配置项中读取对象。
        /// </summary>
        /// <param name="objectType">要读取对象的类型。</param>
        /// <param name="key">要获取配置项的名称。</param>
        /// <param name="defaultObj">当指定的配置项不存在时，返回此默认对象。</param>
        /// <returns></returns>
        public object GetObject(Type objectType, string key, object defaultObj)
        {
            if (objectType == null)
            {
                throw new GameFrameworkException("Object type is invalid.");
            }

            return m_SettingHelper.GetObject(objectType, key, defaultObj);
        }

        /// <summary>
        /// 向指定配置项写入对象。
        /// </summary>
        /// <typeparam name="T">要写入对象的类型。</typeparam>
        /// <param name="key">要写入配置项的名称。</param>
        /// <param name="obj">要写入的对象。</param>
        public void SetObject<T>(string key, T obj)
        {
            m_SettingHelper.SetObject(key, obj);
        }

        /// <summary>
        /// 向指定配置项写入对象。
        /// </summary>
        /// <param name="key">要写入配置项的名称。</param>
        /// <param name="obj">要写入的对象。</param>
        public void SetObject(string key, object obj)
        {
            m_SettingHelper.SetObject(key, obj);
        }
    }
}
