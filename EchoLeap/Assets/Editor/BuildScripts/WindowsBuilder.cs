using UnityEditor;
using UnityEngine;
using System.Linq;

public class WindowsBuilder
{
    public static string[] scenesList;
    public static string buildName = "EchoLeap";

    [MenuItem("Builder/Windows/Development build")]
    public static void BuildDevelopmentForWindows()
    {
        if (!ValidateBuildSettings()) return;

        BuildPlayerOptions buildConfig = CreateBuildPlayerOptions();
        buildConfig.options = BuildOptions.Development | BuildOptions.AllowDebugging | BuildOptions.ConnectWithProfiler | BuildOptions.ShowBuiltPlayer;

        BuildPipeline.BuildPlayer(buildConfig);
        Debug.Log($"Development build created at: {buildConfig.locationPathName}");
    }

    [MenuItem("Builder/Windows/Build scripts only")]
    public static void BuildScriptsOnlyForWindows()
    {
        if (!ValidateBuildSettings()) return;

        BuildPlayerOptions buildConfig = CreateBuildPlayerOptions();
        buildConfig.options = BuildOptions.BuildScriptsOnly | BuildOptions.ShowBuiltPlayer;

        BuildPipeline.BuildPlayer(buildConfig);
        Debug.Log($"Scripts-only build created at: {buildConfig.locationPathName}");
    }

    private static BuildPlayerOptions CreateBuildPlayerOptions()
    {
        BuildPlayerOptions buildConfig = new BuildPlayerOptions();

        buildConfig.scenes = scenesList ?? GetDefaultScenes();
        buildConfig.locationPathName = $"Builds/Windows/{buildName}.exe";
        buildConfig.target = BuildTarget.StandaloneWindows;

        return buildConfig;
    }

    private static bool ValidateBuildSettings()
    {
        if (string.IsNullOrEmpty(buildName))
        {
            Debug.LogError("Build name is not set.");
            return false;
        }

        if (scenesList == null || scenesList.Length == 0)
        {
            Debug.LogWarning("No scenes provided for the build. Using default scenes from Build Settings.");
        }

        return true;
    }

    private static string[] GetDefaultScenes()
    {
        return EditorBuildSettings.scenes
            .Where(scene => scene.enabled)
            .Select(scene => scene.path)
            .ToArray();
    }
}
