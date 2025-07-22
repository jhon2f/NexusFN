using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;
using KeyAuth;
using NoNameFN.Utilities;
using static System.Net.Mime.MediaTypeNames;
namespace NoNameFN
{
    public static class AutoUpdater
    {
        public static void AutoUpdate()
        {
            if (Security.KeyAuthApp.response.message == "invalidver")
            {
                if (!string.IsNullOrEmpty(Security.KeyAuthApp.app_data.downloadLink))
                {
                    Colorful.Console.WriteLine("\nAn update has become available");
                    Thread.Sleep(1000);

                    Colorful.Console.WriteLine("Installing new update..");
                    Thread.Sleep(1500);

                    try
                    {
                        string destFile = Variables.ExecutablePath.Replace(".exe", "-Update.exe");

                        using (WebClient webClient = new WebClient())
                        {
                            webClient.DownloadFile(Security.KeyAuthApp.app_data.downloadLink, destFile);
                        }

                        Process.Start(destFile);
                        Process.Start(new ProcessStartInfo()
                        {
                            Arguments = $"/C choice /C Y /N /D Y /T 3 & Del \"{Variables.ExecutablePath}\"",
                            WindowStyle = ProcessWindowStyle.Hidden,
                            CreateNoWindow = true,
                            FileName = "cmd.exe"
                        });
                        Environment.Exit(0);
                    }
                    catch (WebException ex)
                    {
                        Colorful.Console.WriteLine($"Failed to download the update: {ex.Message}");
                        Thread.Sleep(1500);
                        Environment.Exit(1);
                    }
                }
            }
        }
    }
        internal class Security
    {
        [Obfuscation(Feature = "virtualizer", Exclude = false)]
        public static void Startlogin()
        {
            Security.KeyAuthApp.init();
            AutoUpdater.AutoUpdate();


            if (!File.Exists("License.txt"))
            {
                // Create a file and close it immediately to ensure it's properly initialized
                using (File.Create("License.txt")) { }
            }

            string licenseKey = "";
            while (true)
            {
                try
                {
                    licenseKey = File.ReadAllText("License.txt");
                    break;
                }
                catch
                {
                    Thread.Sleep(500);
                }
            }

            Thread.Sleep(1000);
            if (string.IsNullOrEmpty(licenseKey))
            {
                Colorful.Console.Clear();
                UIManager.PrintLogoN();
                Colorful.Console.Write("Enter your license key: ");
                licenseKey = Colorful.Console.ReadLine();
            }

            Security.KeyAuthApp.license(licenseKey); // Adjust method call to match your API

            if (Security.KeyAuthApp.response.success)
            {
                Colorful.Console.Clear();
                File.WriteAllText("License.txt", licenseKey);
                return;
            }
            else
            {
                File.Delete("License.txt");
                Colorful.Console.WriteLine(Security.KeyAuthApp.response.message, Color.LightSalmon);

                Thread.Sleep(2500);
                Environment.Exit(0); // Exit if login fails
            }
        }

        [Obfuscation(Feature = "virtualizer", Exclude = false)]
        public static api KeyAuthApp = new api("checker", "JF2QcRmfae", "e88fb8a7128d04d2ede8ebc7b01ebb338c99b58e13f53ec6394210126bb0a28c", Variables.version);
    }


}
