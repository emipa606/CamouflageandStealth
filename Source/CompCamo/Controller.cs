using Mlie;
using UnityEngine;
using Verse;

namespace CompCamo;

public class Controller : Mod
{
    public static Settings Settings;
    public static string currentVersion;

    public Controller(ModContentPack content) : base(content)
    {
        Settings = GetSettings<Settings>();
        currentVersion =
            VersionFromManifest.GetVersionFromModMetaData(
                ModLister.GetActiveModWithIdentifier("Mlie.CamouflageandStealth"));
    }

    public override string SettingsCategory()
    {
        return "CompCamo.Name".Translate();
    }

    public override void DoSettingsWindowContents(Rect canvas)
    {
        Settings.DoWindowContents(canvas);
    }
}