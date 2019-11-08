using UnityEngine;
using UnityEngine.UI;
using System;

#if UNITY_STANDALONE_WIN
using Controller = Leap.Controller;
#endif

#if UNITY_STANDALONE_OSX
using Leap;
#endif

namespace UltrahapticsCoreAsset.UnityExamples
{
    public class HardwareConnectionWatcher : MonoBehaviour
    {

        public Button UltrahapticsDeviceConnectedIndicator;
        public Button LeapDeviceConnectedIndicator;

        private static Controller leapController;

        public bool uhConnected = false;
        public bool leapConnected = false;

        private Color activeColor = new Color(93.0f / 255.0f, 203.0f / 255.0f, 126.0f / 255.0f);
        private Color inActiveColor = new Color(226.0f / 255.0f, 226.0f / 255.0f, 226.0f / 255.0f);

        public string uhConnectionStatusText {
            get { return UHDeviceConnectionText(); }
            set { uhConnectionStatusText = value; }
        }

        public string leapConnectionStatusText
        {
            get { return LeapConnectionText(); }
            set { leapConnectionStatusText = value; }
        }

        // Update Hardware check every 3 Seconds
        private int nextUpdate = 3;

        // Start is called before the first frame update
        void Start()
        {
            leapController = new Controller();

            // TODO: Understand why initially reports as UH Device not connected.
            UpdateHardwareStatusIndicators();
        }

        // Update is called once per frame
        void Update()
        {
            // If the next update is reached
            if (Time.time >= nextUpdate)
            {
                UpdateHardwareStatusIndicators();                
            }

        }

        // Update is called once per second
        void UpdateHardwareStatusIndicators()
        {
            uhConnected = IsUltrahapticsDeviceConnected();
            leapConnected = IsLeapDeviceConnected();
            UltrahapticsDeviceConnectedIndicator.image.color = uhConnected ? activeColor : inActiveColor;
            LeapDeviceConnectedIndicator.image.color = leapConnected ? activeColor : inActiveColor;
        }

        private static bool usingMockDevice()
        {
            return SensationCore.Instance.EmitterSerialNumber().Equals("MOCK");
        }

        public static bool IsUltrahapticsDeviceConnected()
        {
            return SensationCore.Instance.IsEmitterConnected() && !usingMockDevice();
        }

        public static string UHDeviceConnectionText()
        {
            string connectionString = "Ultrahaptics Device: " + (usingMockDevice() ? "Mock Device in use." : SensationCore.Instance.EmitterSerialNumber());
            return connectionString;
        }

        public static string LeapConnectionText()
        {
            string connectionString = "Leap Motion Device: " + (IsLeapDeviceConnected() ? "Leap Motion connected. " : "No Leap Motion device detected.");
            return connectionString;
        }

        public static bool IsLeapDeviceConnected()
        {
            if (leapController != null)
            {
                return leapController.IsConnected;
            }
            else
            {
                return false;
            }
        }
    }
}