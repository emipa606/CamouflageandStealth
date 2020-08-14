using System;
using System.Reflection;
using HarmonyLib;
using Multiplayer.API;
using Verse;

namespace CompCamo
{
	// Token: 0x0200001A RID: 26
	[StaticConstructorOnStartup]
	internal static class MultiplayerSupport
	{
		// Token: 0x0600007A RID: 122 RVA: 0x00006AD0 File Offset: 0x00004CD0
		static MultiplayerSupport()
		{
			if (!MP.enabled)
			{
				return;
			}
			MP.RegisterSyncMethod(typeof(ActiveCamoApparel), "ToggleActiveCamo", null);
			MethodInfo[] array = new MethodInfo[]
			{
				AccessTools.Method(typeof(CamoUtility), "Rnd100", null, null),
				AccessTools.Method(typeof(ActiveCamoApparel), "Break", null, null)
			};
			for (int i = 0; i < array.Length; i++)
			{
				MultiplayerSupport.FixRNG(array[i]);
			}
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00006B59 File Offset: 0x00004D59
		private static void FixRNG(MethodInfo method)
		{
			MultiplayerSupport.harmony.Patch(method, new HarmonyMethod(typeof(MultiplayerSupport), "FixRNGPre", null), new HarmonyMethod(typeof(MultiplayerSupport), "FixRNGPos", null), null, null);
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00006B93 File Offset: 0x00004D93
		private static void FixRNGPre()
		{
			Rand.PushState(Find.TickManager.TicksAbs);
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00006BA4 File Offset: 0x00004DA4
		private static void FixRNGPos()
		{
			Rand.PopState();
		}

		// Token: 0x04000026 RID: 38
		private static Harmony harmony = new Harmony("rimworld.pelador.compcamo.multiplayersupport");
	}
}
