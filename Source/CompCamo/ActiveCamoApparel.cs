using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace CompCamo
{
	// Token: 0x02000005 RID: 5
	[StaticConstructorOnStartup]
	public class ActiveCamoApparel : Apparel
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000027 RID: 39 RVA: 0x000038FC File Offset: 0x00001AFC
		public float Energy
		{
			get
			{
				return this.energy;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000028 RID: 40 RVA: 0x00003904 File Offset: 0x00001B04
		public bool IsActiveCamo
		{
			get
			{
				return this.ActiveCamo && this.CamoState == 1;
			}
		}

		// Token: 0x06000029 RID: 41 RVA: 0x0000391C File Offset: 0x00001B1C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.energy, "energy", 0f, false);
			Scribe_Values.Look<int>(ref this.ticksToReset, "ticksToReset", -1, false);
			Scribe_Values.Look<bool>(ref this.ActiveCamo, "ActiveCamo", false, false);
			Scribe_Values.Look<int>(ref this.CamoState, "CamoState", 0, false);
		}

		// Token: 0x0600002A RID: 42 RVA: 0x0000397B File Offset: 0x00001B7B
		public override IEnumerable<Gizmo> GetWornGizmos()
		{
			Pawn wearer = base.Wearer;
			if (Find.Selector.SingleSelectedThing == wearer || Find.Selector.SingleSelectedThing == this)
			{
				if (Find.Selector.SingleSelectedThing == wearer && (wearer?.Map) != null)
				{
					yield return new Command_Toggle
					{
						icon = ContentFinder<Texture2D>.Get(this.ActiveCamoIconPath, true),
						defaultLabel = Translator.Translate("CompCamo.ActiveCamoLabel"),
						defaultDesc = Translator.Translate("CompCamo.ActiveCamoDesc"),
						isActive = (() => this.ActiveCamo && this.CamoState == 1),
						toggleAction = delegate()
						{
							this.ToggleActiveCamo(this.ActiveCamo);
						}
					};
				}
				yield return new Gizmo_EnergyActiveCamoStatus
				{
					camo = this
				};
				yield break;
			}
			yield break;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x0000398C File Offset: 0x00001B8C
		public override void Tick()
		{
			base.Tick();
			if (base.Wearer == null)
			{
				this.energy = 0f;
				this.ActiveCamo = false;
				this.CamoState = 0;
				return;
			}
			if (this.CamoState == 2)
			{
				this.ticksToReset--;
				if (this.ticksToReset <= 0)
				{
					this.Reset();
				}
			}
			else if (this.CamoState == 1)
			{
				this.energy -= ThingCompUtility.TryGetComp<CompGearCamo>(this).Props.CamoEnergyGainPerTick / 700f;
			}
			else if (this.CamoState == 0)
			{
				this.energy += ThingCompUtility.TryGetComp<CompGearCamo>(this).Props.CamoEnergyGainPerTick / 200f;
			}
			if (this.energy > ThingCompUtility.TryGetComp<CompGearCamo>(this).Props.CamoEnergyMax)
			{
				this.energy = ThingCompUtility.TryGetComp<CompGearCamo>(this).Props.CamoEnergyMax;
				return;
			}
			if (this.energy <= 0f && this.CamoState != 2)
			{
				this.Break();
			}
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00003A8D File Offset: 0x00001C8D
		public override void TickRare()
		{
			base.TickRare();
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00003A95 File Offset: 0x00001C95
		public void ToggleActiveCamo(bool flag)
		{
			if (this.CamoState != 2)
			{
				this.ActiveCamo = !flag;
				if (this.ActiveCamo)
				{
					this.CamoState = 1;
					return;
				}
				this.CamoState = 0;
			}
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00003AC1 File Offset: 0x00001CC1
		public override float GetSpecialApparelScoreOffset()
		{
			return ThingCompUtility.TryGetComp<CompGearCamo>(this).Props.CamoEnergyMax * this.ApparelScorePerEnergyMax;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00003ADC File Offset: 0x00001CDC
		public override bool CheckPreAbsorbDamage(DamageInfo dinfo)
		{
			if (this.CamoState == 0)
			{
				return false;
			}
			if (dinfo.Def == DamageDefOf.EMP)
			{
				this.energy = 0f;
				this.Break();
				return false;
			}
			DamageDef named = DefDatabase<DamageDef>.GetNamed("GGHaywireEMP", false);
			if (named != null && dinfo.Def == named)
			{
				this.energy = 0f;
				this.Break();
				return false;
			}
			return false;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00003B44 File Offset: 0x00001D44
		public void Break()
		{
			Pawn wearer = base.Wearer;
			if (wearer != null && (wearer?.Map) != null)
			{
				SoundStarter.PlayOneShot(SoundDefOf.EnergyShield_Broken, new TargetInfo(wearer.Position, wearer.Map, false));
				MoteMaker.MakeStaticMote(GenThing.TrueCenter(wearer), wearer.Map, ThingDefOf.Mote_ExplosionFlash, 12f);
				for (int i = 0; i < 6; i++)
				{
					MoteMaker.ThrowDustPuff(GenThing.TrueCenter(wearer) + Vector3Utility.HorizontalVectorFromAngle((float)Rand.Range(0, 360)) * Rand.Range(0.3f, 0.6f), wearer.Map, Rand.Range(0.8f, 1.2f));
				}
			}
			this.energy = 0f;
			this.ticksToReset = this.StartingTicksToReset;
			this.ActiveCamo = false;
			this.CamoState = 2;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00003C28 File Offset: 0x00001E28
		public void Reset()
		{
			Pawn wearer = base.Wearer;
			if (wearer.Spawned)
			{
				SoundStarter.PlayOneShot(SoundDefOf.EnergyShield_Reset, new TargetInfo(wearer.Position, wearer.Map, false));
				MoteMaker.ThrowLightningGlow(GenThing.TrueCenter(wearer), wearer.Map, 3f);
			}
			this.ticksToReset = -1;
			this.energy = this.EnergyOnReset;
			this.ActiveCamo = false;
			this.CamoState = 0;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00003C9C File Offset: 0x00001E9C
		public override bool AllowVerbCast(IntVec3 root, Map map, LocalTargetInfo targ, Verb v)
		{
			return true;
		}

		// Token: 0x04000007 RID: 7
		public float energy;

		// Token: 0x04000008 RID: 8
		public int ticksToReset = -1;

		// Token: 0x04000009 RID: 9
		public int StartingTicksToReset = 2500;

		// Token: 0x0400000A RID: 10
		public float EnergyOnReset = 0.2f;

		// Token: 0x0400000B RID: 11
		public float ApparelScorePerEnergyMax = 0.25f;

		// Token: 0x0400000C RID: 12
		public int CamoState;

		// Token: 0x0400000D RID: 13
		public bool ActiveCamo;

		// Token: 0x0400000E RID: 14
		[NoTranslate]
		private string ActiveCamoIconPath = "Things/Special/ActiveCamoIcon";
	}
}
