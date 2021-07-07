using System;
using HarmonyLib;
using Verse;
using Verse.AI;

namespace CompCamo
{
    // Token: 0x02000015 RID: 21
    [HarmonyPatch(typeof(Toils_Combat), "FollowAndMeleeAttack", typeof(TargetIndex), typeof(TargetIndex),
        typeof(Action))]
    public class FollowAndMeleeAttack_CamoPrePatch
    {
        // Token: 0x06000071 RID: 113 RVA: 0x0000694C File Offset: 0x00004B4C
        [HarmonyPrefix]
        [HarmonyPriority(800)]
        public static bool PreFix(ref Toil __result, TargetIndex targetInd, Action hitAction)
        {
            var followAndAttack = new Toil();
            followAndAttack.tickAction = delegate
            {
                var actor = followAndAttack.actor;
                var curJob = actor.jobs.curJob;
                var curDriver = actor.jobs.curDriver;
                var thing = curJob.GetTarget(targetInd).Thing;
                var pawn = thing as Pawn;
                if (thing.Spawned && !actor.CanReachImmediate(pawn, PathEndMode.Touch))
                {
                    bool b;
                    if (actor == null)
                    {
                        b = false;
                    }
                    else
                    {
                        var mindState = actor.mindState;
                        b = mindState?.meleeThreat != null;
                    }

                    if (!b && pawn != null && CamoUtility.IsTargetHidden(pawn, actor))
                    {
                        CamoAIUtility.CorrectJob(actor, pawn);
                    }
                }

                if (!thing.Spawned)
                {
                    curDriver.ReadyForNextToil();
                    return;
                }

                if (thing != actor.pather.Destination.Thing ||
                    !actor.pather.Moving && !actor.CanReachImmediate(thing, PathEndMode.Touch))
                {
                    actor.pather.StartPath(thing, PathEndMode.Touch);
                    return;
                }

                if (!actor.CanReachImmediate(thing, PathEndMode.Touch))
                {
                    return;
                }

                if (pawn != null && pawn.Downed && !curJob.killIncappedTarget)
                {
                    curDriver.ReadyForNextToil();
                    return;
                }

                hitAction();
            };
            followAndAttack.defaultCompleteMode = ToilCompleteMode.Never;
            __result = followAndAttack;
            return false;
        }
    }
}