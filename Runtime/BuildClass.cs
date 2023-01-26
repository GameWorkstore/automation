using UnityEditor;
using UnityEngine;

namespace GameWorkstore.Automation
{
    public static class BuildClass
    {
        public static void BuildAndroid()
        {
            var buildScript = GetBuildScript();
            buildScript.TryBuild<AndroidBuildPlatform>();
        }

        public static void BuildIOS()
        {
            var buildScript = GetBuildScript();
            buildScript.TryBuild<IOSBuildPlatform>();
        }

        public static void BuildWindows()
        {
            var buildScript = GetBuildScript();
            buildScript.TryBuild<WindowsBuildPlatform>();
        }

        public static void BuildMacOS()
        {
            var buildScript = GetBuildScript();
            buildScript.TryBuild<MacOSBuildPlatform>();
        }

        public static void BuildLinux()
        {
            var buildScript = GetBuildScript();
            buildScript.TryBuild<LinuxBuildPlatform>();
        }

        public static void BuildServerWindows()
        {
            var buildScript = GetBuildScript();
            buildScript.TryBuild<ServerWindowsBuildPlatform>();
        }
        
        public static void BuildServerLinux()
        {
            var buildScript = GetBuildScript();
            buildScript.TryBuild<ServerLinuxBuildPlatform>();
        }

        public static void BuildServerMacOS()
        {
            var buildScript = GetBuildScript();
            buildScript.TryBuild<ServerMacOSBuildPlatform>();
        }

        public static void BuildUWP()
        {
            var buildScript = GetBuildScript();
            buildScript.TryBuild<UWPBuildPlatform>();
        }

        public static void BuildWebGL()
        {
            var buildScript = GetBuildScript();
            buildScript.TryBuild<WebGLBuildPlatform>();
        }

        public static string Arg(string argument)
        {
            string[] args = System.Environment.GetCommandLineArgs();
            for (int i = 0; i < args.Length; i++)
            {
                if (!args[i].Equals(argument)) continue;
                if (i + 1 >= args.Length) return null;
                return args[i + 1];

            }
            return null;
        }

        public static BuildScript GetBuildScript(bool logPath = false)
        {
            var buildScript = Arg("-buildscript");
            if (string.IsNullOrEmpty(buildScript))
            {
                foreach (var guid in AssetDatabase.FindAssets("t:BuildScript"))
                {
                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    if (logPath) Debug.Log("[GameWorkstore.Automation] BuildScript at path:" + path);
                    return AssetDatabase.LoadAssetAtPath<BuildScript>(path);
                }
            }
            else
            {
                foreach (var guid in AssetDatabase.FindAssets("t:BuildScript"))
                {
                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    //if (logPath)
                    Debug.Log("[GameWorkstore.Automation] BuildScript at path:" + path);
                    if (!path.EndsWith(buildScript)) continue;
                    return AssetDatabase.LoadAssetAtPath<BuildScript>(path);
                }
            }
            return null;
        }
    }
}