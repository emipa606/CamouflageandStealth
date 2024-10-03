using HarmonyLib;
using RimWorld;
using Verse;

namespace CompCamo.Patches;

[HarmonyPatch(typeof(PawnApparelGenerator), nameof(PawnApparelGenerator.GenerateStartingApparelFor))]
public class PawnApparelGenerator_GenerateStartingApparelFor
{
    [HarmonyPriority(800)]
    public static void Postfix(Pawn pawn)
    {
        CamoGearUtility.CalcAndSetCamoEff(pawn);
    }
}