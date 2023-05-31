/* ​
* Copyright © 2023 Go Touch VR SAS. All rights reserved. ​
* ​
*/

#if UNITY_EDITOR_64 || UNITY_STANDALONE_WIN
using System.Runtime.InteropServices;


namespace Interhaptics.Platforms.Hypersense
{

    public sealed class KrakenHypersenseProvider : IHapticProvider
    {

        #region NATIVE
        private static class KrakenHypersenseNative
        {
            const string DLL_NAME = "Interhaptics.KrakenProvider";

            [DllImport(DLL_NAME)]
            public static extern bool InitAll();

            [DllImport(DLL_NAME)]
            public static extern int IsPresent();

            [DllImport(DLL_NAME)]
            public static extern bool Clean();

            [DllImport(DLL_NAME)]
            public static extern int RenderHaptics();
        }
        #endregion

        #region HAPTIC CHARACTERISTICS FIELDS
        private const string DISPLAY_NAME = "Kraken Hypersense";
        private const string DESCRIPTION = "Razer's Haptic headphone";
        private const string MANUFACTURER = "Razer";
        private const string VERSION = "1.0";
        #endregion

        #region HAPTIC CHARACTERISTICS GETTERS
        [UnityEngine.Scripting.Preserve]
        public string DisplayName()
        {
            return DISPLAY_NAME;
        }

        [UnityEngine.Scripting.Preserve]
        public string Description()
        {
            return DESCRIPTION;
        }

        [UnityEngine.Scripting.Preserve]
        public string Manufacturer()
        {
            return MANUFACTURER;
        }

        [UnityEngine.Scripting.Preserve]
        public string Version()
        {
            return VERSION;
        }
        #endregion


        #region PROVIDER LOOP
        [UnityEngine.Scripting.Preserve]
        public bool Init()
        {
            bool res = KrakenHypersenseNative.InitAll();
            if (res)
            {
                Core.HAR.AddBodyPart(HapticBodyMapping.Perception.Vibration, HapticBodyMapping.BodyPartID.Bp_Crane, 2, 1, 1, 50, true, true, true, HapticBodyMapping.EProtocol.I2A29B);
            }
            return res;
        }

        [UnityEngine.Scripting.Preserve]
        public bool IsPresent()
        {
            return KrakenHypersenseNative.IsPresent() == 0;
        }

        [UnityEngine.Scripting.Preserve]
        public bool Clean()
        {
            return KrakenHypersenseNative.Clean();
        }

        [UnityEngine.Scripting.Preserve]
        public void RenderHaptics()
        {
            KrakenHypersenseNative.RenderHaptics();
        }
        #endregion

    }

}
#endif // UNITY_EDITOR_64 || UNITY_STANDALONE_WIN