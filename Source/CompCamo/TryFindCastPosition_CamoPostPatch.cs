using System;
using HarmonyLib;
using Verse;
using Verse.AI;

namespace CompCamo
{
	// Token: 0x02000019 RID: 25
	[HarmonyPatch(typeof(CastPositionFinder), "TryFindCastPosition")]
	public class TryFindCastPosition_CamoPostPatch
	{
		// Token: 0x06000078 RID: 120 RVA: 0x00006A4C File Offset: 0x00004C4C
		[HarmonyPrefix]
		[HarmonyPriority(800)]
		public static bool PreFix(ref bool __result, CastPositionRequest newReq, out IntVec3 dest)
		{
			dest = IntVec3.Invalid;
			Pawn caster = newReq.caster;
			Pawn pawn = newReq.target as Pawn;
			if (caster != null && pawn != null && caster != null && pawn != null)
			{
				bool flag;
				if (caster == null)
				{
					flag = (null != null);
				}
				else
				{
					Pawn_JobTracker jobs = caster.jobs;
					flag = ((jobs?.curJob) != null);
				}
				if (flag && CamoAIUtility.JobIsCastException(caster.jobs.curJob.def))
				{
					return true;
				}
				if (CamoUtility.IsTargetHidden(pawn, caster))
				{
					__result = false;
					return false;
				}
			}
			return true;
		}
	}
}
