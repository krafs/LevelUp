using Verse;

namespace LevelUp
{
    public class Settings : ModSettings
    {
        public bool DoLevelUp = true;
        public bool DoLevelDown;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref DoLevelUp, nameof(DoLevelUp));
            Scribe_Values.Look(ref DoLevelDown, nameof(DoLevelDown));
        }
    }
}