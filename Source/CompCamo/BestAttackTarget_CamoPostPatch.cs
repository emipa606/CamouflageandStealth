using System;
using HarmonyLib;
using Verse;
using Verse.AI;

namespace CompCamo
{
	// Token: 0x02000011 RID: 17
	[HarmonyPatch(typeof(AttackTargetFinder), "BestAttackTarget")]
	public class BestAttackTarget_CamoPostPatch
	{
		// Token: 0x06000069 RID: 105 RVA: 0x000067EC File Offset: 0x000049EC
		[HarmonyPostfix]
		[HarmonyPriority(0)]
		public static void PostFix(ref IAttackTarget __result, IAttackTargetSearcher searcher, TargetScanFlags flags, Predicate<Thing> validator = null, float minDist = 0f, float maxDist = 9999f, IntVec3 locus = default(IntVec3), float maxTravelRadiusFromLocus = 3.40282347E+38f, bool canBash = false, bool canTakeTargetsCloserThanEffectiveMinRange = true)
		{
			if (__result != null)
			{
				Pawn pawn = __result as Pawn;
				if (pawn != null && pawn != null)
				{
					Pawn pawn2 = searcher as Pawn;
					if (pawn2 != null && pawn2 != null && CamoUtility.IsTargetHidden(pawn, pawn2))
					{
						__result = null;
					}
				}
			}
		}
	}
}
