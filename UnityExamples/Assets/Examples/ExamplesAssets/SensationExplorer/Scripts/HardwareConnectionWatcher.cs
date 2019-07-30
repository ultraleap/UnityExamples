using UnityEngine;
using UnityEngine.UI;

#if UNITY_STANDALONE_WIN
using Leap.Unity;
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
            UltrahapticsDeviceConnectedIndicator.image.color = uhConnected ? Color.green : Color.gray;
            LeapDeviceConnectedIndicator.image.color = leapConnected ? Color.green : Color.gray;
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