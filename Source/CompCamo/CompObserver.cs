using Verse;

namespace Observer;

public class CompObserver : ThingComp
{
    public CompProperties_Observer Props => (CompProperties_Observer)props;
}