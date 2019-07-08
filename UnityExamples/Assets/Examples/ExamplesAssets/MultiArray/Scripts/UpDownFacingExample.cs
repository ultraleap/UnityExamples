using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UltrahapticsCoreAsset;

// This example assumes that a STRATOS Inspire is positioned flat on a table
// And a STRATOS Explore is facing downwards, separated by a distance of 50cm in Unity's Y-Axis (Ultrahaptics Z-Axis)
// NOTE: The Leap Motion Controller Tracking Origin is assumed to be positioned at the default position of the STRATOS Inspire.
public class UpDownFacingExample : MonoBehaviour {

    [SerializeField] private GameObject _deviceObject;
    public float stratosHeight = 0.5f;

    public GameObject UpwardFacingArray;
    public GameObject DownwardFacingArray;

    public PalmDirectionManager palmDirectionManager;

    // Use this for initialization
    void Start()
    {
        var devices = SensationCore.Instance.GetConnectedDevices();

        var posX = 0.0f;
        var posY = 0.0f;
        var posZ = 0.0f;
        foreach (var device in devices)
        {
            Debug.Log("DEVICE:" + device);
            var pos = new Vector3(posX, posY, posZ);
            var array = Instantiate(_deviceObject, pos, Quaternion.identity);

            array.name = device;

            // Update the Array Model based on the device ID (USI = Inspire, USX = Explore)
            var arrayModelTransform = array.GetComponentInChildren<Transform>().Find("ArrayModel");
            var trackingOriginTransform = array.GetComponentInChildren<Transform>().Find("TrackingOrigin");

            if (arrayModelTransform && trackingOriginTransform)
            {

                if (array.name.Contains("USI"))
                {
                    // Set the parent to be the upwards facing Array
                    array.transform.parent = UpwardFacingArray.transform;
                    Debug.Log("Setting up STRATOS Inspire");
                    arrayModelTransform.localScale = new Vector3(0.311f, 0.01f, 0.11f);
                    trackingOriginTransform.localPosition = new Vector3(0, -0.00006f, -0.089f);

                    // NOTE: This auto sets up the device ID for the Upward facing Array in the PalmDirection Manager Script
                    palmDirectionManager.UpwardFacingArrayID = device;

                }
                else if (array.name.Contains("USX"))
                {
                    // Set the parent to be the downwards facing Array
                    array.transform.parent = DownwardFacingArray.transform;
                    Debug.Log("Setting up STRATOS Inspire");
                    arrayModelTransform.localScale = new Vector3(0.168f, 0.01f, 0.168f);

                    // Set up the STRATOS Array at 50cm above the Inspire, facing downwards
                    array.transform.position = new Vector3(0.0f, stratosHeight, 0.0f);
                    array.transform.eulerAngles = new Vector3(0.0f, 0.0f, 180.0f);
                    trackingOriginTransform.localPosition = new Vector3(0.0f, 0.0f, 0.121f);

                    // NOTE: This auto sets up the device ID for the Downward facing Array in the PalmDirection Manager Script
                    palmDirectionManager.DownwardFacingArrayID = device;
                }
            }

            // Add the Device to the pool of arrays
            SensationCore.Instance.AddDevice(device, array.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
