using Harmony;
using RimWorld;
using static_quality_plus;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Verse;
using static LevelUp.LevelEvent;

namespace LevelUp
{
    [StaticConstructorOnStartup]
    class Learn_Patch
    {
        static HarmonyInstance harmony;

        static Learn_Patch()
        {
            harmony = HarmonyInstance.Create("LevelUp");
            MethodInfo original;
            MethodInfo prefix;
            MethodInfo postfix;

            // Static Quality Plus mod uses old-fashioned detouring. Cannot patch the same method without one overriding the other.
            // Running custom pre- and postfixes if mod is in load order.
            if (IsModActive("Static Quality Plus 1.1"))
            {
                Type originalType = null;
                Assembly sqpAssembly = null;

                try
                {
                    sqpAssembly = Assembly.Load("static_quality_plus");
                }
                catch (FileNotFoundException)
                {
                    Log.Error("[Level Up!] Expected to find mod 'Static Quality Plus 1.1', but could not find assembly 'static_quality_plus.'");
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

        // Checks if mod is in active load order.
        static bool IsModActive(string modName)
        {
            foreach (ModMetaData modMetaData in ModLister.AllInstalledMods)
                if (modMetaData.Active && modName.Equals(modMetaData.Name))
                    return true;
            return false;
        }

        // Variant of regular prefix to adapt to SQP's detour.
        static void Prefix_SQP(out int __state, SkillRecord _this)
        {
            __state = _this.levelInt;
        }

        // Repackages variables from SQP detour and sends on to regular postfix.
        static void Postfix_SQP(int __state, SkillRecord _this)
        {
            Postfix(__state, _this.levelInt, _this.GetPawn(), _this);
        }

        // Prefix passes the value of skill level before level change.
        static void Prefix(out int __state, int ___levelInt)
        {
            __state = ___levelInt;
        }

        // If original method increased or decreased skill level, add it to queue for level event notifications.
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

            LevelEventQueue.Enqueue(new LevelEvent(___pawn, __instance, __state < ___levelInt ? LevelEventType.LevelUp : LevelEventType.LevelDown));
        }
    }
}