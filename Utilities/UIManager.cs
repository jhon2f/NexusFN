using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading;
using Colorful;
using NoNameFN;
using NoNameFN.Utilities;

namespace NoNameFN.Utilities
{
    // Token: 0x02000015 RID: 21
    internal class UIManager
    {
        // Token: 0x060000A2 RID: 162 RVA: 0x0000242D File Offset: 0x0000062D
        [Obfuscation(Feature = "virtualizer", Exclude = false)]
        public static void PrintLogo()
        {
            string welcomeMessage = "Welcome To NoName Checker | Version : " + Variables.version;
            string loggedInMessage = "Logged in as " + Variables.username + " | Your key will expire in " + Security.KeyAuthApp.ExpiryTimeLeft();
            string bottomMessage = "Hold down shift to exit.";

            int consoleWidth = Colorful.Console.WindowWidth;
            int consoleHeight = Colorful.Console.WindowHeight;

            // Calculate padding for center alignment
            int welcomePadding = (consoleWidth - welcomeMessage.Length) / 2;
            int loggedInPadding = (consoleWidth - loggedInMessage.Length) / 2;

            // Write messages with padding spaces
            Colorful.Console.WriteLine(new string(' ', welcomePadding) + welcomeMessage, Color.DeepSkyBlue);
            Colorful.Console.WriteLine(new string(' ', loggedInPadding) + loggedInMessage, Color.LightGreen);

            // Add new lines to move the bottom message closer to the middle of the console
            int linesToAdd = (consoleHeight / 8) - 4; // Adjusted to move the bottom message up slightly

            // Print empty lines to push the bottom message to the desired position
            for (int i = 0; i < linesToAdd; i++)
            {
                Colorful.Console.WriteLine();
            }

            // Print the bottom message slightly more to the left
            int bottomMessagePadding = (consoleWidth - bottomMessage.Length - 4) / 2; // Adjusted padding for left movement
            Colorful.Console.WriteLine(new string(' ', bottomMessagePadding) + bottomMessage, Color.Red);
        }



        // Token: 0x060000A3 RID: 163 RVA: 0x0000245F File Offset: 0x0000065F
        [Obfuscation(Feature = "virtualizer", Exclude = false)]
        public static void PrintLogoN()
        {


            string welcomeMessage = "Welcome To NoName Checker | Version : " + Variables.version;
            string loggedInMessage = "Not Logged In Yet.";

            int ConsoleWidth = Colorful.Console.WindowWidth;

            // Calculate padding for center alignment
            int welcomePadding = (ConsoleWidth - welcomeMessage.Length) / 2;
            int loggedInPadding = (ConsoleWidth - loggedInMessage.Length) / 2;

            // Write messages with padding spaces
            Colorful.Console.WriteLine(new string(' ', welcomePadding) + welcomeMessage, Color.DeepSkyBlue);
            Colorful.Console.WriteLine(new string(' ', loggedInPadding) + loggedInMessage, Color.LightSalmon);
        }

        // Token: 0x060000A4 RID: 164 RVA: 0x000067C0 File Offset: 0x000049C0
        [Obfuscation(Feature = "virtualizer", Exclude = false)]
        public static void SwitchModes()
        {
            for (; ; )
            {
                bool flag = Colorful.Console.ReadKey().Key == ConsoleKey.Enter;
                if (flag)
                {
                    bool flag2 = Variables.mode == "1";
                    if (flag2)
                    {
                        Variables.mode = "2";
                    }
                    else
                    {
                        bool flag3 = Variables.mode == "2";
                        if (flag3)
                        {
                            Variables.mode = "1";
                        }
                    }
                }
            }
        }

        // Token: 0x060000A5 RID: 165 RVA: 0x0000682C File Offset: 0x00004A2C
        [Obfuscation(Feature = "virtualizer", Exclude = false)]

