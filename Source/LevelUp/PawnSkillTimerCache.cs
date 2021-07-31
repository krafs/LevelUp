using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace LevelUp
{
    public static class PawnSkillTimerCache
    {
        private static readonly Dictionary<ValueTuple<Pawn, SkillDef>, DateTime> _timerCache;
        private static readonly Settings settings;

        static PawnSkillTimerCache()
        {
            settings = LoadedModManager.GetMod<LevelUpMod>().GetSettings<Settings>();
            _timerCache = new Dictionary<ValueTuple<Pawn, SkillDef>, DateTime>();
        }

        public static bool EnoughTimeHasPassed(LevelingInfo levelingInfo)
        {
            DateTime currentDateTime = DateTime.UtcNow;
            var key = new ValueTuple<Pawn, SkillDef>(levelingInfo.Pawn, levelingInfo.SkillRecord.def);
            if (_timerCache.TryGetValue(key, out DateTime lastEntryDateTime))
            {
                DateTime nextAllowedDateTime = lastEntryDateTime.AddSeconds(settings.Profile.GeneralSettingsContent.CooldownSeconds);
                if (currentDateTime < nextAllowedDateTime)
                {
                    return false;
                }
            }

            _timerCache[key] = currentDateTime;

            return true;
        }
    }
}