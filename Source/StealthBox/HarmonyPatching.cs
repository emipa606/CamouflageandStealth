using System.Reflection;
using HarmonyLib;
using Verse;

namespace StealthBox
{
    // Token: 0x02000004 RID: 4
    [StaticConstructorOnStartup]
    internal static class HarmonyPatching
    {
        // Token: 0x0600000E RID: 14 RVA: 0x000023B7 File Offset: 0x000005B7
        static HarmonyPatching()
        {
            new Harmony("com.Pelador.RW.Stealthbox").PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}