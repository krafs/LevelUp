using Verse;

namespace LevelUp
{
    class Settings : ModSettings
    {
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref allowLevelUpTextMessage, "AllowLevelUpTextMessage", true);
            Scribe_Values.Look(ref allowLevelUpSoundEffect, "AllowLevelUpSoundEffect", true);
            Scribe_Values.Look(ref allowLevelUpAnimation, "AllowLevelUpAnimation", true);
        }

        public static bool allowLevelUpTextMessage;
        public static bool allowLevelUpSoundEffect;
        public static bool allowLevelUpAnimation;
    }
}