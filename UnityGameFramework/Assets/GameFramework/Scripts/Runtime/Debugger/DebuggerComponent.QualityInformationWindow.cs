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
        private sealed class QualityInformationWindow : ScrollableDebuggerWindowBase
        {
            protected override void OnDrawScrollableWindow()
            {
                GUILayout.Label("<b>Rendering Information</b>");
                GUILayout.BeginVertical("box");
                {
                    DrawItem("Active Color Space:", QualitySettings.activeColorSpace.ToString());
                    DrawItem("Desired Color Space:", QualitySettings.desiredColorSpace.ToString());
                    DrawItem("Max Queued Frames:", QualitySettings.maxQueuedFrames.ToString());
                    DrawItem("Pixel Light Count:", QualitySettings.pixelLightCount.ToString());
                    DrawItem("Master Texture Limit:", QualitySettings.masterTextureLimit.ToString());
                    DrawItem("Anisotropic Filtering:", QualitySettings.anisotropicFiltering.ToString());
                    DrawItem("Anti Aliasing:", QualitySettings.antiAliasing.ToString());
                    DrawItem("Realtime Reflection Probes:", QualitySettings.realtimeReflectionProbes.ToString());
                    DrawItem("Billboards Face Camera Position:", QualitySettings.billboardsFaceCameraPosition.ToString());
                }
                GUILayout.EndVertical();

                GUILayout.Label("<b>Shadows Information</b>");
                GUILayout.BeginVertical("box");
                {
#if UNITY_5_4_OR_NEWER
                    DrawItem("Shadow Resolution:", QualitySettings.shadowResolution.ToString());
#endif
#if UNITY_5_5_OR_NEWER
                    DrawItem("Shadow Quality:", QualitySettings.shadows.ToString());
#endif
                    DrawItem("Shadow Projection:", QualitySettings.shadowProjection.ToString());
                    DrawItem("Shadow Distance:", QualitySettings.shadowDistance.ToString());
                    DrawItem("Shadow Near Plane Offset:", QualitySettings.shadowNearPlaneOffset.ToString());
                    DrawItem("Shadow Cascades:", QualitySettings.shadowCascades.ToString());
                    DrawItem("Shadow Cascade 2 Split:", QualitySettings.shadowCascade2Split.ToString());
                    DrawItem("Shadow Cascade 4 Split:", QualitySettings.shadowCascade4Split.ToString());
                }
                GUILayout.EndVertical();

                GUILayout.Label("<b>Other Information</b>");
                GUILayout.BeginVertical("box");
                {
                    DrawItem("Blend Weights:", QualitySettings.blendWeights.ToString());
                    DrawItem("VSync Count:", QualitySettings.vSyncCount.ToString());
                    DrawItem("LOD Bias:", QualitySettings.lodBias.ToString());
                    DrawItem("Maximum LOD Level:", QualitySettings.maximumLODLevel.ToString());
                    DrawItem("Particle Raycast Budget:", QualitySettings.particleRaycastBudget.ToString());
                    DrawItem("Async Upload Time Slice:", string.Format("{0} ms", QualitySettings.asyncUploadTimeSlice.ToString()));
                    DrawItem("Async Upload Buffer Size:", string.Format("{0} MB", QualitySettings.asyncUploadBufferSize.ToString()));
#if UNITY_5_5_OR_NEWER
                    DrawItem("Soft Particles:", QualitySettings.softParticles.ToString());
#endif
                    DrawItem("Soft Vegetation:", QualitySettings.softVegetation.ToString());
                }
                GUILayout.EndVertical();
            }
        }
    }
}
