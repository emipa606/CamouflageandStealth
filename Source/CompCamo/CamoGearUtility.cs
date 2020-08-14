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
			List<string> list = new List<string>();
			GenCollection.AddDistinct<string>(list, "Arctic");
			GenCollection.AddDistinct<string>(list, "Desert");
			GenCollection.AddDistinct<string>(list, "Jungle");
			GenCollection.AddDistinct<string>(list, "Stone");
			GenCollection.AddDistinct<string>(list, "Woodland");
			GenCollection.AddDistinct<string>(list, "Urban");
			GenCollection.AddDistinct<string>(list, "notDefined");
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
			if (StealthyBox.IsWearingStealthBox(pawn, out Apparel apparel))
			{
				CompGearCamo compGearCamo = ThingCompUtility.TryGetComp<CompGearCamo>(apparel);
				if (compGearCamo != null)
				{
					num = compGearCamo.Props.ArcticCamoEff;
					num2 = compGearCamo.Props.DesertCamoEff;
					num3 = compGearCamo.Props.JungleCamoEff;
					num4 = compGearCamo.Props.StoneCamoEff;
					num5 = compGearCamo.Props.WoodlandCamoEff;
					num6 = compGearCamo.Props.UrbanCamoEff;
					num7 = (compGearCamo.Props.ArcticCamoEff + compGearCamo.Props.DesertCamoEff + compGearCamo.Props.JungleCamoEff + compGearCamo.Props.StoneCamoEff + compGearCamo.Props.WoodlandCamoEff + compGearCamo.Props.UrbanCamoEff) / 6f;
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
                CamoGearUtility.WearingCamoGear(pawn, out float num8, out float num9, out float num10, out float num11, out float num12, out float num13, out float num14);
                num = num8;
				num2 = num9;
				num3 = num10;
				num4 = num11;
				num5 = num12;
				num6 = num13;
				num7 = num14;
			}
			float pawnArcticCamo = num;
			float pawnDesertCamo = num2;
			float pawnJungleCamo = num3;
			float pawnStoneCamo = num4;
			float pawnWoodlandCamo = num5;
			float pawnUrbanCamo = num6;
			float pawnnotDefinedCamo = num7;
			PawnCamoData pawnCamoData = ThingCompUtility.TryGetComp<PawnCamoData>(pawn);
			if (pawnCamoData != null)
			{
				pawnCamoData.PawnArcticCamo = pawnArcticCamo;
				pawnCamoData.PawnDesertCamo = pawnDesertCamo;
				pawnCamoData.PawnJungleCamo = pawnJungleCamo;
				pawnCamoData.PawnStoneCamo = pawnStoneCamo;
				pawnCamoData.PawnWoodlandCamo = pawnWoodlandCamo;
				pawnCamoData.PawnUrbanCamo = pawnUrbanCamo;
				pawnCamoData.PawnnotDefinedCamo = pawnnotDefinedCamo;
			}
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00003F58 File Offset: 0x00002158
		internal static bool GetCurCamoEff(Pawn pawn, out string type, out float CamoEff)
		{
			CamoEff = 0f;
			type = CamoGearUtility.GetCamoType(pawn);
			PawnCamoData pawnCamoData = ThingCompUtility.TryGetComp<PawnCamoData>(pawn);
			if (pawnCamoData != null)
			{
				string a = type;
				if (!(a == "Arctic"))
				{
					if (!(a == "Desert"))
					{
						if (!(a == "Jungle"))
						{
							if (!(a == "Stone"))
							{
								if (!(a == "Woodland"))
								{
									if (!(a == "Urban"))
									{
										CamoEff = pawnCamoData.PawnnotDefinedCamo;
									}
									else
									{
										CamoEff = pawnCamoData.PawnUrbanCamo;
									}
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
			}
			return CamoEff > 0f;
		}

		internal static uint ComputeStringHash(string s)
		{
			uint num = 0;
			if (s != null)
			{
				num = 2166136261U;
				for (int i = 0; i < s.Length; i++)
				{
					num = ((uint)s[i] ^ num) * 16777619U;
				}
			}
			return num;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00004024 File Offset: 0x00002224
		internal static void WearingCamoGear(Pawn pawn, out float ArcticCamoEff, out float DesertCamoEff, out float JungleCamoEff, out float StoneCamoEff, out float WoodlandCamoEff, out float UrbanCamoEff, out float notDefinedCamoEff)
		{
			ArcticCamoEff = 0f;
			DesertCamoEff = 0f;
			JungleCamoEff = 0f;
			StoneCamoEff = 0f;
			WoodlandCamoEff = 0f;
			UrbanCamoEff = 0f;
			notDefinedCamoEff = 0f;
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			if ((pawn?.apparel) != null && pawn.apparel.WornApparelCount > 0)
			{
				foreach (Apparel apparel in pawn.apparel.WornApparel)
				{
					float apparelArcticEff = 0f;
					float apparelDesertEff = 0f;
					float apparelJungleEff = 0f;
					float apparelStoneEff = 0f;
					float apparelWoodlandEff = 0f;
					float apparelUrbanEff = 0f;
					float apparelnotDefinedEff = 0f;
					foreach (string text in CamoGearUtility.CamoTypes())
					{
						float num = Math.Min(1f, CamoGearUtility.GetApparelCamoEff(pawn, apparel, text) * CamoGearUtility.GetQualFactor(apparel));
						uint num2 = ComputeStringHash(text);
						if (num2 <= 1206763323U)
						{
							if (num2 != 359505389U)
							{
								if (num2 != 437214172U)
								{
									if (num2 == 1206763323U)
									{
										if (text == "Urban")
										{
											apparelUrbanEff = num;
										}
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
								if (num2 == 1858049587U)
								{
									if (text == "notDefined")
									{
										apparelnotDefinedEff = num;
									}
								}
							}
							else if (text == "Stone")
							{
								apparelStoneEff = num;
							}
						}
						else if (num2 != 3655469229U)
						{
							if (num2 == 3729410372U)
							{
								if (text == "Jungle")
								{
									apparelJungleEff = num;
								}
							}
						}
						else if (text == "Woodland")
						{
							apparelWoodlandEff = num;
						}
					}
					List<BodyPartGroupDef> bodyPartGroups = apparel.def.apparel.bodyPartGroups;
					int drawOrder = apparel.def.apparel.LastLayer.drawOrder;
					foreach (BodyPartGroupDef bodyPartGroupDef in bodyPartGroups)
					{
						list.Add(CamoGearUtility.GetNewRecord(bodyPartGroupDef, drawOrder, apparelArcticEff, apparelDesertEff, apparelJungleEff, apparelStoneEff, apparelWoodlandEff, apparelUrbanEff, apparelnotDefinedEff));
						GenCollection.AddDistinct<string>(list2, bodyPartGroupDef.defName);
					}
				}
				if (list.Count > 0 && list2.Count > 0)
				{
					float num3 = 0f;
					float num4 = 0f;
					float num5 = 0f;
					float num6 = 0f;
					float num7 = 0f;
					float num8 = 0f;
					float num9 = 0f;
					int num10 = 0;
					foreach (string b in list2)
					{
						int num11 = 0;
						float num12 = 0f;
						float num13 = 0f;
						float num14 = 0f;
						float num15 = 0f;
						float num16 = 0f;
						float num17 = 0f;
						float num18 = 0f;
						foreach (string valuesStr in list)
						{
							if (CamoGearUtility.GetStrValue(valuesStr, 0) == b)
							{
								int intValue = CamoGearUtility.GetIntValue(valuesStr, 1);
								if (intValue >= num11)
								{
									num11 = intValue;
									num12 = (float)CamoGearUtility.GetIntValue(valuesStr, 2) / 1000f;
									num13 = (float)CamoGearUtility.GetIntValue(valuesStr, 3) / 1000f;
									num14 = (float)CamoGearUtility.GetIntValue(valuesStr, 4) / 1000f;
									num15 = (float)CamoGearUtility.GetIntValue(valuesStr, 5) / 1000f;
									num16 = (float)CamoGearUtility.GetIntValue(valuesStr, 6) / 1000f;
									num17 = (float)CamoGearUtility.GetIntValue(valuesStr, 7) / 1000f;
									num18 = (float)CamoGearUtility.GetIntValue(valuesStr, 8) / 1000f;
								}
							}
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
						ArcticCamoEff = num3 / (float)num10;
						DesertCamoEff = num4 / (float)num10;
						JungleCamoEff = num5 / (float)num10;
						StoneCamoEff = num6 / (float)num10;
						WoodlandCamoEff = num7 / (float)num10;
						UrbanCamoEff = num8 / (float)num10;
						notDefinedCamoEff = num9 / (float)num10;
					}
				}
			}
			list.Clear();
			list2.Clear();
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00004558 File Offset: 0x00002758
		internal static string GetNewRecord(BodyPartGroupDef BPGD, int priority, float apparelArcticEff, float apparelDesertEff, float apparelJungleEff, float apparelStoneEff, float apparelWoodlandEff, float apparelUrbanEff, float apparelnotDefinedEff)
		{
			return BPGD.defName + ";" + priority.ToString() + ";" + ((int)(apparelArcticEff * 1000f)).ToString() + ";" + ((int)(apparelDesertEff * 1000f)).ToString() + ";" + ((int)(apparelJungleEff * 1000f)).ToString() + ";" + ((int)(apparelStoneEff * 1000f)).ToString() + ";" + ((int)(apparelWoodlandEff * 1000f)).ToString() + ";" + ((int)(apparelUrbanEff * 1000f)).ToString() + ";" + ((int)(apparelnotDefinedEff * 1000f)).ToString();
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00004638 File Offset: 0x00002838
		internal static string GetStrValue(string valuesStr, int position)
		{
			char[] separator = new char[]
			{
				';'
			};
			return valuesStr.Split(separator)[position];
		}

		// Token: 0x0600003C RID: 60 RVA: 0x0000465C File Offset: 0x0000285C
		internal static int GetIntValue(string valuesStr, int position)
		{
			char[] separator = new char[]
			{
				';'
			};
			string[] array = valuesStr.Split(separator);
			try
			{
				return int.Parse(array[position]);
			}
			catch (FormatException)
			{
				Log.Message(string.Concat(new string[]
				{
					"Unable to parse Seg[",
					position.ToString(),
					"]: '",
					array[position],
					"'"
				}), false);
			}
			return 0;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x000046D8 File Offset: 0x000028D8
		internal static float GetApparelCamoEff(Pawn pawn, Apparel apparel, string camoType)
		{
			float num = 0f;
			if (pawn != null && (pawn?.Map) != null && camoType != null)
			{
				if (ThingCompUtility.TryGetComp<CompGearCamo>(apparel) != null)
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
			}
			return num;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00004794 File Offset: 0x00002994
		internal static string GetCamoType(Pawn pawn)
		{
			string text = "notDefined";
			if (!(text != "notDefined") && GridsUtility.GetSnowDepth(pawn.Position, pawn.Map) >= 0.25f)
			{
				return "Arctic";
			}
			TerrainDef terrain = GridsUtility.GetTerrain(pawn.Position, pawn.Map);
			if (!(text != "notDefined") && terrain != null)
			{
				if ((terrain?.smoothedTerrain) != null || terrain.affordances.Contains(TerrainAffordanceDefOf.SmoothableStone))
				{
					return "Stone";
				}
                if (CamoGearUtility.IsFluffyStuffed(terrain, out string text2))
                {
                    text = text2;
                }
                else
                {
                    text = CamoDefGet.GetCamoDefTerrain(terrain);
                }
                if (Prefs.DevMode && Controller.Settings.ShowTerrainLogs && Find.TickManager.TicksGame % 120 == 0)
				{
					Log.Message("Terrain: " + terrain.defName + " : " + text, false);
				}
			}
			if (!(text != "notDefined") && !GridsUtility.UsesOutdoorTemperature(pawn.Position, pawn.Map))
			{
				return "Urban";
			}
			if (!(text != "notDefined"))
			{
				text = CamoDefGet.GetCamoDefBiome(pawn.Map.Biome);
			}
			return text;
		}

		// Token: 0x0600003F RID: 63 RVA: 0x000048B4 File Offset: 0x00002AB4
		internal static bool IsFluffyStuffed(TerrainDef terrain, out string camoType)
		{
			camoType = "notDefined";
            if (terrain.defName.Contains("_") && CamoGearUtility.GetFSValue(terrain.defName, out string text))
            {
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
                if (text.StartsWith("FloorsMetal"))
                {
                    camoType = "Urban";
                    return true;
                }
            }
            return false;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00004954 File Offset: 0x00002B54
		internal static bool GetFSValue(string str, out string FString)
		{
			FString = "";
			if (str.LastIndexOf("_") < str.Length)
			{
				string text = str?.Substring(str.LastIndexOf("_") + 1);
				if (text.StartsWith("Stuffed"))
				{
					text = text.Substring(7);
					FString = text;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x000049B0 File Offset: 0x00002BB0
		internal static float GetQualFactor(Apparel apparel)
		{
            if (QualityUtility.TryGetQuality(apparel, out QualityCategory qualityCategory))
            {
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
                    default:
                        break;
                }
            }
            return 1f;
		}


		// Token: 0x06000042 RID: 66 RVA: 0x00004A1A File Offset: 0x00002C1A
		internal static bool GetIsACApparel(ThingDef def)
		{
			return (def?.thingClass.FullName) == "CompCamo.ActiveCamoApparel";
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00004A3C File Offset: 0x00002C3C
		internal static bool IsWearingActiveCamo(Pawn pawn, out Apparel ACItem)
		{
			ACItem = null;
			if ((pawn?.apparel) != null)
			{
				Pawn_ApparelTracker apparel = pawn.apparel;
				if (apparel != null && apparel.WornApparelCount > 0)
				{
					foreach (Apparel apparel2 in pawn.apparel.WornApparel)
					{
						if (apparel2 is ActiveCamoApparel)
						{
							ACItem = apparel2;
							return true;
						}
					}
					return false;
				}
			}
			return false;
		}
	}
}
