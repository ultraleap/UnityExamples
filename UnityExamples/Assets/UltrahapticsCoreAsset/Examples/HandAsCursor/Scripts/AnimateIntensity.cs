using UnityEngine;

namespace UltrahapticsCoreAsset.Examples
{
    public class AnimateIntensity : MonoBehaviour
    {
        public SensationSource Sensation;
        public float Intensity;

        // Update is called once per frame
        void Update()
        {
            if (Sensation != null)
            {
                Sensation.Inputs["intensity"].Value = new UnityEngine.Vector3(Intensity, 0, 0);
            }
        }
    }
}
