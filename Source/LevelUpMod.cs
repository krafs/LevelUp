using HarmonyLib;
using UnityEngine;
using Verse;

namespace LevelUp;

public sealed class LevelUpMod : Mod
{
    public LevelUpMod(ModContentPack content) : base(content)
    {
        Harmony harmony = new(content.PackageId);
        Patcher.ApplyPatches(harmony);
        LongEventHandler.ExecuteWhenFinished(static () =>
        {
            // Retrieve settings to initialize values, so they're in place.
            // We do this in LongEvent because defs aren't loaded in Mod constructor.
            LoadedModManager.GetMod<LevelUpMod>().GetSettings<Settings>();
        });
    }

    public override string SettingsCategory()
    {
        return Content.Name;
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        Settings.profile.Draw(inRect);
    }
}
