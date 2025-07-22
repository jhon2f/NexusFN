using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using Leaf.xNet;

namespace NoNameFN.Utilities
{
	// Token: 0x02000014 RID: 20
	internal class Translate
	{
		// Token: 0x0600009F RID: 159 RVA: 0x00006604 File Offset: 0x00004804
		[Obfuscation(Feature = "virtualizer", Exclude = false)]
        public static (string filteredSkins, int validSkinCount) GrabCosmetics(string items, string itemType)
        {
            int num = 0;
            List<string> list = new List<string>();
            string text = "";

            // Define skins to ignore
            HashSet<string> skinsToIgnore = new HashSet<string>
    {
        "Kirk Hammett",
        "Cursed Jack Sparrow",
        "Robert Trujillo",
        "James Hetfield"
    };

            try
            {
                using (HttpRequest httpRequest = new HttpRequest())
                {
                    string[] separator = new string[] { "," };
                    foreach (string str in items.Split(separator, StringSplitOptions.RemoveEmptyEntries))
                    {
                        string text2 = Translate.ExtractNameFromResponse(httpRequest.Get("https://fortnite-api.com/v2/cosmetics/br/" + str, null).ToString());

                        // Check if the extracted name is in the ignore list
                        if (!string.IsNullOrEmpty(text2) && !skinsToIgnore.Contains(text2))
                        {
                            Interlocked.Increment(ref num);
                            list.Add(text2);
                        }
                    }
                }

                foreach (string text3 in list)
                {
                    text = string.IsNullOrEmpty(text) ? text3 : text + ", " + text3;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                File.AppendAllText("Trans error.txt", ex.Message);
            }

            return (text, num); // Return both the filtered skins and the count
        }



        // Token: 0x060000A0 RID: 160 RVA: 0x00006770 File Offset: 0x00004970
        [Obfuscation(Feature = "virtualizer", Exclude = false)]
		public static string ExtractNameFromResponse(string response)
		{
			string pattern = "\"name\":\"(.*?)\"";
			Match match = Regex.Match(response, pattern);
			bool success = match.Success;
			string result;
			if (success)
			{
				result = match.Groups[1].Value.Replace("\\u0027", "'");
			}
			else
			{
				result = null;
			}
			return result;
		}
	}
}
