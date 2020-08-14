using System;
using HarmonyLib;
using Verse;

namespace CompCamo
{
	// Token: 0x02000012 RID: 18
	[HarmonyPatch(typeof(Verb), "CanHitTargetFrom")]
	public class CanHitTargetFrom_CamoPostPatch
	{
		// Token: 0x0600006B RID: 107 RVA: 0x0000682C File Offset: 0x00004A2C
		[HarmonyPostfix]
		[HarmonyPriority(800)]
		public static void PostFix(ref Verb __instance, ref bool __result, IntVec3 root, LocalTargetInfo targ)
		{
			if (__result && targ.HasThing)
			{
				Thing thing = targ.Thing;
				Thing caster = __instance.caster;
				if (thing is Pawn && caster is Pawn && (((caster as Pawn).IsColonist && Controller.Settings.AllowNPCCamo) || !(caster as Pawn).IsColonist) && CamoUtility.IsTargetHidden(thing as Pawn, caster as Pawn))
				{
					__result = false;
				}
			}
		}
	}
}
