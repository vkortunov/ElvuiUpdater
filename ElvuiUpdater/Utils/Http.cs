using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ElvuiUpdater
{
    public partial class Utils
    {
        public static string GetContent(string url)
        {
            HttpClient client = new HttpClient();
            var response = client.GetAsync(url).GetAwaiter().GetResult();
            var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            return content;
        }

        public static string? GetDownloadLink(string content)
        {
            var template = @"(?<link>/downloads/elvui-.+?\.zip)";
            var regex = new Regex(template, RegexOptions.Singleline | RegexOptions.IgnoreCase);
            var match = regex.Match(content);
            var link = match.Groups["link"]?.Value;
            return link;
        }

        public static string? GetRemoteVerion(string content)
        {
            var template = @"<div.*id=""version"">\s*(.+(?<version>\d\d\.\d\d))";
            var regex = new Regex(template, RegexOptions.Multiline | RegexOptions.IgnoreCase);
            var match = regex.Match(content);
            var version = match.Groups["version"]?.Value;
            return version;
        }

        public static string? GetRemoteVerion2(string content)
        {
            var template = @"(?<link>/downloads/elvui-(?<version>.+?)\.zip)";
            var regex = new Regex(template, RegexOptions.Multiline | RegexOptions.IgnoreCase);
            var match = regex.Match(content);
            var version = match.Groups["version"]?.Value;
            return version;
        }

        public static void Download(string link)
        {
            using (var client = new WebClient())
            {
                client.DownloadFile(link, "elvui.zip");
            }
        }
    }
}
