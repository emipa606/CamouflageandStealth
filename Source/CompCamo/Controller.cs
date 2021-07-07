using UnityEngine;
using Verse;

namespace CompCamo
{
    // Token: 0x0200001B RID: 27
    public class Controller : Mod
    {
        // Token: 0x04000027 RID: 39
        public static Settings Settings;

        // Token: 0x06000080 RID: 128 RVA: 0x00006BC9 File Offset: 0x00004DC9
        public Controller(ModContentPack content) : base(content)
        {
            Settings = GetSettings<Settings>();
        }

        // Token: 0x0600007E RID: 126 RVA: 0x00006BAB File Offset: 0x00004DAB
        public override string SettingsCategory()
        {
            return "CompCamo.Name".Translate();
        }

        // Token: 0x0600007F RID: 127 RVA: 0x00006BBC File Offset: 0x00004DBC
        public override void DoSettingsWindowContents(Rect canvas)
        {
            Settings.DoWindowContents(canvas);
        }
    }
}