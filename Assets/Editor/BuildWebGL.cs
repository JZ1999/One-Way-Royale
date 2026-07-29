using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

// Invoked from command line: Unity.exe -batchmode -quit -executeMethod BuildWebGL.Build
// Builds using whatever scenes are already checked in File > Build Settings.
public static class BuildWebGL
{
    public static void Build()
    {
        string outputPath = "E:/Users/Alejandro/Opal/Builds/One Way";
        string[] scenes = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray();

        BuildReport report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = outputPath,
            target = BuildTarget.WebGL,
            options = BuildOptions.None
        });

        if (report.summary.result != BuildResult.Succeeded)
        {
            Debug.LogError($"WebGL build failed: {report.summary.result}, errors: {report.summary.totalErrors}");
            EditorApplication.Exit(1);
            return;
        }

        Debug.Log($"WebGL build succeeded: {report.summary.totalSize} bytes at {outputPath}");
        EditorApplication.Exit(0);
    }
}
