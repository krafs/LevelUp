using RimWorld;
using Verse;

namespace LevelUp;

public readonly ref struct LevelingInfo(Pawn pawn, SkillRecord skillRecord)
{
    public readonly Pawn Pawn => pawn;
    public readonly SkillRecord SkillRecord => skillRecord;
}
