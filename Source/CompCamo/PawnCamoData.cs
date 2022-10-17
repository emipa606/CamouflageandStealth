using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace CompCamo;

public class PawnCamoData : ThingComp
{
    public int LastCamoCorrectTick;

    public float PawnArcticCamo;

    public float PawnDesertCamo;

    public List<string> PawnHidTickList = new List<string>();

    public float PawnJungleCamo;

    public float PawnnotDefinedCamo;

    public float PawnStoneCamo;

    public float PawnUrbanCamo;

    public float PawnWoodlandCamo;

    private Pawn Pawn => (Pawn)parent;

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

    public class CompProperties_PawnCamoData : CompProperties
    {
        public CompProperties_PawnCamoData()
        {
            compClass = typeof(PawnCamoData);
        }
    }

    [StaticConstructorOnStartup]
    private static class Camo_Setup
    {
        static Camo_Setup()
        {
            Camo_Setup_Pawns();
        }

        private static void Camo_Setup_Pawns()
        {
            CamoSetup_Comp(typeof(CompProperties_PawnCamoData), def => def.race != null);
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
}