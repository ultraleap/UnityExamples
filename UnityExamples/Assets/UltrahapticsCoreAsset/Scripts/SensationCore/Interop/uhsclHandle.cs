using System;

namespace UltrahapticsCoreAsset
{

    public struct uhsclHandle : IEquatable<uhsclHandle>
    {
        public readonly Int32 Value;

        public uhsclHandle(Int32 val) { Value = val; }

        public static bool operator ==(uhsclHandle lhs, uhsclHandle rhs)
        {
            return lhs.Value == rhs.Value;
        }

        public static bool operator !=(uhsclHandle lhs, uhsclHandle rhs)
        {
            return !(lhs == rhs);
        }

        public override bool Equals(object other)
        {
            if (!(other is uhsclHandle)) return false;
            return (uhsclHandle)other == this;
        }

        bool IEquatable<uhsclHandle>.Equals(uhsclHandle other)
        {
            return this == other;
        }

        public override Int32 GetHashCode() { return Value.GetHashCode(); }

        public override string ToString() { return Value.ToString(); }

        public static uhsclHandle INVALID_HANDLE { get { return default(uhsclHandle); } }
    }

}
