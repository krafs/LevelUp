using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;

namespace LevelUp
{
    public static class SkillRecordLearnPatch
    {
        private static LevelEventMaker LevelEventMaker { get; set; }
        private static MethodBase LevelEventMakerGetter { get; } = AccessTools.PropertyGetter(typeof(SkillRecordLearnPatch), nameof(LevelEventMaker));
        private static readonly MethodInfo s_onLevelUp = SymbolExtensions.GetMethodInfo(() => LevelEventMaker.OnLevelUp(default, default));
        private static readonly MethodInfo s_onLevelDown = SymbolExtensions.GetMethodInfo(() => LevelEventMaker.OnLevelDown(default, default));
        private static readonly FieldInfo s_skillRecordLevelInt = AccessTools.Field(typeof(SkillRecord), nameof(SkillRecord.levelInt));
        private static readonly FieldInfo s_skillRecordPawn = AccessTools.Field(typeof(SkillRecord), "pawn");
        private static readonly MethodInfo s_originalMethod = AccessTools.Method(typeof(SkillRecord), nameof(SkillRecord.Learn));
        private static readonly MethodInfo s_transpilerMethod = SymbolExtensions.GetMethodInfo(() => LearnTranspilerPatch(default));

        public static void InitializePatch(Harmony harmony, LevelEventMaker levelEventMaker)
        {
            LevelEventMaker = levelEventMaker;
            harmony.Patch(s_originalMethod, transpiler: new HarmonyMethod(s_transpilerMethod));
        }

        public static IEnumerable<CodeInstruction> LearnTranspilerPatch(IEnumerable<CodeInstruction> instructions)
        {
            CodeInstruction previousInstruction = default;

            foreach (CodeInstruction currentInstruction in instructions)
            {
                yield return currentInstruction;

                if (currentInstruction.StoresField(s_skillRecordLevelInt))
                {
                    MethodBase onLevelChangeMethod = null;
                    if (previousInstruction.opcode == OpCodes.Add)
                    {
                        onLevelChangeMethod = s_onLevelUp;
                    }
                    else if (previousInstruction.opcode == OpCodes.Sub)
                    {
                        onLevelChangeMethod = s_onLevelDown;
                    }

                    if (onLevelChangeMethod != null)
                    {
                        yield return new CodeInstruction(OpCodes.Call, LevelEventMakerGetter);
                        yield return new CodeInstruction(OpCodes.Ldarg_0);
                        yield return new CodeInstruction(OpCodes.Dup);
                        yield return new CodeInstruction(OpCodes.Ldfld, s_skillRecordPawn);
                        yield return new CodeInstruction(OpCodes.Call, onLevelChangeMethod);
                    }
                }

                previousInstruction = currentInstruction;
            }
        }
    }
}