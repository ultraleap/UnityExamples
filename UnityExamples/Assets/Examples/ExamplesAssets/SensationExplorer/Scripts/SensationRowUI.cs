using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace UltrahapticsCoreAsset.UnityExamples
{
    public class SensationRowUI : MonoBehaviour
    {

        public Button button;
        public Text sensationNameText;
        public string sensationName;

        // Use this for initialization
        void Start()
        {

        }

        // TODO: activate the SensationRow in code


        // Update is called once per frame
        void Update()
        {

        }

        public void SetSensationName(string name)
        {
            var rowText = button.GetComponentsInChildren<Text>();
            rowText[0].text = name;
            sensationName = name;
        }
    }
}
