using System.Collections;
using System.IO;
using GameWorkstore.Automation;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PackageTests
{
    [Test]
    public void BuildScriptIsPresent()
    {
        var buildScript = BuildClass.GetBuildScript();
        Assert.IsNotNull(buildScript);
    }
    
    // A Test behaves as an ordinary method
    [Test]
    public void VersionWriter_WritesFile()
    {
        var buildScript = BuildClass.GetBuildScript();

        var path = VersionWriter.GetFilePath(buildScript);
        Debug.Log(path);

        Assert.True(File.Exists(path));
        
        //VersionWriter.WriteVersion();
    }

    /*// A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator PackageTestsWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }*/
}
