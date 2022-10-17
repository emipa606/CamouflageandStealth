using HarmonyLib;
using RimWorld;
using Verse;

namespace CompCamo;

[HarmonyPatch(typeof(PawnApparelGenerator), "GenerateStartingApparelFor")]
public class GenerateStartingApparelFor_CamoPostPatch
{
    [HarmonyPostfix]
    [HarmonyPriority(800)]
    public static void PostFix(Pawn pawn, PawnGenerationRequest request)
    {
        CamoGearUtility.CalcAndSetCamoEff(pawn);
    }
}