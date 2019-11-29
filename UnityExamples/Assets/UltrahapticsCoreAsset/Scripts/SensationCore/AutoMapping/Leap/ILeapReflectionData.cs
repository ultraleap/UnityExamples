namespace UltrahapticsCoreAsset
{
    interface ILeapReflectionData
    {
        UnityEngine.Vector3 PalmPosition(object hand);
        UnityEngine.Vector3 PalmDirection(object hand);
        UnityEngine.Vector3 PalmNormal(object hand);
        float PalmWidth(object hand);
        UnityEngine.Vector3 PalmXAxis(object hand);
        UnityEngine.Vector3 PalmYAxis(object hand);
        UnityEngine.Vector3 PalmZAxis(object hand);
        UnityEngine.Vector3 WristPosition(object hand);
        UnityEngine.Vector3 GetBoneCenter(UnityEngine.Object hand, LeapDataStructures.Finger fingerIndex, LeapDataStructures.Bone boneIndex);

        UnityEngine.Object[] FindAnyHands();
        UnityEngine.Object[] FindLeftHands();
        UnityEngine.Object[] FindRightHands();
    }
}
