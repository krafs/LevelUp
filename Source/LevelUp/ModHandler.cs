using UnityEngine;
using Verse;

namespace LevelUp
{
    public sealed class ModHandler : Mod
    {
        public ModHandler(ModContentPack content) : base(content)
        { }

        public override string SettingsCategory()
        {
            return Content.Name;
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
        }
    }
}