using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

namespace UltrahapticsCoreAsset
{
    public enum LeapVersion { LeapV2, LeapV4 }

    public class LeapDataSource : MonoBehaviour, IDataSource
    {
        public LeapVersion Version = LeapVersion.LeapV4;

        private readonly Dictionary<string, UnityEngine.Vector3> data_ = new Dictionary<string, UnityEngine.Vector3>();

        private const char Delimiter = '_';

        private UnityEngine.Object oldestHand_;
        private readonly IPalmIdentifiers genericPalmIdentifiers_ = new PalmIdentifiers();
        private readonly IWristIdentifiers genericWristIdentifiers_ = new WristIdentifiers();
        private readonly ReadOnlyCollection<string> genericBonePositionsLookup_ = BonePositionLookup();

        private UnityEngine.Object leftHand_;
        private readonly IPalmIdentifiers leftHandPalmIdentifiers_ = new PalmIdentifiers("leftHand_");
        private readonly IWristIdentifiers leftHandWristIdentifiers_ = new WristIdentifiers("leftHand_");
        private readonly ReadOnlyCollection<string> leftHandBonePositionsLookup_ = BonePositionLookup("leftHand_");

        private UnityEngine.Object rightHand_;
        private readonly IPalmIdentifiers rightHandPalmIdentifiers_ = new PalmIdentifiers("rightHand_");
        private readonly IWristIdentifiers rightHanddWristIdentifiers_ = new WristIdentifiers("rightHand_");
        private readonly ReadOnlyCollection<string> rightHandBonePositionsLookup_ = BonePositionLookup("rightHand_");

        private ILeapReflectionData leapReflectionData_;

        public string[] GetAvailableDataItemsForType<T>()
        {
            if (typeof(T) == typeof(UnityEngine.Vector3))
            {
                return data_.Keys.ToArray();
            }
            return null;
        }

        public T GetDataItemByName<T>(string name)
        {
            if (!data_.ContainsKey(name))
            {
                throw new NullReferenceException();
            }
            return (T)(object)data_[name]; // Force conversion from Vector3 to T (which must be Vector3)
        }

        protected void OnEnable()
        {
            try
            {
                if (Version == LeapVersion.LeapV2)
                {
                    leapReflectionData_ = new LeapV2ReflectionData(usingFakeLeapHands_);
                }
                else
                {
                    leapReflectionData_ = new LeapV4ReflectionData(usingFakeLeapHands_);
                }
                this.RegisterToAutoMapper();
            }
            catch(Exception e)
            {
                UCA.Logger.LogWarning("Leap Data Source requested HandModel/FingerModel data, but was unable to obtain it. Please check that the Leap Motion Core Assets for Unity are included with your project.\n" + e.Message);
                enabled = false;
            }
        }

