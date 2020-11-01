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
            if (buildScript == null)
            {
                DebugMessege.Log("Cannot build without a BuildScript.", DebugLevel.ERROR);
                return;
            }

            //Version
            if (!UnityEditorInternal.InternalEditorUtility.isHumanControllingUs)
            {
                Version(out string gameversion, out int bundleversion);
                PlayerSettings.bundleVersion = gameversion;
                PlayerSettings.Android.bundleVersionCode = bundleversion;
            }

            //Backend
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, buildScript.BuildAndroid.ScriptingBackend);

            //Keystore Config
            if (buildScript.BuildAndroid.UseKeystore)
            {
                PlayerSettings.Android.useCustomKeystore = true;
                PlayerSettings.Android.keystoreName = GetProjectPath() + buildScript.BuildAndroid.KeyStoreSettings.KeystorePath;
                PlayerSettings.Android.keystorePass = buildScript.BuildAndroid.KeyStoreSettings.KeystorePassword;
                PlayerSettings.Android.keyaliasName = buildScript.BuildAndroid.KeyStoreSettings.AliasName;
                PlayerSettings.Android.keyaliasPass = buildScript.BuildAndroid.KeyStoreSettings.AliasPassword;
            }

            if (buildScript.BuildAndroid.BuildAPK)
            {
                //Make
                const string subpath_aab = "Build/Android/APK/";
                MakeDirectory(subpath_aab);

                BuildPlayerOptions buildOptions = new BuildPlayerOptions
                {
                    scenes = buildScript.BuildAndroid.GetScenes(),
                    locationPathName = subpath_aab + buildScript.GameName + ".aab",
                    target = BuildTarget.Android,
                    options = BuildOptions.None
                };

                EditorUserBuildSettings.buildAppBundle = true;
                ProcessReport(BuildPipeline.BuildPlayer(buildOptions));
            }

            if (buildScript.BuildAndroid.BuildAAB)
            {
                //Make
                const string subpath_apk = "Build/Android/APK/";
                MakeDirectory(subpath_apk);

                BuildPlayerOptions buildOptions = new BuildPlayerOptions
                {
                    scenes = buildScript.BuildAndroid.GetScenes(),
                    locationPathName = subpath_apk + buildScript.GameName + ".apk",
                    target = BuildTarget.Android,
                    options = BuildOptions.None
                };

                EditorUserBuildSettings.buildAppBundle = false;
                ProcessReport(BuildPipeline.BuildPlayer(buildOptions));
            }
        }

        public static void BuildIOS()
        {
            var buildScript = GetBuildScript();
            if (buildScript == null)
            {
                DebugMessege.Log("Cannot build without a BuildScript.", DebugLevel.ERROR);
                return;
            }

            //Version
            if (!UnityEditorInternal.InternalEditorUtility.isHumanControllingUs)
            {
                Version(out string gameversion, out int bundleversion);
                PlayerSettings.bundleVersion = gameversion;
                PlayerSettings.iOS.buildNumber = bundleversion.ToString();
            }

            //Options
            BuildPlayerOptions buildOptions = new BuildPlayerOptions
            {
                scenes = buildScript.BuildIOS.GetScenes(),
                locationPathName = "Build/iOS/" + buildScript.GameName,
                target = BuildTarget.iOS,
                options = BuildOptions.None
            };

            ProcessReport(BuildPipeline.BuildPlayer(buildOptions));
        }

        public static void BuildWindows()
        {
            var buildScript = GetBuildScript();
            if (buildScript == null)
            {
                DebugMessege.Log("Cannot build without a BuildScript.", DebugLevel.ERROR);
                return;
            }

            //Version
            if (!UnityEditorInternal.InternalEditorUtility.isHumanControllingUs)
            {
                Version(out string gameversion, out int _);
                PlayerSettings.bundleVersion = gameversion;
            }

            //Backend
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Standalone, buildScript.BuildWindows.ScriptingBackend);

            //Options
            BuildPlayerOptions buildOptions = new BuildPlayerOptions
            {
                scenes = buildScript.BuildWindows.GetScenes(),
                locationPathName = "Build/Windows/" + buildScript.GameName + ".exe",
                target = BuildTarget.StandaloneWindows64,
                options = BuildOptions.None
            };

            ProcessReport(BuildPipeline.BuildPlayer(buildOptions));
        }

        public static void BuildMacOS()
        {
            var buildScript = GetBuildScript();
            if (buildScript == null)
            {
                DebugMessege.Log("Cannot build without a BuildScript.", DebugLevel.ERROR);
                return;
            }

            //Version
            if (!UnityEditorInternal.InternalEditorUtility.isHumanControllingUs)
            {
                Version(out string gameversion, out int _);
                PlayerSettings.bundleVersion = gameversion;
            }

            //Backend
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Standalone, buildScript.BuildMacOS.ScriptingBackend);

            //Options
            BuildPlayerOptions buildOptions = new BuildPlayerOptions
            {
                scenes = buildScript.BuildMacOS.GetScenes(),
                locationPathName = "Build/MacOS/" + buildScript.GameName + ".app",
                target = BuildTarget.StandaloneOSX,
                options = BuildOptions.None
            };

            ProcessReport(BuildPipeline.BuildPlayer(buildOptions));
        }

        public static void BuildLinux()
        {
            var buildScript = GetBuildScript();
            if (buildScript == null)
            {
                DebugMessege.Log("Cannot build without a BuildScript.", DebugLevel.ERROR);
                return;
            }

            //Version
            if (!UnityEditorInternal.InternalEditorUtility.isHumanControllingUs)
            {
                Version(out string gameversion, out int _);
                PlayerSettings.bundleVersion = gameversion;
            }

            //Backend
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Standalone, buildScript.BuildLinux.ScriptingBackend);

            //Options
            BuildPlayerOptions buildOptions = new BuildPlayerOptions
            {
                scenes = buildScript.BuildLinux.GetScenes(),
                locationPathName = "Build/Linux/" + buildScript.GameName,
                target = BuildTarget.StandaloneLinux64,
                options = BuildOptions.None
            };

            ProcessReport(BuildPipeline.BuildPlayer(buildOptions));
        }

        public static void BuildGameServerWindows()
        {
            var buildScript = GetBuildScript();
            if (buildScript == null)
            {
                DebugMessege.Log("Cannot build without a BuildScript.", DebugLevel.ERROR);
                return;
            }

            //Version
            if (!UnityEditorInternal.InternalEditorUtility.isHumanControllingUs)
            {
                Version(out string gameversion, out int _);
                PlayerSettings.bundleVersion = gameversion;
            }

            //Backend
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Standalone, buildScript.BuildGameServerWindows.ScriptingBackend);

            //Options
            BuildPlayerOptions buildOptions = new BuildPlayerOptions
            {
                scenes = buildScript.BuildGameServerWindows.GetScenes(),
                locationPathName = "Build/GameServerWindows/" + buildScript.GameName + ".exe",
                target = BuildTarget.StandaloneWindows64,
                options = BuildOptions.EnableHeadlessMode
            };

            ProcessReport(BuildPipeline.BuildPlayer(buildOptions));
        }

        public static void BuildGameServerLinux()
        {
            var buildScript = GetBuildScript();
            if (buildScript == null)
            {
                DebugMessege.Log("Cannot build without a BuildScript.", DebugLevel.ERROR);
                return;
            }

            //Version
            if (!UnityEditorInternal.InternalEditorUtility.isHumanControllingUs)
            {
                Version(out string gameversion, out int _);
                PlayerSettings.bundleVersion = gameversion;
            }

            //Backend
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Standalone, buildScript.BuildGameServerLinux.ScriptingBackend);

            //Options
            BuildPlayerOptions buildOptions = new BuildPlayerOptions
            {
                scenes = buildScript.BuildGameServerLinux.GetScenes(),
                locationPathName = "Build/GameServerLinux/" + buildScript.GameName + ".exe",
                target = BuildTarget.StandaloneLinux64,
                options = BuildOptions.EnableHeadlessMode
            };

            ProcessReport(BuildPipeline.BuildPlayer(buildOptions));
        }

        public static void BuildUWP()
        {
            var buildScript = GetBuildScript();
            if (buildScript == null)
            {
                DebugMessege.Log("Cannot build without a BuildScript.", DebugLevel.ERROR);
                return;
            }

            //Version
            if (!UnityEditorInternal.InternalEditorUtility.isHumanControllingUs)
            {
                Version(out string gameversion, out int _);
                PlayerSettings.bundleVersion = gameversion;
                if (System.Version.TryParse(gameversion, out System.Version version))
                {
                    PlayerSettings.WSA.packageVersion = version;
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
            BuildPlayerOptions buildOptions = new BuildPlayerOptions
            {
                scenes = buildScript.BuildUWP.GetScenes(),
                locationPathName = "Build/UWP/",
                target = BuildTarget.StandaloneWindows64,
                options = BuildOptions.None
            };

            ProcessReport(BuildPipeline.BuildPlayer(buildOptions));
        }

        public static void BuildWebGL()
        {
            var buildScript = GetBuildScript();
            if (buildScript == null)
            {
                DebugMessege.Log("Cannot build without a BuildScript.", DebugLevel.ERROR);
                return;
            }

            //Version
            if (!UnityEditorInternal.InternalEditorUtility.isHumanControllingUs)
            {
                Version(out string gameversion, out int _);
                PlayerSettings.bundleVersion = gameversion;
            }

            //Backend
            //Always IL2CPP
            //PlayerSettings.SetScriptingBackend(BuildTargetGroup.WSA, buildScript.BuildUWP.ScriptingBackend);

            //Options
            BuildPlayerOptions buildOptions = new BuildPlayerOptions
            {
                scenes = buildScript.BuildWebGL.GetScenes(),
                locationPathName = "Build/WebGL/",
                target = BuildTarget.WebGL,
                options = BuildOptions.None
            };

            ProcessReport(BuildPipeline.BuildPlayer(buildOptions));
        }

        public static void Version(out string gameversion, out int bundleversion)
        {
            var g = Arg("-gameversion");
            gameversion = string.IsNullOrEmpty(g)? PlayerSettings.bundleVersion : g;
            bundleversion = 1;
            var argvalue = Arg("-bundleversion");
            if (string.IsNullOrEmpty(argvalue))
            {
                if (!int.TryParse(argvalue, out bundleversion))
                {
                    bundleversion = 1;
                }
            }
            Debug.Log("GameVersion:[" + gameversion + "][" + bundleversion + "]");
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

        private static BuildScript GetBuildScript(bool logPath = false)
        {
            foreach (var guid in AssetDatabase.FindAssets("t:BuildScript"))
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                if (logPath) Debug.Log("[GameWorkstore.Automation] BuildScript at path:" + path);
                return AssetDatabase.LoadAssetAtPath<BuildScript>(path);
            }
            return null;
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
    }
}