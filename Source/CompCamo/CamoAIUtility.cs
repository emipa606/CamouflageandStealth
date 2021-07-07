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
            if (seer == null || target == null || seer.Map == null || target.Map == null || seer.Map != target.Map ||
                seer.IsColonist)
            {
                return;
            }

            if (seer.RaceProps.IsMechanoid)
            {
                return;
            }

            if (!seer.AnimalOrWildMan() || seer.RaceProps.FleshType != FleshTypeDefOf.Insectoid)
            {
                CorrectLordForCamoAction(seer, target, seer.Map.IsPlayerHome);
                return;
            }

            var mindState = seer.mindState;
            if (mindState != null)
            {
                _ = mindState.duty;
            }
        }

        // Token: 0x06000002 RID: 2 RVA: 0x000020F0 File Offset: 0x000002F0
        public static void CorrectLordForCamoAction(Pawn seer, Pawn target, bool playerMap)
        {
            if (seer?.mindState.duty != null && !HasFleeingDuty(seer))
            {
                if (!playerMap)
                {
                    seer.mindState.duty = new PawnDuty(DutyDefOf.DefendBase, GetNearestBaseItem(seer));
                }

                if (!IsMeleeProcess(seer, target))
                {
                    CorrectJob(seer, target);
                }
            }
            else if (!IsMeleeProcess(seer, target))
            {
                CorrectJob(seer, target);
            }
        }

        // Token: 0x06000003 RID: 3 RVA: 0x00002164 File Offset: 0x00000364
        public static bool IsMeleeProcess(Pawn seer, Pawn target)
        {
            if (seer.CurrentEffectiveVerb.IsMeleeAttack)
            {
                var mindState = seer.mindState;

                if (mindState?.enemyTarget != null)
                {
                    return true;
                }
            }

            var mindState2 = seer.mindState;

            if (mindState2?.meleeThreat == null)
            {
                return false;
            }

            return true;
        }

        // Token: 0x06000004 RID: 4 RVA: 0x000021B5 File Offset: 0x000003B5
        public static void StopCurJobAndWait(Pawn pawn)
        {
            ClearAllJobs(pawn);
            GiveWaitJob(pawn, 117);
        }

        // Token: 0x06000005 RID: 5 RVA: 0x000021C5 File Offset: 0x000003C5
        private static JobDef CamoPauseJobDef()
        {
            return JobDefOf.Wait_Wander;
        }

        // Token: 0x06000006 RID: 6 RVA: 0x000021CC File Offset: 0x000003CC
        private static void GiveWaitJob(Pawn pawn, int period)
        {
            pawn.TryGetComp<PawnCamoData>().LastCamoCorrectTick = Find.TickManager.TicksGame;
            var job = new Job(CamoPauseJobDef())
            {
                expiryInterval = period,
                checkOverrideOnExpire = true
            };
            pawn.jobs.jobQueue.EnqueueFirst(job);
        }

        // Token: 0x06000007 RID: 7 RVA: 0x00002224 File Offset: 0x00000424
        private static void ClearAllJobs(Pawn pawn)
        {
            if (pawn == null || !pawn.Spawned || pawn.Map == null)
            {
                return;
            }

            var jobs = pawn.jobs;

            if (jobs?.jobQueue != null)
            {
                pawn.jobs.ClearQueuedJobs();
            }

            var jobs2 = pawn.jobs;

            if (jobs2?.curJob != null)
            {
                pawn.jobs.EndCurrentJob(JobCondition.Succeeded, false);
            }
        }

        // Token: 0x06000008 RID: 8 RVA: 0x00002298 File Offset: 0x00000498
        private static IntVec3 GetNearestBaseItem(Pawn pawn)
        {
            var position = pawn.Position;
            var num = 99999f;
            var list = pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial);
            if (list.Count <= 0)
            {
                return position;
            }

            Thing thing = null;
            foreach (var thing2 in list)
            {
                LocalTargetInfo localTargetInfo = thing2;
                if (!pawn.CanReach(localTargetInfo, PathEndMode.Touch, Danger.Deadly))
                {
                    continue;
                }

                var num2 = pawn.Position.DistanceTo(thing2.Position);
                if (!(num2 < num))
                {
                    continue;
                }

                thing = thing2;
                num = num2;
            }

            if (thing != null)
            {
                position = thing.Position;
            }

            return position;
        }

        // Token: 0x06000009 RID: 9 RVA: 0x00002350 File Offset: 0x00000550
        private static bool HasFleeingDuty(Pawn pawn)
        {
            return pawn.mindState.duty.def == DutyDefOf.ExitMapRandom || pawn.mindState.duty.def == DutyDefOf.Steal ||
                   pawn.mindState.duty.def == DutyDefOf.Kidnap;
        }

        // Token: 0x0600000A RID: 10 RVA: 0x000023A8 File Offset: 0x000005A8
        public static void JobFailIfHid(Pawn seer, Pawn target, Job cur)
        {
            if (seer == null || target == null || cur == null || cur.def == CamoPauseJobDef() || !seer.Spawned ||
                !target.Spawned || seer.Map == null || target.Map == null || seer.Map != target.Map)
            {
                return;
            }

            var mindState = seer.mindState;

            if (mindState?.meleeThreat != null)
            {
                return;
            }

            if (CanSeeSimply(seer, target))
            {
                if (CamoUtility.IsTargetHidden(target, seer))
                {
                    CorrectLordForCamo(seer, target);
                }
            }
            else
            {
                CorrectLordForCamo(seer, target);
            }
        }

        // Token: 0x0600000B RID: 11 RVA: 0x0000244C File Offset: 0x0000064C
        public static void RemoveTarget(Pawn seer, Pawn target)
        {
            if (seer.pather.Moving)
            {
                var jobs = seer.jobs;

                if (jobs?.curJob != null && seer.jobs.curJob.AnyTargetIs(target))
                {
                    seer.jobs.curDriver.Notify_PatherFailed();
                    var pather = seer.pather;

                    if (pather?.curPath != null)
                    {
                        seer.pather.ResetToCurrentPosition();
                        seer.pather.curPath.ReleaseToPool();
                    }
                }
            }

            var mindState = seer.mindState;

            if (mindState?.enemyTarget != null)
            {
                seer.mindState.enemyTarget = null;
            }

            var mindState2 = seer.mindState;

            if (mindState2?.meleeThreat == null)
            {
                StopCurJobAndWait(seer);
            }
        }

        // Token: 0x0600000C RID: 12 RVA: 0x00002524 File Offset: 0x00000724
        public static void CorrectJob(Pawn seer, Pawn target)
        {
            if (seer == null || target == null || seer.Map == null || target.Map == null || seer.Map != target.Map)
            {
                return;
            }

            var mindState = seer.mindState;

            if (mindState?.meleeThreat != null ||
                seer.TryGetComp<PawnCamoData>().LastCamoCorrectTick + 120 > Find.TickManager.TicksGame ||
                !seer.Spawned || !target.Spawned)
            {
                return;
            }

            var jobs = seer.jobs;

            if (jobs?.curJob == null || seer.jobs.curJob.def == CamoPauseJobDef() ||
                !seer.jobs.curJob.AnyTargetIs(target))
            {
                return;
            }

            seer.TryGetComp<PawnCamoData>().LastCamoCorrectTick = Find.TickManager.TicksGame;
            RemoveTarget(seer, target);
        }

        // Token: 0x0600000D RID: 13 RVA: 0x0000261C File Offset: 0x0000081C
        public static bool CanSeeSimply(Thing seer, Thing target)
        {
            return GenSight.LineOfSight(seer.Position, target.Position, seer.Map, true);
        }

        // Token: 0x0600000E RID: 14 RVA: 0x00002640 File Offset: 0x00000840
        public static bool StillMeleeThreat(Pawn seer, Pawn target)
        {
            var result = false;
            if (seer == null || target == null || seer.Map == null || target.Map == null || seer.Map != target.Map)
            {
                return false;
            }

            var mindState = seer.mindState;
            var pawn = mindState?.meleeThreat;

            var pawn2 = pawn;
            if (pawn2 != null && pawn2 == target && pawn2.Spawned && !pawn2.Downed && seer.Spawned &&
                Find.TickManager.TicksGame <= seer.mindState.lastMeleeThreatHarmTick + 83 &&
                (seer.Position - pawn2.Position).LengthHorizontalSquared <= 7f &&
                GenSight.LineOfSight(seer.Position, pawn2.Position, seer.Map))
            {
                result = true;
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