using UnityEngine;
using UnityEngine.UI;

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

        private Color activeColor = new Color(93.0f / 255.0f, 203.0f / 255.0f, 126.0f / 255.0f);
        private Color inActiveColor = new Color(226.0f / 255.0f, 226.0f / 255.0f, 226.0f / 255.0f);

        // Update Hardware check every 3 Seconds
        private int nextUpdate = 3;

        // Start is called before the first frame update
        void Start()
        {
            leapController = new Controller();
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
            bool uhConnected = IsUltrahapticsDeviceConnected();
            bool leapConnected = IsLeapDeviceConnected();
            UltrahapticsDeviceConnectedIndicator.image.color = uhConnected ? activeColor : inActiveColor;
            LeapDeviceConnectedIndicator.image.color = leapConnected ? activeColor : inActiveColor;
        }

        public static bool IsUltrahapticsDeviceConnected()
        {
            return SensationCore.Instance.IsEmitterConnected();
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