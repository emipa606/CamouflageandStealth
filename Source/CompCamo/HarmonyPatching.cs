using System;
using System.Reflection;
using HarmonyLib;
using Verse;

namespace CompCamo
{
	// Token: 0x02000018 RID: 24
	[StaticConstructorOnStartup]
	internal static class HarmonyPatching
	{
		// Token: 0x06000077 RID: 119 RVA: 0x00006A33 File Offset: 0x00004C33
		static HarmonyPatching()
		{
			new Harmony("com.Pelador.RW.CompCamo").PatchAll(Assembly.GetExecutingAssembly());
		}
	}
}
