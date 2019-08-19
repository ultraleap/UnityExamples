using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UltrahapticsCoreAsset.UnityExamples
{
	public class SensationSourceCheckboxControl : MonoBehaviour
	{
		public Toggle checkbox;
		public SensationSource sensation;
		public Text inputName;
		public SensationBlockInput blockInput;

		// Use this for initialization
		void Start()
		{
			checkbox.onValueChanged.AddListener(delegate { CheckboxToggled(); });
		}

		void CheckboxToggled()
		{
			int boolInt = checkbox.isOn ? 1 : 0;

			if (sensation != null && blockInput != null)
			{
				sensation.Inputs[blockInput.Name].Value = new Vector3(boolInt, 0, 0);
			}
		}
	}
}
	
