using System;
using GameWorkstore.Automation;
using UnityEditor;
using UnityEngine;

namespace GameworkStore.Automation.Editor
{
    [CustomEditor(typeof(BuildScript))]
    public class BuildScriptEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            BuildScript buildScript = (BuildScript)target;
            if(!buildScript) return;
            Show(buildScript);
        }

        public void Show(BuildScript buildScript)
        {
            ShowButtonCreate<AndroidBuildPlatform>(buildScript, nameof(AndroidBuildPlatform));
            ShowButtonCreate<IOSBuildPlatform>(buildScript, nameof(IOSBuildPlatform));
            ShowButtonCreate<WindowsBuildPlatform>(buildScript, nameof(WindowsBuildPlatform));
            ShowButtonCreate<LinuxBuildPlatform>(buildScript, nameof(LinuxBuildPlatform));
            ShowButtonCreate<MacOSBuildPlatform>(buildScript, nameof(MacOSBuildPlatform));
            ShowButtonCreate<ServerWindowsBuildPlatform>(buildScript, nameof(ServerWindowsBuildPlatform));
            ShowButtonCreate<ServerMacOSBuildPlatform>(buildScript, nameof(ServerMacOSBuildPlatform));
            ShowButtonCreate<ServerLinuxBuildPlatform>(buildScript, nameof(ServerLinuxBuildPlatform));
            ShowButtonCreate<WebGLBuildPlatform>(buildScript, nameof(WebGLBuildPlatform));
            ShowButtonCreate<UWPBuildPlatform>(buildScript, nameof(UWPBuildPlatform));

        }

        public static void ShowButtonCreate<T>(BuildScript buildScript, string name) where T : BuildPlataform
        {
            if(buildScript.Has<T>()) return;
            if (GUILayout.Button("New "+ObjectNames.NicifyVariableName(name)))
            {
                MakeNewBuildPlatform<T>(buildScript, name);
            }
        }

        public static void MakeNewBuildPlatform<T>(BuildScript buildScript, string name) where T : BuildPlataform
        {
            if(buildScript.Has<T>()) return;
            T bp = CreateInstance<T>();
            bp.name = name;
            PosCreateBuildInstance(buildScript, bp);
        }

        public static void PosCreateBuildInstance(BuildScript buildScript, BuildPlataform bp)
        {
            buildScript.BuildPlataforms.Add(bp);
            bp.InitializeScriptable(buildScript);

            AssetDatabase.AddObjectToAsset(bp, buildScript);
            AssetDatabase.SaveAssets();

            EditorUtility.SetDirty(buildScript);
            EditorUtility.SetDirty(bp);
        }
    }
}