        public static void CUI()
        {
            int retryCount = 0;
            DateTime lastRetryTime = DateTime.Now;

            while (true)
            {
                // Determine the maximum number of retries based on proxy usage
                int maxRetries = Variables.useProxys ? 500 : 100;

                // Check if the retry limit is exceeded within 5 seconds
                if (Variables.retries > maxRetries && (DateTime.Now - lastRetryTime).TotalSeconds <= 5)
                {
                    if (retryCount >= 100)
                    {
                        // Clear the console and display rate limited message
                        Colorful.Console.Clear();
                        CenterAlignText($"   Progress      [RATELIMITED]", Color.White, Color.Tomato);
                        CenterAlignText($"Hits          [{Variables.hits}]", Color.White, Color.MediumSeaGreen);
                        CenterAlignText($"Xbox 2FA      [{Variables.twofa}]", Color.White, Color.Gold);
                        CenterAlignText($"Epicgames 2FA [{Variables.epictwofa}]", Color.White, Color.DarkGoldenrod);
                        CenterAlignText($"Full Access   [{Variables.sfa}]", Color.White, Color.PaleVioletRed);
                        CenterAlignText($"Raped         [{Variables.banned}]", Color.White, Color.LightSlateGray);
                        CenterAlignText($"Fails         [{Variables.bad}]", Color.White, Color.Tomato);
                        Colorful.Console.WriteLine();

                        DisplayColumns();

                        // Display rate limited box with message based on proxy usage
                        DrawBox(new (string, ConsoleColor)[]
                        {
                    ("  Ratelimited!", ConsoleColor.Red),
                    Variables.useProxys
                        ? ("Use Better Proxies.", ConsoleColor.Gray)
                        : ("Use proxies or a VPN.", ConsoleColor.Gray)
                        });

                        // Separator
                        Colorful.Console.WriteLine("");
                        Colorful.Console.WriteLine("                                                           ↓");
                        Colorful.Console.WriteLine("");

                        // Display additional stats
                        CenterAlignText($"Defaults       [{Variables.zeroskin}]", Color.White, Color.LightSlateGray);
                        CenterAlignText($"OGS            [{Variables.ogs}]", Color.White, Color.Gold);
                        CenterAlignText($"STW            [{Variables.stw}]", Color.White, Color.Tomato);

                        // Wait for 3 seconds before resuming
                        Thread.Sleep(3000);

                        // Reset retry count and time
                        retryCount = 0;
                        lastRetryTime = DateTime.Now;
                    }
                    else
                    {
                        // Increment retry count and update last retry time
                        retryCount++;
                        lastRetryTime = DateTime.Now;
                    }
                }
                else
                {
                    // Normal operation
                    if (Variables.ccombos.Count <= Variables.check)
                    {
                        Variables.checkerruning = false;
                    }

                    if (Variables.checkerruning)
                    {
                        // Clear the console and display logo
                        Colorful.Console.Clear();
                        UIManager.PrintLogo();
                        Colorful.Console.WriteLine();

                        // Center align progress and stats
                        CenterAlignText($"   Progress      [{Variables.check}/{Variables.ccombos.Count}]", Color.White, Color.CornflowerBlue);
                        CenterAlignText($"Hits          [{Variables.hits}]", Color.White, Color.MediumSeaGreen);
                        CenterAlignText($"Xbox 2FA      [{Variables.twofa}]", Color.White, Color.Gold);
                        CenterAlignText($"Epicgames 2FA [{Variables.epictwofa}]", Color.White, Color.DarkGoldenrod);
                        CenterAlignText($"Full Access   [{Variables.sfa}]", Color.White, Color.PaleVioletRed);
                        CenterAlignText($"Raped         [{Variables.banned}]", Color.White, Color.LightSlateGray);
                        CenterAlignText($"Fails         [{Variables.bad}]", Color.White, Color.Tomato);
                        Colorful.Console.WriteLine();

                        // Display skins and matches in columns
                        DisplayColumns();

                        // Display progress stats with conditional CPM display
                        int displayCpm = Variables.cpm < 9 ? Variables.cpm : Variables.threadcount;
                        DrawBox(new (string, ConsoleColor)[]
                        {
        ("Retries       [" + Variables.retries + "]", ConsoleColor.Gray),
        ("CPM           [" + displayCpm + "]", ConsoleColor.Gray),
        ("Time elapsed  [" + Variables.sw.Elapsed.Minutes + "m " + Variables.sw.Elapsed.Seconds + "s]", ConsoleColor.Gray)
                        });

                        // Separator
                        Colorful.Console.WriteLine("");
                        Colorful.Console.WriteLine("                                                           ↓");
                        Colorful.Console.WriteLine("");

                        // Display additional stats
                        CenterAlignText($"Defaults       [{Variables.zeroskin}]", Color.White, Color.LightSlateGray);
                        CenterAlignText($"OGS            [{Variables.ogs}]", Color.White, Color.Gold);
                        CenterAlignText($"STW            [{Variables.stw}]", Color.White, Color.Tomato);

                        // Display last hit information
                        DisplayLastHit();

                        // Reset CPM and sleep based on threads
                        Variables.cpm = 0;

                        // Calculate the delay to match the desired checks per minute
                        int checksPerMinute = Variables.threadcount;
                        int millisecondsPerMinute = 60000;
                        int sleepDuration = millisecondsPerMinute / checksPerMinute;

                        // Ensure a minimum sleep duration to prevent excessive CPU usage
                        sleepDuration = Math.Max(sleepDuration, 10); // Minimum of 10 milliseconds

                        Thread.Sleep(sleepDuration);
                    }

                    else
                    {
                        // Clear the console and display progress when not checking
                        Colorful.Console.Clear();
                        CenterAlignText($"   Progress      [{Variables.check}/{Variables.ccombos.Count}]", Color.White, Color.CornflowerBlue);
                        CenterAlignText($"   Hits          [{Variables.hits}]", Color.White, Color.MediumSeaGreen);
                        CenterAlignText($"   Xbox 2FA      [{Variables.twofa}]", Color.White, Color.Gold);
                        CenterAlignText($"   Epicgames 2FA [{Variables.epictwofa}]", Color.White, Color.DarkGoldenrod);
                        CenterAlignText($"   Full Access   [{Variables.sfa}]", Color.White, Color.PaleVioletRed);
                        CenterAlignText($"   Raped         [{Variables.banned}]", Color.White, Color.LightSlateGray);
                        CenterAlignText($"   Fails         [{Variables.bad}]", Color.White, Color.Tomato);

                        // Add spacing before the finished checking box
                        Colorful.Console.WriteLine();
                        DisplayColumns();

                        // Finished checking box
                        DrawBox(new (string, ConsoleColor)[]
                        {
                    ("  Finished Checking!", ConsoleColor.Green),
                    ($"Time elapsed [{Variables.sw.Elapsed.Minutes}m {Variables.sw.Elapsed.Seconds}s]", ConsoleColor.Gray)
                        });

                        // Separator
                        Colorful.Console.WriteLine();
                        Colorful.Console.WriteLine(new string(' ', 55) + "↓");
                        Colorful.Console.WriteLine("");

                        // Display additional stats
                        CenterAlignText($"   Defaults       [{Variables.zeroskin}]", Color.White, Color.LightSlateGray);
                        CenterAlignText($"   OGS            [{Variables.ogs}]", Color.White, Color.Gold);
                        CenterAlignText($"   STW            [{Variables.stw}]", Color.White, Color.Tomato);

                        // Keep the console open
                        Thread.Sleep(Timeout.Infinite);
                    }
                }
            }
        }

