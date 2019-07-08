using UnityEngine;

namespace UltrahapticsPoster
{
	public class PosterGraphicsController : MonoBehaviour
	{
        public GameObject lightningBoltParticles;
        public GameObject orbSphere;
		[SerializeField] private ParticleSystem _particleSystem;

		// Play the particle effects - Called if hand enters the scene
		public void PlayEffects()
		{
			_particleSystem.Play();
            lightningBoltParticles.SetActive(true);
		}

		// Stop the particle effects - Called if hand is not in the scene
		public void StopEffects()
		{
			_particleSystem.Stop();
            lightningBoltParticles.SetActive(false);
		}
	}
}
