using GameWorkstore.NetworkLibrary;
using GameWorkstore.Patterns;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace GameWorkstore.Automation
{
    [Serializable]
    public class CustomScenes
    {
        public SceneAssetPath[] List;
    }

    [Serializable]
    public class BuildBase
    {
        public bool UseCustomScenes = false;
        public bool Development = false;
        [ConditionalField("UseCustomScenes")] public CustomScenes Scenes = new CustomScenes();

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
    }

    [Serializable]
    public class BuildAndroid : BuildBase
    {
        public bool UseKeystore = false;
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
    public class GameVersionWriterConfig
    {
        public bool Enabled = false;
        [Tooltip("Path starting from Assets folder")]
        public string Path = "/Scripts/Util/";
        public string Namespace = "Default";
    }

    [CreateAssetMenu(fileName = "BuildScript", menuName = "Automation/BuildScript")]
    public class BuildScript : ScriptableObject
    {
        public string GameName;

        public GameVersionWriterConfig GameVersionWriterConfig;

        [Header("Android")]
        public bool ViewBuildAndroid = false;
        [ConditionalField("ViewBuildAndroid")] public BuildAndroid BuildAndroid = new BuildAndroid();

        [Header("iOS")]
        public bool ViewBuildIOS = false;
        [ConditionalField("ViewBuildIOS")] public BuildBase BuildIOS = new BuildBase();

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
        [ConditionalField("ViewBuildGameServerWindows")] public BuildStandalone BuildGameServerWindows = new BuildStandalone();

        [Header("GameServer - Linux")]
        public bool ViewBuildGameServerLinux = false;
        [ConditionalField("ViewBuildGameServerLinux")] public BuildStandalone BuildGameServerLinux = new BuildStandalone();

        [Header("Build")]
        public HelpBox build = new HelpBox("Build Now", HelpBoxType.Info);

        [ButtonMethod] public static void Android() { BuildClass.BuildAndroid(); }
        [ButtonMethod] public static void AppleIOS() { BuildClass.BuildIOS(); }

        [ButtonMethod] public static void Windows() { BuildClass.BuildWindows(); }
        [ButtonMethod] public static void MacOS() { BuildClass.BuildMacOS(); }
        [ButtonMethod] public static void Linux() { BuildClass.BuildLinux(); }

        [ButtonMethod] public static void WebGL() { BuildClass.BuildWebGL(); }
        [ButtonMethod] public static void UWP() { BuildClass.BuildUWP(); }

        [ButtonMethod] public static void GameServerWindows() { BuildClass.BuildGameServerWindows(); }
        [ButtonMethod] public static void GameServerLinux() { BuildClass.BuildGameServerLinux(); }
    }
}