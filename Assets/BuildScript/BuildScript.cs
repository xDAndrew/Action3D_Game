using UnityEditor;

namespace BuildScript
{
    public class BuildScript
    {
        public static void BuildDebug()
        {
            var buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = new[] { "Assets/Scenes/Main.unity" },
                locationPathName = "D:/Builds/Debug/Game.exe",
                target = BuildTarget.StandaloneWindows64
            };

            BuildPipeline.BuildPlayer(buildPlayerOptions);
        }
    }
}