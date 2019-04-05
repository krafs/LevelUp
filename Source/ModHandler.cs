using UnityEngine;
using Verse;

namespace LevelUp
{
    class ModHandler : Mod
    {
        public ModHandler(ModContentPack content) : base(content)
        {
            GetSettings<Settings>();
        }

        public override string SettingsCategory() => "LevelUp";

        public override void DoSettingsWindowContents(Rect inRect)
        {
            base.DoSettingsWindowContents(inRect);
            Listing_Standard view = new Listing_Standard();
            view.Begin(inRect);
            view.ColumnWidth = 220f;
            view.CheckboxLabeled("LevelUpTextMessageLabel".Translate(), ref Settings.allowLevelUpTextMessage, "LevelUpTextMessageLabelTooltip".Translate());
            view.CheckboxLabeled("LevelUpSoundEffectLabel".Translate(), ref Settings.allowLevelUpSoundEffect, "LevelUpSoundEffectLabelTooltip".Translate());
            view.CheckboxLabeled("LevelUpAnimationLabel".Translate(), ref Settings.allowLevelUpAnimation, "LevelUpAnimationLabelTooltip".Translate());
            view.End();
        }
    }
}