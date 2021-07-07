using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace CompCamo
{
    // Token: 0x0200000F RID: 15
    public class PawnCamoData : ThingComp
    {
        // Token: 0x04000025 RID: 37
        public int LastCamoCorrectTick;

        // Token: 0x0400001D RID: 29
        public float PawnArcticCamo;

        // Token: 0x0400001E RID: 30
        public float PawnDesertCamo;

        // Token: 0x04000024 RID: 36
        public List<string> PawnHidTickList = new List<string>();

        // Token: 0x0400001F RID: 31
        public float PawnJungleCamo;

        // Token: 0x04000023 RID: 35
        public float PawnnotDefinedCamo;

        // Token: 0x04000020 RID: 32
        public float PawnStoneCamo;

        // Token: 0x04000022 RID: 34
        public float PawnUrbanCamo;

        // Token: 0x04000021 RID: 33
        public float PawnWoodlandCamo;

        // Token: 0x17000008 RID: 8
        // (get) Token: 0x06000065 RID: 101 RVA: 0x000067B5 File Offset: 0x000049B5
        private Pawn Pawn => (Pawn) parent;

        // Token: 0x06000060 RID: 96 RVA: 0x000062C8 File Offset: 0x000044C8
        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref PawnArcticCamo, "PawnArcticCamo");
            Scribe_Values.Look(ref PawnDesertCamo, "PawnDesertCamo");
            Scribe_Values.Look(ref PawnJungleCamo, "PawnJungleCamo");
            Scribe_Values.Look(ref PawnStoneCamo, "PawnStoneCamo");
            Scribe_Values.Look(ref PawnWoodlandCamo, "PawnWoodlandCamo");
            Scribe_Values.Look(ref PawnUrbanCamo, "PawnUrbanCamo");
            Scribe_Values.Look(ref PawnnotDefinedCamo, "PawnnotDefinedCamo");
            Scribe_Collections.Look(ref PawnHidTickList, "PawnHidTickList", LookMode.Value, Array.Empty<object>());
            Scribe_Values.Look(ref LastCamoCorrectTick, "LastCamoCorrectTick");
        }

        // Token: 0x06000061 RID: 97 RVA: 0x000063A0 File Offset: 0x000045A0
        public override void CompTick()
        {
            base.CompTick();
            if (!Controller.Settings.ShowOverlay || !Pawn.IsColonist || !Pawn.Drafted || !Pawn.IsHashIntervalTick(60))
            {
                return;
            }

            var pawn = Pawn;
            if (pawn?.Map != null && Pawn.Spawned && !Pawn.Map.fogGrid.IsFogged(Pawn.Position))
            {
                CamoDrawTools.DrawCamoOverlay(Pawn);
            }
        }

        // Token: 0x06000062 RID: 98 RVA: 0x00006438 File Offset: 0x00004638
        public static void CorrectActiveApparel(Apparel apparel, Pawn pawn = null)
        {
            var thing = ThingMaker.MakeThing(apparel.def, apparel.Stuff);
            if (apparel.TryGetQuality(out var qualityCategory))
            {
                var compQuality = thing.TryGetComp<CompQuality>();
                compQuality?.SetQuality(qualityCategory, ArtGenerationContext.Colony);
            }

            var compColorable = apparel.TryGetComp<CompColorable>();
            if (compColorable != null)
            {
                var compColorable2 = thing.TryGetComp<CompColorable>();
                compColorable2?.SetColor(compColorable.Color);
            }

            if (pawn != null)
            {
                pawn.apparel.Remove(apparel);
                apparel.Destroy();
                pawn.apparel.Wear(thing as Apparel);
                return;
            }

            if (!apparel.Spawned)
            {
                return;
            }

            var intVec = IntVec3.Zero;
            var map = apparel.Map;
            if (map != null)
            {
                intVec = apparel.Position;
            }

            apparel.Destroy();
            if (intVec != IntVec3.Zero)
            {
                GenSpawn.Spawn(thing, intVec, map);
            }
        }

        // Token: 0x06000063 RID: 99 RVA: 0x0000650C File Offset: 0x0000470C
        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            if (respawningAfterLoad)
            {
                var pawn = Pawn;
                if (pawn?.apparel != null && Pawn.apparel.WornApparelCount > 0)
                {
                    foreach (var apparel in Pawn.apparel.WornApparel)
                    {
                        var compGearCamo = apparel.TryGetComp<CompGearCamo>();
                        if (compGearCamo == null || !(compGearCamo.Props.ActiveCamoEff > 0f) ||
                            !(compGearCamo.Props.CamoEnergyMax > 0f) ||
                            apparel.GetType() == typeof(ActiveCamoApparel) ||
                            apparel.def.thingClass != typeof(ActiveCamoApparel))
                        {
                            continue;
                        }

                        CorrectActiveApparel(apparel, Pawn);
                        break;
                    }
                }
            }

            CamoGearUtility.CalcAndSetCamoEff(Pawn);
        }

        // Token: 0x06000064 RID: 100 RVA: 0x00006618 File Offset: 0x00004818
        public override string CompInspectStringExtra()
        {
            if (CamoUtility.IsCamoActive(Pawn, out var apparel))
            {
                if (apparel == null)
                {
                    return null;
                }

                var activeCamoEff = apparel.TryGetComp<CompGearCamo>().Props.ActiveCamoEff;
                string text = "CompCamo.Active".Translate();
                if (apparel.TryGetComp<CompGearCamo>().Props.StealthCamoChance > 0 && activeCamoEff > 0f)
                {
                    text = "CompCamo.Stealth".Translate();
                }

                return "CompCamo.CamouflageDesc".Translate(text, activeCamoEff.ToStringPercent());
            }

            if (!CamoGearUtility.GetCurCamoEff(Pawn, out var a, out var num))
            {
                return null;
            }

            string text2 = "CompCamo.Undefined".Translate();
            switch (a)
            {
                case "Arctic":
                    text2 = "CompCamo.Arctic".Translate();
                    break;
                case "Desert":
                    text2 = "CompCamo.Desert".Translate();
                    break;
                case "Jungle":
                    text2 = "CompCamo.Jungle".Translate();
                    break;
                case "Stone":
                    text2 = "CompCamo.Stone".Translate();
                    break;
                case "Urban":
                    text2 = "CompCamo.Urban".Translate();
                    break;
                case "Woodland":
                    text2 = "CompCamo.Woodland".Translate();
                    break;
            }

            return "CompCamo.CamouflageDesc".Translate(text2, num.ToStringPercent());
        }

        // Token: 0x02000021 RID: 33
        public class CompProperties_PawnCamoData : CompProperties
        {
            // Token: 0x06000091 RID: 145 RVA: 0x000072F7 File Offset: 0x000054F7
            public CompProperties_PawnCamoData()
            {
                compClass = typeof(PawnCamoData);
            }
        }

        // Token: 0x02000022 RID: 34
        [StaticConstructorOnStartup]
        private static class Camo_Setup
        {
            // Token: 0x06000092 RID: 146 RVA: 0x0000730F File Offset: 0x0000550F
            static Camo_Setup()
            {
                Camo_Setup_Pawns();
            }

            // Token: 0x06000093 RID: 147 RVA: 0x00007316 File Offset: 0x00005516
            private static void Camo_Setup_Pawns()
            {
                CamoSetup_Comp(typeof(CompProperties_PawnCamoData), def => def.race != null);
            }

            // Token: 0x06000094 RID: 148 RVA: 0x00007348 File Offset: 0x00005548
            private static void CamoSetup_Comp(Type compType, Func<ThingDef, bool> qualifier)
            {
                var list = DefDatabase<ThingDef>.AllDefsListForReading.Where(qualifier).ToList();
                list.RemoveDuplicates();
                foreach (var thingDef in list)
                {
                    if (thingDef.comps != null && !thingDef.comps.Any(c => (Type) (object) c.GetType() == compType))
                    {
                        thingDef.comps.Add((CompProperties) Activator.CreateInstance(compType));
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
                            !(((CompProperties_GearCamo) compProperties).ActiveCamoEff > 0f) ||
                            !((compProperties as CompProperties_GearCamo).CamoEnergyMax > 0f) ||
                            thingDef.thingClass != typeof(Apparel))
                        {
                            continue;
                        }

                        thingDef.thingClass = typeof(ActiveCamoApparel);
                        if (thingDef.tickerType != TickerType.Normal)
                        {
                            thingDef.tickerType = TickerType.Normal;
                        }
                    }
                }
            }
        }
    }
}