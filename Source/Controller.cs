using Harmony;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Verse;

namespace LevelUp
{
    [StaticConstructorOnStartup]
    class Controller
    {
        internal static Texture2D tex = ContentFinder<Texture2D>.Get("UI/Widgets/FillChangeArrowRight");

        static Controller()
        {
            // Do patches
            HarmonyInstance harmony = HarmonyInstance.Create("LevelUp");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}