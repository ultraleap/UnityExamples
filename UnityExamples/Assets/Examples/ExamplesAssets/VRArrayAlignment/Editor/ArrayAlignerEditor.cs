using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Apps.Unity
{
	[CustomEditor(typeof(ArrayAligner))]
	public class ArrayAlignerEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			ArrayAligner script = (ArrayAligner)target;
			if (GUILayout.Button("Delete Saved Array Alignment"))
			{
				script.DeleteSavedArrayPreferences();
			}
		}
	}
}
