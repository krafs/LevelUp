using System;
using System.Collections.Generic;
using Verse;

namespace LevelUp;

public sealed class ActionDef : Def
{
    internal readonly Type actionClass = null!;

    public override IEnumerable<string> ConfigErrors()
    {
        foreach (string configError in base.ConfigErrors())
        {
            yield return configError;
        }

        if (actionClass is null)
        {
            yield return $"{nameof(actionClass)} cannot be null.";
        }

        if (!typeof(LevelingAction).IsAssignableFrom(actionClass))
        {
            yield return $"{nameof(actionClass)} must inherit from {nameof(LevelingAction)}.";
        }
    }
}
