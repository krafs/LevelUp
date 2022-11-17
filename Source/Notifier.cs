using RimWorld;
using Verse;

namespace LevelUp;

[StaticConstructorOnStartup]
public static class Notifier
{
    static Notifier()
    {
        // Force initialize Settings to make sure profile is set up.
        _ = LoadedModManager.GetMod<LevelUpMod>().GetSettings<Settings>().Profile;
    }

    public static void OnLevelUp(SkillRecord skillRecord, Pawn pawn)
    {
        Settings.CurrentProfile.LevelUpActionMaker.ExecuteActions(new LevelingInfo(pawn, skillRecord));
    }

    public static void OnLevelDown(SkillRecord skillRecord, Pawn pawn)
    {
        Settings.CurrentProfile.LevelDownActionMaker.ExecuteActions(new LevelingInfo(pawn, skillRecord));
    }
}
