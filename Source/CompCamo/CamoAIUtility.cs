using System;
using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace CompCamo
{
	// Token: 0x02000002 RID: 2
	public class CamoAIUtility
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public static void CorrectLordForCamo(Pawn seer, Pawn target)
		{
			if (seer == null || target == null || (seer?.Map) == null || (target?.Map) == null || seer.Map != target.Map || seer.IsColonist)
			{
				return;
			}
			if (seer.RaceProps.IsMechanoid)
			{
				return;
			}
			if (!WildManUtility.AnimalOrWildMan(seer) || seer.RaceProps.FleshType != FleshTypeDefOf.Insectoid)
			{
				CamoAIUtility.CorrectLordForCamoAction(seer, target, seer.Map.IsPlayerHome);
				return;
			}
			if (seer != null)
			{
				Pawn_MindState mindState = seer.mindState;
				if (mindState != null)
				{
                    _ = mindState.duty;
                }
			}
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000020F0 File Offset: 0x000002F0
		public static void CorrectLordForCamoAction(Pawn seer, Pawn target, bool playerMap)
		{
			if ((seer?.mindState.duty) != null && !CamoAIUtility.HasFleeingDuty(seer))
			{
				if (!playerMap)
				{
					seer.mindState.duty = new PawnDuty(DutyDefOf.DefendBase, CamoAIUtility.GetNearestBaseItem(seer), -1f);
				}
				if (!CamoAIUtility.IsMeleeProcess(seer, target))
				{
					CamoAIUtility.CorrectJob(seer, target);
					return;
				}
			}
			else if (!CamoAIUtility.IsMeleeProcess(seer, target))
			{
				CamoAIUtility.CorrectJob(seer, target);
			}
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002164 File Offset: 0x00000364
		public static bool IsMeleeProcess(Pawn seer, Pawn target)
		{
			if (seer.CurrentEffectiveVerb.IsMeleeAttack)
			{
				bool flag;
				if (seer == null)
				{
					flag = (null != null);
				}
				else
				{
					Pawn_MindState mindState = seer.mindState;
					flag = ((mindState?.enemyTarget) != null);
				}
				if (flag)
				{
					return true;
				}
			}
			bool flag2;
			if (seer == null)
			{
				flag2 = (null != null);
			}
			else
			{
				Pawn_MindState mindState2 = seer.mindState;
				flag2 = ((mindState2?.meleeThreat) != null);
			}
			if (!flag2)
			{
				return false;
			}
			return true;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000021B5 File Offset: 0x000003B5
		public static void StopCurJobAndWait(Pawn pawn)
		{
			CamoAIUtility.ClearAllJobs(pawn);
			CamoAIUtility.GiveWaitJob(pawn, 117);
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000021C5 File Offset: 0x000003C5
		private static JobDef CamoPauseJobDef()
		{
			return JobDefOf.Wait_Wander;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000021CC File Offset: 0x000003CC
		private static void GiveWaitJob(Pawn pawn, int period)
		{
			ThingCompUtility.TryGetComp<PawnCamoData>(pawn).LastCamoCorrectTick = Find.TickManager.TicksGame;
            Job job = new Job(CamoAIUtility.CamoPauseJobDef())
            {
                expiryInterval = period,
                checkOverrideOnExpire = true
            };
            pawn.jobs.jobQueue.EnqueueFirst(job, null);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002224 File Offset: 0x00000424
		private static void ClearAllJobs(Pawn pawn)
		{
			if (pawn != null && pawn.Spawned && (pawn?.Map) != null)
			{
				bool flag;
				if (pawn == null)
				{
					flag = (null != null);
				}
				else
				{
					Pawn_JobTracker jobs = pawn.jobs;
					flag = ((jobs?.jobQueue) != null);
				}
				if (flag)
				{
					pawn.jobs.ClearQueuedJobs(true);
				}
				bool flag2;
				if (pawn == null)
				{
					flag2 = (null != null);
				}
				else
				{
					Pawn_JobTracker jobs2 = pawn.jobs;
					flag2 = ((jobs2?.curJob) != null);
				}
				if (flag2)
				{
					pawn.jobs.EndCurrentJob(JobCondition.Succeeded, false, true);
				}
			}
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002298 File Offset: 0x00000498
		private static IntVec3 GetNearestBaseItem(Pawn pawn)
		{
			IntVec3 position = pawn.Position;
			float num = 99999f;
			List<Thing> list = pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial);
			if (list.Count > 0)
			{
				Thing thing = null;
				foreach (Thing thing2 in list)
				{
					LocalTargetInfo localTargetInfo = thing2;
					if (ReachabilityUtility.CanReach(pawn, localTargetInfo, PathEndMode.Touch, Danger.Deadly, false, 0))
					{
						float num2 = IntVec3Utility.DistanceTo(pawn.Position, thing2.Position);
						if (num2 < num)
						{
							thing = thing2;
							num = num2;
						}
					}
				}
				if (thing != null)
				{
					position = thing.Position;
				}
			}
			return position;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002350 File Offset: 0x00000550
		private static bool HasFleeingDuty(Pawn pawn)
		{
			return pawn.mindState.duty.def == DutyDefOf.ExitMapRandom || pawn.mindState.duty.def == DutyDefOf.Steal || pawn.mindState.duty.def == DutyDefOf.Kidnap;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000023A8 File Offset: 0x000005A8
		public static void JobFailIfHid(Pawn seer, Pawn target, Job cur)
		{
			if (seer != null && target != null && cur != null && cur.def != CamoAIUtility.CamoPauseJobDef() && seer.Spawned && target.Spawned && (seer?.Map) != null && (target?.Map) != null && seer.Map == target.Map)
			{
				bool flag;
				if (seer == null)
				{
					flag = (null != null);
				}
				else
				{
					Pawn_MindState mindState = seer.mindState;
					flag = ((mindState?.meleeThreat) != null);
				}
				if (!flag)
				{
					if (CamoAIUtility.CanSeeSimply(seer, target))
					{
						if (CamoUtility.IsTargetHidden(target, seer))
						{
							CamoAIUtility.CorrectLordForCamo(seer, target);
							return;
						}
					}
					else
					{
						CamoAIUtility.CorrectLordForCamo(seer, target);
					}
				}
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x0000244C File Offset: 0x0000064C
		public static void RemoveTarget(Pawn seer, Pawn target)
		{
			if (seer.pather.Moving)
			{
				bool flag;
				if (seer == null)
				{
					flag = (null != null);
				}
				else
				{
					Pawn_JobTracker jobs = seer.jobs;
					flag = ((jobs?.curJob) != null);
				}
				if (flag && seer.jobs.curJob.AnyTargetIs(target))
				{
					seer.jobs.curDriver.Notify_PatherFailed();
					bool flag2;
					if (seer == null)
					{
						flag2 = (null != null);
					}
					else
					{
						Pawn_PathFollower pather = seer.pather;
						flag2 = ((pather?.curPath) != null);
					}
					if (flag2)
					{
						seer.pather.ResetToCurrentPosition();
						seer.pather.curPath.ReleaseToPool();
					}
				}
			}
			bool flag3;
			if (seer == null)
			{
				flag3 = (null != null);
			}
			else
			{
				Pawn_MindState mindState = seer.mindState;
				flag3 = ((mindState?.enemyTarget) != null);
			}
			if (flag3)
			{
				seer.mindState.enemyTarget = null;
			}
			bool flag4;
			if (seer == null)
			{
				flag4 = (null != null);
			}
			else
			{
				Pawn_MindState mindState2 = seer.mindState;
				flag4 = ((mindState2?.meleeThreat) != null);
			}
			if (!flag4)
			{
				CamoAIUtility.StopCurJobAndWait(seer);
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002524 File Offset: 0x00000724
		public static void CorrectJob(Pawn seer, Pawn target)
		{
			if (seer != null && target != null && (seer?.Map) != null && (target?.Map) != null && seer.Map == target.Map)
			{
				bool flag;
				if (seer == null)
				{
					flag = (null != null);
				}
				else
				{
					Pawn_MindState mindState = seer.mindState;
					flag = ((mindState?.meleeThreat) != null);
				}
				if (!flag && ThingCompUtility.TryGetComp<PawnCamoData>(seer).LastCamoCorrectTick + 120 <= Find.TickManager.TicksGame && seer.Spawned && target.Spawned)
				{
					bool flag2;
					if (seer == null)
					{
						flag2 = (null != null);
					}
					else
					{
						Pawn_JobTracker jobs = seer.jobs;
						flag2 = ((jobs?.curJob) != null);
					}
					if (flag2 && seer.jobs.curJob.def != CamoAIUtility.CamoPauseJobDef() && seer.jobs.curJob.AnyTargetIs(target))
					{
						ThingCompUtility.TryGetComp<PawnCamoData>(seer).LastCamoCorrectTick = Find.TickManager.TicksGame;
						CamoAIUtility.RemoveTarget(seer, target);
					}
				}
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x0000261C File Offset: 0x0000081C
		public static bool CanSeeSimply(Thing seer, Thing target)
		{
			return GenSight.LineOfSight(seer.Position, target.Position, seer.Map, true, null, 0, 0);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002640 File Offset: 0x00000840
		public static bool StillMeleeThreat(Pawn seer, Pawn target)
		{
			bool result = false;
			if (seer != null && target != null && (seer?.Map) != null && (target?.Map) != null && seer.Map == target.Map)
			{
				Pawn pawn;
				if (seer == null)
				{
					pawn = null;
				}
				else
				{
					Pawn_MindState mindState = seer.mindState;
					pawn = (mindState?.meleeThreat);
				}
				Pawn pawn2 = pawn;
				if (pawn2 != null && pawn2 == target && pawn2.Spawned && !pawn2.Downed && seer.Spawned && Find.TickManager.TicksGame <= seer.mindState.lastMeleeThreatHarmTick + 83 && (float)(seer.Position - pawn2.Position).LengthHorizontalSquared <= 7f && GenSight.LineOfSight(seer.Position, pawn2.Position, seer.Map, false, null, 0, 0))
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002721 File Offset: 0x00000921
		public static bool JobIsCastException(JobDef def)
		{
			return def == JobDefOf.Hunt || def == JobDefOf.PredatorHunt;
		}
	}
}
