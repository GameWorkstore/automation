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

        public List<BuildPlatform> BuildPlatforms;

        public bool Has<T>() where T : BuildPlatform
        {
            if (BuildPlatforms != null)
            {
                foreach (var p in BuildPlatforms)
                {
                    if (p is T)
                    {
                        return true;
                    }
                }
            }
            return false;  
        }

        public bool TryGet<T>(out T buildPlataform) where T : BuildPlatform
        {
            if (BuildPlatforms != null)
            {
                foreach (var p in BuildPlatforms)
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

        public void TryBuild<T>() where T : BuildPlatform
        {
            if (BuildPlatforms != null)
            {
                foreach (var p in BuildPlatforms)
                {
                    if (p is T t)
                    {
                        t.Build();
                    }
                }
            }
            Console.WriteLine("No build platform in build script found!"); 
            Debug.LogError("No build platform in build script found!"); 
        }
    }
}