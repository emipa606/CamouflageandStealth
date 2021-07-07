using Verse;

namespace CompCamo
{
    // Token: 0x02000003 RID: 3
    public static class CamoDrawTools
    {
        // Token: 0x06000011 RID: 17 RVA: 0x00002740 File Offset: 0x00000940
        public static void DrawCamoOverlay(Pawn pawn)
        {
            var b = false;
            var text = "";
            if (CamoUtility.IsCamoActive(pawn, out var apparel) && apparel != null)
            {
                var activeCamoEff = apparel.TryGetComp<CompGearCamo>().Props.ActiveCamoEff;
                if (apparel.TryGetComp<CompGearCamo>().Props.StealthCamoChance > 0 && activeCamoEff > 0f)
                {
                    text = "Mote_CASStealth";
                    b = true;
                }
                else if (activeCamoEff > 0f)
                {
                    text = GetMoteToUse(activeCamoEff);
                    b = true;
                }
            }

            if (!b && CamoGearUtility.GetCurCamoEff(pawn, out _, out var num) && num > 0f)
            {
                text = GetMoteToUse(num);
                b = true;
            }

            if (b && text != "")
            {
                DoCamoOverlay(pawn, text);
            }
        }

        // Token: 0x06000012 RID: 18 RVA: 0x000027E8 File Offset: 0x000009E8
        public static string GetMoteToUse(float CamoEff)
        {
            var result = "";
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
            var mote = (Mote) ThingMaker.MakeThing(ThingDef.Named(CamoMote));
            mote.Attach(pawn);
            if (pawn.Position.InBounds(pawn.Map))
            {
                GenSpawn.Spawn(mote, pawn.Position, pawn.Map);
            }
        }
    }
}