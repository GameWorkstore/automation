using System.IO;
using GameWorkstore.Automation;
using NUnit.Framework;
using UnityEditor;

public class TestBench_Play
{
    [Test]
    public void BuildScriptIsPresent()
    {
        var buildScript = BuildClass.GetBuildScript("BuildScript.asset");
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
        var buildScript = BuildClass.GetBuildScript("BuildScript.asset");

        var filePath = VersionWriter.GetAbsoluteFilePath(buildScript);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        buildScript.GameVersionWriterConfig.Enabled = true;

        VersionWriter.WriteVersion(buildScript);
        Assert.True(File.Exists(filePath));
    }

    [Test]
    public void AndroidSignConfigurationIsSet()
    {
        PlayerSettings.Android.useCustomKeystore = false;
        PlayerSettings.Android.keystoreName = string.Empty;
        PlayerSettings.Android.keystorePass = string.Empty;
        PlayerSettings.Android.keyaliasName = string.Empty;
        PlayerSettings.Android.keyaliasPass = string.Empty;

        var buildScript = BuildClass.GetBuildScript("BuildScript.asset");
        AndroidBuildPlatform.SetAndroidSignCredentials(buildScript);
        Assert.AreEqual(true,PlayerSettings.Android.useCustomKeystore);
        Assert.AreEqual("Assets/Store/TestKeyStore.keystore",PlayerSettings.Android.keystoreName);
        Assert.AreEqual("testkey123",PlayerSettings.Android.keystorePass);
        Assert.AreEqual("testalias",PlayerSettings.Android.keyaliasName);
        Assert.AreEqual("testalias123",PlayerSettings.Android.keyaliasPass);
    }
}
