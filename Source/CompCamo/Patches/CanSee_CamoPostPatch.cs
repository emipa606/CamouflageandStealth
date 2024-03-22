using HarmonyLib;
using Verse;
using Verse.AI;

namespace CompCamo.Patches;

[HarmonyPatch(typeof(AttackTargetFinder), "CanSee")]
public class CanSee_CamoPostPatch
{
    [HarmonyPostfix]
    [HarmonyPriority(800)]
    public static void PostFix(ref bool __result, Thing seer, Thing target)
    {
        if (__result && target != null && seer != null && target.Spawned && seer.Spawned && target.Map != null &&
            seer.Map != null && target.Map == seer.Map && target is Pawn pawn && seer is Pawn seer1 &&
            CamoUtility.IsTargetHidden(pawn, seer1))
        {
            __result = false;
        }
    }
}