using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace LevelUp;

public static class PawnSkillTimerCache
{
    private static readonly Dictionary<ValueTuple<Pawn, SkillDef>, DateTime> timerCache = [];
    private static readonly Settings settings = LoadedModManager.GetMod<LevelUpMod>().GetSettings<Settings>();

    public static bool EnoughTimeHasPassed(LevelingInfo levelingInfo)
    {
        DateTime currentDateTime = DateTime.UtcNow;
        (Pawn, SkillDef) key = new(levelingInfo.Pawn, levelingInfo.SkillRecord.def);
        if (timerCache.TryGetValue(key, out DateTime lastEntryDateTime))
        {
            DateTime nextAllowedDateTime = lastEntryDateTime.AddSeconds(settings.Profile.GeneralSettingsContent.CooldownSeconds);
            if (currentDateTime < nextAllowedDateTime)
            {
                return false;
            }
        }

        timerCache[key] = currentDateTime;

        return true;
    }
}
