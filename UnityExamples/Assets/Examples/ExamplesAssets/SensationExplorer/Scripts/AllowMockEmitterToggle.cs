using System.Collections;
using UnityEngine;

namespace UltrahapticsCoreAsset.Examples
{
    public class AllowMockEmitterToggle : MonoBehaviour
    {
        public SensationEmitter emitter;
        public ControlPointRenderer controlpointRenderer;

        // Start is called before the first frame update
        void Start()
        {

        }

        public void SetAllowMockEmitter(bool allowMock)
        {
            emitter.AllowMockEmitter = allowMock;
            emitter.enabled = false;
            emitter.enabled = true;
            controlpointRenderer.ControlPointParticleSystem.Clear();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
