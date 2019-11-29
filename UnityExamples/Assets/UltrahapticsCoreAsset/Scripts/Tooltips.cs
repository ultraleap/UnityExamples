
namespace UltrahapticsCoreAsset.Editor
{
    internal static class Tooltips
    {
        internal static class SensationEmitter
        {
            internal static string AllowMockEmitter = "Enabling 'Allow Mock Emitter' lets you visualize and test Sensations without the need for a connection to a physical Ultrahaptics device. If disabled and no physical Ultrahaptics device is available, the Sensation Emitter will not evaluate or visualise any Sensation Sources.";
            internal static string LogEmissionToFile = "Enabling 'Log Emission to File' will create a log file representing array output in the format provided by the Ultrahaptics SDK. This can be used to debug sensation output. Note that this will only work when using the \"Allow Mock Emitter\" setting.";
            internal static string ArrayTransform = "The transform origin for control point visualization (only required if using an Emitter Data Source)";
            internal static string SphereRadius = "The size of the visualized control points";
            internal static string SphereColor = "The color of the visualized control points";
            internal static string HistorySize = "The number of visualized control points";
            internal static string CurrentSensation = "The Sensation Source that is currently controlling emitter output";
        }

        internal static class SensationSource
        {
            internal static string SensationBlock = "The name of the Sensation Block used by this Sensation Source.\n\n" +
                                                    "Sensation Blocks define the inputs and behaviour of mid-air haptic effects ('Sensations').\n\n" +
                                                    "Sensation Blocks included with SensationCore library are described in BlockManifest.\n\n" +
                                                    "New Sensations can be defined via the SensationCore Block Scripting API.";
            internal static string Running = "If enabled, the Sensation Source's Block Graph will be evaluated by SensationCore's playback engine.\n\n" +
                                             "If disabled, the Block Graph will not be evaluated and no haptics will be experienced.\n\n" +
                                             "Note: if multiple Sensations are 'Running' simultaneously, only the Sensation most recently set to be 'Running' will be evaluated.";
            internal static string Priority = "The Priority level of a Sensation Source determines if it should be played over other actively running Sensations or not.\n\n" +
                                              "Larger priority values mean that the Sensation Source is more important and will be chosen over those with lower priority values.";
            internal static string HiddenInputs = "Some Sensation Blocks contain hidden inputs, to view these inputs click the dropdown arrow to expand.\n\n" +
                                                  "Note: most hidden inputs are provided by a DataSource (registered to the AutoMapper) and any edits made to input values may get overridden during scene playback.";
        }

        internal static class UltrahapticsKitLayout
        {
            internal const string UltrahapticsKitModel = "This dropdown sets the default Tracking Origin and Array Model layout for standard Ultrahaptics Kits." ;
            internal const string UseDefaultLayout = "If checked, changing the Ultrahaptics Kit Model will automatically position the " +
                "Tracking Origin using the default layout for the selected kit.";
            internal const string ShowKitModels = "If enabled, a virtual model of the Ultrahaptics Kit will be displayed.";
            internal const string ArrayModel = "The Transform of the ArrayModel GameObject (representative of the Ultrahaptics array).";
            internal const string TrackingOrigin = "The Transform of the Tracking Origin for this UltrahapticsKit.";
            internal const string ApplyDefaultLayoutAtRuntime = "If checked, the Tracking Origin of the UltrahapticsKit is set to the " +
                "default location for the detected Ultrahaptics kit at runtime. Uncheck this option if you are using a non-default " +
                "location for the tracking origin";
            internal const string ShowInteractionZone = "If enabled, a set of bounding boxes approximating the interaction zone for the array will be displayed. These estimate the quality of output you can expect in different regions of the interaction zone.";
            
        }
    }
}
