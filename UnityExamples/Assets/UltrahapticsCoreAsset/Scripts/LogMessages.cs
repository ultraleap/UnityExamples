
namespace UltrahapticsCoreAsset
{
    internal static class LogMessages
    {
        internal static class SensationCore
        {
            internal static string InvalidMethodOnSensationCoreInitialize(string methodName)
            {
                return "Can not invoke `" + methodName + "` " +
                "OnSensationCoreInitialize because the expected method signature does not match." + "\n" +
                "To use the OnSensationCoreInitialize method attribute, the method must be public, static, return void, and have no input arguments" + "\n" +
                "Example Usage : " + "\n" + "\n" +
                "[OnSensationCoreInitialize] " + "\n" +
                "public static void SomeFunction()" + "\n" +
                "{" + "\n" +
                "    Debug.Log(\"Hello World\"); " + "\n" +
                "}" + "\n";
            }

            internal static readonly string InvalidAddSearchPathsUsage = "Detected adding search path during SensationCore's lifetime - this action is unsupported. " +
                                                                         "Please use the OnSensationCoreInitialize Attribute to set the custom Python search paths for SensationCore";
        }
    }
}
