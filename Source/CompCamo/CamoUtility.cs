using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;

namespace CompCamo
{
    // Token: 0x02000004 RID: 4
    public class CamoUtility
    {
        // Token: 0x06000014 RID: 20 RVA: 0x00002890 File Offset: 0x00000A90
        public static bool IsCamoActive(Pawn target, out Apparel ACItem)
        {
            ACItem = null;
            if (!CamoGearUtility.IsWearingActiveCamo(target, out Apparel apparel))
            {
                return false;
            }
            if (apparel != null && CamoUtility.ActiveCamoIsActive(apparel))
            {
                ACItem = apparel;
                return true;
            }
            return false;
        }

        // Token: 0x06000015 RID: 21 RVA: 0x000028BD File Offset: 0x00000ABD
        internal static bool ActiveCamoIsActive(Apparel AC)
        {
            return (AC as ActiveCamoApparel).IsActiveCamo;
        }

        // Token: 0x06000016 RID: 22 RVA: 0x000028CC File Offset: 0x00000ACC
        public static bool IsTargetHidden(Pawn target, Pawn seer)
        {
            if (target == null || seer == null)
            {
                Log.Message("Target/Seer Null", false);
                return false;
            }
            if (target != null && seer != null)
            {
                if (target == seer)
                {
                    return false;
                }
                if (CamoUtility.TryGetCamoHidValue(seer, target, out bool result))
                {
                    return result;
                }
            }
            if (target != null && target.Spawned)
            {
                bool flag = false;
                Apparel apparel = null;
                if (!CamoUtility.IsDebugMode() || !Controller.Settings.forcePassive)
                {
                    flag = CamoUtility.IsCamoActive(target, out Apparel apparel2);
                    if (apparel2 != null)
                    {
                        apparel = apparel2;
                    }
                }
                if ((!flag || (flag && seer.CurrentEffectiveVerb.IsMeleeAttack)) && CamoUtility.SimplyTooClose(seer, target))
                {
                    CamoUtility.TryAddCamoHidValue(seer, target, false);
                    return false;
                }
                if (flag || (CamoUtility.IsDebugMode() && Controller.Settings.forceActive))
                {
                    if (seer == null || !seer.Spawned)
                    {
                        return false;
                    }
                    if ((target?.Map) == null || (seer?.Map) == null || target.Map != seer.Map || (seer.InMentalState && target.InMentalState))
                    {
                        CamoUtility.TryAddCamoHidValue(seer, target, false);
                        return false;
                    }
                    if (!GenSight.LineOfSight(seer.Position, target.Position, seer.Map, false, null, 0, 0))
                    {
                        return true;
                    }
                    float num = 0.75f;
                    int num2 = 0;
                    bool flag2;
                    if (CamoUtility.IsDebugMode() && Controller.Settings.forceActive)
                    {
                        apparel = null;
                        flag2 = Controller.Settings.forceStealth;
                        if (flag2)
                        {
                            num2 = 5;
                        }
                    }
                    else
                    {
                        num = ThingCompUtility.TryGetComp<CompGearCamo>(apparel).Props.ActiveCamoEff;
                        num2 = ThingCompUtility.TryGetComp<CompGearCamo>(apparel).Props.StealthCamoChance;
                        flag2 = (num2 > 0 && num > 0f);
                    }
                    if (CamoUtility.CamoEffectWorked(target, seer, apparel, num, true, flag2, num2, out int chance, out float scaler))
                    {
                        CamoUtility.DoCamoMote(seer, target, true, chance, num, scaler);
                        CamoUtility.TryAddCamoHidValue(seer, target, true);
                        CamoAIUtility.CorrectLordForCamo(seer, target);
                        return true;
                    }
                    CamoUtility.DoCamoMote(seer, target, false, chance, num, scaler);
                    CamoUtility.TryAddCamoHidValue(seer, target, false);
                    return false;
                }
                else if (seer != null && seer.Spawned)
                {
                    if (!CamoGearUtility.GetCurCamoEff(target, out string str, out float camoEff))
                    {
                        CamoUtility.TryAddCamoHidValue(seer, target, false);
                        return false;
                    }
                    if (CamoUtility.IsDebugMode())
                    {
                        if (Controller.Settings.forcePassive)
                        {
                            camoEff = 0.75f;
                        }
                        Log.Message("Camo: " + str + " : " + camoEff.ToString("F2"), false);
                    }
                    if ((target?.Map) == null || (seer?.Map) == null || target.Map != seer.Map || (seer.InMentalState && target.InMentalState))
                    {
                        CamoUtility.TryAddCamoHidValue(seer, target, false);
                        return false;
                    }
                    if (!GenSight.LineOfSight(seer.Position, target.Position, seer.Map, false, null, 0, 0))
                    {
                        return true;
                    }
                    if (CamoUtility.CamoEffectWorked(target, seer, null, camoEff, false, false, 0, out int chance2, out float scaler2))
                    {
                        CamoUtility.DoCamoMote(seer, target, true, chance2, camoEff, scaler2);
                        CamoUtility.TryAddCamoHidValue(seer, target, true);
                        CamoAIUtility.CorrectLordForCamo(seer, target);
                        return true;
                    }
                    CamoUtility.DoCamoMote(seer, target, false, chance2, camoEff, scaler2);
                    CamoUtility.TryAddCamoHidValue(seer, target, false);
                    return false;
                }
            }
            return false;
        }

