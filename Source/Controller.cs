using Harmony;
using Verse;

namespace LevelUp
{
    class Controller : GameComponent
    {
        public Controller(Game game)
        {
            // Initialize harmony patches.
            HarmonyInstance harmony = HarmonyInstance.Create("LevelUp");
            LevelUp.ApplyPatches(harmony);
        }

        public override void GameComponentOnGUI()
        {
            LevelUp.LevelUpOnGUI();
        }
    }
}