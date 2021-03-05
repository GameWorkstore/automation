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
            var buildScript = BuildClass.GetBuildScript();
            WriteVersion(buildScript);
        }

        [DidReloadScripts]
        public static void WriteVersion()
        {
            var buildScript = BuildClass.GetBuildScript();
            WriteVersion(buildScript);
        }

        public static void WriteVersion(BuildScript script)
        {
            if (script == null) return;
            if (!script.GameVersionWriterConfig.Enabled) return;

            var newText =
                "namespace " + script.GameVersionWriterConfig.Namespace + "\r\n" +
                "{"+
                    "\tpublic static class GameVersion\r\n" +
                    "\t{\r\n" +
                        "\t\t"+ FormatVar("IosBundleVersion", PlayerSettings.iOS.buildNumber) + "\r\n" +
                        "\t\t" + FormatVar("AndroidBundleVersion", PlayerSettings.iOS.buildNumber) + "\r\n" +
                    "\t}\r\n" +
                "}";

            var directoryPath = GetDirectory(script);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var filePath = GetFilePath(script);

            var currentText = string.Empty;
            if (File.Exists(filePath))
            {
                currentText = File.ReadAllText(filePath);
            }

            if (currentText == newText) return;
            Debug.Log("Updated GameVersion.cs");

            File.WriteAllText(filePath, newText);
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
            if (script.GameVersionWriterConfig.Path.StartsWith("/"))
            {
                return Path.Combine(Application.dataPath, script.GameVersionWriterConfig.Path.Substring(1));
            }
            return Path.Combine(Application.dataPath, script.GameVersionWriterConfig.Path);
        }
    }
}
#endif