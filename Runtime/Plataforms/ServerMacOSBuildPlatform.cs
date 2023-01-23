using UnityEditor;
using UnityEngine;

namespace GameWorkstore.Automation
{
    [CreateAssetMenu(fileName = nameof(ServerMacOSBuildPlatform), menuName = "Automation/" + nameof(ServerMacOSBuildPlatform))]
    public class ServerMacOSBuildPlatform : StandaloneBuildPlatform
    {
        public override void Build()
        {
            if (!Validate(buildScript)) return;

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
                    UnityEditor.Build.NamedBuildTarget.Server,
                    ScriptDefinitions.Definitions
                );
            }

            //Backend
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Standalone, ScriptingBackend);

            //Options
            var buildOptions = new BuildPlayerOptions
            {
                scenes = GetScenes(),
                locationPathName = "Build/GameServerMacOS/" + buildScript.GameName.ToLower().Replace(" ","_") + "_" + PlayerSettings.bundleVersion.Replace(".","_") + ".app",
                target = BuildTarget.StandaloneOSX,
                subtarget = (int)StandaloneBuildSubtarget.Server,
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