using RimWorld;
using Verse;

namespace LevelUp
{
    public readonly ref struct LevelingInfo
    {
        public readonly Pawn Pawn { get; }
        public readonly SkillRecord SkillRecord { get; }

        public LevelingInfo(Pawn pawn, SkillRecord skillRecord)
        {
            Pawn = pawn;
            SkillRecord = skillRecord;
        }
    }
}