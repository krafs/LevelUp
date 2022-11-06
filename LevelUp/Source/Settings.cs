using System;
using Verse;

namespace LevelUp;

[Serializable]
public class Settings : ModSettings
{
    private Profile profile = null!;
    public Profile Profile => profile;
    public static Profile CurrentProfile { get; private set; } = null!;

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

    private void EnsureInitialized()
    {
        CurrentProfile = profile ??= new Profile();
        ProfileInitializer.InitializeProfile(profile);
        profile.Prepare();
    }
}
