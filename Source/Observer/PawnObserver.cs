using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace Observer
{
	// Token: 0x02000006 RID: 6
	public class PawnObserver : ThingComp
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000007 RID: 7 RVA: 0x000020D5 File Offset: 0x000002D5
		private Pawn Pawn
		{
			get
			{
				return (Pawn)this.parent;
			}
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000020E2 File Offset: 0x000002E2
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.PawnSightOffset, "PawnSightOffset", 0f, false);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002100 File Offset: 0x00000300
		public override void CompTick()
		{
			base.CompTick();
			if (Gen.HashOffsetTicks(this.Pawn) == 180)
			{
				this.CalcAndSetObserver(this.Pawn);
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002126 File Offset: 0x00000326
		public override void CompTickRare()
		{
			base.CompTickRare();
		}

		// Token: 0x0600000B RID: 11 RVA: 0x0000212E File Offset: 0x0000032E
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.CalcAndSetObserver(this.Pawn);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002144 File Offset: 0x00000344
		public void CalcAndSetObserver(Pawn pawn)
		{
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			float num4 = 0.25f;
			bool flag = true;
			bool flag2 = true;
			bool flag3 = false;
			bool flag4 = false;
			if (pawn != null)
			{
				if ((pawn?.apparel) != null && flag && pawn.apparel.WornApparelCount > 0)
				{
					List<Apparel> wornApparel = pawn.apparel.WornApparel;
					if (wornApparel != null && wornApparel.Count > 0)
					{
						foreach (Apparel apparel in wornApparel)
						{
							CompObserver compObserver = ThingCompUtility.TryGetComp<CompObserver>(apparel);
							if (compObserver != null)
							{
								num += compObserver.Props.SightOffset;
								if (compObserver.Props.SightOffset > num2)
								{
									num2 = compObserver.Props.SightOffset;
								}
								if (compObserver.Props.SightOffset < num3)
								{
									num3 = compObserver.Props.SightOffset;
								}
							}
						}
					}
				}
				if ((pawn?.equipment) != null && flag2 && pawn.equipment.HasAnything())
				{
					List<ThingWithComps> allEquipmentListForReading = pawn.equipment.AllEquipmentListForReading;
					if (allEquipmentListForReading != null && allEquipmentListForReading.Count > 0)
					{
						foreach (ThingWithComps thingWithComps in allEquipmentListForReading)
						{
							CompObserver compObserver2 = ThingCompUtility.TryGetComp<CompObserver>(thingWithComps);
							if (compObserver2 != null)
							{
								num += compObserver2.Props.SightOffset;
								if (compObserver2.Props.SightOffset > num2)
								{
									num2 = compObserver2.Props.SightOffset;
								}
								if (compObserver2.Props.SightOffset < num3)
								{
									num3 = compObserver2.Props.SightOffset;
								}
							}
						}
					}
				}
				if ((pawn?.inventory) != null && flag3 && pawn.inventory.innerContainer.Any)
				{
					List<Thing> innerListForReading = pawn.inventory.innerContainer.InnerListForReading;
					if (innerListForReading != null && innerListForReading.Count > 0)
					{
						foreach (Thing thing in innerListForReading)
						{
							CompObserver compObserver3 = ThingCompUtility.TryGetComp<CompObserver>(thing as ThingWithComps);
							if (compObserver3 != null)
							{
								num += compObserver3.Props.SightOffset;
								if (compObserver3.Props.SightOffset > num2)
								{
									num2 = compObserver3.Props.SightOffset;
								}
								if (compObserver3.Props.SightOffset < num3)
								{
									num3 = compObserver3.Props.SightOffset;
								}
							}
						}
					}
				}
			}
			if (num != 0f)
			{
				num = Math.Min(num4, Math.Max(0f - num4, num));
				num2 = Math.Min(num4, Math.Max(0f - num4, num2));
				num3 = Math.Min(num4, Math.Max(0f - num4, num3));
			}
			PawnObserver pawnObserver = ThingCompUtility.TryGetComp<PawnObserver>(pawn);
			if (pawnObserver != null)
			{
				if (flag4)
				{
					pawnObserver.PawnSightOffset = num;
					return;
				}
				pawnObserver.PawnSightOffset = num2 + num3;
			}
		}

		// Token: 0x04000002 RID: 2
		public float PawnSightOffset;

		// Token: 0x02000007 RID: 7
		public class CompProperties_PawnObserver : CompProperties
		{
			// Token: 0x0600000E RID: 14 RVA: 0x0000246C File Offset: 0x0000066C
			public CompProperties_PawnObserver()
			{
				this.compClass = typeof(PawnObserver);
			}
		}

		// Token: 0x02000008 RID: 8
		[StaticConstructorOnStartup]
		private static class Observer_Setup
		{
			// Token: 0x0600000F RID: 15 RVA: 0x00002484 File Offset: 0x00000684
			static Observer_Setup()
			{
				PawnObserver.Observer_Setup.Observer_Setup_Pawns();
			}

			// Token: 0x06000010 RID: 16 RVA: 0x0000248B File Offset: 0x0000068B
			private static void Observer_Setup_Pawns()
			{
				PawnObserver.Observer_Setup.ObserverSetup_Comp(typeof(PawnObserver.CompProperties_PawnObserver), (ThingDef def) => def.race != null);
			}

			// Token: 0x06000011 RID: 17 RVA: 0x000024BC File Offset: 0x000006BC
			private static void ObserverSetup_Comp(Type compType, Func<ThingDef, bool> qualifier)
			{
				List<ThingDef> list = DefDatabase<ThingDef>.AllDefsListForReading.Where(qualifier).ToList();
				GenList.RemoveDuplicates<ThingDef>(list);
				foreach (ThingDef item in list)
				{
					if (item.comps != null && !GenCollection.Any<CompProperties>(item.comps, (Predicate<CompProperties>)((CompProperties c) => (object)((object)c).GetType() == compType)))
					{
						item.comps.Add((CompProperties)(object)(CompProperties)Activator.CreateInstance(compType));
					}
				}
			}
		}
	}
}
