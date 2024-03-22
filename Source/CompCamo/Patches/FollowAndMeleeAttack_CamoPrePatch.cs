using System;
using HarmonyLib;
using Verse;
using Verse.AI;

namespace CompCamo.Patches;

[HarmonyPatch(typeof(Toils_Combat), "FollowAndMeleeAttack", typeof(TargetIndex), typeof(TargetIndex),
    typeof(Action))]
public class FollowAndMeleeAttack_CamoPrePatch
{
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

            if (pawn is { Downed: true } && !curJob.killIncappedTarget)
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