using System;
using RimWorld;
using Verse;

namespace CompCamo
{
	// Token: 0x02000003 RID: 3
	public static class CamoDrawTools
	{
		// Token: 0x06000011 RID: 17 RVA: 0x00002740 File Offset: 0x00000940
		public static void DrawCamoOverlay(Pawn pawn)
		{
			bool flag = false;
			string text = "";
            if (CamoUtility.IsCamoActive(pawn, out Apparel apparel) && apparel != null)
            {
                float activeCamoEff = ThingCompUtility.TryGetComp<CompGearCamo>(apparel).Props.ActiveCamoEff;
                if (ThingCompUtility.TryGetComp<CompGearCamo>(apparel).Props.StealthCamoChance > 0 && activeCamoEff > 0f)
                {
                    text = "Mote_CASStealth";
                    flag = true;
                }
                else if (activeCamoEff > 0f)
                {
                    text = CamoDrawTools.GetMoteToUse(activeCamoEff);
                    flag = true;
                }
            }
            if (!flag && CamoGearUtility.GetCurCamoEff(pawn, out string text2, out float num) && num > 0f)
            {
                text = CamoDrawTools.GetMoteToUse(num);
                flag = true;
            }
            if (flag && text != "")
			{
				CamoDrawTools.DoCamoOverlay(pawn, text);
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000027E8 File Offset: 0x000009E8
		public static string GetMoteToUse(float CamoEff)
		{
			string result = "";
			if (CamoEff < 0.25f)
			{
				result = "Mote_CASPoor";
			}
			else if (CamoEff < 0.5f)
			{
				result = "Mote_CASAverage";
			}
			else if (CamoEff < 0.8f)
			{
				result = "Mote_CASGood";
			}
			else if (CamoEff >= 0.8f)
			{
				result = "Mote_CASExcellent";
			}
			return result;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x0000283C File Offset: 0x00000A3C
		public static void DoCamoOverlay(Pawn pawn, string CamoMote)
		{
			Mote mote = (Mote)ThingMaker.MakeThing(ThingDef.Named(CamoMote), null);
			mote.Attach(pawn);
			if (GenGrid.InBounds(pawn.Position, pawn.Map))
			{
				GenSpawn.Spawn(mote, pawn.Position, pawn.Map, 0);
			}
		}
	}
}
