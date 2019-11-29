using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System;
using System.IO;
using System.Collections.Generic;

namespace UltrahapticsCoreAsset.Editor
{
    public class ExampleSceneLauncher
    {
        public static string FindScenePathByName(string sceneName)
        {
            var guids = AssetDatabase.FindAssets("t:Scene");
            var paths = Array.ConvertAll<string, string>(guids, AssetDatabase.GUIDToAssetPath);
            foreach (var path in paths)
            {
                if (Path.GetFileName(path) == sceneName)
                {
                    return path;
                }
            }
            Debug.LogWarning("Unable to locate Scene by name: " + sceneName);
            return "";
        }

        public static void LaunchSceneByName(string sceneName)
        {
            var scenePath = FindScenePathByName(sceneName);
            if (scenePath != "")
            {
                // Ask to save the currently open Scene first, to avoid lost work.
                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                EditorSceneManager.OpenScene(scenePath);
            }
            else
            {
                Debug.LogWarning("Unable to open Scene: " + sceneName);
            }
        }
    }

    public class ExampleSceneMenu
    {
        // Example Scenes Launcher Menu.

        [MenuItem("Ultrahaptics/Example Scenes/Forcefield")]
        private static void OpenForcefieldScene()
        {
            ExampleSceneLauncher.LaunchSceneByName("Forcefield.unity");
        }

        [MenuItem("Ultrahaptics/Example Scenes/Hand As Cursor")]
        private static void OpenHandAsCursorScene()
        {
            ExampleSceneLauncher.LaunchSceneByName("HandAsCursor.unity");
        }

        [MenuItem("Ultrahaptics/Example Scenes/Hand Triggered Sensation")]
        private static void OpenHandTriggeredSensationScene()
        {
            ExampleSceneLauncher.LaunchSceneByName("HandTriggeredSensation.unity");
        }

        [MenuItem("Ultrahaptics/Example Scenes/Polyline")]
        private static void OpenPolylineScene()
        {
            ExampleSceneLauncher.LaunchSceneByName("Polyline.unity");
        }

        [MenuItem("Ultrahaptics/Example Scenes/Priority")]
        private static void OpenPriorityScene()
        {
            ExampleSceneLauncher.LaunchSceneByName("Priority.unity");
        }

        [MenuItem("Ultrahaptics/Example Scenes/Sensation Animation")]
        private static void OpenSensationAnimationScene()
        {
            ExampleSceneLauncher.LaunchSceneByName("SensationAnimation.unity");
        }

        [MenuItem("Ultrahaptics/Example Scenes/Sensation Sequencing")]
        private static void OpenSensationSequencingScene()
        {
            ExampleSceneLauncher.LaunchSceneByName("SensationSequencing.unity");
        }

        [MenuItem("Ultrahaptics/Example Scenes/Sensation Source Playback")]
        private static void OpenSensationSourcePlaybackScene()
        {
            ExampleSceneLauncher.LaunchSceneByName("SensationSourcePlayback.unity");
        }
    }
}
