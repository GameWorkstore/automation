using UnityEditor;
using UnityEngine;

namespace GameWorkstore.Automation
{
    [CreateAssetMenu(fileName = nameof(WebGLBuildPlatform), menuName = "Automation/" + nameof(WebGLBuildPlatform))]
    public class WebGLBuildPlatform : BuildPlataform
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
                    UnityEditor.Build.NamedBuildTarget.WebGL,
                    ScriptDefinitions.Definitions
                );
            }

            //Options
            var buildOptions = new BuildPlayerOptions
            {
                scenes = GetScenes(),
                locationPathName = "Build/WebGL/",
                target = BuildTarget.WebGL,
                options = GetOptions()
            };

            ProcessReport(BuildPipeline.BuildPlayer(buildOptions));
        }
    }
}