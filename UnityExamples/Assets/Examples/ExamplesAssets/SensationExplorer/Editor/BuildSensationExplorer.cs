using UnityEditor;
using UnityEngine;
using UnityEditor.Build.Reporting;
using System;

using System.Collections.Generic;
using System.IO;

// Output the build size or a failure depending on BuildPlayer.
public class BuildSensationExplorer : MonoBehaviour
{
	[MenuItem("UnityExamples/Sensation Explorer/Build for Mac")]
	public static void BuildMacOS()
	{
		BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
		buildPlayerOptions.scenes = new[] { "Assets/Examples/SensationExplorer.unity" };

		buildPlayerOptions.target = BuildTarget.StandaloneOSX;
		buildPlayerOptions.options = BuildOptions.None;

		var buildDir = "/tmp/";
		var productName = PlayerSettings.productName;
        var productVersion = PlayerSettings.bundleVersion;
		var appName = productName + productVersion + ".app";
		var buildPath = buildDir + appName;

        Debug.Log("Build App name is:" + buildPath);

        buildPlayerOptions.locationPathName = buildPath;

        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
		BuildSummary summary = report.summary;

		if (summary.result == BuildResult.Succeeded)
		{
			Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
		}

		if (summary.result == BuildResult.Failed)
		{
			Debug.Log("Build failed");
		}
	}

    [MenuItem("UnityExamples/Sensation Explorer/Build for Windows")]
    public static void BuildWindows()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[] { "Assets/Examples/SensationExplorer.unity" };

        buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
        buildPlayerOptions.options = BuildOptions.None;

        var productName = PlayerSettings.productName;
        var productVersion = PlayerSettings.bundleVersion;
        var appName = productName + productVersion + ".exe";
        var buildDir = "C:\\temp\\" + productName + productVersion + "\\";
        var buildPath = buildDir + appName;

        Debug.Log("Build App name is:" + buildPath);

        buildPlayerOptions.locationPathName = buildPath;

        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
        }

        if (summary.result == BuildResult.Failed)
        {
            Debug.Log("Build failed");
        }
    }
}