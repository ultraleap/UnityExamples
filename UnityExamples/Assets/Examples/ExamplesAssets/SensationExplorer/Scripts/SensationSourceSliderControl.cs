using UnityEngine;
using UnityEngine.UI;

namespace UltrahapticsCoreAsset.UnityExamples
{
    public class SensationSourceSliderControl : MonoBehaviour
    {

        public SensationSource sensation;
        public Slider slider;
        public Text inputName;
        public InputField inputField;
        public SensationBlockInput blockInput;

        // Use this for initialization
        void Start()
        {
            slider.onValueChanged.AddListener(delegate { SliderValueChanged(); });
            inputField.onEndEdit.AddListener(delegate { TextBoxValueChanged(); });
            inputField.contentType = InputField.ContentType.DecimalNumber;
        }

        void SliderValueChanged()
        {
            inputField.text = slider.value.ToString("F2");
            if (sensation != null && blockInput != null)
            {
                sensation.Inputs[blockInput.Name].Value = new Vector3(slider.value, 0, 0);
            }
        }

        void TextBoxValueChanged()
        {
            var textNumericValue = float.Parse(inputField.text);

            // TODO: Validate values out of range
            slider.value = textNumericValue;

            if (sensation != null && blockInput != null)
            {
                sensation.Inputs[blockInput.Name].Value = new Vector3(slider.value, 0, 0);
            }
        }
    }
}