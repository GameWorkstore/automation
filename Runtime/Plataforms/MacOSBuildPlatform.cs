using UnityEditor;

namespace GameWorkstore.Automation
{
    public class MacOSBuildPlatform : StandaloneBuildPlatform
    {
        public enum MacOSArchitecture
        {
            INTEL64 = 0,
            ARM64 = 1,
            UNIVERSAL = 2
        };

        public MacOSArchitecture Architecture = MacOSArchitecture.UNIVERSAL;

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

            //Architecture
            PlayerSettings.SetArchitecture(BuildTargetGroup.Standalone, (int)Architecture);

            //Options
            var buildOptions = new BuildPlayerOptions
            {
                scenes = GetScenes(),
                locationPathName = "Build/MacOS/" + GetBuildName() + ".app",
                target = BuildTarget.StandaloneOSX,
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