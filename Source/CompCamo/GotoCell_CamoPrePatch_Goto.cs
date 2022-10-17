using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace CompCamo;

[HarmonyPatch(typeof(Toils_Goto), "GotoCell", typeof(TargetIndex), typeof(PathEndMode))]
public class GotoCell_CamoPrePatch_Goto
{
    [HarmonyPrefix]
    [HarmonyPriority(800)]
    public static bool PreFix(ref Toil __result, TargetIndex ind, PathEndMode peMode)
    {
        var toil = new Toil();
        toil.initAction = delegate
        {
            var actor = toil.actor;
            actor.pather.StartPath(actor.jobs.curJob.GetTarget(ind), peMode);
        };
        toil.AddPreTickAction(delegate
        {
            var actor = toil.actor;
            if (!actor.IsHashIntervalTick(240) || !actor.jobs.curJob.GetTarget(ind).HasThing)
            {
                return;
            }

            var thing = actor.jobs.curJob.GetTarget(ind).Thing;
            if (thing is Pawn pawn && actor.HostileTo(pawn))
            {
                CamoAIUtility.JobFailIfHid(actor, pawn, actor.jobs.curJob);
            }
        });
        toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
        __result = toil;
        return false;
    }
}