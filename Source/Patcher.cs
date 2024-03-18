using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using UnityEngine;
using Verse;

namespace LevelUp;

internal static class Patcher
{
    internal static void ApplyPatches(Harmony harmony)
    {
        MethodInfo skillRecordLearnOriginal = AccessTools.Method(typeof(SkillRecord), nameof(SkillRecord.Learn));
        MethodInfo skillRecordDirtyAptitudesOriginal = AccessTools.Method(typeof(SkillRecord), nameof(SkillRecord.DirtyAptitudes));
        MethodInfo compUseEffectLearnSkillDoEffectOriginal = AccessTools.Method(typeof(CompUseEffect_LearnSkill), nameof(CompUseEffect_LearnSkill.DoEffect));

        HarmonyMethod prefix = new(typeof(Patcher), nameof(Prefix));
        HarmonyMethod dirtyAptitudesPostfix = new(typeof(Patcher), nameof(DirtyAptitudesPostfix));
        HarmonyMethod learnPostfix = new(typeof(Patcher), nameof(LearnPostfix));

        HarmonyMethod removeMoteThrowCallTranspiler = new(typeof(Patcher), nameof(RemoveMoteThrowCall));
        HarmonyMethod removeMessageCallTranspiler = new(typeof(Patcher), nameof(RemoveMessageCall));

        harmony.Patch(skillRecordLearnOriginal, prefix, learnPostfix, removeMoteThrowCallTranspiler);
        harmony.Patch(skillRecordDirtyAptitudesOriginal, prefix, dirtyAptitudesPostfix);

        harmony.Patch(compUseEffectLearnSkillDoEffectOriginal, transpiler: removeMessageCallTranspiler);
    }

    [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Harmony naming convention")]
    private static void Prefix(out int __state, SkillRecord __instance)
    {
        __state = __instance.Level;
    }

    [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Harmony naming convention")]
    private static void DirtyAptitudesPostfix(int __state, SkillRecord __instance, Pawn ___pawn)
    {
        // DirtyAptitudes can be called on the Create Character-screen if Biotech is used,
        // and either crashes or makes it impossible to move forward.
        // This causes the mod to try and display notifications for a colonist when not yet in a playable program state.
        if (Current.ProgramState == ProgramState.Entry)
        {
            return;
        }

        int previousLevel = __state;
        int currentLevel = __instance.Level;

        if (currentLevel == previousLevel)
        {
            return;
        }
        else if (currentLevel > previousLevel)
        {
            Settings.profile.levelUpActionMaker.ExecuteActions(new LevelingInfo(___pawn, __instance));
        }
        else
        {
            Settings.profile.levelDownActionMaker.ExecuteActions(new LevelingInfo(___pawn, __instance));
        }
    }

    [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Harmony naming convention")]
    private static void LearnPostfix(int __state, SkillRecord __instance, Pawn ___pawn, bool direct)
    {
        int previousLevel = __state;
        int currentLevel = __instance.Level;

        if (currentLevel == previousLevel)
        {
            return;
        }

        bool isProbablyNaturalLearningOrDecay = !direct;
        if (isProbablyNaturalLearningOrDecay)
        {
            // Likely unnecessarily optimized cache :/
            int currentTime = (int)Time.time;
            int pawnHash = ___pawn.GetHashCode();
            ushort defHash = __instance.def.shortHash;
            // Ensure that defHash fits within the upper 16 bits
            pawnHash &= 0xFFFF; // Mask the upper 16 bits to ensure they are clear

            // Combine pawnHash and defHash
            int key = (pawnHash << 16) | defHash;
            if (Settings.timerCache.TryGetValue(key, out int nextAllowedTime))
            {
                bool enoughTimeHasPassed = currentTime > nextAllowedTime;
                if (enoughTimeHasPassed)
                {
                    Settings.timerCache.Remove(key);
                }
                else
                {
                    return;
                }
            }

            Settings.timerCache[key] = currentTime + 20;
        }

        LevelingInfo levelingInfo = new(___pawn, __instance);
        if (currentLevel > previousLevel)
        {
            Settings.profile.levelUpActionMaker.ExecuteActions(levelingInfo);
        }
        else
        {
            Settings.profile.levelDownActionMaker.ExecuteActions(levelingInfo);
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