        // Token: 0x06000017 RID: 23 RVA: 0x00002BD8 File Offset: 0x00000DD8
        public static bool SimplyTooClose(Pawn seer, Pawn target)
        {
            if (seer != null && target != null && (seer?.Map) != null && (target?.Map) != null && seer.Map == target.Map && seer.Spawned && target.Spawned)
            {
                if (seer.Position == target.Position)
                {
                    return true;
                }
                if (seer.Position.InHorDistOf(target.Position, CamoUtility.NotPossibleMinDist))
                {
                    return true;
                }
            }
            return false;
        }

        // Token: 0x06000018 RID: 24 RVA: 0x00002C58 File Offset: 0x00000E58
        public static bool TryGetCamoHidValue(Pawn seer, Pawn target, out bool hid)
        {
            hid = false;
            if (seer != null)
            {
                PawnCamoData pawnCamoData = ThingCompUtility.TryGetComp<PawnCamoData>(seer);
                if (pawnCamoData != null)
                {
                    int ticksGame = Find.TickManager.TicksGame;
                    List<string> list = pawnCamoData?.PawnHidTickList;
                    if (list != null && list.Count > 0)
                    {
                        foreach (string valuesStr in list)
                        {
                            if (CamoGearUtility.GetIntValue(valuesStr, 1) + CamoUtility.TickElapse >= ticksGame)
                            {
                                int intValue = CamoGearUtility.GetIntValue(valuesStr, 0);
                                if (target != null && target != null)
                                {
                                    int thingIDNumber = target.thingIDNumber;
                                    if (intValue == target.thingIDNumber)
                                    {
                                        string strValue = CamoGearUtility.GetStrValue(valuesStr, 2);
                                        hid = (strValue == "1");
                                        return true;
                                    }
                                }
                            }
                        }
                        return false;
                    }
                }
            }
            return false;
        }

        // Token: 0x06000019 RID: 25 RVA: 0x00002D30 File Offset: 0x00000F30
        public static void TryAddCamoHidValue(Pawn seer, Pawn target, bool value)
        {
            if (seer != null)
            {
                bool flag = false;
                PawnCamoData pawnCamoData = ThingCompUtility.TryGetComp<PawnCamoData>(seer);
                if (pawnCamoData != null)
                {
                    int ticksGame = Find.TickManager.TicksGame;
                    List<string> list = new List<string>();
                    List<string> pawnHidTickList = pawnCamoData.PawnHidTickList;
                    if (pawnHidTickList != null && pawnHidTickList.Count > 0)
                    {
                        foreach (string text in pawnHidTickList)
                        {
                            if (CamoGearUtility.GetIntValue(text, 1) + CamoUtility.TickElapse >= ticksGame)
                            {
                                GenCollection.AddDistinct<string>(list, text);
                                int intValue = CamoGearUtility.GetIntValue(text, 0);
                                if (target != null && target != null)
                                {
                                    int thingIDNumber = target.thingIDNumber;
                                    if (intValue == target.thingIDNumber)
                                    {
                                        flag = true;
                                    }
                                }
                            }
                        }
                    }
                    if (!flag)
                    {
                        string text2 = string.Concat(new string[]
                        {
                            target.thingIDNumber.ToString(),
                            ";",
                            ticksGame.ToString(),
                            ";",
                            value ? "1" : "0"
                        });
                        GenCollection.AddDistinct<string>(list, text2);
                    }
                    pawnCamoData.PawnHidTickList = list;
                }
            }
        }

