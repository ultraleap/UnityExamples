
namespace UltrahapticsCoreAsset.Editor
{
    namespace ReflectionProperties
    {
        // TODO : C#6 upgrade - nameof(SensationEmitter.AllowMockEmitter) to replace "AllowMockEmitter"

        internal static class SensationEmitter
        {
            internal const string AllowMockEmitter = "AllowMockEmitter";
            internal const string LogEmissionToFile = "LogEmissionToFile";
            internal const string ArrayTransform = "ArrayTransform";
            internal const string SphereRadius = "SphereRadius";
            internal const string SphereColor = "SphereColor";
            internal const string HistorySize = "HistorySize";
            internal const string CurrentSensation = "CurrentSensation";

            internal const string registeredSources_ = "registeredSources_";
        }

        internal static class SensationSource
        {
            internal const string sensationBlock_ = "sensationBlock_";
            internal const string running_ = "running_";
            internal const string priority_ = "priority_";
            internal const string Inputs = "Inputs";

        }

        internal static class SensationBlockInputs
        {
            internal const string supportsTransform_ = "supportsTransform_";
            internal const string TrackingObject = "TrackingObject";
            internal const string inputs_ = "inputs_";
        }
    }
}
