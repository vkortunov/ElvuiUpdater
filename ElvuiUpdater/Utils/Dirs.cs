using Microsoft.Win32;

namespace ElvuiUpdater
{
    public partial class Utils
    {
        public static IEnumerable<string> GetElvUIInstalledLocations()
        {

            var gamesPaths = new List<string>() { "_retail_", "_forever_", "_classic_" };

            var key = @"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Blizzard Entertainment\World of Warcraft";
            var installPath = Registry.GetValue(key, "InstallPath", null) as string;
            if (string.IsNullOrEmpty(installPath)) 
                return Enumerable.Empty<string>();
            var dirs = Directory.GetDirectories(installPath + "..\\");
            if(dirs.Length > 0)
            {
                var games = gamesPaths.SelectMany(g => dirs.Where(d => d.EndsWith("\\" + g)));
                games = games.Where(d => Directory.Exists(d + @"\Interface\AddOns\ElvUI"));
                return games;
            }

            return dirs;
        }
        public static string GetWowLocation()
        {
            var key = @"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Blizzard Entertainment\World of Warcraft";
            var installPath = (string)Registry.GetValue(key, "InstallPath", null);
            return installPath + "..\\_retail_\\";
        }

        public static void Backup(string path)
        {
            var addons = @"\Interface\AddOns";
            var backupDir = "Backup-" + DateTime.Now.ToString("yyyyMMddhhmmss");
            var dirs = Directory.GetDirectories(path + addons, "ElvUI*");
            Directory.CreateDirectory(backupDir);
            foreach (var dir in dirs)
            {
                //Console.WriteLine(dir);
                var name = dir.Substring(dir.LastIndexOf('\\') + 1, dir.Length - dir.LastIndexOf('\\') - 1);
                //Console.WriteLine(name);
                Utils.CopyDir(dir, backupDir + "\\" + name);
                Directory.Delete(dir, true);
            }
        }

        public static void Update(string path)
        {
            var addons = @"\Interface\AddOns";
            string extractPath = @".\extract";
            var dirs = Directory.GetDirectories(extractPath);
            foreach (var dir in dirs)
            {
                //Console.WriteLine(dir);
                var name = dir.Substring(dir.LastIndexOf('\\') + 1, dir.Length - dir.LastIndexOf('\\') - 1);
                //Console.WriteLine(name);
                Utils.CopyDir(dir, path + addons + "\\" + name);
                //Directory.Delete(dir, true);
            }
        }

        public static void CopyDir(string sourceFolder, string destFolder)
        {
            if (!Directory.Exists(destFolder))
                Directory.CreateDirectory(destFolder);

            // Get Files & Copy
            string[] files = Directory.GetFiles(sourceFolder);
            foreach (string file in files)
            {
                string name = Path.GetFileName(file);

                // ADD Unique File Name Check to Below!!!!
                string dest = Path.Combine(destFolder, name);
                File.Copy(file, dest);
            }

            // Get dirs recursively and copy files
            string[] folders = Directory.GetDirectories(sourceFolder);
            foreach (string folder in folders)
            {
                string name = Path.GetFileName(folder);
                string dest = Path.Combine(destFolder, name);
                CopyDir(folder, dest);
            }
        }
    }
}
