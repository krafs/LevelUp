using Verse;

namespace LevelUp
{
    [StaticConstructorOnStartup]
    public static class Start
    {
        static Start()
        {
            Settings settings = LoadedModManager.GetMod<LevelUpMod>().GetSettings<Settings>();
            Patcher.Patch(settings);
        }
    }
}