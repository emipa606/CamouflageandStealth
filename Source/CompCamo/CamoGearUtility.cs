using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace CompCamo;

public class CamoGearUtility
{
    public static List<string> CamoTypes()
    {
        var list = new List<string>();
        list.AddDistinct("Arctic");
        list.AddDistinct("Desert");
        list.AddDistinct("Jungle");
        list.AddDistinct("Stone");
        list.AddDistinct("Woodland");
        list.AddDistinct("Urban");
        list.AddDistinct("notDefined");
        return list;
    }

    internal static void CalcAndSetCamoEff(Pawn pawn)
    {
        float num;
        float num2;
        float num3;
        float num4;
        float num5;
        float num6;
        float num7;
        if (StealthyBox.IsWearingStealthBox(pawn, out var apparel))
        {
            var compGearCamo = apparel.TryGetComp<CompGearCamo>();
            if (compGearCamo != null)
            {
                num = compGearCamo.Props.ArcticCamoEff;
                num2 = compGearCamo.Props.DesertCamoEff;
                num3 = compGearCamo.Props.JungleCamoEff;
                num4 = compGearCamo.Props.StoneCamoEff;
                num5 = compGearCamo.Props.WoodlandCamoEff;
                num6 = compGearCamo.Props.UrbanCamoEff;
                num7 = (compGearCamo.Props.ArcticCamoEff + compGearCamo.Props.DesertCamoEff +
                        compGearCamo.Props.JungleCamoEff + compGearCamo.Props.StoneCamoEff +
                        compGearCamo.Props.WoodlandCamoEff + compGearCamo.Props.UrbanCamoEff) / 6f;
            }
            else
            {
                num = 1f;
                num2 = 1f;
                num3 = 1f;
                num4 = 1f;
                num5 = 1f;
                num6 = 1f;
                num7 = 1f;
            }
        }
        else
        {
            WearingCamoGear(pawn, out var num8, out var num9, out var num10, out var num11, out var num12,
                out var num13, out var num14);
            num = num8;
            num2 = num9;
            num3 = num10;
            num4 = num11;
            num5 = num12;
            num6 = num13;
            num7 = num14;
        }

        var pawnArcticCamo = num;
        var pawnDesertCamo = num2;
        var pawnJungleCamo = num3;
        var pawnStoneCamo = num4;
        var pawnWoodlandCamo = num5;
        var pawnUrbanCamo = num6;
        var pawnnotDefinedCamo = num7;
        var pawnCamoData = pawn.TryGetComp<PawnCamoData>();
        if (pawnCamoData == null)
        {
            return;
        }

        pawnCamoData.PawnArcticCamo = pawnArcticCamo;
        pawnCamoData.PawnDesertCamo = pawnDesertCamo;
        pawnCamoData.PawnJungleCamo = pawnJungleCamo;
        pawnCamoData.PawnStoneCamo = pawnStoneCamo;
        pawnCamoData.PawnWoodlandCamo = pawnWoodlandCamo;
        pawnCamoData.PawnUrbanCamo = pawnUrbanCamo;
        pawnCamoData.PawnnotDefinedCamo = pawnnotDefinedCamo;
    }

    internal static bool GetCurCamoEff(Pawn pawn, out string type, out float CamoEff)
    {
        CamoEff = 0f;
        type = getCamoType(pawn);
        var pawnCamoData = pawn.TryGetComp<PawnCamoData>();
        if (pawnCamoData == null)
        {
            return CamoEff > 0f;
        }

        var a = type;
        switch (a)
        {
            case "Arctic":
                CamoEff = pawnCamoData.PawnArcticCamo;
                break;
            case "Desert":
                CamoEff = pawnCamoData.PawnDesertCamo;
                break;
            case "Jungle":
                CamoEff = pawnCamoData.PawnJungleCamo;
                break;
            case "Stone":
                CamoEff = pawnCamoData.PawnStoneCamo;
                break;
            case "Woodland":
                CamoEff = pawnCamoData.PawnWoodlandCamo;
                break;
            case "Urban":
                CamoEff = pawnCamoData.PawnUrbanCamo;
                break;
            default:
            {
                CamoEff = pawnCamoData.PawnnotDefinedCamo;
                break;
            }
        }

        return CamoEff > 0f;
    }

    private static uint ComputeStringHash(string s)
    {
        uint num = 0;
        if (s == null)
        {
            return num;
        }

        num = 2166136261U;
        foreach (var c in s)
        {
            num = (c ^ num) * 16777619U;
        }

        return num;
    }

