using System;
using System.Linq;
using System.Reflection;

namespace UltrahapticsCoreAsset
{
    internal class LeapV4ReflectionData : LeapCommonReflectionData, ILeapReflectionData
    {
        private Type chirality_;
        private MethodInfo handedness_;

        public LeapV4ReflectionData(bool usingTestLeap = false)
            : base(usingTestLeap)
        {
            chirality_ = ReflectionUtilities.GetType("Chirality", usingTestLeap);
            if (chirality_ == null)
            {
                throw new Exception("No Chirality type found");
            }

            var handednessProperty = HandModel.GetProperty("Handedness");

            if (handednessProperty == null)
            {
                throw new Exception("HandModel : Missing Handedness property");
            }

            handedness_ = handednessProperty.GetGetMethod();
            if (handedness_ == null)
            {
                throw new Exception("HandModel : Missing Handedness property get method");
            }
        }

        public UnityEngine.Object[] FindLeftHands()
        {
            return FindAnyHands()
                .Where(x => Enum.GetName(chirality_, handedness_.Invoke(x, null)) == "Left")
                .ToArray();
        }

        public UnityEngine.Object[] FindRightHands()
        {
            return FindAnyHands()
                .Where(x => Enum.GetName(chirality_, handedness_.Invoke(x, null)) == "Right")
                .ToArray();
        }
    }
}
