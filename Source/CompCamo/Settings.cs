using System;
using UnityEngine;
using Verse;

namespace CompCamo
{
	// Token: 0x0200001C RID: 28
	public class Settings : ModSettings
	{
		// Token: 0x06000081 RID: 129 RVA: 0x00006BE0 File Offset: 0x00004DE0
		public void DoWindowContents(Rect canvas)
		{
			float num = 8f;
            Listing_Standard listing_Standard = new Listing_Standard
            {
                ColumnWidth = canvas.width
            };
            listing_Standard.Begin(canvas);
			listing_Standard.Gap(num);
			listing_Standard.CheckboxLabeled(Translator.Translate("CompCamo.ShowOverlay"), ref this.ShowOverlay, null);
			listing_Standard.Gap(num);
			checked
			{
				listing_Standard.Label(Translator.Translate("CompCamo.RelativeCamo") + "  " + (int)this.RelPct, -1f, null);
				this.RelPct = (float)((int)listing_Standard.Slider((float)((int)this.RelPct), 50f, 200f));
				listing_Standard.Gap(num);
				listing_Standard.CheckboxLabeled(Translator.Translate("CompCamo.DoCheckFlash"), ref this.DoCheckFlash, null);
				listing_Standard.Gap(num);
				listing_Standard.CheckboxLabeled(Translator.Translate("CompCamo.DoCheckWeather"), ref this.DoCheckWeather, null);
				listing_Standard.Gap(num);
				listing_Standard.CheckboxLabeled(Translator.Translate("CompCamo.DoCheckLight"), ref this.DoCheckLight, null);
				listing_Standard.Gap(num);
				listing_Standard.CheckboxLabeled(Translator.Translate("CompCamo.DoCheckTemp"), ref this.DoCheckTemp, null);
				listing_Standard.Gap(num);
				if (Prefs.DevMode)
				{
					listing_Standard.Gap(24f);
					listing_Standard.Label(Translator.Translate("CompCamo.DebugTip"), -1f, null);
					Text.Font = GameFont.Small;
					listing_Standard.Gap(num);
					listing_Standard.CheckboxLabeled(Translator.Translate("CompCamo.UseDebug"), ref this.useDebug, null);
					listing_Standard.Gap(num);
					if (this.useDebug)
					{
						listing_Standard.CheckboxLabeled(Translator.Translate("CompCamo.ShowMoteMsgs"), ref this.ShowMoteMsgs, null);
						listing_Standard.Gap(num);
						listing_Standard.CheckboxLabeled(Translator.Translate("CompCamo.ShowTerrainLogs"), ref this.ShowTerrainLogs, null);
						listing_Standard.Gap(num);
						listing_Standard.CheckboxLabeled(Translator.Translate("CompCamo.AllowNPCCamo"), ref this.AllowNPCCamo, null);
						if (this.devTesting)
						{
							listing_Standard.Gap(24f);
							listing_Standard.CheckboxLabeled(Translator.Translate("CompCamo.forceActive"), ref this.forceActive, null);
							listing_Standard.Gap(num);
							if (this.forceActive)
							{
								this.forcePassive = false;
								listing_Standard.CheckboxLabeled(Translator.Translate("CompCamo.forceStealth"), ref this.forceStealth, null);
								listing_Standard.Gap(num);
							}
							else
							{
								this.forceStealth = false;
								listing_Standard.CheckboxLabeled(Translator.Translate("CompCamo.forcePassive"), ref this.forcePassive, null);
								listing_Standard.Gap(num);
								if (this.forcePassive)
								{
									this.forceActive = false;
									this.forceStealth = false;
								}
							}
							listing_Standard.Label(Translator.Translate("CompCamo.bestChance") + "  " + (int)this.bestChance, -1f, null);
							this.bestChance = (float)((int)listing_Standard.Slider((float)((int)this.bestChance), 75f, 95f));
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
			Scribe_Values.Look<bool>(ref this.ShowOverlay, "ShowOverlay", false, false);
			Scribe_Values.Look<float>(ref this.RelPct, "RelPct", 100f, false);
			Scribe_Values.Look<bool>(ref this.DoCheckFlash, "DoCheckFlash", true, false);
			Scribe_Values.Look<bool>(ref this.DoCheckWeather, "DoCheckWeather", true, false);
			Scribe_Values.Look<bool>(ref this.DoCheckLight, "DoCheckLight", true, false);
			Scribe_Values.Look<bool>(ref this.DoCheckTemp, "DoCheckTemp", true, false);
			Scribe_Values.Look<bool>(ref this.useDebug, "useDebug", false, false);
			Scribe_Values.Look<bool>(ref this.ShowMoteMsgs, "ShowMoteMsgs", false, false);
			Scribe_Values.Look<bool>(ref this.ShowTerrainLogs, "ShowTerrainLogs", false, false);
			Scribe_Values.Look<bool>(ref this.AllowNPCCamo, "AllowNPCCamo", true, false);
			Scribe_Values.Look<bool>(ref this.forceActive, "forceActive", false, false);
			Scribe_Values.Look<bool>(ref this.forceStealth, "forceStealth", false, false);
			Scribe_Values.Look<bool>(ref this.forcePassive, "forcePassive", false, false);
			Scribe_Values.Look<float>(ref this.bestChance, "bestChance", 85f, false);
		}

		// Token: 0x04000028 RID: 40
		public bool ShowOverlay;

		// Token: 0x04000029 RID: 41
		public float RelPct = 100f;

		// Token: 0x0400002A RID: 42
		public bool DoCheckFlash = true;

		// Token: 0x0400002B RID: 43
		public bool DoCheckWeather = true;

		// Token: 0x0400002C RID: 44
		public bool DoCheckLight = true;

		// Token: 0x0400002D RID: 45
		public bool DoCheckTemp = true;

		// Token: 0x0400002E RID: 46
		public bool useDebug;

		// Token: 0x0400002F RID: 47
		public bool ShowMoteMsgs;

		// Token: 0x04000030 RID: 48
		public bool ShowTerrainLogs;

		// Token: 0x04000031 RID: 49
		public bool AllowNPCCamo = true;

		// Token: 0x04000032 RID: 50
		public bool forceActive;

		// Token: 0x04000033 RID: 51
		public bool forceStealth;

		// Token: 0x04000034 RID: 52
		public bool forcePassive;

		// Token: 0x04000035 RID: 53
		public float bestChance = 85f;

		// Token: 0x04000036 RID: 54
		public bool devTesting;
	}
}