        private static void DisplayColumns()
        {
            // Skins column
            var skinsColumn = new[]
            {
        new { Label = "250+ Skins", Value = Variables.twohundredfiftyplus, Color = Color.SteelBlue },
        new { Label = "200-250 Skins", Value = Variables.twohundredplus, Color = Color.CornflowerBlue },
        new { Label = "150-200 Skins", Value = Variables.hundredfiftyplus, Color = Color.RoyalBlue },
        new { Label = "100-150 Skins", Value = Variables.hundredplus, Color = Color.MediumSlateBlue },
        new { Label = "50-100 Skins", Value = Variables.fiftyplus, Color = Color.DodgerBlue },
        new { Label = "25-50 Skins", Value = Variables.twentyfiveplus, Color = Color.DeepSkyBlue },
        new { Label = "5-25 Skins", Value = Variables.tenplus, Color = Color.SkyBlue },
        new { Label = "1-5 Skins", Value = Variables.oneplus, Color = Color.LightSkyBlue }
    };

            // Matches column
            var matchesColumn = new[]
            {
        new { Label = "10,000+ Matches", Value = Variables.tenkmplus, Color = Color.SeaGreen },
        new { Label = "5,000+ Matches", Value = Variables.fivekmplus, Color = Color.MediumSeaGreen },
        new { Label = "2,500+ Matches", Value = Variables.twoandhalfandmplus, Color = Color.MediumAquamarine },
        new { Label = "1,000+ Matches", Value = Variables.hundredplus, Color = Color.MediumSpringGreen },
        new { Label = "500+ Matches", Value = Variables.fivehundredmplus, Color = Color.LightGreen },
        new { Label = "100+ Matches", Value = Variables.onetofivehundo, Color = Color.PaleGreen },
        new { Label = "1+ Matches", Value = Variables.onemplus, Color = Color.PaleGreen }
    };

            // Define column widths and offsets
            int leftColumnWidth = 30; // Width of the Skins column
            int rightColumnWidth = 30; // Width of the Matches column
            int columnSeparatorWidth = 10; // Space between columns
            int additionalRightOffset = 32; // Additional space to move Matches column further to the right

            // Get console width
            int consoleWidth = System.Console.WindowWidth;

            // Calculate starting positions
            int totalColumnWidth = leftColumnWidth + columnSeparatorWidth + rightColumnWidth;
            int startSkinsColumn = (consoleWidth / 6) - (totalColumnWidth / 6); // Center Skins column
            int startMatchesColumn = startSkinsColumn + leftColumnWidth + columnSeparatorWidth + additionalRightOffset; // Move Matches more to the right

            // Print columns side by side
            PrintColumnsSideBySide(skinsColumn, matchesColumn, leftColumnWidth, rightColumnWidth, columnSeparatorWidth, startSkinsColumn, startMatchesColumn);
        }

