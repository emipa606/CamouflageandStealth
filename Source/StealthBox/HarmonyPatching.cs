using System.Reflection;
using HarmonyLib;
using Verse;

namespace StealthBox;

[StaticConstructorOnStartup]
internal static class HarmonyPatching
{
    static HarmonyPatching()
    {
        new Harmony("com.Pelador.RW.Stealthbox").PatchAll(Assembly.GetExecutingAssembly());
    }
}