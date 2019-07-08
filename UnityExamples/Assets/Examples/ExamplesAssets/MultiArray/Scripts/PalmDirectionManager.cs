using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UltrahapticsCoreAsset;

// Note: This example requires you to enter the Device IDs of your connected hardware.
// A STRATOS Inspire has a device id in the form: USI:USI-########
// A STRATOS Explore has a device id in the form: USX:USX-########

// Ensure that you have set these 
public class PalmDirectionManager : MonoBehaviour
{

    public IAutoMapper MyAutoMapper;
    public bool palmFacingDownwards = true;

    [SerializeField] private GameObject UpwardFacingArray;
    [SerializeField] private GameObject DownwardFacingArray;

    [SerializeField] private GameObject UpwardDefaultTransform;
    [SerializeField] private GameObject DownwardDefaultTransform;
    [SerializeField] private GameObject DummyTransform;

    [Tooltip("Ensure that this Serial number matches your connected STRATOS Inspire, in the form 'USI:USI-########'")]
    [SerializeField] public string UpwardFacingArrayID = "USI:USI-00000001";

    [Tooltip("Ensure that this Serial number matches your connected STRATOS Explore, in the form 'USX:USX-########'")]
    [SerializeField] public string DownwardFacingArrayID = "USX:USX-00000087";

    // Update is called once per frame
    void Update()
    {
        if (DownwardFacingArray == null)
        {
            DownwardFacingArray = GameObject.Find(DownwardFacingArrayID);
            if (DownwardFacingArray == null) {
                Debug.LogError("Check PalmDirectionManager GameObject! Ensure your STRATOS Explore (downward facing array) has had its Array ID set (currently expecting an ID of: " + DownwardFacingArrayID);
            }
        }

        if (UpwardFacingArray == null)
        {
            UpwardFacingArray = GameObject.Find(UpwardFacingArrayID);
            if (UpwardFacingArray == null) {
                Debug.LogError("Check PalmDirectionManager GameObject! Ensure that your STRATOS Inspire (upward facing array) has had its Array ID set (currently expecting an ID of: " + UpwardFacingArrayID);
            }
        }

        if (MyAutoMapper.HasValueForInputName("palm_normal"))
        {
            var palmNormal = MyAutoMapper.GetValueForInputName("palm_normal");

            if (palmNormal.y > 0.0f)
            {
                palmFacingDownwards = true;
                // Momentarily move the Upwards Facing Array very far away (virtually) so that the DOWNWARDS facing array is emitting
                UpwardFacingArray.transform.SetPositionAndRotation(DummyTransform.transform.localPosition, DummyTransform.transform.rotation);
                DownwardFacingArray.transform.SetPositionAndRotation(DownwardDefaultTransform.transform.localPosition, DownwardDefaultTransform.transform.rotation);
            }
            else
            {
                palmFacingDownwards = false;
                // Momentarily move the Downwards Facing Array very far away (virtually) so that the upwards facing array is emitting
                DownwardFacingArray.transform.SetPositionAndRotation(DummyTransform.transform.localPosition, DummyTransform.transform.rotation);
                UpwardFacingArray.transform.SetPositionAndRotation(UpwardDefaultTransform.transform.localPosition, UpwardDefaultTransform.transform.rotation);
            }
        }
        else {
            Debug.LogError("Unable to get Hand Tracking Data for palm orientation detection! Ensure you have your Leap Hand Controller Prefab in the Scene! Refer to the UltrahapticsCoreAsset README.md for more details on adding Leap Motion support to the scene.");
        }
    }
}
        