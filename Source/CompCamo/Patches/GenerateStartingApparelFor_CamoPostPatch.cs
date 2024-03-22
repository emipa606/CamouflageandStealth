using HarmonyLib;
using RimWorld;
using Verse;

namespace CompCamo.Patches;

[HarmonyPatch(typeof(PawnApparelGenerator), "GenerateStartingApparelFor")]
public class GenerateStartingApparelFor_CamoPostPatch
{
    [HarmonyPostfix]
    [HarmonyPriority(800)]
    public static void PostFix(Pawn pawn)
    {
        CamoGearUtility.CalcAndSetCamoEff(pawn);
    }
}