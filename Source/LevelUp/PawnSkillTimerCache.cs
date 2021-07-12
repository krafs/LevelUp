using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace LevelUp
{
    public sealed class PawnSkillTimerCache
    {
        public const int MinSecondsBetweenLevels = 20;
        private readonly Dictionary<ValueTuple<Pawn, SkillDef>, DateTime> _timerCache;

        public PawnSkillTimerCache(int capacity)
        {
            _timerCache = new Dictionary<ValueTuple<Pawn, SkillDef>, DateTime>(capacity);
        }

        public bool EnoughTimeHasPassed(Pawn pawn, SkillDef skillDef)
        {
            DateTime currentDateTime = DateTime.Now;
            var key = new ValueTuple<Pawn, SkillDef>(pawn, skillDef);

            if (_timerCache.TryGetValue(key, out DateTime minDateTime) && currentDateTime < minDateTime)
            {
                return false;
            }

            _timerCache[key] = currentDateTime.AddSeconds(MinSecondsBetweenLevels);

            return true;
        }
    }
}