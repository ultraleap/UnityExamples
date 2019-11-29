using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UltrahapticsCoreAsset;
public class ForcefieldBlockInputUpdater : MonoBehaviour {

    public Transform forcefieldQuad;
    public SensationSource forcefieldSensation;

    void Update () {
        var pos = forcefieldQuad.transform.position;
        var up = forcefieldQuad.transform.up * (forcefieldQuad.transform.localScale.y / 2 );
        var right = forcefieldQuad.transform.right * (forcefieldQuad.transform.localScale.x / 2);
        forcefieldSensation.Inputs["forcefieldCenter"].Value = pos;
        forcefieldSensation.Inputs["forcefieldUp"].Value = up;
        forcefieldSensation.Inputs["forcefieldRight"].Value = right;
    }
}
