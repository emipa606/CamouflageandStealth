using HarmonyLib;
using Verse;
using Verse.AI;

namespace CompCamo.Patches;

[HarmonyPatch(typeof(AttackTargetFinder), nameof(AttackTargetFinder.BestAttackTarget))]
public class AttackTargetFinder_BestAttackTarget
{
    [HarmonyPriority(0)]
    public static void Postfix(ref IAttackTarget __result, IAttackTargetSearcher searcher)
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