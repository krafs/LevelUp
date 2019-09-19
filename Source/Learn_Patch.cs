using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Harmony;
using RimWorld;
using static_quality_plus;
using Verse;
using static LevelUp.LevelEvent;

namespace LevelUp
{
    [StaticConstructorOnStartup]
    internal class Learn_Patch
    {
        private static HarmonyInstance harmony;

        static Learn_Patch()
        {
            harmony = HarmonyInstance.Create("LevelUp");
            MethodInfo original;
            MethodInfo prefix;
            MethodInfo postfix;

            // Static Quality Plus mod uses old-fashioned detouring. Cannot patch the same method without one overriding the other.
            // Running custom pre- and postfixes if mod is in load order.
            bool isStaticQualityPlusActive = ModLister.AllInstalledMods
                .Where(mod => mod.Active)
                .Select(mod => mod.Name)
                .Any(mod => mod.StartsWith("Static Quality Plus", StringComparison.OrdinalIgnoreCase));

            if (isStaticQualityPlusActive)
            {
                Type originalType = null;
                Assembly sqpAssembly = null;

                try
                {
                    sqpAssembly = Assembly.Load("static_quality_plus");
                }
                catch (FileNotFoundException)
                {
                    Log.Error("[Level Up!] Expected to find mod 'Static Quality Plus', but could not find assembly 'static_quality_plus.'");
                }

                foreach (Type type in sqpAssembly.GetTypes())
                {
                    if (type.Name.Equals("_SkillRecord"))
                    {
                        originalType = type;
                        break;
                    }
                }

                original = AccessTools.Method(originalType, "_Learn", new[] { typeof(SkillRecord), typeof(float), typeof(bool) });
                prefix = AccessTools.Method(typeof(Learn_Patch), "Prefix_SQP");
                postfix = AccessTools.Method(typeof(Learn_Patch), "Postfix_SQP");
            }
            else
            {
                original = AccessTools.Method(typeof(SkillRecord), "Learn", new[] { typeof(float), typeof(bool) });
                prefix = AccessTools.Method(typeof(Learn_Patch), "Prefix");
                postfix = AccessTools.Method(typeof(Learn_Patch), "Postfix");
            }

            harmony.Patch(original, new HarmonyMethod(prefix), new HarmonyMethod(postfix));
        }

        // Variant of regular prefix to adapt to SQP's detour.
        private static void Prefix_SQP(out int __state, SkillRecord _this)
        {
            __state = _this.levelInt;
        }

        // Repackages variables from SQP detour and sends on to regular postfix.
        private static void Postfix_SQP(int __state, SkillRecord _this)
        {
            Postfix(__state, _this.levelInt, _this.GetPawn(), _this);
        }

        // Prefix passes the value of skill level before level change.
        private static void Prefix(out int __state, int ___levelInt)
        {
            __state = ___levelInt;
        }

        // If original method increased or decreased skill level, add it to queue for level event notifications.
        private static void Postfix(int __state, int ___levelInt, Pawn ___pawn, SkillRecord __instance)
        {
            if (__state == ___levelInt)
                return;

            if (___pawn.Faction == null)
                return;

            if (!___pawn.Faction.IsPlayer)
                return;

            // Ignores level downs 10 -> 9. Fix for some pawns immediately leveling down again after lvl 10.
            if (__state == 10 && ___levelInt == 9)
                if (Settings.ignoreLvl10To9)
                    return;

            if (!TimerAllowsNotifications(___pawn))
                return;

            if (!LevelRecord.ContainsKey(___pawn))
                LevelRecord.Add(___pawn, new Dictionary<SkillDef, int>());

            if (!LevelRecord[___pawn].ContainsKey(__instance.def))
                LevelRecord[___pawn].Add(__instance.def, ___levelInt);
            else if (LevelRecord[___pawn][__instance.def] == ___levelInt)
                return;

            LevelRecord[___pawn][__instance.def] = ___levelInt;
            LevelEventType levelEventType = __state < ___levelInt ? LevelEventType.LevelUp : LevelEventType.LevelDown;

            LevelEventQueue.Enqueue(new LevelEvent(___pawn, __instance, levelEventType));
        }

        // Checks if enough time has passed since the last time there were level notification about this particular pawn.
        private static bool TimerAllowsNotifications(Pawn pawn)
        {
            var timerDict = PawnTimers;
            Stopwatch timer;
            if (timerDict.ContainsKey(pawn))
            {
                timer = timerDict[pawn];

                if (timer.Elapsed.Seconds > Settings.notificationTimer)
                {
                    timer.Reset();
                    timer.Start();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                timer = new Stopwatch();
                timer.Start();
                timerDict.Add(pawn, timer);
                return true;
            }
        }
    }
}