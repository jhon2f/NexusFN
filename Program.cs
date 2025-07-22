using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using NoNameFN.Utilities;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;
namespace NoNameFN
{

  


    // Token: 0x0200000F RID: 15
    internal class Program
	{
        static bool _shouldExit = false; // Shared state
        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey);

        private const int VK_SHIFT = 0x10; // Shift key virtual key code

      
        private static void KeyMonitorLoop()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            while (true)
            {
                // Check if Shift key is pressed
                if ((GetAsyncKeyState(VK_SHIFT) & 0x8000) != 0)
                {
                    // Start or reset the stopwatch
                    if (!stopwatch.IsRunning)
                    {
                        stopwatch.Start();
                    }

                    // Check if Shift key has been held down for 3 seconds
                    if (stopwatch.Elapsed.TotalSeconds >= 1)
                    {
                        Console.WriteLine("Shift key held down for 3 seconds. Exiting...");
                        Environment.Exit(0);
                    }
                }
                else
                {
                    // Reset the stopwatch if Shift key is not pressed
                    stopwatch.Reset();
                    stopwatch.Start();
                }

            }
        }
    


    [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool RemoveMenu(IntPtr hMenu, uint uPosition, uint uFlags);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetConsoleScreenBufferSize(IntPtr hConsoleOutput, COORD dwSize);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

        [StructLayout(LayoutKind.Sequential)]
        struct COORD
        {
            public short X;
            public short Y;

            public COORD(short x, short y)
            {
                X = x;
                Y = y;
            }
        }

        const int GWL_STYLE = -16;
        const int WS_SIZEBOX = 0x00040000;
        const uint SC_MAXIMIZE = 0xF030;
        const uint SC_MINIMIZE = 0xF020;
        const uint SC_CLOSE = 0xF060;
        const uint SC_SIZE = 0xF000;
        const uint MF_BYCOMMAND = 0x00000000;
        const int STD_OUTPUT_HANDLE = -11;
        const int STD_INPUT_HANDLE = -10;
        const uint ENABLE_QUICK_EDIT_MODE = 0x0040;
        const uint ENABLE_MOUSE_INPUT = 0x0010;

        static void RestrictConsoleWindow()
        {
            IntPtr consoleWindow = GetConsoleWindow();
            IntPtr systemMenu = GetSystemMenu(consoleWindow, false);

            // Remove the maximize, minimize, close, and size options from the system menu
            RemoveMenu(systemMenu, SC_MAXIMIZE, MF_BYCOMMAND);
            RemoveMenu(systemMenu, SC_MINIMIZE, MF_BYCOMMAND);
            RemoveMenu(systemMenu, SC_CLOSE, MF_BYCOMMAND);
            RemoveMenu(systemMenu, SC_SIZE, MF_BYCOMMAND);

            // Disable console window resizing
            int windowStyle = GetWindowLong(consoleWindow, GWL_STYLE);
            SetWindowLong(consoleWindow, GWL_STYLE, windowStyle & ~WS_SIZEBOX);

            // Disable console window selection
            IntPtr consoleInputHandle = GetStdHandle(STD_INPUT_HANDLE);
            uint consoleMode;
            if (!GetConsoleMode(consoleInputHandle, out consoleMode))
            {
                Console.WriteLine("GetConsoleMode() failed! Reason: " + Marshal.GetLastWin32Error());
                Environment.Exit(1);
            }

            consoleMode &= ~(ENABLE_QUICK_EDIT_MODE | ENABLE_MOUSE_INPUT);
            if (!SetConsoleMode(consoleInputHandle, consoleMode))
            {
                Console.WriteLine("SetConsoleMode() failed! Reason: " + Marshal.GetLastWin32Error());
                Environment.Exit(1);
            }

            // Set the window size and screen buffer size to 120x40
            const short newWidth = 120;
            const short newHeight = 40;

            // Get handle to the console output
            IntPtr hOut = GetStdHandle(STD_OUTPUT_HANDLE);

            // Set the new screen buffer dimensions
            COORD newSize = new COORD(newWidth, newHeight);
            if (!SetConsoleScreenBufferSize(hOut, newSize))
            {
                Console.WriteLine("SetConsoleScreenBufferSize() failed! Reason: " + Marshal.GetLastWin32Error());
                Environment.Exit(1);
            }

            // Set the window size (This will also affect the screen buffer size to ensure compatibility)
            Console.SetWindowSize(newWidth, newHeight);
            Console.SetBufferSize(newWidth, newHeight);

        //    Console.WriteLine("Connected SuccessFully!");
        }

        private static string LoadSavedWebhook(string filePath)
        {
            if (File.Exists(filePath))
            {
                try
                {
                    return File.ReadAllText(filePath).Trim();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading saved webhook URL: {ex.Message}");
                    return string.Empty;
                }
            }
            return string.Empty;
        }

        private static void SaveWebhook(string filePath, string webhookUrl)
        {
            try
            {
                File.WriteAllText(filePath, webhookUrl);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving webhook URL: {ex.Message}");
            }
        }

        private static bool IsValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult)
                   && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

      
        // Token: 0x0600008A RID: 138 RVA: 0x000055B4 File Offset: 0x000037B4
        [Obfuscation(Feature = "virtualizer", Exclude = false)]
        private static void Main(string[] args)
        {
            string savedWebhookFile = "saved_webhook.txt";
            string savedWebhookUrl = LoadSavedWebhook(savedWebhookFile);


            string exePath = Assembly.GetExecutingAssembly().Location;
            var errorLogFile = "error_log.txt";
            var errorLogStream = new StreamWriter(errorLogFile, append: true);
            Console.SetError(errorLogStream);

            Variables.ExecutablePath = exePath;
            Variables.username = Environment.UserName;
            Colorful.Console.Title = "NoName - Sigma Version";
            Colorful.Console.SetWindowSize(120, 40);
            RestrictConsoleWindow();

            Thread exitThread = new Thread(KeyMonitorLoop);
            exitThread.IsBackground = true; // Ensure the thread does not prevent the application from exiting
            exitThread.Start();

            // Start the main program logic
            Security.Startlogin();

            // Load previous webhook URL from file

            Console.Clear();
            Colorful.Console.CursorVisible = false;

            // Ask user about webhook options
           // Console.WriteLine("Webhook Management:");
            if (string.IsNullOrEmpty(savedWebhookUrl))
            {
                Console.WriteLine("No saved webhook URL found.");
                Console.Write("Do you want to set a new Discord webhook URL? (yes/no): ");
                string userInput = Console.ReadLine().Trim().ToLower();

                if (userInput == "yes")
                {
                    Console.Write("Please enter the Discord webhook URL: ");
                    string newWebhookUrl = Console.ReadLine().Trim();
                    if (IsValidUrl(newWebhookUrl))
                    {
                        Variables.discordwebhook = newWebhookUrl;
                        SaveWebhook(savedWebhookFile, newWebhookUrl);
                        Console.WriteLine("Discord webhook has been set and saved.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid URL format. Please try again.");
                    }
                }
                else
                {
                    Console.WriteLine("No Discord webhook will be used.");
                }
            }
            else
            {
                string webhookPath = new Uri(savedWebhookUrl).AbsolutePath.TrimStart('/');

                Console.WriteLine($"Saved Webhook Url: {webhookPath}");
                Console.Write("Would you like to send to the saved webhook, change it, or not send to any webhook? (send/change/none): ");





                string choice = Console.ReadLine().Trim().ToLower();

                switch (choice)
                {
                    case "send":
                        Variables.discordwebhook = savedWebhookUrl;
                        Console.WriteLine("Using the saved webhook URL.");
                        break;
                    case "change":
                        Console.Write("Please enter the new Discord webhook URL: ");
                        string newWebhookUrl = Console.ReadLine().Trim();
                        if (IsValidUrl(newWebhookUrl))
                        {
                            Variables.discordwebhook = newWebhookUrl;
                            SaveWebhook(savedWebhookFile, newWebhookUrl);
                            Console.WriteLine("Discord webhook has been updated and saved.");
                        }
                        else
                        {
                            Console.WriteLine("Invalid URL format. Please try again.");
                        }
                        break;
                    case "none":
                        Console.WriteLine("No Discord webhook will be used.");
                        break;
                    default:
                        Console.WriteLine("Invalid option. No webhook will be used.");
                        break;
                }
                Thread.Sleep(1000);
                Console.Clear();
            }

            // Proceed with application setup
            Variables.useProxys = true;
            Variables.checkerruning = true;
            Program.CreateFLD();
            Import.LoadShit();
            Variables.checkerruning = true;
            Variables.sw.Start();


            new Thread(new ThreadStart(UIManager.CUI))
			{
				IsBackground = false
			}.Start();
			new Thread(new ThreadStart(UIManager.SwitchModes))
			{
				IsBackground = true
			}.Start();
			try
			{
				Threading.StartChecking();
			}
			catch (Exception)
			{
            }
        }

        // Token: 0x0600008B RID: 139 RVA: 0x00005668 File Offset: 0x00003868
        private static void CreateFLD()
        {
            // Define the base path for results
            string basePath = "Results";

            // Get the current date and time in a formatted string
            string dateTimeStamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

            // Create a folder name with the date and time
            string resultsPath = Path.Combine(basePath, "hits_" + dateTimeStamp);

            // Create the base results folder if it does not exist
            if (!Directory.Exists(resultsPath))
            {
                Directory.CreateDirectory(resultsPath);
            }

            // Set Variables.path to the results path



            // Create subfolders for skin count ranges within the results folder
            foreach (var folder in new[] { "1-5 Skins", "5-25 Skins", "25-50 Skins", "50-100 Skins", "100-150 Skins", "150-200 Skins", "200-250 Skins", "250+ Skins" })
            {
                string folderPath = Path.Combine(resultsPath, folder);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
            }
        }



        // Token: 0x0600008C RID: 140 RVA: 0x0000578C File Offset: 0x0000398C
        public static void AsResult(string fileName, string content)
		{
			string path = Path.Combine(Variables.path + Variables.module + "/" + Process.GetCurrentProcess().StartTime.ToString("dd-MM-yyyy-hh-mm"), fileName + ".txt");
			object exportlock = Variables.exportlock;
			object obj = exportlock;
			lock (obj)
			{
				File.AppendAllText(path, content + Environment.NewLine);
			}
		}
	}
}
