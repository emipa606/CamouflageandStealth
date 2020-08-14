using System;
using RimWorld;
using Verse;

namespace CompCamo
{
	// Token: 0x0200000A RID: 10
	public class CamoDefGet
	{
		// Token: 0x06000053 RID: 83 RVA: 0x00005F24 File Offset: 0x00004124
		public static string GetCamoDefBiome(BiomeDef biome)
		{
			if (biome.HasModExtension<CompCamoDefs>())
			{
				string text = biome?.GetModExtension<CompCamoDefs>().CamoType;
				if (CamoGearUtility.CamoTypes().Contains(text))
				{
					return text;
				}
			}
			return "notDefined";
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00005F60 File Offset: 0x00004160
		public static string GetCamoDefTerrain(TerrainDef terrain)
		{
			if (terrain.HasModExtension<CompCamoDefs>())
			{
				string text = terrain?.GetModExtension<CompCamoDefs>().CamoType;
				if (CamoGearUtility.CamoTypes().Contains(text))
				{
					return text;
				}
			}
			return "notDefined";
		}
	}
}
