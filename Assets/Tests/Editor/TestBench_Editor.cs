using System.IO;
using GameWorkstore.Automation;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

public class TestBench_Editor
{
    [Test]
    public void BuildGameServerMacOS()
    {
        var buildScript = BuildClass.GetBuildScript();
        Assert.IsNotNull(buildScript);

        var path = Path.Combine(BuildClass.GetProjectPath(), "Build/GameServerMacOS/" + buildScript.GameName + ".app");
        if (Directory.Exists(path))
        {
            var parent = Directory.GetParent(path);
            if(parent != null)
            {
                Directory.Delete(parent.FullName,true);
            }
        }
        
        BuildClass.BuildGameServerMacOS();
        var success = Directory.Exists(path);
        Assert.AreEqual(true,success);
    }
}
