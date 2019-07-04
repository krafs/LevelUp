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

        static string editBuffer;

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
            view.CheckboxLabeled("LevelUpLevelDesc".Translate(), ref Settings.levelDescLevelUpLabel, "LevelUpLevelDescTooltip".Translate());

            view.Gap();

            List<FloatMenuOption> soundOptions = new List<FloatMenuOption>()
            {
                new FloatMenuOption(DefHandler.LevelUp.label, delegate () { Settings.LvlUpSound = DefHandler.Sound.LevelUp; }),
                new FloatMenuOption(DefHandler.LevelUp2.label, delegate () { Settings.LvlUpSound = DefHandler.Sound.LevelUp2; })
            };

            if (view.ButtonTextLabeled("SoundChooseLabel".Translate(), DefHandler.GetSound(Settings.LvlUpSound).label))
                Find.WindowStack.Add(new FloatMenu(soundOptions));

            if (view.ButtonText("TestPlaySound".Translate()))
                DefHandler.GetSound(Settings.LvlUpSound).PlayOneShotOnCamera(null);

            view.NewColumn();

            view.Label("LevelDownSettingsLabel".Translate());
            view.CheckboxLabeled("LevelDownTextMessageLabel".Translate(), ref Settings.allowLevelDownTextMessage, "LevelDownTextMessageLabelTooltip".Translate());
            view.CheckboxLabeled("LevelDownLetterLabel".Translate(), ref Settings.allowLevelDownLetter, "LevelDownLetterLabelTooltip".Translate());
            view.CheckboxLabeled("LevelDownSoundEffectLabel".Translate(), ref Settings.allowLevelDownSoundEffect, "LevelDownSoundEffectLabelTooltip".Translate());
            view.CheckboxLabeled("LevelDownAnimationLabel".Translate(), ref Settings.allowLevelDownAnimation, "LevelDownAnimationLabelTooltip".Translate());
            view.CheckboxLabeled("LevelDownLevelDesc".Translate(), ref Settings.levelDescLevelDownLabel, "LevelDownLevelDescTooltip".Translate());

            view.NewColumn();

            view.CheckboxLabeled("Ignore10To9Label".Translate(), ref Settings.ignoreLvl10To9, "Ignore10To9LabelTooltip".Translate());

            view.Gap();

            view.Label("NotificationTimerLabel".Translate(), -1, "NotificationTimerLabelTooltip".Translate());
            view.IntEntry(ref Settings.notificationTimer, ref editBuffer);

            view.End();
        }
    }
}