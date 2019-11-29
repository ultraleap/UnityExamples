namespace UltrahapticsCoreAsset
{
    // These types are defined in the SDK and are protected, we include these here
    // so that when we build SensationCore into an assembly we collide with Unity
    // types that we would likely have wanted to use

    // Forcing us to be explicit means that we don't leave ambigious types in the SCL
    // that would switch to resolving to SDK defined types when the package is imported

    internal partial class Vector3
    {}

    internal partial class Transform
    {}
}
