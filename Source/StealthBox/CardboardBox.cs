using System;
using RimWorld;
using UnityEngine;
using Verse;

namespace StealthBox
{
	// Token: 0x02000003 RID: 3
	public class CardboardBox : Apparel
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000003 RID: 3 RVA: 0x000020A7 File Offset: 0x000002A7
		public float DmgBoxKill
		{
			get
			{
				return (float)this.HitPoints;
			}
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020B0 File Offset: 0x000002B0
		public override void ExposeData()
		{
			base.ExposeData();
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000020B8 File Offset: 0x000002B8
		public override void DrawWornExtras()
		{
			if (base.Wearer != null)
			{
				Pawn wearer = base.Wearer;
				if ((wearer?.Map) != null && base.Wearer.Spawned)
				{
					string text = "";
					Rot4 rotation = base.Wearer.Rotation;
					if (rotation == Rot4.North)
					{
						text = this.BoxGraphicNorthPath;
					}
					else if (rotation == Rot4.East)
					{
						text = this.BoxGraphicEastPath;
					}
					else if (rotation == Rot4.South)
					{
						text = this.BoxGraphicSouthPath;
					}
					else if (rotation == Rot4.West)
					{
						text = this.BoxGraphicWestPath;
					}
					float y = Altitudes.AltitudeFor(AltitudeLayer.Blueprint);
					Vector3 drawPos = base.Wearer.Drawer.DrawPos;
					drawPos.y = y;
					float num = 2.5f;
					Material material = MaterialPool.MatFrom(text, true);
					float asAngle = rotation.AsAngle;
					Vector3 vector = new Vector3(num, 1f, num);
					Matrix4x4 matrix4x = default;
					matrix4x.SetTRS(drawPos, Quaternion.AngleAxis(asAngle, Vector3.up), vector);
					Graphics.DrawMesh(MeshPool.plane10, matrix4x, material, 0);
				}
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000021D8 File Offset: 0x000003D8
		public override void Tick()
		{
			base.Tick();
			if (base.Wearer != null)
			{
				Pawn wearer = base.Wearer;
				if ((wearer?.Map) != null && base.Wearer.Spawned && (base.Wearer.Downed || base.Wearer.Dead || this.JobIsLyingDown(base.Wearer)))
				{
					this.DropBox(base.Wearer, this);
				}
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002249 File Offset: 0x00000449
		public override void TickRare()
		{
			base.TickRare();
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002254 File Offset: 0x00000454
		public override bool CheckPreAbsorbDamage(DamageInfo dinfo)
		{
			if (dinfo.Def.harmsHealth && dinfo.Amount > 0f)
			{
				if ((float)((int)dinfo.Amount) >= this.DmgBoxKill)
				{
					this.Destroy(0);
					return false;
				}
				this.HitPoints -= (int)dinfo.Amount;
			}
			return true;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000022AD File Offset: 0x000004AD
		public override bool AllowVerbCast(IntVec3 root, Map map, LocalTargetInfo targ, Verb verb)
		{
			return true;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000022B0 File Offset: 0x000004B0
		public bool JobIsLyingDown(Pawn pawn)
		{
			if (pawn != null && pawn.CurJob != null)
			{
				PawnPosture posture = PawnUtility.GetPosture(pawn);
				if (posture == PawnPosture.LayingOnGroundNormal || posture == PawnPosture.LayingOnGroundFaceUp || posture == PawnPosture.LayingInBed)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000022E0 File Offset: 0x000004E0
		public void DropBox(Pawn pawn, Apparel box)
		{
            if (pawn != null && box != null && (pawn?.Map) != null && pawn.apparel.TryDrop(box, out Apparel apparel, pawn.Position, true))
            {
                if (apparel != null)
                {
                    ForbidUtility.SetForbidden(apparel, true, false);
                }
                pawn.apparel.Notify_ApparelRemoved(box);
            }
        }

		// Token: 0x04000001 RID: 1
		[NoTranslate]
		public static string SSBox = "SSBox";

		// Token: 0x04000002 RID: 2
		[NoTranslate]
		public static string BoxGraphicRootPath = "Things/Special/StealthBox/" + CardboardBox.SSBox;

		// Token: 0x04000003 RID: 3
		[NoTranslate]
		public string BoxGraphicNorthPath = CardboardBox.BoxGraphicRootPath + "_north";

		// Token: 0x04000004 RID: 4
		[NoTranslate]
		public string BoxGraphicEastPath = CardboardBox.BoxGraphicRootPath + "_east";

		// Token: 0x04000005 RID: 5
		[NoTranslate]
		public string BoxGraphicSouthPath = CardboardBox.BoxGraphicRootPath + "_south";

		// Token: 0x04000006 RID: 6
		[NoTranslate]
		public string BoxGraphicWestPath = CardboardBox.BoxGraphicRootPath + "_west";
	}
}
