using UnityEngine;
using UnityEngine.UI;

namespace UltrahapticsCoreAsset.Examples.PolylineHandPathDesigner
{
    public class UpdateDrawFrequency : MonoBehaviour
    {

        public SensationSource _sensation;
        public Slider Slider;
        public InputField InputValue;

        public void SetSliderValueFromInputValue()
        {
            Slider.value = float.Parse(InputValue.text);
            SetDrawFrequencyFromSliderValue();
        }


        public void SetDrawFrequencyFromSliderValue()
        {
            _sensation.Inputs["drawFrequency"].Value = new Vector3(Slider.value, 0.0f, 0.0f);
            InputValue.text = Slider.value.ToString();
        }
    }
}