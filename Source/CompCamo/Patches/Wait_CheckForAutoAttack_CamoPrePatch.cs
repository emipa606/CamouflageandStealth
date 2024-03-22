using HarmonyLib;
using Verse;
using Verse.AI;

namespace CompCamo.Patches;

[HarmonyPatch(typeof(JobDriver_Wait), "CheckForAutoAttack")]
public class Wait_CheckForAutoAttack_CamoPrePatch
{
    [HarmonyPrefix]
    [HarmonyPriority(800)]
    public static bool PreFix(ref JobDriver_Wait __instance)
    {
        return __instance.pawn == null || __instance.pawn.TryGetComp<PawnCamoData>().LastCamoCorrectTick + 120 <=
            Find.TickManager.TicksGame;
    }
}