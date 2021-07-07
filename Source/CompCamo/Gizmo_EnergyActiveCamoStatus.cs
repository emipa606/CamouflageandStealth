using UnityEngine;
using Verse;

namespace CompCamo
{
    // Token: 0x0200000D RID: 13
    [StaticConstructorOnStartup]
    public class Gizmo_EnergyActiveCamoStatus : Gizmo
    {
        // Token: 0x0400001B RID: 27
        private static readonly Texture2D FullCamoBarTex =
            SolidColorMaterials.NewSolidColorTexture(new Color(0.2f, 0.2f, 0.24f));

        // Token: 0x0400001C RID: 28
        private static readonly Texture2D EmptyCamoBarTex = SolidColorMaterials.NewSolidColorTexture(Color.clear);

        // Token: 0x0400001A RID: 26
        public ActiveCamoApparel camo;

        // Token: 0x0600005A RID: 90 RVA: 0x00006080 File Offset: 0x00004280
        public Gizmo_EnergyActiveCamoStatus()
        {
            order = -242f;
        }

        // Token: 0x0600005B RID: 91 RVA: 0x00006093 File Offset: 0x00004293
        public override float GetWidth(float maxWidth)
        {
            return 140f;
        }

        // Token: 0x0600005C RID: 92 RVA: 0x0000609C File Offset: 0x0000429C
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
                (camo.energy * 100f).ToString("F0") + " / " +
                (camo.TryGetComp<CompGearCamo>().Props.CamoEnergyMax * 100f).ToString("F0"));
            Text.Anchor = 0;
            return new GizmoResult(0);
        }
    }
}