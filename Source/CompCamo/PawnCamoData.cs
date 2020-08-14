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
        // Token: 0x06000060 RID: 96 RVA: 0x000062C8 File Offset: 0x000044C8
        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look<float>(ref this.PawnArcticCamo, "PawnArcticCamo", 0f, false);
            Scribe_Values.Look<float>(ref this.PawnDesertCamo, "PawnDesertCamo", 0f, false);
            Scribe_Values.Look<float>(ref this.PawnJungleCamo, "PawnJungleCamo", 0f, false);
            Scribe_Values.Look<float>(ref this.PawnStoneCamo, "PawnStoneCamo", 0f, false);
            Scribe_Values.Look<float>(ref this.PawnWoodlandCamo, "PawnWoodlandCamo", 0f, false);
            Scribe_Values.Look<float>(ref this.PawnUrbanCamo, "PawnUrbanCamo", 0f, false);
            Scribe_Values.Look<float>(ref this.PawnnotDefinedCamo, "PawnnotDefinedCamo", 0f, false);
            Scribe_Collections.Look<string>(ref this.PawnHidTickList, "PawnHidTickList", LookMode.Value, Array.Empty<object>());
            Scribe_Values.Look<int>(ref this.LastCamoCorrectTick, "LastCamoCorrectTick", 0, false);
        }

        // Token: 0x06000061 RID: 97 RVA: 0x000063A0 File Offset: 0x000045A0
        public override void CompTick()
        {
            base.CompTick();
            if (Controller.Settings.ShowOverlay && this.Pawn.IsColonist && this.Pawn.Drafted && Gen.IsHashIntervalTick(this.Pawn, 60))
            {
                Pawn pawn = this.Pawn;
                if ((pawn?.Map) != null && this.Pawn.Spawned && !this.Pawn.Map.fogGrid.IsFogged(this.Pawn.Position))
                {
                    CamoDrawTools.DrawCamoOverlay(this.Pawn);
                }
            }
        }

        // Token: 0x06000062 RID: 98 RVA: 0x00006438 File Offset: 0x00004638
        public static void CorrectActiveApparel(Apparel apparel, Pawn pawn = null)
        {
            Thing thing = ThingMaker.MakeThing(apparel.def, apparel.Stuff);
            if (QualityUtility.TryGetQuality(apparel, out QualityCategory qualityCategory))
            {
                CompQuality compQuality = ThingCompUtility.TryGetComp<CompQuality>(thing);
                if (compQuality != null)
                {
                    compQuality.SetQuality(qualityCategory, ArtGenerationContext.Colony);
                }
            }
            CompColorable compColorable = ThingCompUtility.TryGetComp<CompColorable>(apparel);
            if (compColorable != null)
            {
                CompColorable compColorable2 = ThingCompUtility.TryGetComp<CompColorable>(thing);
                if (compColorable2 != null)
                {
                    compColorable2.Color = compColorable.Color;
                }
            }
            if (pawn != null)
            {
                pawn.apparel.Remove(apparel);
                apparel.Destroy(0);
                pawn.apparel.Wear(thing as Apparel, true, false);
                return;
            }
            if (apparel.Spawned)
            {
                IntVec3 intVec = IntVec3.Zero;
                Map map = apparel?.Map;
                if (map != null)
                {
                    intVec = apparel.Position;
                }
                apparel.Destroy(0);
                if (intVec != IntVec3.Zero)
                {
                    GenSpawn.Spawn(thing, intVec, map, 0);
                }
            }
        }

        // Token: 0x06000063 RID: 99 RVA: 0x0000650C File Offset: 0x0000470C
        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            if (respawningAfterLoad)
            {
                Pawn pawn = this.Pawn;
                if ((pawn?.apparel) != null && this.Pawn.apparel.WornApparelCount > 0)
                {
                    foreach (Apparel apparel in this.Pawn.apparel.WornApparel)
                    {
                        CompGearCamo compGearCamo = ThingCompUtility.TryGetComp<CompGearCamo>(apparel);
                        if (compGearCamo != null && compGearCamo.Props.ActiveCamoEff > 0f && compGearCamo.Props.CamoEnergyMax > 0f && apparel.GetType() != typeof(ActiveCamoApparel) && apparel.def.thingClass == typeof(ActiveCamoApparel))
                        {
                            PawnCamoData.CorrectActiveApparel(apparel, this.Pawn);
                            break;
                        }
                    }
                }
            }
            CamoGearUtility.CalcAndSetCamoEff(this.Pawn);
        }

        // Token: 0x06000064 RID: 100 RVA: 0x00006618 File Offset: 0x00004818
        public override string CompInspectStringExtra()
        {
            if (CamoUtility.IsCamoActive(this.Pawn, out Apparel apparel))
            {
                if (apparel != null)
                {
                    float activeCamoEff = ThingCompUtility.TryGetComp<CompGearCamo>(apparel).Props.ActiveCamoEff;
                    string text = Translator.Translate("CompCamo.Active");
                    if (ThingCompUtility.TryGetComp<CompGearCamo>(apparel).Props.StealthCamoChance > 0 && activeCamoEff > 0f)
                    {
                        text = Translator.Translate("CompCamo.Stealth");
                    }
                    return TranslatorFormattedStringExtensions.Translate("CompCamo.CamouflageDesc", text, GenText.ToStringPercent(activeCamoEff));
                }
            }
            else if (CamoGearUtility.GetCurCamoEff(this.Pawn, out string a, out float num))
            {
                string text2 = Translator.Translate("CompCamo.Undefined");
                if (!(a == "Arctic"))
                {
                    if (!(a == "Desert"))
                    {
                        if (!(a == "Jungle"))
                        {
                            if (!(a == "Stone"))
                            {
                                if (!(a == "Urban"))
                                {
                                    if (a == "Woodland")
                                    {
                                        text2 = Translator.Translate("CompCamo.Woodland");
                                    }
                                }
                                else
                                {
                                    text2 = Translator.Translate("CompCamo.Urban");
                                }
                            }
                            else
                            {
                                text2 = Translator.Translate("CompCamo.Stone");
                            }
                        }
                        else
                        {
                            text2 = Translator.Translate("CompCamo.Jungle");
                        }
                    }
                    else
                    {
                        text2 = Translator.Translate("CompCamo.Desert");
                    }
                }
                else
                {
                    text2 = Translator.Translate("CompCamo.Arctic");
                }
                return TranslatorFormattedStringExtensions.Translate("CompCamo.CamouflageDesc", text2, GenText.ToStringPercent(num));
            }
            return null;
        }

        // Token: 0x17000008 RID: 8
        // (get) Token: 0x06000065 RID: 101 RVA: 0x000067B5 File Offset: 0x000049B5
        private Pawn Pawn
        {
            get
            {
                return (Pawn)this.parent;
            }
        }

        // Token: 0x0400001D RID: 29
        public float PawnArcticCamo;

        // Token: 0x0400001E RID: 30
        public float PawnDesertCamo;

        // Token: 0x0400001F RID: 31
        public float PawnJungleCamo;

        // Token: 0x04000020 RID: 32
        public float PawnStoneCamo;

        // Token: 0x04000021 RID: 33
        public float PawnWoodlandCamo;

        // Token: 0x04000022 RID: 34
        public float PawnUrbanCamo;

        // Token: 0x04000023 RID: 35
        public float PawnnotDefinedCamo;

        // Token: 0x04000024 RID: 36
        public List<string> PawnHidTickList = new List<string>();

        // Token: 0x04000025 RID: 37
        public int LastCamoCorrectTick;

        // Token: 0x02000021 RID: 33
        public class CompProperties_PawnCamoData : CompProperties
        {
            // Token: 0x06000091 RID: 145 RVA: 0x000072F7 File Offset: 0x000054F7
            public CompProperties_PawnCamoData()
            {
                this.compClass = typeof(PawnCamoData);
            }
        }

        // Token: 0x02000022 RID: 34
        [StaticConstructorOnStartup]
        private static class Camo_Setup
        {
            // Token: 0x06000092 RID: 146 RVA: 0x0000730F File Offset: 0x0000550F
            static Camo_Setup()
            {
                PawnCamoData.Camo_Setup.Camo_Setup_Pawns();
            }

            // Token: 0x06000093 RID: 147 RVA: 0x00007316 File Offset: 0x00005516
            private static void Camo_Setup_Pawns()
            {
                PawnCamoData.Camo_Setup.CamoSetup_Comp(typeof(PawnCamoData.CompProperties_PawnCamoData), (ThingDef def) => def.race != null);
            }

            // Token: 0x06000094 RID: 148 RVA: 0x00007348 File Offset: 0x00005548
            private static void CamoSetup_Comp(Type compType, Func<ThingDef, bool> qualifier)
            {
                List<ThingDef> list = DefDatabase<ThingDef>.AllDefsListForReading.Where(qualifier).ToList();
                GenList.RemoveDuplicates<ThingDef>(list);
                foreach (ThingDef thingDef in list)
                {
                    if (thingDef.comps != null && !GenCollection.Any<CompProperties>(thingDef.comps, (Predicate<CompProperties>)((CompProperties c) => (object)((object)c).GetType() == compType)))
                    {
                        thingDef.comps.Add((CompProperties)(object)(CompProperties)Activator.CreateInstance(compType));
                    }
                    if (thingDef.IsApparel)
                    {
                        if (GenCollection.Any<CompProperties>(thingDef.comps, (CompProperties c) => c.GetType() == typeof(CompProperties_GearCamo)))
                        {
                            foreach (CompProperties compProperties in thingDef.comps)
                            {
                                if (compProperties.GetType() == typeof(CompProperties_GearCamo) && (compProperties as CompProperties_GearCamo).ActiveCamoEff > 0f && (compProperties as CompProperties_GearCamo).CamoEnergyMax > 0f && thingDef.thingClass == typeof(Apparel))
                                {
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
        }

    }
}
