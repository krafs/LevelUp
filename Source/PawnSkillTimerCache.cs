using RimWorld;
using System;
using System.Collections.Generic;
using Verse;

namespace LevelUp
{
    public class PawnSkillTimerCache
    {
        public const int MinSecondsBetweenLevels = 20;
        private readonly Dictionary<Tuple<Pawn, SkillDef>, DateTime> timerCache;

        public PawnSkillTimerCache(int capacity)
        {
            this.timerCache = new Dictionary<Tuple<Pawn, SkillDef>, DateTime>(capacity);
        }

        public bool EnoughTimeHasPassed(Pawn pawn, SkillDef skillDef)
        {
            var currentDateTime = DateTime.Now;
            var key = Tuple.Create(pawn, skillDef);

            if (timerCache.TryGetValue(key, out DateTime minDateTime) && currentDateTime < minDateTime)
            {
                return false;
            }

            timerCache[key] = currentDateTime.AddSeconds(MinSecondsBetweenLevels);
            return true;
        }
    }
}