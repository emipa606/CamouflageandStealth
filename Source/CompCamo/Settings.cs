using UnityEngine;
using Verse;

namespace CompCamo;

public class Settings : ModSettings
{
    public bool AllowNpcCamo = true;

    public float BestChance = 85f;

    public bool DevTesting;

    public bool DoCheckFlash = true;

    public bool DoCheckLight = true;

    public bool DoCheckTemp = true;

    public bool DoCheckWeather = true;

    public bool ForceActive;

    public bool ForcePassive;

    public bool ForceStealth;

    public float RelPct = 100f;

    public bool ShowMoteMessages;

    public bool ShowOverlay;

    public bool ShowTerrainLogs;

    public bool useDebug;

    public void DoWindowContents(Rect canvas)
    {
        const float num = 8f;
        var listingStandard = new Listing_Standard
        {
            ColumnWidth = canvas.width
        };
        listingStandard.Begin(canvas);
        listingStandard.Gap(num);
        listingStandard.CheckboxLabeled("CompCamo.ShowOverlay".Translate(), ref ShowOverlay);
        listingStandard.Gap(num);
        checked
        {
            listingStandard.Label("CompCamo.RelativeCamo".Translate() + "  " + (int)RelPct);
            RelPct = (int)listingStandard.Slider((int)RelPct, 50f, 200f);
            listingStandard.Gap(num);
            listingStandard.CheckboxLabeled("CompCamo.DoCheckFlash".Translate(), ref DoCheckFlash);
            listingStandard.Gap(num);
            listingStandard.CheckboxLabeled("CompCamo.DoCheckWeather".Translate(), ref DoCheckWeather);
            listingStandard.Gap(num);
            listingStandard.CheckboxLabeled("CompCamo.DoCheckLight".Translate(), ref DoCheckLight);
            listingStandard.Gap(num);
            listingStandard.CheckboxLabeled("CompCamo.DoCheckTemp".Translate(), ref DoCheckTemp);
            listingStandard.Gap(num);
            if (Prefs.DevMode)
            {
                listingStandard.Gap(24f);
                listingStandard.Label("CompCamo.DebugTip".Translate());
                Text.Font = GameFont.Small;
                listingStandard.Gap(num);
                listingStandard.CheckboxLabeled("CompCamo.UseDebug".Translate(), ref useDebug);
                listingStandard.Gap(num);
                if (useDebug)
                {
                    listingStandard.CheckboxLabeled("CompCamo.ShowMoteMsgs".Translate(), ref ShowMoteMessages);
                    listingStandard.Gap(num);
                    listingStandard.CheckboxLabeled("CompCamo.ShowTerrainLogs".Translate(), ref ShowTerrainLogs);
                    listingStandard.Gap(num);
                    listingStandard.CheckboxLabeled("CompCamo.AllowNPCCamo".Translate(), ref AllowNpcCamo);
                    if (DevTesting)
                    {
                        listingStandard.Gap(24f);
                        listingStandard.CheckboxLabeled("CompCamo.forceActive".Translate(), ref ForceActive);
                        listingStandard.Gap(num);
                        if (ForceActive)
                        {
                            ForcePassive = false;
                            listingStandard.CheckboxLabeled("CompCamo.forceStealth".Translate(), ref ForceStealth);
                            listingStandard.Gap(num);
                        }
                        else
                        {
                            ForceStealth = false;
                            listingStandard.CheckboxLabeled("CompCamo.forcePassive".Translate(), ref ForcePassive);
                            listingStandard.Gap(num);
                            if (ForcePassive)
                            {
                                ForceActive = false;
                                ForceStealth = false;
                            }
                        }

                        listingStandard.Label("CompCamo.bestChance".Translate() + "  " + (int)BestChance);
                        BestChance = (int)listingStandard.Slider((int)BestChance, 75f, 95f);
                        listingStandard.Gap(num);
                    }
                }
            }

            if (Controller.CurrentVersion != null)
            {
                listingStandard.Gap();
                GUI.contentColor = Color.gray;
                listingStandard.Label("CompCamo.ModVersion".Translate(Controller.CurrentVersion));
                GUI.contentColor = Color.white;
            }

            listingStandard.End();
        }
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref ShowOverlay, "ShowOverlay");
        Scribe_Values.Look(ref RelPct, "RelPct", 100f);
        Scribe_Values.Look(ref DoCheckFlash, "DoCheckFlash", true);
        Scribe_Values.Look(ref DoCheckWeather, "DoCheckWeather", true);
        Scribe_Values.Look(ref DoCheckLight, "DoCheckLight", true);
        Scribe_Values.Look(ref DoCheckTemp, "DoCheckTemp", true);
        Scribe_Values.Look(ref useDebug, "useDebug");
        Scribe_Values.Look(ref ShowMoteMessages, "ShowMoteMsgs");
        Scribe_Values.Look(ref ShowTerrainLogs, "ShowTerrainLogs");
        Scribe_Values.Look(ref AllowNpcCamo, "AllowNPCCamo", true);
        Scribe_Values.Look(ref ForceActive, "forceActive");
        Scribe_Values.Look(ref ForceStealth, "forceStealth");
        Scribe_Values.Look(ref ForcePassive, "forcePassive");
        Scribe_Values.Look(ref BestChance, "bestChance", 85f);
    }
}