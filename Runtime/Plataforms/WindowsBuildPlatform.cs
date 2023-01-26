using System;
using System.Linq;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEditor.Build.Reporting;
using GameWorkstore.Patterns;

namespace GameWorkstore.Automation
{
    public class WindowsBuildPlatform : StandaloneBuildPlatform
    {
        public override void OnBuild()
        {
            //Version
            if (!UnityEditorInternal.InternalEditorUtility.isHumanControllingUs)
            {
                Version(out var version, out _);
                PlayerSettings.bundleVersion = version;
            }

            //SetScriptDefinitions
            if (UseCustomScriptDefinitions)
            {
                PlayerSettings.SetScriptingDefineSymbols(
                    UnityEditor.Build.NamedBuildTarget.Standalone,
                    ScriptDefinitions.Definitions
                );
            }

            //Backend
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Standalone, ScriptingBackend);

            //Options
            var buildOptions = new BuildPlayerOptions
            {
                scenes = GetScenes(),
                locationPathName = "Build/Windows/" + GetBuildName() + ".exe",
                target = BuildTarget.StandaloneWindows64,
                options = GetOptions()
            };

            var buildReport = BuildPipeline.BuildPlayer(buildOptions);
            if (ProcessReportIsSuccess(buildReport))
            {
                CopyAdditionalFolders(buildOptions, AdditionalFolders);
            }
            ProcessReport(buildReport);
        }
    }
}