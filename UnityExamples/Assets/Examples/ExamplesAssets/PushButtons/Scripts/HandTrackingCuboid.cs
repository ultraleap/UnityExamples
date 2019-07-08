using UnityEngine;
using UltrahapticsCoreAsset;

using Vector3 = UnityEngine.Vector3;

namespace ButtonSliderExample
{
	/* This script attaches a primitive, hand sized cuboid to the UCA palm. 
	 * This is then used as a proxy for triggering any control collider. */
	public class HandTrackingCuboid : MonoBehaviour {

		private IAutoMapper _autoMapper;
		private bool hasValueForName = false;

		// Use this for initialization
		void Start () {
			_autoMapper = FindObjectOfType<IAutoMapper>();
		}
		
		// Update is called once per frame
		void Update () {
			if (!hasValueForName)
			{
				// One off boolean flag.
				hasValueForName = _autoMapper.HasValueForInputName("palm_position");
			}
			
			if (hasValueForName)
			{
				// Attach to the palm
				Vector3 pos = _autoMapper.GetValueForInputName("palm_position");
				transform.position = pos;
				
				if (!pos.Equals(Vector3.zero))
				{
					// If non-zero, orientate to the hand surface
					Vector3 dir = _autoMapper.GetValueForInputName("palm_direction");
					Vector3 norm = _autoMapper.GetValueForInputName("palm_normal");
							
					transform.rotation = Quaternion.LookRotation(dir, norm);
				}
			}
		}
	}
}