        private bool HandIsPresent(UnityEngine.Object hand)
        {
            if (hand != null)
            {
                if (Version == LeapVersion.LeapV4)
                {
                    var handComponent = hand as UnityEngine.Component;
                    if (handComponent == null)
                    {
                        return false;
                    }
                    return handComponent.gameObject.activeInHierarchy;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        private void UpdateVisibleHands()
        {
            if (!HandIsPresent(leftHand_))
            {
                leftHand_ = leapReflectionData_.FindLeftHands().FirstOrDefault();
            }

            if (!HandIsPresent(rightHand_))
            {
                rightHand_ = leapReflectionData_.FindRightHands().FirstOrDefault();
            }

            if (!HandIsPresent(oldestHand_))
            {
                if (HandIsPresent(leftHand_))
                {
                    oldestHand_ = leftHand_;
                }

                if (HandIsPresent(rightHand_))
                {
                    oldestHand_ = rightHand_;
                }
            }
        }

        protected void Update()
        {
            UpdateVisibleHands();

            if (HandIsPresent(oldestHand_))
            {
                data_["virtualObjectOriginInVirtualSpace"] = leapReflectionData_.PalmPosition(oldestHand_);
                data_["virtualObjectXInVirtualSpace"] = leapReflectionData_.PalmXAxis(oldestHand_);
                data_["virtualObjectYInVirtualSpace"] = leapReflectionData_.PalmYAxis(oldestHand_);
                data_["virtualObjectZInVirtualSpace"] = leapReflectionData_.PalmZAxis(oldestHand_);
                AssignPalmPositionAndOrientation(genericPalmIdentifiers_, oldestHand_);
                AssignWristPosition(genericWristIdentifiers_, oldestHand_);
                AssignBonePositions(genericBonePositionsLookup_, oldestHand_);
            }
            else
            {
                data_["virtualObjectOriginInVirtualSpace"] = UnityEngine.Vector3.zero;
                data_["virtualObjectXInVirtualSpace"] = UnityEngine.Vector3.zero;
                data_["virtualObjectYInVirtualSpace"] = UnityEngine.Vector3.zero;
                data_["virtualObjectZInVirtualSpace"] = UnityEngine.Vector3.zero;
                ZeroPalmPositionAndOrientation(genericPalmIdentifiers_);
                ZeroWristPosition(genericWristIdentifiers_);
                ZeroBonePositions(genericBonePositionsLookup_);
            }

            if (HandIsPresent(leftHand_))
            {
                data_["leftHand_present"] = UnityEngine.Vector3.one;
                AssignPalmPositionAndOrientation(leftHandPalmIdentifiers_, leftHand_);
                AssignWristPosition(leftHandWristIdentifiers_, leftHand_);
                AssignBonePositions(leftHandBonePositionsLookup_, leftHand_);
            }
            else
            {
                data_["leftHand_present"] = UnityEngine.Vector3.zero;
                ZeroPalmPositionAndOrientation(leftHandPalmIdentifiers_);
                ZeroWristPosition(leftHandWristIdentifiers_);
                ZeroBonePositions(leftHandBonePositionsLookup_);
            }

            if (HandIsPresent(rightHand_))
            {
                data_["rightHand_present"] = UnityEngine.Vector3.one;
                AssignPalmPositionAndOrientation(rightHandPalmIdentifiers_, rightHand_);
                AssignWristPosition(rightHanddWristIdentifiers_, rightHand_);
                AssignBonePositions(rightHandBonePositionsLookup_, rightHand_);
            }
            else
            {
                data_["rightHand_present"] = UnityEngine.Vector3.zero;
                ZeroPalmPositionAndOrientation(rightHandPalmIdentifiers_);
                ZeroWristPosition(rightHanddWristIdentifiers_);
                ZeroBonePositions(rightHandBonePositionsLookup_);
            }
        }

        private bool usingFakeLeapHands_ = false;
        internal void SetUsingFakeLeapHands(bool usingFakes)
        {
            usingFakeLeapHands_ = usingFakes;
        }

        private interface IPalmIdentifiers
        {
            string Position { get; }
            string Direction { get; }
            string Normal { get; }
            string ScaledDirection { get; }
            string ScaledTransverse { get; }
        }
        private class PalmIdentifiers : IPalmIdentifiers
        {
            private readonly string position_;
            public string Position { get { return position_; } }

            private readonly string direction_;
            public string Direction { get { return direction_; } }

            private readonly string normal_;
            public string Normal { get { return normal_; } }

            private readonly string scaledDirection_;
            public string ScaledDirection { get { return scaledDirection_; } }

            private readonly string scaledTransverse_;
            public string ScaledTransverse { get { return scaledTransverse_; } }

            public PalmIdentifiers(string prefix = "")
            {
                position_ = prefix + "palm" + Delimiter + "position";
                direction_ = prefix + "palm" + Delimiter + "direction";
                normal_ = prefix + "palm" + Delimiter + "normal";
                scaledDirection_ = prefix + "palm" + Delimiter + "scaled_direction";
                scaledTransverse_ = prefix + "palm" + Delimiter + "scaled_transverse";
            }
        }

        private void ZeroPalmPositionAndOrientation(IPalmIdentifiers palmIdentifiers)
        {
            data_[palmIdentifiers.Position] = UnityEngine.Vector3.zero;
            data_[palmIdentifiers.Direction] = UnityEngine.Vector3.zero;
            data_[palmIdentifiers.Normal] = UnityEngine.Vector3.zero;
            data_[palmIdentifiers.ScaledDirection] = UnityEngine.Vector3.zero;
            data_[palmIdentifiers.ScaledTransverse] = UnityEngine.Vector3.zero;
        }

        private void AssignPalmPositionAndOrientation(IPalmIdentifiers palmIdentifiers, UnityEngine.Object hand)
        {
            data_[palmIdentifiers.Position] = leapReflectionData_.PalmPosition(hand);
            data_[palmIdentifiers.Direction] = leapReflectionData_.PalmDirection(hand);
            data_[palmIdentifiers.Normal] = leapReflectionData_.PalmNormal(hand);

            var palmDirectionNormalized = UnityEngine.Vector3.Normalize(data_[palmIdentifiers.Direction]);
            var crossDirectionNormalized = UnityEngine.Vector3.Normalize(UnityEngine.Vector3.Cross(palmDirectionNormalized, data_[palmIdentifiers.Normal]));

            var middleFinger = leapReflectionData_.GetBoneCenter(hand, LeapDataStructures.Finger.MIDDLE, LeapDataStructures.Bone.DISTAL);
            var halfHandLength = UnityEngine.Vector3.Distance(middleFinger, leapReflectionData_.WristPosition(hand)) / 2.0f;
            var halfHandWidth = leapReflectionData_.PalmWidth(hand) / 2.0f;

            data_[palmIdentifiers.ScaledDirection] = palmDirectionNormalized * halfHandLength;
            data_[palmIdentifiers.ScaledTransverse] = crossDirectionNormalized * halfHandWidth;
        }

        private interface IWristIdentifiers
        {
            string Position { get; }
        }
        private class WristIdentifiers : IWristIdentifiers
        {
            private readonly string position_;
            public string Position { get { return position_; } }

            public WristIdentifiers(string prefix = "")
            {
                position_ = prefix + "wrist" + Delimiter + "position";
            }
        }

        private void ZeroWristPosition(IWristIdentifiers wristIdentifiers)
        {
            data_[wristIdentifiers.Position] = UnityEngine.Vector3.zero;
        }

        private void AssignWristPosition(IWristIdentifiers wristIdentifiers, UnityEngine.Object hand)
        {
            data_[wristIdentifiers.Position] = leapReflectionData_.WristPosition(hand);
        }

        private static ReadOnlyCollection<string> BonePositionLookup(string prefix = "")
        {
            var bonePositionsLookup = new List<string>();
            foreach (var finger in EnumExtensions.Values<LeapDataStructures.Finger>())
            {
                var fingerDescription = LeapCommonReflectionData.FingerToString(finger);
                foreach (var bone in EnumExtensions.Values<LeapDataStructures.Bone>())
                {
                    var boneDescription = LeapCommonReflectionData.BoneToString(bone);
                    var fullName = prefix + fingerDescription + Delimiter + boneDescription + Delimiter + "position";
                    bonePositionsLookup.Add(fullName);
                }
            }

            return bonePositionsLookup.AsReadOnly();
        }

        private void ZeroBonePositions(ReadOnlyCollection<string> bonePositionLookup)
        {
            var i = 0;
            foreach (var finger in EnumExtensions.Values<LeapDataStructures.Finger>())
            {
                foreach (var bone in EnumExtensions.Values<LeapDataStructures.Bone>())
                {
                    data_[bonePositionLookup[i++]] = UnityEngine.Vector3.zero;
                }
            }
        }

        private void AssignBonePositions(ReadOnlyCollection<string> bonePositionLookup, UnityEngine.Object hand)
        {
            var i = 0;
            foreach (var finger in EnumExtensions.Values<LeapDataStructures.Finger>())
            {
                foreach (var bone in EnumExtensions.Values<LeapDataStructures.Bone>())
                {
                    data_[bonePositionLookup[i++]] = leapReflectionData_.GetBoneCenter(hand, finger, bone);
                }
            }
        }

    }
}
