using HarmonyLib;
using Verse;
using Verse.AI;

namespace CompCamo.Patches;

[HarmonyPatch(typeof(AttackTargetFinder), "BestAttackTarget")]
public class BestAttackTarget_CamoPostPatch
{
    [HarmonyPostfix]
    [HarmonyPriority(0)]
    public static void PostFix(ref IAttackTarget __result, IAttackTargetSearcher searcher)
    {
        if (__result is not Pawn pawn)
        {
            return;
        }

        if (searcher is Pawn pawn2 && CamoUtility.IsTargetHidden(pawn, pawn2))
        {
            __result = null;
        }
    }
}