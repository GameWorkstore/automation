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

        public AndroidBuildPlatform AndroidBuildPlatform;
        public IOSBuildPlatform IOSBuildPlatform;
        public WindowsBuildPlatform WindowsBuildPlatform;
        public MacOSBuildPlatform MacOSBuildPlatform;
        public LinuxBuildPlatform LinuxBuildPlatform;
        public ServerWindowsBuildPlatform ServerWindowsBuildPlatform;
        public ServerLinuxBuildPlatform ServerLinuxBuildPlatform;
        public ServerMacOSBuildPlatform ServerMacOSBuildPlatform;
        public UWPBuildPlatform UWPBuildPlatform;
        public WebGLBuildPlatform WebGLBuildPlatform;

        [Header("Generic Settings")]
        public string GameName;

        public AutoVersionWriter GameVersionWriterConfig;
        
        [Header("Build")]
        public HelpBox build = new HelpBox("Build Now", HelpBoxType.Info);
    }
}