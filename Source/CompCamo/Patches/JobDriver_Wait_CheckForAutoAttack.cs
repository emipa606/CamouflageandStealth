using HarmonyLib;
using Verse;
using Verse.AI;

namespace CompCamo.Patches;

[HarmonyPatch(typeof(JobDriver_Wait), "CheckForAutoAttack")]
public class JobDriver_Wait_CheckForAutoAttack
{
    [HarmonyPriority(800)]
    public static bool Prefix(ref JobDriver_Wait __instance)
    {
        return __instance.pawn == null || __instance.pawn.TryGetComp<PawnCamoData>().LastCamoCorrectTick + 120 <=
            Find.TickManager.TicksGame;
    }
}