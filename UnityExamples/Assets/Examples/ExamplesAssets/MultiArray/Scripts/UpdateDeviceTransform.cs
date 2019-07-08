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
            var tmpTransform = new GameObject().transform;
            tmpTransform.gameObject.hideFlags = HideFlags.HideAndDontSave;

            // To use Unity Transform directly to represent the device transform we currently have to swap Y-Z coordinates.
            Vector3 rotationVector = new Vector3(transform.eulerAngles.x, transform.eulerAngles.z, transform.eulerAngles.y);
            Vector3 positionVector = new Vector3(transform.localPosition.x, transform.localPosition.z, transform.localPosition.y);

            Quaternion rotation = Quaternion.Euler(rotationVector);
            tmpTransform.SetPositionAndRotation(positionVector, rotation);

            // Update the Device Transform
            SensationCore.Instance.SetDeviceTransform(deviceID, tmpTransform);
            Destroy(tmpTransform.gameObject);

            transform.hasChanged = false;
        }
    }
}
