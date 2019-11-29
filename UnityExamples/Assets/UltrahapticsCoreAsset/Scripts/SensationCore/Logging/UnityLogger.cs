using UnityEngine;

namespace UltrahapticsCoreAsset
{
    class UnityLogger : ILogger
    {

        // TODO C# 6: static bool EnableLogging { get; set; } = true;
        private bool enabled_ = true;
        public bool Enabled { get { return enabled_; } set{ enabled_ = value; } }

        public void LogInfo(string message)
        {
            if (Enabled)
            {
                Debug.Log(message);
            }
        }

        public void LogWarning(string message)
        {
            if (Enabled)
            {
                Debug.LogWarning(message);
            }
        }

        public void LogError(string message)
        {
            if (Enabled)
            {
                Debug.LogError(message);
            }
        }

    }
}
