using System;
using HarmonyLib;
using Verse;
using Verse.AI;

namespace CompCamo
{
    // Token: 0x02000013 RID: 19
    [HarmonyPatch(typeof(AttackTargetFinder), "CanSee")]
    public class CanSee_CamoPostPatch
    {
        // Token: 0x0600006D RID: 109 RVA: 0x000068AC File Offset: 0x00004AAC
        [HarmonyPostfix]
        [HarmonyPriority(800)]
        public static void PostFix(ref bool __result, Thing seer, Thing target, Func<IntVec3, bool> validator = null)
        {
            if (__result && target != null && seer != null && target.Spawned && seer.Spawned && target.Map != null &&
                seer.Map != null && target.Map == seer.Map && target is Pawn pawn && seer is Pawn seer1 &&
                CamoUtility.IsTargetHidden(pawn, seer1))
            {
                __result = false;
            }
        }
    }
}