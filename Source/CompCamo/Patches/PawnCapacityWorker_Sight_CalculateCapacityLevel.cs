using HarmonyLib;
using RimWorld;
using Verse;
using PawnObserver = Observer.PawnObserver;

namespace CompCamo.Patches;

[HarmonyPatch(typeof(PawnCapacityWorker_Sight), nameof(PawnCapacityWorker_Sight.CalculateCapacityLevel))]
public class PawnCapacityWorker_Sight_CalculateCapacityLevel
{
    [HarmonyPriority(800)]
    public static void Postfix(ref float __result, HediffSet diffSet)
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