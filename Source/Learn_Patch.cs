using Harmony;
using RimWorld;
using System.Collections.Generic;
using System.Reflection;
using Verse;
using static LevelUp.LevelEvent;

namespace LevelUp
{
    class Learn_Patch
    {
        // Harmony.
        internal static void ApplyPatches(HarmonyInstance harmony)
        {
            MethodInfo original = AccessTools.Method(typeof(SkillRecord), "Learn", new[] { typeof(float), typeof(bool) });
            HarmonyMethod prefix = new HarmonyMethod(AccessTools.Method(typeof(Learn_Patch), "Prefix"));
            HarmonyMethod postfix = new HarmonyMethod(AccessTools.Method(typeof(Learn_Patch), "Postfix"));
            harmony.Patch(original, prefix, postfix);
        }

        // Prefix passes the value of skill level before level change.
        static void Prefix(out int __state, int ___levelInt)
        {
            __state = ___levelInt;
        }

        // If original method increased skill level, add it to queue for level event notifications.
        static void Postfix(int __state, int ___levelInt, Pawn ___pawn, SkillRecord __instance)
        {
            if (__state == ___levelInt)
                return;
            if (___pawn.Faction == null)
                return;

            if (!___pawn.Faction.IsPlayer)
                return;

            if (!LevelRecord.ContainsKey(___pawn))
                LevelRecord.Add(___pawn, new Dictionary<SkillDef, int>());

            if (!LevelRecord[___pawn].ContainsKey(__instance.def))
            {
                LevelRecord[___pawn].Add(__instance.def, ___levelInt);
            }
            else
            {
                if (LevelRecord[___pawn][__instance.def] == ___levelInt)
                    return;
                LevelRecord[___pawn][__instance.def] = ___levelInt;
            }
            //Log.Message(__state < ___levelInt ? "Increase" : "Decrease");
            LevelEventQueue.Enqueue(new LevelEvent(___pawn, __instance, __state < ___levelInt ? LevelEventType.LevelUp : LevelEventType.LevelDown));
        }
    }
}