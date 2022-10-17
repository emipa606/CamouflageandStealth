using HarmonyLib;
using RimWorld;

namespace CompCamo;

[HarmonyPatch(typeof(Pawn_ApparelTracker), "Notify_ApparelChanged")]
public class ApparelChanged_CamoPostPatch
{
    [HarmonyPostfix]
    [HarmonyPriority(800)]
    public static void PostFix(ref Pawn_ApparelTracker __instance)
    {
        CamoGearUtility.CalcAndSetCamoEff(__instance.pawn);
    }
}