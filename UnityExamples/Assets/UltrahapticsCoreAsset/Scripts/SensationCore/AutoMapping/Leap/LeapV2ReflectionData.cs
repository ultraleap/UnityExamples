using System.Linq;
using UnityEngine;

namespace UltrahapticsCoreAsset
{
    internal class LeapV2ReflectionData : LeapCommonReflectionData, ILeapReflectionData
    {
        private bool pointIsAbovePlane(UnityEngine.Vector3 point, UnityEngine.Vector3 origin, UnityEngine.Vector3 normal)
        {
            var plane = new Plane(normal, origin);
            return plane.GetSide(point);
        }

        private bool IsLeft(UnityEngine.Object hand)
        {
            var planeOrigin = PalmPosition(hand);
            var planeNormal = UnityEngine.Vector3.Cross(PalmDirection(hand), PalmNormal(hand));
            var thumbPosition = GetBoneCenter(hand, LeapDataStructures.Finger.THUMB, LeapDataStructures.Bone.DISTAL);
            var pinkyPosition = GetBoneCenter(hand, LeapDataStructures.Finger.PINKY, LeapDataStructures.Bone.DISTAL);
            return pointIsAbovePlane(thumbPosition, planeOrigin, planeNormal) && !pointIsAbovePlane(pinkyPosition, planeOrigin, planeNormal);
        }

        private bool IsRight(UnityEngine.Object hand)
        {
            var planeOrigin = PalmPosition(hand);
            var planeNormal = UnityEngine.Vector3.Cross(PalmDirection(hand), PalmNormal(hand));
            var thumbPosition = GetBoneCenter(hand, LeapDataStructures.Finger.THUMB, LeapDataStructures.Bone.DISTAL);
            var pinkyPosition = GetBoneCenter(hand, LeapDataStructures.Finger.PINKY, LeapDataStructures.Bone.DISTAL);
            return !pointIsAbovePlane(thumbPosition, planeOrigin, planeNormal) && pointIsAbovePlane(pinkyPosition, planeOrigin, planeNormal);
        }

        public LeapV2ReflectionData(bool usingTestLeap = false)
            : base(usingTestLeap)
        {}

        public UnityEngine.Object[] FindLeftHands()
        {
            return FindAnyHands()
                .Where(x => IsLeft(x))
                .ToArray();
        }

        public UnityEngine.Object[] FindRightHands()
        {
            return FindAnyHands()
                .Where(x => IsRight(x))
                .ToArray();
        }
    }
}
