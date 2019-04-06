using Harmony;
using Verse;
using static LevelUp.LevelEvent;

namespace LevelUp
{
    class Controller : GameComponent
    {
        public Controller(Game game)
        {
            // Initialize harmony patches.
            HarmonyInstance harmony = HarmonyInstance.Create("LevelUp");
            Learn_Patch.ApplyPatches(harmony);
        }

        public override void GameComponentOnGUI()
        {
            if (LevelEventQueue.Count == 0)
                return;

            LevelEvent levelEvent = LevelEventQueue.Dequeue();
            switch (levelEvent.LevelType)
            {
                case LevelEventType.LevelUp:
                    levelEvent.NotifyLevelUp();
                    break;

                case LevelEventType.LevelDown:
                    levelEvent.NotifyLevelDown();
                    break;
            }
        }
    }
}