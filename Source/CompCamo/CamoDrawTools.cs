using Verse;

namespace CompCamo;

public static class CamoDrawTools
{
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

    public static void DoCamoOverlay(Pawn pawn, string CamoMote)
    {
        var mote = (Mote)ThingMaker.MakeThing(ThingDef.Named(CamoMote));
        mote.Attach(pawn);
        if (pawn.Position.InBounds(pawn.Map))
        {
            GenSpawn.Spawn(mote, pawn.Position, pawn.Map);
        }
    }
}