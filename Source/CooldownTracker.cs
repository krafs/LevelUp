using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace LevelUp;

internal sealed class CooldownTracker
{
    private const int CooldownSecondsDefault = 20;

    private Dictionary<int, DateTime> cache = new();
    internal int cooldownSeconds = CooldownSecondsDefault;

    internal bool EnoughTimeHasPassed(LevelingInfo levelingInfo)
    {
        DateTime now = DateTime.UtcNow;

        int key = levelingInfo.Pawn.GetHashCode() + levelingInfo.SkillRecord.def.GetHashCode();

        if (cache.TryGetValue(key, out DateTime lastCacheTime))
        {
            if (lastCacheTime.AddSeconds(cooldownSeconds) > now)
            {
                return false;
            }
        }

        cache[key] = now;

        return true;
    }

    internal void ExposeData()
    {
        Scribe_Values.Look(ref cooldownSeconds, "cooldownSeconds", CooldownSecondsDefault);

        if (Scribe.mode == LoadSaveMode.Saving)
        {
            UpdateCache();
        }
    }

    internal void UpdateCache()
    {
        cache = cache.Where(ShouldKeepEntry).ToDictionary(x => x.Key, x => x.Value);
    }

    private bool ShouldKeepEntry(KeyValuePair<int, DateTime> entry)
    {
        TimeSpan cooldown = TimeSpan.FromSeconds(cooldownSeconds);
        DateTime now = DateTime.UtcNow;

        return entry.Value + cooldown < now;
    }
}
