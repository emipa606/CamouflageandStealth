using Verse;

namespace Observer;

public class CompProperties_Observer : CompProperties
{
    public float SightOffset;

    public CompProperties_Observer()
    {
        compClass = typeof(CompObserver);
    }
}