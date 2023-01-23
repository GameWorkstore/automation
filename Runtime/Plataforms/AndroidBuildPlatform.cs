using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using GameWorkstore.Patterns;

namespace GameWorkstore.Automation
{
    [CreateAssetMenu(fileName = nameof(AndroidBuildPlatform), menuName = "Automation/"+nameof(AndroidBuildPlatform))]
    public class AndroidBuildPlatform : BuildPlataform
    {
        [Header("Android options")]
        public bool UseKeystore = false;
        public bool UsePackageName = false;
        [ConditionalField("UsePackageName")] public string PackageName;
        public bool BuildAPK = false;
        public bool BuildAAB = true;
        public ScriptingImplementation ScriptingBackend = ScriptingImplementation.IL2CPP;

        [Serializable]
        public struct KeyStoreSettingsStruct
        {
            public string KeystorePath;
            public string KeystorePassword;
            public string AliasName;
            public string AliasPassword;
        }
        [ConditionalField("UseKeystore")] public KeyStoreSettingsStruct KeyStoreSettings;

        [MenuItem("Help/Automation/SetAndroidCredentials")]
        public static void SetAndroidSignCredentials(BuildScript buildScript)
        {
            if(buildScript == null) return;
            if (!buildScript.AndroidBuildPlatform.UseKeystore) return;
            PlayerSettings.Android.useCustomKeystore = true;
            PlayerSettings.Android.keystoreName = GetProjectPath() + buildScript.AndroidBuildPlatform.KeyStoreSettings.KeystorePath;
            PlayerSettings.Android.keystorePass = buildScript.AndroidBuildPlatform.KeyStoreSettings.KeystorePassword;
            PlayerSettings.Android.keyaliasName = buildScript.AndroidBuildPlatform.KeyStoreSettings.AliasName;
            PlayerSettings.Android.keyaliasPass = buildScript.AndroidBuildPlatform.KeyStoreSettings.AliasPassword;
        }

        public static void MakeDirectory(string relativePath)
        {
            var path = GetProjectPath() + relativePath;
            if (Directory.Exists(path)) return;
            Directory.CreateDirectory(path);
        }

        public override void Build()
        {
            if (!Validate(buildScript)) return;
            
            //PackageName
            if (UsePackageName && !string.IsNullOrEmpty(PackageName))
            {
                PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, PackageName);
            }

            //Version
            if (!UnityEditorInternal.InternalEditorUtility.isHumanControllingUs)
            {
                Version(out var gameVersion, out var bundleVersion);
                PlayerSettings.bundleVersion = gameVersion;
                PlayerSettings.Android.bundleVersionCode = bundleVersion;
            }

            //SetScriptDefinitions
            if (UseCustomScriptDefinitions)
            {
                PlayerSettings.SetScriptingDefineSymbols(
                    UnityEditor.Build.NamedBuildTarget.Android,
                    ScriptDefinitions.Definitions
                );
            }

            //Backend
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingBackend);

            //Keystore Config
            SetAndroidSignCredentials(buildScript);

            if (BuildAAB)
            {
                //Make
                const string subPathAab = "Build/Android/AAB/";
                MakeDirectory(subPathAab);

                var buildOptions = new BuildPlayerOptions
                {
                    scenes = GetScenes(),
                    locationPathName = subPathAab + buildScript.GameName.ToLower().Replace(" ","_") + "_" + PlayerSettings.bundleVersion.Replace(".","_") + ".aab",
                    target = BuildTarget.Android,
                    options = GetOptions()
                };

                EditorUserBuildSettings.buildAppBundle = true;
                ProcessReport(BuildPipeline.BuildPlayer(buildOptions));
            }

            if (BuildAPK)
            {
                //Make
                const string subPathAPK = "Build/Android/APK/";
                MakeDirectory(subPathAPK);

                var buildOptions = new BuildPlayerOptions
                {
                    scenes = GetScenes(),
                    locationPathName = subPathAPK + buildScript.GameName.ToLower().Replace(" ","_") + "_" + PlayerSettings.bundleVersion.Replace(".","_") + ".apk",
                    target = BuildTarget.Android,
                    options = GetOptions()
                };

                EditorUserBuildSettings.buildAppBundle = false;
                ProcessReport(BuildPipeline.BuildPlayer(buildOptions));
            }
        }
    }
}