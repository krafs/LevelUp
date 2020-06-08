using Verse;

namespace LevelUp
{
    public class Settings : ModSettings
    {
        public bool DoLevelUp = true;
        public bool DoLevelDown;
        public bool DoLevelUpMessage = true;
        public bool DoLevelDownMessage;
        public bool DoLevelUpAnimation = true;
        public bool DoLevelDownAnimation;
        public bool DoLevelUpSound = true;
        public bool DoLevelDownSound;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref DoLevelUp, nameof(DoLevelUp), true);
            Scribe_Values.Look(ref DoLevelDown, nameof(DoLevelDown), false);
            Scribe_Values.Look(ref DoLevelUpMessage, nameof(DoLevelUpMessage), true);
            Scribe_Values.Look(ref DoLevelDownMessage, nameof(DoLevelDownMessage), false);
            Scribe_Values.Look(ref DoLevelUpAnimation, nameof(DoLevelUpAnimation), true);
            Scribe_Values.Look(ref DoLevelDownAnimation, nameof(DoLevelDownAnimation), false);
            Scribe_Values.Look(ref DoLevelUpSound, nameof(DoLevelUpSound), true);
            Scribe_Values.Look(ref DoLevelDownSound, nameof(DoLevelDownSound), false);
        }
    }
}