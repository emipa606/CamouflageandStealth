using UnityEngine;
using Verse;

namespace CompCamo;

public class Settings : ModSettings
{
    public bool AllowNPCCamo = true;

    public float bestChance = 85f;

    public bool devTesting;

    public bool DoCheckFlash = true;

    public bool DoCheckLight = true;

    public bool DoCheckTemp = true;

    public bool DoCheckWeather = true;

    public bool forceActive;

    public bool forcePassive;

    public bool forceStealth;

    public float RelPct = 100f;

    public bool ShowMoteMsgs;

    public bool ShowOverlay;

    public bool ShowTerrainLogs;

    public bool useDebug;

    public void DoWindowContents(Rect canvas)
    {
        var num = 8f;
        var listing_Standard = new Listing_Standard
        {
            ColumnWidth = canvas.width
        };
        listing_Standard.Begin(canvas);
        listing_Standard.Gap(num);
        listing_Standard.CheckboxLabeled("CompCamo.ShowOverlay".Translate(), ref ShowOverlay);
        listing_Standard.Gap(num);
        checked
        {
            listing_Standard.Label("CompCamo.RelativeCamo".Translate() + "  " + (int)RelPct);
            RelPct = (int)listing_Standard.Slider((int)RelPct, 50f, 200f);
            listing_Standard.Gap(num);
            listing_Standard.CheckboxLabeled("CompCamo.DoCheckFlash".Translate(), ref DoCheckFlash);
            listing_Standard.Gap(num);
            listing_Standard.CheckboxLabeled("CompCamo.DoCheckWeather".Translate(), ref DoCheckWeather);
            listing_Standard.Gap(num);
            listing_Standard.CheckboxLabeled("CompCamo.DoCheckLight".Translate(), ref DoCheckLight);
            listing_Standard.Gap(num);
            listing_Standard.CheckboxLabeled("CompCamo.DoCheckTemp".Translate(), ref DoCheckTemp);
            listing_Standard.Gap(num);
            if (Prefs.DevMode)
            {
                listing_Standard.Gap(24f);
                listing_Standard.Label("CompCamo.DebugTip".Translate());
                Text.Font = GameFont.Small;
                listing_Standard.Gap(num);
                listing_Standard.CheckboxLabeled("CompCamo.UseDebug".Translate(), ref useDebug);
                listing_Standard.Gap(num);
                if (useDebug)
                {
                    listing_Standard.CheckboxLabeled("CompCamo.ShowMoteMsgs".Translate(), ref ShowMoteMsgs);
                    listing_Standard.Gap(num);
                    listing_Standard.CheckboxLabeled("CompCamo.ShowTerrainLogs".Translate(), ref ShowTerrainLogs);
                    listing_Standard.Gap(num);
                    listing_Standard.CheckboxLabeled("CompCamo.AllowNPCCamo".Translate(), ref AllowNPCCamo);
                    if (devTesting)
                    {
                        listing_Standard.Gap(24f);
                        listing_Standard.CheckboxLabeled("CompCamo.forceActive".Translate(), ref forceActive);
                        listing_Standard.Gap(num);
                        if (forceActive)
                        {
                            forcePassive = false;
                            listing_Standard.CheckboxLabeled("CompCamo.forceStealth".Translate(), ref forceStealth);
                            listing_Standard.Gap(num);
                        }
                        else
                        {
                            forceStealth = false;
                            listing_Standard.CheckboxLabeled("CompCamo.forcePassive".Translate(), ref forcePassive);
                            listing_Standard.Gap(num);
                            if (forcePassive)
                            {
                                forceActive = false;
                                forceStealth = false;
                            }
                        }

                        listing_Standard.Label("CompCamo.bestChance".Translate() + "  " + (int)bestChance);
                        bestChance = (int)listing_Standard.Slider((int)bestChance, 75f, 95f);
                        listing_Standard.Gap(num);
                    }
                }
            }

            if (Controller.currentVersion != null)
            {
                listing_Standard.Gap();
                GUI.contentColor = Color.gray;
                listing_Standard.Label("CompCamo.ModVersion".Translate(Controller.currentVersion));
                GUI.contentColor = Color.white;
            }

            listing_Standard.End();
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
        Scribe_Values.Look(ref ShowMoteMsgs, "ShowMoteMsgs");
        Scribe_Values.Look(ref ShowTerrainLogs, "ShowTerrainLogs");
        Scribe_Values.Look(ref AllowNPCCamo, "AllowNPCCamo", true);
        Scribe_Values.Look(ref forceActive, "forceActive");
        Scribe_Values.Look(ref forceStealth, "forceStealth");
        Scribe_Values.Look(ref forcePassive, "forcePassive");
        Scribe_Values.Look(ref bestChance, "bestChance", 85f);
    }
}