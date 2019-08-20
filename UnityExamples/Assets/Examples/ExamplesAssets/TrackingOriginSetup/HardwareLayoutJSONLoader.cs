using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;

public class HardwareLayoutJSONLoader : MonoBehaviour
{
    // This JSON file will allow users to set the white/black list of sensations
    private string trackingOriginAssetFilename = "/TrackingOrigin.json";

    [Serializable]
    public class TrackingOriginTransformList
    {
        public List<string> trackingPosition;
        public List<string> trackingRotation;
    }

    public bool fileOverridesProject = false;
    public Transform trackingOrigin;
    public TrackingOriginTransformList trackingOriginList;

    // Start is called before the first frame update
    void Start()
    {
        if (fileOverridesProject)
        {
            SetTrackingOriginFromJSON();
        }
    }

    void SetTrackingOriginFromJSON()
    {
        var file = Application.streamingAssetsPath + trackingOriginAssetFilename;
        string trackingOriginJSON = File.ReadAllText(file);
        trackingOriginList = JsonUtility.FromJson<TrackingOriginTransformList>(trackingOriginJSON);

        Debug.Log(trackingOriginList);

        if (trackingOriginList.trackingPosition.Count == 3 && trackingOriginList.trackingRotation.Count == 3)
        {
            float xPos = float.Parse(trackingOriginList.trackingPosition[0]);
            float yPos = float.Parse(trackingOriginList.trackingPosition[1]);
            float zPos = float.Parse(trackingOriginList.trackingPosition[2]);

            float xRot = float.Parse(trackingOriginList.trackingRotation[0]);
            float yRot = float.Parse(trackingOriginList.trackingRotation[1]);
            float zRot = float.Parse(trackingOriginList.trackingRotation[2]);

            Debug.Log(String.Format("Overriding TrackingOrigin Position: x={0}, y={1}, z={2}", xPos, yPos, zPos));
            Debug.Log(String.Format("Overriding TrackingOrigin Rotation: x={0}, y={1}, z={2}", xRot, yRot, zRot));

            Vector3 pos = new Vector3(xPos, yPos, zPos);
            Quaternion rot = Quaternion.Euler(new Vector3(xRot, yRot, zRot));
            trackingOrigin.SetPositionAndRotation(pos, rot);
        }
        else
        {
            Debug.Log("Unable to get a trackingPosition and trackingRotation from JSON");
        }
    }

    // We can also 'Live Update' the Tracking origin, by adding a Keyboard shortcut 'T'
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            SetTrackingOriginFromJSON();
        }
    }
}
