using UnityEngine;

namespace UltrahapticsCoreAsset.Examples
{

    public class SensationAnimationExample : MonoBehaviour
    {

        public SensationSource Sensation;

        // We animate this value via the Timeline and bind
        // it to the sensation's 'radius' input.
        public float AnimatedRadius = 0.02f;

        // Use this for initialization
        void Start()
        {
            // Uncomment line below if Sensation Source needs to be enabled when game starts.
            // sensation.enabled = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (Sensation)
            {
                Sensation.Inputs["radius"].Value = new UnityEngine.Vector3(AnimatedRadius, AnimatedRadius, AnimatedRadius);
            }
            else
            {
                Debug.Log("Check that Sensation Source with a 'radius' input has been connected to the 'Sensation' field!");
            }
        }
    }

}
