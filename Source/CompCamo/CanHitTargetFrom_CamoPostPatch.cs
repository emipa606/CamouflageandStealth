using HarmonyLib;
using Verse;

namespace CompCamo;

[HarmonyPatch(typeof(Verb), "CanHitTargetFrom")]
public class CanHitTargetFrom_CamoPostPatch
{
    [HarmonyPostfix]
    [HarmonyPriority(800)]
    public static void PostFix(ref Verb __instance, ref bool __result, IntVec3 root, LocalTargetInfo targ)
    {
        if (!__result || !targ.HasThing)
        {
            return;
        }

        var thing = targ.Thing;
        var caster = __instance.caster;
        if (thing is Pawn pawn && caster is Pawn pawn1 &&
            (pawn1.IsColonist && Controller.Settings.AllowNPCCamo || !pawn1.IsColonist) &&
            CamoUtility.IsTargetHidden(pawn, pawn1))
        {
            __result = false;
        }
    }
}