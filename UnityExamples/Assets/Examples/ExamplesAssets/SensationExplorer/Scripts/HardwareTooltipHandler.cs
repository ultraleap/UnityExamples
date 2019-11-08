using UnityEngine;
using UnityEngine.UI;

namespace UltrahapticsCoreAsset.UnityExamples
{
    public class HardwareTooltipHandler : MonoBehaviour
    {
        public HardwareTooltip hardwareTooltip;
        public HardwareTooltip.ULHardwareType hardwareType;

        // Start is called before the first frame update
        void Start()
        {
            hardwareTooltip.gameObject.SetActive(false);
        }

        void OnMouseOver()
        {
            //If your mouse hovers over the GameObject with the script attached, output this message
            hardwareTooltip.Hardware = hardwareType;
            hardwareTooltip.SetHardwareTooltip();
            hardwareTooltip.gameObject.SetActive(true);
        }

        void OnMouseExit()
        {
            //The mouse is no longer hovering over the GameObject so output this message each frame
            hardwareTooltip.gameObject.SetActive(false);
        }
    }
}