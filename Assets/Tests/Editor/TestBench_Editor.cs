using System.IO;
using GameWorkstore.Automation;
using NUnit.Framework;

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

    [Test]
    public void CustomFolderIsCopiedWindows()
    {
        var buildScript = BuildClass.GetBuildScript();
        Assert.IsNotNull(buildScript);
        var path = Path.Combine(BuildClass.GetProjectPath(), "Build/Windows/");
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }
        BuildClass.BuildWindows();
        path = Path.Combine(path, "Packages");
        bool folderExists = Directory.Exists(path);
        Assert.AreEqual(true,folderExists);
    }

    [Test]
    public void CustomFolderIsCopiedLinux()
    {
        var buildScript = BuildClass.GetBuildScript();
        Assert.IsNotNull(buildScript);
        var path = Path.Combine(BuildClass.GetProjectPath(), "Build/Linux/");
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }
        BuildClass.BuildLinux();
        path = Path.Combine(path, "Packages");
        bool folderExists = Directory.Exists(path);
        Assert.AreEqual(true, folderExists);
    }

    [Test]
    public void CustomFolderIsCopiedGameServerWindows()
    {
        var buildScript = BuildClass.GetBuildScript();
        Assert.IsNotNull(buildScript);
        var path = Path.Combine(BuildClass.GetProjectPath(), "Build/GameServerWindows/");
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }
        BuildClass.BuildGameServerWindows();
        path = Path.Combine(path, "Packages");
        bool folderExists = Directory.Exists(path);
        Assert.AreEqual(true, folderExists);
    }

    [Test]
    public void CustomFolderIsCopiedGameServerLinux()
    {
        var buildScript = BuildClass.GetBuildScript();
        Assert.IsNotNull(buildScript);
        var path = Path.Combine(BuildClass.GetProjectPath(), "Build/GameServerLinux/");
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }
        BuildClass.BuildGameServerLinux();
        path = Path.Combine(path, "Packages");
        bool folderExists = Directory.Exists(path);
        Assert.AreEqual(true, folderExists);
    }

    [Test]
    public void CustomFolderIsCopiedAppleIOS()
    {
        var buildScript = BuildClass.GetBuildScript();
        Assert.IsNotNull(buildScript);
        var path = Path.Combine(BuildClass.GetProjectPath(), "Build/iOS/", buildScript.GameName);
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }
        BuildClass.BuildIOS();
        path = Path.Combine(path, "Packages");
        bool folderExists = Directory.Exists(path);
        Assert.AreEqual(true, folderExists);
    }
}
