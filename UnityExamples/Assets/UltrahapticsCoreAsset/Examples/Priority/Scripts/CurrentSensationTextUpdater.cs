using UnityEngine;
using UnityEngine.UI;

namespace UltrahapticsCoreAsset.Examples
{
    public class CurrentSensationTextUpdater : MonoBehaviour
    {
        private SensationEmitter sensationEmitter_;
        public Text CurrentSourceText;

        // Use this for initialization
        void Start()
        {
            sensationEmitter_ = FindObjectOfType<SensationEmitter>();
        }

        // Update is called once per frame
        void Update()
        {
            if (sensationEmitter_.CurrentSensation != null)
            {
                var sensation = sensationEmitter_.CurrentSensation;
                CurrentSourceText.text = "<b>CURRENT SENSATION:</b> " + sensation.gameObject.name + " (Priority =  " + sensation.Priority + ")";
            }
            else
            {
                CurrentSourceText.text = "<b>CURRENT SENSATION:</b> Unknown";
            }
        }
    }
}