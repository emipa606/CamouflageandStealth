using RimWorld;
using StealthBox;
using Verse;

namespace CompCamo
{
    // Token: 0x0200000E RID: 14
    public class StealthyBox
    {
        // Token: 0x0600005E RID: 94 RVA: 0x00006224 File Offset: 0x00004424
        public static bool IsWearingStealthBox(Pawn pawn, out Apparel box)
        {
            box = null;
            if (pawn?.Map == null || !pawn.Spawned || pawn.apparel == null || pawn.apparel.WornApparelCount <= 0)
            {
                return false;
            }

            foreach (var apparel in pawn.apparel.WornApparel)
            {
                if (apparel is not CardboardBox)
                {
                    continue;
                }

                box = apparel;
                return true;
            }

            return false;
        }
    }
}