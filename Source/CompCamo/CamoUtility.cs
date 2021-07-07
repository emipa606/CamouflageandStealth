using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace CompCamo
{
    // Token: 0x02000004 RID: 4
    public class CamoUtility
    {
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

        // Token: 0x06000014 RID: 20 RVA: 0x00002890 File Offset: 0x00000A90
        public static bool IsCamoActive(Pawn target, out Apparel ACItem)
        {
            ACItem = null;
            if (!CamoGearUtility.IsWearingActiveCamo(target, out var apparel))
            {
                return false;
            }

            if (apparel == null || !ActiveCamoIsActive(apparel))
            {
                return false;
            }

            ACItem = apparel;
            return true;
        }

        // Token: 0x06000015 RID: 21 RVA: 0x000028BD File Offset: 0x00000ABD
        internal static bool ActiveCamoIsActive(Apparel AC)
        {
            return ((ActiveCamoApparel) AC).IsActiveCamo;
        }

        // Token: 0x06000016 RID: 22 RVA: 0x000028CC File Offset: 0x00000ACC
        public static bool IsTargetHidden(Pawn target, Pawn seer)
        {
            if (target == null || seer == null)
            {
                Log.Message("Target/Seer Null");
                return false;
            }

            if (target == seer)
            {
                return false;
            }

            if (TryGetCamoHidValue(seer, target, out var result))
            {
                return result;
            }

            if (!target.Spawned)
            {
                return false;
            }

            var isCamoActive = false;
            Apparel apparel = null;
            if (!IsDebugMode() || !Controller.Settings.forcePassive)
            {
                isCamoActive = IsCamoActive(target, out var apparel2);
                if (apparel2 != null)
                {
                    apparel = apparel2;
                }
            }

            if ((!isCamoActive || seer.CurrentEffectiveVerb.IsMeleeAttack) && SimplyTooClose(seer, target))
            {
                TryAddCamoHidValue(seer, target, false);
                return false;
            }

            if (isCamoActive || IsDebugMode() && Controller.Settings.forceActive)
            {
                if (!seer.Spawned)
                {
                    return false;
                }

                if (target.Map == null || seer.Map == null || target.Map != seer.Map ||
                    seer.InMentalState && target.InMentalState)
                {
                    TryAddCamoHidValue(seer, target, false);
                    return false;
                }

                if (!GenSight.LineOfSight(seer.Position, target.Position, seer.Map))
                {
                    return true;
                }

                var num = 0.75f;
                var num2 = 0;
                bool settingsForceStealth;
                if (IsDebugMode() && Controller.Settings.forceActive)
                {
                    apparel = null;
                    settingsForceStealth = Controller.Settings.forceStealth;
                    if (settingsForceStealth)
                    {
                        num2 = 5;
                    }
                }
                else
                {
                    num = apparel.TryGetComp<CompGearCamo>().Props.ActiveCamoEff;
                    num2 = apparel.TryGetComp<CompGearCamo>().Props.StealthCamoChance;
                    settingsForceStealth = num2 > 0 && num > 0f;
                }

                if (CamoEffectWorked(target, seer, apparel, num, true, settingsForceStealth, num2, out var chance,
                    out var scaler))
                {
                    DoCamoMote(seer, target, true, chance, num, scaler);
                    TryAddCamoHidValue(seer, target, true);
                    CamoAIUtility.CorrectLordForCamo(seer, target);
                    return true;
                }

                DoCamoMote(seer, target, false, chance, num, scaler);
                TryAddCamoHidValue(seer, target, false);
                return false;
            }

            if (!seer.Spawned)
            {
                return false;
            }

            if (!CamoGearUtility.GetCurCamoEff(target, out var str, out var camoEff))
            {
                TryAddCamoHidValue(seer, target, false);
                return false;
            }

            if (IsDebugMode())
            {
                if (Controller.Settings.forcePassive)
                {
                    camoEff = 0.75f;
                }

                Log.Message("Camo: " + str + " : " + camoEff.ToString("F2"));
            }

            if (target.Map == null || seer.Map == null || target.Map != seer.Map ||
                seer.InMentalState && target.InMentalState)
            {
                TryAddCamoHidValue(seer, target, false);
                return false;
            }

            if (!GenSight.LineOfSight(seer.Position, target.Position, seer.Map))
            {
                return true;
            }

            if (CamoEffectWorked(target, seer, null, camoEff, false, false, 0, out var chance2,
                out var scaler2))
            {
                DoCamoMote(seer, target, true, chance2, camoEff, scaler2);
                TryAddCamoHidValue(seer, target, true);
                CamoAIUtility.CorrectLordForCamo(seer, target);
                return true;
            }

            DoCamoMote(seer, target, false, chance2, camoEff, scaler2);
            TryAddCamoHidValue(seer, target, false);
            return false;
        }

        // Token: 0x06000017 RID: 23 RVA: 0x00002BD8 File Offset: 0x00000DD8
        public static bool SimplyTooClose(Pawn seer, Pawn target)
        {
            if (seer == null || target == null || seer.Map == null || target.Map == null || seer.Map != target.Map ||
                !seer.Spawned || !target.Spawned)
            {
                return false;
            }

            if (seer.Position == target.Position)
            {
                return true;
            }

            if (seer.Position.InHorDistOf(target.Position, NotPossibleMinDist))
            {
                return true;
            }

            return false;
        }

        // Token: 0x06000018 RID: 24 RVA: 0x00002C58 File Offset: 0x00000E58
        public static bool TryGetCamoHidValue(Pawn seer, Pawn target, out bool hid)
        {
            hid = false;

            var pawnCamoData = seer?.TryGetComp<PawnCamoData>();
            if (pawnCamoData == null)
            {
                return false;
            }

            var ticksGame = Find.TickManager.TicksGame;
            var list = pawnCamoData.PawnHidTickList;
            if (list == null || list.Count <= 0)
            {
                return false;
            }

            foreach (var valuesStr in list)
            {
                if (CamoGearUtility.GetIntValue(valuesStr, 1) + TickElapse < ticksGame)
                {
                    continue;
                }

                var intValue = CamoGearUtility.GetIntValue(valuesStr, 0);
                if (target == null)
                {
                    continue;
                }

                var unused = target.thingIDNumber;
                if (intValue != target.thingIDNumber)
                {
                    continue;
                }

                var strValue = CamoGearUtility.GetStrValue(valuesStr, 2);
                hid = strValue == "1";
                return true;
            }

            return false;
        }

        // Token: 0x06000019 RID: 25 RVA: 0x00002D30 File Offset: 0x00000F30
        public static void TryAddCamoHidValue(Pawn seer, Pawn target, bool value)
        {
            if (seer == null)
            {
                return;
            }

            var b = false;
            var pawnCamoData = seer.TryGetComp<PawnCamoData>();
            if (pawnCamoData == null)
            {
                return;
            }

            var ticksGame = Find.TickManager.TicksGame;
            var list = new List<string>();
            var pawnHidTickList = pawnCamoData.PawnHidTickList;
            if (pawnHidTickList != null && pawnHidTickList.Count > 0)
            {
                foreach (var text in pawnHidTickList)
                {
                    if (CamoGearUtility.GetIntValue(text, 1) + TickElapse < ticksGame)
                    {
                        continue;
                    }

                    list.AddDistinct(text);
                    var intValue = CamoGearUtility.GetIntValue(text, 0);
                    if (target == null)
                    {
                        continue;
                    }

                    var unused = target.thingIDNumber;
                    if (intValue == target.thingIDNumber)
                    {
                        b = true;
                    }
                }
            }

            if (!b)
            {
                if (target != null)
                {
                    var text2 = string.Concat(target.thingIDNumber.ToString(), ";", ticksGame.ToString(), ";",
                        value ? "1" : "0");
                    list.AddDistinct(text2);
                }
            }

            pawnCamoData.PawnHidTickList = list;
        }

        // Token: 0x0600001A RID: 26 RVA: 0x00002E4C File Offset: 0x0000104C
        public static bool CamoEffectWorked(Pawn target, Thing seer, Apparel ACApparel, float CamoEff, bool isActive,
            bool isStealth, int StealthCamoChance, out int chance, out float scaler)
        {
            var bestChance = Controller.Settings.bestChance;
            var num = 0;
            var num2 = target.Position.DistanceTo(seer.Position);
            var num3 = 0.1f;
            var num4 = 0.2f;
            if (!isActive)
            {
                if (num2 >= minCamoDist)
                {
                    scaler = num2 > maxCamoDist
                        ? 1f
                        : Mathf.Lerp(num3, 1f, Math.Max(0f, (num2 - minCamoDist) / (maxCamoDist - minCamoDist)));
                }
                else
                {
                    scaler = 0f;
                }
            }
            else if (num2 >= ACminCamoDist)
            {
                if (num2 > ACmaxCamoDist)
                {
                    scaler = 1f;
                }
                else
                {
                    scaler = Mathf.Lerp(num4, 1f,
                        Math.Max(0f, (num2 - ACminCamoDist) / (ACmaxCamoDist - ACminCamoDist)));
                }
            }
            else
            {
                scaler = 0f;
            }

            var num5 = Math.Max(0f, Math.Min(1f, CamoEff));
            if (num5 > 0f)
            {
                scaler *= num5;
            }

            if (isActive)
            {
                scaler *= 0.95f + (0.05f * (Controller.Settings.RelPct / 100f));
            }
            else
            {
                scaler *= 0.85f + (0.1f * (Controller.Settings.RelPct / 100f));
            }

            if (scaler < 0f)
            {
                scaler = 0f;
            }

            if (scaler > 1f)
            {
                scaler = 1f;
            }

            var num6 = 0;
            if (StealthyBox.IsWearingStealthBox(target, out _))
            {
                num6 += 25;
            }

            var num7 = 0;
            if (isActive && ACApparel != null)
            {
                num7 = GetQualOffset(ACApparel);
            }

            if (isActive)
            {
                if (isStealth)
                {
                    chance = StealthCamoChance;
                    num = (int) Mathf.Lerp(Math.Min(bestChance, bestChance - num7), Math.Max(1f, 1f - num7), scaler);
                }
                else
                {
                    chance = (int) Mathf.Lerp(Math.Min(bestChance, bestChance - num7), Math.Max(1f, 1f - num7), scaler);
                }
            }
            else
            {
                chance = (int) Mathf.Lerp(bestChance, 1f, scaler);
            }

            var num8 = chance;
            if (!(seer is Pawn))
            {
                return false;
            }

            if (!((Pawn) seer).health.capacities.CapableOf(PawnCapacityDefOf.Sight))
            {
                return true;
            }

            var level = ((Pawn) seer).health.capacities.GetLevel(PawnCapacityDefOf.Sight);
            if (Controller.Settings.DoCheckFlash)
            {
                var gunFlashEff = GetGunFlashEff(target, (Pawn) seer);
                if (gunFlashEff > 1f)
                {
                    var num9 = chance;
                    if (isStealth)
                    {
                        num9 = num;
                    }

                    chance = Math.Min((int) bestChance, (int) (num9 * gunFlashEff));
                }
            }

            var num10 = chance;
            if (chance > 0)
            {
                var miscFactor = GetMiscFactor(target, (Pawn) seer, false);
                chance = Math.Min((int) bestChance, (int) (chance * level * miscFactor));
            }

            var num11 = chance;
            if (IsDebugMode())
            {
                Log.Message("Base: " + num8 + ", Flash: " + num10 + ", Sight: " + num11);
            }

            return chance < 100 && (chance < 1 || Rnd100() + num6 > chance);
        }

        // Token: 0x0600001B RID: 27 RVA: 0x00003198 File Offset: 0x00001398
        internal static int GetQualOffset(Apparel apparel)
        {
            if (!apparel.TryGetQuality(out var qualityCategory))
            {
                return 0;
            }

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
            }

            return 0;
        }

        // Token: 0x0600001C RID: 28 RVA: 0x000031E8 File Offset: 0x000013E8
        internal static float GetMiscFactor(Pawn target, Pawn seer, bool ActiveCamo)
        {
            var num = 1f;
            if (StealthyBox.IsWearingStealthBox(target, out _))
            {
                num *= 0.5f;
            }

            bool b;
            if (target == null)
            {
                b = false;
            }
            else
            {
                var stances = target.stances;
                b = stances?.curStance != null;
            }

            if (b)
            {
                if (!target.Downed)
                {
                    if (target.stances.curStance is Stance_Mobile)
                    {
                        if (StealthyBox.IsWearingStealthBox(target, out _))
                        {
                            num *= 1.5f;
                        }
                        else
                        {
                            num *= 1.1f;
                        }

                        if (IsInsectoid(seer))
                        {
                            num *= 1.25f;
                        }
                    }
                    else if (target.stances.curStance is Stance_Cooldown || target.stances.curStance is Stance_Warmup)
                    {
                        num *= 0.95f;
                        if (IsInsectoid(seer))
                        {
                            num *= 0.75f;
                        }
                    }
                }
                else
                {
                    num *= 0.5f;
                    if (IsInsectoid(seer))
                    {
                        num *= 0.75f;
                    }
                }
            }

            if (Controller.Settings.DoCheckWeather && target?.Map != null)
            {
                var accuracyMultiplier = target.Map.weatherManager.curWeather.accuracyMultiplier;
                if (accuracyMultiplier != 1f)
                {
                    num *= accuracyMultiplier;
                }
            }

            if (Controller.Settings.DoCheckLight && !ActiveCamo)
            {
                if (target != null)
                {
                    var psychGlow = target.Map.glowGrid.PsychGlowAt(target.Position);
                    if (psychGlow == PsychGlow.Overlit)
                    {
                        num *= 1.15f;
                    }
                    else
                    {
                        num *= 0.8f;
                    }
                }

                var psychGlow2 = seer.Map.glowGrid.PsychGlowAt(seer.Position);
                if (psychGlow2 == PsychGlow.Overlit)
                {
                    num *= 0.85f;
                }
                else
                {
                    num *= 1.2f;
                }
            }

            if (!Controller.Settings.DoCheckTemp)
            {
                return num;
            }

            if (target == null)
            {
                return num;
            }

            var temperature = target.Position.GetTemperature(target.Map);
            var num2 = 21f;
            if (temperature > num2)
            {
                num *= Mathf.Lerp(1f, 0.85f, (temperature - num2) / temperature);
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
            if (pawn?.Map == null || seer?.Map == null || pawn.Map != seer.Map)
            {
                return 1f;
            }

            var verb = pawn.CurrentEffectiveVerb;
            if (verb == null || verb.IsMeleeAttack)
            {
                return 1f;
            }

            var muzzleFlashScale = verb.verbProps.muzzleFlashScale;
            if (!(muzzleFlashScale > 0f))
            {
                return 1f;
            }

            var burstShotCount = verb.verbProps.burstShotCount;
            if (burstShotCount <= 0)
            {
                return 1f;
            }

            var stances = pawn.stances;

            if (stances?.curStance == null || pawn.stances.curStance is not Stance_Cooldown)
            {
                return 1f;
            }

            var num = 1f;
            var num2 = Mathf.Lerp(1f, 1.25f, Math.Min(1f, muzzleFlashScale / 7f)) * num;
            var num3 = Mathf.Lerp(1f, 1.15f, Math.Min(burstShotCount, 5) / (float) 5);
            if (!IsDebugMode())
            {
                return 1f * num2 * num3;
            }

            var text = string.Concat("RF: ", num.ToString("F2"), ", FF: ", num2.ToString("F2"),
                " BF: ", num3.ToString("F2"));
            text = string.Concat(text, "flash: MFS: ", muzzleFlashScale.ToString("F2"),
                ", maxburst: ", burstShotCount.ToString());
            Log.Message(text);

            return 1f * num2 * num3;
        }

        // Token: 0x0600001F RID: 31 RVA: 0x000035AF File Offset: 0x000017AF
        public static bool IsDebugMode()
        {
            return Prefs.DevMode && Controller.Settings.useDebug && !IsGamePaused();
        }

        // Token: 0x06000020 RID: 32 RVA: 0x000035CE File Offset: 0x000017CE
        internal static float GetFlashDistFactor(Pawn pawn, Pawn seer)
        {
            return Mathf.Lerp(0.5f, 1f, Math.Min(maxCamoDist, pawn.Position.DistanceTo(seer.Position)) / maxCamoDist);
        }

        // Token: 0x06000021 RID: 33 RVA: 0x00003600 File Offset: 0x00001800
        internal static void DoCamoMote(Thing thing, Thing ghost, bool hidden, int chance, float camoEff, float scaler)
        {
            var hostileTo = false;
            if (!IsDebugMode() || !Controller.Settings.ShowMoteMsgs || thing?.Map == null || !thing.Spawned)
            {
                return;
            }

            if (true)
            {
                if (thing is Pawn)
                {
                    hostileTo = ghost is Pawn && thing.HostileTo(ghost) && !(thing.Position == ghost.Position) &&
                                !thing.Position.InHorDistOf(ghost.Position, NotPossibleMinDist);
                }
            }

            if (!hostileTo || !IsValidThingForCamo(thing))
            {
                return;
            }

            var vector = thing.Position.ToVector3();
            _ = Color.cyan;
            string text;
            Color color;
            if (hidden)
            {
                text = "CompCamo.CannotSee".Translate(chance.ToString(), camoEff.ToStringPercent(),
                    scaler.ToStringPercent());
                color = Color.green;
            }
            else
            {
                text = "CompCamo.CanSee".Translate(chance.ToString(), camoEff.ToStringPercent(),
                    scaler.ToStringPercent());
                color = Color.red;
            }

            MoteMaker.ThrowText(vector, thing.Map, text, color);
            if (!IsDebugMode())
            {
                return;
            }

            var text2 = text;
            bool flag3;
            if (!(thing is Pawn pawn))
            {
                flag3 = false;
            }
            else
            {
                var jobs = pawn.jobs;
                flag3 = jobs?.curJob != null;
            }

            if (flag3)
            {
                text2 = text2 + ", CJ: " + ((Pawn) thing).jobs.curJob.def.defName;
                var str = text2;
                var str2 = ", ET:";
                bool flag4;
                if (!(thing is Pawn pawn2))
                {
                    flag4 = null != null;
                }
                else
                {
                    var mindState = pawn2.mindState;
                    flag4 = mindState?.enemyTarget != null;
                }

                string str3;
                if (!flag4)
                {
                    str3 = "null";
                }
                else
                {
                    var enemyTarget = (thing as Pawn).mindState.enemyTarget;
                    str3 = enemyTarget?.LabelShortCap;
                }

                text2 = str + str2 + str3;
                var str4 = text2;
                var str5 = ", MT:";
                bool flag5;
                if (!(thing is Pawn pawn3))
                {
                    flag5 = null != null;
                }
                else
                {
                    var mindState2 = pawn3.mindState;
                    flag5 = mindState2?.meleeThreat != null;
                }

                string str6;
                if (!flag5)
                {
                    str6 = "null";
                }
                else
                {
                    var meleeThreat = (thing as Pawn).mindState.meleeThreat;
                    str6 = meleeThreat?.LabelShortCap;
                }

                text2 = str4 + str5 + str6;
            }

            Log.Message(text2);
        }

        // Token: 0x06000022 RID: 34 RVA: 0x0000385E File Offset: 0x00001A5E
        internal static bool IsGamePaused()
        {
            return Find.TickManager.Paused;
        }

        // Token: 0x06000023 RID: 35 RVA: 0x0000386F File Offset: 0x00001A6F
        internal static bool IsValidThingForCamo(Thing thing)
        {
            return thing?.Map != null && thing is Pawn {Spawned: true} pawn && !pawn.RaceProps.IsMechanoid;
        }

        // Token: 0x06000024 RID: 36 RVA: 0x000038AC File Offset: 0x00001AAC
        public static int Rnd100()
        {
            return Rand.Range(1, 100);
        }
    }
}