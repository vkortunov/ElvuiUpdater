

using ElvuiUpdater.SoftwareUpdate;
using System.Text.RegularExpressions;

namespace ElvuiUpdater
{
    public class Programm
    {
        public static void Main(string[] args)
        {
            var version = "version-1.1.0";
            GitSoftwareUpdater swUpdater = new GitSoftwareUpdater(version);
            if(swUpdater.HasUpdate)
            {
                var release = swUpdater.GetLastRelease();
                Console.WriteLine($"Has new version ({release?.Name}):\n{release.Body}\n\nPerform update");

                swUpdater.DoUpdate();
                return;
            }
            
            UpdateElvui();
            Console.WriteLine("press any key...");
            Console.ReadKey();
        }

        

        public static void UpdateElvui()
        {
            try
            {
                var content = Utils.GetContent("https://www.tukui.org/welcome.php");
                var version = Utils.GetRemoteVerion2(content);

                if (Utils.CheckVersions(version))
                {
                    Console.WriteLine($"Already updated to latest version {version}");
                    return;
                }

                var link = Utils.GetDownloadLink(content);
                Console.WriteLine("Ready to update " + link.Replace("/downloads/", ""));
                var url = "https://www.tukui.org" + link;
                Console.WriteLine("Download from " + url);
                Utils.Download(url);
                Console.WriteLine("Extracting");
                Utils.Extract();                
                Console.WriteLine("Get WoW location");
                var wowPath = Utils.GetWowLocation();
                Console.WriteLine(wowPath);
                Utils.Backup(wowPath);
                Utils.Update(wowPath);
                Console.WriteLine("Updated successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while updating: " + ex.Message);
            }
            Console.WriteLine("Done");
        }       

    }
}