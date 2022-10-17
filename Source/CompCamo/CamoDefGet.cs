using RimWorld;
using Verse;

namespace CompCamo;

public class CamoDefGet
{
    public static string GetCamoDefBiome(BiomeDef biome)
    {
        if (!biome.HasModExtension<CompCamoDefs>())
        {
            return "notDefined";
        }

        var text = biome.GetModExtension<CompCamoDefs>().CamoType;
        return CamoGearUtility.CamoTypes().Contains(text) ? text : "notDefined";
    }

    public static string GetCamoDefTerrain(TerrainDef terrain)
    {
        if (!terrain.HasModExtension<CompCamoDefs>())
        {
            return "notDefined";
        }

        var text = terrain.GetModExtension<CompCamoDefs>().CamoType;
        return CamoGearUtility.CamoTypes().Contains(text) ? text : "notDefined";
    }
}