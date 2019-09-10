using UnityEngine;
using UnityEngine.UI;

namespace UltrahapticsCoreAsset.UnityExamples
{
    public class SensationRowUI : MonoBehaviour
    {

        public Button button;
        public Text sensationNameText;
        public string sensationName;

        public Color selectedTextColor;
        public Color deselectedTextColor;

        public void SetSensationName(string name)
        {
            var rowText = button.GetComponentsInChildren<Text>();
            rowText[0].text = name;
            sensationName = name;
        }

        // TODO: This could be controlled via an 'OnSelect' event?
        public void SetSelectedState(bool selected)
        {
            sensationNameText.color = selected ? selectedTextColor : deselectedTextColor;
        }
    }
}
