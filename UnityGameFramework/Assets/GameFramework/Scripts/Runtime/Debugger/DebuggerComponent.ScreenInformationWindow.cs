//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    public partial class DebuggerComponent
    {
        private sealed class ScreenInformationWindow : ScrollableDebuggerWindowBase
        {
            protected override void OnDrawScrollableWindow()
            {
                GUILayout.Label("<b>Screen Information</b>");
                GUILayout.BeginVertical("box");
                {
                    DrawItem("Current Resolution", GetResolutionString(Screen.currentResolution));
                    DrawItem("Screen Width", string.Format("{0} px / {1} in / {2} cm", Screen.width.ToString(), Utility.Converter.GetInchesFromPixels(Screen.width).ToString("F2"), Utility.Converter.GetCentimetersFromPixels(Screen.width).ToString("F2")));
                    DrawItem("Screen Height", string.Format("{0} px / {1} in / {2} cm", Screen.height.ToString(), Utility.Converter.GetInchesFromPixels(Screen.height).ToString("F2"), Utility.Converter.GetCentimetersFromPixels(Screen.height).ToString("F2")));
                    DrawItem("Screen DPI", Screen.dpi.ToString("F2"));
                    DrawItem("Screen Orientation", Screen.orientation.ToString());
                    DrawItem("Is Full Screen", Screen.fullScreen.ToString());
                    DrawItem("Sleep Timeout", GetSleepTimeoutDescription(Screen.sleepTimeout));
                    DrawItem("Cursor Visible", Cursor.visible.ToString());
                    DrawItem("Cursor Lock State", Cursor.lockState.ToString());
                    DrawItem("Auto Landscape Left", Screen.autorotateToLandscapeLeft.ToString());
                    DrawItem("Auto Landscape Right", Screen.autorotateToLandscapeRight.ToString());
                    DrawItem("Auto Portrait", Screen.autorotateToPortrait.ToString());
                    DrawItem("Auto Portrait Upside Down", Screen.autorotateToPortraitUpsideDown.ToString());
                    DrawItem("Support Resolutions", GetResolutionsString(Screen.resolutions));
                }
                GUILayout.EndVertical();
            }

            private string GetSleepTimeoutDescription(int sleepTimeout)
            {
                if (sleepTimeout == SleepTimeout.NeverSleep)
                {
                    return "Never Sleep";
                }

                if (sleepTimeout == SleepTimeout.SystemSetting)
                {
                    return "System Setting";
                }

                return sleepTimeout.ToString();
            }

            private string GetResolutionString(Resolution resolution)
            {
                return string.Format("{0} x {1} @ {2}Hz", resolution.width.ToString(), resolution.height.ToString(), resolution.refreshRate.ToString());
            }

            private string GetResolutionsString(Resolution[] resolutions)
            {
                string[] resolutionStrings = new string[resolutions.Length];
                for (int i = 0; i < resolutions.Length; i++)
                {
                    resolutionStrings[i] = GetResolutionString(resolutions[i]);
                }

                return string.Join("; ", resolutionStrings);
            }
        }
    }
}
