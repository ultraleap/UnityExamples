
namespace UltrahapticsCoreAsset
{
    public interface ILogger
    {
        void LogInfo(string message);
        void LogWarning(string message);
        void LogError(string message);

        bool Enabled { get; set; }
    }
}
