using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace LevelUp
{
    class ModHandler : Mod
    {
        public ModHandler(ModContentPack content) : base(content)
        {
            settings = GetSettings<Settings>();
        }

        public static Settings settings;

        public override string SettingsCategory() => "LevelUp";

        //static List<FloatMenuOption> soundOptions = new List<FloatMenuOption>()
        //    {
        //        new FloatMenuOption("Classic", delegate () { Settings.LvlUpSoundDef = DefHandler.LevelUp; }, MenuOptionPriority.Default, delegate () { DefHandler.LevelUp.PlayOneShotOnCamera(null); }),
        //        new FloatMenuOption("New", delegate () { Settings.LvlUpSoundDef = DefHandler.LevelUp2; }, MenuOptionPriority.Default, delegate () { DefHandler.LevelUp2.PlayOneShotOnCamera(null); })
        //    };

        //static FloatMenu soundMenu = new FloatMenu(soundOptions);

        public override void DoSettingsWindowContents(Rect inRect)
        {
            base.DoSettingsWindowContents(inRect);
            Listing_Standard view = new Listing_Standard();
            view.Begin(inRect);
            view.ColumnWidth = 220f;

            view.Label("LevelUpSettingsLabel".Translate());
            view.CheckboxLabeled("LevelUpTextMessageLabel".Translate(), ref Settings.allowLevelUpTextMessage, "LevelUpTextMessageLabelTooltip".Translate());
            view.CheckboxLabeled("LevelUpLetterLabel".Translate(), ref Settings.allowLevelUpLetter, "LevelUpLetterLabelTooltip".Translate());
            view.CheckboxLabeled("LevelUpSoundEffectLabel".Translate(), ref Settings.allowLevelUpSoundEffect, "LevelUpSoundEffectLabelTooltip".Translate());
            view.CheckboxLabeled("LevelUpAnimationLabel".Translate(), ref Settings.allowLevelUpAnimation, "LevelUpAnimationLabelTooltip".Translate());

            view.Gap();

            bool makeSound = false;

            List<FloatMenuOption> soundOptions = new List<FloatMenuOption>()
            {
                new FloatMenuOption("Classic", delegate () { Settings.LvlUpSound = DefHandler.Sound.LevelUp; }, MenuOptionPriority.Default, delegate () { makeSound = true; }),
                new FloatMenuOption("New", delegate () { Settings.LvlUpSound = DefHandler.Sound.LevelUp2; }, MenuOptionPriority.Default, delegate () { makeSound = true; })
            };

            if (view.ButtonTextLabeled("Sound", DefHandler.GetSound(Settings.LvlUpSound).label))
                Find.WindowStack.Add(new FloatMenu(soundOptions));

            if (makeSound)
            {
                //DefHandler.LevelUp.PlayOneShot(null);
                SoundDefOf.Building_Complete.PlayOneShot(null);
                makeSound = false;
            }

            view.NewColumn();

            view.Label("LevelDownSettingsLabel".Translate());
            view.CheckboxLabeled("LevelDownTextMessageLabel".Translate(), ref Settings.allowLevelDownTextMessage, "LevelDownTextMessageLabelTooltip".Translate());
            view.CheckboxLabeled("LevelDownLetterLabel".Translate(), ref Settings.allowLevelDownLetter, "LevelDownLetterLabelTooltip".Translate());
            view.CheckboxLabeled("LevelDownSoundEffectLabel".Translate(), ref Settings.allowLevelDownSoundEffect, "LevelDownSoundEffectLabelTooltip".Translate());
            view.CheckboxLabeled("LevelDownAnimationLabel".Translate(), ref Settings.allowLevelDownAnimation, "LevelDownAnimationLabelTooltip".Translate());

            view.NewColumn();

            view.CheckboxLabeled("Ignore10To9Label".Translate(), ref Settings.ignoreLvl10To9, "Ignore10To9LabelTooltip".Translate());

            view.End();
        }
    }
}