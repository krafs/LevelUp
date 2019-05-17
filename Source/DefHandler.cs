using RimWorld;
using Verse;

namespace LevelUp
{
    [DefOf]
    public static class DefHandler
    {
        public enum Sound
        {
            LevelUp,
            LevelUp2
        }

        public static SoundDef GetSound(Sound sound)
        {
            return sound == Sound.LevelUp2 ? LevelUp2 : LevelUp;
        }

        static DefHandler() => DefOfHelper.EnsureInitializedInCtor(typeof(DefHandler));

        public static SoundDef LevelUp;
        public static SoundDef LevelUp2;
        public static SoundDef LevelDown;

        public static ThingDef Mote_LevelUpBeamInner;
        public static ThingDef Mote_LevelUpBeamMiddle;
        public static ThingDef Mote_LevelUpBeamOuter;

        public static ThingDef Mote_LevelDownBeamTop;
        public static ThingDef Mote_LevelDownBeamBottom;
    }
}