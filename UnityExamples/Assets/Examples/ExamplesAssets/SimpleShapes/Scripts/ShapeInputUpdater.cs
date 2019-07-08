using UnityEngine;
using System.Collections;

namespace UltrahapticsCoreAsset.Examples.SimpleShapes
{
    public class ShapeInputUpdater : MonoBehaviour
    {
        public SensationSource Sensation;
        public Transform ObjectTransform;

        private UnityEngine.Vector3 lastDir;

        void Start()
        {
            lastDir = transform.up;
        }

        // The Sphere and Cylinder Sensations can have their radius updated by changing the GameObject's Transform scale
        protected virtual void Update()
        {
            Sensation.Inputs["radius"].Value = new Vector3(ObjectTransform.localScale.x / 2, 0, 0);

            if (transform.up != lastDir)
            {
                Sensation.Inputs["extrusionDirection"].Value = transform.up;
                lastDir = transform.up;
            }
        }
    }
}