using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;

namespace Observer
{
    // Token: 0x02000002 RID: 2
    [HarmonyPatch(typeof(PawnCapacityWorker_Sight), "CalculateCapacityLevel")]
    public class CalculateCapacityLevel_PostPatch
    {
        // Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
        [HarmonyPostfix]
        [HarmonyPriority(800)]
        public static void PostFix(ref PawnCapacityWorker_Sight __instance, ref float __result, HediffSet diffSet,
            List<PawnCapacityUtility.CapacityImpactor> impactors = null)
        {
            if (!(__result > 0f))
            {
                return;
            }

            var pawnObserver = diffSet.pawn.TryGetComp<PawnObserver>();
            if (pawnObserver == null)
            {
                return;
            }

            var pawnSightOffset = pawnObserver.PawnSightOffset;
            if (pawnSightOffset != 0f)
            {
                __result += pawnSightOffset;
            }
        }
    }
}