using System.Reflection;
using HarmonyLib;
using Verse;

namespace Observer;

[StaticConstructorOnStartup]
internal static class HarmonyPatching
{
    static HarmonyPatching()
    {
        new Harmony("com.Pelador.RW.Observer").PatchAll(Assembly.GetExecutingAssembly());
    }
}