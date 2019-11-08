using System.Collections.Generic;
using System;
using UnityEngine;

namespace UltrahapticsCoreAsset.UnityExamples
{
    public class SensationInputPropertyFactory : MonoBehaviour
    {

        public RectTransform contentRect;
        public GameObject dummySensationTransform;
        public Transform activeSensationTransform;
        public FixationDropdownUI fixationDropdownUI;
        public SensationPlaybackManager playbackManager;
        public LoopToggleUI loopPlaybackUI;
        public List<GameObject> inputRows = null;
        public IAutoMapper autoMapper_;

        // These inputs should generally exist for all path-based Sensations
        private string[] commonInputNames = { "drawFrequency", "intensity" };

        // Use this for initialization
        void Start()
        {
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

        public void AddTimingUI()
        {

        }

        public void SetSensationInputsFromSensation(SensationSource sensation)
        {
            ClearInputGameObjects();

            foreach (Transform child in activeSensationTransform)
            {
                Destroy(child.gameObject);
            }

            // Determine whether Sensation supports Freeform positioning (e.g. Allow-Transform)
            bool supportsTransform;
            try
            {
                supportsTransform = sensation.GetMetaData<bool>("Allow-Transform");
            }
            catch (ArgumentException)
            {
                supportsTransform = false;
            }
            fixationDropdownUI.gameObject.SetActive(supportsTransform);

            // In the case where Sensations are defined by Points only,
            // we need to disable the Global sensation transform so that we can select points only
            dummySensationTransform.gameObject.SetActive(supportsTransform);

            foreach (SensationBlockInput input in sensation.Inputs)
            {
                string inputName = input.Name;
                var minValue = float.NegativeInfinity;
                var maxValue = float.PositiveInfinity;

                // Ignore Time input
                if (inputName == "t") {
                    continue;
                }

                // Ignore invisible inputs
                if (!input.IsVisible)
                {
                    continue;
                }

                // It is currently possible not to have an Input Type
                // Via MetaData we can inject arbitrary Types - This example checks for 'MetaType' data.
                if (input.Type.Length == 0 || input.Type == "")
                {
                    // No Block Input Type specified
                    // Check for MetaTypes...
                    try
                    {
                        string metaType = input.GetMetaData<string>("MetaType");
                        if (metaType == "Boolean")
                        {
                            // Get the name of the point input
                            var CheckboxObject = (GameObject)Instantiate(Resources.Load("BooleanControl"), contentRect.transform);

                            var checkboxControl = CheckboxObject.GetComponent<SensationSourceCheckboxControl>();
                            checkboxControl.sensation = sensation;
                            checkboxControl.blockInput = input;
                            checkboxControl.inputName.text = input.Name;
                            checkboxControl.checkbox.isOn = input.Value.x > 0;
                        }
                    }
                    catch
                    {
                        //No Input type or MetaType could be determined for input: " + input.Name + " - assume Vector3")
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
                }

                try
                {
                    minValue = float.Parse(input.GetMetaData<string>("Min-Value"));
                    maxValue = float.Parse(input.GetMetaData<string>("Max-Value"));
                }
                catch
                {
                    if (Array.IndexOf(commonInputNames, inputName) == -1)
                    {
                        Debug.Log("No Min-Value/Max-Value MetaData for input: " + input.Name + " on Block: " + sensation.SensationBlock);
                    }
                }
 
                // Ignore displaying auto-mapped values (which may be non-hidden)
                if (autoMapper_.HasValueForInputName(inputName)) {
                    continue;
                }

                if (input.Type == "Scalar")
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
                        if (!float.IsNegativeInfinity(minValue) && !float.IsPositiveInfinity(maxValue))
                        {
                            sliderControl.slider.minValue = minValue;
                            sliderControl.slider.maxValue = maxValue;

                        }
                        else
                        {
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
                    }

                    sliderControl.slider.value = initialValue;
                    sliderControl.inputName.text = inputName;
                    sliderControl.inputField.text = initialValue.ToString("F2");
                    sliderControl.sensation = sensation;
                    sliderControl.blockInput = input;
                }

                else if (input.Type == "Point")
                {
                    // Get the name of the point input
                    var PointObject = (GameObject)Instantiate(Resources.Load("Point"), activeSensationTransform.transform);
                    var pointUI = PointObject.GetComponent<SensationPointUI>();

                    PointObject.transform.localPosition = new Vector3(input.Value.x, input.Value.y, input.Value.z);

                    pointUI.sensation = sensation;
                    pointUI.blockInput = input;
                    pointUI.gameObject.name = input.Name;
                }

                // TODO:, add Time-dependent controls for Repeat Forever, Repetitions.
                bool isFinite = false;
                try
                {
                    isFinite = sensation.GetMetaData<bool>("IsFinite");
                }
                catch (ArgumentException)
                {
                    isFinite = false;
                }
                loopPlaybackUI.SetLoopActive(isFinite);
                playbackManager.SetLooping(isFinite);
            }
        }
    }
}