using HarmonyLib;
using RimWorld;
using Verse;

namespace StealthBox;

[HarmonyPatch(typeof(ApparelUtility), "CanWearTogether")]
public class CanWearTogether_PostPatch
{
    [HarmonyPostfix]
    public static void PostFix(ref bool __result, ThingDef A, ThingDef B, BodyDef body)
    {
        if (!__result)
        {
            return;
        }

        var b = false;
        var b1 = false;
        if (A.thingClass.FullName == "StealthBox.CardboardBox")
        {
            b = true;
        }

        if (B.thingClass.FullName == "StealthBox.CardboardBox")
        {
            b1 = true;
        }

        if (b && b1)
        {
            __result = false;
        }
    }
}