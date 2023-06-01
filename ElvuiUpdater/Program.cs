

using ElvuiUpdater.Domain;
using ElvuiUpdater.SoftwareUpdate;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ElvuiUpdater
{
    public class Programm
    {
        public static void Main(string[] args)
        {
            var version = "version-1.3.0";
            try
            {
                GitSoftwareUpdater swUpdater = new GitSoftwareUpdater(version);
                if (swUpdater.HasUpdate)
                {
                    var release = swUpdater.GetLastRelease();
                    Console.WriteLine($"Has new version ({release?.Name}):\n{release.Body}\n\nPerform update");

                    swUpdater.DoUpdate();
                    return;
                }
            }
            catch { }
            
            UpdateElvui();
            Console.WriteLine("press any key...");
            Console.ReadKey();
        }

        

        public static void UpdateElvui()
        {
            try
            {
                var content = Utils.GetContent("https://api.tukui.org/v1/addon/elvui");

                var elvuiVersion = JsonSerializer.Deserialize<TukuiVersion>(content);
                if (elvuiVersion?.Version == null)
                {
                    Console.WriteLine("Cant resolve remote version");
                    return;
                }

                if (Utils.CheckVersions(elvuiVersion?.Version))
                {
                    Console.WriteLine($"Already updated to latest version {elvuiVersion?.Version}");
                    return;
                }                

                Console.WriteLine("Ready to update " + elvuiVersion?.Version);
                Console.WriteLine("Download from " + elvuiVersion?.Url);
                Utils.Download(elvuiVersion.Url);
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