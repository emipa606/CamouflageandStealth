using System;
using System.Linq;
using JetBrains.Annotations;
using Verse;

namespace Observer;

[StaticConstructorOnStartup]
[UsedImplicitly]
public static class Observer_Setup
{
    static Observer_Setup()
    {
        Observer_Setup_Pawns();
    }

    private static void Observer_Setup_Pawns()
    {
        ObserverSetup_Comp(typeof(CompProperties_PawnObserver), def => def.race != null && !def.IsCorpse);
    }

    private static void ObserverSetup_Comp(Type compType, Func<ThingDef, bool> qualifier)
    {
        var list = DefDatabase<ThingDef>.AllDefsListForReading.Where(qualifier).ToList();
        list.RemoveDuplicates();
        foreach (var item in list)
        {
            if (item.comps != null && !item.comps.Any(c => c.GetType() == compType))
            {
                item.comps.Add((CompProperties)Activator.CreateInstance(compType));
            }
        }
    }
}