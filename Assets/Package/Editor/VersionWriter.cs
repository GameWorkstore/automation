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

            var content =
                "namespace " + script.GameVersionWriterConfig.Namespace + "\r\n" +
                "{\r\n"+
                    "\tpublic static class GameVersion\r\n" +
                    "\t{\r\n" +
                        "\t"+ FormatVar("IosBundleVersion", PlayerSettings.iOS.buildNumber) + "\r\n" +
                        "\t" + FormatVar("AndroidBundleVersion", PlayerSettings.Android.bundleVersionCode.ToString()) + "\r\n" +
                    "\t}\r\n" +
                "}";

            var directoryPath = GetDirectory(script);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var filePath = GetAbsoluteFilePath(script);

            var currentText = string.Empty;
            if (File.Exists(filePath))
            {
                currentText = File.ReadAllText(filePath);
            }

            if (currentText == content) return;
            Debug.Log("Updated GameVersion.cs");

            File.WriteAllText(filePath, content);
            AssetDatabase.Refresh();
            AssetDatabase.ImportAsset(GetLocalFilePath(script));
        }

        private static string FormatVar(string varName, string varValue)
        {
            return "\tpublic const string " + varName + " = \"" + varValue + "\";";
        }

        public static string GetAbsoluteFilePath(BuildScript script)
        {
            return Path.Combine(GetDirectory(script), FileName);
        }

        public static string GetLocalFilePath(BuildScript script)
        {
            return Path.Combine("Assets",GetLocalDirectory(script), FileName);
        }

        public static string GetDirectory(BuildScript script)
        {
            return Path.Combine(Application.dataPath, GetLocalDirectory(script));
        }

        public static string GetLocalDirectory(BuildScript script)
        {
            if (script.GameVersionWriterConfig.Path.StartsWith("/"))
            {
                return script.GameVersionWriterConfig.Path.Substring(1);
            }
            return script.GameVersionWriterConfig.Path;
        }
    }
}
#endif