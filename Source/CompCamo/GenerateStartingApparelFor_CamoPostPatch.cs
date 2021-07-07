using HarmonyLib;
using RimWorld;
using Verse;

namespace CompCamo
{
    // Token: 0x02000016 RID: 22
    [HarmonyPatch(typeof(PawnApparelGenerator), "GenerateStartingApparelFor")]
    public class GenerateStartingApparelFor_CamoPostPatch
    {
        // Token: 0x06000073 RID: 115 RVA: 0x000069AC File Offset: 0x00004BAC
        [HarmonyPostfix]
        [HarmonyPriority(800)]
        public static void PostFix(Pawn pawn, PawnGenerationRequest request)
        {
            CamoGearUtility.CalcAndSetCamoEff(pawn);
        }
    }
}