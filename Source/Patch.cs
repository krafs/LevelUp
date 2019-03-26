using Harmony;
using RimWorld;
using System;
using Verse;

namespace LevelUp
{
    [HarmonyPatch(typeof(SkillRecord))]
    [HarmonyPatch("Learn")]
    [HarmonyPatch(new Type[] { typeof(float), typeof(bool) })]
    //[HarmonyPatch("Level", MethodType.Setter)]
    class Patch
    {
        static void Prefix(out int __state, int ___levelInt)
        {
            __state = ___levelInt;
        }

        static void Postfix(int __state, int ___levelInt, Pawn ___pawn, SkillDef ___def)
        {
            if (__state < ___levelInt)
                Log.Message(___pawn.Label + " leveled up " + ___def.skillLabel + " Lvl " + ___levelInt.ToString());
        }
    }
}