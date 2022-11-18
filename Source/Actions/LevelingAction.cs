using System;
using UnityEngine;
using Verse;

namespace LevelUp;

[Serializable]
public abstract class LevelingAction : IExposable, IDrawer
{
    private ActionDef actionDef = null!;
    private bool active;
    private bool cooldown = true;

    internal ActionDef ActionDef { get => actionDef; set => actionDef = value; }
    internal bool Active { get => active; set => active = value; }
    internal bool Cooldown { get => cooldown; set => cooldown = value; }

    internal virtual void Prepare()
    {
        if (actionDef is null)
        {
            throw new InvalidOperationException("def is null.");
        }
    }

    internal abstract void Execute(LevelingInfo levelingInfo);

    public virtual void Draw(Rect rect)
    { }

    public virtual void ExposeData()
    {
        Scribe_Defs.Look(ref actionDef, "def");
        Scribe_Values.Look(ref active, "active");
        Scribe_Values.Look(ref cooldown, "cooldown", true);
    }
}
