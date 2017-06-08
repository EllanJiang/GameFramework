//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.Setting;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 配置组件。
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Game Framework/Setting")]
    public sealed class SettingComponent : GameFrameworkComponent
    {
        private ISettingManager m_SettingManager = null;

        [SerializeField]
        private string m_SettingHelperTypeName = "UnityGameFramework.Runtime.DefaultSettingHelper";

        [SerializeField]
        private SettingHelperBase m_CustomSettingHelper = null;

        /// <summary>
        /// 游戏框架组件初始化。
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            m_SettingManager = GameFrameworkEntry.GetModule<ISettingManager>();
            if (m_SettingManager == null)
            {
                Log.Fatal("Setting manager is invalid.");
                return;
            }

            SettingHelperBase settingHelper = Helper.CreateHelper(m_SettingHelperTypeName, m_CustomSettingHelper);
            if (settingHelper == null)
            {
                Log.Error("Can not create setting helper.");
                return;
            }

            settingHelper.name = string.Format("Setting Helper");
            Transform transform = settingHelper.transform;
            transform.SetParent(this.transform);
            transform.localScale = Vector3.one;

            m_SettingManager.SetSettingHelper(settingHelper);
        }

        private void Start()
        {

        }

        /// <summary>
        /// 保存配置。
        /// </summary>
        public void Save()
        {
            m_SettingManager.Save();
        }

        /// <summary>
        /// 检查是否存在指定配置项。
        /// </summary>
        /// <param name="key">要检查配置项的名称。</param>
        /// <returns>指定的配置项是否存在。</returns>
        public bool HasKey(string key)
        {
            return m_SettingManager.HasKey(key);
        }

        /// <summary>
        /// 移除指定配置项。
        /// </summary>
        /// <param name="key">要移除配置项的名称。</param>
        public void RemoveKey(string key)
        {
            m_SettingManager.RemoveKey(key);
        }

        /// <summary>
        /// 清空所有配置项。
        /// </summary>
        public void RemoveAllKeys()
        {
            m_SettingManager.RemoveAllKeys();
        }

        /// <summary>
        /// 从指定配置项中读取布尔值。
        /// </summary>
        /// <param name="key">要获取配置项的名称。</param>
        /// <returns>读取的布尔值。</returns>
        public bool GetBool(string key)
        {
            return m_SettingManager.GetBool(key);
        }

        /// <summary>
        /// 从指定配置项中读取布尔值。
        /// </summary>
        /// <param name="key">要获取配置项的名称。</param>
        /// <param name="defaultValue">当指定的配置项不存在时，返回此默认值。</param>
        /// <returns>读取的布尔值。</returns>
        public bool GetBool(string key, bool defaultValue)
        {
            return m_SettingManager.GetBool(key, defaultValue);
        }

        /// <summary>
        /// 向指定配置项写入布尔值。
        /// </summary>
        /// <param name="key">要写入配置项的名称。</param>
        /// <param name="value">要写入的布尔值。</param>
        public void SetBool(string key, bool value)
        {
            m_SettingManager.SetBool(key, value);
        }

        /// <summary>
        /// 从指定配置项中读取整数值。
        /// </summary>
        /// <param name="key">要获取配置项的名称。</param>
        /// <returns>读取的整数值。</returns>
        public int GetInt(string key)
        {
            return m_SettingManager.GetInt(key);
        }

        /// <summary>
        /// 从指定配置项中读取整数值。
        /// </summary>
        /// <param name="key">要获取配置项的名称。</param>
        /// <param name="defaultValue">当指定的配置项不存在时，返回此默认值。</param>
        /// <returns>读取的整数值。</returns>
        public int GetInt(string key, int defaultValue)
        {
            return m_SettingManager.GetInt(key, defaultValue);
        }

        /// <summary>
        /// 向指定配置项写入整数值。
        /// </summary>
        /// <param name="key">要写入配置项的名称。</param>
        /// <param name="value">要写入的整数值。</param>
        public void SetInt(string key, int value)
        {
            m_SettingManager.SetInt(key, value);
        }

        /// <summary>
        /// 从指定配置项中读取浮点数值。
        /// </summary>
        /// <param name="key">要获取配置项的名称。</param>
        /// <returns>读取的浮点数值。</returns>
        public float GetFloat(string key)
        {
            return m_SettingManager.GetFloat(key);
        }

        /// <summary>
        /// 从指定配置项中读取浮点数值。
        /// </summary>
        /// <param name="key">要获取配置项的名称。</param>
        /// <param name="defaultValue">当指定的配置项不存在时，返回此默认值。</param>
        /// <returns>读取的浮点数值。</returns>
        public float GetFloat(string key, float defaultValue)
        {
            return m_SettingManager.GetFloat(key, defaultValue);
        }

        /// <summary>
        /// 向指定配置项写入浮点数值。
        /// </summary>
        /// <param name="key">要写入配置项的名称。</param>
        /// <param name="value">要写入的浮点数值。</param>
        public void SetFloat(string key, float value)
        {
            m_SettingManager.SetFloat(key, value);
        }

        /// <summary>
        /// 从指定配置项中读取字符串值。
        /// </summary>
        /// <param name="key">要获取配置项的名称。</param>
        /// <returns>读取的字符串值。</returns>
        public string GetString(string key)
        {
            return m_SettingManager.GetString(key);
        }

        /// <summary>
        /// 从指定配置项中读取字符串值。
        /// </summary>
        /// <param name="key">要获取配置项的名称。</param>
        /// <param name="defaultValue">当指定的配置项不存在时，返回此默认值。</param>
        /// <returns>读取的字符串值。</returns>
        public string GetString(string key, string defaultValue)
        {
            return m_SettingManager.GetString(key, defaultValue);
        }

        /// <summary>
        /// 向指定配置项写入字符串值。
        /// </summary>
        /// <param name="key">要写入配置项的名称。</param>
        /// <param name="value">要写入的字符串值。</param>
        public void SetString(string key, string value)
        {
            m_SettingManager.SetString(key, value);
        }

        /// <summary>
        /// 从指定配置项中读取对象。
        /// </summary>
        /// <typeparam name="T">要读取对象的类型。</typeparam>
        /// <param name="key">要获取配置项的名称。</param>
        /// <returns>读取的对象。</returns>
        public T GetObject<T>(string key)
        {
            return m_SettingManager.GetObject<T>(key);
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
            return m_SettingManager.GetObject<T>(key, defaultObj);
        }

        /// <summary>
        /// 向指定配置项写入对象。
        /// </summary>
        /// <typeparam name="T">要写入对象的类型。</typeparam>
        /// <param name="key">要写入配置项的名称。</param>
        /// <param name="obj">要写入的对象。</param>
        public void SetObject<T>(string key, T obj)
        {
            m_SettingManager.SetObject(key, obj);
        }
    }
}
