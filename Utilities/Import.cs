using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using Colorful;
using Leaf.xNet;
using System.Windows.Forms;

namespace NoNameFN.Utilities
{
    internal class Import
    {
        [STAThread]

        public static bool SubExist(string name)
        {
            if (Security.KeyAuthApp.user_data.subscriptions.Exists(x => x.subscription == name))
                return true;

            return false;
        }

        public static void LoadShit()
        {


            while (true)
            {
                UIManager.PrintLogo();

                // Display proxy type selection options
                Colorful.Console.WriteFormatted("\n[>] Proxys are not required unless you get banned from the epicgames/hotmail api!", Color.DarkGray, Color.DarkGray);
                Colorful.Console.WriteLine();
                Colorful.Console.WriteLine("\nSelect Proxy Type:", Color.LightGray);
                Colorful.Console.WriteFormatted("[{0}] - HTTP/S\n", Color.LightGray, Color.White, "1");
                Colorful.Console.WriteFormatted("[{0}] - Socks4\n", Color.LightGray, Color.White, "2");
                Colorful.Console.WriteFormatted("[{0}] - Socks5\n", Color.LightGray, Color.White, "3");
                Colorful.Console.WriteFormatted("[{0}] - Proxyless\n", Color.LightGray, Color.White, "4");
                Colorful.Console.WriteFormatted("[{0}] ", ">", Color.LightGray, Color.White);

                bool validChoice = false;
                char proxyTypeChoice = ' ';
                while (!validChoice)
                {
                    proxyTypeChoice = Colorful.Console.ReadKey().KeyChar;
                    Colorful.Console.WriteLine();
                    switch (proxyTypeChoice)
                    {
                        case '1':
                            Colorful.Console.Clear();
                            Variables.proxtype = ProxyType.HTTP;
                            validChoice = true;
                            Colorful.Console.WriteLineFormatted("\n[{0}] Proxy type set to HTTP/S.", Color.Green, Color.White, "+");
                            Thread.Sleep(1500);
                            Colorful.Console.Clear();
                            break;
                        case '2':
                            Colorful.Console.Clear();
                            Variables.proxtype = ProxyType.Socks4;
                            validChoice = true;
                            Colorful.Console.WriteLineFormatted("\n[{0}] Proxy type set to Socks4.", Color.Green, Color.White, "+");
                            Thread.Sleep(1500);
                            Colorful.Console.Clear();
                            break;
                        case '3':
                            Colorful.Console.Clear();
                            Variables.proxtype = ProxyType.Socks5;
                            validChoice = true;
                            Colorful.Console.WriteLineFormatted("\n[{0}] Proxy type set to Socks5.", Color.Green, Color.White, "+");
                            Thread.Sleep(1500);
                            Colorful.Console.Clear();
                            break;
                        case '4':
                            Colorful.Console.Clear();
                            Variables.useProxys = false; // Assuming this is how proxyless is handled
                            validChoice = true;
                            Colorful.Console.WriteLineFormatted("\n[{0}] Proxys disabled.", Color.Green, Color.White, "+");
                            Thread.Sleep(1500);
                            Colorful.Console.Clear();
                            break;
                        default:
                            Colorful.Console.Clear();
                            UIManager.PrintLogo();
                            Colorful.Console.WriteFormatted("[I] Proxys are not required unless you get banned from the epicgames/hotmail api!", Color.DarkGray, Color.DarkGray);
                            Colorful.Console.WriteLineFormatted("\n[!] Incorrect value entered. Please try again.", Color.Red);
                            Colorful.Console.WriteLine("\nSelect Proxy Type:", Color.LightGray);
                            Colorful.Console.WriteFormatted("[{0}] - HTTP/S\n", Color.LightGray, Color.White, "1");
                            Colorful.Console.WriteFormatted("[{0}] - Socks4\n", Color.LightGray, Color.White, "2");
                            Colorful.Console.WriteFormatted("[{0}] - Socks5\n", Color.LightGray, Color.White, "3");
                            Colorful.Console.WriteFormatted("[{0}] - Proxyless\n", Color.LightGray, Color.White, "4");
                            Colorful.Console.WriteFormatted("[{0}] ", ">", Color.LightGray, Color.White);
                            break;
                    }
                }

                // Load combos from Combo.txt
                if (!File.Exists("Combo.txt"))
                {
                    Colorful.Console.WriteLineFormatted("\n[!] Put your Combos in Combo.txt", Color.Red, Color.Red);
                    File.WriteAllText("Combo.txt", "example combo");
                    Thread.Sleep(5000);
                    Environment.Exit(0);
                }
                else if (File.ReadAllText("Combo.txt").Trim() == "example combo")
                {
                    Colorful.Console.WriteLineFormatted("\n[!] Put your Combos in Combo.txt", Color.Red, Color.Red);
                    Thread.Sleep(5000);
                    Environment.Exit(0);
                }
                else
                {
                    Variables.ccombos = Enumerable.ToList<string>(File.ReadAllLines("Combo.txt"));
                    foreach (string text in Variables.ccombos)
                    {
                        bool flag = text.Contains(":") || text.Contains(";") || text.Contains("|");
                        if (flag)
                        {
                            Variables.combos.Enqueue(text);
                        }
                    }
                }

                // Display loaded combo count
                Colorful.Console.WriteLineFormatted(
                    "\n[{0}] {1} combos loaded from Combo.txt",
                    "+",
                    Variables.ccombos.Count,
                    Color.Green,
                    Color.White
                );
                Thread.Sleep(1000);

                // Load Proxys if used
                if (Variables.useProxys)
                {
                    if (!File.Exists("Proxys.txt"))
                    {
                        Colorful.Console.WriteLineFormatted("\n[!] Put your Proxys in Proxys.txt", Color.Red, Color.Red);
                        File.WriteAllText("Proxys.txt", "example proxy");
                        Thread.Sleep(5000);
                        Environment.Exit(0);
                    }
                    else if (File.ReadAllText("Proxys.txt") == "example proxy")
                    {
                        Colorful.Console.WriteLineFormatted("\n[!] Put your Proxys in Proxys.txt", Color.Red, Color.Red);
                        Thread.Sleep(5000);
                        Environment.Exit(0);
                    }
                    else
                    {
                        Variables.Proxys = File.ReadAllLines("Proxys.txt").ToList();
                    }

                    Colorful.Console.WriteLineFormatted(
                 "\n[{0}] {1} proxys loaded from Proxys.txt",
                 "+",
                 Variables.Proxys.Count,
                 Color.Green,
                 Color.White
             );
                }
                Thread.Sleep(1000);
                Colorful.Console.Clear();

                if (SubExist("10"))// 1
                {
                    Variables.threadcount = 10;
                }
                else if (SubExist("50"))
                {
                    Variables.threadcount = 50;
                }
                else if (SubExist("100"))//1500
                {
                    Variables.threadcount = 100;
                }
                else if (SubExist("500"))
                {
                    Variables.threadcount = 500;
                }
                else if (SubExist("1000"))
                {
                    Variables.threadcount = 1000;
                }
                else if (SubExist("2000"))
                {
                    Variables.threadcount = 2000;
                }
                else if (SubExist("3000"))
                {
                    Variables.threadcount = 3000;
                }
                else if (SubExist("4000"))
                {
                    Variables.threadcount = 4000;
                }
                else if (SubExist("5000"))
                {
                    Variables.threadcount = 5000;
                }
                else if (SubExist("6000"))
                {
                    Variables.threadcount = 6000;
                }
                else if (SubExist("7000"))
                {
                    Variables.threadcount = 7000;
                }
                else if (SubExist("8000"))
                {
                    Variables.threadcount = 8000;
                }
                else if (SubExist("9000"))
                {
                    Variables.threadcount = 9000;
                }
                else if (SubExist("10000"))
                {
                    Variables.threadcount = 10000;
                }
                else
                {
                    Environment.Exit(0);
                }

                // Prompt to start checking
                // Improved color scheme for better readability

                // Adjust threading dynamically
                if (!Variables.useProxys)
                {
                    Colorful.Console.WriteLineFormatted("[>] Selected CPM: 2 ( PROXYLESS SPEED )", Color.LightGray, Color.LightCoral);
                    Random random = new Random();
                    Variables.threadcount = random.Next(1, 50); // Generates a random integer between 1 and 100 (inclusive)
                }
                else
                {
                    Colorful.Console.WriteLineFormatted("[>] Selected CPM: {0}", Variables.threadcount, Color.LightGray, Color.LightCoral);
                }


                if (Variables.useProxys)
                {
                    Colorful.Console.WriteLineFormatted("[>] Selected Proxy Type: {0}", Variables.proxtype, Color.LightGray, Color.LightCoral); // Light gray text on dark blue background

                }
                else
                {
                    Colorful.Console.WriteLineFormatted("[>] Selected Proxy Type: {0}", "Proxyless", Color.LightGray, Color.LightCoral); // Light gray text on dark blue background

                }
                Colorful.Console.WriteLineFormatted("[{0}] Press [{1}] to start checking.", ">", "ENTER", Color.Yellow, Color.DarkRed); // Yellow '>' on dark red background, and red 'ENTER'


                // Wait for Enter key to be pressed to continue
                while (Colorful.Console.ReadKey().Key != ConsoleKey.Enter)
                {
                    Colorful.Console.WriteLineFormatted("Starting...",  Color.Aqua, Color.Aqua); // Light gray text on dark blue background
                }

                break; // Exit the while (true) loop
            }
        }
    }
}
