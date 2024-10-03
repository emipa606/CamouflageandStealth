using HarmonyLib;
using UnityEngine;
using Verse;

namespace CompCamo.Patches;

[HarmonyPatch(typeof(CompColorableUtility), nameof(CompColorableUtility.SetColor))]
public class CompColorableUtility_SetColor
{
    [HarmonyPriority(0)]
    public static void Postfix(Thing t)
    {
        if (!t.def.IsApparel || !t.def.defName.StartsWith("CASFlak"))
        {
            return;
        }

        var compColorable = t.TryGetComp<CompColorable>();
        if (compColorable == null)
        {
            return;
        }

        var def = t.def;
        bool b;
        if (def == null)
        {
            b = false;
        }
        else
        {
            var graphicData = def.graphicData;
            var color = graphicData != null ? new Color?(graphicData.color) : null;
            b = color != null;
        }

        if (!b)
        {
            return;
        }

        var color2 = t.def.graphicData.color;
        compColorable.SetColor(color2);
    }
}