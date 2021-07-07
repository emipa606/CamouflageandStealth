using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace CompCamo
{
    // Token: 0x02000006 RID: 6
    public class CamoGearUtility
    {
        // Token: 0x06000036 RID: 54 RVA: 0x00003D00 File Offset: 0x00001F00
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

        // Token: 0x06000037 RID: 55 RVA: 0x00003D60 File Offset: 0x00001F60
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

        // Token: 0x06000038 RID: 56 RVA: 0x00003F58 File Offset: 0x00002158
        internal static bool GetCurCamoEff(Pawn pawn, out string type, out float CamoEff)
        {
            CamoEff = 0f;
            type = GetCamoType(pawn);
            var pawnCamoData = pawn.TryGetComp<PawnCamoData>();
            if (pawnCamoData == null)
            {
                return CamoEff > 0f;
            }

            var a = type;
            if (a != "Arctic")
            {
                if (a != "Desert")
                {
                    if (a != "Jungle")
                    {
                        if (a != "Stone")
                        {
                            if (a != "Woodland")
                            {
                                CamoEff = a != "Urban" ? pawnCamoData.PawnnotDefinedCamo : pawnCamoData.PawnUrbanCamo;
                            }
                            else
                            {
                                CamoEff = pawnCamoData.PawnWoodlandCamo;
                            }
                        }
                        else
                        {
                            CamoEff = pawnCamoData.PawnStoneCamo;
                        }
                    }
                    else
                    {
                        CamoEff = pawnCamoData.PawnJungleCamo;
                    }
                }
                else
                {
                    CamoEff = pawnCamoData.PawnDesertCamo;
                }
            }
            else
            {
                CamoEff = pawnCamoData.PawnArcticCamo;
            }

            return CamoEff > 0f;
        }

        internal static uint ComputeStringHash(string s)
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

        // Token: 0x06000039 RID: 57 RVA: 0x00004024 File Offset: 0x00002224
        internal static void WearingCamoGear(Pawn pawn, out float ArcticCamoEff, out float DesertCamoEff,
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
            var list = new List<string>();
            var list2 = new List<string>();
            if (pawn?.apparel != null && pawn.apparel.WornApparelCount > 0)
            {
                foreach (var apparel in pawn.apparel.WornApparel)
                {
                    var apparelArcticEff = 0f;
                    var apparelDesertEff = 0f;
                    var apparelJungleEff = 0f;
                    var apparelStoneEff = 0f;
                    var apparelWoodlandEff = 0f;
                    var apparelUrbanEff = 0f;
                    var apparelnotDefinedEff = 0f;
                    foreach (var text in CamoTypes())
                    {
                        var num = Math.Min(1f, GetApparelCamoEff(pawn, apparel, text) * GetQualFactor(apparel));
                        var num2 = ComputeStringHash(text);
                        if (num2 <= 1206763323U)
                        {
                            if (num2 != 359505389U)
                            {
                                if (num2 != 437214172U)
                                {
                                    if (num2 != 1206763323U)
                                    {
                                        continue;
                                    }

                                    if (text == "Urban")
                                    {
                                        apparelUrbanEff = num;
                                    }
                                }
                                else if (text == "Desert")
                                {
                                    apparelDesertEff = num;
                                }
                            }
                            else if (text == "Arctic")
                            {
                                apparelArcticEff = num;
                            }
                        }
                        else if (num2 <= 1858049587U)
                        {
                            if (num2 != 1842662042U)
                            {
                                if (num2 != 1858049587U)
                                {
                                    continue;
                                }

                                if (text == "notDefined")
                                {
                                    apparelnotDefinedEff = num;
                                }
                            }
                            else if (text == "Stone")
                            {
                                apparelStoneEff = num;
                            }
                        }
                        else if (num2 != 3655469229U)
                        {
                            if (num2 != 3729410372U)
                            {
                                continue;
                            }

                            if (text == "Jungle")
                            {
                                apparelJungleEff = num;
                            }
                        }
                        else if (text == "Woodland")
                        {
                            apparelWoodlandEff = num;
                        }
                    }

                    var bodyPartGroups = apparel.def.apparel.bodyPartGroups;
                    var drawOrder = apparel.def.apparel.LastLayer.drawOrder;
                    foreach (var bodyPartGroupDef in bodyPartGroups)
                    {
                        list.Add(GetNewRecord(bodyPartGroupDef, drawOrder, apparelArcticEff, apparelDesertEff,
                            apparelJungleEff, apparelStoneEff, apparelWoodlandEff, apparelUrbanEff,
                            apparelnotDefinedEff));
                        list2.AddDistinct(bodyPartGroupDef.defName);
                    }
                }

                if (list.Count > 0 && list2.Count > 0)
                {
                    var num3 = 0f;
                    var num4 = 0f;
                    var num5 = 0f;
                    var num6 = 0f;
                    var num7 = 0f;
                    var num8 = 0f;
                    var num9 = 0f;
                    var num10 = 0;
                    foreach (var b in list2)
                    {
                        var num11 = 0;
                        var num12 = 0f;
                        var num13 = 0f;
                        var num14 = 0f;
                        var num15 = 0f;
                        var num16 = 0f;
                        var num17 = 0f;
                        var num18 = 0f;
                        foreach (var valuesStr in list)
                        {
                            if (GetStrValue(valuesStr, 0) != b)
                            {
                                continue;
                            }

                            var intValue = GetIntValue(valuesStr, 1);
                            if (intValue < num11)
                            {
                                continue;
                            }

                            num11 = intValue;
                            num12 = GetIntValue(valuesStr, 2) / 1000f;
                            num13 = GetIntValue(valuesStr, 3) / 1000f;
                            num14 = GetIntValue(valuesStr, 4) / 1000f;
                            num15 = GetIntValue(valuesStr, 5) / 1000f;
                            num16 = GetIntValue(valuesStr, 6) / 1000f;
                            num17 = GetIntValue(valuesStr, 7) / 1000f;
                            num18 = GetIntValue(valuesStr, 8) / 1000f;
                        }

                        num3 += num12;
                        num4 += num13;
                        num5 += num14;
                        num6 += num15;
                        num7 += num16;
                        num8 += num17;
                        num9 += num18;
                        num10++;
                    }

                    if (num10 > 0)
                    {
                        ArcticCamoEff = num3 / num10;
                        DesertCamoEff = num4 / num10;
                        JungleCamoEff = num5 / num10;
                        StoneCamoEff = num6 / num10;
                        WoodlandCamoEff = num7 / num10;
                        UrbanCamoEff = num8 / num10;
                        notDefinedCamoEff = num9 / num10;
                    }
                }
            }

            list.Clear();
            list2.Clear();
        }

        // Token: 0x0600003A RID: 58 RVA: 0x00004558 File Offset: 0x00002758
        internal static string GetNewRecord(BodyPartGroupDef BPGD, int priority, float apparelArcticEff,
            float apparelDesertEff, float apparelJungleEff, float apparelStoneEff, float apparelWoodlandEff,
            float apparelUrbanEff, float apparelnotDefinedEff)
        {
            return BPGD.defName + ";" + priority + ";" + (int) (apparelArcticEff * 1000f) + ";" +
                   (int) (apparelDesertEff * 1000f) + ";" + (int) (apparelJungleEff * 1000f) + ";" +
                   (int) (apparelStoneEff * 1000f) + ";" + (int) (apparelWoodlandEff * 1000f) + ";" +
                   (int) (apparelUrbanEff * 1000f) + ";" + (int) (apparelnotDefinedEff * 1000f);
        }

        // Token: 0x0600003B RID: 59 RVA: 0x00004638 File Offset: 0x00002838
        internal static string GetStrValue(string valuesStr, int position)
        {
            char[] separator =
            {
                ';'
            };
            return valuesStr.Split(separator)[position];
        }

        // Token: 0x0600003C RID: 60 RVA: 0x0000465C File Offset: 0x0000285C
        internal static int GetIntValue(string valuesStr, int position)
        {
            char[] separator =
            {
                ';'
            };
            var array = valuesStr.Split(separator);
            try
            {
                return int.Parse(array[position]);
            }
            catch (FormatException)
            {
                Log.Message(string.Concat("Unable to parse Seg[", position.ToString(), "]: '", array[position], "'"));
            }

            return 0;
        }

        // Token: 0x0600003D RID: 61 RVA: 0x000046D8 File Offset: 0x000028D8
        internal static float GetApparelCamoEff(Pawn pawn, Apparel apparel, string camoType)
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

        // Token: 0x0600003E RID: 62 RVA: 0x00004794 File Offset: 0x00002994
        internal static string GetCamoType(Pawn pawn)
        {
            var text = "notDefined";
            if (text == "notDefined" && pawn.Position.GetSnowDepth(pawn.Map) >= 0.25f)
            {
                return "Arctic";
            }

            var terrain = pawn.Position.GetTerrain(pawn.Map);
            if (text == "notDefined" && terrain != null)
            {
                if (terrain.smoothedTerrain != null ||
                    terrain.affordances.Contains(TerrainAffordanceDefOf.SmoothableStone))
                {
                    return "Stone";
                }

                text = IsFluffyStuffed(terrain, out var text2) ? text2 : CamoDefGet.GetCamoDefTerrain(terrain);

                if (Prefs.DevMode && Controller.Settings.ShowTerrainLogs && Find.TickManager.TicksGame % 120 == 0)
                {
                    Log.Message("Terrain: " + terrain.defName + " : " + text);
                }
            }

            if (text == "notDefined" && !pawn.Position.UsesOutdoorTemperature(pawn.Map))
            {
                return "Urban";
            }

            if (text == "notDefined")
            {
                text = CamoDefGet.GetCamoDefBiome(pawn.Map.Biome);
            }

            return text;
        }

        // Token: 0x0600003F RID: 63 RVA: 0x000048B4 File Offset: 0x00002AB4
        internal static bool IsFluffyStuffed(TerrainDef terrain, out string camoType)
        {
            camoType = "notDefined";
            if (!terrain.defName.Contains("_") || !GetFSValue(terrain.defName, out var text))
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

        // Token: 0x06000040 RID: 64 RVA: 0x00004954 File Offset: 0x00002B54
        internal static bool GetFSValue(string str, out string FString)
        {
            FString = "";
            if (str.LastIndexOf("_", StringComparison.Ordinal) >= str.Length)
            {
                return false;
            }

            var text = str.Substring(str.LastIndexOf("_", StringComparison.Ordinal) + 1);
            if (!text.StartsWith("Stuffed"))
            {
                return false;
            }

            text = text.Substring(7);
            FString = text;
            return true;
        }

        // Token: 0x06000041 RID: 65 RVA: 0x000049B0 File Offset: 0x00002BB0
        internal static float GetQualFactor(Apparel apparel)
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
                    return 1f;
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


        // Token: 0x06000042 RID: 66 RVA: 0x00004A1A File Offset: 0x00002C1A
        internal static bool GetIsACApparel(ThingDef def)
        {
            return def?.thingClass.FullName == "CompCamo.ActiveCamoApparel";
        }

        // Token: 0x06000043 RID: 67 RVA: 0x00004A3C File Offset: 0x00002C3C
        internal static bool IsWearingActiveCamo(Pawn pawn, out Apparel ACItem)
        {
            ACItem = null;

            var apparel = pawn?.apparel;
            if (apparel == null || apparel.WornApparelCount <= 0)
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
}