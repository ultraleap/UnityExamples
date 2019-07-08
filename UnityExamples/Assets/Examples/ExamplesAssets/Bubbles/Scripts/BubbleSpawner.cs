using UnityEngine;

namespace UltrahapticsCoreAsset.Examples.Bubbles
{
	public class BubbleSpawner : MonoBehaviour
	{
		[SerializeField] private GameObject _bubble;
		[SerializeField] private int _numberOfBubblesToSpawn;
		[SerializeField] private bool _moreBubbles;
		[SerializeField] private float _spawnTime;

		private float _spawnTimer;
		private float _bubbleLifeTime = 4f; // assumed a total lifetime of approx 3 seconds
		private float _maxBubbleHeight = 0.4f;

		// Configuration
		[HideInInspector] public float XRange = 0.07f;
		[HideInInspector] public float ZRange = 0.07f;

		private void Start()
		{
			var interval = _bubbleLifeTime / _numberOfBubblesToSpawn;
			for (int i = 0; i < _numberOfBubblesToSpawn; i++)
			{
				SpawnBubble((i + 1) * interval);
			}
			_spawnTimer = _spawnTime;
		}

		// Update is called once per frame
		void Update()
		{
			_spawnTimer += Time.deltaTime;
			//spawn a bubble every half a second at a random position
			if (_spawnTimer >= _spawnTime)
			{
				if(_moreBubbles)
				{
					var numberToSpawn = Random.Range(1, 3);
					for(int i = 0; i < numberToSpawn; ++i)
					{
						SpawnBubble();
					}
				}
				else
				{
					SpawnBubble();
				}
				_spawnTimer = 0;
			}
		}

		private void SpawnBubble(float time = 0f)
		{
			var rndX = Random.Range(-XRange, XRange);
			var posY = _maxBubbleHeight * (time / _bubbleLifeTime);
			var rndZ = Random.Range(-ZRange, ZRange);
			var pos = new Vector3(rndX, posY, rndZ);
			Instantiate(_bubble, pos, Quaternion.identity).GetComponent<BubbleBehaviour>().AdvanceBubble(time);
		}
	}
}