    private static void WearingCamoGear(Pawn pawn, out float ArcticCamoEff, out float DesertCamoEff,
        out float JungleCamoEff, out float StoneCamoEff, out float WoodlandCamoEff, out float UrbanCamoEff,
        out float notDefinedCamoEff)
    {
        ArcticCamoEff = 0f;
        DesertCamoEff = 0f;
        JungleCamoEff = 0f;
        StoneCamoEff = 0f;
        WoodlandCamoEff = 0f;
        UrbanCamoEff = 0f;
        notDefinedCamoEff = 0f;
        if (pawn?.apparel is not { WornApparelCount: > 0 } || pawn.health?.hediffSet == null)
        {
            return;
        }

        var totalCoverage = 0f;
        foreach (var bodyPartRecord in pawn.health.hediffSet.GetNotMissingParts())
        {
            if (!bodyPartRecord.def.IsSkinCovered(bodyPartRecord, pawn.health.hediffSet) ||
                bodyPartRecord.coverageAbs <= 0f)
            {
                continue;
            }

            totalCoverage += bodyPartRecord.coverageAbs;
            var apparel = getTopApparelForPart(pawn, bodyPartRecord);
            if (apparel == null)
            {
                continue;
            }

            var qualFactor = getQualFactor(apparel);
            ArcticCamoEff += bodyPartRecord.coverageAbs * getApparelCamoEffForType(pawn, apparel, qualFactor, "Arctic");
            DesertCamoEff += bodyPartRecord.coverageAbs * getApparelCamoEffForType(pawn, apparel, qualFactor, "Desert");
            JungleCamoEff += bodyPartRecord.coverageAbs * getApparelCamoEffForType(pawn, apparel, qualFactor, "Jungle");
            StoneCamoEff += bodyPartRecord.coverageAbs * getApparelCamoEffForType(pawn, apparel, qualFactor, "Stone");
            WoodlandCamoEff += bodyPartRecord.coverageAbs *
                               getApparelCamoEffForType(pawn, apparel, qualFactor, "Woodland");
            UrbanCamoEff += bodyPartRecord.coverageAbs * getApparelCamoEffForType(pawn, apparel, qualFactor, "Urban");
            notDefinedCamoEff += bodyPartRecord.coverageAbs *
                                 getApparelCamoEffForType(pawn, apparel, qualFactor, "notDefined");
        }

        if (totalCoverage <= 0f)
        {
            return;
        }

        ArcticCamoEff /= totalCoverage;
        DesertCamoEff /= totalCoverage;
        JungleCamoEff /= totalCoverage;
        StoneCamoEff /= totalCoverage;
        WoodlandCamoEff /= totalCoverage;
        UrbanCamoEff /= totalCoverage;
        notDefinedCamoEff /= totalCoverage;
    }

    private static Apparel getTopApparelForPart(Pawn pawn, BodyPartRecord bodyPartRecord)
    {
        Apparel result = null;
        var num = int.MinValue;
        foreach (var apparel in pawn.apparel.WornApparel)
        {
            var apparelProps = apparel.def.apparel;
            if (apparelProps == null || !apparelProps.CoversBodyPart(bodyPartRecord))
            {
                continue;
            }

            var drawOrder = apparelProps.LastLayer.drawOrder;
            if (drawOrder < num)
            {
                continue;
            }

            num = drawOrder;
            result = apparel;
        }

        return result;
    }

    private static float getApparelCamoEffForType(Pawn pawn, Apparel apparel, float qualFactor, string camoType)
    {
        return Math.Min(1f, getApparelCamoEff(pawn, apparel, camoType) * qualFactor);
    }

    internal static string GetStrValue(string valuesStr, int position)
    {
        char[] separator =
        [
            ';'
        ];
        return valuesStr.Split(separator)[position];
    }

    internal static int GetIntValue(string valuesStr, int position)
    {
        char[] separator =
        [
            ';'
        ];
        var array = valuesStr.Split(separator);
        try
        {
            return int.Parse(array[position]);
        }
        catch (FormatException)
        {
            Log.Message($"Unable to parse Seg[{position}]: '{array[position]}'");
        }

        return 0;
    }

