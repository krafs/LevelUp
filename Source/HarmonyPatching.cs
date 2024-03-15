using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace LevelUp;

[SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Injected Harmony parameters must be prefixed with underscores")]
public static class HarmonyPatching
{
    internal static void ApplyPatches(Harmony harmony)
    {
        MethodInfo skillRecordLearnOriginal = AccessTools.Method(typeof(SkillRecord), nameof(SkillRecord.Learn));
        MethodInfo skillRecordDirtyAptitudesOriginal = AccessTools.Method(typeof(SkillRecord), nameof(SkillRecord.DirtyAptitudes));
        MethodInfo compUseEffectLearnSkillDoEffectOriginal = AccessTools.Method(typeof(CompUseEffect_LearnSkill), nameof(CompUseEffect_LearnSkill.DoEffect));

        HarmonyMethod prefix = new(typeof(HarmonyPatching), nameof(Prefix));
        HarmonyMethod postfix = new(typeof(HarmonyPatching), nameof(Postfix));

        HarmonyMethod removeMoteThrowCallTranspiler = new(typeof(HarmonyPatching), nameof(RemoveMoteThrowCall));
        HarmonyMethod removeMessageCallTranspiler = new(typeof(HarmonyPatching), nameof(RemoveMessageCall));

        harmony.Patch(skillRecordLearnOriginal, prefix, postfix, removeMoteThrowCallTranspiler);
        harmony.Patch(skillRecordDirtyAptitudesOriginal, prefix, postfix);

        harmony.Patch(compUseEffectLearnSkillDoEffectOriginal, transpiler: removeMessageCallTranspiler);
    }

    private static void Prefix(out int __state, SkillRecord __instance)
    {
        __state = __instance.Level;
    }

    private static void Postfix(int __state, Pawn ___pawn, SkillRecord __instance)
    {
        int previousLevel = __state;
        int currentLevel = __instance.Level;

        if (currentLevel == previousLevel)
        {
            return;
        }
        else if (currentLevel > previousLevel)
        {
            Notifier.OnLevelUp(__instance, ___pawn);
        }
        else
        {
            Notifier.OnLevelDown(__instance, ___pawn);
        }
    }

    private static IEnumerable<CodeInstruction> RemoveMoteThrowCall(IEnumerable<CodeInstruction> instructions)
    {
        MethodInfo original = SymbolExtensions.GetMethodInfo(() => MoteMaker.ThrowText(default, default, default, default));
        MethodInfo replacement = SymbolExtensions.GetMethodInfo(() => MoteThrowTextProxy);

        return instructions.MethodReplacer(original, replacement);
    }

    private static IEnumerable<CodeInstruction> RemoveMessageCall(IEnumerable<CodeInstruction> instructions)
    {
        MethodInfo original = SymbolExtensions.GetMethodInfo(() => Messages.Message(default, default, default, default));
        MethodInfo replacement = SymbolExtensions.GetMethodInfo(() => MessageProxy);

        return instructions.MethodReplacer(original, replacement);
    }

    private static void MoteThrowTextProxy(Vector3 loc, Map map, string text, float timeBeforeStartFadeout)
    { }

    private static void MessageProxy(string text, LookTargets lookTargets, MessageTypeDef def, bool historical)
    { }
}
