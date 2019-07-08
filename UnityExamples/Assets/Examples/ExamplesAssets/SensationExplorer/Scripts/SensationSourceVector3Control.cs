using UnityEngine;
using UnityEngine.UI;

namespace UltrahapticsCoreAsset.UnityExamples
{
    public class SensationSourceVector3Control : MonoBehaviour
    {

        public SensationSource sensation;
        public Text inputName;
        public SensationBlockInput blockInput;

        public InputField xValue;
        public InputField yValue;
        public InputField zValue;

        // Use this for initialization
        void Start()
        {
            xValue.onEndEdit.AddListener(delegate{XYZValueChanged();});
            yValue.onEndEdit.AddListener(delegate { XYZValueChanged(); });
            zValue.onEndEdit.AddListener(delegate { XYZValueChanged(); });
        }

        void XYZValueChanged()
        {
            var inputString = blockInput.Name;
            if (sensation != null && xValue != null)
            {
                sensation.Inputs[inputString].Value = new Vector3(float.Parse(xValue.text), float.Parse(yValue.text), float.Parse(zValue.text));
            }
        }
    }
}