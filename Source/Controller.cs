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
        public override void FinalizeInit()
        {
            base.FinalizeInit();
            LevelUp.Clear();
        }

        public override void GameComponentOnGUI()
        {
            base.GameComponentOnGUI();
            LevelUp.LevelUpOnGUI();
        }
    }
}