using System.Collections.Generic;
using Verse;

namespace LevelUp;

public sealed class Settings : ModSettings
{
    internal static Dictionary<int, int> timerCache = [];
    internal static Profile profile = new();

    public Settings()
    {
        if (Scribe.mode == LoadSaveMode.Inactive)
        {
            EnsureInitialized();
        }
    }

    public override void ExposeData()
    {
        Scribe_Deep.Look(ref profile, "profile");
        if (Scribe.mode == LoadSaveMode.LoadingVars)
        {
            EnsureInitialized();
        }
        else if (Scribe.mode == LoadSaveMode.Saving)
        {
            profile.Prepare();
        }
    }

    private static void EnsureInitialized()
    {
        ProfileInitializer.InitializeProfile(profile);
        profile.Prepare();
    }
}
