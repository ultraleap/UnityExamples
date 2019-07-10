using UnityEngine;

namespace UltrahapticsCoreAsset.Editor {
    public class AutoDetectUltrahapticsKit : MonoBehaviour {

        public UltrahapticsKitLayout UHKitLayout;

        // Use this for initialization
        void Start() {
            if (UHKitLayout != null)
            {
                // Ensure changing the Kit dropdown automatically sets the layout
                UHKitLayout.UseDefaultLayout = true;

                var uhDeviceString = GetConnectedUHModelString();
                //Debug.Log("Connected device is:" + uhDeviceString);
                if (uhDeviceString.StartsWith("USX"))
                {
                    UHKitLayout.UltrahapticsKitModel = UHKit.Model.STRATOSExplore;
                }
                else if (uhDeviceString.StartsWith("USI"))
                {
                    UHKitLayout.UltrahapticsKitModel = UHKit.Model.STRATOSInspire;
                }
                // Finally, assume an Explore is connected...
                else
                {
                    Debug.LogWarning("Could not detect a valid Ultrahaptics Kit type (USX and USI supported)");
                }
            }
            else
            {
                Debug.LogWarning("Ensure that the UltrahapticsKitLayout is set in the Inspector");
            }
        }

        private string GetConnectedUHModelString()
        {
            var deviceString = "";
            var connectedDevices = SensationCore.Instance.GetConnectedDevices();
            if (connectedDevices.Count != 1)
            {
                Debug.Log("This example assumes one kit is connected! Number of connected devices is: " + connectedDevices.Count);
            }
            else
            {
                deviceString = connectedDevices[0];
            }
            return deviceString;
        }
        // Update is called once per frame
        void Update() {

        }
    }
}