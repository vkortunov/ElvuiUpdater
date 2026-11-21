using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ElvuiUpdater
{
    public partial class Utils
    {
        public static bool CheckVersions()
        {
            var currentVerison = GetMinCurrentVersion();
            var newVerison = GetDownloadedVersion();
            return currentVerison == newVerison;
        }

        public static bool CheckVersions(string? remoteVersion)
        {
            if (remoteVersion == null) return false;
            var currentVerison = GetMinCurrentVersion();            
            return currentVerison == remoteVersion;
        }

        public static string GetCurrentVersion()
        {
            var wowPath = GetWowLocation();
            var addons = @"\Interface\AddOns";
            return GetVerions(wowPath + addons);
        }

        public static string GetMinCurrentVersion()
        {
            var wowPaths = GetElvUIInstalledLocations();
            var addons = @"\Interface\AddOns";
            var versions = new List<string>();
            foreach (var wowPath in wowPaths)
            {
                versions.Add(GetVerions(wowPath + addons));
            }
            var minversion = versions.Min();
            return minversion;            
        }

        public static string GetDownloadedVersion()
        {
            string extractPath = @".\extract";
            return GetVerions(extractPath);

        }

        public static string GetVerions(string path)
        {
            var versFile = File.Exists(path + @"\ElvUI\ElvUI.toc") ? @"\ElvUI\ElvUI.toc" : @"\ElvUI\ElvUI_Mainline.toc";
            var text = File.ReadAllText(path + versFile);
            var template = @"Version: (?<verion>.+)\r?\n?";

            var regex = new Regex(template, RegexOptions.IgnoreCase);
            var match = regex.Match(text);
            var version = match.Groups["verion"]?.Value;
            version = version?.Trim('v', '\n', '\r');
            return version;
        }
    }
}
