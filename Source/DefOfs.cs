using RimWorld;
using Verse;

namespace LevelUp;

[DefOf]
internal static class DefOfs
{
    static DefOfs()
    {
        DefOfHelper.EnsureInitializedInCtor(typeof(DefOfs));
    }

    [DefAlias("LevelUpAnimation_Radiance")]
    public static FleckDef Radiance = null!;

    [DefAlias("LevelUpAnimation_Drain")]
    public static FleckDef Drain = null!;

    [DefAlias("LevelUpSound_Ding")]
    public static SoundDef Ding = null!;

    [DefAlias("LevelUpSound_Negative")]
    public static SoundDef Negative = null!;
}
