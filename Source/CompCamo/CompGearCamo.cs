using RimWorld;
using Verse;

namespace CompCamo
{
    // Token: 0x0200000B RID: 11
    public class CompGearCamo : ThingComp
    {
        // Token: 0x17000007 RID: 7
        // (get) Token: 0x06000056 RID: 86 RVA: 0x00005FA3 File Offset: 0x000041A3
        public CompProperties_GearCamo Props => (CompProperties_GearCamo) props;

        // Token: 0x06000057 RID: 87 RVA: 0x00005FB0 File Offset: 0x000041B0
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
}