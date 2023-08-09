using System;
using System.Linq;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEditor.Build.Reporting;
using GameWorkstore.Patterns;

namespace GameWorkstore.Automation
{
    public abstract class StandaloneBuildPlatform : BuildPlatform
    {
        public ScriptingImplementation ScriptingBackend = ScriptingImplementation.IL2CPP;
        public string[] AdditionalFolders = new string[0];
    }
}