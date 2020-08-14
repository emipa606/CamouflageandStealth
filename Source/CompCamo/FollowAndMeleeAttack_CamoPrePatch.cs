using System;
using HarmonyLib;
using Verse;
using Verse.AI;

namespace CompCamo
{
	// Token: 0x02000015 RID: 21
	[HarmonyPatch(typeof(Toils_Combat), "FollowAndMeleeAttack")]
	public class FollowAndMeleeAttack_CamoPrePatch
	{
		// Token: 0x06000071 RID: 113 RVA: 0x0000694C File Offset: 0x00004B4C
		[HarmonyPrefix]
		[HarmonyPriority(800)]
		public static bool PreFix(ref Toil __result, TargetIndex targetInd, Action hitAction)
		{
			Toil followAndAttack = new Toil();
			followAndAttack.tickAction = delegate()
			{
				Pawn actor = followAndAttack.actor;
				Job curJob = actor.jobs.curJob;
				JobDriver curDriver = actor.jobs.curDriver;
				Thing thing = curJob.GetTarget(targetInd).Thing;
				Pawn pawn = thing as Pawn;
				if (thing.Spawned && !ReachabilityImmediate.CanReachImmediate(actor, pawn, PathEndMode.Touch))
				{
					bool flag;
					if (actor == null)
					{
						flag = (null != null);
					}
					else
					{
						Pawn_MindState mindState = actor.mindState;
						flag = ((mindState?.meleeThreat) != null);
					}
					if (!flag && pawn != null && pawn != null && CamoUtility.IsTargetHidden(pawn, actor))
					{
						CamoAIUtility.CorrectJob(actor, pawn);
					}
				}
				if (!thing.Spawned)
				{
					curDriver.ReadyForNextToil();
					return;
				}
				if (thing != actor.pather.Destination.Thing || (!actor.pather.Moving && !ReachabilityImmediate.CanReachImmediate(actor, thing, PathEndMode.Touch)))
				{
					actor.pather.StartPath(thing, PathEndMode.Touch);
					return;
				}
				if (ReachabilityImmediate.CanReachImmediate(actor, thing, PathEndMode.Touch))
				{
					if (pawn != null && pawn.Downed && !curJob.killIncappedTarget)
					{
						curDriver.ReadyForNextToil();
						return;
					}
					hitAction();
				}
			};
			followAndAttack.defaultCompleteMode = ToilCompleteMode.Never;
			__result = followAndAttack;
			return false;
		}
	}
}
