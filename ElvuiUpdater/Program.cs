

using System.Text.RegularExpressions;

namespace ElvuiUpdater
{
    public class Programm
    {
        public static void Main(string[] args)
        {

            UpdateElvui();
            Console.WriteLine("press any key...");
            Console.ReadKey();
        }

        

        public static void UpdateElvui()
        {
            try
            {
                var content = Utils.GetContent("https://www.tukui.org/welcome.php");
                var link = Utils.GetLink(content);
                Console.WriteLine("Ready to update " + link.Replace("/downloads/", ""));
                var url = "https://www.tukui.org" + link;
                Console.WriteLine("Download from " + url);
                Utils.Download(url);
                Console.WriteLine("Extracting");
                Utils.Extract();
                if (Utils.CheckVersions())
                {
                    Console.WriteLine("Already updated to latest version");
                    return;
                }
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