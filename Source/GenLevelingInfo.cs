using RimWorld;
using Verse;

namespace LevelUp;

public static class GenLevelingInfo
{
    private static readonly Pawn pawn;
    private static readonly SkillRecord skillRecord;

    static GenLevelingInfo()
    {
        pawn = new Pawn()
        {
            Name = new NameSingle("Levelyn")
        };
        var skillTracker = new Pawn_SkillTracker(pawn);
        pawn.skills = skillTracker;
        skillRecord = skillTracker.skills.RandomElement();
        skillRecord.levelInt = Rand.RangeInclusive(SkillRecord.MinLevel, SkillRecord.MaxLevel - 1);
    }

    public static LevelingInfo Fake => new(pawn, skillRecord);
}
