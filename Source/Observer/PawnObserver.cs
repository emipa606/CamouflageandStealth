using System;
using System.Linq;
using Verse;

namespace Observer
{
    // Token: 0x02000006 RID: 6
    public class PawnObserver : ThingComp
    {
        // Token: 0x04000002 RID: 2
        public float PawnSightOffset;

        // Token: 0x17000002 RID: 2
        // (get) Token: 0x06000007 RID: 7 RVA: 0x000020D5 File Offset: 0x000002D5
        private Pawn Pawn => (Pawn) parent;

        // Token: 0x06000008 RID: 8 RVA: 0x000020E2 File Offset: 0x000002E2
        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref PawnSightOffset, "PawnSightOffset");
        }

        // Token: 0x06000009 RID: 9 RVA: 0x00002100 File Offset: 0x00000300
        public override void CompTick()
        {
            base.CompTick();
            if (Pawn.HashOffsetTicks() == 180)
            {
                CalcAndSetObserver(Pawn);
            }
        }

        // Token: 0x0600000A RID: 10 RVA: 0x00002126 File Offset: 0x00000326

        // Token: 0x0600000B RID: 11 RVA: 0x0000212E File Offset: 0x0000032E
        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            CalcAndSetObserver(Pawn);
        }

        // Token: 0x0600000C RID: 12 RVA: 0x00002144 File Offset: 0x00000344
        public void CalcAndSetObserver(Pawn pawn)
        {
            var num = 0f;
            var num2 = 0f;
            var num3 = 0f;
            var num4 = 0.25f;
            if (pawn != null)
            {
                if (pawn.apparel != null && pawn.apparel.WornApparelCount > 0)
                {
                    var wornApparel = pawn.apparel.WornApparel;
                    if (wornApparel != null && wornApparel.Count > 0)
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
                    if (allEquipmentListForReading != null && allEquipmentListForReading.Count > 0)
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

        // Token: 0x02000007 RID: 7
        public class CompProperties_PawnObserver : CompProperties
        {
            // Token: 0x0600000E RID: 14 RVA: 0x0000246C File Offset: 0x0000066C
            public CompProperties_PawnObserver()
            {
                compClass = typeof(PawnObserver);
            }
        }

        // Token: 0x02000008 RID: 8
        [StaticConstructorOnStartup]
        private static class Observer_Setup
        {
            // Token: 0x0600000F RID: 15 RVA: 0x00002484 File Offset: 0x00000684
            static Observer_Setup()
            {
                Observer_Setup_Pawns();
            }

            // Token: 0x06000010 RID: 16 RVA: 0x0000248B File Offset: 0x0000068B
            private static void Observer_Setup_Pawns()
            {
                ObserverSetup_Comp(typeof(CompProperties_PawnObserver), def => def.race != null);
            }

            // Token: 0x06000011 RID: 17 RVA: 0x000024BC File Offset: 0x000006BC
            private static void ObserverSetup_Comp(Type compType, Func<ThingDef, bool> qualifier)
            {
                var list = DefDatabase<ThingDef>.AllDefsListForReading.Where(qualifier).ToList();
                list.RemoveDuplicates();
                foreach (var item in list)
                {
                    if (item.comps != null && !item.comps.Any(c => (Type) (object) c.GetType() == compType))
                    {
                        item.comps.Add((CompProperties) Activator.CreateInstance(compType));
                    }
                }
            }
        }
    }
}