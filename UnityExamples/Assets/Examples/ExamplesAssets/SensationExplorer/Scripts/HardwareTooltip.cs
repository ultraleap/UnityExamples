using UnityEngine;
using UnityEngine.UI;

namespace UltrahapticsCoreAsset.UnityExamples
{
    public class HardwareTooltip : MonoBehaviour
    {
        // Define possible states for enemy using an enum 
        public enum ULHardwareType { ULTRAHAPTICS, LEAPMOTION };
        public Text toolTipText;

        public HardwareConnectionWatcher HardwareManager;

        // The current state of enemy
        public ULHardwareType Hardware = ULHardwareType.ULTRAHAPTICS;

        // Update is called once per frame 
        public void SetHardwareTooltip()
        {
            // Check the ActiveState variable 
            switch (Hardware)
            {
                // Check one case
                case ULHardwareType.ULTRAHAPTICS:
                    {
                        //Perform fight code here
                        toolTipText.text = HardwareManager.uhConnectionStatusText;
                    }
                    break;

                // Check multiple cases at once
                case ULHardwareType.LEAPMOTION:
                    {
                        //Perform fight code here
                        toolTipText.text = HardwareManager.leapConnectionStatusText;
                    }
                    break;

                // Default case when all other states fail 
                default:
                    {
                        //This is used for the chase state 
                        Debug.Log("Unknown Ultraleap Hardware enum");
                    }
                    break;
            }
        }
    }
}