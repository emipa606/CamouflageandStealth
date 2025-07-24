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
                text = getMoteToUse(activeCamoEff);
                b = true;
            }
        }

        if (!b && CamoGearUtility.GetCurCamoEff(pawn, out _, out var num) && num > 0f)
        {
            text = getMoteToUse(num);
            b = true;
        }

        if (b && text != "")
        {
            doCamoOverlay(pawn, text);
        }
    }

    private static string getMoteToUse(float camoEff)
    {
        var result = "";
        switch (camoEff)
        {
            case < 0.25f:
                result = "Mote_CASPoor";
                break;
            case < 0.5f:
                result = "Mote_CASAverage";
                break;
            case < 0.8f:
                result = "Mote_CASGood";
                break;
            case >= 0.8f:
                result = "Mote_CASExcellent";
                break;
        }

        return result;
    }

    private static void doCamoOverlay(Pawn pawn, string camoMote)
    {
        var mote = (Mote)ThingMaker.MakeThing(ThingDef.Named(camoMote));
        mote.Attach(pawn);
        if (pawn.Position.InBounds(pawn.Map))
        {
            GenSpawn.Spawn(mote, pawn.Position, pawn.Map);
        }
    }
}