        // Token: 0x0600001A RID: 26 RVA: 0x00002E4C File Offset: 0x0000104C
        public static bool CamoEffectWorked(Pawn target, Thing seer, Apparel ACApparel, float CamoEff, bool isActive, bool isStealth, int StealthCamoChance, out int chance, out float scaler)
        {
            float bestChance = Controller.Settings.bestChance;
            int num = 0;
            float num2 = IntVec3Utility.DistanceTo(target.Position, seer.Position);
            float num3 = 0.1f;
            float num4 = 0.2f;
            if (!isActive)
            {
                if (num2 >= CamoUtility.minCamoDist)
                {
                    if (num2 > CamoUtility.maxCamoDist)
                    {
                        scaler = 1f;
                    }
                    else
                    {
                        scaler = Mathf.Lerp(num3, 1f, Math.Max(0f, (num2 - CamoUtility.minCamoDist) / (CamoUtility.maxCamoDist - CamoUtility.minCamoDist)));
                    }
                }
                else
                {
                    scaler = 0f;
                }
            }
            else if (num2 >= CamoUtility.ACminCamoDist)
            {
                if (num2 > CamoUtility.ACmaxCamoDist)
                {
                    scaler = 1f;
                }
                else
                {
                    scaler = Mathf.Lerp(num4, 1f, Math.Max(0f, (num2 - CamoUtility.ACminCamoDist) / (CamoUtility.ACmaxCamoDist - CamoUtility.ACminCamoDist)));
                }
            }
            else
            {
                scaler = 0f;
            }
            float num5 = Math.Max(0f, Math.Min(1f, CamoEff));
            if (num5 > 0f)
            {
                scaler *= num5;
            }
            if (isActive)
            {
                scaler *= 0.95f + 0.05f * (Controller.Settings.RelPct / 100f);
            }
            else
            {
                scaler *= 0.85f + 0.1f * (Controller.Settings.RelPct / 100f);
            }
            if (scaler < 0f)
            {
                scaler = 0f;
            }
            if (scaler > 1f)
            {
                scaler = 1f;
            }
            int num6 = 0;
            if (StealthyBox.IsWearingStealthBox(target, out _))
            {
                num6 += 25;
            }
            int num7 = 0;
            if (isActive && ACApparel != null)
            {
                num7 = CamoUtility.GetQualOffset(ACApparel);
            }
            if (isActive)
            {
                if (isStealth)
                {
                    chance = StealthCamoChance;
                    num = (int)Mathf.Lerp(Math.Min(bestChance, bestChance - (float)num7), Math.Max(1f, 1f - (float)num7), scaler);
                }
                else
                {
                    chance = (int)Mathf.Lerp(Math.Min(bestChance, bestChance - (float)num7), Math.Max(1f, 1f - (float)num7), scaler);
                }
            }
            else
            {
                chance = (int)Mathf.Lerp(bestChance, 1f, scaler);
            }
            int num8 = chance;
            if (!(seer is Pawn))
            {
                return false;
            }
            if ((seer as Pawn).health.capacities.CapableOf(PawnCapacityDefOf.Sight))
            {
                float level = (seer as Pawn).health.capacities.GetLevel(PawnCapacityDefOf.Sight);
                if (Controller.Settings.DoCheckFlash)
                {
                    float gunFlashEff = CamoUtility.GetGunFlashEff(target, seer as Pawn);
                    if (gunFlashEff > 1f)
                    {
                        int num9 = chance;
                        if (isStealth)
                        {
                            num9 = num;
                        }
                        chance = Math.Min((int)bestChance, (int)((float)num9 * gunFlashEff));
                    }
                }
                int num10 = chance;
                if (chance > 0)
                {
                    float miscFactor = CamoUtility.GetMiscFactor(target, seer as Pawn, false);
                    chance = Math.Min((int)bestChance, (int)((float)chance * level * miscFactor));
                }
                int num11 = chance;
                if (CamoUtility.IsDebugMode())
                {
                    Log.Message("Base: " + num8.ToString() + ", Flash: " + num10.ToString() + ", Sight: " + num11.ToString(), false);
                }
                return chance < 100 && (chance < 1 || CamoUtility.Rnd100() + num6 > chance);
            }
            return true;
        }

