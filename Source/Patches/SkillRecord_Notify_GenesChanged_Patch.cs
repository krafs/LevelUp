using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;

namespace LevelUp;

[StaticConstructorOnStartup]
public static class SkillRecord_Notify_GenesChanged_Patch
{
    static SkillRecord_Notify_GenesChanged_Patch()
    {
        if (!ModLister.BiotechInstalled)
        {
            return;
        }
        Logger.Debug("SkillRecord_Notify_GenesChanged_Patch");
        Harmony harmony = new("Krafs.LevelUp.Biotech");

        MethodInfo original = SymbolExtensions.GetMethodInfo<SkillRecord>(x => x.Notify_GenesChanged());
        HarmonyMethod prefix = new(typeof(SkillRecord_Notify_GenesChanged_Patch), nameof(SkillRecord_Notify_GenesChanged_Patch.Prefix));
        HarmonyMethod postfix = new(typeof(SkillRecord_Notify_GenesChanged_Patch), nameof(SkillRecord_Notify_GenesChanged_Patch.Postfix));
        harmony.Patch(original, prefix, postfix);
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
}
