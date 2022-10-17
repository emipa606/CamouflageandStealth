using RimWorld;
using Verse;

namespace CompCamo;

public class CompGearCamo : ThingComp
{
    public CompProperties_GearCamo Props => (CompProperties_GearCamo)props;

    public override void PostSpawnSetup(bool respawningAfterLoad)
    {
        base.PostSpawnSetup(respawningAfterLoad);
        if (!respawningAfterLoad || !(Props.ActiveCamoEff > 0f) || !(Props.CamoEnergyMax > 0f))
        {
            return;
        }

        Thing thingWithComps = parent;
        if (thingWithComps.GetType() != typeof(ActiveCamoApparel) &&
            thingWithComps.def.thingClass == typeof(ActiveCamoApparel) && thingWithComps.Spawned &&
            thingWithComps.Map != null)
        {
            PawnCamoData.CorrectActiveApparel(thingWithComps as Apparel);
        }
    }
}