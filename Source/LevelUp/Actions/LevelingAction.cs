using System;
using UnityEngine;
using Verse;

namespace LevelUp
{
    [Serializable]
    public abstract class LevelingAction : IExposable, IDrawer
    {
        private ActionDef actionDef = null!;
        private bool active;
        private bool cooldown = true;

        public ActionDef ActionDef { get => actionDef; set => actionDef = value; }
        public bool Active { get => active; set => active = value; }
        public bool Cooldown { get => cooldown; set => cooldown = value; }

        public virtual void Prepare()
        {
            if (actionDef is null)
            {
                throw new InvalidOperationException("def is null.");
            }
        }

        public abstract void Execute(LevelingInfo levelingInfo);

        public virtual void Draw(Rect rect)
        { }

        public virtual void ExposeData()
        {
            Scribe_Defs.Look(ref actionDef, "def");
            Scribe_Values.Look(ref active, "active");
            Scribe_Values.Look(ref cooldown, "cooldown", true);
        }
    }
}