using System.Reflection;
using HarmonyLib;
using Verse;

namespace CompCamo;

[StaticConstructorOnStartup]
internal static class HarmonyPatching
{
    static HarmonyPatching()
    {
        new Harmony("com.Pelador.RW.CompCamo").PatchAll(Assembly.GetExecutingAssembly());
    }
}