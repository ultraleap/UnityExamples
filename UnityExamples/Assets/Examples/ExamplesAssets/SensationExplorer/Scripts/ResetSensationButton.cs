using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UltrahapticsCoreAsset.UnityExamples {
    public class ResetSensationButton : MonoBehaviour
    {
        [SerializeField] private SensationSource sensationSource;
        [SerializeField] private SensationInputPropertyFactory inputFactory;
        [SerializeField] private GameObject SensationTransform;

        // Start is called before the first frame update
        void Start()
        {

        }

        public void ResetSensationInputs()
        {
            sensationSource.inputsCache.Clear();
            sensationSource.RecreateBlockWithDefaultInputs();

            // Now force the Inputs to update
            inputFactory.SetSensationInputsFromSensation(sensationSource);

            sensationSource.enabled = false;
            sensationSource.enabled = true;
        }

        // This will re-position the SensationTransform Object to the 0,0.2,0 default
        public void ResetSensationTransform()
        {
            sensationSource.inputsCache.Clear();

            SensationTransform.transform.localPosition = new Vector3(0, 0.2f, 0);
            SensationTransform.transform.localRotation = new Quaternion(0, 0, 0, 0);
            SensationTransform.transform.localScale = new Vector3(1, 1, 1);

            sensationSource.enabled = false;
            sensationSource.enabled = true;
        }


        // Update is called once per frame
        void Update()
        {

        }
    }
}