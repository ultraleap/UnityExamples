using UnityEngine;
using UnityEngine.UI;

namespace UltrahapticsCoreAsset.Examples.PolylineHandPathDesigner
{
    public class SensationNameField : MonoBehaviour
    {

        public SaveSensationToPythonBlock SaveSensationScript;
        public Button saveButton;
        public InputField blockTextInputField;
        public Text blockNameText;

        // Use this for initialization
        void Start()
        {
            blockNameText.text = "";
            blockTextInputField.text = "";
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void TextInputChanged() {
            if (blockTextInputField.text.Length <= 0) {
                saveButton.interactable = false;
            }
            else 
            {
                saveButton.interactable = true;
            }
        }


        public void UpdateBlockSaveName() {
            SaveSensationScript.blockName = blockTextInputField.text;
        }

    }
}