        // Token: 0x0600001B RID: 27 RVA: 0x00003198 File Offset: 0x00001398
        internal static int GetQualOffset(Apparel apparel)
        {
            if (QualityUtility.TryGetQuality(apparel, out QualityCategory qualityCategory))
            {
                switch (qualityCategory)
                {
                    case QualityCategory.Awful:
                        return -10;
                    case QualityCategory.Poor:
                        return -5;
                    case QualityCategory.Normal:
                        return 0;
                    case QualityCategory.Good:
                        return 5;
                    case QualityCategory.Excellent:
                        return 10;
                    case QualityCategory.Masterwork:
                        return 15;
                    case QualityCategory.Legendary:
                        return 20;
                    default:
                        break;
                }
            }
            return 0;
        }

        // Token: 0x0600001C RID: 28 RVA: 0x000031E8 File Offset: 0x000013E8
        internal static float GetMiscFactor(Pawn target, Pawn seer, bool ActiveCamo)
        {
            float num = 1f;
            bool flag = StealthyBox.IsWearingStealthBox(target, out _);
            if (flag)
            {
                num *= 0.5f;
            }
            bool flag2;
            if (target == null)
            {
                flag2 = (null != null);
            }
            else
            {
                Pawn_StanceTracker stances = target.stances;
                flag2 = ((stances?.curStance) != null);
            }
            if (flag2)
            {
                if (!target.Downed)
                {
                    if (target.stances.curStance is Stance_Mobile)
                    {
                        if (flag)
                        {
                            num *= 1.5f;
                        }
                        else
                        {
                            num *= 1.1f;
                        }
                        if (CamoUtility.IsInsectoid(seer))
                        {
                            num *= 1.25f;
                        }
                    }
                    else if (target.stances.curStance is Stance_Cooldown || target.stances.curStance is Stance_Warmup)
                    {
                        num *= 0.95f;
                        if (CamoUtility.IsInsectoid(seer))
                        {
                            num *= 0.75f;
                        }
                    }
                }
                else
                {
                    num *= 0.5f;
                    if (CamoUtility.IsInsectoid(seer))
                    {
                        num *= 0.75f;
                    }
                }
            }
            if (Controller.Settings.DoCheckWeather && (target?.Map) != null)
            {
                float accuracyMultiplier = target.Map.weatherManager.curWeather.accuracyMultiplier;
                if (accuracyMultiplier != 1f)
                {
                    num *= accuracyMultiplier;
                }
            }
            if (Controller.Settings.DoCheckLight && !ActiveCamo)
            {
                PsychGlow psychGlow = target.Map.glowGrid.PsychGlowAt(target.Position);
                if (psychGlow == PsychGlow.Overlit)
                {
                    num *= 1.15f;
                }
                else
                {
                    num *= 0.8f;
                }
                PsychGlow psychGlow2 = seer.Map.glowGrid.PsychGlowAt(seer.Position);
                if (psychGlow2 == PsychGlow.Overlit)
                {
                    num *= 0.85f;
                }
                else
                {
                    num *= 1.2f;
                }
            }
            if (Controller.Settings.DoCheckTemp)
            {
                float temperature = GridsUtility.GetTemperature(target.Position, target.Map);
                float num2 = 21f;
                if (temperature > num2)
                {
                    num *= Mathf.Lerp(1f, 0.85f, (temperature - num2) / temperature);
                }
            }
            return num;
        }

