using RimWorld;
using Verse;
using Verse.AI;

namespace CompCamo;

public class CamoAIUtility
{
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
            correctLordForCamoAction(seer, target, seer.Map.IsPlayerHome);
            return;
        }

        var mindState = seer.mindState;
        if (mindState != null)
        {
            _ = mindState.duty;
        }
    }

    private static void correctLordForCamoAction(Pawn seer, Pawn target, bool playerMap)
    {
        if (seer?.mindState.duty != null && !hasFleeingDuty(seer))
        {
            if (!playerMap)
            {
                seer.mindState.duty = new PawnDuty(DutyDefOf.DefendBase, GetNearestBaseItem(seer));
            }

            if (!isMeleeProcess(seer))
            {
                CorrectJob(seer, target);
            }
        }
        else if (!isMeleeProcess(seer))
        {
            CorrectJob(seer, target);
        }
    }

    private static bool isMeleeProcess(Pawn seer)
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

        return mindState2?.meleeThreat != null;
    }

    private static void stopCurJobAndWait(Pawn pawn)
    {
        ClearAllJobs(pawn);
        GiveWaitJob(pawn, 117);
    }

    private static JobDef CamoPauseJobDef()
    {
        return JobDefOf.Wait_Wander;
    }

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

    private static void ClearAllJobs(Pawn pawn)
    {
        if (pawn is not { Spawned: true } || pawn.Map == null)
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

    private static bool hasFleeingDuty(Pawn pawn)
    {
        return pawn.mindState.duty.def == DutyDefOf.ExitMapRandom || pawn.mindState.duty.def == DutyDefOf.Steal ||
               pawn.mindState.duty.def == DutyDefOf.Kidnap;
    }

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

        if (canSeeSimply(seer, target))
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

    private static void removeTarget(Pawn seer, Pawn target)
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
            stopCurJobAndWait(seer);
        }
    }

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
        removeTarget(seer, target);
    }

    private static bool canSeeSimply(Thing seer, Thing target)
    {
        return GenSight.LineOfSight(seer.Position, target.Position, seer.Map, true);
    }

    public static bool JobIsCastException(JobDef def)
    {
        return def == JobDefOf.Hunt || def == JobDefOf.PredatorHunt;
    }
}