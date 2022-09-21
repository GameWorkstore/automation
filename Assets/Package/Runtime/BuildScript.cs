using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using GameWorkstore.Patterns;

namespace GameWorkstore.Automation
{
    [Serializable]
    public class CustomScenes
    {
        public SceneAssetPath[] List;
    }

    [Serializable]
    public class ScriptDefinitions
    {
        public string[] Definitions = new string[0];
    }


    [Serializable]
    public class BuildBase
    {
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
    }

    [Serializable]
    public class BuildStandalone : BuildBase
    {
        public ScriptingImplementation ScriptingBackend = ScriptingImplementation.IL2CPP;
        public string[] AdditionalFolders = new string[0];
    }

    [Serializable]
    public class BuildAppleIOS : BuildBase
    {
        public bool UsePackageName = false;
        [ConditionalField("UsePackageName")] public string PackageName;
        public string[] AdditionalFolders = new string[0];
    }

    [Serializable]
    public class BuildAndroid : BuildBase
    {
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
    }

    [Serializable]
    public class AutoVersionWriter
    {
        public bool Enabled;
        [Tooltip("Path starting from Assets folder")]
        public string Path;
        public string Namespace;

        public AutoVersionWriter()
        {
            Enabled = true;
            Path = "Scripts/Version/";
            Namespace = "Unset.Namespace";
        }
    }

    [CreateAssetMenu(fileName = "BuildScript", menuName = "Automation/BuildScript")]
    public class BuildScript : ScriptableObject
    {
        public string GameName;

        public AutoVersionWriter GameVersionWriterConfig;

        [Header("Android")]
        public bool ViewBuildAndroid = false;
        [ConditionalField("ViewBuildAndroid")] public BuildAndroid BuildAndroid = new BuildAndroid();

        [Header("iOS")]
        public bool ViewBuildIOS = false;
        [ConditionalField("ViewBuildIOS")] public BuildAppleIOS BuildIOS = new BuildAppleIOS();

        [Header("Windows")]
        public bool ViewBuildWindows = false;
        [ConditionalField("ViewBuildWindows")] public BuildStandalone BuildWindows = new BuildStandalone();

        [Header("MacOS")]
        public bool ViewBuildMacOS = false;
        [ConditionalField("ViewBuildMacOS")] public BuildStandalone BuildMacOS = new BuildStandalone();

        [Header("Linux")]
        public bool ViewBuildLinux = false;
        [ConditionalField("ViewBuildLinux")] public BuildStandalone BuildLinux = new BuildStandalone();

        [Header("UWP")]
        public bool ViewBuildUWP = false;
        [ConditionalField("ViewBuildUWP")] public BuildBase BuildUWP = new BuildBase();

        [Header("WebGL")]
        public bool ViewBuildWebGL = false;
        [ConditionalField("ViewBuildWebGL")] public BuildBase BuildWebGL = new BuildBase();

        [Header("GameServer - Windows")]
        public bool ViewBuildGameServerWindows = false;
        [ConditionalField("ViewBuildGameServerWindows")] public BuildStandalone BuildServerWindows = new BuildStandalone();

        [Header("GameServer - Linux")]
        public bool ViewBuildGameServerLinux = false;
        [ConditionalField("ViewBuildGameServerLinux")] public BuildStandalone BuildServerLinux = new BuildStandalone();

        [Header("GameServer - MacOS")]
        public bool ViewBuildGameServerMacOS = false;
        [ConditionalField(("ViewBuildGameServerMacOS"))] public BuildStandalone BuildServerMacOS = new BuildStandalone();
        
        [Header("Build")]
        public HelpBox build = new HelpBox("Build Now", HelpBoxType.Info);

        [ButtonMethod] public void Android() { BuildClass.BuildAndroid(this); }
        [ButtonMethod] public void AppleIOS() { BuildClass.BuildIOS(this); }

        [ButtonMethod] public void Windows() { BuildClass.BuildWindows(this); }
        [ButtonMethod] public void MacOS() { BuildClass.BuildMacOS(this); }
        [ButtonMethod] public void Linux() { BuildClass.BuildLinux(this); }

        [ButtonMethod] public void WebGL() { BuildClass.BuildWebGL(this); }
        [ButtonMethod] public void UWP() { BuildClass.BuildUWP(this); }

        [ButtonMethod] public void ServerWindows() { BuildClass.BuildServerWindows(this); }
        [ButtonMethod] public void ServerLinux() { BuildClass.BuildServerLinux(this); }
        [ButtonMethod] public void ServerMacOS() { BuildClass.BuildServerMacOS(this); }
    }
}