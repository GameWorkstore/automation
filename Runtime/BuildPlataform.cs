using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using GameWorkstore.Patterns;
using UnityEditor.Build.Reporting;
using System.IO;

namespace GameWorkstore.Automation
{
    public abstract class BuildPlataform : ScriptableObject
    {
        public BuildScript buildScript;
        [Header("Generic options")]
        public bool UseCustomScenes = false;
        public bool UseCustomScriptDefinitions = false;
        public bool Development = false;
        [ConditionalField("UseCustomScenes")] public CustomScenes Scenes = new CustomScenes();
        [ConditionalField("UseCustomScriptDefinitions")] public ScriptDefinitions ScriptDefinitions = new ScriptDefinitions();

        public string[] GetScenes()
        {
            return UseCustomScenes ? Scenes.List.Select(GetSceneName).ToArray() : EditorBuildSettingsScene.GetActiveSceneList(EditorBuildSettings.scenes);
        }

        private static string GetSceneName(SceneAssetPath sceneAssetPath)
        {
            return sceneAssetPath.Scene;
        }

        public static void ProcessReport(BuildReport buildReport)
        {
            if (UnityEditorInternal.InternalEditorUtility.isHumanControllingUs) return;
            if (ProcessReportIsSuccess(buildReport)) return;
            EditorApplication.Exit(1);
        }

        public static bool ProcessReportIsSuccess(BuildReport buildReport)
        {
            return buildReport.summary.result == BuildResult.Succeeded;
        }

        protected BuildOptions GetOptions(BuildOptions baseOptions = BuildOptions.None)
        {
            return (Development ? BuildOptions.Development : BuildOptions.None) | baseOptions;
        }

        public static string GetFolder(BuildPlayerOptions buildOptions)
        {
            var path = Path.Combine(GetProjectPath(), buildOptions.locationPathName);
            switch (buildOptions.target)
            {
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                case BuildTarget.StandaloneOSX:
                case BuildTarget.StandaloneLinux64:
                    return Directory.GetParent(path).FullName;
                case BuildTarget.iOS:
                    return path;
                default:
                    return string.Empty;
            }
        }

        public static string GetProjectPath()
        {
            return Application.dataPath.Substring(0, Application.dataPath.Length - 6);
        }

        private static void CopyFilesRecursively(string sourcePath, string targetPath)
        {
            Directory.CreateDirectory(targetPath);
            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
            }

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
            {
                File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
            }
        }

        protected static void CopyAdditionalFolders(BuildPlayerOptions buildOptions, string[] additionalFolders)
        {
            string targetRoot = GetFolder(buildOptions);
            if (string.IsNullOrEmpty(targetRoot)) return;

            foreach(var additionalFolder in additionalFolders)
            {
                var source = Path.Combine(GetProjectPath(), additionalFolder);
                var target = Path.Combine(targetRoot, additionalFolder);
                if (Directory.Exists(target))
                {
                    Directory.Delete(target, true);
                }
                CopyFilesRecursively(source, target);
            }
        }

        [ButtonMethod]
        public abstract void Build();

        public static bool Validate(BuildScript buildScript)
        {
            if (buildScript == null)
            {
                DebugMessege.Log("Cannot build without a BuildScript.", DebugLevel.ERROR);
                return false;
            }
            if (string.IsNullOrEmpty(buildScript.GameName))
            {
                DebugMessege.Log("GameName cannot be null or empty.", DebugLevel.ERROR);
                return false;
            }
            if(buildScript.GameVersionWriterConfig.Enabled && string.IsNullOrEmpty(buildScript.GameVersionWriterConfig.Namespace))
            {
                DebugMessege.Log("GameVersionWriter namespace cannot be null or empty.", DebugLevel.ERROR);
                return false;
            }
            return true;
        }

        public static void Version(out string gameVersion, out int bundleVersion)
        {
            var g = BuildClass.Arg("-gameversion");
            if (string.IsNullOrEmpty(g))
			{
                g = BuildClass.Arg("-buildVersion");
			}
            gameVersion = string.IsNullOrEmpty(g)? PlayerSettings.bundleVersion : g;

            var bv = BuildClass.Arg("-gamebundleversion");
			if (string.IsNullOrEmpty(bv))
			{
                bv = BuildClass.Arg("-androidVersionCode");
                if (string.IsNullOrEmpty(bv))
                {
                    Debug.LogWarning("-bundleversion is legacy and conflicts with Unity iOS bundleversion arg. Use -gamebundleversion instead.");
                    bv = BuildClass.Arg("-bundleversion");
                }
            }
            bundleVersion = int.TryParse(bv, out bundleVersion)? bundleVersion : 1;

            Debug.Log("GameVersion:[" + gameVersion + "][" + bundleVersion + "]");
        }
    }
}
