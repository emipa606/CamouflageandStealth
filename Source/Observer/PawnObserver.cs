using System;
using System.Linq;
using Verse;

namespace Observer;

public class PawnObserver : ThingComp
{
    public float PawnSightOffset;

    private Pawn Pawn => (Pawn)parent;

    public override void PostExposeData()
    {
        base.PostExposeData();
        Scribe_Values.Look(ref PawnSightOffset, "PawnSightOffset");
    }

    public override void CompTick()
    {
        base.CompTick();
        if (Pawn.HashOffsetTicks() == 180)
        {
            CalcAndSetObserver(Pawn);
        }
    }


    public override void PostSpawnSetup(bool respawningAfterLoad)
    {
        base.PostSpawnSetup(respawningAfterLoad);
        CalcAndSetObserver(Pawn);
    }

    public void CalcAndSetObserver(Pawn pawn)
    {
        var num = 0f;
        var num2 = 0f;
        var num3 = 0f;
        var num4 = 0.25f;
        if (pawn != null)
        {
            if (pawn.apparel is { WornApparelCount: > 0 })
            {
                var wornApparel = pawn.apparel.WornApparel;
                if (wornApparel is { Count: > 0 })
                {
                    foreach (var apparel in wornApparel)
                    {
                        var compObserver = apparel.TryGetComp<CompObserver>();
                        if (compObserver == null)
                        {
                            continue;
                        }

                        num += compObserver.Props.SightOffset;
                        if (compObserver.Props.SightOffset > num2)
                        {
                            num2 = compObserver.Props.SightOffset;
                        }

                        if (compObserver.Props.SightOffset < num3)
                        {
                            num3 = compObserver.Props.SightOffset;
                        }
                    }
                }
            }

            if (pawn.equipment != null && pawn.equipment.HasAnything())
            {
                var allEquipmentListForReading = pawn.equipment.AllEquipmentListForReading;
                if (allEquipmentListForReading is { Count: > 0 })
                {
                    foreach (var thingWithComps in allEquipmentListForReading)
                    {
                        var compObserver2 = thingWithComps.TryGetComp<CompObserver>();
                        if (compObserver2 == null)
                        {
                            continue;
                        }

                        num += compObserver2.Props.SightOffset;
                        if (compObserver2.Props.SightOffset > num2)
                        {
                            num2 = compObserver2.Props.SightOffset;
                        }

                        if (compObserver2.Props.SightOffset < num3)
                        {
                            num3 = compObserver2.Props.SightOffset;
                        }
                    }
                }
            }
        }

        if (num != 0f)
        {
            num2 = Math.Min(num4, Math.Max(0f - num4, num2));
            num3 = Math.Min(num4, Math.Max(0f - num4, num3));
        }

        var pawnObserver = pawn.TryGetComp<PawnObserver>();
        if (pawnObserver == null)
        {
            return;
        }

        pawnObserver.PawnSightOffset = num2 + num3;
    }

    public class CompProperties_PawnObserver : CompProperties
    {
        public CompProperties_PawnObserver()
        {
            compClass = typeof(PawnObserver);
        }
    }

    [StaticConstructorOnStartup]
    private static class Observer_Setup
    {
        static Observer_Setup()
        {
            Observer_Setup_Pawns();
        }

        private static void Observer_Setup_Pawns()
        {
            ObserverSetup_Comp(typeof(CompProperties_PawnObserver), def => def.race != null);
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
}