using HarmonyLib;
using Verse;

namespace CompCamo.Patches;

[HarmonyPatch(typeof(Verb), nameof(Verb.CanHitTargetFrom))]
public class Verb_CanHitTargetFrom
{
    [HarmonyPriority(800)]
    public static void Postfix(ref Verb __instance, ref bool __result, LocalTargetInfo targ)
    {
        if (!__result || !targ.HasThing)
        {
            return;
        }

        var thing = targ.Thing;
        var caster = __instance.caster;
        if (thing is Pawn pawn && caster is Pawn pawn1 &&
            (pawn1.IsColonist && Controller.Settings.AllowNpcCamo || !pawn1.IsColonist) &&
            CamoUtility.IsTargetHidden(pawn, pawn1))
        {
            __result = false;
        }
    }
}