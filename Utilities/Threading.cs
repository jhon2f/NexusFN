using System;
using System.Reflection;
using System.Threading;

namespace NoNameFN.Utilities
{
	// Token: 0x02000013 RID: 19
	internal class Threading
	{
		// Token: 0x0600009C RID: 156 RVA: 0x000064FC File Offset: 0x000046FC
		[Obfuscation(Feature = "virtualizer", Exclude = false)]
		public static void StartChecking()
		{
			for (int i = 0; i < Variables.threadcount; i++)
			{
				Thread thread = new Thread(new ThreadStart(Threading.CheckCombos));
				Variables.threads.Add(thread);
				thread.Start();
			}
			foreach (Thread thread2 in Variables.threads)
			{
				thread2.Start();
			}
			foreach (Thread thread3 in Variables.threads)
			{
				thread3.Join();
			}
		}

		// Token: 0x0600009D RID: 157 RVA: 0x000065D8 File Offset: 0x000047D8
		[Obfuscation(Feature = "virtualizer", Exclude = false)]
		public static void CheckCombos()
		{
			string wombo;
			while (Variables.combos.TryDequeue(out wombo))
			{
				API.Check(wombo);
			}
		}
	}
}




































//using System;
//using System.Reflection;
//using System.Threading;

//namespace NoNameFN.Utilities
//{
//    // Token: 0x02000013 RID: 19
//    internal class Threading
//    {
//        // Token: 0x0600009C RID: 156 RVA: 0x000064FC File Offset: 0x000046FC
//        [Obfuscation(Feature = "virtualizer", Exclude = false)]
//        public static void StartChecking()
//        {
//            for (int i = 0; i < Variables.threadcount; i++)
//            {
//                Thread thread = new Thread(new ThreadStart(Threading.CheckCombos));
//                Variables.threads.Add(thread);
//                thread.Start();
//            }
//            foreach (Thread thread2 in Variables.threads)
//            {
//                thread2.Start();
//            }
//            foreach (Thread thread3 in Variables.threads)
//            {
//                thread3.Join();
//            }
//        }

//        // Token: 0x0600009D RID: 157 RVA: 0x000065D8 File Offset: 0x000047D8
//        [Obfuscation(Feature = "virtualizer", Exclude = false)]
//        public static void CheckCombos()
//        {
//            string wombo;
//            while (Variables.combos.TryDequeue(out wombo))
//            {
//                API.Check(wombo);
//            }
//        }
//    }
//}
