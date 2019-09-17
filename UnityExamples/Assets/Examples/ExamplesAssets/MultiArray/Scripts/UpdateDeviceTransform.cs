using UnityEngine;
using UltrahapticsCoreAsset;

public class UpdateDeviceTransform : MonoBehaviour {

    public string deviceID = "Unknown";
    public TextMesh deviceText;

	// Use this for initialization
	void Start () {
        deviceID = name;
        deviceText.text = name;
	}
	
	// Update is called once per frame
	void Update () {
        if (transform.hasChanged)
        {
            print("The Device transform with deviceID: " + deviceID + " has changed: position: " + transform.position + " , rotation: " + transform.rotation);

            // Update the Device Transform
            SensationCore.Instance.SetDeviceTransform(deviceID, transform);

            transform.hasChanged = false;
        }
    }
}
