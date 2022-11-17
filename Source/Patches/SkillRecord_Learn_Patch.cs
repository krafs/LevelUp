using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace LevelUp.Patches;

[StaticConstructorOnStartup]
public static class SkillRecord_Learn_Patch
{
    private static readonly MethodInfo onLevelUpMethod;
    private static readonly MethodInfo onLevelDownMethod;
    private static readonly FieldInfo skillRecordLevelField;
    private static readonly FieldInfo skillRecordPawnField;
    private static readonly MethodInfo moteThrowTextMethod;
    private static readonly MethodInfo moteThrowTextProxyMethod;

    static SkillRecord_Learn_Patch()
    {
        Logger.Debug("SkillRecord_Learn_Patch");
        Harmony harmony = new("Krafs.LevelUp");

        skillRecordLevelField = AccessTools.Field(typeof(SkillRecord), nameof(SkillRecord.levelInt));
        skillRecordPawnField = AccessTools.Field(typeof(SkillRecord), "pawn");
        onLevelUpMethod = SymbolExtensions.GetMethodInfo(() => Notifier.OnLevelUp);
        onLevelDownMethod = SymbolExtensions.GetMethodInfo(() => Notifier.OnLevelDown);
        moteThrowTextMethod = SymbolExtensions.GetMethodInfo(() => MoteMaker.ThrowText(default, default, default, default));
        moteThrowTextProxyMethod = SymbolExtensions.GetMethodInfo(() => MoteThrowTextProxy);

        MethodInfo original = SymbolExtensions.GetMethodInfo<SkillRecord>(x => x.Learn(default, default));
        HarmonyMethod transpiler = new(typeof(SkillRecord_Learn_Patch), nameof(SkillRecord_Learn_Patch.LearnTranspilerPatch));

        harmony.Patch(original, transpiler: transpiler);
    }

    private static IEnumerable<CodeInstruction> LearnTranspilerPatch(IEnumerable<CodeInstruction> instructions)
    {
        CodeInstruction previousInstruction = null!;

        foreach (CodeInstruction currentInstruction in instructions)
        {
            // Replace call to Mote.ThrowText with empty to disable text popup. We replace it with our custom popup.
            if (currentInstruction.Calls(moteThrowTextMethod))
            {
                yield return new CodeInstruction(OpCodes.Call, moteThrowTextProxyMethod);
                continue;
            }

            yield return currentInstruction;

            // We react to additions and subtractions to SkillRecord.levelInt. Those are level changes.
            if (currentInstruction.StoresField(skillRecordLevelField))
            {
                MethodBase? onLevelChangeMethod = null;
                // Resolve method to call if this is a level change.
                if (previousInstruction.opcode == OpCodes.Add)
                {
                    onLevelChangeMethod = onLevelUpMethod;
                }
                else if (previousInstruction.opcode == OpCodes.Sub)
                {
                    onLevelChangeMethod = onLevelDownMethod;
                }

                if (onLevelChangeMethod != null)
                {
                    // Put two SkillRecord instances on stack
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Dup);

                    // Put Pawn instance on stack
                    yield return new CodeInstruction(OpCodes.Ldfld, skillRecordPawnField);

                    // Call method that corresponds to level up/level down.
                    yield return new CodeInstruction(OpCodes.Call, onLevelChangeMethod);
                }
            }

            previousInstruction = currentInstruction;
        }
    }

#pragma warning disable IDE0060 // Remove unused parameter

    private static void MoteThrowTextProxy(Vector3 loc, Map map, string text, float timeBeforeStartFadeout)
    { }

#pragma warning restore IDE0060 // Remove unused parameter
}
