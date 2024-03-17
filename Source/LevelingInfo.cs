using RimWorld;
using Verse;

namespace LevelUp;

internal readonly ref struct LevelingInfo(Pawn pawn, SkillRecord skillRecord)
{
    internal readonly Pawn Pawn => pawn;
    internal readonly SkillRecord SkillRecord => skillRecord;
}
