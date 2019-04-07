using RimWorld;
using System.Linq;
using Verse;
using static LevelUp.LevelEvent;

namespace LevelUp
{
    class Controller : GameComponent
    {
        public Controller(Game game)
        { }

        public override void GameComponentOnGUI()
        {
            if (debug)
                DrawDebugButtons();

            if (LevelEventQueue.Count == 0)
                return;

            LevelEvent levelEvent = LevelEventQueue.Dequeue();
            switch (levelEvent.LevelType)
            {
                case LevelEventType.LevelUp:
                    levelEvent.NotifyLevelUp();
                    break;

                case LevelEventType.LevelDown:
                    Log.Message("LevelDown");
                    levelEvent.NotifyLevelDown();
                    break;
            }
        }

        bool debug = false;

        // DEBUG. Buttons for instantly giving or taking pawn xp.
        void DrawDebugButtons()
        {
            if (Widgets.ButtonText(new UnityEngine.Rect(100f, 100f, 100f, 30f), "-1000 xp"))
            {
                int xp = -1000;
                Pawn pawn = PawnsFinder.AllMaps_Spawned.Where(x => x.IsColonistPlayerControlled).First();
                SkillRecord skill = pawn.skills.skills.First();
                skill.Learn(xp);

                Log.Message(pawn.Label + " lost " + xp + " xp in " + skill.def.label);
            }

            if (Widgets.ButtonText(new UnityEngine.Rect(100f, 150f, 100f, 30f), "+1000 xp"))
            {
                int xp = 1000;
                Pawn pawn = PawnsFinder.AllMaps_Spawned.Where(x => x.IsColonistPlayerControlled).First();
                SkillRecord skill = pawn.skills.skills.First();
                skill.Learn(xp);

                Log.Message(pawn.Label + " gained " + xp + " xp in " + skill.def.label);
            }
        }
    }
}