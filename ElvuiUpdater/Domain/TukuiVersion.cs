using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ElvuiUpdater.Domain
{
    public class TukuiVersion
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("slug")]
        public string Slug { get; set; }

        [JsonPropertyName("author")]
        public string Author { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; }

        [JsonPropertyName("changelog_url")]
        public string ChangelogUrl { get; set; }

        [JsonPropertyName("ticket_url")]
        public string TicketUrl { get; set; }

        [JsonPropertyName("git_url")]
        public string GitUrl { get; set; }

        [JsonPropertyName("patch")]
        public List<string> Patch { get; set; }

        [JsonPropertyName("last_update")]
        public string LastUpdate { get; set; }

        [JsonPropertyName("web_url")]
        public string WebUrl { get; set; }

        [JsonPropertyName("donate_url")]
        public string DonateUrl { get; set; }

        [JsonPropertyName("small_desc")]
        public string SmallDesc { get; set; }

        [JsonPropertyName("screenshot_url")]
        public string ScreenshotUrl { get; set; }

        [JsonPropertyName("directories")]
        public List<string> Directories { get; set; }
    }

}
