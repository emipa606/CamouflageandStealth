using System;
using HarmonyLib;
using RimWorld;
using Verse;

namespace StealthBox
{
	// Token: 0x02000002 RID: 2
	[HarmonyPatch(typeof(ApparelUtility), "CanWearTogether")]
	public class CanWearTogether_PostPatch
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		[HarmonyPostfix]
		public static void PostFix(ref bool __result, ThingDef A, ThingDef B, BodyDef body)
		{
			if (__result)
			{
				bool flag = false;
				bool flag2 = false;
				if (A.thingClass.FullName == "StealthBox.CardboardBox")
				{
					flag = true;
				}
				if (B.thingClass.FullName == "StealthBox.CardboardBox")
				{
					flag2 = true;
				}
				if (flag && flag2)
				{
					__result = false;
				}
			}
		}
	}
}
