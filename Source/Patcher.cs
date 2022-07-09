using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace LevelUp;

public static class Patcher
{
    private static readonly MethodInfo onLevelUpMethod;
    private static readonly MethodInfo onLevelDownMethod;
    private static readonly FieldInfo skillRecordLevelField;
    private static readonly FieldInfo skillRecordPawnField;
    private static readonly MethodInfo moteThrowTextMethod;
    private static readonly MethodInfo moteThrowTextProxyMethod;

    static Patcher()
    {
        skillRecordLevelField = AccessTools.Field(typeof(SkillRecord), nameof(SkillRecord.levelInt));
        skillRecordPawnField = AccessTools.Field(typeof(SkillRecord), "pawn");
        onLevelUpMethod = AccessTools.Method(typeof(Patcher), nameof(OnLevelUp));
        onLevelDownMethod = AccessTools.Method(typeof(Patcher), nameof(OnLevelDown));
        moteThrowTextMethod = AccessTools.Method(typeof(MoteMaker), nameof(MoteMaker.ThrowText),
            new[] { typeof(Vector3), typeof(Map), typeof(string), typeof(float) });
        moteThrowTextProxyMethod = AccessTools.Method(typeof(Patcher), nameof(MoteThrowTextProxy));

        var skillRecordLearnMethod = AccessTools.Method(typeof(SkillRecord), nameof(SkillRecord.Learn));
        var skillRecordLearnTranspilerMethod = AccessTools.Method(typeof(Patcher), nameof(LearnTranspilerPatch));

        new Harmony("Krafs.LevelUp").Patch(skillRecordLearnMethod, transpiler: new HarmonyMethod(skillRecordLearnTranspilerMethod));
    }

    private static IEnumerable<CodeInstruction> LearnTranspilerPatch(IEnumerable<CodeInstruction> instructions)
    {
        CodeInstruction previousInstruction = null!;

        foreach (var currentInstruction in instructions)
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

    private static void OnLevelUp(SkillRecord skillRecord, Pawn pawn)
    {
        Settings.CurrentProfile.LevelUpActionMaker.ExecuteActions(new LevelingInfo(pawn, skillRecord));
    }

    private static void OnLevelDown(SkillRecord skillRecord, Pawn pawn)
    {
        Settings.CurrentProfile.LevelDownActionMaker.ExecuteActions(new LevelingInfo(pawn, skillRecord));
    }

#pragma warning disable IDE0060 // Remove unused parameter

    private static void MoteThrowTextProxy(Vector3 loc, Map map, string text, float timeBeforeStartFadeout)
    { }

#pragma warning restore IDE0060 // Remove unused parameter
}
