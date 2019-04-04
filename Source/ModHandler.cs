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
            view.Label("SettingsExplanationLabel".Translate().Bold());
            view.Gap();
            view.CheckboxLabeled("TextMessageLabel".Translate(), ref Settings.allowTextMessage, "TextMessageLabelTooltip".Translate());
            view.CheckboxLabeled("SoundEffectLabel".Translate(), ref Settings.allowSoundEffect, "SoundEffectLabelTooltip".Translate());
            view.CheckboxLabeled("AnimationLabel".Translate(), ref Settings.allowAnimation, "AnimationLabelTooltip".Translate());
            view.End();
        }
    }
}