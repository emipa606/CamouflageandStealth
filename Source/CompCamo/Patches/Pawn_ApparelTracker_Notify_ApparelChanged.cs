using HarmonyLib;
using RimWorld;

namespace CompCamo;

[HarmonyPatch(typeof(Pawn_ApparelTracker), nameof(Pawn_ApparelTracker.Notify_ApparelChanged))]
public class Pawn_ApparelTracker_Notify_ApparelChanged
{
    [HarmonyPriority(800)]
    public static void Postfix(ref Pawn_ApparelTracker __instance)
    {
        CamoGearUtility.CalcAndSetCamoEff(__instance.pawn);
    }
}