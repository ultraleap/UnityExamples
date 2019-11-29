using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace UltrahapticsCoreAsset
{
    internal abstract class LeapCommonReflectionData
    {
        private static readonly ReadOnlyCollection<string> boneNames_;
        public static string BoneToString(LeapDataStructures.Bone bone)
        {
            return boneNames_.ElementAt((int)bone);
        }
        private static ReadOnlyCollection<string> BoneToStringCollection()
        {
            var boneToString = new List<string>();
            foreach (var bone in EnumExtensions.Values<LeapDataStructures.Bone>())
            {
                boneToString.Add(bone.GetDescription());
            }
            return boneToString.AsReadOnly();
        }

        private static readonly ReadOnlyCollection<string> fingerNames_;
        public static string FingerToString(LeapDataStructures.Finger finger)
        {
            return fingerNames_.ElementAt((int)finger);
        }
        private static ReadOnlyCollection<string> FingerToStringCollection()
        {
            var fingerToString = new List<string>();
            foreach (var finger in EnumExtensions.Values<LeapDataStructures.Finger>())
            {
                fingerToString.Add(finger.GetDescription());
            }
            return fingerToString.AsReadOnly();
        }

        static LeapCommonReflectionData()
        {
            boneNames_ = BoneToStringCollection();
            fingerNames_ = FingerToStringCollection();
        }

        protected readonly Type HandModel;
        private readonly FieldInfo fingers_;

        private readonly MethodInfo palmPosition_;
        private readonly MethodInfo palmDirection_;
        private readonly MethodInfo palmNormal_;
        private readonly FieldInfo palmWidth_;

        private readonly MethodInfo wristPosition_;

        public UnityEngine.Vector3 PalmPosition(object hand)
        {
            return (UnityEngine.Vector3)palmPosition_.Invoke(hand, null);
        }
        public UnityEngine.Vector3 PalmDirection(object hand)
        {
            return (UnityEngine.Vector3)palmDirection_.Invoke(hand, null);
        }
        public UnityEngine.Vector3 PalmNormal(object hand)
        {
            return (UnityEngine.Vector3)palmNormal_.Invoke(hand, null);
        }

        public float PalmWidth(object hand)
        {
            return (float)palmWidth_.GetValue(hand);
        }
        public UnityEngine.Vector3 PalmXAxis(object hand)
        {
            var y = PalmYAxis(hand);
            var z = PalmZAxis(hand);
            return UnityEngine.Vector3.Cross(y, z);
        }
        public UnityEngine.Vector3 PalmYAxis(object hand)
        {
            return (UnityEngine.Vector3)palmNormal_.Invoke(hand, null);
        }
        public UnityEngine.Vector3 PalmZAxis(object hand)
        {
            return (UnityEngine.Vector3)palmDirection_.Invoke(hand, null);
        }
        public UnityEngine.Vector3 WristPosition(object hand)
        {
            return (UnityEngine.Vector3)wristPosition_.Invoke(hand, null);
        }

        private MethodInfo boneCenter_;
        public UnityEngine.Vector3 GetBoneCenter(UnityEngine.Object hand,LeapDataStructures.Finger fingerIndex, LeapDataStructures.Bone boneIndex)
        {
            var fingers = (UnityEngine.Object[])fingers_.GetValue(hand);
            var finger = fingers.ElementAt((int)fingerIndex);
            var parameters = new object[1]{(int)boneIndex};
            return (UnityEngine.Vector3)boneCenter_.Invoke(finger, parameters);
        }

        protected LeapCommonReflectionData(bool usingTestLeap = false)
        {
            HandModel = ReflectionUtilities.GetType("HandModel", usingTestLeap);
            if (HandModel == null)
            {
                throw new Exception("No HandModel found");
            }

            palmPosition_ = HandModel.GetMethod("GetPalmPosition");
            palmDirection_ = HandModel.GetMethod("GetPalmDirection");
            palmNormal_ = HandModel.GetMethod("GetPalmNormal");
            if (palmPosition_ == null || palmDirection_ == null || palmNormal_ == null)
            {
                throw new Exception("HandModel : Missing Palm methods");
            }

            palmWidth_ = HandModel.GetField("handModelPalmWidth");
            if (palmWidth_ == null)
            {
                throw new Exception("HandModel : Missing Palm field");
            }

            wristPosition_ = HandModel.GetMethod("GetWristPosition");
            if (wristPosition_ == null)
            {
                throw new Exception("HandModel : Missing GetWristPosition method");
            }

            fingers_ = HandModel.GetField("fingers");
            if (fingers_ == null)
            {
                throw new Exception("HandModel : Missing Fingers field");
            }

            var fingerModel = ReflectionUtilities.GetType("FingerModel", usingTestLeap);
            if (fingerModel == null)
            {
                throw new Exception("No FingerModel found");
            }

            boneCenter_ = fingerModel.GetMethod("GetBoneCenter");
            if (boneCenter_ == null)
            {
                throw new Exception("FingerModel : Missing GetBoneCenter method");
            }
        }

        public UnityEngine.Object[] FindAnyHands()
        {
            return UnityEngine.Object.FindObjectsOfType(HandModel);
        }

    }
}
