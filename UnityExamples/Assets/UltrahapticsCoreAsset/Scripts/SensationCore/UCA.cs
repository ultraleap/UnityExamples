
namespace UltrahapticsCoreAsset
{
    public static class UCA
    {
        // TODO C# 6 : static ILogger Logger { get; set; } = new UnityLogger();
        private static ILogger logger_ = new UnityLogger();
        private static string mockEmitterLogFile_ = "logFile.txt";
        private static string mockEmitterModel_ = "USX";
        private static bool mockEmitterLoggingEnabled_ = false;

        public static ILogger Logger { get{ return logger_; } set{ logger_ = value; } }
        public static string MockEmitterModel { get{ return  mockEmitterModel_; } set{ mockEmitterModel_ = value; } }
        public static string MockEmitterLogFile { get{ return  mockEmitterLogFile_; } set{ mockEmitterLogFile_ = value; } }
        public static bool MockEmitterLoggingEnabled { get{ return mockEmitterLoggingEnabled_; } set{ mockEmitterLoggingEnabled_ = value; } }

    }
}
