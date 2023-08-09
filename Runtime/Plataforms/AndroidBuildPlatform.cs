using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using GameWorkstore.Patterns;

namespace GameWorkstore.Automation
{
    public class AndroidBuildPlatform : BuildPlatform
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
        public static void SetAndroidSignCredentials()
        {
            var buildScript = BuildClass.GetBuildScript();
            if (buildScript == null) return;
            SetAndroidSignCredentials(buildScript);
        }

        public static void SetAndroidSignCredentials(BuildScript buildScript)
        {
            if(buildScript.TryGet(out AndroidBuildPlatform androidBuildPlatform))
            {
                if (!androidBuildPlatform.UseKeystore) return;
                PlayerSettings.Android.useCustomKeystore = true;
                PlayerSettings.Android.keystoreName = GetProjectPath() + androidBuildPlatform.KeyStoreSettings.KeystorePath;
                PlayerSettings.Android.keystorePass = androidBuildPlatform.KeyStoreSettings.KeystorePassword;
                PlayerSettings.Android.keyaliasName = androidBuildPlatform.KeyStoreSettings.AliasName;
                PlayerSettings.Android.keyaliasPass = androidBuildPlatform.KeyStoreSettings.AliasPassword;
            }
            
        }

        public static void MakeDirectory(string relativePath)
        {
            var path = GetProjectPath() + relativePath;
            if (Directory.Exists(path)) return;
            Directory.CreateDirectory(path);
        }

        public override void OnBuild()
        {
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
                    locationPathName = subPathAab + GetBuildName() + ".aab",
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
                    locationPathName = subPathAPK + GetBuildName() + ".apk",
                    target = BuildTarget.Android,
                    options = GetOptions()
                };

                EditorUserBuildSettings.buildAppBundle = false;
                ProcessReport(BuildPipeline.BuildPlayer(buildOptions));
            }
        }
    }
}