    private static float getApparelCamoEff(Pawn pawn, Apparel apparel, string camoType)
    {
        var num = 0f;
        if (pawn?.Map == null || camoType == null)
        {
            return num;
        }

        if (apparel.TryGetComp<CompGearCamo>() != null)
        {
            if (camoType == "notDefined")
            {
                num += CamoPresets.GetCamoPresetEff(apparel, "Arctic");
                num += CamoPresets.GetCamoPresetEff(apparel, "Desert");
                num += CamoPresets.GetCamoPresetEff(apparel, "Jungle");
                num += CamoPresets.GetCamoPresetEff(apparel, "Stone");
                num += CamoPresets.GetCamoPresetEff(apparel, "Woodland");
                num += CamoPresets.GetCamoPresetEff(apparel, "Urban");
                num = num / 6f * 0.75f;
            }
            else
            {
                num = CamoPresets.GetCamoPresetEff(apparel, camoType);
            }
        }
        else
        {
            num = CamoPresets.GetCamoPresetEff(apparel, camoType);
        }

        return num;
    }

    private static string getCamoType(Pawn pawn)
    {
        var position = pawn.Position;
        var map = pawn.Map;

        if (position == IntVec3.Invalid)
        {
            position = pawn.PositionHeld;
        }

        map ??= pawn.MapHeld;

        if (map == null)
        {
            return "notDefined";
        }

        if (position == IntVec3.Invalid)
        {
            return CamoDefGet.GetCamoDefBiome(map.Biome);
        }

        var text = "notDefined";
        if (position.GetSnowDepth(map) >= 0.25f)
        {
            return "Arctic";
        }

        var terrain = position.GetTerrain(map);
        if (text == "notDefined" && terrain != null)
        {
            if (terrain.smoothedTerrain != null ||
                terrain.affordances.Contains(TerrainAffordanceDefOf.SmoothableStone))
            {
                return "Stone";
            }

            text = isFluffyStuffed(terrain, out var text2) ? text2 : CamoDefGet.GetCamoDefTerrain(terrain);

            if (Prefs.DevMode && Controller.Settings.ShowTerrainLogs && Find.TickManager.TicksGame % 120 == 0)
            {
                Log.Message($"Terrain: {terrain.defName} : {text}");
            }
        }

        switch (text)
        {
            case "notDefined" when !position.UsesOutdoorTemperature(map):
                return "Urban";
            case "notDefined":
                text = CamoDefGet.GetCamoDefBiome(map.Biome);
                break;
        }

        return text;
    }

    private static bool isFluffyStuffed(TerrainDef terrain, out string camoType)
    {
        camoType = "notDefined";
        if (!terrain.defName.Contains("_") || !getFsValue(terrain.defName, out var text))
        {
            return false;
        }

        if (text.StartsWith("Wood") || text.StartsWith("Plywood"))
        {
            camoType = "Woodland";
            return true;
        }

        if (text.StartsWith("Stone") || text.StartsWith("Flagstones") || text.StartsWith("Smooth"))
        {
            camoType = "Stone";
            return true;
        }

        if (!text.StartsWith("FloorsMetal"))
        {
            return false;
        }

        camoType = "Urban";
        return true;
    }

    private static bool getFsValue(string str, out string FString)
    {
        FString = "";
        if (str.LastIndexOf("_", StringComparison.Ordinal) >= str.Length)
        {
            return false;
        }

        var text = str[(str.LastIndexOf("_", StringComparison.Ordinal) + 1)..];
        if (!text.StartsWith("Stuffed"))
        {
            return false;
        }

        text = text[7..];
        FString = text;
        return true;
    }

    private static float getQualFactor(Apparel apparel)
    {
        if (!apparel.TryGetQuality(out var qualityCategory))
        {
            return 1f;
        }

        switch (qualityCategory)
        {
            case QualityCategory.Awful:
                return 0.96f;
            case QualityCategory.Poor:
                return 0.98f;
            case QualityCategory.Normal:
                break;
            case QualityCategory.Good:
                return 1.02f;
            case QualityCategory.Excellent:
                return 1.04f;
            case QualityCategory.Masterwork:
                return 1.06f;
            case QualityCategory.Legendary:
                return 1.08f;
        }

        return 1f;
    }


    internal static bool GetIsAcApparel(ThingDef def)
    {
        return def?.thingClass.FullName == "CompCamo.ActiveCamoApparel";
    }

    internal static bool IsWearingActiveCamo(Pawn pawn, out Apparel ACItem)
    {
        ACItem = null;

        var apparel = pawn?.apparel;
        if (apparel is not { WornApparelCount: > 0 })
        {
            return false;
        }

        foreach (var apparel2 in pawn.apparel.WornApparel)
        {
            if (apparel2 is not ActiveCamoApparel)
            {
                continue;
            }

            ACItem = apparel2;
            return true;
        }

        return false;
    }
}