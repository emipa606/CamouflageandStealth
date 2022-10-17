using HarmonyLib;
using Verse;
using Verse.AI;

namespace CompCamo;

[HarmonyPatch(typeof(CastPositionFinder), "TryFindCastPosition")]
public class TryFindCastPosition_CamoPostPatch
{
    [HarmonyPrefix]
    [HarmonyPriority(800)]
    public static bool PreFix(ref bool __result, CastPositionRequest newReq, out IntVec3 dest)
    {
        dest = IntVec3.Invalid;
        var caster = newReq.caster;
        if (caster == null || newReq.target is not Pawn pawn)
        {
            return true;
        }

        var jobs = caster.jobs;

        if (jobs?.curJob != null && CamoAIUtility.JobIsCastException(caster.jobs.curJob.def))
        {
            return true;
        }

        if (!CamoUtility.IsTargetHidden(pawn, caster))
        {
            return true;
        }

        __result = false;
        return false;
    }
}