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
            var levelEventMaker = new LevelEventMaker(pawnSkillTimerCache);
            SkillRecordLearnPatch.InitializePatch(new Harmony("Krafs.LevelUp"), levelEventMaker);
        }
    }
}