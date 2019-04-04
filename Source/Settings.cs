using Verse;

namespace LevelUp
{
    class Settings : ModSettings
    {
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref allowTextMessage, "AllowTextMessage", true);
            Scribe_Values.Look(ref allowSoundEffect, "AllowSoundEffect", true);
            Scribe_Values.Look(ref allowAnimation, "AllowAnimation", true);
        }

        public static bool allowTextMessage;
        public static bool allowSoundEffect;
        public static bool allowAnimation;
    }
}