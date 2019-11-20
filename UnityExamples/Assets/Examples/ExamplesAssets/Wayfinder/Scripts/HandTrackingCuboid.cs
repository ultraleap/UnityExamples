using UnityEngine;
using UltrahapticsCoreAsset;

using Vector3 = UnityEngine.Vector3;

/* This script attaches a primitive, hand sized cuboid to the UCA palm. 
 * This is then used as a proxy for triggering any control collider. */
public class HandTrackingCuboid : MonoBehaviour
{

    private IAutoMapper _autoMapper;
    private bool hasValueForName = false;
    public bool leftHandPresent = false;
    public bool rightHandPresent = false;

    // Use this for initialization
    void Start()
    {
        _autoMapper = FindObjectOfType<IAutoMapper>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasValueForName)
        {
            // One off boolean flag.
            hasValueForName = _autoMapper.HasValueForInputName("palm_position");
        }

        if (hasValueForName)
        {
            leftHandPresent = Mathf.RoundToInt(_autoMapper.GetValueForInputName("leftHand_present").x) != 0;
            rightHandPresent = Mathf.RoundToInt(_autoMapper.GetValueForInputName("rightHand_present").x) != 0;

            // Attach to the palm
            Vector3 pos = _autoMapper.GetValueForInputName("palm_position");
            if (pos != Vector3.zero && (leftHandPresent || rightHandPresent))
            {
                transform.position = pos;
            }
            
        }
    }
}