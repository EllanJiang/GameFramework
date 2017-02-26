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
        private sealed class InputSummaryInformationWindow : ScrollableDebuggerWindowBase
        {
            protected override void OnDrawScrollableWindow()
            {
                GUILayout.Label("<b>Input Summary Information</b>");
                GUILayout.BeginVertical("box");
                {
                    DrawItem("Back Button Leaves App:", Input.backButtonLeavesApp.ToString());
                    DrawItem("Device Orientation:", Input.deviceOrientation.ToString());
                    DrawItem("Mouse Present:", Input.mousePresent.ToString());
                    DrawItem("Mouse Position:", Input.mousePosition.ToString());
                    DrawItem("Mouse Scroll Delta:", Input.mouseScrollDelta.ToString());
                    DrawItem("Any Key:", Input.anyKey.ToString());
                    DrawItem("Any Key Down:", Input.anyKeyDown.ToString());
                    DrawItem("Input String:", Input.inputString);
                    DrawItem("IME Is Selected:", Input.imeIsSelected.ToString());
                    DrawItem("IME Composition Mode:", Input.imeCompositionMode.ToString());
                    DrawItem("Compensate Sensors:", Input.compensateSensors.ToString());
                    DrawItem("Composition Cursor Position:", Input.compositionCursorPos.ToString());
                    DrawItem("Composition String:", Input.compositionString);
                }
                GUILayout.EndVertical();
            }
        }
    }
}
