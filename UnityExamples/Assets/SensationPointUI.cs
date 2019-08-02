using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UltrahapticsCoreAsset.UnityExamples
{
    public class SensationPointUI : MonoBehaviour
    {
        public SensationSource sensation;
        public SensationBlockInput blockInput;

        // Start is called before the first frame update
        void Start()
        {
            // Bind the transform of the point to the block Input
            blockInput.Object = transform;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
