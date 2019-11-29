using UnityEditor;
using UnityEngine;

namespace UltrahapticsCoreAsset.Editor
{
    public class HelpMenuItems
    {
        private const string ultrahapticsDevSiteURL = "http://developer.ultrahaptics.com";
        private const string leapMotionUnityAsssetsURL = "https://developer.leapmotion.com/unity";
        private const string contactEmailAddress = "eap@ultrahaptics.com";

        // Developer Resource URLs
        [MenuItem("Ultrahaptics/Ultrahaptics Developer Site...")]
        private static void OpenUltrahapticsDevSiteURL()
        {
            Application.OpenURL(ultrahapticsDevSiteURL);
        }

        [MenuItem("Ultrahaptics/Leap Motion Unity Assets...")]
        private static void OpenLeapMotionUnityURL()
        {
            Application.OpenURL(leapMotionUnityAsssetsURL);
        }

        [MenuItem("Ultrahaptics/Report an Issue...")]
        private static void OpenContactEmailURL()
        {
            Debug.Log("Attempting to open email client. If this fails, please contact: " + contactEmailAddress);
            Application.OpenURL("mailto:" + contactEmailAddress);
        }
    }
}
