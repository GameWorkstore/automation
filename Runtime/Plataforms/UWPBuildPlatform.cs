using UnityEditor;
using UnityEngine;
using GameWorkstore.Patterns;

namespace GameWorkstore.Automation
{
    public class UWPBuildPlatform : BuildPlatform
    {
        public override void OnBuild()
        {
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

            //SetScriptDefinitions
            if (UseCustomScriptDefinitions)
            {
                PlayerSettings.SetScriptingDefineSymbols(
                    UnityEditor.Build.NamedBuildTarget.WindowsStoreApps,
                    ScriptDefinitions.Definitions
                );
            }

            //Backend
            //Always IL2CPP
            //PlayerSettings.SetScriptingBackend(BuildTargetGroup.WSA, buildScript.BuildUWP.ScriptingBackend);

            //Options
            var buildOptions = new BuildPlayerOptions
            {
                scenes = GetScenes(),
                locationPathName = "Build/UWP/",
                target = BuildTarget.WSAPlayer,
                options = GetOptions()
            };

            ProcessReport(BuildPipeline.BuildPlayer(buildOptions));
        }
    }
}