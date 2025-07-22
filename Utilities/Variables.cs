using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Leaf.xNet;
using Newtonsoft.Json.Linq;

namespace NoNameFN.Utilities
{
	// Token: 0x02000016 RID: 22
	internal static class Variables
	{
		// Token: 0x060000A7 RID: 167 RVA: 0x00007534 File Offset: 0x00005734
		public static int CountOccurrences(string text, string subString)
		{
			int num = 0;
			int num2 = 0;
			while ((num2 = text.IndexOf(subString, num2)) != -1)
			{
				num2 += subString.Length;
				num++;
			}
			return num;
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00007570 File Offset: 0x00005770
		public static string Parse(string source, string left, string right)
		{
			return source.Split(new string[]
			{
				left
			}, StringSplitOptions.None)[1].Split(new string[]
			{
				right
			}, StringSplitOptions.None)[0];
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x000075A8 File Offset: 0x000057A8
		private static void parseJSON(string A, string B, List<KeyValuePair<string, string>> jsonlist)
		{
			jsonlist.Add(new KeyValuePair<string, string>(A, B));
			bool flag = B.StartsWith("[");
			if (flag)
			{
				JArray jarray = null;
				try
				{
					jarray = JArray.Parse(B);
				}
				catch
				{
					return;
				}
				foreach (JToken jtoken in jarray.Children())
				{
					Variables.parseJSON("", jtoken.ToString(), jsonlist);
				}
			}
			bool flag2 = B.Contains("{");
			if (flag2)
			{
				JObject jobject = null;
				try
				{
					jobject = JObject.Parse(B);
				}
				catch
				{
					return;
				}
				foreach (KeyValuePair<string, JToken> keyValuePair in jobject)
				{
					Variables.parseJSON(keyValuePair.Key, keyValuePair.Value.ToString(), jsonlist);
				}
			}
		}

		// Token: 0x060000AA RID: 170 RVA: 0x000076D0 File Offset: 0x000058D0
		public static IEnumerable<string> JSON(string input, string field, bool recursive = false, bool useJToken = false)
		{
			List<string> list = new List<string>();
			if (useJToken)
			{
				if (recursive)
				{
					bool flag = input.Trim().StartsWith("[");
					if (flag)
					{
						using (IEnumerator<JToken> enumerator = JArray.Parse(input).SelectTokens(field, false).GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								JToken jtoken = enumerator.Current;
								list.Add(jtoken.ToString());
							}
							return list;
						}
					}
					using (IEnumerator<JToken> enumerator2 = JObject.Parse(input).SelectTokens(field, false).GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							JToken jtoken2 = enumerator2.Current;
							list.Add(jtoken2.ToString());
						}
						return list;
					}
				}
				bool flag2 = input.Trim().StartsWith("[");
				if (flag2)
				{
					JArray jarray = JArray.Parse(input);
					list.Add(jarray.SelectToken(field, false).ToString());
				}
				else
				{
					JObject jobject = JObject.Parse(input);
					list.Add(jobject.SelectToken(field, false).ToString());
				}
			}
			else
			{
				List<KeyValuePair<string, string>> list2 = new List<KeyValuePair<string, string>>();
				Variables.parseJSON("", input, list2);
				foreach (KeyValuePair<string, string> keyValuePair in list2)
				{
					bool flag3 = keyValuePair.Key == field;
					if (flag3)
					{
						list.Add(keyValuePair.Value);
					}
				}
				bool flag4 = !recursive && list.Count > 1;
				if (flag4)
				{
					list = new List<string>
					{
						list.First<string>()
					};
				}
			}
			return list;
		}