        // Token: 0x0600001D RID: 29 RVA: 0x000033BF File Offset: 0x000015BF
        internal static bool IsInsectoid(Pawn pawn)
        {
            return pawn != null && pawn.RaceProps.Animal && pawn.RaceProps.FleshType == FleshTypeDefOf.Insectoid;
        }

        // Token: 0x0600001E RID: 30 RVA: 0x000033E8 File Offset: 0x000015E8
        internal static float GetGunFlashEff(Pawn pawn, Pawn seer)
        {
            if (pawn != null && (pawn?.Map) != null && seer != null && (seer?.Map) != null && (pawn?.Map) == (seer?.Map))
            {
                Verb verb = pawn?.CurrentEffectiveVerb;
                if (verb != null && !verb.IsMeleeAttack)
                {
                    float muzzleFlashScale = verb.verbProps.muzzleFlashScale;
                    if (muzzleFlashScale > 0f)
                    {
                        int burstShotCount = verb.verbProps.burstShotCount;
                        if (burstShotCount > 0)
                        {
                            bool flag;
                            if (pawn == null)
                            {
                                flag = (null != null);
                            }
                            else
                            {
                                Pawn_StanceTracker stances = pawn.stances;
                                flag = ((stances?.curStance) != null);
                            }
                            if (flag && pawn.stances.curStance is Stance_Cooldown)
                            {
                                float num = 1f;
                                float num2 = Mathf.Lerp(1f, 1.25f, Math.Min(1f, muzzleFlashScale / 7f)) * num;
                                float num3 = Mathf.Lerp(1f, 1.15f, (float)(Math.Min(burstShotCount, 5) / 5));
                                if (CamoUtility.IsDebugMode())
                                {
                                    string text = string.Concat(new string[]
                                    {
                                        "RF: ",
                                        num.ToString("F2"),
                                        ", FF: ",
                                        num2.ToString("F2"),
                                        " BF: ",
                                        num3.ToString("F2")
                                    });
                                    text = string.Concat(new string[]
                                    {
                                        text,
                                        "flash: MFS: ",
                                        muzzleFlashScale.ToString("F2"),
                                        ", maxburst: ",
                                        burstShotCount.ToString()
                                    });
                                    Log.Message(text, false);
                                }
                                return 1f * num2 * num3;
                            }
                        }
                    }
                }
            }
            return 1f;
        }

        // Token: 0x0600001F RID: 31 RVA: 0x000035AF File Offset: 0x000017AF
        public static bool IsDebugMode()
        {
            return Prefs.DevMode && Controller.Settings.useDebug && !CamoUtility.IsGamePaused();
        }

        // Token: 0x06000020 RID: 32 RVA: 0x000035CE File Offset: 0x000017CE
        internal static float GetFlashDistFactor(Pawn pawn, Pawn seer)
        {
            return Mathf.Lerp(0.5f, 1f, Math.Min(CamoUtility.maxCamoDist, IntVec3Utility.DistanceTo(pawn.Position, seer.Position)) / CamoUtility.maxCamoDist);
        }

