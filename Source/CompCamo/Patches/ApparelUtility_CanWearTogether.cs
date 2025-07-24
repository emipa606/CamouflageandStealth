using HarmonyLib;
using RimWorld;
using Verse;

namespace CompCamo.Patches;

[HarmonyPatch(typeof(ApparelUtility), nameof(ApparelUtility.CanWearTogether))]
public class ApparelUtility_CanWearTogether
{
    public static void Postfix(ref bool __result, ThingDef A, ThingDef B)
    {
        if (!__result)
        {
            return;
        }

        if (A.thingClass.FullName == "StealthBox.CardboardBox" && B.thingClass.FullName == "StealthBox.CardboardBox")
        {
            __result = false;
            return;
        }

        if (!CamoGearUtility.GetIsAcApparel(A) || !CamoGearUtility.GetIsAcApparel(B))
        {
            return;
        }

        __result = false;
    }
}