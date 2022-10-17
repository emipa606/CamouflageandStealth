using UnityEngine;
using Verse;

namespace CompCamo;

[StaticConstructorOnStartup]
public class Gizmo_EnergyActiveCamoStatus : Gizmo
{
    private static readonly Texture2D FullCamoBarTex =
        SolidColorMaterials.NewSolidColorTexture(new Color(0.2f, 0.2f, 0.24f));

    private static readonly Texture2D EmptyCamoBarTex = SolidColorMaterials.NewSolidColorTexture(Color.clear);

    public ActiveCamoApparel camo;

    public Gizmo_EnergyActiveCamoStatus()
    {
        base.Order = -242f;
    }

    public override float GetWidth(float maxWidth)
    {
        return 140f;
    }

    public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
    {
        var rect = new Rect(topLeft.x, topLeft.y, GetWidth(maxWidth), 75f);
        Widgets.DrawWindowBackground(rect);
        Rect rect3;
        var rect2 = rect3 = rect.ContractedBy(6f);
        rect3.height = rect.height / 2f;
        Text.Font = 0;
        Widgets.Label(rect3, camo.LabelCap);
        var rect4 = rect2;
        rect4.yMin = rect3.yMin + (rect.height / 2f) - 6f;
        rect4.height = (rect.height / 2f) - 6f;
        var num = camo.energy / Mathf.Max(1f, camo.TryGetComp<CompGearCamo>().Props.CamoEnergyMax);
        Widgets.FillableBar(rect4, num, FullCamoBarTex, EmptyCamoBarTex, false);
        Text.Font = GameFont.Small;
        Text.Anchor = TextAnchor.MiddleCenter;
        Widgets.Label(rect4,
            $"{camo.energy * 100f:F0} / {camo.TryGetComp<CompGearCamo>().Props.CamoEnergyMax * 100f:F0}");
        Text.Anchor = 0;
        return new GizmoResult(0);
    }
}