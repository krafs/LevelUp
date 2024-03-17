using UnityEngine;
using Verse;

namespace LevelUp;

public abstract class LevelingAction : IExposable
{
    internal ActionDef actionDef = null!;
    internal bool active;

    internal virtual void Prepare()
    { }

    internal abstract void Execute(LevelingInfo levelingInfo);

    internal abstract void Draw(Rect rect);

    public virtual void ExposeData()
    {
        Scribe_Defs.Look(ref actionDef, "def");
        Scribe_Values.Look(ref active, "active");
    }
}
