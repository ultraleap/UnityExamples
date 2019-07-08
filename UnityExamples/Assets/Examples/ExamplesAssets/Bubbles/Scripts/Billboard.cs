using UnityEngine;
using System.Collections;

namespace UltrahapticsCoreAsset.Examples.Bubbles
{
    public class Billboard : MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            //Make the transform point towards the camera.
            transform.LookAt(Camera.main.transform.position);
        }
    }
}