		// Token: 0x060000AB RID: 171 RVA: 0x000078D0 File Offset: 0x00005AD0
		public static IEnumerable<string> LR(string input, string left, string right, bool recursive = false, bool useRegex = false)
		{
			bool flag = left == string.Empty && right == string.Empty;
			IEnumerable<string> result;
			if (flag)
			{
				result = new string[]
				{
					input
				};
			}
			else
			{
				bool flag2 = (left != string.Empty && !input.Contains(left)) || (right != string.Empty && !input.Contains(right));
				if (flag2)
				{
					result = new string[0];
				}
				else
				{
					string text = input;
					List<string> list = new List<string>();
					if (recursive)
					{
						if (useRegex)
						{
							try
							{
								string pattern = Variables.BuildLRPattern(left, right);
								foreach (object obj in Regex.Matches(text, pattern))
								{
									Match match = (Match)obj;
									list.Add(match.Value);
								}
								return list;
							}
							catch
							{
								return list;
							}
						}
						try
						{
							while (left == string.Empty || (text.Contains(left) && (right == string.Empty || text.Contains(right))))
							{
								int startIndex = (left == string.Empty) ? 0 : (text.IndexOf(left) + left.Length);
								text = text.Substring(startIndex);
								int length = (right == string.Empty) ? (text.Length - 1) : text.IndexOf(right);
								string text2 = text.Substring(0, length);
								list.Add(text2);
								text = text.Substring(text2.Length + right.Length);
							}
							return list;
						}
						catch
						{
							return list;
						}
					}
					if (useRegex)
					{
						string pattern2 = Variables.BuildLRPattern(left, right);
						MatchCollection matchCollection = Regex.Matches(text, pattern2);
						bool flag3 = matchCollection.Count > 0;
						if (flag3)
						{
							list.Add(matchCollection[0].Value);
						}
					}
					else
					{
						try
						{
							int startIndex2 = (left == string.Empty) ? 0 : (text.IndexOf(left) + left.Length);
							text = text.Substring(startIndex2);
							int length2 = (right == string.Empty) ? text.Length : text.IndexOf(right);
							list.Add(text.Substring(0, length2));
						}
						catch
						{
						}
					}
					result = list;
				}
			}
			return result;
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00007B84 File Offset: 0x00005D84
		private static string BuildLRPattern(string ls, string rs)
		{
			string text = string.IsNullOrEmpty(ls) ? "^" : Regex.Escape(ls);
			string text2 = string.IsNullOrEmpty(rs) ? "$" : Regex.Escape(rs);
			return string.Concat(new string[]
			{
				"(?<=",
				text,
				").+?(?=",
				text2,
				")"
			});
		}

        public class LastHit
        {
            public int Skins { get; set; }
            public int Matches { get; set; }
            public string Time { get; set; }
            public bool STW { get; set; }
        }

        public static LastHit lastHit;


        // Token: 0x0400003E RID: 62
        public static Stopwatch sw = new Stopwatch();

		// Token: 0x0400003F RID: 63
		public static bool checkerruning = false;



        // Token: 0x04000040 RID: 64
        public static int twofa = 0;

		// Token: 0x04000041 RID: 65
		public static int hits = 0;

		// Token: 0x04000042 RID: 66
		public static int bad = 0;

		// Token: 0x04000043 RID: 67
		public static int check = 0;

		// Token: 0x04000044 RID: 68
		public static int frees = 0;

		// Token: 0x04000045 RID: 69
		public static int flagged = 0;

		// Token: 0x04000046 RID: 70
		public static int tocheck = 0;

		// Token: 0x04000047 RID: 71
		public static int threadcount = 0;

		// Token: 0x04000048 RID: 72
		public static int retries = 0;

		// Token: 0x04000049 RID: 73
		public static int perrors = 0;

		// Token: 0x0400004A RID: 74
		public static string username;

        public static string ExecutablePath;

        public static string version = "1.2.3";
        public static string discordwebhook = "NULL";
        public static int realskincount = 0;


        // Token: 0x0400004B RID: 75
        public static string proxytype;

		// Token: 0x0400004C RID: 76
		public static int custom = 0;

		// Token: 0x0400004D RID: 77
		public static int cpm = 0;

		public static int matchescount = 0;

        
        // Token: 0x0400004E RID: 78
        public static ConcurrentQueue<string> combos = new ConcurrentQueue<string>();

		// Token: 0x0400004F RID: 79
		public static List<string> Proxys = new List<string>();

		// Token: 0x04000050 RID: 80
		public static List<string> ccombos = new List<string>();
        
        // Token: 0x04000051 RID: 81
        public static bool useProxys = true;

		// Token: 0x04000052 RID: 82
		public static string module;

		// Token: 0x04000053 RID: 83
		public static List<Func<string, bool>> Methods = new List<Func<string, bool>>();

		// Token: 0x04000054 RID: 84
		public static List<Thread> threads = new List<Thread>();

		// Token: 0x04000055 RID: 85
		public static string inboxword = "";

		// Token: 0x04000056 RID: 86
		public static bool isinbox = false;

		// Token: 0x04000057 RID: 87
		public static readonly object exportlock = new object();

		// Token: 0x04000058 RID: 88
		public static string path;

		// Token: 0x04000059 RID: 89
		public static int nonreg;

		// Token: 0x0400005A RID: 90
		public static string cookiess;

		// Token: 0x0400005B RID: 91
		public static int epictwofa;

		// Token: 0x0400005C RID: 92
		public static int looted;

		// Token: 0x0400005D RID: 93
		public static int fullaccess;

		// Token: 0x0400005E RID: 94
		public static int sfa;

		// Token: 0x0400005F RID: 95
		public static string password;

		// Token: 0x04000060 RID: 96
		public static ProxyType proxtype;

		// Token: 0x04000061 RID: 97
		public static int rares;

		// Token: 0x04000062 RID: 98
		public static int weird;

		// Token: 0x04000063 RID: 99
		public static int bans;

		// Token: 0x04000064 RID: 100
		public static int stw;

		// Token: 0x04000065 RID: 101
		public static int zeroskin;

		// Token: 0x04000066 RID: 102
		public static int oneplus;

		// Token: 0x04000067 RID: 103
		public static int tenplus;

		// Token: 0x04000068 RID: 104
		public static int twentyfiveplus;

		// Token: 0x04000069 RID: 105
		public static int fiftyplus;

		// Token: 0x0400006A RID: 106
		public static int hundredplus;

		// Token: 0x0400006B RID: 107
		public static int hundredfiftyplus;

		// Token: 0x0400006C RID: 108
		public static int twohundredplus;

		// Token: 0x0400006D RID: 109
		public static int twohundredfiftyplus;

		// Token: 0x0400006E RID: 110
		public static int BlackK;

		// Token: 0x0400006F RID: 111
		public static int SparkleS;

		// Token: 0x04000070 RID: 112
		public static int BlueS;

		// Token: 0x04000071 RID: 113
		public static int TheR;

		// Token: 0x04000072 RID: 114
		public static int Galaxy;

		// Token: 0x04000073 RID: 115
		public static int Ikonik;

		// Token: 0x04000074 RID: 116
		public static int Glow;

		// Token: 0x04000075 RID: 117
		public static int RoyaleB;

		// Token: 0x04000076 RID: 118
		public static int PsychoB;

		// Token: 0x04000077 RID: 119
		public static int TravisS;

		// Token: 0x04000078 RID: 120
		public static int Trailblazer;

		// Token: 0x04000079 RID: 121
		public static int AssaultT;

		// Token: 0x0400007A RID: 122
		public static int Wonder;

		// Token: 0x0400007B RID: 123
		public static int Wildcat;

		// Token: 0x0400007C RID: 124
		public static int OGSkull;

		// Token: 0x0400007D RID: 125
		public static int OGGhoul;

		// Token: 0x0400007E RID: 126
		public static int RenegadeR;

		// Token: 0x0400007F RID: 127
		public static int ogs;

		// Token: 0x04000080 RID: 128
		public static int banned;

		// Token: 0x04000081 RID: 129
		public static int zeromatches;

		// Token: 0x04000082 RID: 130
		public static int onemplus;

        public static int onetofivehundo;

        // Token: 0x04000083 RID: 131
        public static int fivehundredmplus;

		// Token: 0x04000084 RID: 132
		public static int onekmplus;

		// Token: 0x04000085 RID: 133
		public static int twoandhalfandmplus;

		// Token: 0x04000086 RID: 134
		public static int fivekmplus;

		// Token: 0x04000087 RID: 135
		public static int tenkmplus;

		// Token: 0x04000088 RID: 136
		public static int privatestats;

		// Token: 0x04000089 RID: 137
		public static string mode = "1";
	}
}
