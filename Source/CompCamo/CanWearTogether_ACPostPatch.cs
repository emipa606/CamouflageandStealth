using System;
using HarmonyLib;
using RimWorld;
using Verse;

namespace CompCamo
{
	// Token: 0x02000014 RID: 20
	[HarmonyPatch(typeof(ApparelUtility), "CanWearTogether")]
	public class CanWearTogether_ACPostPatch
	{
		// Token: 0x0600006F RID: 111 RVA: 0x0000692B File Offset: 0x00004B2B
		[HarmonyPostfix]
		public static void PostFix(ref bool __result, ThingDef A, ThingDef B, BodyDef body)
		{
			if (__result && CamoGearUtility.GetIsACApparel(A) && CamoGearUtility.GetIsACApparel(B))
			{
				__result = false;
			}
		}
	}
}