        // Token: 0x06000021 RID: 33 RVA: 0x00003600 File Offset: 0x00001800
        internal static void DoCamoMote(Thing thing, Thing ghost, bool hidden, int chance, float camoEff, float scaler)
        {
            bool flag = true;
            bool flag2 = false;
            if (CamoUtility.IsDebugMode() && Controller.Settings.ShowMoteMsgs && thing != null && (thing?.Map) != null && thing.Spawned)
            {
                if (flag)
                {
                    if (thing is Pawn)
                    {
                        flag2 = (ghost is Pawn && GenHostility.HostileTo(thing, ghost) && (!(thing.Position == ghost.Position) && !thing.Position.InHorDistOf(ghost.Position, CamoUtility.NotPossibleMinDist)));
                    }
                }
                else
                {
                    flag2 = true;
                }
                if (flag2 && CamoUtility.IsValidThingForCamo(thing))
                {
                    Vector3 vector = thing.Position.ToVector3();
                    _ = Color.cyan;
                    string text;
                    Color color;
                    if (hidden)
                    {
                        text = TranslatorFormattedStringExtensions.Translate("CompCamo.CannotSee", chance.ToString(), GenText.ToStringPercent(camoEff), GenText.ToStringPercent(scaler));
                        color = Color.green;
                    }
                    else
                    {
                        text = TranslatorFormattedStringExtensions.Translate("CompCamo.CanSee", chance.ToString(), GenText.ToStringPercent(camoEff), GenText.ToStringPercent(scaler));
                        color = Color.red;
                    }
                    MoteMaker.ThrowText(vector, thing.Map, text, color, -1f);
                    if (CamoUtility.IsDebugMode())
                    {
                        string text2 = text;
                        bool flag3;
                        if (!(thing is Pawn pawn))
                        {
                            flag3 = (null != null);
                        }
                        else
                        {
                            Pawn_JobTracker jobs = pawn.jobs;
                            flag3 = ((jobs?.curJob) != null);
                        }
                        if (flag3)
                        {
                            text2 = text2 + ", CJ: " + (thing as Pawn).jobs.curJob.def.defName;
                            string str = text2;
                            string str2 = ", ET:";
                            bool flag4;
                            if (!(thing is Pawn pawn2))
                            {
                                flag4 = (null != null);
                            }
                            else
                            {
                                Pawn_MindState mindState = pawn2.mindState;
                                flag4 = ((mindState?.enemyTarget) != null);
                            }
                            string str3;
                            if (!flag4)
                            {
                                str3 = "null";
                            }
                            else
                            {
                                Thing enemyTarget = (thing as Pawn).mindState.enemyTarget;
                                str3 = (enemyTarget?.LabelShortCap);
                            }
                            text2 = str + str2 + str3;
                            string str4 = text2;
                            string str5 = ", MT:";
                            bool flag5;
                            if (!(thing is Pawn pawn3))
                            {
                                flag5 = (null != null);
                            }
                            else
                            {
                                Pawn_MindState mindState2 = pawn3.mindState;
                                flag5 = ((mindState2?.meleeThreat) != null);
                            }
                            string str6;
                            if (!flag5)
                            {
                                str6 = "null";
                            }
                            else
                            {
                                Pawn meleeThreat = (thing as Pawn).mindState.meleeThreat;
                                str6 = (meleeThreat?.LabelShortCap);
                            }
                            text2 = str4 + str5 + str6;
                        }
                        Log.Message(text2, false);
                    }
                }
            }
        }

        // Token: 0x06000022 RID: 34 RVA: 0x0000385E File Offset: 0x00001A5E
        internal static bool IsGamePaused()
        {
            return Find.TickManager.Paused;
        }

        // Token: 0x06000023 RID: 35 RVA: 0x0000386F File Offset: 0x00001A6F
        internal static bool IsValidThingForCamo(Thing thing)
        {
            return thing != null && (thing?.Map) != null && thing is Pawn && (thing as Pawn).Spawned && !(thing as Pawn).RaceProps.IsMechanoid;
        }

        // Token: 0x06000024 RID: 36 RVA: 0x000038AC File Offset: 0x00001AAC
        public static int Rnd100()
        {
            return Rand.Range(1, 100);
        }

        // Token: 0x04000001 RID: 1
        public static float minCamoDist = 1f;

        // Token: 0x04000002 RID: 2
        public static float maxCamoDist = 60f;

        // Token: 0x04000003 RID: 3
        public static float ACminCamoDist = 5f;

        // Token: 0x04000004 RID: 4
        public static float ACmaxCamoDist = 60f;

        // Token: 0x04000005 RID: 5
        public static float NotPossibleMinDist = 2f;

        // Token: 0x04000006 RID: 6
        public static int TickElapse = 300;
    }
}
