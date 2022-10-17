using HarmonyLib;
using RimWorld;
using Verse;

namespace CompCamo;

[HarmonyPatch(typeof(ApparelUtility), "CanWearTogether")]
public class CanWearTogether_ACPostPatch
{
    [HarmonyPostfix]
    public static void PostFix(ref bool __result, ThingDef A, ThingDef B, BodyDef body)
    {
        if (__result && CamoGearUtility.GetIsACApparel(A) && CamoGearUtility.GetIsACApparel(B))
        {
            __result = false;
        }
    }
}