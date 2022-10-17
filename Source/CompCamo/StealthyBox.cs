using RimWorld;
using StealthBox;
using Verse;

namespace CompCamo;

public class StealthyBox
{
    public static bool IsWearingStealthBox(Pawn pawn, out Apparel box)
    {
        box = null;
        if (pawn is { Map: null, Spawned: true } || pawn.apparel is not { WornApparelCount: > 0 })
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