using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class BuildScript
    {
        public static void BuildDebug()
        {
            const string path = "D:/Builds/Game.exe";
            var options = new BuildPlayerOptions
            {
                scenes = new[] { "Assets/Scenes/Main.unity" },
                locationPathName = path,
                target = BuildTarget.StandaloneWindows64
            };

            var report = BuildPipeline.BuildPlayer(options);
            Debug.Log("Build result: " + report.summary.result);
        }
    }
}