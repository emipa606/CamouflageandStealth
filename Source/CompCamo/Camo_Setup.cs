using System;
using System.Linq;
using JetBrains.Annotations;
using RimWorld;
using Verse;

namespace CompCamo;

[StaticConstructorOnStartup]
[UsedImplicitly]
public static class Camo_Setup
{
    static Camo_Setup()
    {
        Camo_Setup_Pawns();
    }

    private static void Camo_Setup_Pawns()
    {
        CamoSetup_Comp(typeof(CompProperties_PawnCamoData), def => def.race != null && !def.IsCorpse);
    }

    private static void CamoSetup_Comp(Type compType, Func<ThingDef, bool> qualifier)
    {
        var list = DefDatabase<ThingDef>.AllDefsListForReading.Where(qualifier).ToList();
        list.RemoveDuplicates();
        foreach (var thingDef in list)
        {
            if (thingDef.comps != null && !thingDef.comps.Any(c => c.GetType() == compType))
            {
                thingDef.comps.Add((CompProperties)Activator.CreateInstance(compType));
            }

            if (!thingDef.IsApparel)
            {
                continue;
            }

            if (!thingDef.comps.Any(c => c.GetType() == typeof(CompProperties_GearCamo)))
            {
                continue;
            }

            if (thingDef.comps == null)
            {
                continue;
            }

            foreach (var compProperties in thingDef.comps)
            {
                if (compProperties.GetType() != typeof(CompProperties_GearCamo) ||
                    !(((CompProperties_GearCamo)compProperties).ActiveCamoEff > 0f) ||
                    !((compProperties as CompProperties_GearCamo).CamoEnergyMax > 0f) ||
                    thingDef.thingClass != typeof(Apparel))
                {
                    continue;
                }

                thingDef.thingClass = typeof(ActiveCamoApparel);
                thingDef.tickerType = TickerType.Normal;
            }
        }
    }
}