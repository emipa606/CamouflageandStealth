using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace CompCamo
{
	// Token: 0x02000008 RID: 8
	public class CamoPresets
	{
		// Token: 0x0600004D RID: 77 RVA: 0x00004D8C File Offset: 0x00002F8C
		internal static List<string> GetCamoTags()
		{
			return new List<string>
			{
				"PassiveCamo_Arctic_Low",
				"PassiveCamo_Arctic_Med",
				"PassiveCamo_Arctic_High",
				"PassiveCamo_Desert_Low",
				"PassiveCamo_Desert_Med",
				"PassiveCamo_Desert_High",
				"PassiveCamo_Jungle_Low",
				"PassiveCamo_Jungle_Med",
				"PassiveCamo_Jungle_High",
				"PassiveCamo_Stone_Low",
				"PassiveCamo_Stone_Med",
				"PassiveCamo_Stone_High",
				"PassiveCamo_Woodland_Low",
				"PassiveCamo_Woodland_Med",
				"PassiveCamo_Woodland_High",
				"PassiveCamo_Urban_Low",
				"PassiveCamo_Urban_Med",
				"PassiveCamo_Urban_High"
			};
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00004E64 File Offset: 0x00003064
		internal static List<string> GetColourTags()
		{
			return new List<string>
			{
				"PassiveCamo_Black_Set",
				"PassiveCamo_White_Set",
				"PassiveCamo_Red_Set",
				"PassiveCamo_Orange_Set",
				"PassiveCamo_Yellow_Set",
				"PassiveCamo_Green_Set",
				"PassiveCamo_Blue_Set",
				"PassiveCamo_Cyan_Set",
				"PassiveCamo_Violet_Set",
				"PassiveCamo_Purple_Set",
				"PassiveCamo_Brown_Set",
				"PassiveCamo_Rainbow_Set",
				"PassiveCamo_Light_Set",
				"PassiveCamo_Dark_Set"
			};
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00004F10 File Offset: 0x00003110
		public static float GetCamoPresetEff(Apparel apparel, string type)
		{
			float num = 0f;
			CompGearCamo compGearCamo = ThingCompUtility.TryGetComp<CompGearCamo>(apparel);
			if (compGearCamo != null)
			{
				if (!(type == "Arctic"))
				{
					if (!(type == "Desert"))
					{
						if (!(type == "Jungle"))
						{
							if (!(type == "Stone"))
							{
								if (!(type == "Woodland"))
								{
									if (type == "Urban")
									{
										num = compGearCamo.Props.UrbanCamoEff;
									}
								}
								else
								{
									num = compGearCamo.Props.WoodlandCamoEff;
								}
							}
							else
							{
								num = compGearCamo.Props.StoneCamoEff;
							}
						}
						else
						{
							num = compGearCamo.Props.JungleCamoEff;
						}
					}
					else
					{
						num = compGearCamo.Props.DesertCamoEff;
					}
				}
				else
				{
					num = compGearCamo.Props.ArcticCamoEff;
				}
			}
			else
			{
				ThingDef def = apparel.def;
				List<string> list;
				if (def == null)
				{
					list = null;
				}
				else
				{
					ApparelProperties apparel2 = def.apparel;
					list = (apparel2?.tags);
				}
				List<string> list2 = list;
				if (list2 != null && list2.Count > 0)
				{
					foreach (string text in list2)
					{
						if (text.StartsWith("PassiveCamo") && (CamoPresets.GetCamoTags().Contains(text) || CamoPresets.GetColourTags().Contains(text) || text.StartsWith("PassiveCamo_Multi") || text.StartsWith("PassiveCamo_Colour")))
						{
							string text2 = CamoPresets.GetTagValue(text, 1);
							if (!CamoGearUtility.CamoTypes().Contains(text2) && !(text2 == "Multi"))
							{
								if (text2 == "Colour")
								{
									text2 = CamoPresetColour.GetClosestType(apparel);
								}
								if (text2 == "Black")
								{
									if (!(type == "Arctic"))
									{
										if (!(type == "Desert"))
										{
											if (!(type == "Jungle"))
											{
												if (!(type == "Stone"))
												{
													if (!(type == "Woodland"))
													{
														if (type == "Urban")
														{
															num = 0.3f;
														}
													}
													else
													{
														num = 0.29f;
													}
												}
												else
												{
													num = 0.27f;
												}
											}
											else
											{
												num = 0.3f;
											}
										}
										else
										{
											num = 0.25f;
										}
									}
									else
									{
										num = 0.12f;
									}
								}
								else if (text2 == "White")
								{
									if (!(type == "Arctic"))
									{
										if (!(type == "Desert"))
										{
											if (!(type == "Jungle"))
											{
												if (!(type == "Stone"))
												{
													if (!(type == "Woodland"))
													{
														if (type == "Urban")
														{
															num = 0.3f;
														}
													}
													else
													{
														num = 0.22f;
													}
												}
												else
												{
													num = 0.3f;
												}
											}
											else
											{
												num = 0.22f;
											}
										}
										else
										{
											num = 0.22f;
										}
									}
									else
									{
										num = 0.65f;
									}
								}
								else if (text2 == "Red")
								{
									if (!(type == "Arctic"))
									{
										if (!(type == "Desert"))
										{
											if (!(type == "Jungle"))
											{
												if (!(type == "Stone"))
												{
													if (!(type == "Woodland"))
													{
														if (type == "Urban")
														{
															num = 0.15f;
														}
													}
													else
													{
														num = 0.25f;
													}
												}
												else
												{
													num = 0.15f;
												}
											}
											else
											{
												num = 0.25f;
											}
										}
										else
										{
											num = 0.32f;
										}
									}
									else
									{
										num = 0.05f;
									}
								}
								else if (text2 == "Orange")
								{
									if (!(type == "Arctic"))
									{
										if (!(type == "Desert"))
										{
											if (!(type == "Jungle"))
											{
												if (!(type == "Stone"))
												{
													if (!(type == "Woodland"))
													{
														if (type == "Urban")
														{
															num = 0.17f;
														}
													}
													else
													{
														num = 0.24f;
													}
												}
												else
												{
													num = 0.15f;
												}
											}
											else
											{
												num = 0.25f;
											}
										}
										else
										{
											num = 0.55f;
										}
									}
									else
									{
										num = 0.07f;
									}
								}
								else if (text2 == "Yellow")
								{
									if (!(type == "Arctic"))
									{
										if (!(type == "Desert"))
										{
											if (!(type == "Jungle"))
											{
												if (!(type == "Stone"))
												{
													if (!(type == "Woodland"))
													{
														if (type == "Urban")
														{
															num = 0.18f;
														}
													}
													else
													{
														num = 0.27f;
													}
												}
												else
												{
													num = 0.12f;
												}
											}
											else
											{
												num = 0.29f;
											}
										}
										else
										{
											num = 0.6f;
										}
									}
									else
									{
										num = 0.12f;
									}
								}
								else if (text2 == "Green")
								{
									if (!(type == "Arctic"))
									{
										if (!(type == "Desert"))
										{
											if (!(type == "Jungle"))
											{
												if (!(type == "Stone"))
												{
													if (!(type == "Woodland"))
													{
														if (type == "Urban")
														{
															num = 0.19f;
														}
													}
													else
													{
														num = 0.55f;
													}
												}
												else
												{
													num = 0.15f;
												}
											}
											else
											{
												num = 0.58f;
											}
										}
										else
										{
											num = 0.37f;
										}
									}
									else
									{
										num = 0.1f;
									}
								}
								else if (text2 == "Blue")
								{
									if (!(type == "Arctic"))
									{
										if (!(type == "Desert"))
										{
											if (!(type == "Jungle"))
											{
												if (!(type == "Stone"))
												{
													if (!(type == "Woodland"))
													{
														if (type == "Urban")
														{
															num = 0.25f;
														}
													}
													else
													{
														num = 0.35f;
													}
												}
												else
												{
													num = 0.25f;
												}
											}
											else
											{
												num = 0.37f;
											}
										}
										else
										{
											num = 0.22f;
										}
									}
									else
									{
										num = 0.1f;
									}
								}
								else if (text2 == "Cyan")
								{
									if (!(type == "Arctic"))
									{
										if (!(type == "Desert"))
										{
											if (!(type == "Jungle"))
											{
												if (!(type == "Stone"))
												{
													if (!(type == "Woodland"))
													{
														if (type == "Urban")
														{
															num = 0.23f;
														}
													}
													else
													{
														num = 0.24f;
													}
												}
												else
												{
													num = 0.25f;
												}
											}
											else
											{
												num = 0.24f;
											}
										}
										else
										{
											num = 0.12f;
										}
									}
									else
									{
										num = 0.19f;
									}
								}
								else if (text2 == "Violet")
								{
									if (!(type == "Arctic"))
									{
										if (!(type == "Desert"))
										{
											if (!(type == "Jungle"))
											{
												if (!(type == "Stone"))
												{
													if (!(type == "Woodland"))
													{
														if (type == "Urban")
														{
															num = 0.19f;
														}
													}
													else
													{
														num = 0.19f;
													}
												}
												else
												{
													num = 0.19f;
												}
											}
											else
											{
												num = 0.21f;
											}
										}
										else
										{
											num = 0.26f;
										}
									}
									else
									{
										num = 0.09f;
									}
								}
								else if (text2 == "Purple")
								{
									if (!(type == "Arctic"))
									{
										if (!(type == "Desert"))
										{
											if (!(type == "Jungle"))
											{
												if (!(type == "Stone"))
												{
													if (!(type == "Woodland"))
													{
														if (type == "Urban")
														{
															num = 0.21f;
														}
													}
													else
													{
														num = 0.22f;
													}
												}
												else
												{
													num = 0.22f;
												}
											}
											else
											{
												num = 0.24f;
											}
										}
										else
										{
											num = 0.22f;
										}
									}
									else
									{
										num = 0.07f;
									}
								}
								else if (text2 == "Brown")
								{
									if (!(type == "Arctic"))
									{
										if (!(type == "Desert"))
										{
											if (!(type == "Jungle"))
											{
												if (!(type == "Stone"))
												{
													if (!(type == "Woodland"))
													{
														if (type == "Urban")
														{
															num = 0.23f;
														}
													}
													else
													{
														num = 0.41f;
													}
												}
												else
												{
													num = 0.42f;
												}
											}
											else
											{
												num = 0.4f;
											}
										}
										else
										{
											num = 0.42f;
										}
									}
									else
									{
										num = 0.09f;
									}
								}
								else if (text2 == "Dark")
								{
									if (!(type == "Arctic"))
									{
										if (!(type == "Desert"))
										{
											if (!(type == "Jungle"))
											{
												if (!(type == "Stone"))
												{
													if (!(type == "Woodland"))
													{
														if (type == "Urban")
														{
															num = 0.31f;
														}
													}
													else
													{
														num = 0.3f;
													}
												}
												else
												{
													num = 0.28f;
												}
											}
											else
											{
												num = 0.31f;
											}
										}
										else
										{
											num = 0.26f;
										}
									}
									else
									{
										num = 0.11f;
									}
								}
								else if (text2 == "Light")
								{
									if (!(type == "Arctic"))
									{
										if (!(type == "Desert"))
										{
											if (!(type == "Jungle"))
											{
												if (!(type == "Stone"))
												{
													if (!(type == "Woodland"))
													{
														if (type == "Urban")
														{
															num = 0.35f;
														}
													}
													else
													{
														num = 0.2f;
													}
												}
												else
												{
													num = 0.2f;
												}
											}
											else
											{
												num = 0.2f;
											}
										}
										else
										{
											num = 0.3f;
										}
									}
									else
									{
										num = 0.4f;
									}
								}
								else if (text2 == "Rainbow")
								{
									if (!(type == "Arctic"))
									{
										if (!(type == "Desert"))
										{
											if (!(type == "Jungle"))
											{
												if (!(type == "Stone"))
												{
													if (!(type == "Woodland"))
													{
														if (type == "Urban")
														{
															num = 0.3f;
														}
													}
													else
													{
														num = 0.25f;
													}
												}
												else
												{
													num = 0.25f;
												}
											}
											else
											{
												num = 0.25f;
											}
										}
										else
										{
											num = 0.25f;
										}
									}
									else
									{
										num = 0.25f;
									}
								}
								return num;
							}
							if (text2 != null)
							{
								if (text2 == "Arctic")
								{
									if (!(type == "Arctic"))
									{
										if (!(type == "Desert"))
										{
											if (!(type == "Jungle"))
											{
												if (!(type == "Stone"))
												{
													if (!(type == "Woodland"))
													{
														if (type == "Urban")
														{
															num = 0.32f;
														}
													}
													else
													{
														num = 0.22f;
													}
												}
												else
												{
													num = 0.39f;
												}
											}
											else
											{
												num = 0.22f;
											}
										}
										else
										{
											num = 0.22f;
										}
									}
									else
									{
										num = 0.54f;
									}
								}
								else if (text2 == "Desert")
								{
									if (!(type == "Arctic"))
									{
										if (!(type == "Desert"))
										{
											if (!(type == "Jungle"))
											{
												if (!(type == "Stone"))
												{
													if (!(type == "Woodland"))
													{
														if (type == "Urban")
														{
															num = 0.32f;
														}
													}
													else
													{
														num = 0.39f;
													}
												}
												else
												{
													num = 0.22f;
												}
											}
											else
											{
												num = 0.39f;
											}
										}
										else
										{
											num = 0.54f;
										}
									}
									else
									{
										num = 0.22f;
									}
								}
								else if (text2 == "Jungle")
								{
									if (!(type == "Arctic"))
									{
										if (!(type == "Desert"))
										{
											if (!(type == "Jungle"))
											{
												if (!(type == "Stone"))
												{
													if (!(type == "Woodland"))
													{
														if (type == "Urban")
														{
															num = 0.32f;
														}
													}
													else
													{
														num = 0.48f;
													}
												}
												else
												{
													num = 0.22f;
												}
											}
											else
											{
												num = 0.54f;
											}
										}
										else
										{
											num = 0.39f;
										}
									}
									else
									{
										num = 0.22f;
									}
								}
								else if (text2 == "Stone")
								{
									if (!(type == "Arctic"))
									{
										if (!(type == "Desert"))
										{
											if (!(type == "Jungle"))
											{
												if (!(type == "Stone"))
												{
													if (!(type == "Woodland"))
													{
														if (type == "Urban")
														{
															num = 0.37f;
														}
													}
													else
													{
														num = 0.22f;
													}
												}
												else
												{
													num = 0.54f;
												}
											}
											else
											{
												num = 0.22f;
											}
										}
										else
										{
											num = 0.22f;
										}
									}
									else
									{
										num = 0.39f;
									}
								}
								else if (text2 == "Woodland")
								{
									if (!(type == "Arctic"))
									{
										if (!(type == "Desert"))
										{
											if (!(type == "Jungle"))
											{
												if (!(type == "Stone"))
												{
													if (!(type == "Woodland"))
													{
														if (type == "Urban")
														{
															num = 0.32f;
														}
													}
													else
													{
														num = 0.54f;
													}
												}
												else
												{
													num = 0.22f;
												}
											}
											else
											{
												num = 0.48f;
											}
										}
										else
										{
											num = 0.39f;
										}
									}
									else
									{
										num = 0.22f;
									}
								}
								else if (text2 == "Urban")
								{
									if (!(type == "Arctic"))
									{
										if (!(type == "Desert"))
										{
											if (!(type == "Jungle"))
											{
												if (!(type == "Stone"))
												{
													if (!(type == "Woodland"))
													{
														if (type == "Urban")
														{
															num = 0.54f;
														}
													}
													else
													{
														num = 0.32f;
													}
												}
												else
												{
													num = 0.37f;
												}
											}
											else
											{
												num = 0.32f;
											}
										}
										else
										{
											num = 0.32f;
										}
									}
									else
									{
										num = 0.32f;
									}
								}
								else if (text2 == "Multi")
								{
									if (!(type == "Arctic"))
									{
										if (!(type == "Desert"))
										{
											if (!(type == "Jungle"))
											{
												if (!(type == "Stone"))
												{
													if (!(type == "Woodland"))
													{
														if (type == "Urban")
														{
															num = 0.35f;
														}
													}
													else
													{
														num = 0.49f;
													}
												}
												else
												{
													num = 0.37f;
												}
											}
											else
											{
												num = 0.47f;
											}
										}
										else
										{
											num = 0.49f;
										}
									}
									else
									{
										num = 0.32f;
									}
								}
								string tagValue = CamoPresets.GetTagValue(text, 2);
								if (tagValue != null)
								{
									if (tagValue == "Med")
									{
										num *= 1.2f;
									}
									else if (tagValue == "High")
									{
										num *= 1.33f;
									}
								}
								TechLevel techLevel = apparel.def.techLevel;
								if (techLevel != TechLevel.Spacer)
								{
									if (techLevel == TechLevel.Ultra)
									{
										num *= 1.1f;
									}
								}
								else
								{
									num *= 1.05f;
								}
								return num;
							}
						}
					}
					return num;
				}
			}
			return num;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00005EDC File Offset: 0x000040DC
		internal static string GetTagValue(string valuesStr, int position)
		{
			char[] separator = new char[]
			{
				'_'
			};
			string[] array = valuesStr.Split(separator);
			if (array[position] != null)
			{
				return array[position];
			}
			return null;
		}
	}
}
