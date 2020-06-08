using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace LevelUp
{
    public static class SkillRecordLearnPatch
    {
        private static LevelEventMaker LevelEventMaker { get; set; }
        private static MethodBase LevelEventMakerGetter { get; } = AccessTools.PropertyGetter(typeof(SkillRecordLearnPatch), nameof(LevelEventMaker));
        private static MethodInfo onLevelUp = SymbolExtensions.GetMethodInfo(() => LevelEventMaker.OnLevelUp(default, default));
        private static MethodInfo onLevelDown = SymbolExtensions.GetMethodInfo(() => LevelEventMaker.OnLevelDown(default, default));
        private static FieldInfo skillRecordLevelInt = AccessTools.Field(typeof(SkillRecord), nameof(SkillRecord.levelInt));
        private static FieldInfo skillRecordPawn = AccessTools.Field(typeof(SkillRecord), "pawn");
        private static MethodInfo originalMethod = AccessTools.Method(typeof(SkillRecord), nameof(SkillRecord.Learn));
        private static MethodInfo transpilerMethod = SymbolExtensions.GetMethodInfo(() => LearnTranspilerPatch(default));

        public static void InitializePatch(Harmony harmony, LevelEventMaker levelEventMaker)
        {
            LevelEventMaker = levelEventMaker;
            harmony.Patch(originalMethod, transpiler: new HarmonyMethod(transpilerMethod));
        }

        public static IEnumerable<CodeInstruction> LearnTranspilerPatch(IEnumerable<CodeInstruction> instructions)
        {
            CodeInstruction previousInstruction = default;

            foreach (var currentInstruction in instructions)
            {
                yield return currentInstruction;

                if (currentInstruction.StoresField(skillRecordLevelInt))
                {
                    MethodBase onLevelChangeMethod = null;
                    if (previousInstruction.opcode == OpCodes.Add)
                    {
                        onLevelChangeMethod = onLevelUp;
                    }
                    else if (previousInstruction.opcode == OpCodes.Sub)
                    {
                        onLevelChangeMethod = onLevelDown;
                    }

                    if (onLevelChangeMethod != null)
                    {
                        yield return new CodeInstruction(OpCodes.Call, LevelEventMakerGetter);
                        yield return new CodeInstruction(OpCodes.Ldarg_0);
                        yield return new CodeInstruction(OpCodes.Dup);
                        yield return new CodeInstruction(OpCodes.Ldfld, skillRecordPawn);
                        yield return new CodeInstruction(OpCodes.Call, onLevelChangeMethod);
                    }
                }

                previousInstruction = currentInstruction;
            }
        }
    }
}