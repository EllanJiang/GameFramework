//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEditor;
using UnityEngine;

namespace UnityGameFramework.Editor
{
    /// <summary>
    /// 构建配置相关的实用函数。
    /// </summary>
    internal static class BuildSettings
    {
        private const string ConfigurationName = "GameFramework/Configs/BuildSettings.xml";

        private static readonly List<string> s_DefaultSceneNames = new List<string>();
        private static readonly List<string> s_SearchScenePaths = new List<string>();

        static BuildSettings()
        {
            s_DefaultSceneNames.Clear();
            s_SearchScenePaths.Clear();

            string configurationPath = Utility.Path.GetCombinePath(Application.dataPath, ConfigurationName);
            if (!File.Exists(configurationPath))
            {
                return;
            }

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(configurationPath);
                XmlNode xmlRoot = xmlDocument.SelectSingleNode("UnityGameFramework");
                XmlNode xmlBuildSettings = xmlRoot.SelectSingleNode("BuildSettings");
                XmlNode xmlDefaultScenes = xmlBuildSettings.SelectSingleNode("DefaultScenes");
                XmlNode xmlSearchScenePaths = xmlBuildSettings.SelectSingleNode("SearchScenePaths");

                XmlNodeList xmlNodeList = null;
                XmlNode xmlNode = null;

                xmlNodeList = xmlDefaultScenes.ChildNodes;
                for (int i = 0; i < xmlNodeList.Count; i++)
                {
                    xmlNode = xmlNodeList.Item(i);
                    if (xmlNode.Name != "DefaultScene")
                    {
                        continue;
                    }

                    string defaultSceneName = xmlNode.Attributes.GetNamedItem("Name").Value;
                    s_DefaultSceneNames.Add(defaultSceneName);
                }

                xmlNodeList = xmlSearchScenePaths.ChildNodes;
                for (int i = 0; i < xmlNodeList.Count; i++)
                {
                    xmlNode = xmlNodeList.Item(i);
                    if (xmlNode.Name != "SearchScenePath")
                    {
                        continue;
                    }

                    string searchScenePath = xmlNode.Attributes.GetNamedItem("Path").Value;
                    s_SearchScenePaths.Add(searchScenePath);
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// 将构建场景设置为默认。
        /// </summary>
        [MenuItem("Game Framework/Scenes in Build Settings/Default Scenes", false, 20)]
        private static void DefaultScenes()
        {
            HashSet<string> sceneNames = new HashSet<string>();
            foreach (string sceneName in s_DefaultSceneNames)
            {
                sceneNames.Add(sceneName);
            }

            List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>();
            foreach (string sceneName in sceneNames)
            {
                scenes.Add(new EditorBuildSettingsScene(sceneName, true));
            }

            EditorBuildSettings.scenes = scenes.ToArray();

            Debug.Log("Set scenes of build settings to default scenes.");
        }

        /// <summary>
        /// 将构建场景设置为所有。
        /// </summary>
        [MenuItem("Game Framework/Scenes in Build Settings/All Scenes", false, 21)]
        private static void AllScenes()
        {
            HashSet<string> sceneNames = new HashSet<string>();
            foreach (string sceneName in s_DefaultSceneNames)
            {
                sceneNames.Add(sceneName);
            }

            string[] sceneGuids = AssetDatabase.FindAssets("t:Scene", s_SearchScenePaths.ToArray());
            foreach (string sceneGuid in sceneGuids)
            {
                string sceneName = AssetDatabase.GUIDToAssetPath(sceneGuid);
                sceneNames.Add(sceneName);
            }

            List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>();
            foreach (string sceneName in sceneNames)
            {
                scenes.Add(new EditorBuildSettingsScene(sceneName, true));
            }

            EditorBuildSettings.scenes = scenes.ToArray();

            Debug.Log("Set scenes of build settings to all scenes.");
        }
    }
}
