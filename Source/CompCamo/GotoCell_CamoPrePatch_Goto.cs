using System;
using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace CompCamo
{
	// Token: 0x02000017 RID: 23
	[HarmonyPatch(typeof(Toils_Goto), "GotoCell", new Type[]
	{
		typeof(TargetIndex),
		typeof(PathEndMode)
	})]
	public class GotoCell_CamoPrePatch_Goto
	{
		// Token: 0x06000075 RID: 117 RVA: 0x000069BC File Offset: 0x00004BBC
		[HarmonyPrefix]
		[HarmonyPriority(800)]
		public static bool PreFix(ref Toil __result, TargetIndex ind, PathEndMode peMode)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				actor.pather.StartPath(actor.jobs.curJob.GetTarget(ind), peMode);
			};
			toil.AddPreTickAction(delegate()
			{
				Pawn actor = toil.actor;
				if (Gen.IsHashIntervalTick(actor, 240) && actor.jobs.curJob.GetTarget(ind).HasThing)
				{
					Thing thing = actor.jobs.curJob.GetTarget(ind).Thing;
					if (thing is Pawn && GenHostility.HostileTo(actor, thing as Pawn))
					{
						CamoAIUtility.JobFailIfHid(actor, thing as Pawn, actor.jobs.curJob);
					}
				}
			});
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			__result = toil;
			return false;
		}
	}
}
