using UnityEngine;
using UnityEngine.UI;

namespace UltrahapticsCoreAsset.Examples
{
    public class SensationSourcePlaybackExample : MonoBehaviour
    {
        private Text displayText_;
        private SensationEmitter sensationEmitter_;

        private void Start()
        {
            displayText_ = GetComponent<Text>();
            sensationEmitter_ = FindObjectOfType<SensationEmitter>();
        }

        // Update is called once per frame
        void Update()
        {
            if (sensationEmitter_ != null)
            {
                var currentSensationText_ = "";
                if (sensationEmitter_.CurrentSensation != null)
                {
                    currentSensationText_ = sensationEmitter_.CurrentSensation.SensationBlock;
                }
                displayText_.text = "<b>Current Sensation: " + currentSensationText_ + "</b>";
            }
        }
    }
}
