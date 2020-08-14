using System;
using HarmonyLib;
using UnityEngine;
using Verse;

namespace CompCamo
{
	// Token: 0x0200001D RID: 29
	[HarmonyPatch(typeof(CompColorableUtility), "SetColor")]
	public class CompColorable_SetColor_PostPatch
	{
		// Token: 0x06000084 RID: 132 RVA: 0x00007060 File Offset: 0x00005260
		[HarmonyPostfix]
		[HarmonyPriority(0)]
		public static void PostFix(Thing t, Color newColor, bool reportFailure = true)
		{
			if (t.def.IsApparel && t.def.defName.StartsWith("CASFlak"))
			{
				CompColorable compColorable = ThingCompUtility.TryGetComp<CompColorable>(t);
				if (compColorable != null)
				{
					ThingDef def = t.def;
					bool flag;
					if (def == null)
					{
						flag = false;
					}
					else
					{
						GraphicData graphicData = def.graphicData;
						Color? color = (graphicData != null) ? new Color?(graphicData.color) : null;
						flag = (color != null);
					}
					if (flag)
					{
						Color color2 = t.def.graphicData.color;
						compColorable.Color = color2;
					}
				}
			}
		}
	}
}
