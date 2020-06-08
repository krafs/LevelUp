using HarmonyLib;
using Verse;

namespace LevelUp
{
    [StaticConstructorOnStartup]
    public static class Start
    {
        static Start()
        {
            var pawnSkillTimerCache = new PawnSkillTimerCache(25);
            var modSettings = LoadedModManager.GetMod<ModHandler>().GetSettings<Settings>();
            var levelEventMaker = new LevelEventMaker(pawnSkillTimerCache, modSettings);
            SkillRecordLearnPatch.InitializePatch(new Harmony("Krafs.LevelUp"), levelEventMaker);
        }
    }
}