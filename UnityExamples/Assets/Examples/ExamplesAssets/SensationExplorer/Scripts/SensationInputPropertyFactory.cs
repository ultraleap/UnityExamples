using System.Collections.Generic;
using UnityEngine;

namespace UltrahapticsCoreAsset.UnityExamples
{
    public class SensationInputPropertyFactory : MonoBehaviour
    {

        public RectTransform contentRect;
        public List<GameObject> inputRows = null;

        // This is required until we have metadata to say that a Block is defined in Virtual Space
        private List<string> sensationsDefinedInVirtualSpace = new List<string>();

        private IAutoMapper autoMapper_;

        // Use this for initialization
        void Start()
        {
            autoMapper_ = GameObject.FindObjectOfType<IAutoMapper>();

            sensationsDefinedInVirtualSpace.Add("FingerPatch");
            sensationsDefinedInVirtualSpace.Add("HandScan");
            sensationsDefinedInVirtualSpace.Add("PalmTrackedPulsingCircle");
            ClearInputGameObjects();
        }

        // Used to remove all of the Child Game Objects in Content GameObject.
        public void ClearInputGameObjects()
        {
            foreach (Transform child in contentRect.gameObject.transform)
            {
                Destroy(child.gameObject);
            }
            inputRows.Clear();
        }

        public void SetSensationInputsFromSensation(SensationSource sensation)
        {
            ClearInputGameObjects();
            foreach (SensationBlockInput input in sensation.Inputs)
            {
                string inputName = input.Name;

                // Ignore Time input
                if (inputName == "t") {
                    continue;
                }

                // Ignore displaying auto-mapped values (which may be non-hidden)
                if (autoMapper_.HasValueForInputName(inputName)) {
                    continue;
                }

                if (input.Type == "Scalar" && input.IsVisible)
                {
                    var initialValue = input.Value.x;
                    var scalarInputRow = (GameObject)Instantiate(Resources.Load("ScalarSlider"), contentRect.transform);

                    inputRows.Add(scalarInputRow);

                    var sliderControl = scalarInputRow.GetComponent<SensationSourceSliderControl>();

                    // Special Case intensity (always 0-1)
                    if (inputName == "intensity")
                    {
                        sliderControl.slider.minValue = 0.0f;
                        sliderControl.slider.maxValue = 1.0f;
                    }

                    // Special Case drawFrequency (always 1-200Hz)
                    else if (inputName == "drawFrequency")
                    {
                        sliderControl.slider.minValue = 1.0f;
                        sliderControl.slider.maxValue = 200.0f;
                    }

                    else
                    {
                        // TODO - fix the negative UI case - it's not rendering as expected
                        if (initialValue < 0)
                        {
                            sliderControl.slider.minValue = 2.0f * initialValue;
                            sliderControl.slider.maxValue = 2.0f * -initialValue;
                        }
                        if (initialValue > 0)
                        {
                            sliderControl.slider.minValue = 0.01f * initialValue;
                            sliderControl.slider.maxValue = 2.0f * initialValue;
                        }
                        else
                        {
                            sliderControl.slider.minValue = 0.0f;
                            sliderControl.slider.maxValue = 1.0f;
                        }
                    }

                    sliderControl.slider.value = initialValue;
                    sliderControl.inputName.text = inputName;
                    sliderControl.inputField.text = initialValue.ToString("F2");
                    sliderControl.sensation = sensation;
                    sliderControl.blockInput = input;
                }

                else if (input.IsVisible == true)
                {
                    var XYZInputRow = (GameObject)Instantiate(Resources.Load("XYZControl"), contentRect.transform);

                    inputRows.Add(XYZInputRow);

                    var vector3Control = XYZInputRow.GetComponent<SensationSourceVector3Control>();
                    vector3Control.sensation = sensation;
                    vector3Control.blockInput = input;
                    vector3Control.inputName.text = inputName;

                    vector3Control.xValue.text = input.Value.x.ToString();
                    vector3Control.yValue.text = input.Value.y.ToString();
                    vector3Control.zValue.text = input.Value.z.ToString();
                }

                sensation.enabled = true;
            }
        }
    }
}