//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.Network;
using UnityEditor;
using UnityGameFramework.Runtime;

namespace UnityGameFramework.Editor
{
    [CustomEditor(typeof(NetworkComponent))]
    internal sealed class NetworkComponentInspector : GameFrameworkInspector
    {
        private SerializedProperty m_NetworkChannels = null;

        private HelperInfo<NetworkHelperBase> m_NetworkHelperInfo = new HelperInfo<NetworkHelperBase>("Network");

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            NetworkComponent t = (NetworkComponent)target;

            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            {
                m_NetworkHelperInfo.Draw();

                EditorGUILayout.PropertyField(m_NetworkChannels, true);
            }
            EditorGUI.EndDisabledGroup();

            if (EditorApplication.isPlaying)
            {
                EditorGUILayout.LabelField("Network Channel Count", t.NetworkChannelCount.ToString());

                INetworkChannel[] networkChannels = t.GetAllNetworkChannels();
                foreach (INetworkChannel networkChannel in networkChannels)
                {
                    DrawNetworkChannel(networkChannel);
                }
            }

            serializedObject.ApplyModifiedProperties();

            Repaint();
        }

        protected override void OnCompileComplete()
        {
            base.OnCompileComplete();

            RefreshTypeNames();
        }

        private void OnEnable()
        {
            m_NetworkChannels = serializedObject.FindProperty("m_NetworkChannels");

            m_NetworkHelperInfo.Init(serializedObject);

            RefreshTypeNames();
        }

        private void DrawNetworkChannel(INetworkChannel networkChannel)
        {
            string networkChannelName = networkChannel.Name;
            switch (networkChannel.NetworkType)
            {
                case NetworkType.IPv4:
                    networkChannelName += " (IPv4)";
                    break;
                case NetworkType.IPv6:
                    networkChannelName += " (IPv6)";
                    break;
                default:
                    break;
            }

            EditorGUILayout.LabelField(networkChannelName, networkChannel.Connected ? "Connected" : "Disconnected");
        }

        private void RefreshTypeNames()
        {
            m_NetworkHelperInfo.Refresh();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
