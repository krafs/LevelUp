using Verse;
using static LevelUp.DefHandler;

namespace LevelUp
{
    class Settings : ModSettings
    {
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref allowLevelUpTextMessage, "AllowLevelUpTextMessage", true, true);
            Scribe_Values.Look(ref allowLevelUpLetter, "AllowLevelUpLetter", false, true);
            Scribe_Values.Look(ref allowLevelUpSoundEffect, "AllowLevelUpSoundEffect", true, true);
            Scribe_Values.Look(ref allowLevelUpAnimation, "AllowLevelUpAnimation", true, true);

            Scribe_Values.Look(ref allowLevelDownTextMessage, "AllowLevelDownTextMessage", false, true);
            Scribe_Values.Look(ref allowLevelDownLetter, "AllowLevelDownLetter", false, true);
            Scribe_Values.Look(ref allowLevelDownSoundEffect, "AllowLevelDownSoundEffect", false, true);
            Scribe_Values.Look(ref allowLevelDownAnimation, "AllowLevelDownAnimation", false, true);

            Scribe_Values.Look(ref ignoreLvl10To9, "Ignore10To9Label", false, true);

            Scribe_Values.Look(ref LvlUpSound, "LevelUpSoundDef", Sound.LevelUp, true);
        }

        // Level Up

        public static bool allowLevelUpTextMessage = true;
        public static bool allowLevelUpLetter;
        public static bool allowLevelUpAnimation = true;
        public static bool allowLevelUpSoundEffect = true;

        public static Sound LvlUpSound;

        // Level Down

        public static bool allowLevelDownTextMessage;
        public static bool allowLevelDownLetter;
        public static bool allowLevelDownSoundEffect;
        public static bool allowLevelDownAnimation;

        public static bool ignoreLvl10To9;
    }
}