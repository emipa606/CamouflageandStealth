using System.Reflection;
using HarmonyLib;
using Multiplayer.API;
using Verse;

namespace CompCamo;

[StaticConstructorOnStartup]
internal static class MultiplayerSupport
{
    private static readonly Harmony harmony = new Harmony("rimworld.pelador.compcamo.multiplayersupport");

    static MultiplayerSupport()
    {
        if (!MP.enabled)
        {
            return;
        }

        MP.RegisterSyncMethod(typeof(ActiveCamoApparel), "ToggleActiveCamo");
        MethodInfo[] array =
        {
            AccessTools.Method(typeof(CamoUtility), "Rnd100"),
            AccessTools.Method(typeof(ActiveCamoApparel), "Break")
        };
        foreach (var methodInfo in array)
        {
            FixRNG(methodInfo);
        }
    }

    private static void FixRNG(MethodInfo method)
    {
        harmony.Patch(method, new HarmonyMethod(typeof(MultiplayerSupport), "FixRNGPre"),
            new HarmonyMethod(typeof(MultiplayerSupport), "FixRNGPos"));
    }

    private static void FixRNGPre()
    {
        Rand.PushState(Find.TickManager.TicksAbs);
    }

    private static void FixRNGPos()
    {
        Rand.PopState();
    }
}