        private static void PrintColumnsSideBySide(dynamic[] leftColumn, dynamic[] rightColumn, int leftColumnWidth, int rightColumnWidth, int columnSeparatorWidth, int startSkinsColumn, int startMatchesColumn)
        {
            int maxRows = Math.Max(leftColumn.Length, rightColumn.Length);
            for (int i = 0; i < maxRows; i++)
            {
                // Print spaces to align the Skins column
                Colorful.Console.Write(new string(' ', startSkinsColumn));

                // Print left column (Skins)
                if (i < leftColumn.Length)
                {
                    PrintColumnText(leftColumn[i].Label, leftColumn[i].Value, leftColumn[i].Color, leftColumnWidth);
                }
                else
                {
                    Colorful.Console.Write(new string(' ', leftColumnWidth));
                }

                // Print separator
                Colorful.Console.Write(new string(' ', columnSeparatorWidth));

                // Print right column (Matches)
                if (i < rightColumn.Length)
                {
                    // Move cursor to the starting position for the Matches column
                    Colorful.Console.Write(new string(' ', startMatchesColumn - System.Console.CursorLeft));
                    PrintColumnText(rightColumn[i].Label, rightColumn[i].Value, rightColumn[i].Color, rightColumnWidth);
                }
                else
                {
                    Colorful.Console.Write(new string(' ', rightColumnWidth));
                }

                Colorful.Console.WriteLine();
            }
        }

        private static void PrintColumnText(string label, object value, Color color, int columnWidth)
        {
            // Adjust padding to ensure the text fits well within the column
            string formattedText = string.Format("{0} [{1}]", label.PadRight(columnWidth - 5), value);
            Colorful.Console.WriteFormatted(formattedText.PadRight(columnWidth), Color.White, color); // Ensure proper padding
        }


        private static void CenterAlignText(string text, Color textColor, Color bgColor)
        {
            int windowWidth = System.Console.WindowWidth;
            int textWidth = text.Length;
            int spaces = (windowWidth - textWidth) / 2;
            Colorful.Console.Write(new string(' ', spaces));
            Colorful.Console.WriteLineFormatted(text, textColor, bgColor);
        }


        public static void DrawBox((string text, ConsoleColor color)[] lines)
        {
            int consoleWidth = System.Console.WindowWidth;
            int maxLength = 0;

            // Find the maximum length of the lines
            foreach (var line in lines)
            {
                if (line.text.Length > maxLength)
                {
                    maxLength = line.text.Length;
                }
            }

            string border = new string('═', maxLength + 2);
            int totalWidth = maxLength + 4; // 2 for the padding spaces and 2 for the box borders
            int spaces = (consoleWidth - totalWidth) / 2;
            string padding = new string(' ', spaces);

            // Print the top border
            System.Console.WriteLine($"{padding}╔{border}╗");

            // Print each line with its specified color
            foreach (var line in lines)
            {
                System.Console.ForegroundColor = line.color;
                System.Console.WriteLine($"{padding}║ {line.text.PadRight(maxLength)} ║");
            }

            // Reset color and print the bottom border
            System.Console.ResetColor();
            System.Console.WriteLine($"{padding}╚{border}╝");
        }


        private static void DisplayLastHit()
        {
            if (Variables.lastHit != null)
            {
                Colorful.Console.WriteLine();
                CenterAlignText("Last Hit Info:", Color.White, Color.LightBlue);
                CenterAlignText($"Skins: {Variables.lastHit.Skins}", Color.White, Color.Crimson);
                CenterAlignText($"Matches: {Variables.lastHit.Matches}", Color.White, Color.Crimson);
                CenterAlignText($"Time: {Variables.lastHit.Time}", Color.White, Color.Crimson);
                CenterAlignText($"STW: {(Variables.lastHit.STW ? "Yes" : "No")}", Color.White, Color.Crimson);
            }
        }

        private static void Ratelimited()
        {
            if (Variables.lastHit != null)
            {
                CenterAlignText("Ratelimited", Color.White, Color.Red);
            }
        }



    }
}