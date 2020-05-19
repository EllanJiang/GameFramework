//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GameFramework
{
    /// <summary>
    /// 游戏框架序列化器基类。
    /// </summary>
    /// <typeparam name="T">要序列化的数据类型。</typeparam>
    public abstract class GameFrameworkSerializer<T>
    {
        private readonly Dictionary<byte, SerializeCallback> m_SerializeCallbacks;
        private readonly Dictionary<byte, DeserializeCallback> m_DeserializeCallbacks;
        private readonly Dictionary<byte, TryGetValueCallback> m_TryGetValueCallbacks;
        private byte m_LatestSerializeCallbackVersion;

        /// <summary>
        /// 初始化游戏框架序列化器基类的新实例。
        /// </summary>
        public GameFrameworkSerializer()
        {
            m_SerializeCallbacks = new Dictionary<byte, SerializeCallback>();
            m_DeserializeCallbacks = new Dictionary<byte, DeserializeCallback>();
            m_TryGetValueCallbacks = new Dictionary<byte, TryGetValueCallback>();
            m_LatestSerializeCallbackVersion = 0;
        }

        /// <summary>
        /// 序列化回调函数。
        /// </summary>
        /// <param name="binaryWriter">目标流。</param>
        /// <param name="data">要序列化的数据。</param>
        /// <returns>序列化数据是否成功。</returns>
        public delegate bool SerializeCallback(BinaryWriter binaryWriter, T data);

        /// <summary>
        /// 反序列化回调函数。
        /// </summary>
        /// <param name="binaryReader">指定流。</param>
        /// <returns>反序列化的数据。</returns>
        public delegate T DeserializeCallback(BinaryReader binaryReader);

        /// <summary>
        /// 尝试从指定流获取指定键的值回调函数。
        /// </summary>
        /// <param name="binaryReader">指定流。</param>
        /// <param name="key">指定键。</param>
        /// <param name="value">指定键的值。</param>
        /// <returns>从指定流获取指定键的值是否成功。</returns>
        public delegate bool TryGetValueCallback(BinaryReader binaryReader, string key, out object value);

        /// <summary>
        /// 注册序列化回调函数。
        /// </summary>
        /// <param name="version">序列化回调函数的版本。</param>
        /// <param name="callback">序列化回调函数。</param>
        public void RegisterSerializeCallback(byte version, SerializeCallback callback)
        {
            if (callback == null)
            {
                throw new GameFrameworkException("Serialize callback is invalid.");
            }

            m_SerializeCallbacks[version] = callback;
            if (version > m_LatestSerializeCallbackVersion)
            {
                m_LatestSerializeCallbackVersion = version;
            }
        }

        /// <summary>
        /// 注册反序列化回调函数。
        /// </summary>
        /// <param name="version">反序列化回调函数的版本。</param>
        /// <param name="callback">反序列化回调函数。</param>
        public void RegisterDeserializeCallback(byte version, DeserializeCallback callback)
        {
            if (callback == null)
            {
                throw new GameFrameworkException("Deserialize callback is invalid.");
            }

            m_DeserializeCallbacks[version] = callback;
        }

        /// <summary>
        /// 注册尝试从指定流获取指定键的值回调函数。
        /// </summary>
        /// <param name="version">尝试从指定流获取指定键的值回调函数的版本。</param>
        /// <param name="callback">尝试从指定流获取指定键的值回调函数。</param>
        public void RegisterTryGetValueCallback(byte version, TryGetValueCallback callback)
        {
            if (callback == null)
            {
                throw new GameFrameworkException("Try get value callback is invalid.");
            }

            m_TryGetValueCallbacks[version] = callback;
        }

        /// <summary>
        /// 序列化数据到目标流中。
        /// </summary>
        /// <param name="stream">目标流。</param>
        /// <param name="data">要序列化的数据。</param>
        /// <returns>序列化数据是否成功。</returns>
        public bool Serialize(Stream stream, T data)
        {
            if (m_SerializeCallbacks.Count <= 0)
            {
                throw new GameFrameworkException("No serialize callback registered.");
            }

            return Serialize(stream, data, m_LatestSerializeCallbackVersion);
        }

        /// <summary>
        /// 序列化数据到目标流中。
        /// </summary>
        /// <param name="stream">目标流。</param>
        /// <param name="data">要序列化的数据。</param>
        /// <param name="version">序列化回调函数的版本。</param>
        /// <returns>序列化数据是否成功。</returns>
        public bool Serialize(Stream stream, T data, byte version)
        {
            using (BinaryWriter binaryWriter = new BinaryWriter(stream, Encoding.UTF8))
            {
                byte[] header = GetHeader();
                binaryWriter.Write(header[0]);
                binaryWriter.Write(header[1]);
                binaryWriter.Write(header[2]);
                binaryWriter.Write(version);
                SerializeCallback callback = null;
                if (!m_SerializeCallbacks.TryGetValue(version, out callback))
                {
                    throw new GameFrameworkException(Utility.Text.Format("Serialize callback '{0}' is not exist.", version.ToString()));
                }

                return callback(binaryWriter, data);
            }
        }

        /// <summary>
        /// 从指定流反序列化数据。
        /// </summary>
        /// <param name="stream">指定流。</param>
        /// <returns>反序列化的数据。</returns>
        public T Deserialize(Stream stream)
        {
            using (BinaryReader binaryReader = new BinaryReader(stream, Encoding.UTF8))
            {
                byte[] header = GetHeader();
                byte header0 = binaryReader.ReadByte();
                byte header1 = binaryReader.ReadByte();
                byte header2 = binaryReader.ReadByte();
                if (header0 != header[0] || header1 != header[1] || header2 != header[2])
                {
                    throw new GameFrameworkException(Utility.Text.Format("Header is invalid, need '{0}{1}{2}', current '{3}{4}{5}'.",
                        ((char)header[0]).ToString(), ((char)header[1]).ToString(), ((char)header[2]).ToString(),
                        ((char)header0).ToString(), ((char)header1).ToString(), ((char)header2).ToString()));
                }

                byte version = binaryReader.ReadByte();
                DeserializeCallback callback = null;
                if (!m_DeserializeCallbacks.TryGetValue(version, out callback))
                {
                    throw new GameFrameworkException(Utility.Text.Format("Deserialize callback '{0}' is not exist.", version.ToString()));
                }

                return callback(binaryReader);
            }
        }

        /// <summary>
        /// 尝试从指定流获取指定键的值。
        /// </summary>
        /// <param name="stream">指定流。</param>
        /// <param name="key">指定键。</param>
        /// <param name="value">指定键的值。</param>
        /// <returns>从指定流获取指定键的值是否成功。</returns>
        public bool TryGetValue(Stream stream, string key, out object value)
        {
            value = null;
            using (BinaryReader binaryReader = new BinaryReader(stream, Encoding.UTF8))
            {
                byte[] header = GetHeader();
                byte header0 = binaryReader.ReadByte();
                byte header1 = binaryReader.ReadByte();
                byte header2 = binaryReader.ReadByte();
                if (header0 != header[0] || header1 != header[1] || header2 != header[2])
                {
                    return false;
                }

                byte version = binaryReader.ReadByte();
                TryGetValueCallback callback = null;
                if (!m_TryGetValueCallbacks.TryGetValue(version, out callback))
                {
                    return false;
                }

                return callback(binaryReader, key, out value);
            }
        }

        /// <summary>
        /// 获取数据头标识。
        /// </summary>
        /// <returns>数据头标识。</returns>
        protected abstract byte[] GetHeader();
    }
}
