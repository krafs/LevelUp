using UnityEngine;
using Verse;

namespace LevelUp;

public class LevelUpMod : Mod
{
    public LevelUpMod(ModContentPack content) : base(content)
    { }

    public override string SettingsCategory()
    {
        return Content.Name;
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        GetSettings<Settings>().Profile.Draw(inRect);
    }
}
