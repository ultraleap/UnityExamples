using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UltrahapticsCoreAsset.Examples
{
    public class SensationSourcePlaybackDurationExample : MonoBehaviour
    {
        public SensationSource SensationSource;
        private Slider slider_;
        public Text DurationText;
        // Use this for initialization
        void Start()
        {
            slider_ = GetComponentInChildren<Slider>();
        }

        public void RunSensationForSliderDuration()
        {
            SensationSource.RunForDuration(slider_.value);
        }

        public void Update()
        {
            DurationText.text = "Run For " + slider_.value.ToString("F1") + " s";
        }
    }
}