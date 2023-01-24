using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using GameWorkstore.Patterns;
using System.Collections.Generic;

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

        [Header("Generic Settings")]
        public string GameName;
        public AutoVersionWriter GameVersionWriterConfig;

        [HideInInspector] public List<BuildPlataform> BuildPlataforms;

        public bool Has<T>() where T : BuildPlataform
        {
            if (BuildPlataforms != null)
            {
                foreach (var p in BuildPlataforms)
                {
                    if (p is T)
                    {
                        return true;
                    }
                }
            }
            return false;  
        }

        public bool TryGet<T>(out T buildPlataform) where T : BuildPlataform
        {
            if (BuildPlataforms != null)
            {
                foreach (var p in BuildPlataforms)
                {
                    if (p is T t)
                    {
                        buildPlataform = t;
                        return true;
                    }
                }
            }
            buildPlataform = null;
            return false;  
        }

        public void TryBuild<T>() where T : BuildPlataform
        {
            if (BuildPlataforms != null)
            {
                foreach (var p in BuildPlataforms)
                {
                    if (p is T t)
                    {
                        t.Build();
                    }
                }
            }
            Debug.LogError("No build platform in build script found!"); 
        }
    }
}