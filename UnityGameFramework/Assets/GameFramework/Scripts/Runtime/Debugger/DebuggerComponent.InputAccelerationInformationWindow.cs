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
        private sealed class InputAccelerationInformationWindow : ScrollableDebuggerWindowBase
        {
            protected override void OnDrawScrollableWindow()
            {
                GUILayout.Label("<b>Input Acceleration Information</b>");
                GUILayout.BeginVertical("box");
                {
                    DrawItem("Acceleration:", Input.acceleration.ToString());
                    DrawItem("Acceleration Event Count:", Input.accelerationEventCount.ToString());
                    DrawItem("Acceleration Events:", GetAccelerationEventsString(Input.accelerationEvents));
                }
                GUILayout.EndVertical();
            }

            private string GetAccelerationEventString(AccelerationEvent accelerationEvent)
            {
                return string.Format("{0}, {1}", accelerationEvent.acceleration.ToString(), accelerationEvent.deltaTime.ToString());
            }

            private string GetAccelerationEventsString(AccelerationEvent[] accelerationEvents)
            {
                string[] accelerationEventStrings = new string[accelerationEvents.Length];
                for (int i = 0; i < accelerationEvents.Length; i++)
                {
                    accelerationEventStrings[i] = GetAccelerationEventString(accelerationEvents[i]);
                }

                return string.Join("; ", accelerationEventStrings);
            }
        }
    }
}
