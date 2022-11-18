using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace LevelUp;

internal sealed class CooldownTracker
{
    private Dictionary<int, DateTime> cache = new();
    internal int CooldownSecondsDefault;
    internal int CooldownSeconds;

    internal bool EnoughTimeHasPassed(LevelingInfo levelingInfo)
    {
        DateTime now = DateTime.UtcNow;

        int key = levelingInfo.Pawn.GetHashCode() + levelingInfo.SkillRecord.def.GetHashCode();

        if (cache.TryGetValue(key, out DateTime lastCacheTime))
        {
            if (lastCacheTime.AddSeconds(CooldownSeconds) > now)
            {
                return false;
            }
        }

        cache[key] = now;

        return true;
    }

    internal void ExposeData()
    {
        Scribe_Values.Look(ref CooldownSeconds, "cooldownSeconds", CooldownSecondsDefault);
    }

    internal void Maintain()
    {
        // TODO: Remove from existing instance instead of creating new.
        cache = cache.Where(ShouldKeepEntry).ToDictionary(x => x.Key, x => x.Value);
    }

    private bool ShouldKeepEntry(KeyValuePair<int, DateTime> entry)
    {
        TimeSpan cooldown = TimeSpan.FromSeconds(CooldownSeconds);
        DateTime now = DateTime.UtcNow;

        return entry.Value + cooldown < now;
    }
}
