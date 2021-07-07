using HarmonyLib;
using Verse;
using Verse.AI;

namespace CompCamo
{
    // Token: 0x0200001E RID: 30
    [HarmonyPatch(typeof(JobDriver_Wait), "CheckForAutoAttack")]
    public class Wait_CheckForAutoAttack_CamoPrePatch
    {
        // Token: 0x06000086 RID: 134 RVA: 0x000070F0 File Offset: 0x000052F0
        [HarmonyPrefix]
        [HarmonyPriority(800)]
        public static bool PreFix(ref JobDriver_Wait __instance)
        {
            return __instance.pawn == null || __instance.pawn.TryGetComp<PawnCamoData>().LastCamoCorrectTick + 120 <=
                Find.TickManager.TicksGame;
        }
    }
}