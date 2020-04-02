using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UltrahapticsCoreAsset;

[RequireComponent(typeof(SensationSource))]
public class SensationControl : MonoBehaviour
{
    SensationSource source;
    public Transform pathFollower;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnValidate()
    {
        source = GetComponent<SensationSource>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = pathFollower.position;
        transform.rotation = pathFollower.rotation;
    }
}
