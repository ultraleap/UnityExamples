using System.Collections.Generic;
using UnityEngine;

namespace UltrahapticsCoreAsset.Examples.Polyline
{
    public class ControlPointParticleRenderer : MonoBehaviour
    {

        public ParticleSystem ControlPointParticleSystem;
        private ParticleSystem.Particle[] particlePoints_;
        public SensationEmitter SensationEmitter;

        public Color Color;

        [Range(0.001f,0.05f)]
        public float Size = 0.01f;

        // Check required Components are available, report errors if not.
        void Start()
        {
            if (SensationEmitter == null)
            {
                Debug.LogError("Control Point Particle Renderer requires a Sensation Emitter. Ensure one is set in the Inspector Panel!");
            }
            if (ControlPointParticleSystem == null)
            {
                Debug.LogError("Control Point Particle Renderer requires a Particle System. Ensure one is set in the Inspector Panel!");
            }
            else
            {
                // Ensure that the particle System is always in a non-looping state - we update the particle positions every frame
                var main = ControlPointParticleSystem.main;
                main.loop = false;
            }
        }

        // Update is called once per frame
        void Update()
        {
            Render();
        }

        // Clear the history if Component is disabled
        private void OnDisable()
        {
            ControlPointParticleSystem.Clear();
        }

        private void CreateControlPointParticles(List<UnityEngine.Vector4> focalPoints, Color color, float radius)
        {
            // The number of particles is driven by the History Size of Sensation Emitter
            var numPoints = focalPoints.Count;
            particlePoints_ = new ParticleSystem.Particle[numPoints];

            var i = 0;
            foreach (var focalPoint in focalPoints)
            {
                var focalPointInEmitterSpace = new UnityEngine.Vector3(focalPoint.x, focalPoint.y, focalPoint.z);
                var focalPointInUnitySpace = UnityToEmitterSpace.Transform.inverse * focalPointInEmitterSpace;
                if (SensationEmitter.ArrayTransform != null)
                {
                    focalPointInUnitySpace = SensationEmitter.ArrayTransform.TransformPoint(focalPointInUnitySpace);
                }
                particlePoints_[i].position = focalPointInUnitySpace;
                particlePoints_[i].startSize = radius;
                particlePoints_[i].startColor = color;
                i += 1;
            }

            if (ControlPointParticleSystem != null)
            {
                ControlPointParticleSystem.SetParticles(particlePoints_, numPoints);
            }
        }

        private void Render()
        {
            if (SensationCore.Instance != null && SensationCore.Instance.IsEmitterConnected())
            {
                var focalPoints = SensationCore.Instance.GetEvaluationHistory();
                if (focalPoints != null)
                {
                    CreateControlPointParticles(focalPoints, Color, Size);
                }
            }
        }
    }
}

