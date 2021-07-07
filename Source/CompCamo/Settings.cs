using UnityEngine;
using Verse;

namespace CompCamo
{
    // Token: 0x0200001C RID: 28
    public class Settings : ModSettings
    {
        // Token: 0x04000031 RID: 49
        public bool AllowNPCCamo = true;

        // Token: 0x04000035 RID: 53
        public float bestChance = 85f;

        // Token: 0x04000036 RID: 54
        public bool devTesting;

        // Token: 0x0400002A RID: 42
        public bool DoCheckFlash = true;

        // Token: 0x0400002C RID: 44
        public bool DoCheckLight = true;

        // Token: 0x0400002D RID: 45
        public bool DoCheckTemp = true;

        // Token: 0x0400002B RID: 43
        public bool DoCheckWeather = true;

        // Token: 0x04000032 RID: 50
        public bool forceActive;

        // Token: 0x04000034 RID: 52
        public bool forcePassive;

        // Token: 0x04000033 RID: 51
        public bool forceStealth;

        // Token: 0x04000029 RID: 41
        public float RelPct = 100f;

        // Token: 0x0400002F RID: 47
        public bool ShowMoteMsgs;

        // Token: 0x04000028 RID: 40
        public bool ShowOverlay;

        // Token: 0x04000030 RID: 48
        public bool ShowTerrainLogs;

        // Token: 0x0400002E RID: 46
        public bool useDebug;

        // Token: 0x06000081 RID: 129 RVA: 0x00006BE0 File Offset: 0x00004DE0
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
                listing_Standard.Label("CompCamo.RelativeCamo".Translate() + "  " + (int) RelPct);
                RelPct = (int) listing_Standard.Slider((int) RelPct, 50f, 200f);
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

                            listing_Standard.Label("CompCamo.bestChance".Translate() + "  " + (int) bestChance);
                            bestChance = (int) listing_Standard.Slider((int) bestChance, 75f, 95f);
                            listing_Standard.Gap(num);
                        }
                    }
                }

                listing_Standard.End();
            }
        }

        // Token: 0x06000082 RID: 130 RVA: 0x00006EFC File Offset: 0x000050FC
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
}