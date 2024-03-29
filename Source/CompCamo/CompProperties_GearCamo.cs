using Verse;

namespace CompCamo;

public class CompProperties_GearCamo : CompProperties
{
    public readonly float CamoEnergyGainPerTick = 0.1f;

    public readonly float CamoEnergyMax = 0.5f;
    public float ActiveCamoEff;

    public float ArcticCamoEff;

    public float DesertCamoEff;

    public float JungleCamoEff;

    public int StealthCamoChance;

    public float StoneCamoEff;

    public float UrbanCamoEff;

    public float WoodlandCamoEff;

    public CompProperties_GearCamo()
    {
        compClass = typeof(CompGearCamo);
    }
}