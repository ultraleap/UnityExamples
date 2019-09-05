using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace UltrahapticsCoreAsset.UnityExamples
{
    public class SensationRowUI : MonoBehaviour
    {

        public Button button;
        public Text sensationNameText;
        public string sensationName;

        public Color selectedTextColor;
        public Color deselectedTextColor;

        public Sprite selectedSprite;

        // Use this for initialization
        void Start()
        {

        }

        // TODO: activate the SensationRow in code


        // Update is called once per frame
        void Update()
        {

        }

        private void OnMouseOver()
        {
            Debug.Log("Mouse Hover on row:" + sensationName);
        }

        public void SetSensationName(string name)
        {
            var rowText = button.GetComponentsInChildren<Text>();
            rowText[0].text = name;
            sensationName = name;
        }

        public void SetSelectedState(bool selected)
        {
            Debug.Log("seteSelected called:" + selected);
            sensationNameText.color = selected ? selectedTextColor : deselectedTextColor;
        }
    }
}
