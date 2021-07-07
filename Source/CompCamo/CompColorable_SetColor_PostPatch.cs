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
}