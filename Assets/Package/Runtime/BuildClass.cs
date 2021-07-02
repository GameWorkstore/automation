using GameWorkstore.Patterns;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace GameWorkstore.Automation
{
    public static class BuildClass
    {
        public static void BuildAndroid()
        {
            var buildScript = GetBuildScript();
            if (!Validate(buildScript)) return;

            //Version
            if (!UnityEditorInternal.InternalEditorUtility.isHumanControllingUs)
            {
                Version(out var gameVersion, out var bundleVersion);
                PlayerSettings.bundleVersion = gameVersion;
                PlayerSettings.Android.bundleVersionCode = bundleVersion;
            }

            //Backend
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, buildScript.BuildAndroid.ScriptingBackend);

            //Keystore Config
            SetAndroidSignCredentials(buildScript);

            if (buildScript.BuildAndroid.BuildAAB)
            {
                //Make
                const string subPathAab = "Build/Android/AAB/";
                MakeDirectory(subPathAab);

                var buildOptions = new BuildPlayerOptions
                {
                    scenes = buildScript.BuildAndroid.GetScenes(),
                    locationPathName = subPathAab + buildScript.GameName + ".aab",
                    target = BuildTarget.Android,
                    options = GetOptions(buildScript.BuildAndroid)
                };

                EditorUserBuildSettings.buildAppBundle = true;
                ProcessReport(BuildPipeline.BuildPlayer(buildOptions));
            }

            if (buildScript.BuildAndroid.BuildAPK)
            {
                //Make
                const string subPathAPK = "Build/Android/APK/";
                MakeDirectory(subPathAPK);

                var buildOptions = new BuildPlayerOptions
                {
                    scenes = buildScript.BuildAndroid.GetScenes(),
                    locationPathName = subPathAPK + buildScript.GameName + ".apk",
                    target = BuildTarget.Android,
                    options = GetOptions(buildScript.BuildAndroid)
                };

                EditorUserBuildSettings.buildAppBundle = false;
                ProcessReport(BuildPipeline.BuildPlayer(buildOptions));
            }
        }

        public static void BuildIOS()
        {
            var buildScript = GetBuildScript();
            if (!Validate(buildScript)) return;

            //Version
            if (!UnityEditorInternal.InternalEditorUtility.isHumanControllingUs)
            {
                Version(out var gameVersion, out var bundleVersion);
                PlayerSettings.bundleVersion = gameVersion;
                PlayerSettings.iOS.buildNumber = bundleVersion.ToString();
            }

            //Options
            var buildOptions = new BuildPlayerOptions
            {
                scenes = buildScript.BuildIOS.GetScenes(),
                locationPathName = "Build/iOS/" + buildScript.GameName,
                target = BuildTarget.iOS,
                options = GetOptions(buildScript.BuildIOS)
            };

            var buildReport = BuildPipeline.BuildPlayer(buildOptions);
            if (ProcessReportIsSuccess(buildReport))
            {
                CopyAdditionalFolders(buildOptions, buildScript.BuildIOS.AdditionalFolders);
            }
            ProcessReport(buildReport);
        }

        public static void BuildWindows()
        {
            var buildScript = GetBuildScript();
            if (!Validate(buildScript)) return;

            //Version
            if (!UnityEditorInternal.InternalEditorUtility.isHumanControllingUs)
            {
                Version(out var version, out _);
                PlayerSettings.bundleVersion = version;
            }

            //Backend
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Standalone, buildScript.BuildWindows.ScriptingBackend);

            //Options
            var buildOptions = new BuildPlayerOptions
            {
                scenes = buildScript.BuildWindows.GetScenes(),
                locationPathName = "Build/Windows/" + buildScript.GameName + ".exe",
                target = BuildTarget.StandaloneWindows64,
                options = GetOptions(buildScript.BuildWindows)
            };

            var buildReport = BuildPipeline.BuildPlayer(buildOptions);
            if (ProcessReportIsSuccess(buildReport))
            {
                CopyAdditionalFolders(buildOptions, buildScript.BuildWindows.AdditionalFolders);
            }
            ProcessReport(buildReport);
        }

        public static void BuildMacOS()
        {
            var buildScript = GetBuildScript();
            if (!Validate(buildScript)) return;

            //Version
            if (!UnityEditorInternal.InternalEditorUtility.isHumanControllingUs)
            {
                Version(out var version, out _);
                PlayerSettings.bundleVersion = version;
            }

            //Backend
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Standalone, buildScript.BuildMacOS.ScriptingBackend);

            //Options
            var buildOptions = new BuildPlayerOptions
            {
                scenes = buildScript.BuildMacOS.GetScenes(),
                locationPathName = "Build/MacOS/" + buildScript.GameName + ".app",
                target = BuildTarget.StandaloneOSX,
                options = GetOptions(buildScript.BuildMacOS)
            };

            var buildReport = BuildPipeline.BuildPlayer(buildOptions);
            if (ProcessReportIsSuccess(buildReport))
            {
                CopyAdditionalFolders(buildOptions, buildScript.BuildMacOS.AdditionalFolders);
            }
            ProcessReport(buildReport);
        }

        public static void BuildLinux()
        {
            var buildScript = GetBuildScript();
            if (!Validate(buildScript)) return;

            //Version
            if (!UnityEditorInternal.InternalEditorUtility.isHumanControllingUs)
            {
                Version(out var version, out _);
                PlayerSettings.bundleVersion = version;
            }

            //Backend
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Standalone, buildScript.BuildLinux.ScriptingBackend);

            //Options
            var buildOptions = new BuildPlayerOptions
            {
                scenes = buildScript.BuildLinux.GetScenes(),
                locationPathName = "Build/Linux/" + buildScript.GameName,
                target = BuildTarget.StandaloneLinux64,
                options = GetOptions(buildScript.BuildLinux)
            };

            var buildReport = BuildPipeline.BuildPlayer(buildOptions);
            if (ProcessReportIsSuccess(buildReport))
            {
                CopyAdditionalFolders(buildOptions, buildScript.BuildLinux.AdditionalFolders);
            }
            ProcessReport(buildReport);
        }

        public static void BuildGameServerWindows()
        {
            var buildScript = GetBuildScript();
            if (!Validate(buildScript)) return;

            //Version
            if (!UnityEditorInternal.InternalEditorUtility.isHumanControllingUs)
            {
                Version(out var version, out _);
                PlayerSettings.bundleVersion = version;
            }

            //Backend
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Standalone, buildScript.BuildGameServerWindows.ScriptingBackend);

            //Options
            var buildOptions = new BuildPlayerOptions
            {
                scenes = buildScript.BuildGameServerWindows.GetScenes(),
                locationPathName = "Build/GameServerWindows/" + buildScript.GameName + ".exe",
                target = BuildTarget.StandaloneWindows64,
                options = GetOptions(buildScript.BuildGameServerWindows,BuildOptions.EnableHeadlessMode)
            };

            var buildReport = BuildPipeline.BuildPlayer(buildOptions);
            if (ProcessReportIsSuccess(buildReport))
            {
                CopyAdditionalFolders(buildOptions, buildScript.BuildGameServerWindows.AdditionalFolders);
            }
            ProcessReport(buildReport);
        }

        public static void BuildGameServerLinux()
        {
            var buildScript = GetBuildScript();
            if (!Validate(buildScript)) return;

            //Version
            if (!UnityEditorInternal.InternalEditorUtility.isHumanControllingUs)
            {
                Version(out var version, out _);
                PlayerSettings.bundleVersion = version;
            }

            //Backend
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Standalone, buildScript.BuildGameServerLinux.ScriptingBackend);

            //Options
            var buildOptions = new BuildPlayerOptions
            {
                scenes = buildScript.BuildGameServerLinux.GetScenes(),
                locationPathName = "Build/GameServerLinux/" + buildScript.GameName,
                target = BuildTarget.StandaloneLinux64,
                options = GetOptions(buildScript.BuildGameServerLinux,BuildOptions.EnableHeadlessMode)
            };

            var buildReport = BuildPipeline.BuildPlayer(buildOptions);
            if (ProcessReportIsSuccess(buildReport))
            {
                CopyAdditionalFolders(buildOptions, buildScript.BuildGameServerLinux.AdditionalFolders);
            }
            ProcessReport(buildReport);
        }

        public static void BuildGameServerMacOS()
        {
            var buildScript = GetBuildScript();
            if (!Validate(buildScript)) return;

            //Version
            if (!UnityEditorInternal.InternalEditorUtility.isHumanControllingUs)
            {
                Version(out var version, out _);
                PlayerSettings.bundleVersion = version;
            }

            var buildConfig = buildScript.BuildGameServerMacOS;
            //Backend
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Standalone, buildConfig.ScriptingBackend);

            //Options
            var buildOptions = new BuildPlayerOptions
            {
                scenes = buildConfig.GetScenes(),
                locationPathName = "Build/GameServerMacOS/" + buildScript.GameName + ".app",
                target = BuildTarget.StandaloneOSX,
                options = GetOptions(buildConfig,BuildOptions.EnableHeadlessMode)
            };

            var buildReport = BuildPipeline.BuildPlayer(buildOptions);
            if (ProcessReportIsSuccess(buildReport))
            {
                CopyAdditionalFolders(buildOptions, buildScript.BuildGameServerMacOS.AdditionalFolders);
            }
            ProcessReport(buildReport);
        }

        public static void BuildUWP()
        {
            var buildScript = GetBuildScript();
            if (!Validate(buildScript)) return;

            //Version
            if (!UnityEditorInternal.InternalEditorUtility.isHumanControllingUs)
            {
                Version(out var version, out _);
                PlayerSettings.bundleVersion = version;
                if (System.Version.TryParse(version, out var iVersion))
                {
                    PlayerSettings.WSA.packageVersion = iVersion;
                }
                else
                {
                    DebugMessege.Log("Failed to parse game version.", DebugLevel.ERROR);
                }
            }
            

            //Backend
            //Always IL2CPP
            //PlayerSettings.SetScriptingBackend(BuildTargetGroup.WSA, buildScript.BuildUWP.ScriptingBackend);

            //Options
            var buildOptions = new BuildPlayerOptions
            {
                scenes = buildScript.BuildUWP.GetScenes(),
                locationPathName = "Build/UWP/",
                target = BuildTarget.WSAPlayer,
                options = GetOptions(buildScript.BuildUWP)
            };

            ProcessReport(BuildPipeline.BuildPlayer(buildOptions));
        }

        public static void BuildWebGL()
        {
            var buildScript = GetBuildScript();
            if (!Validate(buildScript)) return;

            //Version
            if (!UnityEditorInternal.InternalEditorUtility.isHumanControllingUs)
            {
                Version(out var version, out _);
                PlayerSettings.bundleVersion = version;
            }

            //Options
            var buildOptions = new BuildPlayerOptions
            {
                scenes = buildScript.BuildWebGL.GetScenes(),
                locationPathName = "Build/WebGL/",
                target = BuildTarget.WebGL,
                options = GetOptions(buildScript.BuildWebGL)
            };

            ProcessReport(BuildPipeline.BuildPlayer(buildOptions));
        }

        public static void Version(out string gameVersion, out int bundleVersion)
        {
            var g = Arg("-gameversion");
            if (string.IsNullOrEmpty(g))
			{
                g = Arg("-buildVersion");
			}
            gameVersion = string.IsNullOrEmpty(g)? PlayerSettings.bundleVersion : g;

            var bv = Arg("-bundleversion");
			if (string.IsNullOrEmpty(bv))
			{
                bv = Arg("-androidVersionCode");
            }
            bundleVersion = int.TryParse(bv, out bundleVersion)? bundleVersion : 1;

            Debug.Log("GameVersion:[" + gameVersion + "][" + bundleVersion + "]");
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

        public static void MakeDirectory(string relativePath)
        {
            var path = GetProjectPath() + relativePath;
            if (Directory.Exists(path)) return;
            Directory.CreateDirectory(path);
        }

        public static string GetProjectPath()
        {
            return Application.dataPath.Substring(0, Application.dataPath.Length - 6);
        }

        public static BuildScript GetBuildScript(bool logPath = false)
        {
            foreach (var guid in AssetDatabase.FindAssets("t:BuildScript"))
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                if (logPath) Debug.Log("[GameWorkstore.Automation] BuildScript at path:" + path);
                return AssetDatabase.LoadAssetAtPath<BuildScript>(path);
            }
            return null;
        }

        private static BuildOptions GetOptions(BuildBase buildBase, BuildOptions baseOptions = BuildOptions.None)
        {
            return (buildBase.Development ? BuildOptions.Development : BuildOptions.None) | baseOptions;
        }

        public static bool ProcessReportIsSuccess(BuildReport buildReport)
        {
            return buildReport.summary.result == BuildResult.Succeeded;
        }

        public static void ProcessReport(BuildReport buildReport)
        {
            if (UnityEditorInternal.InternalEditorUtility.isHumanControllingUs) return;
            if (ProcessReportIsSuccess(buildReport)) return;
            EditorApplication.Exit(1);
        }

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

        [MenuItem("Help/Automation/SetAndroidCredentials")]
        public static void SetAndroidCredentials()
        {
            var buildScript = BuildClass.GetBuildScript();
            BuildClass.SetAndroidSignCredentials(buildScript);
        }

        public static void SetAndroidSignCredentials(BuildScript buildScript)
        {
            if(buildScript == null) return;
            if (!buildScript.BuildAndroid.UseKeystore) return;
            PlayerSettings.Android.useCustomKeystore = true;
            PlayerSettings.Android.keystoreName = GetProjectPath() + buildScript.BuildAndroid.KeyStoreSettings.KeystorePath;
            PlayerSettings.Android.keystorePass = buildScript.BuildAndroid.KeyStoreSettings.KeystorePassword;
            PlayerSettings.Android.keyaliasName = buildScript.BuildAndroid.KeyStoreSettings.AliasName;
            PlayerSettings.Android.keyaliasPass = buildScript.BuildAndroid.KeyStoreSettings.AliasPassword;
        }

        private static void CopyAdditionalFolders(BuildPlayerOptions buildOptions, string[] additionalFolders)
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
    }
}