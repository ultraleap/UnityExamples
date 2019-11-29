using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
using System;

namespace UltrahapticsCoreAsset.Editor
{
    public class PythonBundler
    {
        private static string PythonLibrariesLocation()
        {
            var guids = AssetDatabase.FindAssets("python36");
            var paths = Array.ConvertAll<string, string>(guids, AssetDatabase.GUIDToAssetPath);
            foreach (var path in paths)
            {
                if (Path.GetFileName(path) == "python36.zip")
                {
                    return path;
                }
            }
            return "";
        }

        // There is no idiomatic way in Unity to include plugin data dependencies, such as python36.zip,
        // in compiled apps (or at least, in the right location). We have to copy it manually
        [PostProcessBuild]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
        {
            if (target == BuildTarget.StandaloneWindows || target == BuildTarget.StandaloneWindows64)
            {
                var baseDir = Path.GetDirectoryName(pathToBuiltProject);
                var buildName = Path.GetFileNameWithoutExtension(pathToBuiltProject);
                var dataDir = Path.Combine(baseDir, buildName + "_Data"); // This is a special Unity folder! https://docs.unity3d.com/ScriptReference/Application-dataPath.html
                var pluginDir = Path.Combine(dataDir, "Plugins");
                var destName = Path.Combine(pluginDir, "python36.zip");
                File.Copy(PythonLibrariesLocation(), destName);
            }
        }
    }
}
