using System;
using Verse;

namespace LevelUp;

[Serializable]
public class Settings : ModSettings
{
    private Profile profile = null!;
    public Profile Profile => profile;

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
        profile ??= new Profile();
        ProfileInitializer.InitializeProfile(profile);
        profile.Prepare();
    }
}
