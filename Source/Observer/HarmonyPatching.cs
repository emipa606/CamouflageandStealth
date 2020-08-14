using System;
using System.Reflection;
using HarmonyLib;
using Verse;

namespace Observer
{
	// Token: 0x02000005 RID: 5
	[StaticConstructorOnStartup]
	internal static class HarmonyPatching
	{
		// Token: 0x06000006 RID: 6 RVA: 0x000020BF File Offset: 0x000002BF
		static HarmonyPatching()
		{
			new Harmony("com.Pelador.RW.Observer").PatchAll(Assembly.GetExecutingAssembly());
		}
	}
}
