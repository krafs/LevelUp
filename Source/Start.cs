using Verse;

namespace LevelUp;

[StaticConstructorOnStartup]
public static class Start
{
    static Start()
    {
        var settings = LoadedModManager.GetMod<LevelUpMod>().GetSettings<Settings>();
        Patcher.Patch(settings);
    }
}
