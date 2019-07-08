using UnityEngine;

namespace UltrahapticsCoreAsset.Examples.Bubbles
{
	public class BubbleDemoController : MonoBehaviour
	{


		// Update is called once per frame
		void Update()
		{
			if (Input.GetKeyDown (KeyCode.Escape))
				Application.Quit ();
		}
	}
}
