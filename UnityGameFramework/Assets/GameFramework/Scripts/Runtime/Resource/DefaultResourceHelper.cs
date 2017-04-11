//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.Resource;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 默认资源辅助器。
    /// </summary>
    public class DefaultResourceHelper : ResourceHelperBase
    {
        /// <summary>
        /// 直接从指定文件路径读取数据流。
        /// </summary>
        /// <param name="fileUri">文件路径。</param>
        /// <param name="loadBytesCallback">读取数据流回调函数。</param>
        public override void LoadBytes(string fileUri, LoadBytesCallback loadBytesCallback)
        {
            StartCoroutine(LoadBytesCo(fileUri, loadBytesCallback));
        }

        /// <summary>
        /// 卸载场景。
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称。</param>
        /// <param name="unloadSceneCallbacks">卸载场景回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        public override void UnloadScene(string sceneAssetName, UnloadSceneCallbacks unloadSceneCallbacks, object userData)
        {
#if UNITY_5_5_OR_NEWER
            StartCoroutine(UnloadSceneCo(sceneAssetName, unloadSceneCallbacks, userData));
#else
            if (SceneManager.UnloadScene(SceneComponent.GetSceneName(sceneAssetName)))
            {
                if (unloadSceneCallbacks.UnloadSceneSuccessCallback != null)
                {
                    unloadSceneCallbacks.UnloadSceneSuccessCallback(sceneAssetName, userData);
                }
            }
            else
            {
                if (unloadSceneCallbacks.UnloadSceneFailureCallback != null)
                {
                    unloadSceneCallbacks.UnloadSceneFailureCallback(sceneAssetName, userData);
                }
            }
#endif
        }

        /// <summary>
        /// 释放资源。
        /// </summary>
        /// <param name="objectToRelease">要释放的资源。</param>
        public override void Release(object objectToRelease)
        {
            AssetBundle assetBundle = objectToRelease as AssetBundle;
            if (assetBundle != null)
            {
                assetBundle.Unload(true);
                return;
            }

            /* Unity 当前 Resources.UnloadAsset 在 iOS 设备上会导致一些诡异问题，先不用这部分
            DummySceneObject dummySceneObject = objectToRelease as DummySceneObject;
            if (dummySceneObject != null)
            {
                return;
            }

            Object unityObject = objectToRelease as Object;
            if (unityObject == null)
            {
                Log.Warning("Asset is invalid.");
                return;
            }

            if (unityObject is GameObject || unityObject is MonoBehaviour)
            {
                // UnloadAsset may only be used on individual assets and can not be used on GameObject's / Components or AssetBundles.
                return;
            }

            Resources.UnloadAsset(unityObject);
            */
        }

        private void Start()
        {

        }

        private IEnumerator LoadBytesCo(string fileUri, LoadBytesCallback loadBytesCallback)
        {
            WWW www = new WWW(fileUri);
            yield return www;

            byte[] bytes = www.bytes;
            string errorMessage = www.error;
            www.Dispose();

            if (loadBytesCallback != null)
            {
                loadBytesCallback(fileUri, bytes, errorMessage);
            }
        }

#if UNITY_5_5_OR_NEWER
        private IEnumerator UnloadSceneCo(string sceneAssetName, UnloadSceneCallbacks unloadSceneCallbacks, object userData)
        {
            AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(sceneAssetName);
            if (asyncOperation == null)
            {
                yield break;
            }

            yield return asyncOperation;

            if (asyncOperation.allowSceneActivation)
            {
                if (unloadSceneCallbacks.UnloadSceneSuccessCallback != null)
                {
                    unloadSceneCallbacks.UnloadSceneSuccessCallback(sceneAssetName, userData);
                }
            }
            else
            {
                if (unloadSceneCallbacks.UnloadSceneFailureCallback != null)
                {
                    unloadSceneCallbacks.UnloadSceneFailureCallback(sceneAssetName, userData);
                }
            }
        }
#endif
    }
}
