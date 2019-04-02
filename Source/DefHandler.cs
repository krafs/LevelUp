using RimWorld;
using Verse;

namespace LevelUp
{
    [DefOf]
    public static class DefHandler
    {
        static DefHandler() => DefOfHelper.EnsureInitializedInCtor(typeof(DefHandler));

        public static SoundDef LevelUp;
        public static ThingDef Mote_LevelBeamInner;
        public static ThingDef Mote_LevelBeamMiddle;
        public static ThingDef Mote_LevelBeamOuter;
    }
}