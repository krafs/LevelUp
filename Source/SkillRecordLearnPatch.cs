using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Verse;

namespace LevelUp
{
    public static class SkillRecordLearnPatch
    {
        private static PatchProcessor emptyPatchProcessor;

        private static MethodBase levelEventMakerGetter;

        private static Settings modSettings;

        private static MethodBase onLevelDown;

        private static MethodBase onLevelUp;

        private static FieldInfo skillRecordLevelInt;

        private static FieldInfo skillRecordPawn;

        private static MethodBase OnLevelDown => onLevelDown ??=
            SymbolExtensions.GetMethodInfo(() => LevelEventMaker.OnLevelDown(default, default, default));

        private static MethodBase OnLevelUp => onLevelUp ??=
            SymbolExtensions.GetMethodInfo(() => LevelEventMaker.OnLevelUp(default, default, default));

        private static LevelEventMaker LevelEventMaker { get; set; }

        private static MethodBase LevelEventMakerGetter => levelEventMakerGetter ??=
            AccessTools.PropertyGetter(typeof(SkillRecordLearnPatch), nameof(LevelEventMaker));

        private static Settings ModSettings => modSettings ??= LoadedModManager.GetMod<ModHandler>().GetSettings<Settings>();

        private static FieldInfo SkillRecordLevelInt => skillRecordLevelInt ??=
            typeof(SkillRecord).GetField(nameof(SkillRecord.levelInt));

        private static FieldInfo SkillRecordPawn => skillRecordPawn ??=
            typeof(SkillRecord).GetField("pawn", BindingFlags.NonPublic | BindingFlags.Instance);

        public static void InitializePatch(Harmony harmony, LevelEventMaker levelEventMaker)
        {
            LevelEventMaker = levelEventMaker;
            var original = typeof(SkillRecord).GetMethod(nameof(SkillRecord.Learn));

            if (!Harmony.HasAnyPatches(harmony.Id))
            {
                var transpiler = SymbolExtensions.GetMethodInfo(() => LearnTranspilerPatch(default));
                harmony.Patch(original, transpiler: new HarmonyMethod(transpiler));
            }

            if (emptyPatchProcessor is null)
            {
                emptyPatchProcessor = new PatchProcessor(harmony, original);
            }
        }

        public static IEnumerable<CodeInstruction> LearnTranspilerPatch(IEnumerable<CodeInstruction> instructions)
        {
            CodeInstruction previousInstruction = default;

            foreach (var currentInstruction in instructions)
            {
                yield return currentInstruction;

                if (currentInstruction.opcode == OpCodes.Stfld && currentInstruction.operand as FieldInfo == SkillRecordLevelInt)
                {
                    MethodBase onLevelChangeMethod = null;
                    if (previousInstruction.opcode == OpCodes.Add && ModSettings.DoLevelUp)
                    {
                        onLevelChangeMethod = OnLevelUp;
                    }
                    else if (previousInstruction.opcode == OpCodes.Sub && ModSettings.DoLevelDown)
                    {
                        onLevelChangeMethod = OnLevelDown;
                    }

                    if (onLevelChangeMethod != null)
                    {
                        yield return new CodeInstruction(OpCodes.Call, LevelEventMakerGetter);
                        yield return new CodeInstruction(OpCodes.Ldarg_0);
                        yield return new CodeInstruction(OpCodes.Ldarg_0);
                        yield return new CodeInstruction(OpCodes.Ldfld, SkillRecordPawn);
                        yield return new CodeInstruction(OpCodes.Ldarg_0);
                        yield return new CodeInstruction(OpCodes.Ldfld, SkillRecordLevelInt);
                        yield return new CodeInstruction(OpCodes.Call, onLevelChangeMethod);
                    }
                }

                previousInstruction = currentInstruction;
            }
        }

        public static void UpdatePatch()
        {
            emptyPatchProcessor.Patch();
        }
    }
}