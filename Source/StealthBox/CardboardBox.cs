using RimWorld;
using UnityEngine;
using Verse;

namespace StealthBox
{
    // Token: 0x02000003 RID: 3
    public class CardboardBox : Apparel
    {
        // Token: 0x04000001 RID: 1
        [NoTranslate] public static string SSBox = "SSBox";

        // Token: 0x04000002 RID: 2
        [NoTranslate] public static string BoxGraphicRootPath = "Things/Special/StealthBox/" + SSBox;

        // Token: 0x04000004 RID: 4
        [NoTranslate] public string BoxGraphicEastPath = BoxGraphicRootPath + "_east";

        // Token: 0x04000003 RID: 3
        [NoTranslate] public string BoxGraphicNorthPath = BoxGraphicRootPath + "_north";

        // Token: 0x04000005 RID: 5
        [NoTranslate] public string BoxGraphicSouthPath = BoxGraphicRootPath + "_south";

        // Token: 0x04000006 RID: 6
        [NoTranslate] public string BoxGraphicWestPath = BoxGraphicRootPath + "_west";

        // Token: 0x17000001 RID: 1
        // (get) Token: 0x06000003 RID: 3 RVA: 0x000020A7 File Offset: 0x000002A7
        public float DmgBoxKill => HitPoints;

        // Token: 0x06000004 RID: 4 RVA: 0x000020B0 File Offset: 0x000002B0

        // Token: 0x06000005 RID: 5 RVA: 0x000020B8 File Offset: 0x000002B8
        public override void DrawWornExtras()
        {
            var wearer = Wearer;
            if (wearer?.Map == null || !Wearer.Spawned)
            {
                return;
            }

            var text = "";
            var rotation = Wearer.Rotation;
            if (rotation == Rot4.North)
            {
                text = BoxGraphicNorthPath;
            }
            else if (rotation == Rot4.East)
            {
                text = BoxGraphicEastPath;
            }
            else if (rotation == Rot4.South)
            {
                text = BoxGraphicSouthPath;
            }
            else if (rotation == Rot4.West)
            {
                text = BoxGraphicWestPath;
            }

            var y = AltitudeLayer.Blueprint.AltitudeFor();
            var drawPos = Wearer.Drawer.DrawPos;
            drawPos.y = y;
            var num = 2.5f;
            var material = MaterialPool.MatFrom(text, true);
            var asAngle = rotation.AsAngle;
            var vector = new Vector3(num, 1f, num);
            Matrix4x4 matrix4x = default;
            matrix4x.SetTRS(drawPos, Quaternion.AngleAxis(asAngle, Vector3.up), vector);
            Graphics.DrawMesh(MeshPool.plane10, matrix4x, material, 0);
        }

        // Token: 0x06000006 RID: 6 RVA: 0x000021D8 File Offset: 0x000003D8
        public override void Tick()
        {
            base.Tick();

            var wearer = Wearer;
            if (wearer?.Map != null && Wearer.Spawned && (Wearer.Downed || Wearer.Dead || JobIsLyingDown(Wearer)))
            {
                DropBox(Wearer, this);
            }
        }

        // Token: 0x06000007 RID: 7 RVA: 0x00002249 File Offset: 0x00000449

        // Token: 0x06000008 RID: 8 RVA: 0x00002254 File Offset: 0x00000454
        public override bool CheckPreAbsorbDamage(DamageInfo dinfo)
        {
            if (!dinfo.Def.harmsHealth || !(dinfo.Amount > 0f))
            {
                return true;
            }

            if ((int) dinfo.Amount >= DmgBoxKill)
            {
                Destroy();
                return false;
            }

            HitPoints -= (int) dinfo.Amount;

            return true;
        }

        // Token: 0x06000009 RID: 9 RVA: 0x000022AD File Offset: 0x000004AD
        public override bool AllowVerbCast(Verb verb)
        {
            return true;
        }

        // Token: 0x0600000A RID: 10 RVA: 0x000022B0 File Offset: 0x000004B0
        public bool JobIsLyingDown(Pawn pawn)
        {
            if (pawn?.CurJob == null)
            {
                return false;
            }

            var posture = pawn.GetPosture();
            if (posture == PawnPosture.LayingOnGroundNormal || posture == PawnPosture.LayingOnGroundFaceUp ||
                posture == PawnPosture.LayingInBed)
            {
                return true;
            }

            return false;
        }

        // Token: 0x0600000B RID: 11 RVA: 0x000022E0 File Offset: 0x000004E0
        public void DropBox(Pawn pawn, Apparel box)
        {
            if (pawn == null || box == null || pawn.Map == null ||
                !pawn.apparel.TryDrop(box, out var apparel, pawn.Position))
            {
                return;
            }

            apparel?.SetForbidden(true, false);

            pawn.apparel.Notify_ApparelRemoved(box);
        }
    }
}