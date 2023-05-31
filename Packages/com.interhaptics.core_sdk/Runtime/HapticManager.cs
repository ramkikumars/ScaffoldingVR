/* ​
* Copyright © 2022 Go Touch VR SAS. All rights reserved. ​
* ​
*/


using System.Collections;
using UnityEngine;

namespace Interhaptics
{

    [UnityEngine.AddComponentMenu("Interhaptics/HapticManager")]
    public sealed class HapticManager : Internal.Singleton<HapticManager>
    {
        private bool isRendering = false;
        public void Init()
        {
            Core.HAR.Init();
            Internal.HapticDeviceManager.DeviceInitLoop();
#if UNITY_ANDROID && !ENABLE_METAQUEST
            Platforms.Mobile.GenericAndroidHapticAbstraction.Initialize();
            Platforms.Mobile.GenericAndroidHapticAbstraction.m_timer = 30;
            Platforms.Mobile.GenericAndroidHapticAbstraction.pulse = 30;
            Platforms.Mobile.GenericAndroidHapticAbstraction.m_last_time = UnityEngine.Time.fixedTime;
#elif UNITY_IPHONE
            UnityCoreHaptics.UnityCoreHapticsProxy.CreateEngine();
#endif
        }

        override protected void OnAwake()
        {
#if !UNITY_EDITOR_OSX
            Init();
#endif
        }

        private void Start()
        {
            isRendering = true;
            StartCoroutine(ComputeLoop());
        }

        public IEnumerator ComputeLoop()
        {
            while (isRendering)
            {
                // Debug.Log("Compute Loop");
                //Compute all haptics event
                Core.HAR.ComputeAllEvents(UnityEngine.Time.realtimeSinceStartup);
                //Insert device rendering loop here
                Internal.HapticDeviceManager.DeviceRenderLoop();
                yield return new WaitForSeconds(0.030f);
            }
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            // TODO pause the haptic playback
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            // TODO pause the haptic playback
        }

        protected override void OnOnApplicationQuit()
        {
#if !UNITY_EDITOR_OSX
            isRendering = false;
            Internal.HapticDeviceManager.DeviceCleanLoop();
            Core.HAR.ClearActiveEvents();
            Core.HAR.ClearInactiveEvents();
            Core.HAR.Quit();
#endif
        }

    }

}