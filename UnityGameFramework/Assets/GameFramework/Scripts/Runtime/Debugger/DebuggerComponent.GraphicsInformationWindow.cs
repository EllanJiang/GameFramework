//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using UnityEngine;

namespace UnityGameFramework.Runtime
{
    public partial class DebuggerComponent
    {
        private sealed class GraphicsInformationWindow : ScrollableDebuggerWindowBase
        {
            protected override void OnDrawScrollableWindow()
            {
                GUILayout.Label("<b>Graphics Information</b>");
                GUILayout.BeginVertical("box");
                {
                    DrawItem("Device ID:", SystemInfo.graphicsDeviceID.ToString());
                    DrawItem("Device Name:", SystemInfo.graphicsDeviceName);
                    DrawItem("Device Vendor ID:", SystemInfo.graphicsDeviceVendorID.ToString());
                    DrawItem("Device Vendor:", SystemInfo.graphicsDeviceVendor);
                    DrawItem("Device Type:", SystemInfo.graphicsDeviceType.ToString());
                    DrawItem("Device Version:", SystemInfo.graphicsDeviceVersion);
                    DrawItem("Memory Size:", string.Format("{0} MB", SystemInfo.graphicsMemorySize.ToString()));
                    DrawItem("Multi Threaded:", SystemInfo.graphicsMultiThreaded.ToString());
                    DrawItem("Shader Level:", GetShaderLevelString(SystemInfo.graphicsShaderLevel));
                    DrawItem("NPOT Support:", SystemInfo.npotSupport.ToString());
                    DrawItem("Max Texture Size:", SystemInfo.maxTextureSize.ToString());
#if UNITY_5_6_OR_NEWER
                    DrawItem("Max Cubemap Size:", SystemInfo.maxCubemapSize.ToString());
#endif
#if UNITY_5_4_OR_NEWER
                    DrawItem("Copy Texture Support:", SystemInfo.copyTextureSupport.ToString());
#endif
                    DrawItem("Supported Render Target Count:", SystemInfo.supportedRenderTargetCount.ToString());
#if UNITY_5_3 || UNITY_5_4
                    DrawItem("Supports Stencil:", SystemInfo.supportsStencil.ToString());
                    DrawItem("Supports Render Textures:", SystemInfo.supportsRenderTextures.ToString());
#endif
                    DrawItem("Supports Sparse Textures:", SystemInfo.supportsSparseTextures.ToString());
                    DrawItem("Supports 3D Textures:", SystemInfo.supports3DTextures.ToString());
#if UNITY_5_6_OR_NEWER
                    DrawItem("Supports 3D Render Textures:", SystemInfo.supports3DRenderTextures.ToString());
#endif
#if UNITY_5_4_OR_NEWER
                    DrawItem("Supports 2D Array Textures:", SystemInfo.supports2DArrayTextures.ToString());
#endif
                    DrawItem("Supports Shadows:", SystemInfo.supportsShadows.ToString());
                    DrawItem("Supports Raw Shadow Depth Sampling:", SystemInfo.supportsRawShadowDepthSampling.ToString());
                    DrawItem("Supports Render To Cubemap:", SystemInfo.supportsRenderToCubemap.ToString());
                    DrawItem("Supports Compute Shader:", SystemInfo.supportsComputeShaders.ToString());
                    DrawItem("Supports Instancing:", SystemInfo.supportsInstancing.ToString());
                    DrawItem("Supports Image Effects:", SystemInfo.supportsImageEffects.ToString());
#if UNITY_5_5_OR_NEWER
                    DrawItem("Supports Cubemap Array Textures:", SystemInfo.supportsCubemapArrayTextures.ToString());
#endif
#if UNITY_5_4_OR_NEWER
                    DrawItem("Supports Motion Vectors:", SystemInfo.supportsMotionVectors.ToString());
#endif
#if UNITY_5_6_OR_NEWER
                    DrawItem("Graphics UV Starts At Top:", SystemInfo.graphicsUVStartsAtTop.ToString());
#endif
#if UNITY_5_5_OR_NEWER
                    DrawItem("Uses Reversed ZBuffer:", SystemInfo.usesReversedZBuffer.ToString());
#endif
                }
                GUILayout.EndVertical();
            }

            private string GetShaderLevelString(int shaderLevel)
            {
                return string.Format("Shader Model {0}.{1}", (shaderLevel / 10).ToString(), (shaderLevel % 10).ToString());
            }
        }
    }
}
