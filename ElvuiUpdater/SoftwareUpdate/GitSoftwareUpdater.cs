using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ElvuiUpdater.SoftwareUpdate
{
    public class GitSoftwareUpdater
    {
        private string GetContent(string url)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Chrome");
            var response = client.GetAsync(url).GetAwaiter().GetResult();
            var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            return content;
        }
        private void Download(string link, string file)
        {

            Thread thread = new Thread(() => {
                WebClient client = new WebClient();
                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
                client.DownloadFileAsync(new Uri(link), file);
            });
            thread.Start();
            while (!completeDownload) { Thread.Sleep(1000); }
            completeDownload = false;
        }
        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (progressIndicator == null)
                progressIndicator = new ProgressIndicator(e.TotalBytesToReceive);
            progressIndicator.SetProgress(e.BytesReceived);
        }
        void client_DownloadFileCompleted(object? sender, AsyncCompletedEventArgs e)
        {
            Console.WriteLine("Completed");
            completeDownload = true;
            progressIndicator = null;
        }

        bool completeDownload = false;
        ProgressIndicator? progressIndicator;
        private Release? lastVersion;
        readonly string extractPath = "tmp";
        public GitSoftwareUpdater(string currentVersion)
        {
            Clean();
            var url = "https://api.github.com/repos/vkortunov/ElvuiUpdater/releases";
            var curVersion = Version.Parse(currentVersion);

            var content = GetContent(url);
            var releases = JsonSerializer.Deserialize<List<Release>>(content);


            lastVersion = releases?.Where(v => v.Draft == false && v.Prerelease == false && Version.Parse(v.Name) > curVersion)?.MaxBy(v => Version.Parse(v.Name));
        }
        public bool HasUpdate => lastVersion != null && lastVersion.Assets.Count > 0;

        public Release? GetLastRelease()
        {
            return lastVersion;
        }


        private void CloseApp(string ArrayProcessName)
        {
            string[] processName = ArrayProcessName.Split(',');
            foreach (string appName in processName)
            {
                Process[] localByNameApp = Process.GetProcessesByName(appName);//Get all processes with program name
                if (localByNameApp.Length > 0)
                {
                    foreach (var app in localByNameApp)
                    {
                        if (!app.HasExited && app.MainWindowTitle == "your file name")
                        {
                            app.Kill();//close the process
                        }
                    }
                }
            }
        }

        void Clean()
        {
            if (File.Exists("update.bat"))
                File.Delete("update.bat");

            foreach (var file in Directory.GetFiles("./","*.bcp"))
                File.Delete(file);
        }

        void Restart()
        {
            var bat = $"timeout 5\nren ElvuiUpdater.exe ElvuiUpdater.exe.bcp \ncopy tmp\\ElvuiUpdater.exe ElvuiUpdater.exe \nElvuiUpdater.exe";
            using (var stream = File.CreateText("update.bat"))
            {
                stream.WriteLine(bat);
            }
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe"; // запуск команды в консоли Windows
            process.StartInfo.Arguments = "/c update.bat"; // команда, которую нужно выполнить
            process.StartInfo.UseShellExecute = true; // если true, то команда будет выполнена в текущем окне консоли
            process.StartInfo.RedirectStandardOutput = false; // позволяет получать вывод команды
            
            process.Start();

        }
        public void DoUpdate()
        {
            if (lastVersion != null && lastVersion.Assets.Count > 0)
            {
                if(Directory.Exists(extractPath))
                    Directory.Delete(extractPath, true);
                
                foreach (var asset in lastVersion.Assets)
                {
                    Download(asset.BrowserDownloadUrl, asset.Name);
                    using (ZipArchive archive = ZipFile.OpenRead(asset.Name))
                    {
                        foreach (var entry in archive.Entries)
                        {
                            Console.WriteLine(entry.FullName);
                            string destinationPath = Path.GetFullPath(Path.Combine(extractPath, entry.FullName));

                            if (!File.Exists(destinationPath.Replace(Path.GetFileName(destinationPath), "")))
                                Directory.CreateDirectory(destinationPath.Replace(Path.GetFileName(destinationPath), ""));

                            entry.ExtractToFile(destinationPath);
                        }
                    }
                }
                Restart();
            }
        }
    }
}
