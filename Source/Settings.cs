using Verse;

namespace LevelUp
{
    class Settings : ModSettings
    {
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref allowLevelUpTextMessage, "AllowLevelUpTextMessage", true, true);
            Scribe_Values.Look(ref allowLevelUpSoundEffect, "AllowLevelUpSoundEffect", true, true);
            Scribe_Values.Look(ref allowLevelUpAnimation, "AllowLevelUpAnimation", true, true);

            Scribe_Values.Look(ref allowLevelDownTextMessage, "AllowLevelDownTextMessage", false, true);
            Scribe_Values.Look(ref allowLevelDownSoundEffect, "AllowLevelDownSoundEffect", false, true);
            Scribe_Values.Look(ref allowLevelDownAnimation, "AllowLevelDownAnimation", false, true);
        }

        // Level Up

        public static bool allowLevelUpTextMessage;
        public static bool allowLevelUpSoundEffect;
        public static bool allowLevelUpAnimation;

        // Level Down

        public static bool allowLevelDownTextMessage;
        public static bool allowLevelDownSoundEffect;
        public static bool allowLevelDownAnimation;
    }
}