using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UltrahapticsCoreAsset;

// This example assumes two Ultrahaptics Devices are positoined flat on table,
// Separated by a distance of 50cm
public class MultiArrayExample : MonoBehaviour {

    [SerializeField] private GameObject _deviceObject;

    // Use this for initialization
    void Start () {
        var devices = SensationCore.Instance.GetConnectedDevices();

        var posX = 0.25f;
        var posY = 0.0f;
        var posZ = 0.0f;
        foreach (var device in devices) {
            Debug.Log("DEVICE:"  + device);
            var pos = new Vector3(posX, posY, posZ);
            var array = Instantiate(_deviceObject, pos, Quaternion.identity);

            array.name = device;

            // Update the Array Model based on the device ID (USI = Inspire, USX = Explore)
            var arrayModelTransform = array.GetComponentInChildren<Transform>().Find("ArrayModel");
            var trackingOriginTransform = array.GetComponentInChildren<Transform>().Find("TrackingOrigin");

            if (arrayModelTransform && trackingOriginTransform) {
                if (array.name.Contains("USI")) {
                    arrayModelTransform.localScale = new Vector3(0.311f, 0.01f, 0.11f);
                    trackingOriginTransform.localPosition = new Vector3(0, -0.00006f, -0.089f);
                }
                else if (array.name.Contains("USX"))
                {
                    arrayModelTransform.localScale = new Vector3(0.168f, 0.01f, 0.168f);
                    trackingOriginTransform.localPosition = new Vector3(0.0f, 0.0f, 0.121f);
                }
            }

            // Add some offset in X of 50cm for each array.
            posX -= 0.5f;

            // Add the Device to the pool of arrays
            SensationCore.Instance.AddDevice(device, array.transform);
        }
	}

    // Update is called once per frame
    void Update () {
		
	}
}
