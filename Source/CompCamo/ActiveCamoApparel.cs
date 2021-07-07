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
        // Token: 0x0400000E RID: 14
        [NoTranslate] private readonly string ActiveCamoIconPath = "Things/Special/ActiveCamoIcon";

        // Token: 0x0400000D RID: 13
        public bool ActiveCamo;

        // Token: 0x0400000B RID: 11
        public float ApparelScorePerEnergyMax = 0.25f;

        // Token: 0x0400000C RID: 12
        public int CamoState;

        // Token: 0x04000007 RID: 7
        public float energy;

        // Token: 0x0400000A RID: 10
        public float EnergyOnReset = 0.2f;

        // Token: 0x04000009 RID: 9
        public int StartingTicksToReset = 2500;

        // Token: 0x04000008 RID: 8
        public int ticksToReset = -1;

        // Token: 0x17000001 RID: 1
        // (get) Token: 0x06000027 RID: 39 RVA: 0x000038FC File Offset: 0x00001AFC
        public float Energy => energy;

        // Token: 0x17000002 RID: 2
        // (get) Token: 0x06000028 RID: 40 RVA: 0x00003904 File Offset: 0x00001B04
        public bool IsActiveCamo => ActiveCamo && CamoState == 1;

        // Token: 0x06000029 RID: 41 RVA: 0x0000391C File Offset: 0x00001B1C
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref energy, "energy");
            Scribe_Values.Look(ref ticksToReset, "ticksToReset", -1);
            Scribe_Values.Look(ref ActiveCamo, "ActiveCamo");
            Scribe_Values.Look(ref CamoState, "CamoState");
        }

        // Token: 0x0600002A RID: 42 RVA: 0x0000397B File Offset: 0x00001B7B
        public override IEnumerable<Gizmo> GetWornGizmos()
        {
            var wearer = Wearer;
            if (Find.Selector.SingleSelectedThing != wearer && Find.Selector.SingleSelectedThing != this)
            {
                yield break;
            }

            if (Find.Selector.SingleSelectedThing == wearer && wearer?.Map != null)
            {
                yield return new Command_Toggle
                {
                    icon = ContentFinder<Texture2D>.Get(ActiveCamoIconPath),
                    defaultLabel = "CompCamo.ActiveCamoLabel".Translate(),
                    defaultDesc = "CompCamo.ActiveCamoDesc".Translate(),
                    isActive = () => ActiveCamo && CamoState == 1,
                    toggleAction = delegate { ToggleActiveCamo(ActiveCamo); }
                };
            }

            yield return new Gizmo_EnergyActiveCamoStatus
            {
                camo = this
            };
        }

        // Token: 0x0600002B RID: 43 RVA: 0x0000398C File Offset: 0x00001B8C
        public override void Tick()
        {
            base.Tick();
            if (Wearer == null)
            {
                energy = 0f;
                ActiveCamo = false;
                CamoState = 0;
                return;
            }

            if (CamoState == 2)
            {
                ticksToReset--;
                if (ticksToReset <= 0)
                {
                    Reset();
                }
            }
            else if (CamoState == 1)
            {
                energy -= this.TryGetComp<CompGearCamo>().Props.CamoEnergyGainPerTick / 700f;
            }
            else if (CamoState == 0)
            {
                energy += this.TryGetComp<CompGearCamo>().Props.CamoEnergyGainPerTick / 200f;
            }

            if (energy > this.TryGetComp<CompGearCamo>().Props.CamoEnergyMax)
            {
                energy = this.TryGetComp<CompGearCamo>().Props.CamoEnergyMax;
                return;
            }

            if (energy <= 0f && CamoState != 2)
            {
                Break();
            }
        }

        // Token: 0x0600002C RID: 44 RVA: 0x00003A8D File Offset: 0x00001C8D

        // Token: 0x0600002D RID: 45 RVA: 0x00003A95 File Offset: 0x00001C95
        public void ToggleActiveCamo(bool flag)
        {
            if (CamoState == 2)
            {
                return;
            }

            ActiveCamo = !flag;
            if (ActiveCamo)
            {
                CamoState = 1;
                return;
            }

            CamoState = 0;
        }

        // Token: 0x0600002E RID: 46 RVA: 0x00003AC1 File Offset: 0x00001CC1
        public override float GetSpecialApparelScoreOffset()
        {
            return this.TryGetComp<CompGearCamo>().Props.CamoEnergyMax * ApparelScorePerEnergyMax;
        }

        // Token: 0x0600002F RID: 47 RVA: 0x00003ADC File Offset: 0x00001CDC
        public override bool CheckPreAbsorbDamage(DamageInfo dinfo)
        {
            if (CamoState == 0)
            {
                return false;
            }

            if (dinfo.Def == DamageDefOf.EMP)
            {
                energy = 0f;
                Break();
                return false;
            }

            var named = DefDatabase<DamageDef>.GetNamed("GGHaywireEMP", false);
            if (named == null || dinfo.Def != named)
            {
                return false;
            }

            energy = 0f;
            Break();
            return false;
        }

        // Token: 0x06000030 RID: 48 RVA: 0x00003B44 File Offset: 0x00001D44
        public void Break()
        {
            var wearer = Wearer;
            if (wearer?.Map != null)
            {
                SoundDefOf.EnergyShield_Broken.PlayOneShot(new TargetInfo(wearer.Position, wearer.Map));
                FleckMaker.Static(wearer.TrueCenter(), wearer.Map, FleckDefOf.ExplosionFlash, 12f);
                for (var i = 0; i < 6; i++)
                {
                    FleckMaker.ThrowDustPuff(
                        wearer.TrueCenter() + (Vector3Utility.HorizontalVectorFromAngle(Rand.Range(0, 360)) *
                                               Rand.Range(0.3f, 0.6f)), wearer.Map, Rand.Range(0.8f, 1.2f));
                }
            }

            energy = 0f;
            ticksToReset = StartingTicksToReset;
            ActiveCamo = false;
            CamoState = 2;
        }

        // Token: 0x06000031 RID: 49 RVA: 0x00003C28 File Offset: 0x00001E28
        public void Reset()
        {
            var wearer = Wearer;
            if (wearer.Spawned)
            {
                SoundDefOf.EnergyShield_Reset.PlayOneShot(new TargetInfo(wearer.Position, wearer.Map));
                FleckMaker.ThrowLightningGlow(wearer.TrueCenter(), wearer.Map, 3f);
            }

            ticksToReset = -1;
            energy = EnergyOnReset;
            ActiveCamo = false;
            CamoState = 0;
        }

        // Token: 0x06000032 RID: 50 RVA: 0x00003C9C File Offset: 0x00001E9C
        public override bool AllowVerbCast(Verb v)
        {
            return true;
        }
    }
}