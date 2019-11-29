using System.ComponentModel;

namespace UltrahapticsCoreAsset
{
    namespace LeapDataStructures
    {
        public enum Bone
        {
            [Description("metacarpal")]
            METACARPAL = 0,
            [Description("proximal")]
            PROXIMAL = 1,
            [Description("intermediate")]
            INTERMEDIATE = 2,
            [Description("distal")]
            DISTAL = 3
        }

        public enum Finger
        {
            [Description("thumb")]
            THUMB = 0,
            [Description("indexFinger")]
            INDEX = 1,
            [Description("middleFinger")]
            MIDDLE = 2,
            [Description("ringFinger")]
            RING = 3,
            [Description("pinkyFinger")]
            PINKY = 4
        }
    }
}
