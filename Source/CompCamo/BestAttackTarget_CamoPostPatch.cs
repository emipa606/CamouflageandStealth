using HarmonyLib;
using Verse;
using Verse.AI;

namespace CompCamo
{
    // Token: 0x02000011 RID: 17
    [HarmonyPatch(typeof(AttackTargetFinder), "BestAttackTarget")]
    public class BestAttackTarget_CamoPostPatch
    {
        // Token: 0x06000069 RID: 105 RVA: 0x000067EC File Offset: 0x000049EC
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
}