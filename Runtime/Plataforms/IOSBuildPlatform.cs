using System;
using System.Linq;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEditor.Build.Reporting;
using GameWorkstore.Patterns;

namespace GameWorkstore.Automation
{
    [CreateAssetMenu(fileName = nameof(IOSBuildPlatform), menuName = "Automation/" + nameof(IOSBuildPlatform))]
    public class IOSBuildPlatform : BuildPlataform
    {

        public bool UsePackageName = false;
        [ConditionalField("UsePackageName")] public string PackageName;
        public string[] AdditionalFolders = new string[0];

        public override void OnBuild()
        {
            //PackageName
            if (UsePackageName && !string.IsNullOrEmpty(PackageName))
            {
                PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, PackageName);
            }

            //SetScriptDefinitions
            if (UseCustomScriptDefinitions)
            {
                PlayerSettings.SetScriptingDefineSymbols(
                    UnityEditor.Build.NamedBuildTarget.iOS,
                    ScriptDefinitions.Definitions
                );
            }

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
                scenes = GetScenes(),
                locationPathName = "Build/iOS/" + buildScript.GameName.ToLower().Replace(" ","_") + "_" + PlayerSettings.bundleVersion.Replace(".","_"),
                target = BuildTarget.iOS,
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