using UnityEngine;

namespace UltrahapticsCoreAsset.Examples.Bubbles
{
	public class BubbleBehaviour : MonoBehaviour
	{
		public Sprite _bubblePopSprite;

		[Range(0.0001f,0.2f)] 
		public float _bubblePopTime = .2f;

		[Range(0.1f,3.0f)] 
		public float _bubblePopSpriteScale = .6f;

		private Rigidbody _rigidBody;
		private float _adjustmentTimer = 0f;
		private float _targetScale = 1.0f;
		private float _currentScale = 0f;
        private float _popHeight = 0.4f;
        private bool _targetScaleReached = false;

		void Awake()
		{
			_rigidBody = gameObject.GetComponent<Rigidbody>();
			
			// Choose a random scale for the full sized bubble
			_targetScale = Random.Range(.015f, .035f);

			// Set the initial scale to zero
			gameObject.transform.localScale = Vector3.zero;
		}

		void Update()
		{
			if (transform.position.y > _popHeight)
			{
				Destroy(gameObject);
				return;
			}

			AdvanceBubble(Time.deltaTime);
		}

        public void PopBubble(GameObject bubble)
        {
			// change the sprite image to popped
			var spriteRenderer = GetComponentInChildren<SpriteRenderer>();
			if (spriteRenderer != null) {
				spriteRenderer.sprite = _bubblePopSprite;
				// Need to add a scale to the popped sprite
				var newScale = _bubblePopSpriteScale;
				spriteRenderer.transform.localScale = new Vector3 (newScale, newScale, newScale);
			}
			// delay the destruction of the gameobject to allow the user to register the image change
			Destroy(gameObject, _bubblePopTime);
		}
			

		public void AdvanceBubble(float time)
		{
			// Check if the bubble has grown to the target scale
			if (!_targetScaleReached)
			{
				// Bubble is still growing so increase the current scale
				_currentScale += 0.03f * time;

				if (_currentScale >= _targetScale)
				{
					// Avoid overscaling when large amount of time is given
					_currentScale = _targetScale;

					// Bubble has finished growing so add some force to 'release' it
					var randX = Random.Range(-.4f, .4f);
					var randY = Random.Range(6, 8);
					var randZ = Random.Range(-.4f, .4f);
					_rigidBody.AddForce(new Vector3(randX, randY, randZ));
					_targetScaleReached = true;
				}

				gameObject.transform.localScale = new Vector3(_currentScale, _currentScale, _currentScale);
			}

			if (_targetScaleReached)
			{
				// Adjust the position of X and Z towards zero to prevent bubbles floating away from array
				_adjustmentTimer += time;
				if (_adjustmentTimer > .01f)
				{
					if (transform.position.x > 0)
					{
						transform.position -= new Vector3(0.00002f, 0, 0);
					}
					else if (transform.position.x < 0)
					{
						transform.position += new Vector3(0.00002f, 0, 0);
					}
					if (transform.position.z > 0)
					{
						transform.position -= new Vector3(0, 0, 0.00002f);
					}
					else if (transform.position.z < 0)
					{
						transform.position += new Vector3(0, 0, 0.0002f);
					}
					_adjustmentTimer = 0;
				}
			}
		}
	}
}
