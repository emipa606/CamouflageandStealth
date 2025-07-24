using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace CompCamo;

[StaticConstructorOnStartup]
public class ActiveCamoApparel : Apparel
{
    [NoTranslate] private readonly string ActiveCamoIconPath = "Things/Special/ActiveCamoIcon";

    private readonly float ApparelScorePerEnergyMax = 0.25f;

    private readonly float EnergyOnReset = 0.2f;

    private readonly SoundDef EnergyShield_Broken = SoundDef.Named("EnergyShield_Broken");

    private readonly int StartingTicksToReset = 2500;

    private bool ActiveCamo;

    private int CamoState;

    public float energy;

    private int ticksToReset = -1;

    public float Energy => energy;

    public bool IsActiveCamo => ActiveCamo && CamoState == 1;

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref energy, "energy");
        Scribe_Values.Look(ref ticksToReset, "ticksToReset", -1);
        Scribe_Values.Look(ref ActiveCamo, "ActiveCamo");
        Scribe_Values.Look(ref CamoState, "CamoState");
    }

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
                toggleAction = delegate { toggleActiveCamo(ActiveCamo); }
            };
        }

        yield return new Gizmo_EnergyActiveCamoStatus
        {
            Camo = this
        };
    }

    protected override void Tick()
    {
        base.Tick();
        if (Wearer == null)
        {
            energy = 0f;
            ActiveCamo = false;
            CamoState = 0;
            return;
        }

        switch (CamoState)
        {
            case 2:
            {
                ticksToReset--;
                if (ticksToReset <= 0)
                {
                    reset();
                }

                break;
            }
            case 1:
                energy -= this.TryGetComp<CompGearCamo>().Props.CamoEnergyGainPerTick / 700f;
                break;
            case 0:
                energy += this.TryGetComp<CompGearCamo>().Props.CamoEnergyGainPerTick / 200f;
                break;
        }

        if (energy > this.TryGetComp<CompGearCamo>().Props.CamoEnergyMax)
        {
            energy = this.TryGetComp<CompGearCamo>().Props.CamoEnergyMax;
            return;
        }

        if (energy <= 0f && CamoState != 2)
        {
            breakCamo();
        }
    }


    private void toggleActiveCamo(bool flag)
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

    public override float GetSpecialApparelScoreOffset()
    {
        return this.TryGetComp<CompGearCamo>().Props.CamoEnergyMax * ApparelScorePerEnergyMax;
    }

    public override bool CheckPreAbsorbDamage(DamageInfo dinfo)
    {
        if (CamoState == 0)
        {
            return false;
        }

        if (dinfo.Def == DamageDefOf.EMP)
        {
            energy = 0f;
            breakCamo();
            return false;
        }

        var named = DefDatabase<DamageDef>.GetNamed("GGHaywireEMP", false);
        if (named == null || dinfo.Def != named)
        {
            return false;
        }

        energy = 0f;
        breakCamo();
        return false;
    }

    private void breakCamo()
    {
        var wearer = Wearer;
        if (wearer?.Map != null)
        {
            EnergyShield_Broken.PlayOneShot(new TargetInfo(wearer.Position, wearer.Map));
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

    private void reset()
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

    public override bool AllowVerbCast(Verb v)
    {
        return true;
    }
}