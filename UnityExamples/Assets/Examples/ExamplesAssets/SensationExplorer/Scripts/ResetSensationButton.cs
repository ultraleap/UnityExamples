using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UltrahapticsCoreAsset.UnityExamples {
    public class ResetSensationButton : MonoBehaviour
    {
        public SensationSource sensationSource;
        public SensationInputPropertyFactory inputFactory;

        // Start is called before the first frame update
        void Start()
        {

        }

        public void ResetSensation()
        {
            sensationSource.inputsCache.Clear();
            sensationSource.RecreateBlockWithDefaultInputs();

            // Now force the Inputs to update
            inputFactory.SetSensationInputsFromSensation(sensationSource);

            sensationSource.enabled = false;
            sensationSource.enabled = true;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}