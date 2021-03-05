using System.IO;
using GameWorkstore.Automation;
using NUnit.Framework;

public class PackageTests
{
    [Test]
    public void BuildScriptIsPresent()
    {
        var buildScript = BuildClass.GetBuildScript();
        Assert.IsNotNull(buildScript);
    }

    [Test]
    public void VersionWriter_InitialValues()
    {
        var gameVersion = new AutoVersionWriter();

        Assert.AreEqual(gameVersion.Enabled, true);
        Assert.AreEqual(gameVersion.Path, "Scripts/Version/");
        Assert.AreEqual(gameVersion.Namespace, "Unset.Namespace");
    }


    // A Test behaves as an ordinary method
    [Test]
    public void VersionWriter_WritesFile()
    {
        var buildScript = BuildClass.GetBuildScript();

        var filePath = VersionWriter.GetFilePath(buildScript);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        buildScript.GameVersionWriterConfig.Enabled = true;

        VersionWriter.WriteVersion(buildScript);
        Assert.True(File.Exists(filePath));
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
