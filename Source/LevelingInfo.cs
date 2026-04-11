using RimWorld;
using Verse;

namespace LevelUp;

internal readonly ref struct LevelingInfo(Pawn pawn, SkillRecord skillRecord)
{
    internal Pawn Pawn => pawn;
    internal SkillRecord SkillRecord => skillRecord;
}
