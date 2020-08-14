using System;
using UnityEngine;
using Verse;

namespace CompCamo
{
	// Token: 0x0200000D RID: 13
	[StaticConstructorOnStartup]
	public class Gizmo_EnergyActiveCamoStatus : Gizmo
	{
		// Token: 0x0600005A RID: 90 RVA: 0x00006080 File Offset: 0x00004280
		public Gizmo_EnergyActiveCamoStatus()
		{
			this.order = -242f;
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00006093 File Offset: 0x00004293
		public override float GetWidth(float maxWidth)
		{
			return 140f;
		}

		// Token: 0x0600005C RID: 92 RVA: 0x0000609C File Offset: 0x0000429C
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth)
		{
			Rect rect = new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f);
			Widgets.DrawWindowBackground(rect);
			Rect rect3;
			Rect rect2 = rect3 = GenUI.ContractedBy(rect, 6f);
			rect3.height = rect.height / 2f;
			Text.Font = 0;
			Widgets.Label(rect3, this.camo.LabelCap);
			Rect rect4 = rect2;
			rect4.yMin = rect3.yMin + rect.height / 2f - 6f;
			rect4.height = rect.height / 2f - 6f;
			float num = this.camo.energy / Mathf.Max(1f, ThingCompUtility.TryGetComp<CompGearCamo>(this.camo).Props.CamoEnergyMax);
			Widgets.FillableBar(rect4, num, Gizmo_EnergyActiveCamoStatus.FullCamoBarTex, Gizmo_EnergyActiveCamoStatus.EmptyCamoBarTex, false);
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect4, (this.camo.energy * 100f).ToString("F0") + " / " + (ThingCompUtility.TryGetComp<CompGearCamo>(this.camo).Props.CamoEnergyMax * 100f).ToString("F0"));
			Text.Anchor = 0;
			return new GizmoResult(0);
		}

		// Token: 0x0400001A RID: 26
		public ActiveCamoApparel camo;

		// Token: 0x0400001B RID: 27
		private static readonly Texture2D FullCamoBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.2f, 0.2f, 0.24f));

		// Token: 0x0400001C RID: 28
		private static readonly Texture2D EmptyCamoBarTex = SolidColorMaterials.NewSolidColorTexture(Color.clear);
	}
}
