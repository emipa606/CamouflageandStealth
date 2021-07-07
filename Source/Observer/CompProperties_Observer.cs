using Verse;

namespace Observer
{
    // Token: 0x02000004 RID: 4
    public class CompProperties_Observer : CompProperties
    {
        // Token: 0x04000001 RID: 1
        public float SightOffset;

        // Token: 0x06000005 RID: 5 RVA: 0x000020A7 File Offset: 0x000002A7
        public CompProperties_Observer()
        {
            compClass = typeof(CompObserver);
        }
    }
}