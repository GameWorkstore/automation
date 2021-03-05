#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Callbacks;
using UnityEngine;

namespace GameWorkstore.Automation
{
    public class VersionWriter : IPreprocessBuildWithReport
    {
        public const string FileName = "GameVersion.cs";

        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            WriteVersion();
        }

        [DidReloadScripts]
        public static void WriteVersion()
        {
            return;
            var buildScript = BuildClass.GetBuildScript();
            if (!BuildClass.Validate(buildScript)) return;

            var config = buildScript.GameVersionWriterConfig;
            var finalDir = Path.Combine(Application.dataPath, config.Path);
            var finalPath = Path.Combine(finalDir, FileName);

            var newText =
                "namespace " + config.Namespace + "\r\n" +
                "{"+
                    "\tpublic static class GameVersion\r\n" +
                    "\t{\r\n" +
                        "\t\t"+ FormatVar("IosBundleVersion", PlayerSettings.iOS.buildNumber) + "\r\n" +
                        "\t\t" + FormatVar("AndroidBundleVersion", PlayerSettings.iOS.buildNumber) + "\r\n" +
                    "\t}\r\n" +
                "}";
            if (!Directory.Exists(finalDir))
            {
                Directory.CreateDirectory(finalDir);
            }

            var currentText = string.Empty;
            if (File.Exists(finalPath))
            {
                currentText = File.ReadAllText(finalPath);
            }

            if (currentText == newText) return;
            Debug.Log("Updated GameVersion.cs");

            File.WriteAllText(finalPath, newText);
            AssetDatabase.Refresh();
        }

        private static string FormatVar(string varName, string varValue)
        {
            return "\tpublic const string " + varName + " = \"" + varValue + "\";";
        }

        public static string GetFilePath(BuildScript script)
        {
            return Path.Combine(GetDirectory(script), FileName);
        }

        public static string GetDirectory(BuildScript script)
        {
            return Path.Combine(Application.dataPath, script.GameVersionWriterConfig.Path);
        }
    }
}
#endif