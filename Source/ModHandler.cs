using UnityEngine;
using Verse;

namespace LevelUp
{
    public class ModHandler : Mod
    {
        private readonly Listing_Standard listing;
        private Settings settings;

        public ModHandler(ModContentPack content) : base(content)
        {
            this.listing = new Listing_Standard
            {
                ColumnWidth = 300f
            };
        }

        public Settings Settings
        {
            get
            {
                if (this.settings is null)
                {
                    this.settings = this.GetSettings<Settings>();
                }

                return this.settings;
            }
        }

        public override string SettingsCategory() => base.Content.Name;

        public override void DoSettingsWindowContents(Rect rect)
        {
            listing.Begin(rect);

            listing.CheckboxLabeled("Krafs.LevelUp.LevelUpLabel".Translate(), ref this.Settings.DoLevelUp);
            if (this.Settings.DoLevelUp)
            {
                listing.CheckboxLabeled("Krafs.LevelUp.DoMessage".Translate(), ref this.Settings.DoLevelUpMessage);
                listing.CheckboxLabeled("Krafs.LevelUp.DoSound".Translate(), ref this.Settings.DoLevelUpSound);
                listing.CheckboxLabeled("Krafs.LevelUp.DoAnimation".Translate(), ref this.Settings.DoLevelUpAnimation);
            }

            listing.NewColumn();

            listing.CheckboxLabeled("Krafs.LevelUp.LevelDownLabel".Translate(), ref this.Settings.DoLevelDown);
            if (this.Settings.DoLevelDown)
            {
                listing.CheckboxLabeled("Krafs.LevelUp.DoMessage".Translate(), ref this.Settings.DoLevelDownMessage);
                listing.CheckboxLabeled("Krafs.LevelUp.DoSound".Translate(), ref this.Settings.DoLevelDownSound);
                listing.CheckboxLabeled("Krafs.LevelUp.DoAnimation".Translate(), ref this.Settings.DoLevelDownAnimation);
            }

            listing.End();
        }
    }
}