using RimWorld;
using Verse;

namespace LevelUp
{
    [DefOf]
    public static class DefHandler
    {
        static DefHandler() => DefOfHelper.EnsureInitializedInCtor(typeof(DefHandler));

        public static SoundDef LevelUp;
        public static SoundDef LevelDown;

        public static ThingDef Mote_LevelUpBeamInner;
        public static ThingDef Mote_LevelUpBeamMiddle;
        public static ThingDef Mote_LevelUpBeamOuter;

        public static ThingDef Mote_LevelDownBeamTop;
        public static ThingDef Mote_LevelDownBeamBottom;
    }
}