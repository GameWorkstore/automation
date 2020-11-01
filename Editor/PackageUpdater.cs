#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.PackageManager;

namespace GameWorkstore.Automation
{
    public class PackageUpdater
    {
        [MenuItem("Help/PackageUpdate/GameWorkstore.Automation")]
        public static void TrackPackages()
        {
            Client.Add("git://github.com/GameWorkstore/automation.git");
        }
    }
}
#endif