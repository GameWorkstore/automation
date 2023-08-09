using System.IO;
using GameWorkstore.Automation;
using NUnit.Framework;

public class TestBench_Editor
{
    [Test]
    public void BuildServerMacOS()
    {
        var buildScript = BuildClass.GetBuildScript("BuildScript.asset");
        Assert.IsNotNull(buildScript);

        var path = Path.Combine(BuildPlatform.GetProjectPath(), "Build/GameServerMacOS/" + buildScript.GameName + ".app");
        if (Directory.Exists(path))
        {
            var parent = Directory.GetParent(path);
            if(parent != null)
            {
                Directory.Delete(parent.FullName,true);
            }
        }

        buildScript.TryBuild<ServerMacOSBuildPlatform>();
        var success = Directory.Exists(path);
        Assert.AreEqual(true,success);
    }

    [Test]
    public void CustomFolderIsCopiedWindows()
    {
        var buildScript = BuildClass.GetBuildScript("BuildScript.asset");
        Assert.IsNotNull(buildScript);
        var path = Path.Combine(BuildPlatform.GetProjectPath(), "Build/Windows/");
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }
        buildScript.TryBuild<WindowsBuildPlatform>();
        path = Path.Combine(path, "Packages");
        bool folderExists = Directory.Exists(path);
        Assert.AreEqual(true,folderExists);
    }

    [Test]
    public void CustomFolderIsCopiedLinux()
    {
        var buildScript = BuildClass.GetBuildScript("BuildScript.asset");
        Assert.IsNotNull(buildScript);
        var path = Path.Combine(BuildPlatform.GetProjectPath(), "Build/Linux/");
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }
        buildScript.TryBuild<LinuxBuildPlatform>();
        path = Path.Combine(path, "Packages");
        bool folderExists = Directory.Exists(path);
        Assert.AreEqual(true, folderExists);
    }

    [Test]
    public void CustomFolderIsCopiedServerWindows()
    {
        var buildScript = BuildClass.GetBuildScript("BuildScript.asset");
        Assert.IsNotNull(buildScript);
        var path = Path.Combine(BuildPlatform.GetProjectPath(), "Build/GameServerWindows/");
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }
        buildScript.TryBuild<ServerWindowsBuildPlatform>();
        path = Path.Combine(path, "Packages");
        bool folderExists = Directory.Exists(path);
        Assert.AreEqual(true, folderExists);
    }

    [Test]
    public void CustomFolderIsCopiedServerLinux()
    {
        var buildScript = BuildClass.GetBuildScript("BuildScript.asset");
        Assert.IsNotNull(buildScript);
        var path = Path.Combine(BuildPlatform.GetProjectPath(), "Build/GameServerLinux/");
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }
        buildScript.TryBuild<ServerLinuxBuildPlatform>();
        path = Path.Combine(path, "Packages");
        bool folderExists = Directory.Exists(path);
        Assert.AreEqual(true, folderExists);
    }

    [Test]
    public void CustomFolderIsCopiedAppleIOS()
    {
        var buildScript = BuildClass.GetBuildScript("BuildScript.asset");
        Assert.IsNotNull(buildScript);
        var path = Path.Combine(BuildPlatform.GetProjectPath(), "Build/iOS/", buildScript.GameName);
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }
        buildScript.TryBuild<IOSBuildPlatform>();
        path = Path.Combine(path, "Packages");
        bool folderExists = Directory.Exists(path);
        Assert.AreEqual(true, folderExists);
    }
}
