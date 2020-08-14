using System;
using Verse;

namespace CompCamo
{
	// Token: 0x0200000C RID: 12
	public class CompProperties_GearCamo : CompProperties
	{
		// Token: 0x06000059 RID: 89 RVA: 0x00006052 File Offset: 0x00004252
		public CompProperties_GearCamo()
		{
			this.compClass = typeof(CompGearCamo);
		}

		// Token: 0x04000010 RID: 16
		public float ArcticCamoEff;

		// Token: 0x04000011 RID: 17
		public float DesertCamoEff;

		// Token: 0x04000012 RID: 18
		public float JungleCamoEff;

		// Token: 0x04000013 RID: 19
		public float StoneCamoEff;

		// Token: 0x04000014 RID: 20
		public float WoodlandCamoEff;

		// Token: 0x04000015 RID: 21
		public float UrbanCamoEff;

		// Token: 0x04000016 RID: 22
		public float ActiveCamoEff;

		// Token: 0x04000017 RID: 23
		public int StealthCamoChance;

		// Token: 0x04000018 RID: 24
		public float CamoEnergyMax = 0.5f;

		// Token: 0x04000019 RID: 25
		public float CamoEnergyGainPerTick = 0.1f;
	}
}
