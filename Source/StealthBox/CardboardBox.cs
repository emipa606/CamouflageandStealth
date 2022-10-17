using RimWorld;
using UnityEngine;
using Verse;

namespace StealthBox;

public class CardboardBox : Apparel
{
    [NoTranslate] public static string SSBox = "SSBox";

    [NoTranslate] public static string BoxGraphicRootPath = $"Things/Special/StealthBox/{SSBox}";

    [NoTranslate] public string BoxGraphicEastPath = $"{BoxGraphicRootPath}_east";

    [NoTranslate] public string BoxGraphicNorthPath = $"{BoxGraphicRootPath}_north";

    [NoTranslate] public string BoxGraphicSouthPath = $"{BoxGraphicRootPath}_south";

    [NoTranslate] public string BoxGraphicWestPath = $"{BoxGraphicRootPath}_west";

    public float DmgBoxKill => HitPoints;


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

    public override void Tick()
    {
        base.Tick();

        var wearer = Wearer;
        if (wearer?.Map != null && Wearer.Spawned && (Wearer.Downed || Wearer.Dead || JobIsLyingDown(Wearer)))
        {
            DropBox(Wearer, this);
        }
    }


    public override bool CheckPreAbsorbDamage(DamageInfo dinfo)
    {
        if (!dinfo.Def.harmsHealth || !(dinfo.Amount > 0f))
        {
            return true;
        }

        if ((int)dinfo.Amount >= DmgBoxKill)
        {
            Destroy();
            return false;
        }

        HitPoints -= (int)dinfo.Amount;

        return true;
    }

    public override bool AllowVerbCast(Verb verb)
    {
        return true;
    }

    public bool JobIsLyingDown(Pawn pawn)
    {
        if (pawn?.CurJob == null)
        {
            return false;
        }

        var posture = pawn.GetPosture();
        return posture is PawnPosture.LayingOnGroundNormal
            or PawnPosture.LayingOnGroundFaceUp or PawnPosture.LayingInBed;
    }

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