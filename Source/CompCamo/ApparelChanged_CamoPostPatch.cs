using HarmonyLib;
using RimWorld;

namespace CompCamo
{
    // Token: 0x02000010 RID: 16
    [HarmonyPatch(typeof(Pawn_ApparelTracker), "Notify_ApparelChanged")]
    public class ApparelChanged_CamoPostPatch
    {
        // Token: 0x06000067 RID: 103 RVA: 0x000067D5 File Offset: 0x000049D5
        [HarmonyPostfix]
        [HarmonyPriority(800)]
        public static void PostFix(ref Pawn_ApparelTracker __instance)
        {
            CamoGearUtility.CalcAndSetCamoEff(__instance.pawn);
        }
    }
}